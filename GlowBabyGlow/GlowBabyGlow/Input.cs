using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GlowBabyGlow
{
    public delegate void InputChangeHandler(int playerIndex);

    class Input
    {
        static event InputChangeHandler Jump;
        static List<GamePadState> gamepad = new List<GamePadState>();
        static List<GamePadState> prevgamepad = new List<GamePadState>();

        public static GamePadThumbSticks GetLeftThumbs(int index)
        {
            return gamepad[index].ThumbSticks;
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

            for (int i = 0; i < 4; i++)
            {
                if (gamepad[i].Buttons.A == ButtonState.Pressed &&
                    prevgamepad[i].Buttons.A == ButtonState.Released)
                {
                    Jump(i);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                prevgamepad[i] = gamepad[i];
            }
        }

       
    }
}
