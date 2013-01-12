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
    class XBMainMenuScreen:Screen
    {
        bool loaded;

        float counter = 0;

        Texture2D blackground;
        Background background;
        Texture2D GameTitle;
        Texture2D GameSubTitle;

        bool onControllerGrab = true;

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
            credits
        }
        //options menu
        enum optionsMenu
        {
            sfxVolume,
            musicVolume,
            cursorSpeed
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
        string musVol;
        string sfxVol;
        string cursorSpeed;

        int randomLengthCount = 1;
        List<String> listOfLevels = new List<string>();
        List<String> listOfSequences = new List<string>();
        int singleLevelSelection = 0;
        int sequenceSelection = 0;

        bool finishedOnScreen = false;

        HighScoreManager.HighScoreData highScores;

        bool menusActive = false;

        //music
        bool playmusic = true;

        public XBMainMenuScreen()
        {
            loaded = false;
            Game1.gammgr.playerShip.visible = false;

            for (int i = 0; i < (int)mainMenu.credits + 1; i++)
            {
                menuSizes.Add(1);
                menuColors.Add(Color.White);
            }
        }

        public XBMainMenuScreen(bool goToMenu)
        {
            loaded = false;
            Game1.gammgr.playerShip.visible = false;
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
            GameSubTitle = Game1.content.Load<Texture2D>("DBP");

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


            String[] seq = Directory.GetFiles(Path.Combine(Game1.content.RootDirectory, "Levels"), "*.seq");

            for (int i = 0; i < seq.Length; i++)
            {
                seq[i] = seq[i].Remove(0, Path.Combine(Game1.content.RootDirectory, "Levels").Length + 1);
                seq[i] = seq[i].Remove(seq[i].LastIndexOf(".seq"), 4);
                listOfSequences.Add(seq[i]);
            }

            //Get High scores
            highScores = HighScoreManager.LoadHighScores();
            HighScoreManager.SaveHighScores(highScores);

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
                Game1.gammgr.playerShip.visible = false;

                musVol = Strings.MusicVolume + " = " + (int)(Game1.musman.volume * 100);
                sfxVol = Strings.EffectsVolume + " = " + (int)(Game1.sndmgr.masterFXVolume * 100) ;
                cursorSpeed = Strings.CursorSpeed + " = " + (int)(SettingsManager.settings.stickMod * 100);

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
                                        if (currentSelection == (int)optionsMenu.cursorSpeed)
                                        {
                                            currentSelection = (int)optionsMenu.sfxVolume;
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
                                        if (currentSelection == (int)optionsMenu.sfxVolume)
                                        {
                                            currentSelection = (int)optionsMenu.cursorSpeed;
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
                        #region selection colors
                        for (int i = 0; i < menuColors.Count; i++)
                        {
                            if (i == (int)currentSelection)
                            {
                                menuSizes[i] = 1 + (float)(Math.Sin(counter / 200) / 10);
                                menuColors[i] = Color.White;
                            }
                            else
                            {
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

                                if (currentSelection == (int)optionsMenu.cursorSpeed)
                                {
                                    cursorSpeed = "<" + Strings.CursorSpeed + " = " + (int)(SettingsManager.settings.stickMod * 100) + ">";

                                    if (Game1.inpmgr.playerOneInput.leftStick.X > 0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if (SettingsManager.settings.stickMod < 1)
                                            SettingsManager.settings.stickMod += 0.05f;
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X < -0.3f && !lrMoved)
                                    {
                                        Game1.sndmgr.playSound(SFX.menuMove);
                                        lrMoved = true;
                                        if (SettingsManager.settings.stickMod > 0.1)
                                            SettingsManager.settings.stickMod -= 0.05f;
                                    }
                                    else if (Game1.inpmgr.playerOneInput.leftStick.X > -0.3f &&
                                        Game1.inpmgr.playerOneInput.leftStick.X < 0.3f && lrMoved)
                                    {
                                        lrMoved = false;
                                    }
                                }
                                else
                                {
                                    cursorSpeed = Strings.CursorSpeed + " = " + (int)(SettingsManager.settings.stickMod * 100);
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
                                            Game1.scrmgr.changeScreen(new Screens.ShopScreen(new Screens.XBMainMenuScreen(true), false));
                                            playSound = true;
                                            break;
                                        case mainMenu.optionsMenu:
                                            onSubMenu = subMenu.OptionsMenu;
                                            currentSelection = 0;
                                            playSound = true;
                                            break;
                                        case mainMenu.awardsMenu:
                                            finishedOnScreen = true;
                                            Game1.scrmgr.changeScreen(new Screens.AwardsStatsScreen());
                                            playSound = true;
                                            break;
                                        case mainMenu.credits:
                                            finishedOnScreen = true;
                                            Game1.scrmgr.changeScreen(new Screens.Credits());
                                            playSound = true;
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
                                            if (Levels.LevelManager.LoadLevelLists(listOfSequences[sequenceSelection]))
                                            {
                                            finishedOnScreen = true;
                                            playSound = true;

                                            Game1.gammgr.currentLevelInSequence = 0;
                                            Game1.scrmgr.changeScreen(new Screens.Levels.LevelManLevel(Game1.gammgr.levelSequence[Game1.gammgr.currentLevelInSequence]));
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
                                    Game1.plzExit = true;
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

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blackground, new Rectangle(0, 0, 2000, 2000), Color.White);
            background.Draw(gametime, spriteBatch);
            Game1.scrmgr.drawTexture(spriteBatch, GameTitle, new Vector2(0.5f, 0.2f), Color.White, 1 + (float)(Math.Sin(counter / 750) / 7.5), (float)(Math.Sin(counter / 1250) / 12.5));
            //Game1.scrmgr.drawTexture(spriteBatch, GameSubTitle, new Vector2(0.5f, 0.315f), Color.White, 0.6f + (float)(Math.Sin(counter / 2750) / 10.5), (float)(Math.Sin(counter / 5250) / 12.5));

            if (onControllerGrab)
            {
                
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.ButtonToContinue, new Vector2(0.5f, 0.7f), justification.centre);

            }
            else if (!onControllerGrab)
            {
                //Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.SHMUP, new Vector2(0.5f, 0.2f), 1.5f, (float)(Math.Sin(counter / 150) / 7.5), justification.centre);
                
                //input guide
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Select, new Vector2(0.12f, 0.86f), justification.left);
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
                        Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseLeft, new Vector2(0.1f, 0.86f), Color.White, 0.5f, 0);
                        Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseRight, new Vector2(0.1f, 0.9f), Color.White, 0.5f, 0);
                
                       break;
                    case subMenu.OptionsMenu:
                        //Options Menu options
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, sfxVol, new Vector2(0.1f, Game1.scrmgr.menuTopPosition), menuColors[0], menuSizes[0], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, musVol, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.05f), menuColors[1], menuSizes[1], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, cursorSpeed, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.1f), menuColors[2], menuSizes[2], justification.left);
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
                        }
                        Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseRight, new Vector2(0.1f, 0.9f), Color.White, 0.5f, 0);
                
                        break;
                   
                    case subMenu.PlayMenu:
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.OriginalGame, new Vector2(0.1f, Game1.scrmgr.menuTopPosition), menuColors[0], menuSizes[0], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.LevelSequence + ": " + listOfSequences[sequenceSelection], new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.05f), menuColors[1], menuSizes[1], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.RandomGame + ": " + randomLengthCount.ToString() + " " + Strings.Levels, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.1f), menuColors[2], menuSizes[2], justification.left);
                        Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.SingleLevel + listOfLevels[singleLevelSelection], new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.15f), menuColors[3], menuSizes[3], justification.left);
                        
                        Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseLeft, new Vector2(0.1f, 0.86f), Color.White, 0.5f, 0);
                        Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseRight, new Vector2(0.1f, 0.9f), Color.White, 0.5f, 0);
                
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

                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.HighScores, new Vector2(0.9f, Game1.scrmgr.menuTopPosition), justification.right);
                        
                for (int i = 0; i < highScores.Count; i++)
                {
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, highScores.PlayerName[i] + " - " + highScores.Score[i].ToString(), new Vector2(0.9f, Game1.scrmgr.menuTopPosition + 0.0333f + ((float)i * 0.03f)),
                        0.5f + (((float)highScores.Count - i) * 0.08f), justification.right);
                }

                if (Game1.gammgr.loggedIn)
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.LoggedInAs + ": " + Game1.gammgr.currentPlayer, new Vector2(0.5f, 0.9f), justification.centre);
                else
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.NotLoggedIn, new Vector2(0.5f, 0.9f), justification.centre);
             }
        }
    }
}