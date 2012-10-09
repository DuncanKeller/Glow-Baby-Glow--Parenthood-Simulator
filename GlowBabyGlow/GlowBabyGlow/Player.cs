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
        static int height = 38;
        bool facingRight = true;
        int score = 0;

        float jumpStrength = 500;
        float acceleration = 50;
        float maxSpeed = 175;  //pixels per second
        float ladderSpeed = 175;

        bool holdingBaby = true;
        bool readyToThrow = false;
        bool shaking = false;
        float throwStrength = 75;
        Baby baby = null;
        float prevAngle = 0;
        float shakeSpeed = 0;

        float maxBabyLife = 175;
        float babyLife;
        float babyDecay = 8.5f;
        float shakePower = 5;

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

        public Baby Baby
        {
            get { return baby; }
            set { baby = value; }
        }

        public float BabyLife
        {
            get { return babyLife; }
            set { babyLife = value; }
        }

        public float MaxBabyLife
        {
            get { return maxBabyLife; }
        }

        public bool HoldingBaby
        {
            get { return holdingBaby; }
            set { holdingBaby = value; }
        }

        public int Index
        {
            get { return index; }
        }

        public bool ReadyToThrow
        {
            get { return readyToThrow; }
            set { readyToThrow = value; }
        }

        public bool InAir
        {
            get { return inAir; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public int Lives
        {
            get { return lives; }
        }

        public Player(Point pos)
        {
            this.pos = new Vector2(pos.X, pos.Y);
            rect = new Rectangle(pos.X, pos.Y, width, height);
            hitRect = new Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height);
            hitOffset = new Point(rect.Width / 4, 0);
            babyLife = maxBabyLife;
            testAnim = new Animator(TextureManager.testRun, 13, 6);
            testAnim.AddAnimation("run", 0, 17, 16.5f, true);
            testAnim.AddAnimation("idle", 18, 0, 0, true);
            testAnim.AddAnimation("jump", 24, 3, 15, true, 26);
            testAnim.AddAnimation("fall", 42, 3, 24, true, 44);
            testAnim.AddAnimation("shoot", 30, 10, 24, true);
            testAnim.AddAnimation("climb", 48, 8, 30, true);
            testAnim.AddAnimation("shake", 60, 17, 200, true);
    
            testAnim.Play("idle");
        }

        public override void Update(float dt)
        {
            if (alive)
            {
                if (reloadTimer > 0)
                { reloadTimer -= dt / 1000; }
                else if (reloadTimer < 0) 
                { reloadTimer = 0; bullets = maxBullets; }

                if (recoilTimer > 0)
                { recoilTimer -= dt / 1000; }
                else
                {
                    recoilTimer = 0; 
                    if(testAnim.CurrentAnimation == "shoot")
                    { testAnim.Play("idle"); }
                }

                if (babyLife <= 0)
                {
                    World.Explode();
                }

                if (baby != null)
                {
                    baby.Update(dt);
                }

                if (holdingBaby)
                {
                    testAnim.SwapSpriteSheet(TextureManager.testBaby);
                }
                else
                {
                    testAnim.SwapSpriteSheet(TextureManager.testRun);
                }

                if (!shaking)
                {
                    if (babyLife > 0)
                    {
                        babyLife -= (dt / 1000) * babyDecay;
                    }
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
            if (!inAir && !readyToThrow && !shaking)
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
                Vector2 throwPos = new Vector2(pos.X + rect.Width / 2, pos.Y + rect.Height / 2);
                baby = new Baby(throwPos, throwStrength * Input.GetThumbs(index).X, index);
                holdingBaby = false;
            }
        }

        public void Die()
        {
            alive = false;
            respawnTimer = respawnTime;
            babyLife = maxBabyLife;
            baby = null;
            pos.Y = Config.screenH + 200;
            lives--;
        }

        public void Respawn()
        {
            baby = null;
            holdingBaby = true;
            velocity = Vector2.Zero;
            pos.X = Config.screenW / 2;
            pos.Y = Config.screenH - Tile.Size - rect.Height - 5;
            alive = true;
            World.EnemyManager.ClearEnemies();
            World.CoinManager.ClearCoins();
            World.ParticleManager.ClearParticles();
            World.BulletManager.ClearBullets();
        }

        public void Shoot()
        {
            if (reloadTimer == 0 && !onLadder && !inAir && !holdingBaby)
            {
                int direction = facingRight ? 1 : -1;
                Vector2 shootPoint = new Vector2(rect.X + (facingRight ? width : 0), rect.Center.Y);
                Bullet b = new Bullet(shootPoint, direction, this);

                recoilTimer = recoilTime;
                bullets--;
                World.BulletManager.Bullets.Add(b);

                if (testAnim.CurrentAnimation == "shoot")
                {
                    testAnim.Reset();
                }
                else
                {
                    testAnim.Play("shoot");
                }

                if (bullets == 0)
                {
                    reloadTimer = reloadTime;
                }
            }
        }

        public void StopShaking()
        {
            if (shaking)
            {
                shakeSpeed = 0;
                shaking = false;
                testAnim.Play("idle");
            }
        }

        public void KeyShake(float key)
        {
            shakeSpeed += 25;
            if (shakeSpeed > 100)
            { shakeSpeed = 100; }
            babyLife += shakePower;
            if (babyLife > maxBabyLife)
            { babyLife = maxBabyLife; }
        }

        public void StartShake()
        {
            testAnim.Play("shake");
            shaking = true;
            if (shakeSpeed > 0)
            { shakeSpeed -= 3; }
            else
            { shakeSpeed = 0; }

            if (Input.keys)
            {
                testAnim.SetSpeed(shakeSpeed);
            }
        }

        public void Shake(float angle)
        {
            shaking = true;
            float dAngle = angle - prevAngle;
            testAnim.Play("shake");
            testAnim.SetSpeed(dAngle * 120);
            prevAngle = angle;
        }

        public void HandleMovement(float dt)
        {
            float xInput = Input.GetThumbs(index).X;
            float yInput = Input.GetThumbs(index).Y;

            if (!shaking)
            {
                if (xInput > 0)
                {
                    facingRight = true;
                }
                if (xInput < 0)
                {
                    facingRight = false;
                }
            }

            if (onLadder || readyToThrow || recoilTimer > 0)
            {
                xInput = 0;
                if (onLadder && testAnim.CurrentAnimation == "climb")
                {
                    testAnim.SetSpeed(velocity.Y / 10);
                }
            }
            else
            {
                //consider changing to yMax
                yInput = 0;
            }

            if (shaking || readyToThrow)
            {
                xInput = 0;
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
                        testAnim.CurrentAnimation != "fall" &&
                        testAnim.CurrentAnimation != "shake")
                    { testAnim.Play("idle"); }
                }
            }

            if (wallLeft && xInput < 0)
            { velocity.X = 0; }
            if (wallRight && xInput > 0)
            { velocity.X = 0; }
        }

        public void CoinCollision()
        {
            CoinManager coins = World.CoinManager;
            foreach (Coin coin in coins.Coins)
            {
                if (coin.Rect.Intersects(hitRect))
                {
                    score += 500;
                    coins.Remove(coin);
                    for (int i = 0; i < Coin.numParticles; i++)
                    {
                        GoldParticle gp = new GoldParticle(new Vector2(
                            coin.Rect.Center.X, coin.Rect.Center.Y));
                        World.ParticleManager.AddParticle(gp);
                    }
                }
            }
        }

        public void Collision(ref List<Tile> tiles, ref List<Ladder> ladders)
        {
            CoinCollision();
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
                    if (e.HitRect.Intersects(hitRect))
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
                if(Input.GetThumbs(index).Y > 0)
                {
                    foreach (Ladder l in ladders)
                    {
                        if (l.LadderAbove(hitRect))
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
                if (Input.GetThumbs(index).Y < 0)
                {
                    foreach (Ladder l in ladders)
                    {
                        if (l.LadderBelow(hitRect))
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
                    if (l.Rect.Intersects(hitRect))
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
                if (onLadder)
                {
                    if (t.OverlappingAbove(hitRect) > 0 && velocity.Y > 0)
                    {

                        bool intersectingLadder = false;
                        foreach (Ladder l in ladders)
                        {
                            if (l.Rect.Intersects(t.Rect))
                            {
                                intersectingLadder = true;
                            }
                            
                        }
                        if (!intersectingLadder)
                        {
                            onLadder = false;
                            testAnim.Play("idle");
                        }
                    }
                }
                if (inAir && !onLadder)
                {
                    int overlappingAbove = t.OverlappingAbove(hitRect);
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
                    if (t.StandingOn(hitRect))
                    {
                        fall = false;
                    }
                }
                if (!onLadder)
                {
                    if (Input.GetThumbs(index).X < 0)
                    {
                        int overlappingRight = t.OverlappingRight(hitRect);
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
                    if (Input.GetThumbs(index).X > 0)
                    {
                        int overlappingLeft = t.OverlappingLeft(hitRect);
                        if (overlappingLeft > 0)
                        {
                            velocity.X = 0;

                            if (!wallRight)
                            {
                                pos.X = t.Rect.Left - hitRect.Width;
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

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            if (alive)
            {
                if (baby != null)
                {
                    baby.Draw(sb, SpriteEffects.None);
                    if (!World.Exploding)
                    {
                        LineBatch.DrawCircle(sb, new Vector2(baby.Rect.Center.X, baby.Rect.Center.Y), (int)babyLife);
                    }
                }
                else
                {
                    if (!World.Exploding)
                    {
                        if (holdingBaby)
                        {
                            LineBatch.DrawCircle(sb, new Vector2(hitRect.Center.X, hitRect.Center.Y), (int)babyLife);
                        }
                    }
                }
            }

            effect = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            //sb.Draw(TextureManager.blankTexture, rect, Color.Green);
            if (!World.Exploding)
            {
                base.Draw(sb, effect);
            }
        }
    }
}