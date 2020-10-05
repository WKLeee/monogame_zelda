using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework.Graphics;
using Sprint_1.Controllers;
using Sprint_1.Sprites;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Sprint_1.Interfaces;
using Sprint_1.Sounds;

[assembly:CLSCompliant(true)]
namespace Sprint_1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly GraphicsDeviceManager graphics;
        private static ContentManager _contents;
        SpriteBatch spriteBatch;
        private List<ISprite> backgroundSprites;
        private List<ISprite> midgroundSprites;
        private List<ISprite> sprites;
        private Transition transition;
        ISprite link;
        private double deadTime;
        private ControllerHandler controller;
        bool paused;
        bool muted;
        private Texture2D pixel; //Generic pixel texture for collision box rectangles
        private Grid grid;
        private Collider collider;
        private const int HeightMult = 3;
        private Camera camera;
        private int levelWidth;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private enum GameState
        {
            Gameplay, GameOver
        }
        GameState myState;
        private const float BackgroundSpeed = 0.3f;
        private const float MidgroundSpeed = 0.6f;

        private Song overworld;
       // private  SoundEffect gameOver;
        private Collection<SoundEffectInstance> gameSounds;
        private SoundHandler soundEffects;
        private HUD scoreBoard;
        private PausedScreen pausedScreen;
        private GameOverScreen gameoverScreen;
        bool timeLow;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _contents = Content;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        //Since we need to initialize all of our commands, there is no real way around coupling here.
        protected override void Initialize()
        {
            camera = new Camera(GraphicsDevice.Viewport)
            {
                Limits = new Rectangle(0, 0, levelWidth, GraphicsDevice.Viewport.Height)
            };

            transition = new Transition(camera);
            if (scoreBoard == null || scoreBoard.OutOfHealth())
            {
                scoreBoard = HUD.GetInstance();
                scoreBoard.SetUpHUD(this, _contents);
            }
            else
            {
                scoreBoard.ResetTime();
                scoreBoard.ResetHealth();
                scoreBoard.ResetItem();
            }

            pausedScreen = new PausedScreen(this, _contents);
            gameoverScreen = new GameOverScreen(this, _contents);

            base.Initialize();            
            deadTime = 0;
            //For Collision rectangle texture
            pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });

            controller = new ControllerHandler();
           
            controller.GameplaySetUp(this, link, pixel, graphics);
            controller.PauseSetUp(this);
            MediaPlayer.Play(overworld);
            MediaPlayer.IsRepeating = true;
            // MediaPlayer.Volume = 1;
            if (!muted)
            {
                MediaPlayer.IsMuted = false;
                SoundEffect.MasterVolume = 1;
            }
            else
            {
                MediaPlayer.IsMuted = true;
                SoundEffect.MasterVolume = 0;
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            const int levelWidthMult = 4;
            levelWidth = levelWidthMult * GraphicsDevice.Viewport.Width;
            int height = graphics.GraphicsDevice.Viewport.Height * HeightMult;
            grid = new Grid(levelWidth, height, levelWidthMult, HeightMult, levelWidthMult);
            collider = new Collider(grid);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteFactory factory = SpriteFactory.GetInstance();
            factory.AddContentManager(Content);
            factory.AddCollider(collider);

            //LevelDefinition will handle sprite creation
            XmlReaderSettings settings = new XmlReaderSettings
            {
                IgnoreWhitespace = true
            };
            XmlReader reader = XmlReader.Create("Content/TileMapXml.xml", settings);
            
            LevelDefinition level = IntermediateSerializer.Deserialize<LevelDefinition>(reader, null);
            level.MakeLink(factory, grid, transition);
            link = level.GetLink();
            level.MakeLevel(factory, grid, transition, levelWidthMult);                    
            sprites = level.GetForeground();
            backgroundSprites = level.GetBackground();
            midgroundSprites = level.GetMidground();
            foreach (ISprite sprite in sprites)
            {
                if (sprite is EnemySprite && ((EnemySprite) sprite).enemyType == EnemySprite.EnemyTyping.Bat)
                {
                    ((EnemySprite)sprite).GiveKey();
                    break;
                    
                }
            }


            CollisionObject.UpdateGraphics(graphics);
            overworld = Content.Load<Song>("Sounds/04 Main Theme"); //Background music
            //gameOver = Content.Load<SoundEffect>("Sounds/LOZ_Recorder");  Game over Sound
            soundEffects = SoundHandler.GetInstance();
            soundEffects.CreateGameSoundsList(Content);
            gameSounds = soundEffects.GameSounds;
            
            scoreBoard.LoadContent();
            scoreBoard.SubscribeToSprites(sprites);
            scoreBoard.LinkToPlayer(link);

            pausedScreen.LoadContent();
            gameoverScreen.LoadContent();
            ((PlayerSprite)link).SubscribeToHUD(scoreBoard);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        protected override void Update(GameTime gameTime)
        {
            bool dead = (link as PlayerSprite)?.PowerUpState.Current == (link as PlayerSprite)?.PowerUpState.Dead;
            if (gameTime != null)
            {
                //Camera update

                   camera.Limits = new Rectangle(0, 0, levelWidth, GraphicsDevice.Viewport.Height * 3);

                   if (transition.Check())
                    {
                        if (transition.Count > 0)
                        {
                            
                            camera.LookAt(link.MovingPosition+transition.Adjustment);
                            transition.Receive(false);
                           // transition.DoorSwitch();
                        }
                    }

                //Update controllers
                controller.Update(paused, gameTime);
                if (!paused)
                {
                    collider.Update(gameTime);
                    base.Update(gameTime);
                }

                //scoreBoard.Update(gameTime);

                if (dead)
                {
                    if (deadTime == 0)
                    {
                        MediaPlayer.Stop();                        
                    }
                    deadTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (deadTime > 2750 && !scoreBoard.OutOfHealth())
                    {
                        myState = GameState.GameOver;
                    }
                    if (deadTime == 2800 && scoreBoard.OutOfHealth())
                    {
                        gameSounds[2].Play(); //Gameover sound
                    }
                }
                else
                {
                    scoreBoard.Update(gameTime);
                }
                if (scoreBoard.TimeRunningOut() )
                {
                    MediaPlayer.Pause();
                    gameSounds[0].Play(); //Time running out sound
                    timeLow = true;
                }
                if(gameSounds[0].State != SoundState.Playing && timeLow)
                {
                    MediaPlayer.Resume();
                }
                pausedScreen.GetCurrentPosition(link.MovingPosition);           
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw the sprites


            //Background
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(new Vector2(BackgroundSpeed, 1)));
            
            foreach (ISprite sprite in backgroundSprites)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.End();

            //Mid-ground
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(new Vector2(MidgroundSpeed, 1)));

            foreach (ISprite sprite in midgroundSprites)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.End();

            //Foreground
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(Vector2.One));

            link.Draw(spriteBatch);

            foreach (ISprite sprite in sprites)
            {
                sprite.Draw(spriteBatch);
            }

            base.Draw(gameTime);

            spriteBatch.End();

            // HUD
            spriteBatch.Begin();

            scoreBoard.Draw(spriteBatch, gameTime);

            if (myState == GameState.GameOver) gameoverScreen.Draw(spriteBatch);
            //if (_state != GameState.GameOver) scoreBoard.Draw(spriteBatch, gameTime);
            if (paused) pausedScreen.Draw(spriteBatch);
            spriteBatch.End();
        }

        //Will be used later to pause the game at a specific moment
        public void PauseToggle()
        {
            if ((link as PlayerSprite)?.PowerUpState.Current != (link as PlayerSprite)?.PowerUpState.Dead)
            {
                paused = !paused;
                if (paused)
                {
                    SoundEffect.MasterVolume = 0;
                }
                else
                {
                    SoundEffect.MasterVolume = 1;
                }
            }
        }

        public void Mute()
        {
            muted = !muted;
            if (muted)
            {
                MediaPlayer.IsMuted = true;
                SoundEffect.MasterVolume = 0;               
            }
            else
            {
                MediaPlayer.IsMuted = false;
                SoundEffect.MasterVolume = 1;
            }
        }
        public void Reset()
        {
                //LoadContent();
                Initialize();
                myState = GameState.Gameplay;

        }
        public bool IsPaused()
        {
            return paused;
        }
    }
}
