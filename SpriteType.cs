using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_1.Classes.Commands;
using Sprint_1.Classes.Controllers;
using Sprint_1.Classes.Sprites;
using Sprint_1.Interfaces;

[assembly:CLSCompliant(true)]
namespace Sprint_1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly GraphicsDeviceManager graphics;   
        
        //We'll use arrays for objects when making actual level,
        //everything displayed now just to demonstrate we have it and show how it all works

        SpriteBatch spriteBatch;
        ISprite mario;
        ISprite goomba;
        ISprite rKoopa;
        ISprite gKoopa;
        ISprite oneUp;
        ISprite coin;
        ISprite mushroom;
        ISprite fireFlower;
        ISprite star;
        ISprite question;
        ISprite normal;
        ISprite used;
        ISprite brick;
        ISprite broken;
        ISprite hidden;
        IController controller;
        IController gamePadController;
        bool paused;
        private Texture2D pixel; //Generic pixel texture for collision box rectangles

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
            base.Initialize();

            paused = false;
            CommandHandler handler = CommandHandler.getInstance();

            /*
             * Add key as [button name] + [Press/Hold/Release]
             *
             * Press executes on the initial press only
             * Hold executes once after a brief delay
             * Release executes once the button is released, regardless of how long button was held
             *
             * http://www.monogame.net/documentation/?page=T_Microsoft_Xna_Framework_Input_Keys
             * This page lists button names for the keyboard
             *
             * http://www.monogame.net/documentation/?page=T_Microsoft_Xna_Framework_Input_Buttons
             * This page lists button names for the game pad
             */

            //Keyboard input controller
            controller = new KeyboardController(handler);           
            controller.AddKeyMapping("QPress", new QuitCommand(this, handler));
            controller.AddKeyMapping("TPress", new SpriteVisibilityToggleCommand(mario, handler));
            controller.AddKeyMapping("CPress", new BoundaryToggleCommand(((PlayerSprite)mario).GetBoundingBox, handler));

            controller.AddKeyMapping("WPress", new UpCommand((PlayerSprite) mario, graphics, handler));
            controller.AddKeyMapping("SPress", new DownCommand((PlayerSprite) mario, graphics, handler));             
            controller.AddKeyMapping("APress", new LeftCommand((PlayerSprite) mario, graphics, handler)); 
            controller.AddKeyMapping("DPress", new RightCommand((PlayerSprite) mario, graphics, handler));

            controller.AddKeyMapping("PPress", new PauseCommand(this, handler));

            controller.AddKeyMapping("UpPress", new UpCommand((PlayerSprite) mario, graphics, handler));
            controller.AddKeyMapping("DownPress", new DownCommand((PlayerSprite) mario, graphics, handler));
            controller.AddKeyMapping("LeftPress", new LeftCommand((PlayerSprite) mario, graphics, handler));
            controller.AddKeyMapping("RightPress", new RightCommand((PlayerSprite) mario, graphics, handler));

            //PowerUp changes
            controller.AddKeyMapping("YPress", new YyCommand((PlayerSprite)mario, handler));
            controller.AddKeyMapping("UPress", new UuCommand((PlayerSprite)mario, handler));
            controller.AddKeyMapping("IPress", new IiCommand((PlayerSprite)mario, handler));
            controller.AddKeyMapping("OPress", new OoCommand((PlayerSprite)mario, handler));

            //Movement
            controller.AddKeyMapping("LeftHold", new LeftHoldCommand((PlayerSprite) mario, handler, graphics));
            controller.AddKeyMapping("LeftRelease", new DirectionReleaseCommand((PlayerSprite) mario));
            controller.AddKeyMapping("RightHold", new RightHoldCommand((PlayerSprite) mario, handler, graphics));
            controller.AddKeyMapping("RightRelease", new DirectionReleaseCommand((PlayerSprite) mario));

            controller.AddKeyMapping("UpHold", new UpHoldCommand((PlayerSprite) mario, graphics, handler));
            controller.AddKeyMapping("UpRelease", new DirectionReleaseCommand((PlayerSprite) mario));
            controller.AddKeyMapping("DownHold", new DownHoldCommand((PlayerSprite) mario, graphics, handler));
            controller.AddKeyMapping("DownRelease", new DirectionReleaseCommand((PlayerSprite) mario));

            controller.AddKeyMapping("AHold", new LeftHoldCommand((PlayerSprite) mario, handler, graphics));
            controller.AddKeyMapping("ARelease", new DirectionReleaseCommand((PlayerSprite) mario));
            controller.AddKeyMapping("DHold", new RightHoldCommand((PlayerSprite) mario, handler, graphics));
            controller.AddKeyMapping("DRelease", new DirectionReleaseCommand((PlayerSprite) mario));

            controller.AddKeyMapping("WHold", new UpHoldCommand((PlayerSprite) mario, graphics, handler));
            controller.AddKeyMapping("WRelease", new DirectionReleaseCommand((PlayerSprite) mario));
            controller.AddKeyMapping("SHold", new DownHoldCommand((PlayerSprite) mario, graphics, handler));
            controller.AddKeyMapping("SRelease", new DirectionReleaseCommand((PlayerSprite) mario));

            // block
            controller.AddKeyMapping("XPress", new XxCommand((BlockSprite) question, (PlayerSprite) mario, handler));
            controller.AddKeyMapping("HPress", new HhCommand((BlockSprite)hidden, (PlayerSprite)mario, handler));
            controller.AddKeyMapping("BPress", new BbCommand((BlockSprite)brick, (PlayerSprite)mario, handler));
            controller.AddKeyMapping("OemQuestionPress", new OemQustionCommand((BlockSprite)question, (PlayerSprite)mario, handler));

            //GamePad input controller
            gamePadController = new GamePadController(PlayerIndex.One, handler);
            gamePadController.AddKeyMapping("XHold", new QuitCommand(this, handler));
            gamePadController.AddKeyMapping("YPress", new SpriteVisibilityToggleCommand(mario, handler));

            gamePadController.AddKeyMapping("StartPress", new PauseCommand(this, handler));

            //Movement
            gamePadController.AddKeyMapping("DPadLeftHold", new LeftHoldCommand((PlayerSprite) mario, handler, graphics));
            gamePadController.AddKeyMapping("DPadRightHold", new RightHoldCommand((PlayerSprite) mario, handler, graphics));
            gamePadController.AddKeyMapping("DPadDownHold", new DownHoldCommand((PlayerSprite) mario, graphics, handler));
            gamePadController.AddKeyMapping("DPadUpHold", new UpHoldCommand((PlayerSprite) mario, graphics, handler));

            gamePadController.AddKeyMapping("DPadLeftRelease", new DirectionReleaseCommand((PlayerSprite) mario));
            gamePadController.AddKeyMapping("DPadRightRelease", new DirectionReleaseCommand((PlayerSprite) mario));
            gamePadController.AddKeyMapping("DPadDownRelease", new DirectionReleaseCommand((PlayerSprite) mario));
            gamePadController.AddKeyMapping("DPadUpRelease", new DirectionReleaseCommand((PlayerSprite) mario));

        }       

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteFactory factory = SpriteFactory.GetInstance();

            //For Collision rectangle texture
            pixel = new Texture2D(GraphicsDevice, 1,1, false, SurfaceFormat.Color);
            pixel.SetData(new[] {Color.White});

            //Creating sprites
            Texture2D marioTexture = Content.Load<Texture2D>("Mario Sprites/standardMario");
            Texture2D goombaTexture = Content.Load<Texture2D>("Enemies/Goomba");
            Texture2D rKoopaTexture = Content.Load<Texture2D>("Enemies/RedKoopa");
            Texture2D gKoopaTexture = Content.Load<Texture2D>("Enemies/GreenKoopa");
            Texture2D oneUpTexture = Content.Load<Texture2D>("Item Sprites/1Up");
            Texture2D coinTexture = Content.Load<Texture2D>("Item Sprites/coin");
            Texture2D mushroomTexture = Content.Load<Texture2D>("Item Sprites/Mushroom");
            Texture2D fireFlowerTexture = Content.Load<Texture2D>("Item Sprites/FireFlower");
            Texture2D starTexture = Content.Load<Texture2D>("Item Sprites/Star");
            Texture2D normalBlockTexture = Content.Load<Texture2D>("Blocks/NormalBlock");
            Texture2D usedBlockTexture = Content.Load<Texture2D>("Blocks/UsedBlock");
            Texture2D questionBlockTexture = Content.Load<Texture2D>("Blocks/QuestionMarkBlock");
            Texture2D brickBlockTexture = Content.Load<Texture2D>("Blocks/brickblock");
            Texture2D brokenBlockTexture = Content.Load<Texture2D>("Blocks/BrokenBlock");
            Content.Load<Texture2D>("Blocks/HiddenBlock");
            Texture2D debrisBlockTexture = Content.Load<Texture2D>("Blocks/DebrisBlock");

            mario = factory.MakeSprite(SpriteType.PlayerSprite, marioTexture, 1, 1, new Vector2(100, 100), new Vector2(0, 0), false, 0, 0);
            goomba = factory.MakeSprite(SpriteType.EnemySprite, goombaTexture, 1, 2, new Vector2(200, 100), new Vector2(0, 0), true, 300, 0);
            rKoopa = factory.MakeSprite(SpriteType.EnemySprite, rKoopaTexture, 1, 2, new Vector2(300, 100), new Vector2(0, 0), true, 300, 0);
            gKoopa = factory.MakeSprite(SpriteType.EnemySprite, gKoopaTexture, 1, 2, new Vector2(400, 100), new Vector2(0, 0), true, 300, 0);
            oneUp = factory.MakeSprite(SpriteType.ItemSprite, oneUpTexture, 1, 1, new Vector2(500, 100), new Vector2(0, 0), true, 0, 0);
            mushroom = factory.MakeSprite(SpriteType.ItemSprite, mushroomTexture, 1, 1, new Vector2(600, 100), new Vector2(0, 0), true, 0, 0);
            fireFlower = factory.MakeSprite(SpriteType.ItemSprite, fireFlowerTexture, 1, 1, new Vector2(500, 200), new Vector2(0, 0), true, 0, 0);
            coin = factory.MakeSprite(SpriteType.ItemSprite, coinTexture, 1, 4, new Vector2(600, 200), new Vector2(0, 0), true, 200, 0);
            star = factory.MakeSprite(SpriteType.ItemSprite, starTexture, 1, 4, new Vector2(400, 200), new Vector2(0, 0), true, 100, 0);
            normal = factory.MakeSprite(SpriteType.BlockSprite, normalBlockTexture, 1, 1, new Vector2(100, 300), new Vector2(0, 0), true, 0, 7);
            used = factory.MakeSprite(SpriteType.BlockSprite, usedBlockTexture, 1, 1, new Vector2(200, 300), new Vector2(0, 0), true, 0, 4);
            question = factory.MakeSprite(SpriteType.BlockSprite, questionBlockTexture, 1, 3, new Vector2(300, 300), new Vector2(0, 0), true, 0, 2);
            brick = factory.MakeSprite(SpriteType.BlockSprite, brickBlockTexture, 1, 1, new Vector2(400, 300), new Vector2(0, 0), true, 0, 1);
            broken = factory.MakeSprite(SpriteType.BlockSprite, brokenBlockTexture, 1, 1, new Vector2(500, 300), new Vector2(0, 0), true, 0, 3);
            hidden = factory.MakeSprite(SpriteType.HiddenBlock, brickBlockTexture, 1, 1, new Vector2(600, 300), new Vector2(0, 0), false, 0, 6);

            ((PlayerSprite)mario).Textures(Content);
            ((BlockSprite)brick).Textures(brokenBlockTexture, brickBlockTexture, debrisBlockTexture, questionBlockTexture, usedBlockTexture);
            ((BlockSprite)question).Textures(brokenBlockTexture, brickBlockTexture, debrisBlockTexture, questionBlockTexture, usedBlockTexture);
            ((BlockSprite)used).Textures(brokenBlockTexture, brickBlockTexture, debrisBlockTexture, questionBlockTexture, usedBlockTexture);


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {

                //Update controllers
                controller.Update(gameTime);
                gamePadController.Update(gameTime);
                if (!paused)
                {
                    mario.Update(gameTime); //PlayerSprite updates its own frames since it changes fps
                    rKoopa.Update(gameTime);
                    gKoopa.Update(gameTime);
                    goomba.Update(gameTime);
                    coin.Update(gameTime);
                    star.Update(gameTime);
                    normal.Update(gameTime);
                    used.Update(gameTime);
                    question.Update(gameTime);
                    brick.Update(gameTime);
                    broken.Update(gameTime);
                    hidden.Update(gameTime);

                    base.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw the sprites
            spriteBatch.Begin();
            
            mario.Draw(spriteBatch, pixel);
            goomba.Draw(spriteBatch, pixel);
            rKoopa.Draw(spriteBatch, pixel);
            gKoopa.Draw(spriteBatch, pixel);
            oneUp.Draw(spriteBatch, pixel);
            mushroom.Draw(spriteBatch, pixel);
            fireFlower.Draw(spriteBatch, pixel);
            coin.Draw(spriteBatch, pixel);
            star.Draw(spriteBatch, pixel);
            normal.Draw(spriteBatch, pixel);
            used.Draw(spriteBatch, pixel);
            question.Draw(spriteBatch, pixel);
            brick.Draw(spriteBatch, pixel);
            broken.Draw(spriteBatch, pixel);
            hidden.Draw(spriteBatch, pixel);

            base.Draw(gameTime);

            spriteBatch.End();
        }

        //Will be used later to pause the game at a specific moment
        public void PauseToggle()
        {
            paused = !paused;
        }
    }
}
