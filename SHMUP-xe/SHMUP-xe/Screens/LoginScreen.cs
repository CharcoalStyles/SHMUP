using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace SHMUP.Screens
{
    class LoginScreen : Screen
    {
        bool loaded = false;

        public string[,] KeyConvert = 
        { 
            {"D1", "1"}, {"D2", "2"}, 
            {"D3", "3"}, {"D4", "4"}, 
            {"D5", "5"}, {"D6", "6"},
            {"D7", "7"}, {"D8", "8"}, 
            {"D9", "9"}, {"D0", "0"}, 
            {"NumPad1", "1"}, {"NumPad2", "2"}, 
            {"NumPad3", "3"}, {"NumPad4", "4"}, 
            {"NumPad5", "5"}, {"NumPad6", "6"}, 
            {"NumPad7", "7"}, {"NumPad8", "8"}, 
            {"NumPad9", "9"}, {"NumPad0", "0"}, 
            {"Space", " "}, 
            {"A", "a"}, {"B", "b"}, 
            {"C", "c"}, {"D", "d"}, 
            {"E", "e"}, {"F", "f"},
            {"G", "g"}, {"H", "h"}, 
            {"I", "i"}, {"J", "j"}, 
            {"K", "k"}, {"L", "l"}, 
            {"M", "m"}, {"N", "n"}, 
            {"O", "o"}, {"P", "p"}, 
            {"Q", "q"}, {"R", "r"}, 
            {"S", "s"}, {"T", "t"}, 
            {"U", "u"}, {"V", "v"}, 
            {"W", "w"}, {"X", "x"},
            {"Y", "y"}, {"Z", "z"}
        };

        Keys[] keymap;
        List<string> entryStrings = new List<string>();
        List<string> displayStrings = new List<string>();
        int currentIndex = 0;
        int maxLength = 15;
        KeyboardState ks, last_ks;

        int counter;
        bool cursorsOn = true;

        bool uppercase = false;

        bool gotoNextScreen = false;
        Color loginColor, skipColor;

        MouseState ms, last_ms;

        Background bkgrnd;

        public LoginScreen()
        {
            loaded = false;
        }

        public override void Load()
        {
            Game1.gammgr.playerShip.visible = true;
            entryStrings.Add(""); //index 0: Game username

            displayStrings.Add(""); //index 0: Game username
            currentIndex = 0;

            loginColor = Color.SlateGray;
            skipColor = Color.SlateGray;

            bkgrnd = new Background(new Vector4(0.3f, 0.1f, 0.4f, 0.25f), new Vector4(0.6f, 0.2f, 0.8f, 0.25f), false);

            loaded = true;

        }

        public override bool IsLoaded()
        {
            return loaded;
        }

        public override void Update(GameTime gameTime)
        {
            bkgrnd.Update();
            ks = Keyboard.GetState();
            ms = Mouse.GetState();

            Game1.gammgr.playerShip.update(new Vector2(ms.X / Game1.scrmgr.screenSize.X, ms.Y / Game1.scrmgr.screenSize.Y), true);

            if (ks.IsKeyDown(Keys.Enter) &&
                last_ks.IsKeyUp(Keys.Enter))
            {
                switch (currentIndex)
                {
                    case 0:
                        Game1.sndmgr.playSound(SFX.menuSelect);
                        ProcessGameLogin();
                        break;
                }
            }

            if (ks.IsKeyDown(Keys.Tab) &&
                last_ks.IsKeyUp(Keys.Tab))
            {
                switch (currentIndex)
                {
                    case 1:
                        currentIndex = 2;
                        break;
                    case 2:
                        currentIndex = 1;
                        break;
                }
            }

            #region typing code
            if (ks.IsKeyDown(Keys.Back) &&
                last_ks.IsKeyUp(Keys.Back) &&
                entryStrings[currentIndex].Length > 0)
            {
                entryStrings[currentIndex] = entryStrings[currentIndex].Substring(0, entryStrings[currentIndex].Length - 1);
            }


            if (entryStrings[currentIndex].Length < maxLength)
            {
                keymap = (Keys[])ks.GetPressedKeys();
                bool shiftDown = false;
                foreach (Keys k in keymap)
                {
                    // 47 keys stored in KeyConvert[,]
                    for (int I = 0; I < 47; I++)
                    {
                        if (k == Keys.RightShift ||
                            k == Keys.LeftShift)
                        {
                            shiftDown = true;
                            uppercase = true;
                        }
                        if (k.ToString() == KeyConvert[I, 0] &&
                            last_ks.IsKeyUp(k))
                        {

                            if (uppercase)
                            {
                                entryStrings[currentIndex] += KeyConvert[I, 1].ToUpper();
                            }
                            else
                            {
                                entryStrings[currentIndex] += KeyConvert[I, 1];
                            }

                            Game1.sndmgr.playSound(SFX.menuMove);
                            break;
                        }
                    }
                }

                if (shiftDown == false)
                {
                    uppercase = false;
                }
            }

            last_ks = ks;
            #endregion

            for (int i = 0; i < displayStrings.Count; i++)
            {
                displayStrings[i] = entryStrings[i];
                if (entryStrings[i] == "" && i != currentIndex)
                {
                    displayStrings[i] = "<>";
                }
                if (i == 2 && entryStrings[2].Length != 0)
                {
                    displayStrings[i] = "";
                    for (int o = 0; o < entryStrings[i].Length; o++)
                    {
                        displayStrings[i] += "*";
                    }
                }
            }

            counter++;
            if (counter == 8)
            {
                counter = 0;
                if (cursorsOn)
                {
                    displayStrings[currentIndex] = ">" + displayStrings[currentIndex] + "<";
                    cursorsOn = false;
                }
                else
                {
                    displayStrings[currentIndex] = displayStrings[currentIndex];
                    cursorsOn = true;
                }
            }

            if (ms.LeftButton == ButtonState.Pressed && last_ms.LeftButton == ButtonState.Released)
            {
                if (Game1.gammgr.playerShip.position.X >= (0.9f - Game1.consoleFont.MeasureString(Strings.Login).X / 2) &&
                    Game1.gammgr.playerShip.position.X <= (0.9f + Game1.consoleFont.MeasureString(Strings.Login).X / 2) &&
                    Game1.gammgr.playerShip.position.Y >= 0.875f)
                {
                    ProcessGameLogin();
                    Game1.sndmgr.playSound(SFX.menuSelect);
                }

            }

            if (Game1.gammgr.playerShip.position.X >= (0.9f - Game1.consoleFont.MeasureString(Strings.Login).X / 2) &&
                    Game1.gammgr.playerShip.position.X <= (0.9f + Game1.consoleFont.MeasureString(Strings.Login).X / 2) &&
                    Game1.gammgr.playerShip.position.Y >= 0.825f &&
                    Game1.gammgr.playerShip.position.Y <= 0.875f)
            {
                loginColor = Color.White;
                skipColor = Color.SlateGray;
            }
            else if (Game1.gammgr.playerShip.position.X >= (0.9f - Game1.consoleFont.MeasureString(Strings.Login).X / 2) &&
                    Game1.gammgr.playerShip.position.X <= (0.9f + Game1.consoleFont.MeasureString(Strings.Login).X / 2) &&
                    Game1.gammgr.playerShip.position.Y >= 0.875f)
            {
                loginColor = Color.SlateGray;
                skipColor = Color.White;
            }
            else
            {
                loginColor = Color.SlateGray;
                skipColor = Color.SlateGray;
            }

            if (gotoNextScreen)
            {
#if WINDOWS
                Game1.scrmgr.changeScreen(new MainMenuScreen());
#elif XBOX
                Game1.scrmgr.changeScreen(new XBMainMenuScreen());
#endif
            }
        }

        public void ProcessGameLogin()
        {
            if (entryStrings[0] == "")
            {
                MessageManager.newMessage(Strings.LoginEmpty);
            }
            else
            {
                Game1.gammgr.saveGameData = SaveGameManager.LoadGame(entryStrings[0]);
                Game1.gammgr.currentPlayer = entryStrings[0];
                Game1.gammgr.loggedIn = true;
                SettingsManager.settings.lastPlayer = entryStrings[0];
                SettingsManager.SaveSettings();
                gotoNextScreen = true;
            }
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            bkgrnd.Draw(gametime, spriteBatch);
            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.PlayerName, new Vector2(0.5f, 0.2f), justification.centre);

            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, displayStrings[0], new Vector2(0.5f, 0.25f), justification.centre);

            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Login, new Vector2(0.9f, 0.9f), skipColor, Color.White, justification.right);
        }

    }
}
