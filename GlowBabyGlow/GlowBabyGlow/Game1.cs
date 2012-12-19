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
     public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        World world = new World();
        bool inMenu = true;
        Thread loadingThread;
        Thread loadingScreenThread;

        Texture2D blankTexture;
        Texture2D loadingTexture;
        float loadingTimer = 0;

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

            graphics.PreferredBackBufferWidth = Config.screenW;
            graphics.PreferredBackBufferHeight = Config.screenH;
            graphics.IsFullScreen = Config.fullScrn;
            graphics.ApplyChanges();
            blankTexture = Content.Load<Texture2D>("blank");
            loadingTexture = Content.Load<Texture2D>("Menu\\loading");
            TextureManager.Init(Content);
            Config.Init();

            loadingScreenThread = new Thread(DrawLoadingScreen);
            loadingScreenThread.Start();
            loadingThread = new Thread(TextureManager.LoadContent);
            loadingThread.Start();
            

            loadingThread.Join();

            LineBatch.Init(graphics.GraphicsDevice);
            Input.Init(world);
            //world.Init();
            MenuSystem.Init(this);
            
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Input.Update();
            if (!inMenu)
            {
                world.Update(gameTime.ElapsedGameTime.Milliseconds);
            }
            else
            {
                world = MenuSystem.GetCurrentLevel();
                MenuSystem.Update(gameTime.ElapsedGameTime.Milliseconds);
            }
            Input.LateUpdate();
            // TODO: Add your update logic here

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

                // menu draw
                if (inMenu)
                {
                    MenuSystem.Draw(spriteBatch, graphics.GraphicsDevice);
                }
                else
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
                     DepthStencilState.Default, RasterizerState.CullNone, null,
                     world.Cam.get_transformation(graphics.GraphicsDevice));

                    world.Draw(spriteBatch);

                    spriteBatch.End();

                }
            }
          
            base.Draw(gameTime);
        }

        void DrawLoadingScreen()
        {
            if (!TextureManager.loaded)
            {
                GraphicsDevice.Clear(new Color(158, 252, 254));

                loadingTimer += 0.01f;
                spriteBatch.Begin();
                spriteBatch.Draw(blankTexture,
                    new Rectangle(0, 0, Config.screenW, Config.screenH), Color.Black);
                int size = Config.screenW / 10;
                Rectangle dest = new Rectangle(Config.screenW - size - 20, Config.screenH - size - 20, size, size);
                Rectangle src = new Rectangle(0, 0, loadingTexture.Width, loadingTexture.Height);
                spriteBatch.Draw(loadingTexture, dest, src, Color.White, loadingTimer,
                    new Vector2(src.Center.X, src.Center.Y), SpriteEffects.None, 0);
                spriteBatch.End();
            }
        }
    }
}
