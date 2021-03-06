﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Duncanimation;

namespace GlowBabyGlow
{
    class Player : Actor
    {
        int index = 0;
        public static int width = (int)(35 * 2 * Config.screenR);
        public static int height = (int)(38 * 2 * Config.screenR);
        bool facingRight = true;
        int score = 0;
        Vector2 spawnPoint = new Vector2();
        float spawnWidth = 0;
        float particleTimer = 0;

        float jumpStrength = (int)(1000 * Config.screenR);
        float acceleration = (int)(100 * Config.screenR);
        float maxSpeed = (int)(350 * Config.screenR);   //pixels per second
        float ladderSpeed = (int)(350 * Config.screenR);
        float ladderSnapX;
        float ladderThumbSensitivity = 0.4f;
        SoundEffectInstance runSound;

        bool holdingBaby = true;
        bool readyToThrow = false;
        bool shaking = false;
        float throwStrength = (int)(150 * Config.screenR);
        float prevAngle = 0;
        float shakeSpeed = 0;

        float maxBabyLife = 175;
        float babyLife;
        float babyDecay = 8.5f;
        float keyshakePower = 5;
        float joyshakePower = 1.3f;
        SoundEffectInstance shakeSound;
        SoundEffectInstance crySound;
        SoundEffectInstance ladderSound;
        float explodeTime = -1;

        bool alive = true;
        float respawnTimer = 0;
        float respawnTime = 3.4f;
        int lives = 3;

        int bullets = 6;
        int maxBullets = 6;
        float reloadTimer;
        float reloadTime = 1.25f; // seconds
        float recoilTimer;
        float recoilTime = 0.3f;

        bool automateRight = false;
        bool automateLeft = false;
        float automateBullet;
        bool automateDirection = false;
        float automateShake;
        bool automateUpLadder = false;
        bool automateDownLadder = false;
        float automateLadderTimer;
        float automateDirectionChange = 0;
        bool automateStartShake = false;
        bool automate = true;
        int automatePos = 0;
        bool directed = false;

        bool spawnBaby = true;
        float pointTimer = 0;
        Powerup currentPowerup = null;
        bool hide = false;

        #region Properties

        public bool Automate
        {
            get { return automate; }
            set
            {
                automate = value;
                if (value == false)
                {
                    automateStartShake = false;
                    automateUpLadder = false;
                    automateDownLadder = false;
                    automateDirection = false;
                    automateRight = false;
                    automateLeft = false;
                }
            }
        }

        public bool Shaking
        {
            get { return shaking; }
        }

        public Powerup Powerup
        {
            get { return currentPowerup; }
            set { currentPowerup = value; }
        }

        public bool Alive
        {
            get { return alive; }
        }

