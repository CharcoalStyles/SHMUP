using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace BossMaker
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public static SpriteFont debugFont;
        public static ScreenManager scrmgr;
        SpriteBatch spriteBatch;
        Background b;
        Random r = new Random();

        SaveFileDialog sfd = new SaveFileDialog();

        //cursor stuff
        MouseState ms;
        MouseState lms;
        public Vector2 position;
        public float tempX;
        public float tempY;
        float drawScale;
        float drawScaleAlt;
        int currentBlock = 0;

        Vector2 tempPos1;
        Vector2 tempPos2;
        bool onMiddle;

        Color k = new Color(0, 0, 0, 0.5f);
        Color s = new Color(1, 1, 1, 0.5f);
        Color c = new Color(1, 0, 0, 0.5f);

        Color menuNewColor = Color.White;
        Color menuSaveColor = Color.White;
        Color menuShotsColor = Color.White;
        Color menuMissileColor = Color.White;

        string LMB = "";
        string RMB = "";
        
        //BossData
        List<Vector2> blockPosition = new List<Vector2>();
        List<int> blockType = new List<int>();
        int coreShotsVolley = 1;
        int coreMissilesVolley = 0;
        float diff = 0;

        List<Color> blockColor = new List<Color>();//Just for hte editor
        List<bool> lastAddOnMiddle = new List<bool>();//for undos

        List<Texture2D> bossTextures = new List<Texture2D>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            scrmgr = new ScreenManager();
            b = new Background(new Vector4((float)r.NextDouble(),
                (float)r.NextDouble(),
                (float)r.NextDouble(),
                0.25f),
                new Vector4((float)r.NextDouble(),
                (float)r.NextDouble(),
                (float)r.NextDouble(),
                0.25f));

            resetBoss();
            sfd.Filter = "SHMUP Boss File (*.bos)|*.bos";
        }

        public void resetBoss()
        {
            blockColor.Clear();
            blockPosition.Clear();
            blockType.Clear();
            lastAddOnMiddle.Clear();

            blockColor.Add(Color.Tomato);
            blockPosition.Add(new Vector2(0.6f, 0.5f));
            blockType.Add(1);

            coreShotsVolley = 1;
            coreMissilesVolley = 0;
            diff = 0;
        }

        public void SaveBoss()
        {
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SHMUP.Enemies.BossManager.BossData bd = new SHMUP.Enemies.BossManager.BossData();
                bd.blockOffset = new List<Vector2>();
                bd.blockType = new List<int>();

                for (int i = 1; i < blockPosition.Count; i++)
                {
                    bd.blockOffset.Add(blockPosition[i] - blockPosition[0]);
                    bd.blockType.Add(blockType[i]);
                }

                // Open the file, creating it if necessary
                FileStream stream = File.Open(sfd.FileName, FileMode.Create);
                try
                {
                    // Convert the object to XML data and put it in the stream
                    XmlSerializer serializer = new XmlSerializer(typeof(SHMUP.Enemies.BossManager.BossData));
                    serializer.Serialize(stream, bd);
                }
                finally
                {
                    // Close the file
                    stream.Close();
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bossTextures.Add(Content.Load<Texture2D>("Enemy/Solid3"));
            bossTextures.Add(Content.Load<Texture2D>("Enemy/Solid4"));
            bossTextures.Add(Content.Load<Texture2D>("Enemy/SoftStar3"));
            bossTextures.Add(Content.Load<Texture2D>("Enemy/SoftStar4"));
            bossTextures.Add(Content.Load<Texture2D>("Enemy/SoftStar5"));
            bossTextures.Add(Content.Load<Texture2D>("Enemy/SoftStar6"));
            bossTextures.Add(Content.Load<Texture2D>("Enemy/HardStar3"));
            bossTextures.Add(Content.Load<Texture2D>("Enemy/HardStar4"));
            bossTextures.Add(Content.Load<Texture2D>("Enemy/HardStar5"));
            bossTextures.Add(Content.Load<Texture2D>("Enemy/HardStar6"));
            bossTextures.Add(Content.Load<Texture2D>("Enemy/Solid5"));
            bossTextures.Add(Content.Load<Texture2D>("Enemy/Solid6"));

            Shape.CircleFade = Content.Load<Texture2D>("Background/CircleF");

            debugFont = Content.Load<SpriteFont>("SpriteFont1");
        }


        protected override void Update(GameTime gameTime)
        {
            b.Update(gameTime);
            lms = ms;
            ms = Mouse.GetState();

            if (Keyboard.GetState().IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Z))
            {
            tempX = ms.X / scrmgr.screenSize.X;
            }

            if (Keyboard.GetState().IsKeyUp(Microsoft.Xna.Framework.Input.Keys.X))
            {
                tempY = ms.Y / scrmgr.screenSize.Y;
            }

            position = new Vector2(tempX, tempY);


            #region mousewheel logic
            if (lms.ScrollWheelValue > ms.ScrollWheelValue)
            {
                currentBlock--;
                if (currentBlock == -1)
                    currentBlock = bossTextures.Count - 1;
            }
            else if (lms.ScrollWheelValue < ms.ScrollWheelValue)
            {
                currentBlock++;
                if (currentBlock == bossTextures.Count)
                    currentBlock = 0;
            }
            #endregion

            if (position.Y < 0.475f)
            {
                onMiddle = false;
                tempPos1 = position;
                tempPos2 = new Vector2(position.X, 1 - position.Y);
            }
            else if (position.Y > 0.525f)
            {
                onMiddle = false;
                tempPos1 = new Vector2(position.X, 1 - position.Y);
                tempPos2 = position;
            }
            else
            {
                onMiddle = true;
                tempPos1 =
                tempPos1 = new Vector2(position.X, 0.5f);
            }

            #region editor mouse buttons
            if (lms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (position.X > 0.1f)
                {
                    blockPosition.Add(tempPos1);
                    blockType.Add(currentBlock);
                    blockColor.Add(new Color(Vector3.One - Vector3.Lerp(b.topColor.ToVector3(), b.botColor.ToVector3(), tempPos1.Y)));
                    if (!onMiddle)
                    {
                        blockPosition.Add(tempPos2);
                        blockType.Add(currentBlock);
                        blockColor.Add(new Color(Vector3.One - Vector3.Lerp(b.topColor.ToVector3(), b.botColor.ToVector3(), tempPos2.Y)));
                        lastAddOnMiddle.Add(false);
                    }
                    else
                    {
                        lastAddOnMiddle.Add(true);
                    }
                }

                int mod;
                if (onMiddle)
                    mod = 1;
                else
                    mod = 2;

                switch (currentBlock)
                {
                    case 0:
                        //Solid3
                        diff += 0.1f * mod;
                        break;
                    case 1:
                        //Solid4
                        diff += 0.15f * mod;
                        break;
                    case 2:
                        //SoftStar3
                        diff += 0.2f * mod;
                        break;
                    case 3:
                        //SoftStar4
                        diff += 0.3f * mod;
                        break;
                    case 4:
                        //SoftStar5
                        diff += 0.4f * mod;
                        break;
                    case 5:
                        //SoftStar6
                        diff += 0.45f * mod;
                        break;
                    case 6:
                        //HardStar3
                        diff += 0.35f * mod;
                        break;
                    case 7:
                        //HardStar4
                        diff += 0.45f * mod;
                        break;
                    case 8:
                        //HardStar5
                        diff += 0.6f * mod;
                        break;
                    case 9:
                        //HardStar6
                        diff += 0.8f * mod;
                        break;
                }
            }

            if (lms.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                ms.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (lastAddOnMiddle.Count != 0)
                {
                    bool lam = lastAddOnMiddle[lastAddOnMiddle.Count - 1];
                    int remBlock = blockType[blockType.Count - 1];
                    if (lastAddOnMiddle[lastAddOnMiddle.Count - 1])
                    {
                        blockPosition.RemoveAt(blockPosition.Count - 1);
                        blockType.RemoveAt(blockType.Count - 1);
                        blockColor.RemoveAt(blockColor.Count - 1);
                    }
                    else
                    {
                        blockPosition.RemoveAt(blockPosition.Count - 1);
                        blockType.RemoveAt(blockType.Count - 1);
                        blockColor.RemoveAt(blockColor.Count - 1);
                        blockPosition.RemoveAt(blockPosition.Count - 1);
                        blockType.RemoveAt(blockType.Count - 1);
                        blockColor.RemoveAt(blockColor.Count - 1);
                    }
                    lastAddOnMiddle.RemoveAt(lastAddOnMiddle.Count - 1);


                    int mod;
                    if (lam)
                        mod = 1;
                    else
                        mod = 2;

                    switch (remBlock)
                    {
                        case 0:
                            //Solid3
                            diff -= 0.1f * mod;
                            break;
                        case 1:
                            //Solid4
                            diff -= 0.15f * mod;
                            break;
                        case 2:
                            //SoftStar3
                            diff -= 0.2f * mod;
                            break;
                        case 3:
                            //SoftStar4
                            diff -= 0.3f * mod;
                            break;
                        case 4:
                            //SoftStar5
                            diff -= 0.4f * mod;
                            break;
                        case 5:
                            //SoftStar6
                            diff -= 0.45f * mod;
                            break;
                        case 6:
                            //HardStar3
                            diff -= 0.35f * mod;
                            break;
                        case 7:
                            //HardStar4
                            diff -= 0.45f * mod;
                            break;
                        case 8:
                            //HardStar5
                            diff -= 0.6f * mod;
                            break;
                        case 9:
                            //HardStar6
                            diff -= 0.8f * mod;
                            break;
                    }
                }
            }
            #endregion

            #region menu handling
            if (position.X < 0.1f)
            {
                //new
                if (position.Y < 0.1f)
                {
                    menuNewColor = Color.Green;
                    LMB = "Start New Boss";
                    RMB = "";
                    if (lms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                        ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        resetBoss();
                    }
                }
                else
                    menuNewColor = Color.White;

                //Save
                if (position.Y > 0.1f && position.Y < 0.2f)
                {
                    LMB = "Save Boss";
                    RMB = "";
                    menuSaveColor = Color.Green;
                    if (lms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                        ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        SaveBoss();
                    }
                }
                else
                    menuSaveColor = Color.White;

                if (position.Y > 0.3f && position.Y < 0.4f)
                {
                    LMB = "Add to Bullets fired (nax: 21)";
                    RMB = "Remove from bullets fired (min: 1)";
                    menuShotsColor = Color.Green;

                    if (lms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                        ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (coreShotsVolley < 20)
                        {
                            coreShotsVolley += 2;
                            diff += 0.25f;
                        }
                    }

                    if (lms.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                        ms.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (coreShotsVolley > 2)
                        {
                            coreShotsVolley -= 2;
                            diff -= 0.25f;
                        }
                    }
                }
                else
                    menuShotsColor = Color.White;

                if (position.Y > 0.4f && position.Y < 0.5f)
                {
                    LMB = "Add to Missiles fired (nax: 20)";
                    RMB = "Remove from Missiles fired (min: 0)";
                    menuMissileColor = Color.Green;

                    if (lms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                        ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (coreMissilesVolley < 20)
                        {
                            coreMissilesVolley += 2;
                            diff += 0.5f;
                        }
                    }

                    if (lms.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                        ms.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (coreMissilesVolley > 0)
                        {
                            coreMissilesVolley -= 2;
                            diff -= 0.5f;
                        }
                    }
                }
                else
                    menuMissileColor = Color.White;
            }
            else
            {
                LMB = "Place blocks";
                RMB = "Undo";
                menuNewColor = Color.White;
                menuSaveColor = Color.White;
                menuShotsColor = Color.White;
                menuMissileColor = Color.White;
            }
            #endregion

            drawScale = (float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 300) * 0.1f;
            drawScaleAlt = (float)Math.Cos(gameTime.TotalGameTime.TotalMilliseconds / 300) * 0.1f;

            blockColor[0] = new Color(((float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 300) + 1) / 2,
                ((float)Math.Cos(gameTime.TotalGameTime.TotalMilliseconds / 1000) + 1) / 2, 
                ((float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 1000) + 1) / 2);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            b.Draw(gameTime, spriteBatch);

            #region draw saved blocks
            for (int i = 0; i < blockPosition.Count; i++)
            {
                Game1.scrmgr.drawTexture(spriteBatch, bossTextures[blockType[i]], blockPosition[i], Color.Black, 0.5f, ((float)gameTime.TotalGameTime.TotalMilliseconds / 200) * ((2 * blockPosition[i].Y) - 1));
             }

            for (int i = 0; i < blockPosition.Count; i++)
            {
                Game1.scrmgr.drawTexture(spriteBatch, bossTextures[blockType[i]], blockPosition[i], Color.White, 0.5f - 0.05f, ((float)gameTime.TotalGameTime.TotalMilliseconds / 200) * ((2 * blockPosition[i].Y) - 1));
            }
                
            for (int i = 0; i < blockPosition.Count; i++)
            {
                Game1.scrmgr.drawTexture(spriteBatch, bossTextures[blockType[i]], blockPosition[i], blockColor[i], 0.5f - 0.175f, ((float)gameTime.TotalGameTime.TotalMilliseconds / 200) * ((2 * blockPosition[i].Y) - 1));
            }
            #endregion

            #region draw new blocks
            if (position.X > 0.1f)
            {
                if (onMiddle)
                {
                    Game1.scrmgr.drawTexture(spriteBatch, bossTextures[currentBlock], tempPos1, k, 0.5f, 0);
                    Game1.scrmgr.drawTexture(spriteBatch, bossTextures[currentBlock], tempPos1, s, 0.5f - 0.05f, 0);
                    Game1.scrmgr.drawTexture(spriteBatch, bossTextures[currentBlock], tempPos1, c, 0.5f - 0.175f, 0);
                }
                else
                {
                    Game1.scrmgr.drawTexture(spriteBatch, bossTextures[currentBlock], tempPos1, k, 0.5f, 0);
                    Game1.scrmgr.drawTexture(spriteBatch, bossTextures[currentBlock], tempPos2, k, 0.5f, 0);
                    Game1.scrmgr.drawTexture(spriteBatch, bossTextures[currentBlock], tempPos1, s, 0.5f - 0.05f, 0);
                    Game1.scrmgr.drawTexture(spriteBatch, bossTextures[currentBlock], tempPos2, s, 0.5f - 0.05f, 0);
                    Game1.scrmgr.drawTexture(spriteBatch, bossTextures[currentBlock], tempPos1, c, 0.5f - 0.175f, 0);
                    Game1.scrmgr.drawTexture(spriteBatch, bossTextures[currentBlock], tempPos2, c, 0.5f - 0.175f, 0);
                }
            }
            #endregion


            scrmgr.drawString(spriteBatch, debugFont, "Difficulty: " + diff.ToString(), new Vector2(0.6f, 0.1f), justification.centre);

            //Menu
            scrmgr.drawString(spriteBatch, debugFont, "New", new Vector2(0.01f, 0.05f), menuNewColor, Color.Black, justification.left);
            scrmgr.drawString(spriteBatch, debugFont, "Save", new Vector2(0.01f, 0.15f), menuSaveColor, Color.Black, justification.left);

            scrmgr.drawString(spriteBatch, debugFont, "Shots:" + coreShotsVolley.ToString(), new Vector2(0.01f, 0.35f), menuShotsColor, Color.Black, justification.left);
            scrmgr.drawString(spriteBatch, debugFont, "Miss.:" + coreMissilesVolley.ToString(), new Vector2(0.01f, 0.45f), menuMissileColor, Color.Black, justification.left);

            scrmgr.drawString(spriteBatch, debugFont, "LMB - " + LMB, new Vector2(0.01f, 0.93f), justification.left);
            scrmgr.drawString(spriteBatch, debugFont, "RMB - " + RMB, new Vector2(0.01f, 0.95f), justification.left);
            scrmgr.drawString(spriteBatch, debugFont, "Mouse Wheel - change boss element", new Vector2(0.01f, 0.97f), justification.left);

            //Draw Cursor
            Game1.scrmgr.drawTexture(spriteBatch, bossTextures[3], position, Color.White, drawScale, (float)gameTime.TotalGameTime.TotalMilliseconds / 800);
            Game1.scrmgr.drawTexture(spriteBatch, bossTextures[3], position, Color.Black, drawScaleAlt, (float)gameTime.TotalGameTime.TotalMilliseconds / 800);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
