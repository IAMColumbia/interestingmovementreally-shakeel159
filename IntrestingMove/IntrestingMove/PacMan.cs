using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace IntrestingMove
{
    public class PacMan : DrawableSprite
    {
        InputHandler input;
        bool isOnGround;
        public Vector2 GravityDir;
        public float GravityAccel;
        public int jumpHeight = -50;
        public PacMan(Game game) : base(game)
        {
            input = new InputHandler(this.Game);
            this.Game.Components.Add(input);
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("PacmanSingle");
            this.Location = new Vector2(100, 100);
            this.Direction = new Vector2 (50, 0);
            this.Speed= 100;
            isOnGround = false; 
            GravityDir = new Vector2(-1, 0);
            GravityAccel = 200.0f;
            base.LoadContent();
        }
        Vector2 KeyDirection;
        public void UpdatePacmanMove(GameTime gameTime)
        {
            UpdateKeepPacmanOnScreen(GraphicsDevice);

            UpdateInputFromKeyboard(gameTime);
            UpdateSpecialMove(gameTime);

            this.Direction = Vector2.Zero;
            this.Direction += KeyDirection;
            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);
            this.Direction = this.Direction + (GravityDir * GravityAccel) * (gameTime.ElapsedGameTime.Milliseconds / 1000);

        }
        public void UpdateInputFromKeyboard(GameTime gameTime)
        {
            KeyDirection = Vector2.Zero;
            if (input.KeyboardState.IsHoldingKey(Keys.A))
            {
                KeyDirection += new Vector2(-1,0);
            }
            if (input.KeyboardState.IsHoldingKey(Keys.D))
            {
                KeyDirection += new Vector2(1, 0);
            }
            if (input.KeyboardState.IsHoldingKey(Keys.S))
            {
                KeyDirection += new Vector2(0, 1);
            }
            if (input.KeyboardState.IsHoldingKey(Keys.W))
            {
                KeyDirection += new Vector2(0, -1);
            }
            if (isOnGround)
            {
                if (input.KeyboardState.WasKeyPressed(Keys.Space))
                {
                    KeyDirection += new Vector2(0, jumpHeight);
                    isOnGround = false; 
                }

            }


        }
        public void UpdateSpecialMove(GameTime gameTime)
        {
            if(input.KeyboardState.IsKeyDown(Keys.LeftShift) && input.KeyboardState.IsHoldingKey(Keys.D))
            {
                this.Location += new Vector2(5, 0);
            }
            if (input.KeyboardState.IsKeyDown(Keys.LeftShift) && input.KeyboardState.IsHoldingKey(Keys.A))
            {
                this.Location += new Vector2(-5, 0);
            }

        }
        public void UpdateKeepPacmanOnScreen(GraphicsDevice graphics)
        {
            //Keep PacMan On Screen
            if (
                //X right
                (this.Location.X >
                    graphics.Viewport.Width - this.spriteTexture.Width)
                ||
                //X left
                (this.Location.X < 0)
                )
            {
                //Negate X
                this.Location = this.Direction * new Vector2(-1, 1);
            }

            //Y stop at 400
            //Hack Floor location is hard coded
            //TODO viloates single resposibilty principle should be moved to it's own method
            if (this.Location.Y > 300) //HACK
            {
                this.Location.Y = 300;
                this.Direction.Y = 0;
                isOnGround = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            UpdatePacmanMove(gameTime);

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
