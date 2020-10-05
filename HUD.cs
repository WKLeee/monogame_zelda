using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint_1.HUDEventArgs;
using Sprint_1.Interfaces;
using Sprint_1.Sprites;

namespace Sprint_1
{
    // ReSharper disable once InconsistentNaming
    public class HUD
    {
        private const int InitialTime = 400;
        private const int InitialHealth = 6;
        private const int ScoreMultiplier = 2;

        private int keys;
        private int coins;
        private int health;
        private int score;
        private int time;
        private bool flag;
        private double timeSinceUpdate;
        private double gameOverTimer;
        private Game1 game;
        private ContentManager contents;
        private SpriteFont fontTexture;
        private Texture2D symbolsSprite;
        private Texture2D lifeLetterSprite;
        private Texture2D fullHeartSprite;
        private Texture2D halfHeartSprite;
        private Texture2D emptyHeartSprite;

        public event EventHandler OutOfTime;

        private static HUD _instance;

        public static HUD GetInstance()
        {
            if (_instance == null)
            {
                _instance = new HUD();
            }
            return _instance;
        }

        private HUD()
        {

        }

        // ReSharper disable once InconsistentNaming
        public void SetUpHUD(Game1 gameParameter, ContentManager c)
        {
            contents = c;
            game = gameParameter;
            timeSinceUpdate = 0;
            coins = 0;
            health = InitialHealth;
            score = 0;
            time = InitialTime;
            flag = false;
            keys = 0;
        }


        public void LoadContent()
        {
            if (game != null)
            {
                fontTexture = contents.Load<SpriteFont>("Font");
                symbolsSprite = contents.Load<Texture2D>("HUD/HUDSymbols");
                lifeLetterSprite = contents.Load<Texture2D>("HUD/Life");
                fullHeartSprite = contents.Load<Texture2D>("HUD/FullHeart");
                halfHeartSprite = contents.Load<Texture2D>("HUD/HalfHeart");
                emptyHeartSprite = contents.Load<Texture2D>("HUD/EmptyHeart");
            }
        }

        public void SubscribeToSprites(ICollection<ISprite> sprites)
        {
            foreach (ISprite item in sprites) {
                if (item is ItemSprite)
                {
                    ((ItemSprite)item).ItemCollect += ItemCollectHandler;
                } else if (item is EnemySprite)
                {
                    ((EnemySprite)item).EnemyDeath += EnemyDeathHandler;
                }
            }
        }

        public void LinkToPlayer(ISprite player)
        {
            ((PlayerSprite)player).DamageEvent += PlayerDamageHandler;
            ((PlayerSprite)player).HealEvent += PlayerHealHandler;
            //((PlayerSprite) player).LevelComplete += new EventHandler<LevelCompleteArgs>(LevelCompleteHandler);

        }

        public void ItemCollectHandler(object sender, ItemCollectEventArgs e)
        {
            int type = e.ItemType;
            switch (type)
            {
                case 1:
                    coins++;
                    score += 200;
                    break;
                case 4:
                    // lives++;
                    break;
                //Case 2,3, 5 all have same effect, so here we take advantage of fallthrough
                case 2:
                    keys++;
                    break;
                case 3:
                case 5:
                    score += 1000;
                    break;

            }

        }

        public void EnemyDeathHandler(object sender, EnemyDeathEventArgs e)
        {
            EnemySprite.EnemyTyping type = e.EnemyType;
            switch (type)
            {
                case EnemySprite.EnemyTyping.Bat:
                    score += 1000;
                    break;
                //Case 2, 3 and 4 have same effect, so here we take advantage of fallthrough
                case EnemySprite.EnemyTyping.Skeleton:
                case EnemySprite.EnemyTyping.Saw:
                case EnemySprite.EnemyTyping.BladeTrap:
                    score += 2000;
                    break;

            }
        }

        public void PlayerDamageHandler(object sender, EventArgs e)
        {
            health--;
        }
        public void PlayerHealHandler(object sender, EventArgs e)
        {
            health++;
        }

