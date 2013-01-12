using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

namespace SHMUP
{
    class MessageManager
    {
        private static List<string> messages = new List<string>();
        private static List<Vector2> msgPosition = new List<Vector2>();
        private static List<int> msgMode = new List<int>();
        private static List<Color> msgColor = new List<Color>();

        public static void newMessage(String s)
        {
            if (messages.Count > 0 && s == messages[messages.Count - 1])
            {
            }
            else
            {
                bool messageAlreadyShowing = false;
                for (int i = 0; i < messages.Count; i++)
                {
                    if (String.Equals(s, messages[i]))
                        messageAlreadyShowing = true;
                }

                if (!messageAlreadyShowing)
                {
                messages.Add(s);
                msgPosition.Add(new Vector2(0.5f, 1.2f));
                msgMode.Add(0);
                msgColor.Add(new Color(0.75f, 1, 0.75f, 0));
                }
            }
        }

        public static void Update()
        {
            for (int i = 0; i < messages.Count; i++)
            {
                msgMode[i]++;
                if (msgMode[i] < 30)
                {
                    msgColor[i] = new Color(0.75f, 1, 0.75f, ((float)msgMode[i] / 30f));
                    msgPosition[i] = Vector2.Lerp(new Vector2(0.5f, 1.2f), new Vector2(0.5f, 0.9f - (0.018f * i)), ((float)msgMode[i] / 30f));
                }
                else if (msgMode[i] > 200 && msgMode[i] < 250)
                {
                    msgColor[i] = new Color(0.75f, 1, 0.75f, 1 - ((float)(msgMode[i] - 200f) / 50f));
                }
                else if (msgMode[i] > 250)
                {
                    messages.RemoveAt(i);
                    msgPosition.RemoveAt(i);
                    msgMode.RemoveAt(i);
                    msgColor.RemoveAt(i);
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, messages[i], msgPosition[i], msgColor[i], 0.75f, justification.centre);
            }
        }
    }
}
