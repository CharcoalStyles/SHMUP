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
    class ShopScreen:Screen
    {
        Texture2D blackground;
        Background background;

        string shootSpeed;
        string missileBank;
        string noOfBuffs;
        string buffHealth;
        string buffSpeed;
        string levHealthOrbs;
        string missSplod;
        string pickUpRate;
        string tipString;
        string holdTip;

        int playerBuffPriceMod = 3600;
        int buffHealthPriceMod = 1900;
        int buffSpeedPriceMod = 1200;
        int shootSpeedPriceMod = 480;
        int missileBankPriceMod = 10;
        int missileSplodePriceMod = 60000;
        int levelHealthPriceMod = 100;
        int pickUpRatePriceMod = 700;

        int playerBuffMaxSwitch;
        int buffHealthMaxSwitch;
        int buffSpeedMaxSwitch;
        float shootSpeedMaxSwitch;
        int missileBankMaxSwitch;
        double missileSplodeMaxSwitch;
        int levelHealthMaxSwitch;
        double pickUpRateMaxSwitch;

        String maxString;
        enum subMenu
        {
            ShopMenu,
            ShipShopMenu,
            BulletShopMenu,
            OtherShopMenu
        }

        subMenu onSubMenu = subMenu.ShopMenu;
        float counter = 0;

        //shop menu
        enum shopMenu
        {
            shipMenu,
            bulletMenu,
            otherMenu
        }
        //ShipShopMenu,
        enum shipShopMenu
        {
            playerBuffs,
            buffHealth,
            buffSpeed
        }
        // BulletShopMenu,
        enum bulletShopMenu
        {
            shootSpeed,
            missilebank,
            missileSplode
        }
        //OtherShopMenu
        enum otherShopMenu
        {
            levelHealthOrbs,
            pickups
        }

        int currentSelection = 0;
        List<float> menuSizes = new List<float>();
        List<Color> menuColors = new List<Color>();
        bool moved = false;

        bool loaded;
        bool showContinue = false;

        Screen nextScreen;

        public ShopScreen(Screen s, bool sContinue)
        {
            loaded = false;

#if XBOX
                Game1.gammgr.playerShip.visible = false;
#endif

            for (int i = 0; i < (int)shopMenu.otherMenu + 1; i++)
            {
                menuSizes.Add(1);
                menuColors.Add(Color.White);
            }
            nextScreen = s;

            showContinue = sContinue;

            switch (Game1.gammgr.r.Next(11))
            {
                case 0:
                    holdTip = Strings.Tip1;
                    break;
                case 1:
                    holdTip = Strings.Tip2;
                    break;
                case 2:
                    holdTip = Strings.Tip3;
                    break;
                case 3:
                    holdTip = Strings.Tip4;
                    break;
                case 4:
                    holdTip = Strings.Tip5;
                    break;
                case 5:
                    holdTip = Strings.Tip6;
                    break;
                case 6:
                    holdTip = Strings.Tip7;
                    break;
                case 7:
                    holdTip = Strings.Tip8;
                    break;
                case 8:
                    holdTip = Strings.Tip9;
                    break;
                case 9:
                    holdTip = Strings.Tip10;
                    break;
                case 10:
                    holdTip = Strings.Tip11;
                    break;
            }

            tipString = holdTip;

            //Demo limits setup
            if (Game1.IS_DEMO)
            {
                playerBuffMaxSwitch = 6;//
                buffHealthMaxSwitch = 7;//
                buffSpeedMaxSwitch = 4;//
                shootSpeedMaxSwitch = 0.1f;//
                missileBankMaxSwitch = 70;//
                missileSplodeMaxSwitch = 0.45;//
                levelHealthMaxSwitch = 15;//
                pickUpRateMaxSwitch = 0.025;//

                maxString = " (demo limit)";//
            }
            else
            {
                playerBuffMaxSwitch = 15;
                buffHealthMaxSwitch = 15;
                buffSpeedMaxSwitch = 10;
                shootSpeedMaxSwitch = 0.2f;
                missileBankMaxSwitch = 200;
                missileSplodeMaxSwitch = 1;
                levelHealthMaxSwitch = 20;
                pickUpRateMaxSwitch = 0.04999;

                maxString = " (max)";
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

            blackground.SetData<Color>(pixels);

            background = new Background(new Vector4(0.4f, 0.2f, 0.2f, 0.25f), new Vector4(0.8f, 0.4f, 0.4f, 0.25f), false);


            shootSpeed = Strings.ShootSpeed + " = " + (Game1.gammgr.saveGameData.playerShootspeed * 200).ToString();
            missileBank = Strings.MissileBank + " = " + Game1.gammgr.saveGameData.maxMissiles.ToString();

            noOfBuffs = Strings.NumberBuffs + " = " + Game1.gammgr.saveGameData.totalPlayerBuffs.ToString();
            buffHealth = Strings.BuffHealth + " = " + Game1.gammgr.saveGameData.buffHealth.ToString();
            buffSpeed = Strings.BuffSpeed + " = " + Game1.gammgr.saveGameData.buffSpeed.ToString();

            missSplod = Strings.MissileStrength + " = " + (Game1.gammgr.saveGameData.missileExpolsion * 100).ToString();

            levHealthOrbs = Strings.ShieldStrength + " = " + Game1.gammgr.saveGameData.levelHeathOrbs.ToString();
            pickUpRate = Strings.PickUpRate + " = " + (Game1.gammgr.saveGameData.basePickUpRate * 100).ToString() + "%";

            loaded = true;
        }

        public override bool IsLoaded()
        {
            return loaded;
        }

        int calcPrice(int type)
        {
            int retInt = 0;

            switch (type)
            {
                case 0:
                    retInt = (int)Math.Round(Math.Pow((double)Game1.gammgr.saveGameData.totalPlayerBuffs, 1.8) * 9.9) * playerBuffPriceMod / 10;
                    break;
                case 1:
                    retInt = (int)Math.Round(Math.Pow((double)Game1.gammgr.saveGameData.buffHealth, 1.9) * 9.9) * buffHealthPriceMod / 10;
                    break;
                case 2:
                    retInt = (int)Math.Round(Math.Pow((double)(Game1.gammgr.saveGameData.playerShootspeed * 100), 1.85) * 9.9) * shootSpeedPriceMod / 10;
                    break;
                case 3:
                    retInt = (int)Math.Round(Math.Pow((double)Game1.gammgr.saveGameData.maxMissiles, 1.75)) * missileBankPriceMod;
                    break;
                case 4:
                    retInt = (int)Math.Round(Math.Pow((double)Game1.gammgr.saveGameData.missileExpolsion, 1.9) * 9.9) * missileSplodePriceMod / 10;
                    break;
                case 5:
                    retInt = (int)Math.Round(Math.Pow((double)Game1.gammgr.saveGameData.levelHeathOrbs, 1.75) * 9.9) * levelHealthPriceMod / 10;
                    break;
                case 6:
                    retInt = (int)Math.Round(Math.Pow((double)Game1.gammgr.saveGameData.basePickUpRate * 100, 1.85) * 9.9) * pickUpRatePriceMod / 10 + pickUpRatePriceMod;
                    break;
                case 7:
                    retInt = (int)Math.Round(Math.Pow((double)Game1.gammgr.saveGameData.buffSpeed, 1.9) * 9.9) * buffSpeedPriceMod / 10;
                    break;
            }

            return retInt;
        }

        public override void Update(GameTime gameTime)
        {
            background.Update();
            counter += gameTime.ElapsedGameTime.Milliseconds;
            shootSpeed = Strings.ShootSpeed + " = " + (Game1.gammgr.saveGameData.playerShootspeed * 200).ToString();
            missileBank = Strings.MissileBank + " = " + (Game1.gammgr.saveGameData.maxMissiles).ToString();
            noOfBuffs = Strings.NumberBuffs + " = " + Game1.gammgr.saveGameData.totalPlayerBuffs.ToString();
            buffHealth = Strings.BuffHealth + " = " + Game1.gammgr.saveGameData.buffHealth.ToString();
            buffSpeed = Strings.BuffSpeed + " = " + Game1.gammgr.saveGameData.buffSpeed.ToString();
            missSplod = Strings.MissileStrength + " = " + (Game1.gammgr.saveGameData.missileExpolsion * 100).ToString();

            levHealthOrbs = Strings.ShieldStrength + " = " + Game1.gammgr.saveGameData.levelHeathOrbs.ToString();

            pickUpRate = Strings.PickUpRate + " = " + (Game1.gammgr.saveGameData.basePickUpRate * 100).ToString() + "%";


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
                        case subMenu.ShopMenu:
                            if (currentSelection == (int)shopMenu.otherMenu)
                            {
                                currentSelection = (int)shopMenu.shipMenu;
                            }
                            else
                            {
                                currentSelection++;
                            }
                            break;
                        case subMenu.ShipShopMenu:
                            if (currentSelection == (int)shipShopMenu.buffSpeed)
                            {
                                currentSelection = (int)shipShopMenu.playerBuffs;
                            }
                            else
                            {
                                currentSelection++;
                            }
                            break;
                        case subMenu.BulletShopMenu:

                            if (currentSelection == (int)bulletShopMenu.missileSplode)
                            {
                                currentSelection = (int)bulletShopMenu.shootSpeed;
                            }
                            else
                            {
                                currentSelection++;
                            }
                            break;
                        case subMenu.OtherShopMenu:

                            if (currentSelection == (int)otherShopMenu.pickups)
                            {
                                currentSelection = (int)otherShopMenu.levelHealthOrbs;
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
                        case subMenu.ShopMenu:
                            if (currentSelection == (int)shopMenu.shipMenu)
                            {
                                currentSelection = (int)shopMenu.otherMenu;
                            }
                            else
                            {
                                currentSelection--;
                            }
                            break;
                        case subMenu.ShipShopMenu:
                            if (currentSelection == (int)shipShopMenu.playerBuffs)
                            {
                                currentSelection = (int)shipShopMenu.buffSpeed;
                            }
                            else
                            {
                                currentSelection--;
                            }
                            break;
                        case subMenu.BulletShopMenu:

                            if (currentSelection == (int)bulletShopMenu.shootSpeed)
                            {
                                currentSelection = (int)bulletShopMenu.missileSplode;
                            }
                            else
                            {
                                currentSelection--;
                            }
                            break;
                        case subMenu.OtherShopMenu:

                            if (currentSelection == (int)otherShopMenu.levelHealthOrbs)
                            {
                                currentSelection = (int)otherShopMenu.pickups;
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
                    else
                    {
                        currentSelection = -1;
                    }
                }
                #endregion
            }

            #region tipText
            switch (currentSelection)
            {
                case -1:
                    tipString = holdTip;
                    break;
                case 0:
                    switch (onSubMenu)
                    {
                        case subMenu.BulletShopMenu:
                            tipString = Strings.ShopBulletSpeed;
                            break;
                        case subMenu.OtherShopMenu:
                            tipString = Strings.ShopOtherWall;
                            break;
                        case subMenu.ShipShopMenu:
                            tipString = Strings.ShopShipNumber;
                            break;
                        case subMenu.ShopMenu:
                            tipString = holdTip;
                            break;
                    }
                    break;
                case 1:
                    switch (onSubMenu)
                    {
                        case subMenu.BulletShopMenu:
                            tipString = Strings.ShopBulletMisNum;
                            break;
                        case subMenu.OtherShopMenu:
                            tipString = Strings.ShopOtherPickups;
                            break;
                        case subMenu.ShipShopMenu:
                            tipString = Strings.ShopShipHealth;
                            break;
                        case subMenu.ShopMenu:
                            tipString = holdTip;
                            break;
                    }
                    break;
                case 2:
                    switch (onSubMenu)
                    {
                        case subMenu.BulletShopMenu:
                            tipString = Strings.ShopBulletMisStr;
                            break;
                        case subMenu.OtherShopMenu:
                            tipString = holdTip;
                            break;
                        case subMenu.ShipShopMenu:
                            tipString = Strings.ShopShipSpeed;
                            break;
                        case subMenu.ShopMenu:
                            tipString = holdTip;
                            break;
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

            #region shopColorings
            if (onSubMenu == subMenu.ShipShopMenu)
            {
                switch ((shipShopMenu)currentSelection)
                {
                    case shipShopMenu.playerBuffs:
                        if (Game1.gammgr.saveGameData.totalPlayerBuffs < playerBuffMaxSwitch)
                        {
                            noOfBuffs = Strings.NumberBuffs + " = " + Game1.gammgr.saveGameData.totalPlayerBuffs.ToString() +
                                " + 1 (" + calcPrice(0).ToString() + "p)";

                            if (Game1.gammgr.saveGameData.totalScore < calcPrice(0))
                            {
                                menuColors[currentSelection] = Color.Red;
                            }
                        }
                        else
                        {
                            noOfBuffs = Strings.NumberBuffs + " = " + Game1.gammgr.saveGameData.totalPlayerBuffs.ToString() +
                               maxString;
                            menuColors[currentSelection] = Color.Red;
                        }
                        break;
                    case shipShopMenu.buffHealth:
                        if (Game1.gammgr.saveGameData.buffHealth < buffHealthMaxSwitch)
                        {
                            buffHealth = Strings.BuffHealth + " = " + Game1.gammgr.saveGameData.buffHealth.ToString() +
                                " + 1 (" + calcPrice(1).ToString() + "p)";

                            if (Game1.gammgr.saveGameData.totalScore < calcPrice(1))
                            {
                                menuColors[currentSelection] = Color.Red;
                            }
                        }
                        else
                        {
                            buffHealth = Strings.BuffHealth + " = " + Game1.gammgr.saveGameData.buffHealth.ToString() +
                                maxString;
                            menuColors[currentSelection] = Color.Red;
                        }
                        break;
                    case shipShopMenu.buffSpeed:
                        if (Game1.gammgr.saveGameData.buffSpeed < buffSpeedMaxSwitch)
                        {
                            buffSpeed = Strings.BuffSpeed + " = " + Game1.gammgr.saveGameData.buffSpeed.ToString() +
                                " + 1 (" + calcPrice(7).ToString() + "p)";

                            if (Game1.gammgr.saveGameData.totalScore < calcPrice(7))
                            {
                                menuColors[currentSelection] = Color.Red;
                            }
                        }
                        else
                        {
                            buffSpeed = Strings.BuffSpeed + " = " + Game1.gammgr.saveGameData.buffSpeed.ToString() +
                                maxString;
                            menuColors[currentSelection] = Color.Red;
                        }
                        break;
                }
            }
            else if (onSubMenu == subMenu.BulletShopMenu)
            {
                switch ((bulletShopMenu)currentSelection)
                {
                    case bulletShopMenu.shootSpeed:
                        if (Game1.gammgr.saveGameData.playerShootspeed < shootSpeedMaxSwitch)
                        {
                            shootSpeed = Strings.ShootSpeed + " = " + (Game1.gammgr.saveGameData.playerShootspeed * 200).ToString() +
                                " + 2 (" + calcPrice(2).ToString() + "p)";

                            if (Game1.gammgr.saveGameData.totalScore < calcPrice(2))
                            {
                                menuColors[currentSelection] = Color.Red;
                            }
                        }
                        else
                        {
                            shootSpeed = Strings.ShootSpeed + " = " + (Game1.gammgr.saveGameData.playerShootspeed * 200).ToString() +
                                maxString;
                            menuColors[currentSelection] = Color.Red;
                        }
                        break;
                    case bulletShopMenu.missilebank:
                        if (Game1.gammgr.saveGameData.maxMissiles < missileBankMaxSwitch)
                        {
                            missileBank = Strings.MissileBank + " = " + (Game1.gammgr.saveGameData.maxMissiles).ToString() +
                                " + 10 (" + calcPrice(3).ToString() + "p)";

                            if (Game1.gammgr.saveGameData.totalScore < calcPrice(3))
                            {
                                menuColors[currentSelection] = Color.Red;
                            }
                        }
                        else
                        {
                            missileBank = Strings.MissileBank + " = " + (Game1.gammgr.saveGameData.maxMissiles).ToString() +
                                maxString;
                            menuColors[currentSelection] = Color.Red;
                        }
                        break;
                    case bulletShopMenu.missileSplode:
                        if (Game1.gammgr.saveGameData.missileExpolsion < missileSplodeMaxSwitch)
                        {
                            missSplod = Strings.MissileStrength + " = " + (Game1.gammgr.saveGameData.missileExpolsion * 100).ToString() +
                                " + 5 (" + calcPrice(4).ToString() + "p)";

                            if (Game1.gammgr.saveGameData.totalScore < calcPrice(4))
                            {
                                menuColors[currentSelection] = Color.Red;
                            }
                        }
                        else
                        {
                           missSplod = Strings.MissileBank + " = " + (Game1.gammgr.saveGameData.missileExpolsion * 100).ToString() +
                               maxString;
                           menuColors[currentSelection] = Color.Red;
                        }
                        break;
                }

            }
            else if (onSubMenu == subMenu.OtherShopMenu)
            {
                switch ((otherShopMenu)currentSelection)
                {
                    case otherShopMenu.levelHealthOrbs:
                        if (Game1.gammgr.saveGameData.levelHeathOrbs < levelHealthMaxSwitch)
                        {

                            levHealthOrbs = Strings.ShieldStrength + " = " + Game1.gammgr.saveGameData.levelHeathOrbs.ToString() +
                                " + 1 (" + calcPrice(5).ToString() + "p)";

                            if (Game1.gammgr.saveGameData.totalScore < calcPrice(5))
                            {
                                menuColors[currentSelection] = Color.Red;
                            }
                        }
                        else
                        {
                            levHealthOrbs = Strings.ShieldStrength + " = " + Game1.gammgr.saveGameData.levelHeathOrbs.ToString() +
                               maxString;
                            menuColors[currentSelection] = Color.Red;
                        }
                        break;
                    case otherShopMenu.pickups:
                        if (Game1.gammgr.saveGameData.basePickUpRate < pickUpRateMaxSwitch)
                        {

                            pickUpRate = Strings.PickUpRate + " = " + (Game1.gammgr.saveGameData.basePickUpRate * 100).ToString() +
                                "% + 0.5% (" + calcPrice(6).ToString() + "p)";

                            if (Game1.gammgr.saveGameData.totalScore < calcPrice(6))
                            {
                                menuColors[currentSelection] = Color.Red;
                            }
                        }
                        else
                        {
                            pickUpRate = Strings.PickUpRate + " = " + (Game1.gammgr.saveGameData.basePickUpRate * 100).ToString() + "% " +
                               maxString;
                            menuColors[currentSelection] = Color.Red;
                        }
                        break;
                }
            }
            #endregion

            #region Selection and Back
            if (Game1.inpmgr.playerOneInput.Start == expButtonState.Pressed ||
            Game1.inpmgr.playerOneInput.A == expButtonState.Pressed)
            {
                bool playSound = false;
                switch (onSubMenu)
                {
                    case subMenu.ShopMenu:
                        switch ((shopMenu)currentSelection)
                        {
                            case shopMenu.shipMenu:
                                onSubMenu = subMenu.ShipShopMenu;
                                currentSelection = 0;
                                playSound = true;
                                break;
                            case shopMenu.bulletMenu:
                                onSubMenu = subMenu.BulletShopMenu;
                                currentSelection = 0;
                                playSound = true;
                                break;
                            case shopMenu.otherMenu:
                                onSubMenu = subMenu.OtherShopMenu;
                                currentSelection = 0;
                                playSound = true;
                                break;
                        }
                        break;
                    case subMenu.ShipShopMenu:
                        switch ((shipShopMenu)currentSelection)
                        {
                            case shipShopMenu.playerBuffs:
                                if (Game1.gammgr.saveGameData.totalScore > calcPrice(0)
                                    && Game1.gammgr.saveGameData.totalPlayerBuffs < playerBuffMaxSwitch)
                                {
                                    Game1.gammgr.saveGameData.totalScore -= calcPrice(0);
                                    Game1.gammgr.saveGameData.totalPlayerBuffs++;
                                    Game1.gammgr.numBuffs = Game1.gammgr.saveGameData.totalPlayerBuffs;

                                    SaveGameManager.SaveGame(Game1.gammgr.currentPlayer, Game1.gammgr.saveGameData);
                                    playSound = true;
                                }
                                break;
                            case shipShopMenu.buffHealth:
                                if (Game1.gammgr.saveGameData.totalScore > calcPrice(1) &&
                                    Game1.gammgr.saveGameData.buffHealth < buffHealthMaxSwitch)
                                {
                                    Game1.gammgr.saveGameData.totalScore -= calcPrice(1);
                                    Game1.gammgr.saveGameData.buffHealth++;

                                    SaveGameManager.SaveGame(Game1.gammgr.currentPlayer, Game1.gammgr.saveGameData);
                                    playSound = true;
                                }
                                break;
                            case shipShopMenu.buffSpeed:
                                if (Game1.gammgr.saveGameData.totalScore > calcPrice(7) &&
                                    Game1.gammgr.saveGameData.buffSpeed < buffSpeedMaxSwitch)
                                {
                                    Game1.gammgr.saveGameData.totalScore -= calcPrice(7);
                                    Game1.gammgr.saveGameData.buffSpeed++;

                                    SaveGameManager.SaveGame(Game1.gammgr.currentPlayer, Game1.gammgr.saveGameData);
                                    playSound = true;
                                }
                                break;
                        }
                        break;
                    case subMenu.BulletShopMenu:
                        switch ((bulletShopMenu)currentSelection)
                        {
                            case bulletShopMenu.shootSpeed:
                                if (Game1.gammgr.saveGameData.totalScore > calcPrice(2) &&
                                    Game1.gammgr.saveGameData.playerShootspeed < shootSpeedMaxSwitch)
                                {
                                    Game1.gammgr.saveGameData.totalScore -= calcPrice(2);
                                    Game1.gammgr.saveGameData.playerShootspeed += 0.01f;

                                    if (Game1.gammgr.saveGameData.playerShootspeed > shootSpeedMaxSwitch)
                                        Game1.gammgr.saveGameData.playerShootspeed = shootSpeedMaxSwitch;

                                    SaveGameManager.SaveGame(Game1.gammgr.currentPlayer, Game1.gammgr.saveGameData);
                                    playSound = true;
                                }
                                break;
                            case bulletShopMenu.missilebank:
                                if (Game1.gammgr.saveGameData.totalScore > calcPrice(3) &&
                                    Game1.gammgr.saveGameData.maxMissiles < missileBankMaxSwitch)
                                {
                                    Game1.gammgr.saveGameData.totalScore -= calcPrice(3);
                                    Game1.gammgr.saveGameData.maxMissiles += 10;

                                    SaveGameManager.SaveGame(Game1.gammgr.currentPlayer, Game1.gammgr.saveGameData);
                                    playSound = true;
                                }
                                break;
                            case bulletShopMenu.missileSplode:
                                if (Game1.gammgr.saveGameData.totalScore > calcPrice(4) &&
                                    Game1.gammgr.saveGameData.missileExpolsion < missileSplodeMaxSwitch)
                                {
                                    Game1.gammgr.saveGameData.totalScore -= calcPrice(4);
                                    Game1.gammgr.saveGameData.missileExpolsion += 0.05;

                                    SaveGameManager.SaveGame(Game1.gammgr.currentPlayer, Game1.gammgr.saveGameData);
                                    playSound = true;
                                }
                                break;
                        }
                        break;
                    case subMenu.OtherShopMenu:
                        switch ((otherShopMenu)currentSelection)
                        {
                            case otherShopMenu.levelHealthOrbs:
                                if (Game1.gammgr.saveGameData.totalScore > calcPrice(5) &&
                                    Game1.gammgr.saveGameData.levelHeathOrbs < levelHealthMaxSwitch)
                                {
                                    Game1.gammgr.saveGameData.totalScore -= calcPrice(5);
                                    Game1.gammgr.saveGameData.levelHeathOrbs++;

                                    SaveGameManager.SaveGame(Game1.gammgr.currentPlayer, Game1.gammgr.saveGameData);
                                    playSound = true;
                                }
                                break;
                            case otherShopMenu.pickups:
                                if (Game1.gammgr.saveGameData.totalScore > calcPrice(6) &&
                                    Game1.gammgr.saveGameData.basePickUpRate < pickUpRateMaxSwitch)
                                {
                                    Game1.gammgr.saveGameData.totalScore -= calcPrice(6);
                                    Game1.gammgr.saveGameData.basePickUpRate += 0.005;

                                    SaveGameManager.SaveGame(Game1.gammgr.currentPlayer, Game1.gammgr.saveGameData);
                                    playSound = true;
                                }
                                break;
                        }
                        break;
                }

                if (playSound)
                {
                    Game1.sndmgr.playSound(SFX.menuSelect);

                    if (Game1.gammgr.saveGameData.playerShootspeed >= shootSpeedMaxSwitch &&
                        Game1.gammgr.saveGameData.maxMissiles >= missileBankMaxSwitch &&
                        Game1.gammgr.saveGameData.missileExpolsion >= missileSplodeMaxSwitch)
                    {
                        AwardsManager.checkMaxWeaponsAward();

                        if (Game1.gammgr.saveGameData.totalPlayerBuffs >= playerBuffMaxSwitch &&
                            Game1.gammgr.saveGameData.buffHealth >= buffHealthMaxSwitch &&
                            Game1.gammgr.saveGameData.buffSpeed >= buffSpeedMaxSwitch &&
                            Game1.gammgr.saveGameData.levelHeathOrbs >= levelHealthMaxSwitch &&
                            Game1.gammgr.saveGameData.basePickUpRate >= pickUpRateMaxSwitch)
                        {
                            AwardsManager.checkMaxEverythingAward();
                        }
                    }
                }
            }
            else if (Game1.inpmgr.playerOneInput.B == expButtonState.Pressed)
            {
                switch (onSubMenu)
                {
                    case subMenu.ShopMenu:
                        Game1.scrmgr.changeScreen(nextScreen);
                        break;
                    case subMenu.ShipShopMenu:
                        onSubMenu = subMenu.ShopMenu;
                        break;
                    case subMenu.BulletShopMenu:
                        onSubMenu = subMenu.ShopMenu;
                        break;
                    case subMenu.OtherShopMenu:
                        onSubMenu = subMenu.ShopMenu;
                        break;
                }
            }
            #endregion
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blackground, new Rectangle(0, 0, 2000, 2000), Color.White);
            background.Draw(gametime, spriteBatch);
           
            //Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.SHMUP, new Vector2(0.5f, 0.2f), 1.5f, (float)(Math.Sin(counter / 150) / 7.5), justification.centre);

            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Shop, new Vector2(0.5f, 0.2f), 1.5f, justification.centre);
            
            //input guide
            Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseLeft, new Vector2(0.1f, 0.86f), Color.White, 0.5f, 0);
            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Select, new Vector2(0.12f, 0.86f), justification.left);

            if (onSubMenu != subMenu.ShopMenu)
            {
                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseRight, new Vector2(0.1f, 0.9f), Color.White, 0.5f, 0);
                Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Back, new Vector2(0.12f, 0.9f), justification.left);
            }
            else
            {
                Game1.scrmgr.drawTexture(spriteBatch, InputManager.mouseRight, new Vector2(0.1f, 0.9f), Color.White, 0.5f, 0);
                if (showContinue)
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Continue, new Vector2(0.12f, 0.9f), justification.left);
                else
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.Back, new Vector2(0.12f, 0.9f), justification.left);
            }


            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.PointsToSpend + " : " + Game1.gammgr.saveGameData.totalScore.ToString() + "p", new Vector2(0.5f, 0.4f), justification.centre);

            Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, tipString, new Vector2(0.5f, 0.75f), 0.8f, justification.centre);
                    
            switch (onSubMenu)
            {
                case subMenu.ShopMenu:
                    //Shop Menu options
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.ShipMenu, new Vector2(0.1f, Game1.scrmgr.menuTopPosition), menuColors[0], menuSizes[0], justification.left);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.BulletMenu, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.05f), menuColors[1], menuSizes[1], justification.left);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, Strings.OtherMenu, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.1f), menuColors[2], menuSizes[2], justification.left);
break;
                case subMenu.ShipShopMenu:
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, noOfBuffs, new Vector2(0.1f, Game1.scrmgr.menuTopPosition), menuColors[0], menuSizes[0], justification.left);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, buffHealth, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.05f), menuColors[1], menuSizes[1], justification.left);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, buffSpeed, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.1f), menuColors[2], menuSizes[2], justification.left);
                    break;
                case subMenu.BulletShopMenu:
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, shootSpeed, new Vector2(0.1f, Game1.scrmgr.menuTopPosition), menuColors[0], menuSizes[0], justification.left);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, missileBank, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.05f), menuColors[1], menuSizes[1], justification.left);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, missSplod, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.1f), menuColors[2], menuSizes[2], justification.left);
break;
                case subMenu.OtherShopMenu:
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, levHealthOrbs, new Vector2(0.1f, Game1.scrmgr.menuTopPosition), menuColors[0], menuSizes[0], justification.left);
                    Game1.scrmgr.drawString(spriteBatch, Game1.consoleFont, pickUpRate, new Vector2(0.1f, Game1.scrmgr.menuTopPosition + 0.05f), menuColors[1], menuSizes[1], justification.left);
break;
            }
        }
    }
}