        public void LevelCompleteHandler(object sender, LevelCompleteArgs e)
        {
            int height = e.Height;

            if (height < 18)
            {
                score += 100;
            } else if (height < 57)
            {
                score += 400;
            }
            else if (height < 81)
            {
                score += 800;
            }
            else if (height < 127)
            {
                score += 2000;
            }
            else if (height < 153)
            {
                score += 4000;
            }
            else
            {
               // lives++;
            }

            score += ScoreMultiplier * time;
            time = 0;
            flag = true;
        }

        public void ResetTime()
        {
            time = InitialTime;
        }
        public void ResetHealth()
        {
            health = InitialHealth;
        }
        public bool TimeRunningOut()
        {
            return (time == 30);
        }
        public void ResetItem()
        {
            keys = 0;   
        }
        public bool OutOfHealth()
        {
            return health == 0;
        }

        public void GameOver(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(fontTexture, "GAMEOVER", new Vector2(144, 50), Color.White);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            if (!game.IsPaused())
            {
                if (coins == 100)
                {
                    coins = 0;
                    //lives += 1;
                }

                timeSinceUpdate += gameTime.ElapsedGameTime.Milliseconds;

                if (timeSinceUpdate >= 1000f && time > 0 && !OutOfHealth())
                {
                    time--;
                    timeSinceUpdate -= 1000f;
                    if (time == 0)
                    {
                        OutOfTime?.Invoke(this, null);
                    }
                }
            }
        }

        public bool GameOver(GameTime gameTime)
        {
            bool over = false;
            gameOverTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (gameOverTimer >= 2750f)
            {
                over = true;
            }
            return over;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int positionX = 16;
            int positionY = 10;
            float scale = 1.5f;
            //spriteBatch.Draw(symbolsSprite, new Rectangle(positionX, positionY, symbolsSprite.Width*2, symbolsSprite.Height*2), Color.White);
            spriteBatch.Draw(symbolsSprite, new Vector2(positionX, positionY), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            spriteBatch.Draw(lifeLetterSprite, new Vector2(positionX, positionY), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            spriteBatch.DrawString(fontTexture, "" + keys, new Vector2(positionX + 42, positionY + 15), Color.White);
            spriteBatch.DrawString(fontTexture, "" + coins, new Vector2(positionX, positionY + 15), Color.White);

            if (health > 0)
            {
                if (health == 1) spriteBatch.Draw(halfHeartSprite, new Vector2(positionX + 170, positionY + 15), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                if (health >= 2) spriteBatch.Draw(fullHeartSprite, new Vector2(positionX + 170, positionY + 15), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                if (health == 3) spriteBatch.Draw(halfHeartSprite, new Vector2(positionX + 182, positionY + 15), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                if (health >= 4) spriteBatch.Draw(fullHeartSprite, new Vector2(positionX + 182, positionY + 15), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                if (health == 5) spriteBatch.Draw(halfHeartSprite, new Vector2(positionX + 194, positionY + 15), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                if (health >= 6) spriteBatch.Draw(fullHeartSprite, new Vector2(positionX + 194, positionY + 15), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                if (health < 5) spriteBatch.Draw(emptyHeartSprite, new Vector2(positionX + 194, positionY + 15), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                if (health < 3) spriteBatch.Draw(emptyHeartSprite, new Vector2(positionX + 182, positionY + 15), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                if (health < 1) spriteBatch.Draw(emptyHeartSprite, new Vector2(positionX + 170, positionY + 15), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
             
            }

            if (OutOfHealth())
            {
                if (GameOver(gameTime)){
                    spriteBatch.DrawString(fontTexture, "GAME OVER", new Vector2(positionX + 296, positionY + 200), Color.White);
                    spriteBatch.DrawString(fontTexture, "PRESS R TO RESTART OR Q TO EXIT", new Vector2(positionX + 216, positionY + 218), Color.White);
                }
            }
            if (flag)
            {
                spriteBatch.DrawString(fontTexture, "YOU   WIN", new Vector2(positionX + 350, positionY + 200), Color.Yellow);
                spriteBatch.DrawString(fontTexture, "Your Score: " + score, new Vector2(positionX + 325, positionY + 220), Color.Yellow);
                spriteBatch.DrawString(fontTexture, "PRESS R TO RESTART OR Q TO EXIT", new Vector2(positionX + 240, positionY + 240), Color.Yellow);
            }
            if (time != 0) flag = false;

        }
    }
}