        public Baby Baby
        {
            get
            {
                if (w.Babies.ContainsKey(this))
                {
                    return w.Babies[this];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (w.Babies.ContainsKey(this))
                {
                    w.Babies[this] = value;
                }
                else
                {
                    w.Babies.Add(this, value);
                }
            }
        }

        public float BabyLife
        {
            get { return babyLife; }
            set { babyLife = value; }
        }

        public World World
        {
            get { return w; }
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

        #endregion

        public Player(Point pos, World w, int i) : base(w)
        {
            alive = false;
            respawnTimer = respawnTime;
            spawnPoint = new Vector2(pos.X, pos.Y);
            this.pos = new Vector2(pos.X, Config.screenH);
            rect = new Rectangle((int)(pos.X * Config.screenR),
                (int)(Config.screenH), 
                width,
                height);
            hitRect = new Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height);
            hitOffset = new Point(rect.Width / 4, 0);
            babyLife = maxBabyLife;

            switch(index){
                case 0:
                    testAnim = new Animator(TextureManager.testRun, 13, 6);
                    break;
                case 1:
                    testAnim = new Animator(TextureManager.santa, 13, 6);
                    break;
                case 2:
                    testAnim = new Animator(TextureManager.bum, 13, 6);
                    break;
                case 3:
                    testAnim = new Animator(TextureManager.pedo, 13, 6);
                    break;
                default:
                    testAnim = new Animator(TextureManager.testRun, 13, 6);
                    break;
            }

            testAnim.AddAnimation("run", 0, 17, 16.5f, true);
            testAnim.AddAnimation("idle", 18, 0, 0, true);
            testAnim.AddAnimation("throw", 19, 0, 0, true);
            testAnim.AddAnimation("jump", 24, 3, 15, true, 26);
            testAnim.AddAnimation("fall", 42, 3, 24, true, 44);
            testAnim.AddAnimation("shoot", 30, 10, 24, true);
            testAnim.AddAnimation("climb", 48, 8, 30, true);
            testAnim.AddAnimation("shake", 60, 17, 200, true);
    
            testAnim.Play("idle");

            runSound = SoundManager.run.CreateInstance();
            shakeSound = SoundManager.shake.CreateInstance();
            ladderSound = SoundManager.ladder.CreateInstance();

            //test
            if (currentPowerup != null)
            {
                currentPowerup.Activate(this);
            }
            index = i;

            if (MenuSystem.gameType == GameType.vsSurvival ||
                MenuSystem.gameType == GameType.survival ||
                MenuSystem.gameType == GameType.hotPotato)
            {
                lives = 1;
            }

            if (MenuSystem.gameType == GameType.hotPotato ||
                MenuSystem.gameType == GameType.thief)
            {
                if (index != 0) // change to a randomly generated number in menusystem
                {
                    holdingBaby = false;
                    Baby = null;
                    spawnBaby = false;
                }
            }

            if (MenuSystem.gameType == GameType.thief)
            {
                babyLife = maxBabyLife / 2;
            }

            respawnTimer -= 1.4f;
        }

        public void Direct(int pos)
        {
            automate = true;
            directed = true;
            automatePos = pos;
        }

        public void BabyExplode()
        {
            // sets some kind of value to kill after a few frames
            // so you have time to see the death circle
            if (explodeTime < 0)
            {
                explodeTime = 0;
            }
            //lives = 0;
            //Die();
            //if (MenuSystem.gameType == GameType.thief)
            //{
            //    w.SpawnBaby();
            //}
        }

        public override void Update(float dt)
        {
            if (explodeTime >= 0)
            {
                explodeTime += dt / 1000;
            }
            if (spawnWidth > 1)
            {
                particleTimer += dt / 1000;
                if (particleTimer > 0.004f && lives > 0)
                {
                    Vector2 particle = new Vector2(
                         (int)(spawnPoint.X - (spawnWidth / 2)) + (Player.width / 2)
                         + Config.rand.Next((int)spawnWidth),
                         Config.rand.Next((int)(spawnPoint.Y + (height))));
                    World.ParticleManager.AddParticle(new SpawnParticle(particle));
                    particleTimer = 0;
                }
            }

            if (explodeTime > 0.025f)
            {
                lives = 0;
                Die();
                if (MenuSystem.gameType == GameType.thief ||
                    MenuSystem.gameType == GameType.hotPotato)
                {
                    w.SpawnBaby();
                }
                explodeTime = -1;
            }

            if (alive)
            {
                if (holdingBaby &&
                    MenuSystem.gameType == GameType.thief)
                {
                    pointTimer += dt / 1000;

                    if (pointTimer > 0.5f)
                    {
                        pointTimer = 0;
                        score += 25;
                    }
                }
                else
                {
                    pointTimer = 0;
                }

                if (MenuSystem.gameType == GameType.thief ||
                    MenuSystem.gameType == GameType.vsSurvival)
                {
                    if (Baby != null)
                    {
                        if (Baby.splodin)
                        {
                            BabyExplode();
                        }
                    }
                }

                if (automateBullet > 0)
                { automateBullet -= dt / 1000; }
                else
                { automateBullet = 0; }

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
                    lives = 0;
                    w.Explode();
                    BabyExplode();
                    GameOver.death = DeathType.life;
                }

                if (holdingBaby)
                {
                    switch (index)
                    {
                        case 0:
                            testAnim.SwapSpriteSheet(TextureManager.testBaby);
                            break;
                        case 1:
                            testAnim.SwapSpriteSheet(TextureManager.santaBaby);
                            break;
                        case 2:
                            testAnim.SwapSpriteSheet(TextureManager.bumBaby);
                            break;
                        case 3:
                            testAnim.SwapSpriteSheet(TextureManager.pedoBaby);
                            break;
                        default:
                            testAnim.SwapSpriteSheet(TextureManager.testBaby);
                            break;
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0:
                            testAnim.SwapSpriteSheet(TextureManager.testRun);
                            break;
                        case 1:
                           testAnim.SwapSpriteSheet(TextureManager.santa);
                            break;
                        case 2:
                            testAnim.SwapSpriteSheet(TextureManager.bum);
                            break;
                        case 3:
                            testAnim.SwapSpriteSheet(TextureManager.pedo);
                            break;
                        default:
                            testAnim.SwapSpriteSheet(TextureManager.testRun);
                            break;
                    }

                    if (Baby != null)
                    {
                        Baby.Life = babyLife;
                    }
                }

                if (!shaking)
                {
                    if (holdingBaby ||
                        (!holdingBaby && Baby != null))
                    {
                        if (babyLife > 0)
                        {
                            if (Powerup is Pacifier)
                            {
                                babyLife -= (dt / 1000) * (babyDecay / 2.5f);
                            }
                            else
                            {
                                babyLife -= (dt / 1000) * babyDecay;
                            }
                        }
                    }
                    shakeSound.Stop();
                }
                else
                {
                    if (shakeSound.State != SoundState.Playing)
                    {
                        if (!automate)
                        {
                            shakeSound.Play();
                            shakeSound.Volume = ((shakeSpeed / 100.0f) * .7f) + .3f;
                            if (shakeSpeed == 0)
                            { shakeSound.Volume = 0; }
                        }
                    }
                    else
                    {
                        shakeSound.Volume = ((shakeSpeed / 100.0f) * .7f) + .3f;
                        if (shakeSpeed == 0)
                        { shakeSound.Volume = 0; }
                    }
                }

                if (currentPowerup != null)
                {
                    currentPowerup.Update(dt);
                    if (!currentPowerup.Active)
                    {
                        currentPowerup = null;
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

                    rect.X = (int)pos.X;
                    rect.Y = (int)pos.Y;
                    hitRect.X = rect.X + hitOffset.X;
                    hitRect.Y = rect.Y + hitOffset.Y;

                    if(lives >0){
                    for (int i = 0; i < 40; i++)
                    {
                        World.ParticleManager.AddParticle(new SpawnParticle2(new Vector2(
                            hitRect.Center.X, hitRect.Center.Y)));
                    }}
                }
            }

            if (pos.Y - height / 2 > Config.screenH)
            {
                Die();
            }

            testAnim.Update(dt);
            base.Update(dt);
        }

        public void Jump()
        {
            if (!(Powerup is SpringShoes))
            {
                if (!inAir && !readyToThrow && !shaking)
                {
                    inAir = true;
                    velocity.Y = -jumpStrength;
                    testAnim.Play("jump");
                }
            }
            else
            {
                if (!inAir && !readyToThrow && !shaking)
                {
                    inAir = true;
                    velocity.Y = -jumpStrength * 1.75f;
                    testAnim.Play("jump");
                    for (int i = 0; i < 50; i++)
                    {
                        w.ParticleManager.AddParticle(new SpringParticle(new Vector2(
                            hitRect.Center.X, hitRect.Bottom)));
                    }
                    if (!automate)
                    {
                        SoundEffectInstance spring = SoundManager.spring.CreateInstance();
                        spring.Play();
                    }
                }
            }
        }

        public void Throw()
        {
            if (Baby == null)
            {
                if (holdingBaby)
                {
                    Vector2 throwPos = new Vector2(pos.X + rect.Width / 2, pos.Y + rect.Height / 2);
                    Baby = new Baby(throwPos, throwStrength * Input.GetThumbs(index).X, index, w);
                    holdingBaby = false;
                    int i = Config.rand.Next(SoundManager.cry.Length);
                    if (!automate)
                    {
                        if (crySound != null)
                        {
                            if (crySound.State == SoundState.Playing)
                            {
                                crySound.Stop();
                            }
                        }
                        SoundEffectInstance swoosh = SoundManager.swoosh.CreateInstance();
                        swoosh.Volume = .8f;
                        swoosh.Play();

                        crySound = SoundManager.cry[i].CreateInstance();
                        crySound.Volume = .1f;
                        crySound.Play();
                    }
                }
            }
        }

        public void ResetPos()
        {
            //stub
            pos = spawnPoint;
        }

        public void Die()
        {
            if (alive)
            {
                currentPowerup = null;
                alive = false;
                respawnTimer = respawnTime;
                babyLife = maxBabyLife;
                Baby = null;
                pos.Y = Config.screenH + 200;
                //if (MenuSystem.gameType != GameType.thief)
                {
                    lives--;
                }
                spawnWidth = 0;
            }
        }

        public override void Explode()
        {
            lives = 0;

            if (!sploded)
            {
                Texture2D t = null;

                switch (index)
                {
                    case 0:
                        t = TextureManager.playerDead;
                        break;
                    case 1:
                        t = TextureManager.playerDeadSanta;
                        break;
                    case 2:
                        t = TextureManager.playerDeadBum;
                        break;
                    default:
                        t = TextureManager.playerDeadPedo;
                        break;
                }

                w.ParticleManager.AddParticle(new PlayerDeathParticle(new Vector2(
                    hitRect.Center.X, hitRect.Center.Y), t));

                base.Explode();
            }
        }

        public void Respawn()
        {
            if (lives > 0)
            {
                Baby = null;
                if (spawnBaby)
                { holdingBaby = true; }
                velocity = Vector2.Zero;
                ResetPos();
                alive = true;
                World.EnemyManager.KillEnemies();
                World.CoinManager.ClearCoins();
                World.ParticleManager.ClearParticles();
                World.BulletManager.ClearBullets();
                if (!automate)
                {
                    SoundEffectInstance spawnSound = SoundManager.spawn2.CreateInstance();
                    spawnSound.Play();
                }
                if (World.LevelName == "tutorial")
                {
                    Baby = null;
                    holdingBaby = false;
                }
            }
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
                if (!automate)
                {
                    SoundEffectInstance gunSound = SoundManager.gun.CreateInstance();
                    gunSound.Pitch = (float)(Config.rand.NextDouble() / 2.0f) - .5f;
                    gunSound.Play();
                }
                if (!automate)
                {
                    if (bullets == 0)
                    {
                        reloadTimer = reloadTime;
                        SoundEffectInstance reloadSound = SoundManager.reload.CreateInstance();
                        reloadSound.Play();
                    }
                }
                automateBullet = 0.25f;
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
            if (!ReadyToThrow && !onLadder) 
            {
                shakeSpeed += 25;
                if (shakeSpeed > 100)
                { shakeSpeed = 100; }
                babyLife += keyshakePower;
                if (babyLife > maxBabyLife)
                { babyLife = maxBabyLife; }
            }
        }

        public void StartShake()
        {
            if (!ReadyToThrow && !onLadder)
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
        }

        public void Shake(float angle)
        {
            if (!ReadyToThrow && !onLadder)
            {
                shaking = true;
                float dAngle = angle - prevAngle;
                testAnim.Play("shake");


                shakeSpeed = Math.Abs(dAngle * 40);

                if (shakeSpeed > 100)
                { shakeSpeed = 100; }

                testAnim.SetSpeed(dAngle * 120);
                prevAngle = angle;

                babyLife += joyshakePower * Math.Abs(dAngle);
                if (babyLife > maxBabyLife)
                { babyLife = maxBabyLife; }
            }
        }

        public void AutomatePlayer(float dt)
        {
            automateShake += dt / 60;
            automateLeft = false;
            automateRight = false;
            lives = 3;

            automateDirectionChange += dt / 1000;

            if (automateDirectionChange > 4)
            {
                automateDirectionChange = 0;
                automateDirection = Config.rand.Next(2) == 0 ? true : false;
            }

            if (automateLadderTimer > 0)
            {
                automateLadderTimer -= dt / 1000;
            }
            else
            {
                automateLadderTimer = 0;
            }

            Rectangle senseEnemyRight = new Rectangle(hitRect.Right, hitRect.Top,
                hitRect.Width * 20, hitRect.Width);
            Rectangle senseEnemyLeft = new Rectangle(hitRect.Left - (hitRect.Width * 20), hitRect.Top,
                hitRect.Width * 20, hitRect.Width);

            if (Baby != null)
            {
                if (hitRect.Center.X > Baby.HitRect.Right)
                {
                    automateLeft = true;
                    facingRight = false;
                }
                else if (hitRect.Center.X < Baby.HitRect.Left)
                {
                    automateRight = true;
                    facingRight = true;
                }

                foreach (Enemy e in w.EnemyManager.Enemies)
                {
                    if (automateBullet == 0)
                    {
                        if (e.HitRect.Intersects(senseEnemyRight))
                        {
                            facingRight = true;
                            Shoot();
                        }
                        else if (e.HitRect.Intersects(senseEnemyLeft))
                        {
                            facingRight = false;
                            Shoot();
                        }
                    }
                }
            }
            else
            {
                if (onLadder)
                {

                }
                else
                {
                    foreach (Enemy e in w.EnemyManager.Enemies)
                    {
                        if (!onLadder)
                        {
                            if (e.HitRect.Intersects(senseEnemyRight))
                            {
                                Throw();
                                break;
                            }
                            else if (e.HitRect.Intersects(senseEnemyLeft))
                            {
                                Throw();
                                break;
                            }
                        }
                    }

                    if (babyLife < maxBabyLife / 2
                        && !automateStartShake)
                    {
                        automateStartShake = true;
                    }
                    else if (babyLife > (maxBabyLife / 3) * 2 &&
                        automateStartShake)
                    {
                        automateStartShake = false;
                    }
                    else if (automateStartShake)
                    {
                        Shake((float)Math.Sin(automateShake));
                    }
                    else
                    {
                        shaking = false;
                        if (automateLadderTimer == 0)
                        {
                            if (automateUpLadder)
                            {
                                onLadder = true;
                                velocity.X = 0;
                                velocity.Y = 0;
                                testAnim.Play("climb");
                                automateLadderTimer = 2;
                            }
                            else if (automateDownLadder)
                            {
                                onLadder = true;
                                velocity.X = 0;
                                velocity.Y = 0;
                                testAnim.Play("climb");
                                automateLadderTimer = 2;
                            }
                            else
                            {
                                automateRight = automateDirection;
                                automateLeft = !automateDirection;
                                facingRight = automateDirection;
                            }
                        }
                        else
                        {
                            automateRight = automateDirection;
                            facingRight = automateDirection;
                            automateLeft = !automateDirection;
                        }
                    }
                }
            }
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

            if (automate)
            {
                AutomatePlayer(dt);

                if (directed)
                {
                    if (pos.X < automatePos - width)
                    {
                        xInput = 1;
                    }
                    else if (pos.X > automatePos + width)
                    {
                        xInput = -1;
                    }
                }

                if (wallLeft)
                {
                    automateLeft = false;
                    automateRight = true;
                }
                if (wallRight)
                {
                    automateLeft = true;
                    automateRight = false;
                }
            }

            if (automateLeft)
            {
                xInput = -1;
            }
            else if (automateRight)
            {
                xInput = 1;
            }

            if (onLadder)
            {
                pos.X = Vector2.Lerp(
                    new Vector2(pos.X, 0),
                    new Vector2(ladderSnapX - (width/2), 0),
                    0.2f).X;

                if (ladderSound.State != SoundState.Playing &&
                    Math.Abs(velocity.Y) > 3 && !automate)
                {
                    ladderSound.Pitch = (float)Config.rand.NextDouble() - 0.5f;
                    ladderSound.Volume = 0.1f;
                    ladderSound.Play();
                }

                if (automate)
                {
                    if (automateUpLadder)
                    {
                        yInput = 1;
                    }
                    else if (automateDownLadder)
                    {
                        yInput = -1;
                    }
                }
            }

            if (onLadder || readyToThrow || recoilTimer > 0)
            {
                xInput = 0;
                if (onLadder && testAnim.CurrentAnimation == "climb")
                {
                    testAnim.SetSpeed(velocity.Y / 13);
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

            float speedMod = 1;
            float accMod = 1;
            if (Powerup is SpeedShoes)
            {
                speedMod = 3f;
                accMod = 1.5f;
                if (!automate)
                {
                    if (Math.Abs(velocity.X) > 10 && !InAir)
                    {
                        if (!automate)
                        {
                            if (runSound.State != SoundState.Playing)
                            {
                                runSound.Play();
                            }
                        }
                    }
                    else
                    {
                        runSound.Stop();
                    }
                }
            }

            float xMax = Math.Abs(maxSpeed * xInput * speedMod);
            float yMax = Math.Abs(ladderSpeed * yInput);

            velocity.X += xInput * acceleration * accMod;
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
                    if (testAnim.CurrentAnimation != "jump" &&
                        testAnim.CurrentAnimation != "shoot" &&
                        testAnim.CurrentAnimation != "fall" &&
                        testAnim.CurrentAnimation != "shake")
                    {
                        if (Input.HoldingPrimary(index) &&
                            holdingBaby)
                        {
                            testAnim.Play("throw");
                        }
                        else
                        {
                            testAnim.Play("idle");
                        }
                    }
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
                    if (MenuSystem.gameType == GameType.survival ||
                        MenuSystem.gameType == GameType.hotPotato)
                    {
                        w.Players[0].score += 100;
                    }
                    else
                    {
                        score += 100;
                    }

                    coins.Remove(coin);
                    for (int i = 0; i < Coin.numParticles; i++)
                    {
                        GoldParticle gp = new GoldParticle(new Vector2(
                            coin.Rect.Center.X, coin.Rect.Center.Y));
                        World.ParticleManager.AddParticle(gp);
                    }
                    if (!automate)
                    {
                        SoundEffectInstance coinSound = SoundManager.coin.CreateInstance();
                        coinSound.Play();
                    }
                }
            }
        }

        public void Collision(ref List<Tile> tiles, ref List<Ladder> ladders)
        {
            CoinCollision();
            if (!holdingBaby)
            {
                if (Baby != null)
                {
                    Baby.Collision(ref tiles);
                    if (Baby != null)
                    {
                        if (Baby.Rect.Intersects(rect)
                            && Baby.ReadyToCatch)
                        {
                            Baby = null;
                            holdingBaby = true;

                            if (!automate)
                            {
                                SoundEffectInstance babysfx = SoundManager.catchBaby.CreateInstance();
                                babysfx.Pitch = (float)Config.rand.NextDouble() - 0.5f;
                                babysfx.Play();
                            }
                        }
                    }
                }
                
                foreach (KeyValuePair<Player, Baby> b in w.Babies)
                {
                    if (b.Value != Baby)
                    {
                        if (b.Value != null)
                        {
                            if (b.Value.Rect.Intersects(hitRect)
                                && b.Value.ReadyToCatch)
                            {
                                Baby temp = Baby;
                                Baby = b.Value;
                                babyLife = Baby.Life;
                                w.Babies[b.Key] = temp;

                                if (!automate)
                                {
                                    SoundEffectInstance babysfx = SoundManager.catchBaby.CreateInstance();
                                    babysfx.Pitch = (float)Config.rand.NextDouble() - 0.5f;
                                    babysfx.Play();
                                }

                                break;
                            }
                        }
                    }
                }
                
            }
            if (alive)
            {
                foreach (Enemy e in World.EnemyManager.Enemies)
                {
                    if (e.HitRect.Intersects(hitRect))
                    {
                        Die();
                        GameOver.death = DeathType.zombie;
                        Texture2D t;

                        switch (index)
                        {
                            case 0:
                                t = TextureManager.playerDead;
                                break;
                            case 1:
                                t = TextureManager.playerDeadSanta;
                                break;
                            case 2:
                                t = TextureManager.playerDeadBum;
                                break;
                            default:
                                t = TextureManager.playerDeadPedo;
                                break;
                        }

                        w.ParticleManager.AddParticle(new PlayerDeathParticle(new Vector2(
                            hitRect.Center.X, hitRect.Center.Y), t));
                    }
                }
            }

            bool fall = true;
            bool tileCollideLeft = false;
            bool tileCollideRight = false;
            

            // ladders
            if (!onLadder)
            {
                if (!Shaking)
                {
                    automateDownLadder = false;
                    automateUpLadder = false;
                    //if (Input.GetThumbs(index).Left.Y > 0.2)

                    foreach (Ladder l in ladders)
                    {
                        if (l.LadderAbove(hitRect))
                        {
                            if (!onLadder && !Shaking)
                            {
                                if (Input.GetThumbs(index).Y > ladderThumbSensitivity)
                                {
                                    onLadder = true;
                                    velocity.X = 0;
                                    velocity.Y = 0;
                                    testAnim.Play("climb");
                                }
                                ladderSnapX = l.Rect.Center.X;
                                automateUpLadder = true;
                            }
                        }
                    }

                    //else if (Input.GetThumbs(index).Left.Y < -0.2)

                    foreach (Ladder l in ladders)
                    {
                        if (l.LadderBelow(hitRect))
                        {
                            if (!onLadder && !Shaking)
                            {
                                if (Input.GetThumbs(index).Y < -ladderThumbSensitivity)
                                {
                                    onLadder = true;
                                    velocity.X = 0;
                                    velocity.Y = 0;
                                    testAnim.Play("climb");
                                }
                                ladderSnapX = l.Rect.Center.X;
                                automateDownLadder = true;
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
                if (!stillOnLadder)
                {
                    automateDownLadder = false;
                    automateUpLadder = false;
                }
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
                            automateDownLadder = false;
                            automateUpLadder = false;
                            testAnim.Play("idle");
                        }
                    }
                }
                if (inAir && !onLadder)
                {
                    int overlappingAbove = t.OverlappingAbove(hitRect);
                    if (overlappingAbove > 0 && velocity.Y > 0 &&
                        hitRect.Bottom < t.Rect.Center.Y + (velocity.Y / 50))
                    {
                        inAir = false;
                        velocity.Y = 0;
                        if (!onLadder)
                        {
                            testAnim.Play("idle");
                            pos.Y -= overlappingAbove;
                        }
                        onLadder = false;
                        automateDownLadder = false;
                        automateUpLadder = false;
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
                    if (Input.GetThumbs(index).X < 0 || automate)
                    {
                        
                        int overlappingRight = t.OverlappingRight(hitRect);
                        if (overlappingRight > 0)
                        {
                            bool through = false;
                            foreach (Tile t2 in tiles)
                            {
                                if (t.HasBlockToTheRight(t2.Rect)
                                    && t != t2)
                                {
                                    through = true;
                                }
                            }
                            if (!through)
                            {
                                velocity.X = 0;

                                if (!wallLeft)
                                {
                                    pos.X = t.Rect.Right - hitRect.Width;
                                    tileCollideLeft = true;
                                }
                            }
                        }
                    }
                    if (Input.GetThumbs(index).X > 0 || automate)
                    {
                        int overlappingLeft = t.OverlappingLeft(hitRect);
                        if (overlappingLeft > 0)
                        {
                            bool through = false;
                            foreach (Tile t2 in tiles)
                            {
                                if (t.HasBlockToTheLeft(t2.Rect)
                                    && t != t2)
                                {
                                    through = true;
                                }
                            }
                            if (!through)
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
            }

            if (fall)
            {
                if (!inAir && currentPowerup is SpeedShoes)
                {
                    velocity.Y = -(jumpStrength / 4);
                }
                inAir = true;
            }
            wallRight = tileCollideRight;
            wallLeft = tileCollideLeft;
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            // s = d / t
            int spawnMax = Config.screenW / 15;
            float timeToLaunch = 0.35f;
            float speedToLaunch = (spawnPoint.Y) / timeToLaunch;
            if (respawnTimer > 0 && respawnTimer < respawnTime - 2f)
            {
                spawnWidth = Vector2.Lerp(new Vector2(spawnWidth, 0), new Vector2(spawnMax, 0), 0.2f).X;

                if (respawnTimer < timeToLaunch && lives > 0)
                {
                    sb.Draw(TextureManager.blankTexture, new Rectangle(
                         (int)(spawnPoint.X - (0)),
                         (int)(speedToLaunch * (timeToLaunch - respawnTimer)),
                         width / 2, height), Color.White);
                   
                }
                else if (lives > 0)
                {
                    if (!automate)
                    {
                        SoundManager.Play(SoundManager.iSpawn);
                    }
                }
            }
            else
            {
                spawnWidth = Vector2.Lerp(new Vector2(spawnWidth, 0), new Vector2(0, 0), 0.5f).X;
            }

            int a = 10;
            if (spawnMax > 0 && lives > 0)
            {
                Color c = new Color(200, 200, 75, a);

                if (MenuSystem.gameType != GameType.single)
                {
                    switch (index)
                    {
                        case 0:
                            c = new Color(75, 200, 75, a);
                            break;
                        case 1:
                            c = new Color(200, 75, 75, a);
                            break;
                        case 2:
                            c = new Color(75, 75, 200, a);
                            break;
                        case 3:
                            c = new Color(75, 200, 200, a);
                            break;
                    }
                }

                sb.Draw(TextureManager.blankTexture, new Rectangle(
                      (int)(spawnPoint.X - (spawnWidth / 2)) + (Player.width / 2), 0,
                      (int)spawnWidth, (int)(spawnPoint.Y + height)),
                      c);
            }
            if (alive)
            {
                if (currentPowerup is SpeedShoes)
                {
                    float dir = 0;
                    if(velocity.X > 0)
                    { dir = 1; } 
                    else if(velocity.X < 0)
                    { dir = -1; }

                    if (dir != 0)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            w.ParticleManager.AddParticle(new SparkParticle(new Vector2(
                                hitRect.Center.X, hitRect.Bottom), dir));
                        }
                    }
                }

                Color c = new Color(0, 50, 0, 20);
                switch (index)
                {
                    case 1:
                        c = new Color(50, 0, 0, 20);
                        break;
                    case 2:
                        c = new Color(0, 0, 50, 20);
                        break;
                    case 3:
                        c = new Color(0, 50, 50, 20);
                        break;
                }

                if (currentPowerup is Pacifier)
                {
                    c = new Color(25, 25, 25, 100);
                }

                if (Baby != null)
                {
                    Baby.Draw(sb, SpriteEffects.None);
                    if (!World.Exploding && !hide)
                    {
                        LineBatch.DrawCircle(sb, new Vector2(Baby.Rect.Center.X, Baby.Rect.Center.Y), 
                            (int)(babyLife * 2 * Config.screenR), c);
                    }
                }
                else
                {
                    if (!World.Exploding && !hide)
                    {
                        if (holdingBaby)
                        {
                            LineBatch.DrawCircle(sb, new Vector2(hitRect.Center.X, hitRect.Center.Y),
                                (int)(babyLife * 2 * Config.screenR), c);

                            if (ReadyToThrow)
                            {
                                LineBatch.DrawArc(sb, Input.GetThumbs(index).X, 
                                    new Vector2(hitRect.Center.X, hitRect.Center.Y), Color.Black);
                            }
                        }
                    }
                }

                if (reloadTimer > 0)
                {
                    int w = (int)((TextureManager.reloading.Width) * Config.screenR);
                    int h = (int)((TextureManager.reloading.Height) * Config.screenR);
                    sb.Draw(TextureManager.reloading, new Rectangle(
                        hitRect.Center.X - (w / 2),
                        hitRect.Bottom - h, w, h), Color.White);
                }
            }

            effect = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            //sb.Draw(TextureManager.blankTexture, rect, Color.Green);
            if (!World.Exploding && !hide)
            {
                base.Draw(sb, effect);
            }
        }
    }
}