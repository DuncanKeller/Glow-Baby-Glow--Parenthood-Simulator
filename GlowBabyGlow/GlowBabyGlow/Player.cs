using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Duncanimation;

namespace GlowBabyGlow
{
    class Player : Actor
    {
        int index = 0;
        static int width = 35;
        static int height = 35;
        bool facingRight = true;

        float jumpStrength = 700;
        float acceleration = 50;
        float maxSpeed = 150;  //pixels per second
        float ladderSpeed = 150;

        bool holdingBaby = true;
        bool readyToThrow = false;
        float throwStrength = 75;
        Baby baby = null;
        float prevAngle = 0;

        bool alive = true;
        float respawnTimer = 0;
        float respawnTime = 1.4f;
        int lives = 3;

        int bullets = 6;
        int maxBullets = 6;
        float reloadTimer;
        float reloadTime = 1.9f; // seconds
        float recoilTimer;
        float recoilTime = 0.3f;

        Animator testAnim;


        public bool HoldingBaby
        {
            get { return holdingBaby; }
        }

        public bool ReadyToThrow
        {
            get { return readyToThrow; }
            set { readyToThrow = value; }
        }

        public Player(Point pos)
        {
            this.pos = new Vector2(pos.X, pos.Y);
            rect = new Rectangle(pos.X, pos.Y, width, height);
            testAnim = new Animator(TextureManager.testRun, 13, 6);
            testAnim.AddAnimation("run", 0, 17, 16.5f, true);
            testAnim.AddAnimation("idle", 18, 0, 0, true);
            testAnim.AddAnimation("jump", 24, 3, 15, true, 26);
            testAnim.AddAnimation("fall", 42, 3, 24, true, 44);
            testAnim.AddAnimation("shoot", 30, 10, 24, true, 40);
            testAnim.AddAnimation("climb", 48, 9, 15, true, 40);
            testAnim.AddAnimation("shake", 60, 18, 24, true, 40);
            
    
            testAnim.Play("idle");
        }

        public override void Update(float dt)
        {
            if (alive)
            {
                if (reloadTimer > 0)
                { reloadTimer -= dt / 1000; }
                else if (reloadTimer < 0) { reloadTimer = 0; bullets = maxBullets; }

                if (recoilTimer > 0)
                { recoilTimer -= dt / 1000; }
                else
                {
                    recoilTimer = 0; 
                    if(testAnim.CurrentAnimation == "shoot")
                    { testAnim.Play("idle"); }
                }

                if (baby != null)
                {
                    baby.Update(dt);
                }

                HandleMovement(dt);
            }
            else
            {
                respawnTimer -= dt / 1000;

                if (respawnTimer <= 0)
                {
                    Respawn();
                }
            }

            testAnim.Update(dt);
            base.Update(dt);
        }

        public void Jump()
        {
            if (!inAir && !readyToThrow)
            {
                inAir = true;
                velocity.Y = -jumpStrength;
                testAnim.Play("jump");
            }
        }

        public void Throw()
        {
            if (baby == null)
            {
                baby = new Baby(pos, throwStrength * Input.GetThumbs(index).Left.X, index);
                holdingBaby = false;
            }
        }

        public void Die()
        {
            alive = false;
            respawnTimer = respawnTime;
            baby = null;
            pos.Y = Config.screenH + 200;
            lives--;
        }

        public void Respawn()
        {
            pos.X = Config.screenW / 2;
            pos.Y = Config.screenH - Tile.Size - rect.Height - 5;
            alive = true;
            World.EnemyManager.ClearEnemies();
        }

        public void Shoot()
        {
            if (reloadTimer == 0 && !onLadder && !inAir && !holdingBaby)
            {
                int direction = facingRight ? 1 : -1;
                Vector2 shootPoint = new Vector2(rect.X + (facingRight ? width : 0), rect.Center.Y);
                Bullet b = new Bullet(shootPoint, direction);

                recoilTimer = recoilTime;
                bullets--;
                World.BulletManager.Bullets.Add(b);

                testAnim.Play("shoot"); 

                if (bullets == 0)
                {
                    reloadTimer = reloadTime;
                }
            }
        }

        public void Shake(float angle)
        {
            float dAngle = angle - prevAgnle;
            // animation speed = somenum * dAngle
        }

