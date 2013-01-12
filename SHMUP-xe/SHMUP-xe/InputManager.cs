using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP
{
    public class InputManager
    {
        PlayerIndex playerOne;
        public bool playerOneSet = false;
        public bool playerOneKeyboard = false;

        public InputState playerOneInput;

        public static Texture2D mouseLeft;
        public static Texture2D mouseRight;
        public static Texture2D mouseMiddle;

        public InputManager()
        {
            playerOneInput = new InputState();
        }

        public void setPlayerOne(PlayerIndex inPI)
        {
            playerOne = inPI;
            playerOneSet = true;
        }

        public bool checkInitalInput()
        {
#if WINDOWS
            if (Keyboard.GetState().GetPressedKeys().Length > 0 ||
                Mouse.GetState().LeftButton == ButtonState.Pressed ||
                Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                playerOneSet = true;
                playerOneKeyboard = true;
                InputManager.mouseLeft = Game1.content.Load<Texture2D>("mouseL");
                InputManager.mouseRight = Game1.content.Load<Texture2D>("mouseR");
                InputManager.mouseMiddle = Game1.content.Load<Texture2D>("mouseM");
                return true;
            }
#endif
            for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
            {
                Boolean retbool = false;
                if (GamePad.GetState(index).Buttons.Start == ButtonState.Pressed)
                {
                    playerOne = index;
                    playerOneSet = true;
                    retbool = true;
                }

                if (GamePad.GetState(index).Buttons.A == ButtonState.Pressed)
                {
                    playerOne = index;
                    playerOneSet = true;
                    retbool = true;
                }

                if (GamePad.GetState(index).Buttons.B == ButtonState.Pressed)
                {
                    playerOne = index;
                    playerOneSet = true;
                    retbool = true;
                }

                if (GamePad.GetState(index).Buttons.X == ButtonState.Pressed)
                {
                    playerOne = index;
                    playerOneSet = true;
                    retbool = true;
                }

                if (GamePad.GetState(index).Buttons.Y == ButtonState.Pressed)
                {
                    playerOne = index;
                    playerOneSet = true;
                    retbool = true;
                }

                if (retbool)
                {
                    InputManager.mouseLeft = Game1.content.Load<Texture2D>("button_a");
                    InputManager.mouseRight = Game1.content.Load<Texture2D>("button_b");
                    InputManager.mouseMiddle = Game1.content.Load<Texture2D>("lStickLR");
                    return retbool;
                }
            }
            return false;
        }
        
        public void update(GameTime gameTime)
        {
            if (playerOneSet)
            {
                if (playerOneKeyboard)
                    playerOneInput.Update(Keyboard.GetState());
                else
                {
                    playerOneInput.Update(GamePad.GetState(playerOne));
                }
            }
        }
    }


    public enum expButtonState
    {
        notPressed,
        Pressed,
        Held
    }

    public class InputState
    {

        public Vector2 leftStick;
        public Vector2 rightStick;

        public float leftTrigger;
        public float rightTrigger;

        public float leftRumble;
        public float rightRumble;

        public expButtonState A = expButtonState.notPressed;
        public expButtonState B = expButtonState.notPressed;
        public expButtonState X = expButtonState.notPressed;
        public expButtonState Y = expButtonState.notPressed;

        public expButtonState Rb = expButtonState.notPressed;
        public expButtonState Lb = expButtonState.notPressed;

        public expButtonState Start = expButtonState.notPressed;
        public expButtonState Back = expButtonState.notPressed;

        public expButtonState dU = expButtonState.notPressed;
        public expButtonState dD = expButtonState.notPressed;
        public expButtonState dL = expButtonState.notPressed;
        public expButtonState dR = expButtonState.notPressed;

        //Keys keyboardlsUp = Keys.Up;
        //Keys keyboardlsDown = Keys.Down;
        //Keys keyboardlsLeft = Keys.Left;
        //Keys keyboardlsRight = Keys.Right;

        //Keys keyboardrsUp;
        //Keys keyboardrsDown;
        //Keys keyboardrsLeft;
        //Keys keyboardrsRight;

        //Keys keyboardLeftTrigger;
        //Keys keyboardRightTrigger = Keys.LeftControl;

        //Keys keyboardA = Keys.Space;
        //Keys keyboardB = Keys.LeftControl;
        //Keys keyboardX = Keys.Q;
        //Keys keyboardY = Keys.W;

        //Keys keyboardRb;
        //Keys keyboardLb;

        Keys keyboardStart = Keys.Enter;
        Keys keyboardBack = Keys.Escape;

        //Keys keyboarddU;
        //Keys keyboarddD;
        //Keys keyboarddL;
        //Keys keyboarddR;

        public MouseState mouseState;
        int lastMouseWheelNumber = 0;
        public int mouseWheelState = 0;


        public InputState()
        {
        }

        public void Update(GamePadState gps)
        {
            leftStick = gps.ThumbSticks.Left;
            rightStick = gps.ThumbSticks.Right;

            leftStick.Y *= -1;
            rightStick.Y *= -1;

            leftTrigger = gps.Triggers.Left;
            rightTrigger = gps.Triggers.Right;

            if (leftRumble > 0.01f)
                leftRumble -= 0.025f;
            else
                leftRumble = 0;

            if (rightRumble > 0.01f)
                rightRumble -= 0.01f;
            else
                rightRumble = 0;

            GamePad.SetVibration(PlayerIndex.One, leftRumble, rightRumble);

            #region A button
            if (gps.Buttons.A == ButtonState.Pressed)
            {
                if (A == expButtonState.notPressed)
                {
                    A = expButtonState.Pressed;
                }
                else if (A == expButtonState.Pressed)
                {
                    A = expButtonState.Held;
                }
            }
            else
            {
                A = expButtonState.notPressed;
            }
            #endregion
            #region B button
            if (gps.Buttons.B == ButtonState.Pressed)
            {
                if (B == expButtonState.notPressed)
                {
                    B = expButtonState.Pressed;
                }
                else if (B == expButtonState.Pressed)
                {
                    B = expButtonState.Held;
                }
            }
            else
            {
                B = expButtonState.notPressed;
            }
            #endregion
            #region X button
            if (gps.Buttons.X == ButtonState.Pressed)
            {
                if (X == expButtonState.notPressed)
                {
                    X = expButtonState.Pressed;
                }
                else if (X == expButtonState.Pressed)
                {
                    X = expButtonState.Held;
                }
            }
            else
            {
                X = expButtonState.notPressed;
            }
            #endregion
            #region Y button
            if (gps.Buttons.Y == ButtonState.Pressed)
            {
                if (Y == expButtonState.notPressed)
                {
                    Y = expButtonState.Pressed;
                }
                else if (Y == expButtonState.Pressed)
                {
                    Y = expButtonState.Held;
                }
            }
            else
            {
                Y = expButtonState.notPressed;
            }
            #endregion

            #region Rb button
            if (gps.Buttons.RightShoulder == ButtonState.Pressed)
            {
                if (Rb == expButtonState.notPressed)
                {
                    Rb = expButtonState.Pressed;
                }
                else if (Rb == expButtonState.Pressed)
                {
                    Rb = expButtonState.Held;
                }
            }
            else
            {
                Rb = expButtonState.notPressed;
            }
            #endregion
            #region Lb button
            if (gps.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                if (Lb == expButtonState.notPressed)
                {
                    Lb = expButtonState.Pressed;
                }
                else if (Lb == expButtonState.Pressed)
                {
                    Lb = expButtonState.Held;
                }
            }
            else
            {
                Lb = expButtonState.notPressed;
            }
            #endregion

            #region Start button
            if (gps.Buttons.Start == ButtonState.Pressed)
            {
                if (Start == expButtonState.notPressed)
                {
                    Start = expButtonState.Pressed;
                }
                else if (Start == expButtonState.Pressed)
                {
                    Start = expButtonState.Held;
                }
            }
            else
            {
                Start = expButtonState.notPressed;
            }
            #endregion
            #region Back button
            if (gps.Buttons.Back == ButtonState.Pressed)
            {
                if (Back == expButtonState.notPressed)
                {
                    Back = expButtonState.Pressed;
                }
                else if (Back == expButtonState.Pressed)
                {
                    Back = expButtonState.Held;
                }
            }
            else
            {
                Back = expButtonState.notPressed;
            }
            #endregion

            #region dU button
            if (gps.DPad.Up == ButtonState.Pressed)
            {
                if (dU == expButtonState.notPressed)
                {
                    dU = expButtonState.Pressed;
                }
                else if (dU == expButtonState.Pressed)
                {
                    dU = expButtonState.Held;
                }
            }
            else
            {
                dU = expButtonState.notPressed;
            }
            #endregion
            #region dD button
            if (gps.DPad.Down == ButtonState.Pressed)
            {
                if (dD == expButtonState.notPressed)
                {
                    dD = expButtonState.Pressed;
                }
                else if (dD == expButtonState.Pressed)
                {
                    dD = expButtonState.Held;
                }
            }
            else
            {
                dD = expButtonState.notPressed;
            }
            #endregion
            #region dL button
            if (gps.DPad.Left == ButtonState.Pressed)
            {
                if (dL == expButtonState.notPressed)
                {
                    dL = expButtonState.Pressed;
                }
                else if (dL == expButtonState.Pressed)
                {
                    dL = expButtonState.Held;
                }
            }
            else
            {
                dL = expButtonState.notPressed;
            }
            #endregion
            #region dR button
            if (gps.DPad.Right == ButtonState.Pressed)
            {
                if (dR == expButtonState.notPressed)
                {
                    dR = expButtonState.Pressed;
                }
                else if (dR == expButtonState.Pressed)
                {
                    dR = expButtonState.Held;
                }
            }
            else
            {
                dR = expButtonState.notPressed;
            }
            #endregion
        }

        int scrollPauser = 0;

        public void Update(KeyboardState kbs)
        {
            #region Kbd->ls
            //if (kbs.IsKeyDown(keyboardlsUp))
            //{
            //    leftStick.Y += -0.07f;
            //    if (leftStick.Y <= -1)
            //    {
            //        leftStick.Y = -1;
            //    }
            //}
            //if (kbs.IsKeyDown(keyboardlsDown))
            //{
            //    leftStick.Y += 0.07f;
            //    if (leftStick.Y >= 1)
            //    {
            //        leftStick.Y = 1;
            //    }
            //}
            //if (kbs.IsKeyDown(keyboardlsRight))
            //{
            //    leftStick.X += 0.07f;
            //    if (leftStick.X >= 1)
            //    {
            //        leftStick.X = 1;
            //    }
            //}
            //if (kbs.IsKeyDown(keyboardlsLeft))
            //{
            //    leftStick.X += -0.07f;
            //    if (leftStick.X <= -1)
            //    {
            //        leftStick.X = -1;
            //    }
            //}

            //if (kbs.IsKeyUp(keyboardlsLeft) &&
            //    kbs.IsKeyUp(keyboardlsRight))
            //{
            //    leftStick.X *= 0.8f;
            //}
            //if (kbs.IsKeyUp(keyboardlsDown) &&
            //    kbs.IsKeyUp(keyboardlsUp))
            //{
            //    leftStick.Y *= 0.8f;
            //}
            #endregion

            mouseState = Mouse.GetState();

            //if (!Game1.gammgr.isPaused)
            //{
            //    int tX = mouseState.X;
            //    int tY = mouseState.Y;
            //    if (tX < 0)
            //        tX = 0;
            //    if (tY < -5)
            //        tY = -5;
            //    if (tX > Game1.scrmgr.screenSize.X)
            //        tX = (int)Game1.scrmgr.screenSize.X;
            //    if (tY > Game1.scrmgr.screenSize.Y)
            //        tY = (int)Game1.scrmgr.screenSize.Y;
            //    Mouse.SetPosition(tX, tY);
            //}

            #region mouse->ls

            leftStick.X = (float)mouseState.X / (float)Game1.scrmgr.screenSize.X;
            leftStick.Y = (float)mouseState.Y / (float)Game1.scrmgr.screenSize.Y;
            #endregion

            if (scrollPauser < 15)
            {
                scrollPauser++;
                mouseWheelState = 0;
            }
            else
            {
                if (mouseState.ScrollWheelValue > lastMouseWheelNumber)
                {
                    mouseWheelState = 1;
                    lastMouseWheelNumber = mouseState.ScrollWheelValue;
                    scrollPauser = 0;
                }
                else if (mouseState.ScrollWheelValue < lastMouseWheelNumber)
                {
                    mouseWheelState = -1;
                    lastMouseWheelNumber = mouseState.ScrollWheelValue;
                    scrollPauser = 0;
                }
                else
                {
                    mouseWheelState = 0;
                }
            }

            #region A button
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (A == expButtonState.notPressed)
                {
                    A = expButtonState.Pressed;
                }
                else if (A == expButtonState.Pressed)
                {
                    A = expButtonState.Held;
                }
            }
            else
            {
                A = expButtonState.notPressed;
            }
            #endregion
            #region B button
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                if (B == expButtonState.notPressed)
                {
                   B = expButtonState.Pressed;
                }
                else if (B == expButtonState.Pressed)
                {
                    B = expButtonState.Held;
                }
            }
            else
            {
                B = expButtonState.notPressed;
            }
            #endregion

            #region Start button
            if (kbs.IsKeyDown(keyboardStart))
            {
                if (Start == expButtonState.notPressed)
                {
                    Start = expButtonState.Pressed;
                }
                else if (Start == expButtonState.Pressed)
                {
                    Start = expButtonState.Held;
                }
            }
            else
            {
                Start = expButtonState.notPressed;
            }
            #endregion
            #region Back button
            if (kbs.IsKeyDown(keyboardBack))
            {
                if (Back == expButtonState.notPressed)
                {
                    Back = expButtonState.Pressed;
                }
                else if (Back == expButtonState.Pressed)
                {
                    Back = expButtonState.Held;
                }
            }
            else
            {
                Back = expButtonState.notPressed;
            }
            #endregion
        }


        public void DEBUGOUTPUT(SpriteBatch spriteBatch)
        {
            String s = "";
            
        s += leftStick.ToString() + "  ";
        s += rightStick.ToString() + Environment.NewLine;

        s += leftTrigger + "  ";
        s += rightTrigger + Environment.NewLine;

        s += "A: " + A.ToString() + "  ";
        s += "B: " + B.ToString() + "  ";
        s += "X: " + X.ToString() + "  ";
        s += "Y: " + Y.ToString() + Environment.NewLine;

        s += "Rb: " + Rb.ToString() + "  ";
        s += "Lb: " + Lb.ToString() + Environment.NewLine;

        s += "Start: " + Start.ToString() + "  ";
        s += "Back: " + Back.ToString();

        Game1.scrmgr.drawString(spriteBatch, Game1.debugFont, s, new Vector2(0.5f, 0.5f), justification.left);
        }
    }
}