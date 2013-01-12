using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using System.IO;

namespace SHMUP
{
    public class CrashDebugGame : Game
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private readonly Exception exception;

        public CrashDebugGame(Exception exception)
        {
            this.exception = exception;
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("SpriteFont1");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Initialize()
        {
            base.Initialize();
#if WINDOWS
            string fullpath;
            fullpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString(), "CrashLog.txt");
            TextWriter stream = new StreamWriter(fullpath);
            stream.Write(
               "-=Crash Log=-" + Environment.NewLine +
               string.Format("Exception: {0}", exception.Message) + Environment.NewLine +
               font, string.Format("Stack Trace:\n{0}", exception.StackTrace));
            stream.Close();
#endif
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.DrawString(
               font,
               "We're sorry, but S.H.M.U.P has crashed.",
               new Vector2(10f, 10f),
               Color.White);
            spriteBatch.DrawString(
               font,
               "The game has created a file called CrashLog.txt in your My Documents.",
               new Vector2(10f, 35f),
               Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
