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
using Microsoft.Xna.Framework.Input.Touch;

namespace Shooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Player player;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Image for static background
        Texture2D mainBackground;

        //Parallaxing layers
        ParallaxingBackground bgLayer1;
        ParallaxingBackground bgLayer2;

        //Enemies
        Texture2D enemyTexture;
        List<Enemy> enemies;

        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;

        Random random;
        //Boolets

        Texture2D projectileTexture;
        List<Projectile> projectiles;

        TimeSpan fireTime;
        TimeSpan previousFireTime;
        //Explosions!!
        Texture2D explosionTexture;
        List<Animation> explosions;

        //Audio
        SoundEffect laserSound;
        //Sound when players/enemies die
        SoundEffect explosionSound;

        Song gameplayMusic;

        //UI
        UI ui;

        //Controls

        Keys movingLeft;
        Keys movingRight;
        Keys movingUp;
        Keys movingDown;
        //Keys shootin;



        //KeyBoard state used to determind key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        //gamepad state used to determine gamepad presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        //A movement speed for the player
        float playerMoveSpeed;


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
            // TODO: Add your initialization logic here
            player = new Player();
            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();
            enemies = new List<Enemy>();
            ui = new UI();

            previousSpawnTime = TimeSpan.Zero;

            enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            explosions = new List<Animation>();
            random = new Random();
            projectiles = new List<Projectile>();
            fireTime = TimeSpan.FromSeconds(.15f);

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
            //Load Player Resources
            explosionTexture = Content.Load<Texture2D>("explosion");
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("shipAnimation");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y
+ GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(playerAnimation, playerPosition);
            //Enemy Resoursces
            enemyTexture = Content.Load<Texture2D>("mineAnimation");

            //Set a constant player move speed
            playerMoveSpeed = 8.0f;

            //Projectiles
            projectileTexture = Content.Load<Texture2D>("laser");

            //Set player movement keys
            movingDown = Keys.Down;
            movingUp = Keys.Up;
            movingRight = Keys.Right;
            movingLeft = Keys.Left;

            //Audio
            gameplayMusic = Content.Load<Song>("sound/gameMusic");

            laserSound = Content.Load<SoundEffect>("sound/laserFire");
            explosionSound = Content.Load<SoundEffect>("sound/explosion");

            PlayMusic(gameplayMusic);

            //Load UI content
            ui.Initialize(player,Content,"gameFont");

            //Enable freedrag gesture
            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            // Load background textures
            bgLayer1.Initialize(Content, "bgLayer1", GraphicsDevice.Viewport.Width, -1);
            bgLayer2.Initialize(Content, "bgLayer2", GraphicsDevice.Viewport.Width, -2);
            mainBackground = Content.Load<Texture2D>("mainbackground");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                currentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // TODO: Add your update logic here
            UpdatePlayer(gameTime);
            bgLayer1.Update();
            bgLayer2.Update();
            UpdateEnemy(gameTime);
            UpdateProjectiles();
            UpdateCollision();
            UpdateExplosions(gameTime);
            ui.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //Start Drawing
            spriteBatch.Begin();

            //Draw background
            spriteBatch.Draw(mainBackground, Vector2.Zero, Color.Azure);
            bgLayer1.Draw(spriteBatch, Color.Azure);
            bgLayer2.Draw(spriteBatch, Color.Azure);

            //Draw Enemies
            foreach (Enemy enemy in enemies)
                enemy.Draw(spriteBatch);

            //Draw projectiles
            foreach (Projectile proj in projectiles)
                proj.Draw(spriteBatch);

            //Draw Player
            player.Draw(spriteBatch);

            //Draw Explosions
            foreach (Animation explo in explosions)
                explo.Draw(spriteBatch);

            //Draw UI
            ui.Draw(spriteBatch, GraphicsDevice.Viewport);

            //Stop drawing
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);
            //Get thumbstick controls
            player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

            //use keyboard/dpad
            if (currentKeyboardState.IsKeyDown(movingLeft) ||
                currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                player.Position.X -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(movingRight) ||
                currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                player.Position.X += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(movingUp) ||
                currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                player.Position.Y -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(movingDown) ||
                currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                player.Position.Y += playerMoveSpeed;
            }

            //Makesure player does not go out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X,
                player.PlayerAnimation.FrameWidth / 2, GraphicsDevice.Viewport.Width - (player.PlayerAnimation.FrameWidth / 2));
            player.Position.Y = MathHelper.Clamp(player.Position.Y,
                player.Height / 2, GraphicsDevice.Viewport.Height - (player.Height / 2));

            //Have the player shoot a thing!
            if (gameTime.TotalGameTime - previousFireTime > fireTime)
            {
                previousFireTime = gameTime.TotalGameTime;
                AddProjectile(player.Position + new Vector2(player.Width / 2, 0));
                //and make it go PEW
                laserSound.Play();
            }

        }
        private void UpdateProjectiles()
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update();
                if (projectiles[i].Active == false)
                    projectiles.RemoveAt(i);
            }
        }
        private void AddEnemy()
        {
            Animation enemyAnimation = new Animation();

            enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);

            //Random enemy position.
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));
            Enemy enemy = new Enemy();

            enemy.Initialize(enemyAnimation, position);
            enemies.Add(enemy);
        }
        private void AddProjectile(Vector2 position)
        {
            Projectile projectile = new Projectile();
            projectile.Initialize(GraphicsDevice.Viewport, projectileTexture, position);
            projectiles.Add(projectile);
        }
        private void AddExplosion(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);
            explosions.Add(explosion);
        }
        private void UpdateEnemy(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;
                AddEnemy();
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(gameTime);
                if (enemies[i].Active == false)
                {
                    if (enemies[i].Health <= 0)
                    {
                        AddExplosion(enemies[i].Position);
                        explosionSound.Play();
                        ui.ChangeScore(enemies[i].Value);
                    }
                    enemies.RemoveAt(i);
                }
            }
        }
        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].Active == false)
                    explosions.RemoveAt(i);
            }
        }

        private void PlayMusic(Song song)
        {
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
        }

        private void UpdateCollision()
        {
            Rectangle rectangle1;
            Rectangle rectangle2;

            rectangle1 = new Rectangle((int)player.Position.X, (int)player.Position.Y, (int)player.Width, (int)player.Height);

            for (int i = 0; i < enemies.Count; i++)
            {
                rectangle2 = new Rectangle((int)enemies[i].Position.X, (int)enemies[i].Position.Y, 
                    (int)enemies[i].Width, (int)enemies[i].Height);
                if (rectangle1.Intersects(rectangle2))
                {
                    player.Health -= enemies[i].Damage;
                    enemies[i].Health = 0;
                    if (player.Health <= 0)
                        player.Active = false;

                }

            }
            //For projectiles
            for (int i = 0; i < projectiles.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    //Create some rectangles
                    rectangle1 = new Rectangle((int)projectiles[i].Position.X -
                        projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                        projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);
                    rectangle2 = new Rectangle((int)enemies[j].Position.X, (int)enemies[j].Position.Y, 
                        (int)enemies[j].Width, (int)enemies[j].Height);
                    if (rectangle1.Intersects(rectangle2))
                    {
                        enemies[j].Health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }

                }

            }
        }
    }
}