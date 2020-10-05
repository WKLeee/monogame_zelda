using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint_1
{
    public class PausedScreen
    {
        private Texture2D mapTexture;
        private Texture2D pointTexture;
        private Vector2 currentPosition = Vector2.Zero; 
        private readonly int positionX = GraphicsDeviceManager.DefaultBackBufferWidth / 2;
        private readonly int positionY = GraphicsDeviceManager.DefaultBackBufferHeight / 4;
        private readonly int pointX = GraphicsDeviceManager.DefaultBackBufferWidth / 2 - 140;
        private readonly int pointY = GraphicsDeviceManager.DefaultBackBufferHeight / 4 + 80;

        private SpriteFont fontTexture;
        private readonly Game1 game;
        private readonly ContentManager contents;
        private readonly float scale = 0.5f;
        public PausedScreen(Game1 game, ContentManager c)
        {
            contents = c;
            this.game = game;
        }

        public void GetCurrentPosition(Vector2 currentPos)
        {
            currentPosition = currentPos;
        }

        public void LoadContent()
        {
            if (game != null)
            {
                fontTexture = contents.Load<SpriteFont>("Font");
                mapTexture = contents.Load<Texture2D>("Map");
                pointTexture = contents.Load<Texture2D>("Point");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            spriteBatch.DrawString(fontTexture, "PAUSED", new Vector2(positionX-36, positionY), Color.Red);
            spriteBatch.Draw(mapTexture, new Vector2(positionX - 140, positionY + 50), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            spriteBatch.Draw(pointTexture, new Vector2(pointX + currentPosition.X/4, pointY+currentPosition.Y/4-30), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
    }
}