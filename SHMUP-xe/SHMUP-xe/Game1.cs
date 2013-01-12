using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace SHMUP
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Public Statics
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static ThreadManager tmgr;

        public static ScreenManager scrmgr;

        public static SpriteFont debugFont;
        public static SpriteFont consoleFont;

        public static ContentManager content;

        public static InputManager inpmgr;

        public static GameManager gammgr;

        public static ParticleManager pclmgr;

        public static SoundManager sndmgr;

        public static MusicManager musman;

        public static BloomComponent bloom;

        public static bool plzExit = false;

        public static bool IS_DEMO = false;

        public static string VERSION = "v1.202";

        //public static IndieCityComponent icc;

        #endregion

        public Game1(ScreenMode sm, bool fs)
        {
            Window.Title = "S.H.M.U.P";
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            tmgr = new ThreadManager(this);
            tmgr.AddThread(new ThreadCode(Thread1), 50);
            tmgr.AddThread(new ThreadCode(Thread2), 10);
            tmgr.AddThread(new ThreadCode(Thread3), 50);
            Components.Add(tmgr);

            scrmgr = new ScreenManager(sm, fs);
            scrmgr.loadScreen = new Screens.LoadingScreen();
            scrmgr.currentScreen = new Screens.SplashScreen();

            inpmgr = new InputManager();

            sndmgr = new SoundManager();

            content = Content;
            musman = new MusicManager();

            bloom = new BloomComponent(this);
            Components.Add(bloom);

            Strings.Culture = CultureInfo.CurrentCulture;
            /*icc= new IndieCityComponent(
                "fbdb6004-47c5-4613-882c-c8c4a4ee6716",   // replace with your Game ID
                "80514114-4048-4382-b34d-468252fcacee",   // replace with your ICELib ID
                "303d3914-86e1-43bd-9f63-100eb9dbf282", // replace with your ICELib Secret
                true,   // replace with false if your game has no achievements
                false,   // replace with false if your game has no leaderboards
                this);
            Components.Add(icc);*/
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //load static textures (which basically renders useless your lodaing screen stuff)
            PlayerBuffs.tex = content.Load<Texture2D>("Background/Circle");
            Bullet.playerBulletTex = content.Load<Texture2D>("PlayerBullet");
            Bullet.enemyBulletTex = content.Load<Texture2D>("Background/CircleF");

            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/Solid3"));
            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/Solid4"));
            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/SoftStar3"));
            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/SoftStar4"));
            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/SoftStar5"));
            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/SoftStar6"));
            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/HardStar3"));
            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/HardStar4"));
            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/HardStar5"));
            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/HardStar6"));
            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/Solid5"));
            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/Solid6"));
            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/bulRep"));
            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Enemy/missRep"));

            Enemies.Enemy.shipTex.Add(content.Load<Texture2D>("Background/Hex"));

            Shape.CircleFade = content.Load<Texture2D>("Background/CircleF");
            Shape.CircleHard = content.Load<Texture2D>("Background/Circle");

            awardsheet.back = content.Load<Texture2D>("awardsbkgrnd");

            InputManager.mouseLeft = content.Load<Texture2D>("mouseL");
            InputManager.mouseRight = content.Load<Texture2D>("mouseR");
            InputManager.mouseMiddle = content.Load<Texture2D>("mouseM");

            LevelHealth.wallTex = content.Load<Texture2D>("wallSection");

            MissileManager.wave = content.Load<Effect>("Wave");

            Enemies.bulletRepeller.shieldTex = content.Load<Texture2D>("bulRepShield");

            AwardsManager.LoadAwardIcons();

            gammgr = new GameManager();
            gammgr.playerShip = new PlayerShip();

            pclmgr = new ParticleManager();


            sndmgr.load();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            debugFont = Content.Load<SpriteFont>("SpriteFont1");
            consoleFont = Content.Load<SpriteFont>("Chintzy");
            awardsheet.font = content.Load<SpriteFont>("AwardsFont");

            scrmgr.currentScreen.Load();
            scrmgr.loadScreen.Load();
            scrmgr.loadFSO();

            //bloom.LC();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            PickupEffectManager.Update();

            if (IsActive)
                inpmgr.update(gameTime);
            else if (gammgr.isPlaying && !gammgr.isPaused)
            {
                gammgr.isPlaying = false;
                gammgr.isPaused = true;
            }

            scrmgr.Update(gameTime);
            gammgr.Update(gameTime);
            pclmgr.Update(gameTime);
            AwardsManager.Update();
            MessageManager.Update();
            //bloom.update();

            if (plzExit)
            {
                this.Exit();
            }

            base.Update(gameTime);
        }
        /// <summary>
        /// Helper for looking up the name of a localized .xnb asset. It returns
        /// a modified version of the asset name which can then be passed to
        /// ContentManager.Load. This allows you localize data such as textures,
        /// models, and sound effects.
        /// 
        /// This uses a simple naming convention. If you have a default asset named
        /// "Foo", you can provide a specialized French version by calling it
        /// "Foo.fr", and a Japanese version called "Foo.ja". You can specialize even
        /// further by country as well as language, so if you wanted different assets
        /// for the United States vs. United Kingdom, you would add "Foo.en-US" and
        /// "Foo.en-GB".
        /// 
        /// This function looks first for the most specialized version of the asset,
        /// which includes both language and country. If that does not exist, it looks
        /// for a version that only specifies the language. If that still does not
        /// exist, it falls back to the original non-localized asset name.
        /// </summary>
        string GetLocalizedAssetName(string assetName)
        {
            string[] cultureNames =
            {
                CultureInfo.CurrentCulture.Name,                        // eg. "en-US"
                CultureInfo.CurrentCulture.TwoLetterISOLanguageName     // eg. "en"
            };

            // Look first for a specialized language-country version of the asset,
            // then if that fails, loop back around to see if we can find one that
            // specifies just the language without the country part.
            foreach (string cultureName in cultureNames)
            {
                string localizedAssetName = assetName + '.' + cultureName;

                // Does this localized asset file actually exist on disk?
                string localizedAssetPath = Path.Combine(Content.RootDirectory,
                                                         localizedAssetName + ".xnb");

                if (File.Exists(localizedAssetPath))
                {
                    return localizedAssetName;
                }
            }

            // If we didn't find any localized asset, fall back to the default name.
            return assetName;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            bloom.BeginDraw();
            /*if (SettingsManager.settings.postProcessQuality != 0)
            {
                bloom.BeginDraw();
                //bloom.Draw(spriteBatch);
                //bloom.Draw(gameTime);
            }*/

            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            scrmgr.Draw(gameTime, spriteBatch);
            gammgr.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            //inpmgr.playerOneInput.DEBUGOUTPUT(spriteBatch);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            pclmgr.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            
            base.Draw(gameTime);
            AwardsManager.Draw(gameTime, spriteBatch);
            MessageManager.Draw(spriteBatch);

            //scrmgr.drawString(spriteBatch, debugFont, HighScoreManager.finalResponse, new Vector2(0.5f, 0.97f), justification.centre); 

            spriteBatch.End();

        }

        private void Thread1(GameTime gameTime)
        {
            if (scrmgr.screenState == ScreenState.loadScreen)
            {
                scrmgr.currentScreen.Load();
            }
            else
            {

                if (AwardsManager.checkEnemyKilledAwards)
                {
                    AwardsManager.CheckEnemyKilledAwards();
                }
                if (AwardsManager.checkPlayerScoreAwards)
                {
                    AwardsManager.CheckScoreAwards();
                }
            }
        }

        private void Thread2(GameTime gameTime)
        {
            sndmgr.update();
        }

        private void Thread3(GameTime gameTime)
        {
            musman.Update();
        }
    }
}
