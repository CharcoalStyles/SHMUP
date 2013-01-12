using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BossMaker
{
    public class ScreenManager
    {
        public Vector2 screenSize;
        Vector2 targetScreenSize = new Vector2(1280, 720);
        float screenSizeScale;

        public ScreenMode screenMode;
        public bool isFullScreen;

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
        {1920, 1200}
        };

        public ScreenManager()
        {
            changeResolution(ScreenMode.WXGA, false);
        }

        public void changeResolution(ScreenMode inSM, bool inFullScreen)
        {
            Game1.graphics.PreferredBackBufferWidth = resolutions[(int)inSM,0];
            Game1.graphics.PreferredBackBufferHeight = resolutions[(int)inSM, 1];

            Game1.graphics.IsFullScreen = inFullScreen;

            Game1.graphics.ApplyChanges();

            screenMode = inSM;
            isFullScreen = inFullScreen;

            screenSize = new Vector2(resolutions[(int)inSM,0],resolutions[(int)inSM,1]);
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

            spriteBatch.DrawString(spriteFont, s, position * screenSize, color, 0, origin, screenSizeScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, s, position * screenSize, Color.Black, 0, origin, screenSizeScale, SpriteEffects.None, 0);
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
    }

}