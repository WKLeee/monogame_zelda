using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint_1
{
    public class GameOverScreen
    {
        private readonly int positionX = GraphicsDeviceManager.DefaultBackBufferWidth / 2;
        private readonly int positionY = GraphicsDeviceManager.DefaultBackBufferHeight / 4;
        private SpriteFont fontTexture;
        private readonly Game1 game;
        private readonly ContentManager contents;

        public GameOverScreen(Game1 game, ContentManager c)
        {
            contents = c;
            this.game = game;
        }

        public void LoadContent()
        {
            if (game != null)
            {
                fontTexture = contents.Load<SpriteFont>("Font");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            spriteBatch.DrawString(fontTexture, "Game Over", new Vector2(positionX-36, positionY), Color.Red);
            spriteBatch.DrawString(fontTexture, "You Failed This Class", new Vector2(positionX - 70, positionY+50), Color.Red);
            spriteBatch.DrawString(fontTexture, "Press R to Retake", new Vector2(positionX - 60, positionY + 100), Color.Red);
        }
    }
}
