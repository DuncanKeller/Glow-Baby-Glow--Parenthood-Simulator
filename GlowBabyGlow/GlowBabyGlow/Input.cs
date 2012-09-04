using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GlowBabyGlow
{
    public delegate void InputChangeHandler();

    class Input
    {
        static event InputChangeHandler Jump;
        static List<GamePadState> gamepad = new List<GamePadState>();
        static List<GamePadState> prevgamepad = new List<GamePadState>();

        public static GamePadThumbSticks GetThumbs(int index)
        {
            return gamepad[index].ThumbSticks;
        }

        public static int GetThumbsDebugX()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) { return -1; }
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) { return 1; }
            return 0;
        }

        public static int GetThumbsDebugY()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) { return -1; }
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) { return 1; }
            return 0;
        }

        public static GamePadTriggers GetTriggers(int index)
        {
            return gamepad[index].Triggers;
        }

        public static void Init()
        {
            for (int i = 0; i < 4; i++)
            {
                gamepad.Add(new GamePadState());
                prevgamepad.Add(new GamePadState());
            }
        }

        public static void Update()
        {
            gamepad[0] = GamePad.GetState(PlayerIndex.One);
            gamepad[1] = GamePad.GetState(PlayerIndex.Two);
            gamepad[2] = GamePad.GetState(PlayerIndex.Three);
            gamepad[3] = GamePad.GetState(PlayerIndex.Four);

            for (int i = 0; i < World.Players.Count; i++)
            {
                if (gamepad[i].Buttons.A == ButtonState.Pressed &&
                    prevgamepad[i].Buttons.A == ButtonState.Released)
                {
                    World.Players[i].Jump();
                }

                if (World.Players[i].HoldingBaby)
                {
                    if (gamepad[i].Buttons.X == ButtonState.Pressed)
                    {
                        World.Players[i].ReadyToThrow = true;
                    }
                    else
                    {
                        if (prevgamepad[i].Buttons.X == ButtonState.Pressed)
                        {
                            World.Players[i].Throw();
                        }
                        World.Players[i].ReadyToThrow = false;
                    }
                }
                else
                {
                    if (gamepad[i].Buttons.X == ButtonState.Pressed && 
                        prevgamepad[i].Buttons.X == ButtonState.Released)
                    {
                        World.Players[i].Shoot();
                    }
                }
                
            }
            //debug only
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                World.Players[0].Jump();
            }

            for (int i = 0; i < 4; i++)
            {
                prevgamepad[i] = gamepad[i];
            }
        }

       
    }
}
