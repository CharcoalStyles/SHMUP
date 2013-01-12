using System;
using System.IO;

namespace SHMUP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            
            try
            {
                if (args.Length == 0)
                {
                    string toSHMUPSavePath = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        "My Games"), "SHMUP"), "SaveGames");
                    if (!Directory.Exists(toSHMUPSavePath))
                    {
                        Directory.CreateDirectory(toSHMUPSavePath);
                    }

                    SettingsManager.LoadSettings();
                    using (Game1 game = new Game1((ScreenMode)SettingsManager.settings.resolution,
                        SettingsManager.settings.fullScreen))
                    {
                        game.Run();
                    }

                }
                else
                {
                    SettingsManager.LoadSettings();
                    SettingsManager.settings.resolution = int.Parse(args[0].ToString());
                    SettingsManager.settings.fullScreen = false;
                    SettingsManager.SaveSettings();
                    using (Game1 game = new Game1((ScreenMode)int.Parse(args[0].ToString()), false))
                    {
                        game.Run();
                    }
                }
            }
            catch (Exception e)
            {
                using (CrashDebugGame game = new CrashDebugGame(e))
                    game.Run();
            }
        }
    }
}

