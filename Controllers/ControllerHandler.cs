using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint_1.Commands;
using Sprint_1.Interfaces;
using Sprint_1.Sprites;

namespace Sprint_1.Controllers
{
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    class ControllerHandler
    {
        private GamePadController gamepadGameplay;
        private GamePadController gamepadPause;
        private KeyboardController keyboardGameplay;
        private KeyboardController keyboardPause;
        private const int Delay = 39;
        private int count;

        public void GameplaySetUp(Game1 game, ISprite mario, Texture2D pixel, GraphicsDeviceManager graphics)
        {
            /*
             * Press executes on the initial press only
             * Hold executes once after a brief delay
             * Release executes once the button is released, regardless of how long button was held
             *
             * Get the Keys/Buttons with the toString, and use the Action enum in each controller
             */

            //Keyboard input controller
            keyboardGameplay = new KeyboardController();
            keyboardGameplay.AddKeyMapping(Keys.Q.ToString(), (int)KeyboardController.Action.Press, new QuitCommand(game));
            keyboardGameplay.AddKeyMapping(Keys.T.ToString(), (int)KeyboardController.Action.Press, new SpriteVisibilityToggleCommand(mario));
            keyboardGameplay.AddKeyMapping(Keys.C.ToString(), (int)KeyboardController.Action.Press, new BoundaryToggleCommand(pixel));

            keyboardGameplay.AddKeyMapping(Keys.P.ToString(), (int)KeyboardController.Action.Press, new PauseCommand(game));

            keyboardGameplay.AddKeyMapping(Keys.R.ToString(), (int)KeyboardController.Action.Press, new ResetCommand(game));
            keyboardGameplay.AddKeyMapping(Keys.M.ToString(), (int)KeyboardController.Action.Press, new MuteCommand(game));
            
            //Movement
            keyboardGameplay.AddKeyMapping(Keys.W.ToString(), (int)KeyboardController.Action.Press, new UpCommand((PlayerSprite)mario, graphics));
            keyboardGameplay.AddKeyMapping(Keys.S.ToString(), (int)KeyboardController.Action.Press, new DownCommand((PlayerSprite)mario, graphics));
            keyboardGameplay.AddKeyMapping(Keys.A.ToString(), (int)KeyboardController.Action.Press, new LeftCommand((PlayerSprite)mario, graphics));
            keyboardGameplay.AddKeyMapping(Keys.D.ToString(), (int)KeyboardController.Action.Press, new RightCommand((PlayerSprite)mario, graphics));

            keyboardGameplay.AddKeyMapping(Keys.Up.ToString(), (int)KeyboardController.Action.Press, new UpCommand((PlayerSprite)mario, graphics));
            keyboardGameplay.AddKeyMapping(Keys.Down.ToString(), (int)KeyboardController.Action.Press, new DownCommand((PlayerSprite)mario, graphics));
            keyboardGameplay.AddKeyMapping(Keys.Left.ToString(), (int)KeyboardController.Action.Press, new LeftCommand((PlayerSprite)mario, graphics));
            keyboardGameplay.AddKeyMapping(Keys.Right.ToString(), (int)KeyboardController.Action.Press, new RightCommand((PlayerSprite)mario, graphics));

            keyboardGameplay.AddKeyMapping(Keys.W.ToString(), (int)KeyboardController.Action.Release, new UpReleaseCommand((PlayerSprite)mario, graphics));
            keyboardGameplay.AddKeyMapping(Keys.S.ToString(), (int)KeyboardController.Action.Release, new DownReleaseCommand((PlayerSprite)mario, graphics));
            keyboardGameplay.AddKeyMapping(Keys.A.ToString(), (int)KeyboardController.Action.Release, new HorizontalReleaseCommand((PlayerSprite)mario, graphics));
            keyboardGameplay.AddKeyMapping(Keys.D.ToString(), (int)KeyboardController.Action.Release, new HorizontalReleaseCommand((PlayerSprite)mario, graphics));

            keyboardGameplay.AddKeyMapping(Keys.Up.ToString(), (int)KeyboardController.Action.Release, new UpReleaseCommand((PlayerSprite)mario, graphics));
            keyboardGameplay.AddKeyMapping(Keys.Down.ToString(), (int)KeyboardController.Action.Release, new DownReleaseCommand((PlayerSprite)mario, graphics));
            keyboardGameplay.AddKeyMapping(Keys.Left.ToString(), (int)KeyboardController.Action.Release, new HorizontalReleaseCommand((PlayerSprite)mario, graphics));
            keyboardGameplay.AddKeyMapping(Keys.Right.ToString(), (int)KeyboardController.Action.Release, new HorizontalReleaseCommand((PlayerSprite)mario, graphics));

            keyboardGameplay.AddKeyMapping(Keys.K.ToString(), (int)KeyboardController.Action.Press, new AttackCommand((PlayerSprite)mario));
            keyboardGameplay.AddKeyMapping(Keys.K.ToString(), (int)KeyboardController.Action.Release, new AttackReleaseCommand((PlayerSprite)mario));

            //GamePad input controller
            gamepadGameplay = new GamePadController(PlayerIndex.One);
            gamepadGameplay.AddKeyMapping(Buttons.X.ToString(), (int)GamePadController.Action.Hold, new QuitCommand(game));
            gamepadGameplay.AddKeyMapping(Buttons.Y.ToString(), (int)GamePadController.Action.Press, new SpriteVisibilityToggleCommand(mario));

            gamepadGameplay.AddKeyMapping(Buttons.Start.ToString(), (int)GamePadController.Action.Press, new PauseCommand(game));

            //Movement
            gamepadGameplay.AddKeyMapping(Buttons.DPadLeft.ToString(), (int)GamePadController.Action.Press, new LeftCommand((PlayerSprite)mario, graphics));
            gamepadGameplay.AddKeyMapping(Buttons.DPadRight.ToString(), (int)GamePadController.Action.Press, new RightCommand((PlayerSprite)mario, graphics));
            gamepadGameplay.AddKeyMapping(Buttons.DPadDown.ToString(), (int)GamePadController.Action.Press, new DownCommand((PlayerSprite)mario, graphics));
            gamepadGameplay.AddKeyMapping(Buttons.DPadUp.ToString(), (int)GamePadController.Action.Press, new UpCommand((PlayerSprite)mario, graphics));

            /*
            gamePadController.AddKeyMapping(Buttons.DPadLeft.ToString(), (int)GamePadController.Action.Hold, new LeftHoldCommand((PlayerSprite) mario, handler, graphics));
            gamePadController.AddKeyMapping(Buttons.DPadRight.ToString(), (int)GamePadController.Action.Hold, new RightHoldCommand((PlayerSprite) mario, handler, graphics));
            gamePadController.AddKeyMapping(Buttons.DPadDown.ToString(), (int)GamePadController.Action.Hold, new DownHoldCommand((PlayerSprite) mario, graphics, handler));
            gamePadController.AddKeyMapping(Buttons.DPadUp.ToString(), (int)GamePadController.Action.Hold, new UpHoldCommand((PlayerSprite) mario, graphics, handler));

            gamePadController.AddKeyMapping(Buttons.DPadLeft.ToString(), (int)GamePadController.Action.Release, new DirectionReleaseCommand((PlayerSprite) mario));
            gamePadController.AddKeyMapping(Buttons.DPadRight.ToString(), (int)GamePadController.Action.Release, new DirectionReleaseCommand((PlayerSprite) mario));
            gamePadController.AddKeyMapping(Buttons.DPadDown.ToString(), (int)GamePadController.Action.Release, new DirectionReleaseCommand((PlayerSprite) mario));
            gamePadController.AddKeyMapping(Buttons.DPadUp.ToString(), (int)GamePadController.Action.Release, new DirectionReleaseCommand((PlayerSprite) mario));
            */
        }

        public void PauseSetUp(Game1 game)
        {
            /*
            * Press executes on the initial press only
            * Hold executes once after a brief delay
            * Release executes once the button is released, regardless of how long button was held
            *
            * Get the Keys/Buttons with the toString, and use the Action enum in each controller
            */

            //Keyboard input controller
            keyboardPause = new KeyboardController();
            keyboardPause.AddKeyMapping(Keys.Q.ToString(), (int)KeyboardController.Action.Press, new QuitCommand(game));
            keyboardPause.AddKeyMapping(Keys.P.ToString(), (int)KeyboardController.Action.Press, new PauseCommand(game));
            keyboardPause.AddKeyMapping(Keys.R.ToString(), (int)KeyboardController.Action.Press, new ResetCommand(game));
            keyboardPause.AddKeyMapping(Keys.M.ToString(), (int)KeyboardController.Action.Press, new MuteCommand(game));

            gamepadPause = new GamePadController(PlayerIndex.One);
            gamepadPause.AddKeyMapping(Buttons.Start.ToString(), (int)GamePadController.Action.Press, new PauseCommand(game));
        }

        public void Update(bool pause, GameTime gameTime)
        {
            if (pause)
            {
                //Without delay, keyboardPause receives same input as keyboardGameplay on the FIRST run through Update when
                // pause is pressed. After the first time Update runs, it works properly.
                // delay is to not run keyboardPause on first time.
                if (count > Delay) 
                {
                    keyboardPause.Update(gameTime);
                    gamepadPause.Update(gameTime);
                    
                }
                else
                {
                    count++;
                }
            }
            else
            {
                
                keyboardGameplay.Update(gameTime);
                gamepadGameplay.Update(gameTime);
            }
        }
    }
}
