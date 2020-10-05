using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint_1.Interfaces
{
     public interface ISprite
    {
       void Update(GameTime gameTime);

       CollisionObject Collision { get; set; }

       void CollisionUpdate();

       bool Visible { get; set; }

       Vector2 Velocity { get; }

       Vector2 MovingPosition { get; set; }

       Texture2D MyTexture { get; }

       double[] GridPosition();

       double[] FutureGridPosition();

        void Draw(SpriteBatch spriteBatch);

       void ToggleVisibility();

       Rectangle BoxSize();

       Color BoxColor { get; }

       Rectangle OldPosition { get; set; }

       CollisionObject.CollisionDirection CollisionDirection { get; set; }

       void SetVelocity(Vector2 givenVelocity);
       
    }
}
