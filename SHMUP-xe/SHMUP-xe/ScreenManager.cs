using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SHMUP
{
    public enum ScreenState
    {
        normal,
        fadeToLoad,
        loadScreen,
        fadeFromLoad
    }
    public class ScreenManager
    {
        public Screens.Screen currentScreen;
        public Screens.Screen holdScreen;

        public Screens.LoadingScreen loadScreen;

        float fullScreenFade = 0;
        Color fadeColor;

        public int loadScreenTimer;

        public ScreenState screenState = ScreenState.normal;

        public Vector2 screenSize;
        Vector2 targetScreenSize = new Vector2(1280, 720);
        float screenSizeScale;

        public ScreenMode screenMode;
        public bool isFullScreen;

        public float menuTopPosition = 0.5f;

        public static readonly int[,] resolutions =
        new int[,] {
        // Normal
        {640, 480},
        {800, 600},
        {1024, 768},
        {1280, 1024},
        {1600, 1200},
        // Widescreen
        {1280, 720},
        {1440, 900},
        {1680, 1050},
        {1920, 1080},
        {1920, 1200},
        //fix for Gayle's laptop
        {1280, 800},
        {1024, 600}
        };

        public Texture2D FSO;
        public Texture2D aLine;

        public ScreenManager()
        {
            holdScreen = null;
            //changeResolution(ScreenMode.WXGA, false);
            changeResolution(ScreenMode.XGA, false);
        }

        public ScreenManager(ScreenMode sm, bool fs)
        {
            holdScreen = null;
            //changeResolution(ScreenMode.WXGA, false);
            changeResolution(sm, fs);
        }

        public void loadFSO()
        {
            FSO = new Texture2D(Game1.graphics.GraphicsDevice, 1280, 720, false, SurfaceFormat.Color);

            Color[] pixels = new Color[FSO.Width * FSO.Height];
            bool biLine = false;
            for (int x = 0; x < FSO.Width; x++)
            {
                for (int y = 0; y < FSO.Height; y++)
                {
                
                    if (biLine)
                    {
                        biLine = false;
                        pixels[y * FSO.Width + x] = Color.Black;
                        pixels[y * FSO.Width + x].A = 64;
                    }
                    else
                    {
                        biLine = true;
                        pixels[y * FSO.Width + x] = Color.Black;
                        pixels[y * FSO.Width + x].A = 32;
                    }
                }
            }

            FSO.SetData<Color>(pixels);

        }
        //public void updateScreenSize(Vector2 newScreenSize)
        //{
        //    screenSize = newScreenSize;
        //    screenSizeScale = ((screenSize / targetScreenSize).X + (screenSize / targetScreenSize).Y) / 2;
        //}

        public void Update(GameTime gameTime)
        {
            switch (screenState)
            {
                case ScreenState.normal:
                    currentScreen.Update(gameTime);
                    break;
                case ScreenState.fadeToLoad:
                    Game1.gammgr.isPlaying = false;
                    fullScreenFade += 0.03f;
                    if (fullScreenFade >= 1)
                    {
                        fullScreenFade = 1;
                        screenState = ScreenState.loadScreen;
                        currentScreen = null;
                        currentScreen = holdScreen;
                        holdScreen = null;
                        GC.Collect();
                        break;
                    }
                    currentScreen.Update(gameTime);
                    loadScreen.Update(gameTime);
                    break;
                case ScreenState.loadScreen:
                    if (loadScreenTimer > 500)
                    {
                        loadScreenTimer = 0;
                        loadScreen.Update(gameTime);
                        GC.Collect();
                    }
                    if (currentScreen.IsLoaded())
                    {
                        currentScreen.Update(gameTime);
                        screenState = ScreenState.fadeFromLoad;
                    }
                    break;
                case ScreenState.fadeFromLoad:
                    fullScreenFade -= 0.03f;
                    if (fullScreenFade <= 0)
                    {
                        fullScreenFade = 0;
                        screenState = ScreenState.normal;
                    }
                    currentScreen.Update(gameTime);
                    loadScreen.Update(gameTime);
                    break;
            }

            fadeColor = new Color(1, 1, 1, fullScreenFade);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (screenState)
            {
                case ScreenState.normal:
                    currentScreen.Draw(gameTime, spriteBatch);
                    break;
                case ScreenState.fadeToLoad:
                    currentScreen.Draw(gameTime, spriteBatch);
                    loadScreen.Draw(gameTime, spriteBatch, fadeColor);
                    break;
                case ScreenState.loadScreen:
                    loadScreenTimer += gameTime.ElapsedGameTime.Milliseconds;
                    if (loadScreenTimer > 500)
                    {
                        loadScreen.Draw(gameTime, spriteBatch, fadeColor);
                    }
                    break;
                case ScreenState.fadeFromLoad:
                    currentScreen.Draw(gameTime, spriteBatch);
                    loadScreen.Draw(gameTime, spriteBatch, fadeColor);
                    break;
            }

             //spriteBatch.DrawString(Game1.debugFont, "NULL", new Vector2(5, 5), Color.WhiteSmoke);
        }

        public void changeScreen(Screens.Screen inScreen)
        {
            holdScreen = inScreen;
            screenState = ScreenState.fadeToLoad;
        }

        public void changeResolution(ScreenMode inSM, bool inFullScreen)
        {
            Game1.graphics.PreferredBackBufferWidth = resolutions[(int)inSM, 0];
            Game1.graphics.PreferredBackBufferHeight = resolutions[(int)inSM, 1];

            Game1.graphics.IsFullScreen = inFullScreen;

            Game1.graphics.ApplyChanges();

            screenMode = inSM;
            isFullScreen = inFullScreen;

            screenSize = new Vector2(resolutions[(int)inSM, 0], resolutions[(int)inSM, 1]);
            screenSizeScale = ((screenSize / targetScreenSize).X + (screenSize / targetScreenSize).Y) / 2;
        }

        Vector2 origin;
        public void drawTexture(SpriteBatch spriteBatch, Texture2D texture2D, Vector2 position)
        {
            origin = new Vector2(texture2D.Width / 2, texture2D.Height / 2);
            spriteBatch.Draw(texture2D, position * screenSize, null, Color.White, 0, origin, screenSizeScale, SpriteEffects.None, 0);
        }

        public void drawTexture(SpriteBatch spriteBatch, Texture2D texture2D, Vector2 position, Color color)
        {
            origin = new Vector2(texture2D.Width / 2, texture2D.Height / 2);
            spriteBatch.Draw(texture2D, position * screenSize, null, color, 0, origin, screenSizeScale, SpriteEffects.None, 0);
        }

        public void drawTexture(SpriteBatch spriteBatch, Texture2D texture2D, Vector2 position, Color color, float scale, float rotation)
        {
            origin = new Vector2(texture2D.Width / 2, texture2D.Height / 2);
            spriteBatch.Draw(texture2D, position * screenSize, null, color, rotation, origin, screenSizeScale * scale, SpriteEffects.None, 0);
        }

        public void drawTexture(SpriteBatch spriteBatch, Texture2D texture2D, Vector2 position, Color color, Vector2 scale, float rotation)
        {
            origin = new Vector2(texture2D.Width / 2, texture2D.Height / 2);
            spriteBatch.Draw(texture2D, position * screenSize, null, color, rotation, origin, scale * screenSizeScale, SpriteEffects.None, 0);
        }

        public void drawString(SpriteBatch spriteBatch, SpriteFont spriteFont, String str, Vector2 position, Color color, justification j)
        {
            string s = str;//.ToUpper();
            origin = spriteFont.MeasureString(s) / 2;

            switch (j)
            {
                case justification.left:
                    origin.X = 0;
                    break;
                case justification.right:
                    origin.X = spriteFont.MeasureString(s).X;
                    break;
            }

            spriteBatch.DrawString(spriteFont, s, position * screenSize + new Vector2(1,1), Color.Black, 0, origin, screenSizeScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, s, position * screenSize, color, 0, origin, screenSizeScale, SpriteEffects.None, 0);
        }

        public void drawString(SpriteBatch spriteBatch, SpriteFont spriteFont, String str, Vector2 position, justification j)
        {
            string s = str;//.ToLower();
            origin = spriteFont.MeasureString(s) / 2;

            switch (j)
            {
                case justification.left:
                    origin.X = 0;
                    break;
                case justification.right:
                    origin.X = spriteFont.MeasureString(s).X;
                    break;
            }

            spriteBatch.DrawString(spriteFont, s, position * screenSize + new Vector2(1, 1), Color.Black, 0, origin, screenSizeScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, s, position * screenSize, Color.White, 0, origin, screenSizeScale, SpriteEffects.None, 0);
        }

        public void drawString(SpriteBatch spriteBatch, SpriteFont spriteFont, String str, Vector2 position, float scale, justification j)
        {
            string s = str;//.ToLower();
            origin = spriteFont.MeasureString(s) / 2;

            switch (j)
            {
                case justification.left:
                    origin.X = 0;
                    break;
                case justification.right:
                    origin.X = spriteFont.MeasureString(s).X;
                    break;
            }

            spriteBatch.DrawString(spriteFont, s, position * screenSize + new Vector2(1, 1), Color.Black, 0, origin, scale * screenSizeScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, s, position * screenSize, Color.White, 0, origin, scale * screenSizeScale, SpriteEffects.None, 0);
        }

        public void drawString(SpriteBatch spriteBatch, SpriteFont spriteFont, String str, Vector2 position, Color color, float scale, justification j)
        {
            string s = str;//.ToLower();
            origin = spriteFont.MeasureString(s) / 2;

            switch (j)
            {
                case justification.left:
                    origin.X = 0;
                    break;
                case justification.right:
                    origin.X = spriteFont.MeasureString(s).X;
                    break;
            }

            spriteBatch.DrawString(spriteFont, s, position * screenSize + new Vector2(1, 1), Color.Black, 0, origin, scale * screenSizeScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, s, position * screenSize, color, 0, origin, scale * screenSizeScale, SpriteEffects.None, 0);
        }

        public void drawString(SpriteBatch spriteBatch, SpriteFont spriteFont, String str, Vector2 position, float scale, float rotation, justification j)
        {
            string s = str;//.ToLower();
            origin = spriteFont.MeasureString(s) / 2;

            switch (j)
            {
                case justification.left:
                    origin.X = 0;
                    break;
                case justification.right:
                    origin.X = spriteFont.MeasureString(s).X;
                    break;
            }

            spriteBatch.DrawString(spriteFont, s, position * screenSize + new Vector2(1, 1), Color.Black, rotation * 1.5f, origin, scale * screenSizeScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, s, position * screenSize, Color.White, rotation, origin, scale * screenSizeScale, SpriteEffects.None, 0);
        }

        public void drawString(SpriteBatch spriteBatch, SpriteFont spriteFont, String str, Vector2 position, Color mainColor, Color shadowColor, justification j)
        {
            string s = str;//.ToLower();
            origin = Game1.debugFont.MeasureString(s) / 2;

            switch (j)
            {
                case justification.left:
                    origin.X = 0;
                    break;
                case justification.right:
                    origin.X = Game1.debugFont.MeasureString(s).X;
                    break;
            }

            spriteBatch.DrawString(spriteFont, s, position * screenSize + new Vector2(1, 1), shadowColor, 0, origin, screenSizeScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, s, position * screenSize, mainColor, 0, origin, screenSizeScale, SpriteEffects.None, 0);
        }

        public void drawString(SpriteBatch spriteBatch, SpriteFont spriteFont, String str, Vector2 position, Color mainColor, Color shadowColor, float scale, justification j)
        {
            string s = str;//.ToLower();
            origin = Game1.debugFont.MeasureString(s) / 2;

            switch (j)
            {
                case justification.left:
                    origin.X = 0;
                    break;
                case justification.right:
                    origin.X = Game1.debugFont.MeasureString(s).X;
                    break;
            }

            spriteBatch.DrawString(spriteFont, s, position * screenSize + new Vector2(1, 1), shadowColor, 0, origin, scale * screenSizeScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, s, position * screenSize, mainColor, 0, origin, scale * screenSizeScale, SpriteEffects.None, 0);
        }

        public void drawLine(SpriteBatch spriteBatch, Vector2 startPosition, Vector2 endPosition)
        {
            Rectangle r = new Rectangle((int)(startPosition.X * screenSize.X),
                                        (int)(startPosition.Y * screenSize.Y),
                                        (int)((endPosition.X - startPosition.X) * screenSize.X),
                                        (int)((endPosition.Y - startPosition.Y) * screenSize.Y));
            spriteBatch.Draw(aLine, r, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
    public enum justification
    {
        left,
        centre,
        right
    }
    public enum ScreenMode
    {
        /// <summary>
        /// 640x480
        /// </summary>
        VGA,
        /// <summary>
        /// 800x600
        /// </summary>
        SVGA,
        /// <summary>
        /// 1024x768
        /// </summary>
        XGA,
        /// <summary>
        /// 1280x1024
        /// </summary>
        SXGA,
        /// <summary>
        /// 1600x1200
        /// </summary>
        HUXGA,
        /// <summary>
        /// 1280,720
        /// </summary>
        WXGA,
        /// <summary>
        /// 1440x900
        /// </summary>
        WSXGA,
        /// <summary>
        /// 1680x1050
        /// </summary>
        tv1080p,
        /// <summary>
        /// 1920x1080
        /// </summary>
        WSXGAplus,
        /// <summary>
        /// 1920x1200
        /// </summary>
        lappy,
        /// <summary>
        /// 1024x600
        /// </summary>
        Netbook
        /// <summary>
        /// 1024x600
        /// </summary>
    }

}