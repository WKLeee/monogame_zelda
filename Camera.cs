using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Sprint_1
{
    public class Camera
    {
        public Camera(Viewport viewport)
        {
            myViewport = viewport;
            Origin = new Vector2(0,0);
            Zoom = 2.0f;
        }

        public Vector2 Position
        {
            get
            {
                return myPosition;
            }
            set
            {
                myPosition = value;

                // If there's a limit set and there's no zoom or rotation clamp the position
                if (Limits != null && Zoom == 2.0f && Rotation == 0.0f)
                {
                    myPosition.X = MathHelper.Clamp(myPosition.X, Limits.Value.X, Limits.Value.X + Limits.Value.Width - myViewport.Width);
                    myPosition.Y = MathHelper.Clamp(myPosition.Y, Limits.Value.Y, Limits.Value.Y + Limits.Value.Height - myViewport.Height);
                }
            }
        }

        public Vector2 Origin { get; set; }

        public float Zoom { get; set; }

        public float Rotation { get; set; }


        public Rectangle? Limits
        {
            get
            {
                return myLimits;
            }
            set
            {
                if (value != null)
                {
                    // Assign limit but make sure it's always bigger than the viewport
                    myLimits = new Rectangle
                    {
                        X = value.Value.X,
                        Y = value.Value.Y,
                        Width = Math.Max(myViewport.Width, value.Value.Width),
                        Height = Math.Max(myViewport.Height, value.Value.Height)
                    };

                    // Validate camera position with new limit
                    Position = Position;
                }
                else
                {
                    myLimits = null;
                }
            }
        }

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            return Matrix.CreateTranslation(new Vector3(-Position * parallax, 0.0f)) *
                   Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom, Zoom, 1.0f) *
                   Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        public void LookAt(Vector2 position)
        {
            Position = position - new Vector2(myViewport.Width / 2.0f, myViewport.Height / 2.0f);
        }

        /*public void Move(Vector2 displacement, bool respectRotation = false)
        {
            if (respectRotation)
            {
                displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(-Rotation));
            }

            Position += displacement;
        }*/

        private readonly Viewport myViewport;
        private Vector2 myPosition;
        private Rectangle? myLimits;
    }
}
