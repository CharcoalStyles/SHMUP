using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
#if XBOX
using Microsoft.Xna.Framework.GamerServices;
#endif


namespace SHMUP.Screens
{
    public class SplashScreen:Screen
    {
        
        bool loaded;

        float counter = 0;

        Texture2D logo1;

        Texture2D blackground;

#if XBOX
        bool storageChosen = false;
        bool GameSaveRequested = false;

        IAsyncResult result;
#endif
       
        public SplashScreen()
        {
            loaded = false;
        }

        public override void Load()
        {
            logo1 = Game1.content.Load<Texture2D>("Logos/CSLogo");


            blackground = new Texture2D(Game1.graphics.GraphicsDevice, 32, 32, false, SurfaceFormat.Color);

            Color[] pixels = new Color[blackground.Width * blackground.Height];

            for (int y = 0; y < blackground.Height; y++)
            {
                for (int x = 0; x < blackground.Width; x++)
                {
                    pixels[y * blackground.Width + x] = Color.White;
                }
            }

            blackground.SetData<Color>(pixels);

            //Game1.icc.RequestSessionStart(null);

            loaded = true;
        }

        public override bool IsLoaded()
        {
            return loaded;
        }

        public override void Update(GameTime gameTime)
        {
            //Main Menu Input stuff;
            if (loaded)
            {
                counter += gameTime.ElapsedGameTime.Milliseconds;
                if (counter < 4000)
                {
                    if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    {
                        counter += 100;
                    }
                    for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
                    {
                        if (GamePad.GetState(index).Buttons.A == ButtonState.Pressed ||
                            GamePad.GetState(index).Buttons.B == ButtonState.Pressed ||
                            GamePad.GetState(index).Buttons.Back == ButtonState.Pressed ||
                            GamePad.GetState(index).Buttons.LeftShoulder == ButtonState.Pressed ||
                            GamePad.GetState(index).Buttons.LeftStick == ButtonState.Pressed ||
                            GamePad.GetState(index).Buttons.RightShoulder == ButtonState.Pressed ||
                            GamePad.GetState(index).Buttons.RightStick == ButtonState.Pressed ||
                            GamePad.GetState(index).Buttons.Start == ButtonState.Pressed ||
                            GamePad.GetState(index).Buttons.X == ButtonState.Pressed ||
                            GamePad.GetState(index).Buttons.Y == ButtonState.Pressed
                            )
                        {
                            counter += 100;
                        }
                    }
                }
                else
                {
#if WINDOWS
                    /*if (SettingsManager.settings.lastPlayer != "")
                    {
                        Game1.scrmgr.changeScreen(new MainMenuScreen());

                        Game1.gammgr.currentPlayer = SettingsManager.settings.lastPlayer;
                        Game1.gammgr.saveGameData = SaveGameManager.LoadGame(Game1.gammgr.currentPlayer);
                        Game1.gammgr.loggedIn = true;
                    }
                    else
                    {*/
                        Game1.scrmgr.changeScreen(new LoginScreen());
                    //}

#elif XBOX
                    if (storageChosen)
                    {
                        Game1.gammgr.currentPlayer = Microsoft.Xna.Framework.GamerServices.Gamer.SignedInGamers[0].Gamertag;
                        Game1.gammgr.saveGameData = SaveGameManager.LoadGame(Game1.gammgr.currentPlayer);
                        Game1.gammgr.loggedIn = true;
                        Game1.scrmgr.changeScreen(new XBMainMenuScreen());
                    }
                    else
                    {
                       // Set the request flag
                        if ((!Guide.IsVisible) && (GameSaveRequested == false))
                        {
                            GameSaveRequested = true;
                            result = Guide.BeginShowStorageDeviceSelector(null,null);
                        }

                        if ((GameSaveRequested) && (result.IsCompleted))
                        {
                            StorageDevice device = Guide.EndShowStorageDeviceSelector(result);
                            if (device.IsConnected)
                            {
                                storageChosen = true;
                                HighScoreManager.container = device.OpenContainer("SHMUP");
                                SettingsManager.LoadSettings();
                            }
                            // Reset the request flag
                            GameSaveRequested = false;
                        }

                    }
#endif
                }
            }
            else
            {
                throw new Exception("Screen Not Loaded");
            }
        }


        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(blackground, new Rectangle(0, 0, 2000, 2000), Color.White);
            Game1.scrmgr.drawTexture(spriteBatch, logo1, new Vector2(0.5f, 0.5f), Color.White, 1f, 0); 
           
        }
    }
}
