using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP
{
    public class Background
    {
        Texture2D blackground;

        List<Shape> shapeList = new List<Shape>();
        List<Shape> unusedShapeList = new List<Shape>();

        Random r;

        public Color topColor;
        public Color botColor;

        private bool isHardBackground;

        //utility variables
        int i;
        Shape s;

        float rot = 0;

        public Background(Vector4 inColor, bool hardBackground)
        {
            r = new Random();

            topColor = new Color(inColor);
            botColor = topColor;

            isHardBackground = hardBackground;

            blackground = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);

            Color[] pixels = new Color[blackground.Width * blackground.Height];

            for (int y = 0; y < blackground.Height; y++)
            {
                for (int x = 0; x < blackground.Width; x++)
                {
                    pixels[y * blackground.Width + x] = topColor;
                }
            }

            blackground.SetData<Color>(pixels);

            reset();
        }

        public Background(Vector4 inTopColor, Vector4 inBotColor, bool hardBackground)
        {
            r = new Random();

            topColor = new Color(inTopColor);
            botColor = new Color(inBotColor);

            isHardBackground = hardBackground;

            blackground = new Texture2D(Game1.graphics.GraphicsDevice, 1, 128, false, SurfaceFormat.Color);

            Color[] pixels = new Color[blackground.Width * blackground.Height];

            for (int y = 0; y < blackground.Height; y++)
            {
                for (int x = 0; x < blackground.Width; x++)
                {
                    pixels[y * blackground.Width + x] = new Color(Vector4.Lerp(inTopColor, inBotColor, (float)((float)y / (float)blackground.Height)));
                }
            }

            blackground.SetData<Color>(pixels);

            reset();
        }


        public void reset()
        {
            Shape s;
            for (i = 0; i < 30; i++)
            {
                if (unusedShapeList.Count == 0)
                {
                    s = new Shape(Vector2.Zero, 0, 0, r, isHardBackground);
                }
                else
                {
                    s = unusedShapeList[0];
                    unusedShapeList.Remove(s);
                }

                s.position = new Vector2((float)r.NextDouble() * 3 - 1, (float)r.NextDouble());

                s.size = ((float)r.NextDouble() * 4) + 1;

                s.rotation = 0;

                s.color = new Color(Vector4.Lerp(topColor.ToVector4(), botColor.ToVector4(), s.position.Y));
                //s.color.A = (byte)(0.2f - (s.size / 10));

                s.live = true;

                shapeList.Add(s);
            }
        }

        public void addShape()
        {
            if (unusedShapeList.Count == 0)
            {
                s = new Shape(Vector2.Zero, 0, 0, r, isHardBackground);
            }
            else
            {
                s = unusedShapeList[0];
                unusedShapeList.Remove(s);
            }

            s.position = new Vector2((float)r.NextDouble() * 2.5f + 1, (float)r.NextDouble());

            s.size = ((float)r.NextDouble() * 4) + 1;

            s.rotation = 0;

            s.color = new Color(Vector4.Lerp(topColor.ToVector4(), botColor.ToVector4(), s.position.Y));
           // s.color.A = (byte)(0.2f - (s.size / 10));

            s.live = true;

            shapeList.Add(s);
        }

        public void Update()
        {
            for (i = 0; i < shapeList.Count; i++)
            {
                shapeList[i].update();

                if (shapeList[i].position.X < -2f)
                {
                    shapeList[i].live = false;
                    unusedShapeList.Add(shapeList[i]);
                    shapeList.RemoveAt(i);
                    addShape();
                }
            }

            rot += 0.002f;

            //if (Game1.musman.lastvd.Frequencies[0]
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blackground, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.DisplayMode.Width, Game1.graphics.GraphicsDevice.DisplayMode.Height), Color.White);

            for (i = 0; i < shapeList.Count; i++)
            {
                shapeList[i].Draw(gameTime, spriteBatch);
                //Game1.scrmgr.drawString(spriteBatch, shapeList[i].color.ToString(), shapeList[i].position);
            }

            Game1.scrmgr.drawTexture(spriteBatch, Game1.scrmgr.FSO, new Vector2(0.5f, 0.5f), Color.White, 2.5f, rot);

            //spriteBatch.Draw(Game1.scrmgr.FSO, new Rectangle((int)(Game1.graphics.GraphicsDevice.DisplayMode.Width * -0.5), (int)(Game1.graphics.GraphicsDevice.DisplayMode.Height * -0.5)
             //   , (int)(Game1.graphics.GraphicsDevice.DisplayMode.Width * 1.5), (int)(Game1.graphics.GraphicsDevice.DisplayMode.Height)),null, Color.White, 2,);
            

        }
    }

    public class Shape
    {
        public Vector2 position;
        public float size;
        public float rotation;
        public Color color;
        public bool isHard;

        public int texNum;

        public bool live = false;

        public static Texture2D CircleFade;
        public static Texture2D CircleHard;

        public Shape(Vector2 inPos, float inSize, float inRot, Random r,bool hard)
        {
            position = inPos;
            size = inSize;
            rotation = inRot;

            isHard = hard;

            //texNum = r.Next(0, 3);
            texNum = 0;
        }

        public void update()
        {
            position.X -= (1 / size) / (300 - (size * 10));
            //position.Y += (float)Math.Sin((double)size + (double)position.X) / 3000;
            rotation += (1 / size) / 150;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isHard)
                Game1.scrmgr.drawTexture(spriteBatch, Shape.CircleHard, position, color, size, rotation);
            else
                Game1.scrmgr.drawTexture(spriteBatch, Shape.CircleFade, position, color, size, rotation);
        }
    }
}
