using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP.Screens
{
    class MainMenuScreen:Screen
    {
        bool loaded;

        float counter = 0;

        Texture2D blackground;
        Background background;
        Texture2D GameTitle;

        bool onControllerGrab = true;

        bool onExitFrame = false;

        enum subMenu
        {
            MainMenu,
            OptionsMenu,
            PlayMenu
        }

        subMenu onSubMenu = subMenu.MainMenu;
        //menu
        enum mainMenu
        {
            startGame,
            shopMenu,
            optionsMenu,
            awardsMenu,
            login,
            credits
        }
        //options menu
        enum optionsMenu
        {
            setResolution,
            fullScreen,
            sfxVolume,
            musicVolume,
            numParticles,
            postProcess
        }
        
        //Playmenu
        enum playMenu
        {
            original,
            sequence,
            random,
            single
        }
        
        //menu vars
        int currentSelection = 0;
        List<float> menuSizes = new List<float>();
        List<Color> menuColors = new List<Color>();
        bool moved = false;
        bool lrMoved = false;
        string resolutionString;
        int tempResolutionMod = 0;
        bool fullscreen = false;
        string fullScreenString;
        string musVol;
        string sfxVol;
        string particleModString;
        string postProcessString;

        int randomLengthCount = 1;
        List<String> listOfLevels = new List<string>();
        List<String> listOfSequences = new List<string>();
        int singleLevelSelection = 0;
        int sequenceSelection = 0;

        bool finishedOnScreen = false;

        HighScoreManager.HighScoreData highScores;
        HighScoreManager.HighScoreData globalHighScores;
        bool gHSactive = false;

        bool menusActive = false;

        bool showLMB = true;
        bool LMBisSelect = true; //if false LMB is Activate

        bool clickedBuy;

        //music
        bool playmusic = true;

        public MainMenuScreen()
        {
            loaded = false;
            clickedBuy = false;

            for (int i = 0; i < (int)mainMenu.credits + 1; i++)
            {
                menuSizes.Add(1);
                menuColors.Add(Color.White);
            }
        }

        public MainMenuScreen(bool goToMenu)
        {
            loaded = false;
            for (int i = 0; i < (int)mainMenu.credits + 1; i++)
            {
                menuSizes.Add(1);
                menuColors.Add(Color.White);
            }

            if (goToMenu)
            {
                onControllerGrab = false;
                menusActive = true;
                counter = 8000;
                playmusic = true;
            }
        }

        public override void Load()
        {
            blackground = new Texture2D(Game1.graphics.GraphicsDevice, 32, 32, false, SurfaceFormat.Color);

            Color[] pixels = new Color[blackground.Width * blackground.Height];

            for (int y = 0; y < blackground.Height; y++)
            {
                for (int x = 0; x < blackground.Width; x++)
                {
                    pixels[y * blackground.Width + x] = Color.Black;

                }
            }

            GameTitle = Game1.content.Load<Texture2D>("Logos/SHMUP-title");

            blackground.SetData<Color>(pixels);

            //background = new Background(new Vector4(0.8f, 0.2f, 0.3f, 0.2f));
            //background = new Background(new Vector4(0.8f, 0.15f, 0.25f, 0.3f), new Vector4(0.3f, 0.3f, 1, 0.3f));
            //background = new Background(new Vector4(0.1f, 0.2f, 0.75f, 0.3f), new Vector4(0.3f, 0.8f, 0.1f, 0.3f));
            //background = new Background(new Vector4(1, 0, 0, 0.3f), new Vector4(0, 0, 1, 0.3f));background = new Background(new Vector4(1, 0, 0, 0.3f), new Vector4(0, 0, 1, 0.3f));
            background = new Background(new Vector4(0.3f, 0.55f, 0.95f, 0.25f), new Vector4(0.55f, 0.3f, 0.95f, 0.25f), false);

            String[] levels = Directory.GetFiles(Path.Combine(Game1.content.RootDirectory, "Levels"), "*.lvl");

            for (int i = 0; i < levels.Length; i++)
            {
                levels[i] = levels[i].Remove(0, Path.Combine(Game1.content.RootDirectory, "Levels").Length + 1);
                levels[i] = levels[i].Remove(levels[i].LastIndexOf(".lvl"), 4);
                listOfLevels.Add(levels[i]);
            }

            if (!Game1.IS_DEMO)
            {
                String[] seq = Directory.GetFiles(Path.Combine(Game1.content.RootDirectory, "Levels"), "*.seq");

                for (int i = 0; i < seq.Length; i++)
                {
                    seq[i] = seq[i].Remove(0, Path.Combine(Game1.content.RootDirectory, "Levels").Length + 1);
                    seq[i] = seq[i].Remove(seq[i].LastIndexOf(".seq"), 4);
                    listOfSequences.Add(seq[i]);
                }
            }

            //Get High scores
            highScores = HighScoreManager.LoadHighScores();
            HighScoreManager.SaveHighScores(highScores);

            globalHighScores = HighScoreManager.GetGlobalHighScores();
            if (globalHighScores.Score[0] > 10)
                gHSactive = true;

            finishedOnScreen = false;

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
                resolutionString = ScreenManager.resolutions[(int)Game1.scrmgr.screenMode + tempResolutionMod, 0]
                              + "x" + ScreenManager.resolutions[(int)Game1.scrmgr.screenMode + tempResolutionMod, 1];

                fullScreenString = Strings.FullScreen + " = " + fullscreen;

                musVol = Strings.MusicVolume + " = " + (Game1.musman.volume * 10);
                sfxVol = Strings.EffectsVolume + " = " + (Game1.sndmgr.masterFXVolume * 10);

                particleModString = Strings.ParticleMod + ": " + (int)(SettingsManager.settings.particleMod * 100) + "%";

                string set = "";
                switch (SettingsManager.settings.postProcessQuality)
                {
                    case 0:
                        set = Strings.Off;
                        break;
                    case 1:
                        set = Strings.Low;
                        break;
                    case 2:
                        set = Strings.Mid;
                        break;
                    case 3:
                        set = Strings.High;
                        break;
                    case 4:
                        set = Strings.Ultra;
                        break;
                }
                postProcessString = Strings.PostProcess + ": " + set;

                background.Update();
                counter += gameTime.ElapsedGameTime.Milliseconds;
                if (playmusic)
                {
                    playmusic = false;
                    Game1.musman.PlayNewSong("Hallucinoshapes");
                }
                if (!playmusic && onControllerGrab)
                {
                    if (Game1.inpmgr.checkInitalInput())
                    {
                        Game1.inpmgr.playerOneInput.Start = expButtonState.Held;

                        Game1.gammgr.playerShip.visible = true;

                        onControllerGrab = false;
                    }
                }
                else if (onExitFrame)
                {
                    if (Game1.gammgr.playerShip.position.Y > 0.555f && Game1.gammgr.playerShip.position.Y < 0.595f)
                    {
                        ondemolink = true;
                        demoLinkSize += 0.05f;
                        if (demoLinkSize > 1.25f)
                            demoLinkSize = 1.25f;

                    }
                    else
                    {
                        ondemolink = false;
                        demoLinkSize -= 0.05f;
                        if (demoLinkSize < 1f)
                            demoLinkSize = 1f;
                    }

                    if (Game1.inpmgr.playerOneInput.A == expButtonState.Pressed)
                    {
#if WINDOWS
                        if (Game1.IS_DEMO && ondemolink && !clickedBuy)
                        {
                            clickedBuy = true;
                            System.Diagnostics.Process.Start("http://store.indiecity.com/game/SHMUP");
                        }
#endif
                        Game1.plzExit = true;
                    }
                    else if (Game1.inpmgr.playerOneInput.B == expButtonState.Pressed)
                        onExitFrame = false;
                }

                else if (!onControllerGrab)
                {

                    if (Game1.inpmgr.playerOneInput.A == expButtonState.notPressed &&
                        Game1.inpmgr.playerOneInput.Start == expButtonState.notPressed)
                    {
                        menusActive = true;
                    }

                    if (!finishedOnScreen)
                    {
                        if (Game1.inpmgr.playerOneKeyboard == false)
                        {
                            #region joystickSelection

                            if (Game1.inpmgr.playerOneInput.leftStick.Y > 0.3f && !moved)
                            {
                                moved = true;
                                menuSizes[(int)currentSelection] = 1;
                                tempResolutionMod = 0;

                                Game1.sndmgr.playSound(SFX.menuMove);

                                switch (onSubMenu)
                                {
                                    case subMenu.MainMenu:
                                        if (currentSelection == (int)mainMenu.credits)
                                        {
                                            currentSelection = (int)mainMenu.startGame;
                                        }
                                        else
                                        {
                                            currentSelection++;
                                        }
                                        break;
                                    case subMenu.PlayMenu:
                                        if (currentSelection == (int)playMenu.single)
                                        {
                                            currentSelection = (int)playMenu.original;
                                        }
                                        else
                                        {
                                            currentSelection++;
                                        }
                                        break;
                                    case subMenu.OptionsMenu:
                                        if (currentSelection == (int)optionsMenu.musicVolume)
                                        {
                                            currentSelection = (int)optionsMenu.postProcess;
                                        }
                                        else
                                        {
                                            currentSelection++;
                                        }
                                        break;
                                }


                            }
                            else if (Game1.inpmgr.playerOneInput.leftStick.Y < -0.3f && !moved)
                            {
                                moved = true;
                                menuSizes[(int)currentSelection] = 1;
                                tempResolutionMod = 0;

                                Game1.sndmgr.playSound(SFX.menuMove);

                                switch (onSubMenu)
                                {
                                    case subMenu.MainMenu:
                                        if (currentSelection == (int)mainMenu.startGame)
                                        {
                                            currentSelection = (int)mainMenu.credits;
                                        }
                                        else
                                        {
                                            currentSelection--;
                                        }
                                        break;
                                    case subMenu.PlayMenu:
                                        if (currentSelection == (int)playMenu.original)
                                        {
                                            currentSelection = (int)playMenu.single;
                                        }
                                        else
                                        {
                                            currentSelection--;
                                        }
                                        break;
                                    case subMenu.OptionsMenu:
                                        if (currentSelection == (int)optionsMenu.postProcess)
                                        {
                                            currentSelection = (int)optionsMenu.musicVolume;
                                        }
                                        else
                                        {
                                            currentSelection--;
                                        }
                                        break;
                                }
                            }
                            else if (Game1.inpmgr.playerOneInput.leftStick.Y > -0.3f &&
                                Game1.inpmgr.playerOneInput.leftStick.Y < 0.3f && moved)
                            {
                                moved = false;
                            }

                            #endregion
                        }
                        else
                        {
                            #region mouseSelection
                            if (Game1.gammgr.playerShip.position.Y < Game1.scrmgr.menuTopPosition - 0.025f)
                            {
                                currentSelection = -1;
                            }
                            else
                            {
                                if (Game1.gammgr.playerShip.position.Y > Game1.scrmgr.menuTopPosition - 0.025f &&
                                    Game1.gammgr.playerShip.position.Y < Game1.scrmgr.menuTopPosition + 0.025f)
                                {
                                    //0.7f
                                    currentSelection = 0;
                                }
                                else if (Game1.gammgr.playerShip.position.Y > Game1.scrmgr.menuTopPosition + 0.025f &&
                                    Game1.gammgr.playerShip.position.Y < Game1.scrmgr.menuTopPosition + 0.075f)
                                {
                                    //0.75f
                                    currentSelection = 1;
                                }
                                else if (Game1.gammgr.playerShip.position.Y > Game1.scrmgr.menuTopPosition + 0.075f &&
                                    Game1.gammgr.playerShip.position.Y < Game1.scrmgr.menuTopPosition + 0.125f)
                                {
                                    //0.8f
                                    currentSelection = 2;
                                }
                                else if (Game1.gammgr.playerShip.position.Y > Game1.scrmgr.menuTopPosition + 0.125f &&
                                    Game1.gammgr.playerShip.position.Y < Game1.scrmgr.menuTopPosition + 0.175f)
                                {
                                    //0.85f
                                    currentSelection = 3;
                                }
                                else if (Game1.gammgr.playerShip.position.Y > Game1.scrmgr.menuTopPosition + 0.175f &&
                                    Game1.gammgr.playerShip.position.Y < Game1.scrmgr.menuTopPosition + 0.225f)
                                {
                                    //0.9f
                                    currentSelection = 4;
                                }
                                else if (Game1.gammgr.playerShip.position.Y > Game1.scrmgr.menuTopPosition + 0.225f &&
                                    Game1.gammgr.playerShip.position.Y < Game1.scrmgr.menuTopPosition + 0.275f)
                                {
                                    //0.9f
                                    currentSelection = 5;
                                }
                                else
                                {
                                    currentSelection = -1;
                                }
                            }
                            #endregion
                        }

                        #region LMB Stuff
                        switch (currentSelection)
                        {
                            case -1:
                                showLMB = false;
                                break;
                            case 0:
                                if (onSubMenu == subMenu.OptionsMenu)
                                {
                                    showLMB = true;
                                    LMBisSelect = false;
                                }
                                else
                                {
                                    showLMB = true;
                                    LMBisSelect = true;
                                }
                                break;
                            case 1:
                            if (onSubMenu == subMenu.OptionsMenu)
                            {
                                showLMB = true;
                                LMBisSelect = false;
                            }
                            else
                            {
                                showLMB = true;
                                LMBisSelect = true;
                            }
                                break;
                            case 2:
                            if (onSubMenu == subMenu.OptionsMenu)
                            {
                                showLMB = false;
                            }
                            else
                            {
                                showLMB = true;
                                LMBisSelect = true;
                            }
                                break;
                            case 3:if (onSubMenu == subMenu.OptionsMenu)
                            {
                                showLMB = false;
                            }
                            else
                            {
                                showLMB = true;
                                LMBisSelect = true;
                            }
                                break;
                            case 4: if (onSubMenu == subMenu.OptionsMenu || onSubMenu == subMenu.PlayMenu)
                            {
                                showLMB = false;
                            }
                            else
                            {
                                showLMB = true;
                                LMBisSelect = true;
                            }
                                break;
                            case 5: if (onSubMenu == subMenu.OptionsMenu || onSubMenu == subMenu.PlayMenu || onSubMenu == subMenu.MainMenu)
                            {
                                showLMB = false;
                            }
                            else
                            {
                                showLMB = true;
                                LMBisSelect = true;
                            }
                                break;
                        }
                        #endregion

                        #region selection colors
                        for (int i = 0; i < menuColors.Count; i++)
                        {
                            if (i == (int)currentSelection)
                            {
                                menuSizes[i] += 0.05f;
                                if (menuSizes[i] > 1.25f)
                                    menuSizes[i] = 1.25f;
                                menuColors[i] = Color.White;
                            }
                            else
                            {
                                menuSizes[i] -= 0.05f;
                                if (menuSizes[i] < 1)
                                    menuSizes[i] = 1;
                                menuColors[i] = Color.SlateGray;
                            }
                        }
                        #endregion


                        if (Game1.inpmgr.playerOneKeyboard == false)
                        {
                            #region controlPadModification
                            if (onSubMenu == subMenu.OptionsMenu)
                            {
                                if (currentSelection == (int)optionsMenu.setResolution)
                                {
                                    resolutionString = "< " + ScreenManager.resolutions[(int)Game1.scrmgr.screenMode + tempResolutionMod, 0]
                                        + "x" + ScreenManager.resolutions[(int)Game1.scrmgr.screenMode + tempResolutionMod, 1] + " >";
                                    if (Game1.inpmgr.playerOneInput.leftStick.X > 0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if ((int)Game1.scrmgr.screenMode + tempResolutionMod == ScreenManager.resolutions.Length / 2 - 1)
                                        {
                                            tempResolutionMod = 0 - (int)Game1.scrmgr.screenMode;
                                        }
                                        else
                                        {
                                            tempResolutionMod++;
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X < -0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if ((int)Game1.scrmgr.screenMode + tempResolutionMod == 0)
                                        {
                                            tempResolutionMod = (ScreenManager.resolutions.Length / 2 - 1) - (int)Game1.scrmgr.screenMode;
                                        }
                                        else
                                        {
                                            tempResolutionMod--;
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X > -0.3f &&
                                        Game1.inpmgr.playerOneInput.leftStick.X < 0.3f && lrMoved)
                                    {
                                        lrMoved = false;
                                    }
                                }
                                else
                                {
                                    resolutionString = ScreenManager.resolutions[(int)Game1.scrmgr.screenMode + tempResolutionMod, 0]
                                        + "x" + ScreenManager.resolutions[(int)Game1.scrmgr.screenMode + tempResolutionMod, 1];
                                }

                                if (currentSelection == (int)optionsMenu.fullScreen)
                                {
                                    fullScreenString = "< " + Strings.FullScreen + " = " + fullscreen + " >";
                                    if (Game1.inpmgr.playerOneInput.leftStick.X > 0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if (fullscreen)
                                            fullscreen = false;
                                        else
                                            fullscreen = true;
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X < -0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if (fullscreen)
                                            fullscreen = false;
                                        else
                                            fullscreen = true;
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X > -0.3f &&
                                        Game1.inpmgr.playerOneInput.leftStick.X < 0.3f && lrMoved)
                                    {
                                        lrMoved = false;
                                    }
                                }
                                else
                                {
                                    fullscreen = Game1.scrmgr.isFullScreen;
                                    fullScreenString = Strings.FullScreen + " = " + fullscreen;
                                }

                                if (currentSelection == (int)optionsMenu.musicVolume)
                                {
                                    musVol = "< " + Strings.MusicVolume + " = " + (int)(Game1.musman.volume * 100) + " >";
                                    if (Game1.inpmgr.playerOneInput.leftStick.X > 0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if (Game1.musman.volume < 1)
                                            Game1.musman.volume += 0.05m;
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X < -0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if (Game1.musman.volume > 0)
                                            Game1.musman.volume -= 0.05m;
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X > -0.3f &&
                                        Game1.inpmgr.playerOneInput.leftStick.X < 0.3f && lrMoved)
                                    {
                                        lrMoved = false;
                                    }
                                }
                                else
                                {
                                    musVol = Strings.MusicVolume + " = " + (int)(Game1.musman.volume * 100);
                                }


                                if (currentSelection == (int)optionsMenu.sfxVolume)
                                {
                                    sfxVol = "< " + Strings.EffectsVolume + " = " + (int)(Game1.sndmgr.masterFXVolume * 100) + " >";
                                    if (Game1.inpmgr.playerOneInput.leftStick.X > 0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if (Game1.sndmgr.masterFXVolume < 1)
                                            Game1.sndmgr.masterFXVolume += 0.05m;
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X < -0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if (Game1.sndmgr.masterFXVolume > 0)
                                            Game1.sndmgr.masterFXVolume -= 0.05m;
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X > -0.3f &&
                                        Game1.inpmgr.playerOneInput.leftStick.X < 0.3f && lrMoved)
                                    {
                                        lrMoved = false;
                                    }
                                }
                                else
                                {
                                    sfxVol = Strings.EffectsVolume + " = " + (int)(Game1.sndmgr.masterFXVolume * 100);
                                }

                                if (currentSelection == (int)optionsMenu.numParticles)
                                {
                                    particleModString = "< " + Strings.ParticleMod + ": " + (int)(SettingsManager.settings.particleMod * 100) + "% >";
                                    if (Game1.inpmgr.playerOneInput.leftStick.X > 0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if (SettingsManager.settings.particleMod > 0)
                                        {
                                            SettingsManager.settings.particleMod -= 0.05f;
                                            SettingsManager.SaveSettings();
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X < -0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if (SettingsManager.settings.particleMod < 1)
                                        {
                                            SettingsManager.settings.particleMod += 0.05f;
                                            SettingsManager.SaveSettings();
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X > -0.3f &&
                                        Game1.inpmgr.playerOneInput.leftStick.X < 0.3f && lrMoved)
                                    {
                                        lrMoved = false;
                                    }
                                }
                                else
                                {
                                    particleModString = Strings.ParticleMod + ": " + (int)(SettingsManager.settings.particleMod * 100) + "%";
                                }


                                if (currentSelection == (int)optionsMenu.postProcess)
                                {
                                    string s = "";
                                    switch (SettingsManager.settings.postProcessQuality)
                                    {
                                        case 0:
                                            s = Strings.Off;
                                            break;
                                        case 1:
                                            s = Strings.Low;
                                            break;
                                        case 2:
                                            s = Strings.Mid;
                                            break;
                                        case 3:
                                            s = Strings.High;
                                            break;
                                        case 4:
                                            s = Strings.Ultra;
                                            break;
                                    }

                                    postProcessString = "< " + Strings.PostProcess + ": " + s + " >";
                                    if (Game1.inpmgr.playerOneInput.leftStick.X > 0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if (SettingsManager.settings.postProcessQuality > 0)
                                        {
                                            SettingsManager.settings.postProcessQuality--;
                                            SettingsManager.SaveSettings();
                                            Game1.bloom.resetRenderTarget(SettingsManager.settings.postProcessQuality);
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X < -0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if (SettingsManager.settings.postProcessQuality < 4)
                                        {
                                            SettingsManager.settings.postProcessQuality++;
                                            SettingsManager.SaveSettings();
                                            Game1.bloom.resetRenderTarget(SettingsManager.settings.postProcessQuality);
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X > -0.3f &&
                                        Game1.inpmgr.playerOneInput.leftStick.X < 0.3f && lrMoved)
                                    {
                                        lrMoved = false;
                                    }
                                }
                                else
                                {
                                    string s = "";
                                    switch (SettingsManager.settings.postProcessQuality)
                                    {
                                        case 0:
                                            s = Strings.Off;
                                            break;
                                        case 1:
                                            s = Strings.Low;
                                            break;
                                        case 2:
                                            s = Strings.Mid;
                                            break;
                                        case 3:
                                            s = Strings.High;
                                            break;
                                        case 4:
                                            s = Strings.Ultra;
                                            break;
                                    }

                                    postProcessString = Strings.PostProcess + ": " + s;
                                }
                            }

                            if (onSubMenu == subMenu.PlayMenu)
                            {
                                if (currentSelection == (int)playMenu.random)
                                {
                                    if (Game1.inpmgr.playerOneInput.leftStick.X > 0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;

                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if (randomLengthCount < 10)
                                            randomLengthCount++;
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X < -0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;

                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if (randomLengthCount > 1)
                                            randomLengthCount--;
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X > -0.3f &&
                                        Game1.inpmgr.playerOneInput.leftStick.X < 0.3f && lrMoved)
                                    {
                                        lrMoved = false;
                                    }
                                }
                                else if (currentSelection == (int)playMenu.single)
                                {
                                    if (Game1.inpmgr.playerOneInput.leftStick.X > 0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        singleLevelSelection++;
                                        if (singleLevelSelection == listOfLevels.Count)
                                        {
                                            singleLevelSelection = 0;
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X < -0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        singleLevelSelection--;
                                        if (singleLevelSelection < 0)
                                        {
                                            singleLevelSelection = listOfLevels.Count - 1;
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X > -0.3f &&
                                        Game1.inpmgr.playerOneInput.leftStick.X < 0.3f && lrMoved)
                                    {
                                        lrMoved = false;
                                    }
                                }

                            }
                            #endregion
                        }
                        else
                        {
                            #region mouseModification
                            if (onSubMenu == subMenu.OptionsMenu)
                            {
                                if (currentSelection == (int)optionsMenu.setResolution)
                                {
                                    resolutionString = "< " + ScreenManager.resolutions[(int)Game1.scrmgr.screenMode + tempResolutionMod, 0]
                                        + "x" + ScreenManager.resolutions[(int)Game1.scrmgr.screenMode + tempResolutionMod, 1] + " >";
                                    if (Game1.inpmgr.playerOneInput.mouseWheelState == 1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if ((int)Game1.scrmgr.screenMode + tempResolutionMod == ScreenManager.resolutions.Length / 2 - 1)
                                        {
                                            tempResolutionMod = 0 - (int)Game1.scrmgr.screenMode;
                                        }
                                        else
                                        {
                                            tempResolutionMod++;
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.mouseWheelState == -1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if ((int)Game1.scrmgr.screenMode + tempResolutionMod == 0)
                                        {
                                            tempResolutionMod = (ScreenManager.resolutions.Length / 2 - 1) - (int)Game1.scrmgr.screenMode;
                                        }
                                        else
                                        {
                                            tempResolutionMod--;
                                        }
                                    }
                                }
                                else
                                {
                                    resolutionString = ScreenManager.resolutions[(int)Game1.scrmgr.screenMode + tempResolutionMod, 0]
                                        + "x" + ScreenManager.resolutions[(int)Game1.scrmgr.screenMode + tempResolutionMod, 1];
                                }

                                if (currentSelection == (int)optionsMenu.fullScreen)
                                {
                                    fullScreenString = "< " + Strings.FullScreen + " = " + fullscreen + " >";
                                    if (Game1.inpmgr.playerOneInput.mouseWheelState == 1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if (fullscreen)
                                            fullscreen = false;
                                        else
                                            fullscreen = true;
                                    }
                                    else if (Game1.inpmgr.playerOneInput.mouseWheelState == -1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if (fullscreen)
                                            fullscreen = false;
                                        else
                                            fullscreen = true;
                                    }
                                }
                                else
                                {
                                    fullscreen = Game1.scrmgr.isFullScreen;
                                    fullScreenString = Strings.FullScreen + " = " + fullscreen;
                                }

                                if (currentSelection == (int)optionsMenu.musicVolume)
                                {
                                    musVol = "< " + Strings.MusicVolume + " = " + (int)(Game1.musman.volume * 100) + " >";
                                    if (Game1.inpmgr.playerOneInput.mouseWheelState == 1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if (Game1.musman.volume < 1)
                                            Game1.musman.volume += 0.05m;
                                    }
                                    else if (Game1.inpmgr.playerOneInput.mouseWheelState == -1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if (Game1.musman.volume > 0)
                                            Game1.musman.volume -= 0.05m;
                                    }
                                }
                                else
                                {
                                    musVol = Strings.MusicVolume + " = " + (int)(Game1.musman.volume * 100);
                                }

                                if (currentSelection == (int)optionsMenu.sfxVolume)
                                {
                                    sfxVol = "< " + Strings.EffectsVolume + " = " + (int)(Game1.sndmgr.masterFXVolume * 100) + " >";
                                    if (Game1.inpmgr.playerOneInput.mouseWheelState == 1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if (Game1.sndmgr.masterFXVolume < 1)
                                            Game1.sndmgr.masterFXVolume += 0.05m;
                                    }
                                    else if (Game1.inpmgr.playerOneInput.mouseWheelState == -1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if (Game1.sndmgr.masterFXVolume > 0)
                                            Game1.sndmgr.masterFXVolume -= 0.05m;
                                    }
                                }
                                else
                                {
                                    sfxVol = Strings.EffectsVolume + " = " + (int)(Game1.sndmgr.masterFXVolume * 100);
                                }

                                if (currentSelection == (int)optionsMenu.numParticles)
                                {
                                    particleModString = "< " + Strings.ParticleMod + ": " + (int)(SettingsManager.settings.particleMod * 100) + "% >";
                                    if (Game1.inpmgr.playerOneInput.mouseWheelState == -1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if (SettingsManager.settings.particleMod > 0)
                                        {
                                            SettingsManager.settings.particleMod -= 0.1f;
                                            SettingsManager.SaveSettings();
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.mouseWheelState == 1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if (SettingsManager.settings.particleMod < 1)
                                        {
                                            SettingsManager.settings.particleMod += 0.1f;
                                            SettingsManager.SaveSettings();
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X > -0.3f &&
                                        Game1.inpmgr.playerOneInput.leftStick.X < 0.3f && lrMoved)
                                    {
                                        lrMoved = false;
                                    }
                                }
                                else
                                {
                                    particleModString = Strings.ParticleMod + ": " + (int)(SettingsManager.settings.particleMod * 100) + "%";
                                }


                                if (currentSelection == (int)optionsMenu.postProcess)
                                {
                                    string s = "";
                                    switch (SettingsManager.settings.postProcessQuality)
                                    {
                                        case 0:
                                            s = Strings.Off;
                                            break;
                                        case 1:
                                            s = Strings.Low;
                                            break;
                                        case 2:
                                            s = Strings.Mid;
                                            break;
                                        case 3:
                                            s = Strings.High;
                                            break;
                                        case 4:
                                            s = Strings.Ultra;
                                            break;
                                    }

                                    postProcessString = "< " + Strings.PostProcess + ": " + s + " >";

                                    if (Game1.inpmgr.playerOneInput.mouseWheelState == 1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);

                                        if (SettingsManager.settings.postProcessQuality < 4)
                                        {
                                            SettingsManager.settings.postProcessQuality++;
                                            SettingsManager.SaveSettings();
                                            Game1.bloom.resetRenderTarget(SettingsManager.settings.postProcessQuality);
                                        }
                                    }
                                    else
                                        if (Game1.inpmgr.playerOneInput.mouseWheelState == -1)
                                        {
                                            Game1.sndmgr.playSound(SFX.menuMove);

                                            if (SettingsManager.settings.postProcessQuality > 0)
                                            {
                                                SettingsManager.settings.postProcessQuality--;
                                                SettingsManager.SaveSettings();
                                                Game1.bloom.resetRenderTarget(SettingsManager.settings.postProcessQuality);
                                            }
                                        }
                                }
                                else
                                {
                                    string s = "";
                                    switch (SettingsManager.settings.postProcessQuality)
                                    {
                                        case 0:
                                            s = Strings.Off;
                                            break;
                                        case 1:
                                            s = Strings.Low;
                                            break;
                                        case 2:
                                            s = Strings.Mid;
                                            break;
                                        case 3:
                                            s = Strings.High;
                                            break;
                                        case 4:
                                            s = Strings.Ultra;
                                            break;
                                    }

                                    postProcessString = Strings.PostProcess + ": " + s;
                                }
                            }
                            else if (onSubMenu == subMenu.PlayMenu)
                            {
                                if (currentSelection == (int)playMenu.random)
                                {
                                    if (Game1.inpmgr.playerOneInput.mouseWheelState == 1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if (Game1.IS_DEMO)
                                        {
                                            if (randomLengthCount < 3)
                                                randomLengthCount++;
                                        }
                                        else
                                        {
                                            if (randomLengthCount < 10)
                                                randomLengthCount++;
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.mouseWheelState == -1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        if (randomLengthCount > 1)
                                            randomLengthCount--;
                                    }
                                }
                                else if (currentSelection == (int)playMenu.single)
                                {
                                    if (Game1.inpmgr.playerOneInput.mouseWheelState == 1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        singleLevelSelection++;
                                        if (singleLevelSelection == listOfLevels.Count)
                                        {
                                            singleLevelSelection = 0;
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.mouseWheelState == -1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        singleLevelSelection--;
                                        if (singleLevelSelection < 0)
                                        {
                                            singleLevelSelection = listOfLevels.Count - 1;
                                        }
                                    }
                                }
                                else if (currentSelection == (int)playMenu.sequence && !Game1.IS_DEMO)
                                {
                                    if (Game1.inpmgr.playerOneInput.mouseWheelState == 1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        sequenceSelection++;
                                        if (sequenceSelection == listOfSequences.Count)
                                        {
                                            sequenceSelection = 0;
                                        }
                                    }
                                    else if (Game1.inpmgr.playerOneInput.mouseWheelState == -1)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        sequenceSelection--;
                                        if (sequenceSelection < 0)
                                        {
                                            sequenceSelection = listOfSequences.Count - 1;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                        #region Selection and Back
                        if (Game1.inpmgr.playerOneInput.Start == expButtonState.Pressed ||
                        Game1.inpmgr.playerOneInput.A == expButtonState.Pressed && menusActive)
                        {
                            bool playSound = false;

                            switch (onSubMenu)
                            {
                                case subMenu.MainMenu:
                                    switch ((mainMenu)currentSelection)
                                    {
                                        case mainMenu.startGame:
                                            onSubMenu = subMenu.PlayMenu;
                                            currentSelection = 0;
                                            playSound = true;
                                            break;
                                        case mainMenu.shopMenu:

                                            Game1.scrmgr.changeScreen(new Screens.ShopScreen(new Screens.MainMenuScreen(true), false));
                                            playSound = true;
                                            break;
                                        case mainMenu.optionsMenu:
                                            onSubMenu = subMenu.OptionsMenu;
                                            currentSelection = 0;
                                            playSound = true;
                                            break;
                                        case mainMenu.login:
                                            finishedOnScreen = true;
                                            Game1.scrmgr.changeScreen(new Screens.Credits());
                                            playSound = true;
                                           /* finishedOnScreen = true;
                                            Game1.scrmgr.changeScreen(new Screens.LoginScreen());
                                            playSound = true;*/
                                            break;
                                        case mainMenu.credits:
                                            break;
                                        case mainMenu.awardsMenu:
                                            finishedOnScreen = true;
                                            Game1.scrmgr.changeScreen(new Screens.AwardsStatsScreen());
                                            playSound = true;
                                            break;
                                    }
                                    break;
                                case subMenu.OptionsMenu:
                                    switch ((optionsMenu)currentSelection)
                                    {
                                        case optionsMenu.setResolution:
                                            Game1.scrmgr.changeResolution((ScreenMode)Game1.scrmgr.screenMode + tempResolutionMod, fullscreen);
                                            Game1.bloom.LC();
                                            tempResolutionMod = 0;
                                            playSound = true;
                                            SettingsManager.settings.resolution = (int)Game1.scrmgr.screenMode;
                                            SettingsManager.SaveSettings();
                                            break;
                                        case optionsMenu.fullScreen:
                                            Game1.scrmgr.changeResolution(Game1.scrmgr.screenMode, fullscreen);
                                            Game1.bloom.LC();
                                            playSound = true;
                                            SettingsManager.settings.fullScreen = fullscreen;
                                            SettingsManager.SaveSettings();
                                            break;
                                    }
                                    break;
                                case subMenu.PlayMenu:
                                    Game1.gammgr.numBuffs = Game1.gammgr.saveGameData.totalPlayerBuffs;
                                    switch ((playMenu)currentSelection)
                                    {
                                        case playMenu.original:
                                            finishedOnScreen = true;
                                            playSound = true;
                                            if (Game1.gammgr.saveGameData.skipTutorial)
                                            {
                                                Screens.Levels.LevelManager.GenerateOriginalLevelSequence();
                                                Game1.scrmgr.changeScreen(new Screens.Levels.LevelManLevel(Game1.gammgr.levelSequence[Game1.gammgr.currentLevelInSequence]));
                                            }
                                            else
                                            {
                                                Game1.scrmgr.changeScreen(new Screens.Levels.TutorialLevel());
                                            }
                                            break;
                                        case playMenu.random:
                                            finishedOnScreen = true;
                                            playSound = true;
                                            Levels.LevelManager.GanerateRandomLevelSet(randomLengthCount);

                                            Game1.gammgr.currentLevelInSequence = 0;
                                            Game1.scrmgr.changeScreen(new Screens.Levels.LevelManLevel(Game1.gammgr.levelSequence[Game1.gammgr.currentLevelInSequence]));
                                            break;
                                        case playMenu.single:
                                            finishedOnScreen = true;
                                            playSound = true;
                                            Game1.gammgr.levelSequence.Clear();
                                            Game1.gammgr.levelSequence.Add(listOfLevels[singleLevelSelection]);

                                            Game1.gammgr.currentLevelInSequence = 0;
                                            Game1.scrmgr.changeScreen(new Screens.Levels.LevelManLevel(Game1.gammgr.levelSequence[Game1.gammgr.currentLevelInSequence]));
                                            break;
                                        case playMenu.sequence:
                                            if (!Game1.IS_DEMO)
                                            {
                                                if (Levels.LevelManager.LoadLevelLists(listOfSequences[sequenceSelection]))
                                                {
                                                    finishedOnScreen = true;
                                                    playSound = true;

                                                    Game1.gammgr.currentLevelInSequence = 0;
                                                    Game1.scrmgr.changeScreen(new Screens.Levels.LevelManLevel(Game1.gammgr.levelSequence[Game1.gammgr.currentLevelInSequence]));
                                                }
                                            }
                                            break;
                                    }
                                    break;
                            }

                            if (playSound)
                            {
                                Game1.sndmgr.playSound(SFX.menuSelect);
                            }
                        }
                        else if (Game1.inpmgr.playerOneInput.B == expButtonState.Pressed && menusActive)
                        {
                            switch (onSubMenu)
                            {
                                case subMenu.MainMenu:
                                    onExitFrame = true;
                                    break;
                                case subMenu.OptionsMenu:
                                    onSubMenu = subMenu.MainMenu;
                                    break;
                                case subMenu.PlayMenu:
                                    onSubMenu = subMenu.MainMenu;
                                    break;
                            }
                        }
                        #endregion
                    }
                }
            }
            else
            {
                throw new Exception("Screen Not Loaded");
            }
        }
        float demoLinkSize = 1;
        bool ondemolink = false;
        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blackground, new Rectangle(0, 0, 2000, 2000), Color.White);
            background.Draw(gametime, spriteBatch);
            Game1.scrmgr.drawTexture(spriteBatch, GameTitle, new Vector2(0.5f, 0.2f), Color.White, 1 + (float)(Math.Sin(counter / 750) / 7.5), (float)(Math.Sin(counter / 1250) / 12.5));
            
            if (Game1.IS_DEMO)
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Game1.VERSION + "(DEMO)", new Vector2(0.5f, 0.95f), Color.LemonChiffon, 0.667f, justification.centre);
            else
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Game1.VERSION, new Vector2(0.5f, 0.95f), Color.LemonChiffon, 0.667f, justification.centre);


            if (onControllerGrab)
            {
                
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.ButtonToContinue, new Vector2(0.5f, 0.7f), justification.centre);

            }
            else if (onExitFrame)
            {

                if (Game1.IS_DEMO)
                {
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.DemoOut1, new Vector2(0.5f, 0.48f), Color.LightGray, 0.75f, justification.centre);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.DemoOut2, new Vector2(0.5f, 0.5f), Color.LightGray, 0.75f, justification.centre);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.DemoOut4, new Vector2(0.5f, 0.52f), Color.LightGray, 0.75f, justification.centre);

                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.DemoOut3, new Vector2(0.5f, 0.57f), demoLinkSize, justification.centre);

                    Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseLeft, new Vector2(0.1f, 0.86f), Color.White, 0.5f, 0);
                    Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseRight, new Vector2(0.1f, 0.9f), Color.White, 0.5f, 0);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Back, new Vector2(0.12f, 0.9f), justification.left);

                    if (ondemolink)
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Yes, new Vector2(0.12f, 0.86f), justification.left);
                    else
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Quit, new Vector2(0.12f, 0.86f), justification.left);
                }
                else
                {
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.WantToExit, new Vector2(0.5f, 0.6f), justification.centre);

                    Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseLeft, new Vector2(0.1f, 0.86f), Color.White, 0.5f, 0);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Yes, new Vector2(0.12f, 0.86f), justification.left);
                    Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseRight, new Vector2(0.1f, 0.9f), Color.White, 0.5f, 0);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.No, new Vector2(0.12f, 0.9f), justification.left);
                }
            }
            else if (!onControllerGrab)
            {
                //Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.SHMUP, new Vector2(0.5f, 0.2f), 1.5f, (float)(Math.Sin(counter / 150) / 7.5), justification.centre);
                
                //input guide
                if (showLMB)
                {
                    Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseLeft, new Vector2(0.1f, 0.86f), Color.White, 0.5f, 0);
                    if (LMBisSelect)
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Select, new Vector2(0.12f, 0.86f), justification.left);
                    else
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Activate, new Vector2(0.12f, 0.86f), justification.left);

                }
                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseRight, new Vector2(0.1f, 0.9f), Color.White, 0.5f, 0);
                if (onSubMenu == subMenu.MainMenu)
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.ExitGame, new Vector2(0.12f, 0.9f), justification.left);
                else
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Back, new Vector2(0.12f, 0.9f), justification.left);


                switch (onSubMenu)
                {
                    case subMenu.MainMenu:
                        //Main Menu options
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.StartGame, new Vector2(0.1f, Game1.scrmgr.menuTopPosition), menuColors[0], menuSizes[0], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Shop, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.05f), menuColors[1], menuSizes[1], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Options, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.1f), menuColors[2], menuSizes[2], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Awards, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.15f), menuColors[3], menuSizes[3], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Credits, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.2f), menuColors[4], menuSizes[4], justification.left);

                        //Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Credits, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.25f), menuColors[4], menuSizes[4], justification.left);
                        break;
                    case subMenu.OptionsMenu:
                        //Options Menu options
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, resolutionString, new Vector2(0.1f, Game1.scrmgr.menuTopPosition), menuColors[0], menuSizes[0], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, fullScreenString, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.05f), menuColors[1], menuSizes[1], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, sfxVol, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.1f), menuColors[2], menuSizes[2], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, musVol, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.15f), menuColors[3], menuSizes[3], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, particleModString, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.2f), menuColors[4], menuSizes[4], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, postProcessString, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.25f), menuColors[5], menuSizes[5], justification.left);
                        switch (currentSelection)
                        {
                            case 0:
                                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.08f, Game1.scrmgr.menuTopPosition), Color.White, 0.5f, 0);
                                break;
                            case 1:
                                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.08f, Game1.scrmgr.menuTopPosition + 0.05f), Color.White, 0.5f, 0);
                                break;
                            case 2:
                                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.08f, Game1.scrmgr.menuTopPosition + 0.1f), Color.White, 0.5f, 0);
                                break;
                            case 3:
                                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.08f, Game1.scrmgr.menuTopPosition + 0.15f), Color.White, 0.5f, 0);
                                break;
                            case 4:
                                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.08f, Game1.scrmgr.menuTopPosition + 0.2f), Color.White, 0.5f, 0);
                                break;
                            case 5:
                                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.08f, Game1.scrmgr.menuTopPosition + 0.25f), Color.White, 0.5f, 0);
                                break;
                        }
                        break;
                   
                    case subMenu.PlayMenu:
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.OriginalGame, new Vector2(0.1f, Game1.scrmgr.menuTopPosition), menuColors[0], menuSizes[0], justification.left);
                        if (!Game1.IS_DEMO)
                            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.LevelSequence + ": " + listOfSequences[sequenceSelection], new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.05f), menuColors[1], menuSizes[1], justification.left);
                        else
                            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.LevelSequence + ": (" + Strings.Notindemo + ")", new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.05f), Color.DimGray, menuSizes[1], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.RandomGame + ": " + randomLengthCount.ToString() + " " + Strings.Levels, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.1f), menuColors[2], menuSizes[2], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.SingleLevel + listOfLevels[singleLevelSelection], new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.15f), menuColors[3], menuSizes[3], justification.left);

                        switch (currentSelection)
                        {
                            case 1:
                                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.08f, Game1.scrmgr.menuTopPosition + 0.05f), Color.White, 0.5f, 0);
                                break;
                            case 2:
                                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.08f, Game1.scrmgr.menuTopPosition + 0.1f), Color.White, 0.5f, 0);
                                break;
                            case 3:
                                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseMiddle, new Vector2(0.08f, Game1.scrmgr.menuTopPosition + 0.15f), Color.White, 0.5f, 0);
                                break;
                        }
                        break;
                }


                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.localHighScores, new Vector2(0.9f, Game1.scrmgr.menuTopPosition), justification.right);

                for (int i = 0; i < highScores.Count; i++)
                {
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, highScores.PlayerName[i] + " - " + highScores.Score[i].ToString(), new Vector2(0.9f, Game1.scrmgr.menuTopPosition + 0.03f + ((float)i * 0.021f)),
                        0.48f + (((float)highScores.Count - i) * 0.07f), justification.right);
                }

                /*if (gHSactive)
                {
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.globalHighScores, new Vector2(0.9f, Game1.scrmgr.menuTopPosition +0.15f), justification.right);

                    for (int i = 0; i < globalHighScores.Count; i++)
                    {
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, globalHighScores.PlayerName[i] + " - " + globalHighScores.Score[i].ToString(), new Vector2(0.9f, Game1.scrmgr.menuTopPosition + 0.15f + 0.03f + ((float)i * 0.021f)),
                            0.48f + (((float)highScores.Count - i) * 0.07f), justification.right);
                    }
                }
                else
                {
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.CannotConnet, new Vector2(0.9f, Game1.scrmgr.menuTopPosition + 0.15f), justification.right);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.globalHighScores, new Vector2(0.9f, Game1.scrmgr.menuTopPosition + 0.18f), justification.right);

                }*/

                if (Game1.gammgr.loggedIn)
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.LoggedInAs + ": " + Game1.gammgr.currentPlayer, new Vector2(0.5f, 0.9f), justification.centre);
                else
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.NotLoggedIn, new Vector2(0.5f, 0.9f), justification.centre);
             }
        }
    }
}