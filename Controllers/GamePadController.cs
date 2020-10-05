using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint_1.Commands;
using Sprint_1.Interfaces;

namespace Sprint_1.Controllers
{
    class GamePadController : IController
    {
        private const int HoldDelay = 500;

        private GamePadState gamePadState;
        private readonly Dictionary<string, ICommand> buttonCommandMapping;
        private readonly Dictionary<string, int> heldButtons;
        private readonly PlayerIndex myIndex;
  
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Action
        {
            Press, Hold, Release, Chord
        }

        public GamePadController(PlayerIndex index)
        {
            myIndex = index;
            gamePadState = GamePad.GetState(myIndex);
            buttonCommandMapping = new Dictionary<string, ICommand>();
            heldButtons = new Dictionary<string, int>();
        }

        public void AddKeyMapping(string button, int action, ICommand command)
        {
            buttonCommandMapping.Add(button + (Action)action, command);
        }

        //The print commands are to demonstrate detection, they can be replaced with calling commands
        //added to the dictionary
        public void Update(GameTime gameTime)
        {
            //Only update if connected
            if (GamePad.GetState(myIndex).IsConnected)
            {
                GamePadState oldState = gamePadState;
                gamePadState = GamePad.GetState(myIndex);

                var buttonList = (Buttons[])Enum.GetValues(typeof(Buttons));

                foreach (var button in buttonList)
                {
                    if (gamePadState.IsButtonDown(button))
                    {
                        //Keep track of held buttons
                        if (!heldButtons.ContainsKey(button.ToString()))
                        {
                            heldButtons.Add(button.ToString(), 0);
                        }
                        else
                        {
                            //Manage the continued held buttons

                            //To ensure hold command executes only once
                            if (heldButtons[button.ToString()] >= 0)
                            {
                                heldButtons[button.ToString()] = heldButtons[button.ToString()] + gameTime.ElapsedGameTime.Milliseconds;
                            }
                     
                            //Short time delay
                            if (heldButtons[button.ToString()] > HoldDelay)
                            {
                                ICommand buttonHeldCommand = new PrintHoldCommand(button.ToString());
                                buttonHeldCommand.Execute();
                            
                                if (buttonCommandMapping.ContainsKey(button + Action.Hold.ToString()))
                                {
                                    buttonCommandMapping[button + Action.Hold.ToString()].Execute();
                                }

                                heldButtons[button.ToString()] = -1;
                            }
                        }

                        //Button is newly pressed
                        if (oldState.IsButtonUp(button))
                        {
                            ICommand buttonPressCommand = new PrintPressCommand(button.ToString());
                            buttonPressCommand.Execute();

                            if (buttonCommandMapping.ContainsKey(button + Action.Press.ToString()))
                            {
                                buttonCommandMapping[button + Action.Press.ToString()].Execute();
                            }
                        }

                    }
                    else
                    {
                        //Button is released

                        if (heldButtons.ContainsKey(button.ToString()))
                        {
                            heldButtons.Remove(button.ToString());

                            ICommand buttonReleaseCommand = new PrintReleaseCommand(button.ToString());
                            buttonReleaseCommand.Execute();

                            if (buttonCommandMapping.ContainsKey(button + Action.Release.ToString()))
                            {
                                buttonCommandMapping[button + Action.Release.ToString()].Execute();
                            }
                        }
                    }
                }

                //Check chords
                if (!oldState.Equals(gamePadState) && heldButtons.Count > 1)
                {
                    string buttonStrings = "";

                    List<string> heldButtonList = new List<string>(heldButtons.Keys);

                    //sort for consistency
                    heldButtonList.Sort();
                    foreach (string buttonString in heldButtonList)
                    {
                        buttonStrings += buttonString;
                    }

                    ICommand buttonChordCommand = new PrintChordCommand(buttonStrings);
                    buttonChordCommand.Execute();

                    if (buttonCommandMapping.ContainsKey(buttonStrings + Action.Chord.ToString()))
                    {
                        buttonCommandMapping[buttonStrings + Action.Chord.ToString()].Execute();
                    }
                }
            }
        }
    }
    
}
