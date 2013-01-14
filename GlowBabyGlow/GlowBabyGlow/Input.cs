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
        public static bool keys = false;
        public static int defaultIndex = 0;
        static event InputChangeHandler Jump;
        
        static List<GamePadState> gamepad = new List<GamePadState>();
        static List<GamePadState> prevgamepad = new List<GamePadState>();

        static KeyboardState keyboard = new KeyboardState();
        static KeyboardState prevkeyboard = new KeyboardState();

        static Keys primary = Keys.Space;
        static Keys secondary = Keys.LeftShift;

        static World world;
        public static bool spaceBarPreventativeMeasureFlag = false;

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

        public static Vector2 GetPrevThumbs(int index)
        {
            float x = 0;
            float y = 0;
            if (keys)
            {
                if (prevkeyboard.IsKeyDown(Keys.Left)) { x = -1; }
                if (prevkeyboard.IsKeyDown(Keys.Right)) { x = 1; }
                if (prevkeyboard.IsKeyDown(Keys.Up)) { y = 1; }
                if (prevkeyboard.IsKeyDown(Keys.Down)) { y = -1; ; }
            }
            else
            {
                x = prevgamepad[index].ThumbSticks.Left.X;
                y = prevgamepad[index].ThumbSticks.Left.Y;
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

        public static bool StartPressed()
        {
            if (keys)
            {
                if (keyboard.IsKeyDown(Keys.Enter)
                    && prevkeyboard.IsKeyUp(Keys.Enter))
                {
                    return true;
                }
            }
            else
            {
                if (gamepad[defaultIndex].IsButtonDown(Buttons.Start) &&
                    prevgamepad[defaultIndex].IsButtonUp(Buttons.Start))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ExitPressed()
        {
            if (keys)
            {
                if (keyboard.IsKeyDown(Keys.Escape)
                    && prevkeyboard.IsKeyUp(Keys.Escape))
                {
                    return true;
                }
            }
            else
            {
                if (gamepad[defaultIndex].IsButtonDown(Buttons.Back) &&
                    prevgamepad[defaultIndex].IsButtonUp(Buttons.Back))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool PausePressed()
        {
            if (keys)
            {
                if (keyboard.IsKeyDown(Keys.Enter) ||
                   keyboard.IsKeyDown(Keys.P))
                {
                    if (prevkeyboard.IsKeyUp(Keys.Enter) &&
                        prevkeyboard.IsKeyUp(Keys.P))
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (Player p in world.Players)
                {
                    if (gamepad[p.Index].Buttons.Start == ButtonState.Pressed)
                    {
                        if (prevgamepad[p.Index].Buttons.Start == ButtonState.Released)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static void Init(World w)
        {
            world = w;
            for (int i = 0; i < 4; i++)
            {
                gamepad.Add(new GamePadState());
                prevgamepad.Add(new GamePadState());
            }

            bool connected = false;
            for (int i = 0; i < 4; i++)
            {
                if (gamepad[i].IsConnected)
                {
                    keys = false;
                    defaultIndex = i;
                    connected = true;
                    break;
                }
            }

            if (!connected)
            { keys = true; }
        }

        #region KeyFunctions

        public static bool HoldingPrimary(int index)
        {
            if (keys)
            {
                if (index == 0)
                {
                    return keyboard.IsKeyDown(primary);
                }
                return false;
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
                if (index == 0)
                {
                    return prevkeyboard.IsKeyDown(primary);
                }
                return false;
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
                if (index == 0)
                {
                    return keyboard.IsKeyDown(secondary);
                }
                return false;
            }
            else
            {
                return gamepad[index].Buttons.X == ButtonState.Pressed ||
                    gamepad[index].Buttons.B == ButtonState.Pressed;
            }
        }

        public static bool HoldingSecondaryPrev(int index)
        {
            if (keys)
            {
                if (index == 0)
                {
                    return prevkeyboard.IsKeyDown(secondary);
                }
                return false;
            }
            else
            {
                return prevgamepad[index].Buttons.X == ButtonState.Pressed ||
                    prevgamepad[index].Buttons.B == ButtonState.Pressed;
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

            if (PausePressed())
            {
                world.Paused = !world.Paused;
            }

            if (!world.Paused)
            {
                for (int i = 0; i < world.Players.Count; i++)
                {
                    if (world.Players[i].HoldingBaby)
                    {
                        if (!world.Players[i].InAir)
                        {
                            if (HoldingPrimary(world.Players[i].Index))
                            {
                                if (!world.Players[i].Shaking)
                                {
                                    world.Players[i].ReadyToThrow = true;
                                }
                            }
                            else
                            {
                                if (!world.Players[i].Shaking)
                                {
                                    if (HoldingPrimaryPrev(world.Players[i].Index) && spaceBarPreventativeMeasureFlag)
                                    {
                                        world.Players[i].Throw();

                                    }
                                    else if (HoldingPrimaryPrev(world.Players[i].Index))
                                    {
                                        spaceBarPreventativeMeasureFlag = true;
                                    }
                                    world.Players[i].ReadyToThrow = false;
                                }
                            }
                        }

                        if (!world.Players[i].ReadyToThrow)
                        {
                            if (HoldingSecondary(world.Players[i].Index))
                            {
                                if (!world.Players[i].InAir)
                                {
                                    if (world.Players[i].Baby == null)
                                    {
                                        float x = GetThumbs(world.Players[i].Index).X;
                                        float y = GetThumbs(world.Players[i].Index).Y;
                                        float angle = 0;
                                        if (keys)
                                        {
                                            world.Players[i].StartShake();
                                            if (keyboard.IsKeyDown(Keys.Right) &&
                                                prevkeyboard.IsKeyUp(Keys.Right) ||
                                                keyboard.IsKeyDown(Keys.Left) &&
                                                prevkeyboard.IsKeyUp(Keys.Left))
                                            {
                                                world.Players[i].KeyShake(x);
                                            }
                                        }
                                        else
                                        {
                                            angle = (float)Math.Atan2(x, y);
                                            world.Players[i].Shake(angle);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                world.Players[i].StopShaking();
                            }
                        }
                    }
                    else
                    {
                        if (HoldingPrimary(world.Players[i].Index) &&
                        !HoldingPrimaryPrev(world.Players[i].Index))
                        {
                            world.Players[i].Jump();
                        }

                        if (HoldingSecondary(world.Players[i].Index) &&
                            !HoldingSecondaryPrev(world.Players[i].Index))
                        {
                            world.Players[i].Shoot();
                        }
                    }
                }
            }
            else
            {
                if (ExitPressed())
                {
                    GameOver.Reset();
                    world.Reset();
                    world.Init(world.LevelName);
                    MenuSystem.Reset();
                    world.Paused = false;
                }
            }
        }

        public static void LateUpdate()
        {
            for (int i = 0; i < 4; i++)
            {
                prevgamepad[i] = gamepad[i];
            }

            prevkeyboard = keyboard;
        }

       
    }
}
