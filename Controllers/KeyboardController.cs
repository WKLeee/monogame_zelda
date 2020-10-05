using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint_1.Commands;
using Sprint_1.Interfaces;

namespace Sprint_1.Controllers
{
    class KeyboardController : IController
    {
        private const int HoldDelay = 500;

        private KeyboardState keyState;
        private readonly Dictionary<string, ICommand> keyCommandMapping;
        private readonly Dictionary<Keys, int> heldKeys;

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Action
        {
            Press, Hold, Release, Chord
        }

        public KeyboardController()
        {
            keyState = Keyboard.GetState();
            keyCommandMapping = new Dictionary<string, ICommand>();
            heldKeys = new Dictionary<Keys, int>();
        }

        public void AddKeyMapping(string key, int action, ICommand command)
        {
            keyCommandMapping.Add(key + (Action)action, command);
        }

        //The print commands are to demonstrate detection and can be removed
        public void Update(GameTime gameTime)
        {
            KeyboardState oldState = keyState;
            keyState = Keyboard.GetState();
            Keys[] pressedKeys = keyState.GetPressedKeys();

            //Go through each pressed key
            foreach (Keys pressed in pressedKeys)
            {
                //Key is newly pressed
                if (!oldState.IsKeyDown(pressed))
                {
                    //Keep track of held keys
                    if (!heldKeys.ContainsKey(pressed))
                    {
                        heldKeys.Add(pressed, 0);
                    }

                    ICommand buttonPressCommand = new PrintPressCommand(pressed.ToString());
                    buttonPressCommand.Execute();

                    //Executes command in dictionary
                    if (keyCommandMapping.ContainsKey(pressed + Action.Press.ToString()))
                    {
                        keyCommandMapping[pressed + Action.Press.ToString()].Execute();
                    }
                } 

            }

            List<Keys> keys = new List<Keys>(heldKeys.Keys);

            //Checks held keys
            foreach (Keys previouslyHeld in keys)
            {
                //Key has been released
                if (keyState.IsKeyUp(previouslyHeld))
                {
                    heldKeys.Remove(previouslyHeld);

                    if (keyCommandMapping.ContainsKey(previouslyHeld + Action.Release.ToString()))
                    {
                        keyCommandMapping[previouslyHeld + Action.Release.ToString()].Execute();
                    }

                    ICommand buttonReleaseCommand = new PrintReleaseCommand(previouslyHeld.ToString());
                    buttonReleaseCommand.Execute();
                } else
                {
                    //Key continues to be held

                    //This is to ensure the held command only executes once
                    if (heldKeys[previouslyHeld] >= 0)
                    {
                        heldKeys[previouslyHeld] = heldKeys[previouslyHeld] + gameTime.ElapsedGameTime.Milliseconds;
                    }
                    
                    //Short time delay
                    if (heldKeys[previouslyHeld] > HoldDelay)
                    {
                        ICommand buttonHeldCommand = new PrintHoldCommand(previouslyHeld.ToString());
                        buttonHeldCommand.Execute();

                        if (keyCommandMapping.ContainsKey(previouslyHeld + Action.Hold.ToString()))
                        {
                            keyCommandMapping[previouslyHeld + Action.Hold.ToString()].Execute();
                        }

                        heldKeys[previouslyHeld] = -1;
                    }
                }
            } 

            //Checks chords
            if (!keyState.Equals(oldState) && heldKeys.Count > 1)
            {
                string keyStrings = "";

                //Sort for consistency
                keys.Sort();
                foreach (Keys key in keys)
                {
                    keyStrings += key.ToString();
                }

                if (keyCommandMapping.ContainsKey(keyStrings + Action.Chord.ToString()))
                {
                    keyCommandMapping[keyStrings + Action.Chord.ToString()].Execute();
                }

                ICommand buttonChordCommand = new PrintChordCommand(keyStrings);
                buttonChordCommand.Execute();
            }
        }
    }
}
