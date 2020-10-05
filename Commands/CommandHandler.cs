using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_1.Interfaces;
using Sprint_1.MovementStates;
using Sprint_1.Sprites;

namespace Sprint_1.Commands
{
    //Holds many overloaded Execute methods to act as an in between for ICommands and
    //the actual code for what command does
    public static class CommandHandler
    {
        public static void Execute(MarioMoveState.Command command, PlayerSprite mario, GraphicsDeviceManager graphics)
        {
            if (mario != null)
            {
                mario.MoveState.Current.Execute(command, mario, graphics); //Change state if needed
                mario.MoveState.Current.Action(mario); //Change sprite to new state
            }
        }

        

        public static void Execute(Texture2D pixel)
        {            
                CollisionObject.ToggleVisibility(pixel);
        }

        public static void Execute(ISprite sprite)
        {
            sprite?.ToggleVisibility();
        }

        public static void Execute(Game game, String type)
        {
            if (game != null && type != null)
            {
                if (type.Equals("Quit"))
                {
                    game.Exit();
                }
                else if (type.Equals("Pause"))
                {
                    ((Game1) game).PauseToggle();
                }
                else if (type.Equals("Reset"))
                {
                    ((Game1)game).Reset();
                }
                else if (type.Equals("Mute"))
                {
                    ((Game1)game).Mute();
                }
            }
        }

        public static void Execute(String message, String key)
        {
            Console.WriteLine(message + key);
        }

        public static void Execute(PlayerSprite mario, GraphicsDeviceManager graphics, PlayerSprite.MovementDirection command)
        {
            mario?.Move(graphics, command);
        }
    }
}