        public void HandleMovement(float dt)
        {
            float xInput = Input.GetThumbs(0).Left.X;
            float yInput = Input.GetThumbs(0).Left.Y;

            if (xInput > 0)
            {
                facingRight = true;
            }
            if (xInput < 0)
            {
                facingRight = false;
            }

            if (onLadder || readyToThrow || recoilTimer > 0)
            {
                xInput = 0;
            }
            else
            {
                //consider changing to yMax
                yInput = 0;
            }

            float xMax = Math.Abs(maxSpeed * xInput);
            float yMax = Math.Abs(ladderSpeed * yInput);

            velocity.X += xInput * acceleration;
            velocity.Y += -yInput * acceleration;

            if (velocity.X > xMax)
            { velocity.X = xMax; }
            else if (velocity.X < -xMax)
            { velocity.X = -xMax; }

            if (onLadder)
            {
                if (velocity.Y > yMax)
                { velocity.Y = yMax; }
                else if (velocity.Y < -yMax)
                { velocity.Y = -yMax; }
            }
            else
            {
                if (inAir && velocity.Y > 500 &&
                     testAnim.CurrentAnimation != "fall")
                {
                    testAnim.Play("fall");
                }
                else if (Math.Abs(velocity.X) > 0.1)
                {
                    if (testAnim.CurrentAnimation != "run" &&
                        testAnim.CurrentAnimation != "jump" &&
                        testAnim.CurrentAnimation != "shoot" &&
                        testAnim.CurrentAnimation != "fall")
                    { testAnim.Play("run"); }
                }
                else
                {
                    if (testAnim.CurrentAnimation != "idle" &&
                        testAnim.CurrentAnimation != "jump" &&
                        testAnim.CurrentAnimation != "shoot" &&
                        testAnim.CurrentAnimation != "fall")
                    { testAnim.Play("idle"); }
                }
            }

            if (wallLeft && xInput < 0)
            { velocity.X = 0; }
            if (wallRight && xInput > 0)
            { velocity.X = 0; }
        }

        public void Collision(ref List<Tile> tiles, ref List<Ladder> ladders)
        {
            if (baby != null)
            {
                baby.Collision(ref tiles);

                if (baby.Rect.Intersects(rect)
                    && baby.ReadyToCatch)
                {
                    baby = null;
                    holdingBaby = true;
                }
            }
            if (alive)
            {
                foreach (Enemy e in World.EnemyManager.Enemies)
                {
                    if (e.Rect.Intersects(rect))
                    {
                        Die();
                    }
                }
            }

            bool fall = true;
            bool tileCollideLeft = false;
            bool tileCollideRight = false;

            // ladders
            if (!onLadder)
            {
                //if (Input.GetThumbs(index).Left.Y > 0.2)
                if(Input.GetThumbs(index).Left.Y > 0)
                {
                    foreach (Ladder l in ladders)
                    {
                        if (l.LadderAbove(rect))
                        {
                            if (!onLadder)
                            {
                                onLadder = true;
                                velocity.X = 0;
                                velocity.Y = 0;
                                testAnim.Play("climb");
                            }
                        }
                    }  
                }
                //else if (Input.GetThumbs(index).Left.Y < -0.2)
                if (Input.GetThumbs(index).Left.Y < 0)
                {
                    foreach (Ladder l in ladders)
                    {
                        if (l.LadderBelow(rect))
                        {
                            if (!onLadder)
                            {
                                onLadder = true;
                                velocity.X = 0;
                                velocity.Y = 0;
                                testAnim.Play("climb");
                            }
                        }
                    }
                }
            }
            else
            {
                bool stillOnLadder = false;
                foreach (Ladder l in ladders)
                {
                    if (l.Rect.Intersects(rect))
                    {
                        stillOnLadder = true;
                    }
                }
                if (!stillOnLadder)
                {
                    testAnim.Play("idle");
                }

                onLadder = stillOnLadder;
            }

            // air collision, landing, walls, etc
            foreach (Tile t in tiles)
            {
                if (inAir && !onLadder)
                {
                    int overlappingAbove = t.OverlappingAbove(rect);
                    if (overlappingAbove > 0 && velocity.Y > 0)
                    {
                        inAir = false;
                        velocity.Y = 0;
                        if (!onLadder)
                        {
                            testAnim.Play("idle"); 
                            pos.Y -= overlappingAbove;
                        }
                        onLadder = false;
                        return;
                    }
                }
                if (!inAir)
                {
                    if (t.StandingOn(rect))
                    {
                        fall = false;
                    }
                }
                if (!onLadder)
                {
                    if (Input.GetThumbs(index).Left.X < 0)
                    {
                        int overlappingRight = t.OverlappingRight(rect);
                        if (overlappingRight > 0)
                        {
                            velocity.X = 0;

                            if (!wallLeft)
                            {
                                pos.X = t.Rect.Right;
                                tileCollideLeft = true;
                            }
                        }
                    }
                    if (Input.GetThumbs(index).Left.X > 0)
                    {
                        int overlappingLeft = t.OverlappingLeft(rect);
                        if (overlappingLeft > 0)
                        {
                            velocity.X = 0;

                            if (!wallRight)
                            {
                                pos.X = t.Rect.Left - rect.Width;
                                tileCollideRight = true;
                            }
                        }
                    }
                }
            }

            if (fall)
            {
                inAir = true;
            }
            wallRight = tileCollideRight;
            wallLeft = tileCollideLeft;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (baby != null)
            {
                baby.Draw(sb);
            }

            SpriteEffects effect = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            testAnim.Draw(sb, rect, Color.White, 0, Vector2.Zero, effect);
            //sb.Draw(TextureManager.blankTexture, rect, Color.Green);
        }
    }
}
