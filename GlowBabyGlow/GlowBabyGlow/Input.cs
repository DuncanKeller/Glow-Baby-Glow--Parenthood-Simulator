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
        public static bool keys = true;
        static event InputChangeHandler Jump;
        
        static List<GamePadState> gamepad = new List<GamePadState>();
        static List<GamePadState> prevgamepad = new List<GamePadState>();

        static KeyboardState keyboard = new KeyboardState();
        static KeyboardState prevkeyboard = new KeyboardState();

        static Keys primary = Keys.Space;
        static Keys secondary = Keys.LeftShift;


        public static Vector2 GetThumbs(int index)
        {
            float x = 0;
            float y = 0;
            if (keys)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left)) { x = -1; }
                if (Keyboard.GetState().IsKeyDown(Keys.Right)) { x = 1; }
                if (Keyboard.GetState().IsKeyDown(Keys.Up)) { y = 1; }
                if (Keyboard.GetState().IsKeyDown(Keys.Down)) { y = -1; ; }
            }
            else
            {
                x = gamepad[index].ThumbSticks.Left.X;
                y = gamepad[index].ThumbSticks.Left.Y;
            }
            return new Vector2(x, y);
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

        #region KeyFunctions

        public static bool HoldingPrimary(int index)
        {
            if (keys)
            {
                return keyboard.IsKeyDown(primary);
            }
            else
            {
                return gamepad[index].Buttons.A == ButtonState.Pressed;
            }
        }

        public static bool HoldingPrimaryPrev(int index)
        {
            if (keys)
            {
                return prevkeyboard.IsKeyDown(primary);
            }
            else
            {
                return prevgamepad[index].Buttons.A == ButtonState.Pressed;
            }
        }

        public static bool HoldingSecondary(int index)
        {
            if (keys)
            {
                return keyboard.IsKeyDown(secondary);
            }
            else
            {
                return gamepad[index].Buttons.X == ButtonState.Pressed;
            }
        }

        public static bool HoldingSecondaryPrev(int index)
        {
            if (keys)
            {
                return prevkeyboard.IsKeyDown(secondary);
            }
            else
            {
                return prevgamepad[index].Buttons.X == ButtonState.Pressed;
            }
        }
#endregion


        public static void Update()
        {
            gamepad[0] = GamePad.GetState(PlayerIndex.One);
            gamepad[1] = GamePad.GetState(PlayerIndex.Two);
            gamepad[2] = GamePad.GetState(PlayerIndex.Three);
            gamepad[3] = GamePad.GetState(PlayerIndex.Four);
            keyboard = Keyboard.GetState();

            for (int i = 0; i < World.Players.Count; i++)
            {
                if (keyboard.IsKeyDown(Keys.T))
                {
                    World.Explode();
                }

                if (World.Players[i].HoldingBaby)
                {
                    if (!World.Players[i].InAir)
                    {
                        if (HoldingPrimary(i))
                        {
                            World.Players[i].ReadyToThrow = true;
                        }
                        else
                        {
                            if (HoldingPrimaryPrev(i))
                            {
                                World.Players[i].Throw();
                            }
                            World.Players[i].ReadyToThrow = false;
                        }
                    }

                    if (!World.Players[i].ReadyToThrow)
                    {
                        if (HoldingSecondary(i))
                        {
                            if (!World.Players[i].InAir)
                            {
                                float x = GetThumbs(i).X;
                                float y = GetThumbs(i).Y;
                                float angle = 0;
                                if (keys)
                                {
                                    World.Players[i].StartShake();
                                    if (keyboard.IsKeyDown(Keys.Right) &&
                                        prevkeyboard.IsKeyUp(Keys.Right) ||
                                        keyboard.IsKeyDown(Keys.Left) &&
                                        prevkeyboard.IsKeyUp(Keys.Left))
                                    {
                                        World.Players[i].KeyShake(x);
                                    }
                                }
                                else
                                {
                                    angle = (float)Math.Atan2(x, y);
                                    World.Players[i].Shake(angle);
                                }
                            }
                        }
                        else
                        {
                            World.Players[i].StopShaking();
                        }
                    }
                }
                else
                {
                    if (HoldingPrimary(i) &&
                    !HoldingPrimaryPrev(i))
                    {
                        World.Players[i].Jump();
                    }

                    if (HoldingSecondary(i) &&
                        !HoldingSecondaryPrev(i))
                    {
                        World.Players[i].Shoot();
                    }
                }

            }

            for (int i = 0; i < 4; i++)
            {
                prevgamepad[i] = gamepad[i];
            }
            prevkeyboard = keyboard;
        }

       
    }
}
