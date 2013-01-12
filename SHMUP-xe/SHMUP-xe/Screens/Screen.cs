using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Screens
{
    public abstract class Screen
    {
        public abstract void Load();
        public abstract bool IsLoaded();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gametime, SpriteBatch spriteBatch);
    }
}