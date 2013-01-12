using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossMaker
{
    public class Background
    {
        List<Shape> shapeList = new List<Shape>();
        List<Shape> unusedShapeList = new List<Shape>();

        Random r;

        public Color topColor;
        public Color botColor;

        //utility variables
        int i;
        Shape s;

        float rot = 0;

        public Background(Vector4 inTopColor, Vector4 inBotColor)
        {
            r = new Random();

            topColor = new Color(inTopColor);
            botColor = new Color(inBotColor);

            reset();
        }


        public void reset()
        {
            Shape s;
            for (i = 0; i < 200; i++)
            {
                if (unusedShapeList.Count == 0)
                {
                    s = new Shape(Vector2.Zero, 0, 0, r);
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
                s = new Shape(Vector2.Zero, 0, 0, r);
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

        public void Update(GameTime gameTime)
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
            for (i = 0; i < shapeList.Count; i++)
            {
                Game1.scrmgr.drawTexture(spriteBatch, Shape.CircleFade, shapeList[i].position, shapeList[i].color, shapeList[i].size, shapeList[i].rotation);
            }

         }
    }

    public class Shape
    {
        public Vector2 position;
        public float size;
        public float rotation;
        public Color color;

        public int texNum;

        public bool live = false;

        public static Texture2D CircleFade;

        public Shape(Vector2 inPos, float inSize, float inRot, Random r)
        {
            position = inPos;
            size = inSize;
            rotation = inRot;

            //texNum = r.Next(0, 3);
            texNum = 0;
        }

        public void update()
        {
            position.X -= (1 / size) / (300 - (size * 10));
            //position.Y += (float)Math.Sin((double)size + (double)position.X) / 3000;
            rotation += (1 / size) / 150;
        }
    }
}
