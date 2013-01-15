using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace GlowBabyGlow
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 

    delegate void LoadAction();

     public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        World world = new World();
        bool inMenu = true;
        Thread loadingThread;

        Texture2D blankTexture;
        Texture2D loadingTexture;
        Texture2D loadingText;
        Texture2D loadingIcon;
        Texture2D loadingCircle;
        float loadingTimer = 0;

        World tutorial = new World();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Config.Init(this);

            graphics.PreferredBackBufferWidth = Config.screenW;
            graphics.PreferredBackBufferHeight = Config.realH;
            graphics.IsFullScreen = Config.fullScrn;
            graphics.ApplyChanges();
            blankTexture = Content.Load<Texture2D>("blank");
            loadingTexture = Content.Load<Texture2D>("Menu\\loading");
            loadingText = Content.Load<Texture2D>("Menu\\loadingText");
            loadingIcon = Content.Load<Texture2D>("Menu\\loadingIcon");
            loadingCircle = Content.Load<Texture2D>("Menu\\loadingCircle");
            TextureManager.Init(Content);
            SoundManager.Init(Content);
            

            //loadingScreenThread = new Thread(DrawLoadingScreen);
            //loadingScreenThread.Start();
            loadingThread = new Thread(DoLoad);
            loadingThread.Start();
            

            //loadingThread.Join();

          
        }

        public void SetDefaultRes()
        {
            int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            if (OptionsMenu.ContainsResolution(w, h))
            {
                Config.realW = w;
                Config.realH = h;
                Config.screenW = Config.realW;
                Config.screenH = (int)(Config.realW / Config.Aspect);
                Config.screenR = Config.screenW / 1920.0f;
                Config.fontRatio = Config.screenR;
            }
            else
            {
                Config.realW = 1366;
                Config.realH = 768;
                Config.screenW = Config.realW;
                Config.screenH = (int)(Config.realW / Config.Aspect);
                Config.screenR = Config.screenW / 1920.0f;
                Config.fontRatio = Config.screenR;
            }
        }

        public void SetRes()
        {
            graphics.PreferredBackBufferWidth = Config.realW;
            graphics.PreferredBackBufferHeight = Config.realH;
            graphics.IsFullScreen = Config.fullScrn;

            graphics.ApplyChanges();
        }

        public void DoLoad()
        {
            TextureManager.LoadContent(FinishLoad);
        }

        public void FinishLoad()
        {
            LineBatch.Init(graphics.GraphicsDevice);

            //world.Init();
            MenuSystem.Init(this);
            tutorial.Init("tutorial");
            tutorial.Automate = false;

            if (Config.tutorial)
            {
                Input.Init(tutorial);
            }
            else
            {
                Input.Init(world);
            }

            base.Initialize();
        }

        public void ChangeLevel(string name)
        {
            //world.Init(name);
            inMenu = false;
        }

        public void Reset()
        {
            inMenu = true;
        }

        protected override void OnExiting(Object sender, EventArgs args)
        {
            Config.InitiateSave();
            base.OnExiting(sender, args);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            

            // TODO: use this.Content to load your game content here
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
            if (TextureManager.loaded)
            {
                Input.Update();

                if (Config.tutorial)
                {
                    tutorial.Update(gameTime.ElapsedGameTime.Milliseconds);
                }
                else
                {
                    if (!inMenu)
                    {
                        world.Update(gameTime.ElapsedGameTime.Milliseconds);
                    }
                    else
                    {
                        world = MenuSystem.GetCurrentLevel();
                        MenuSystem.Update(gameTime.ElapsedGameTime.Milliseconds);
                    }
                }

                Input.LateUpdate();
                // TODO: Add your update logic here
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (TextureManager.loaded)
            {
                GraphicsDevice.Clear(new Color(158, 252, 254));
                int w = (Config.realW - Config.screenW) / 2;
                int h = (Config.realH - Config.screenH) / 2;

                // menu draw
                if (inMenu)
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
                    DepthStencilState.Default, RasterizerState.CullNone, null,
                    Matrix.CreateTranslation(new Vector3(0, 0, 0)) *
                                         Matrix.CreateRotationZ(0) *
                                         Matrix.CreateScale(new Vector3(1, 1, 1)) *
                                         Matrix.CreateTranslation(new Vector3(w,
                                            h, 0)));

                    MenuSystem.Draw(spriteBatch, graphics.GraphicsDevice);

                    spriteBatch.End();
                }
                else
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
                     DepthStencilState.Default, RasterizerState.CullNone, null,
                     world.Cam.get_transformation(graphics.GraphicsDevice));

                    world.Draw(spriteBatch);

                    spriteBatch.End();

                }

                if (Config.tutorial)
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
                 DepthStencilState.Default, RasterizerState.CullNone, null,
                 Matrix.CreateTranslation(new Vector3(0, 0, 0)) *
                                      Matrix.CreateRotationZ(0) *
                                      Matrix.CreateScale(new Vector3(1, 1, 1)) *
                                      Matrix.CreateTranslation(new Vector3(w,
                                         h, 0)));

                    tutorial.Draw(spriteBatch);

                    spriteBatch.End();
                }

                  spriteBatch.Begin();

                  spriteBatch.Draw(TextureManager.blankTexture, new Rectangle(0, 0, Config.realW, h), Color.Black);
                  spriteBatch.Draw(TextureManager.blankTexture, new Rectangle(0, Config.realH - h, Config.realW, h), Color.Black);

                  spriteBatch.Draw(TextureManager.blankTexture, new Rectangle(0, 0, w, Config.realH), Color.Black);
                  spriteBatch.Draw(TextureManager.blankTexture, new Rectangle(Config.realW - w, 0, w, Config.realH), Color.Black);

                spriteBatch.End();
          
            }
            else
            {
                DrawLoadingScreen();
            }
            base.Draw(gameTime);
        }

        void DrawLoadingScreen()
        {
            if (!TextureManager.loaded)
            {
                GraphicsDevice.Clear(new Color(158, 252, 254));

                loadingTimer += 0.05f;
                spriteBatch.Begin();
                spriteBatch.Draw(blankTexture,
                    new Rectangle(0, 0, Config.screenW, Config.screenH), Color.Black);
                int size = Config.screenW / 8;
                Rectangle dest = new Rectangle(Config.screenW - size - 20, Config.screenH - size - 20, size, size);
                Rectangle src = new Rectangle(0, 0, loadingTexture.Width, loadingTexture.Height);
                spriteBatch.Draw(loadingTexture, dest, src, Color.White, 0,
                    new Vector2(src.Center.X, src.Center.Y), SpriteEffects.None, 0);

                spriteBatch.Draw(loadingCircle, dest, src, Color.White, loadingTimer,
                    new Vector2(src.Center.X, src.Center.Y), SpriteEffects.None, 0);

                spriteBatch.Draw(loadingText, new Rectangle(0, 0, 
                    (int)(loadingText.Width * Config.screenR), (int)(loadingText.Height * Config.screenR)), Color.White);

                int w = (int)(loadingIcon.Width / 2);
                int h = (int)(loadingIcon.Height / 2);

                spriteBatch.Draw(loadingIcon, new Rectangle(dest.Center.X - (w / 2) - (dest.Width / 2),
                    dest.Center.Y - (h / 2) - (dest.Height / 2), w, h),
                    Color.White);

                spriteBatch.End();
            }
        }
    }
}
