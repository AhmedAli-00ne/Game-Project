using System.Security.Cryptography.X509Certificates;

namespace project_1
{
    public class BGLayers
    {
        public Bitmap Img;
        public int X;
        public int NextX;
        public bool StartNext;
        public BGLayers(Bitmap img, int x, bool Next)
        {
            Img = img;
            X = x;
            NextX = x;
            StartNext = Next;
        }
    }
    public class PlayerSprite
    {
        public Bitmap Img;
        public int X;
        public int Y;
        public int ImageNo = 1;
        public bool super = false;
        public int SuperTimer = 0;
        public PlayerSprite(Bitmap img, int x, int y)
        {
            Img = img;
            X = x;
            Y = y;
        }
    }
    public class Bullet
    {
        public Bitmap Img;
        public int X;
        public int Y;
        public int CoolDown;
        public Bullet(Bitmap img, int x, int y, int C)
        {
            Img = img;
            X = x;
            Y = y;
            CoolDown = C;
        }
    }
    public class Sprite
    {
        public Bitmap Img;
        public int X;
        public int Y;
        public int Health;
        public string Type;
        public int ImageNo = 1;
        public Sprite(Bitmap img, int x, int y, string T, int H)
        {
            Img = img;
            X = x;
            Y = y;
            Type = T;
            Health = H;
        }
    }
    public partial class Form1 : Form
    {
        Bitmap offImage;
        System.Windows.Forms.Timer T = new System.Windows.Forms.Timer();
        Random R = new Random();
        BGLayers Layer1 = new BGLayers(new Bitmap("layer1.png"), 0, false);
        BGLayers Layer2 = new BGLayers(new Bitmap("layer2.png"), 0, false);
        BGLayers Layer3 = new BGLayers(new Bitmap("layer3.png"), 0, false);
        BGLayers Layer4 = new BGLayers(new Bitmap("layer4.png"), 0, false);
        PlayerSprite Player = new PlayerSprite(new Bitmap("player/player (1).png"), 0, 0);
        List<Bitmap> PlayerImages = new List<Bitmap>();
        List<Bitmap> PlayerSuperImages = new List<Bitmap>();
        List<Bitmap> HiveWhaleImages = new List<Bitmap>();
        List<Bitmap> Lucky1 = new List<Bitmap>();
        List<Bitmap> Lucky2 = new List<Bitmap>();
        List<Bitmap> SmokeImages = new List<Bitmap>();
        List<Bitmap> FireImages = new List<Bitmap>();
        List<Bullet> Bullets = new List<Bullet>();
        List<Bullet> TailBullets = new List<Bullet>();
        List<Sprite> ActiveSprites = new List<Sprite>();
        
        int HiveWhaleTickCounter = 0;
        int HiveWhaleTickObjective = 0;
        int LuckyTickCounter = 0;
        int LuckyTickObjective = 0;
        bool HiveWhaleGenerateRand = true;
        bool LuckyGenerateRand = true;
        
        bool BulletOk = true;
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            T.Tick += T_Tick;
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up && Player.Y>10)
            {
                Player.Y -= 10;
            }
            if (e.KeyCode == Keys.Down && Player.Y < this.ClientSize.Height - Player.Img.Height)
            {
                Player.Y += 10;
            }
            if(e.KeyCode == Keys.Space && BulletOk)
            {
                Bullets.Add(new Bullet(new Bitmap("projectile.png"), Player.Img.Width - 25, Player.Y + 25, 3));
                BulletOk = false;
                if (Player.super)
                {
                    TailBullets.Add(new Bullet(new Bitmap("projectile.png"), Player.Img.Width -25, Player.Y + Player.Img.Height-20, 3));
                }
            }
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            DoubleBuffer(e.Graphics);
        }

        private void T_Tick(object? sender, EventArgs e)
        {
            ProjectileCollision();
            GenerateNumber();
            IncrementCounter();
            AnimatePlayer();
            AnimateSprites();
            MoveLayers();
            MoveBullet();
            MoveSprites();
            PreventSpam();
            DoubleBuffer(this.CreateGraphics());
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            offImage = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            for(int i=1;i<=39;i++)
            {
                PlayerImages.Add(new Bitmap("player/player (" + i + ").png"));
            }
            for (int i = 1; i <= 39; i++)
            {
                PlayerSuperImages.Add(new Bitmap("player/playerEvolve (" + i + ").png"));
            }
            for (int i=1;i<=39;i++)
            {
                HiveWhaleImages.Add(new Bitmap("hiveWhale/hiveWhale (" + i + ").png"));
            }
            for (int i = 1; i <= 39; i++)
            {
                Lucky1.Add(new Bitmap("lucky/lucky1 (" + i + ").png"));
            }
            for (int i = 1; i <= 39; i++)
            {
                Lucky2.Add(new Bitmap("lucky/lucky2 (" + i + ").png"));
            }
            for (int i = 1; i <= 8; i++)
            {
                SmokeImages.Add(new Bitmap("smoke"+ i.ToString() +".png"));
            }
            for (int i = 1; i <= 8; i++)
            {
                FireImages.Add(new Bitmap("fire/fire (" + i.ToString() + ").png"));
            }
            T.Interval = 1;
            T.Start();
        }

        void DoubleBuffer(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(offImage);
            DrawScene(g2);
            g.DrawImage(offImage, 0, 0);
        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            g.DrawImage(Layer1.Img, Layer1.X, 0);
            g.DrawImage(Layer1.Img, Layer1.NextX, 0);
            g.DrawImage(Layer2.Img, Layer2.X, 0);
            g.DrawImage(Layer2.Img, Layer2.NextX, 0);
            g.DrawImage(Layer3.Img, Layer3.X, 0);
            g.DrawImage(Layer3.Img, Layer3.NextX, 0);
            g.DrawImage(Layer4.Img, Layer3.X, 0);
            g.DrawImage(Layer4.Img, Layer3.NextX, 0);
            g.DrawImage(Player.Img, Player.X, Player.Y);
            for (int i = 0; i < Bullets.Count; i++)
            {
                g.DrawImage(Bullets[i].Img, Bullets[i].X, Bullets[i].Y);
                if (Bullets[i].X > this.ClientSize.Width)
                {
                    Bullets.RemoveAt(i);
                }
            }
            for (int i = 0; i < TailBullets.Count; i++)
            {
                g.DrawImage(TailBullets[i].Img, TailBullets[i].X, TailBullets[i].Y);
                if (TailBullets[i].X > this.ClientSize.Width)
                {
                    TailBullets.RemoveAt(i);
                }
            }
            for (int i=0;i<ActiveSprites.Count;i++)
            {
                g.DrawImage(ActiveSprites[i].Img, ActiveSprites[i].X, ActiveSprites[i].Y);
            }
        }
        void AnimateSpriteDeath(int pos)
        {
            if (ActiveSprites[pos].Type == "hiveWhale")
            {
                int DeathX = ActiveSprites[pos].X;
                int DeathY = ActiveSprites[pos].Y;
                ActiveSprites.RemoveAt(pos);
                ActiveSprites.Insert(pos, new Sprite(new Bitmap("smoke1.png"), DeathX, DeathY, "Smoke", 200));
            }
            else if (ActiveSprites[pos].Type == "lucky1" || ActiveSprites[pos].Type == "lucky2")
            {
                int DeathX = ActiveSprites[pos].X;
                int DeathY = ActiveSprites[pos].Y;
                ActiveSprites.RemoveAt(pos);
                ActiveSprites.Insert(pos, new Sprite(new Bitmap("fire/fire (1).png"), DeathX, DeathY, "Fire", 200));
                Player.super = true;
                Player.SuperTimer += 500;
            }
        }
        void ProjectileCollision()
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                for (int j = 0; j < ActiveSprites.Count; j++)
                {
                    if (Bullets[i].X + Bullets[i].Img.Width > ActiveSprites[j].X && Bullets[i].X < ActiveSprites[j].X + ActiveSprites[j].Img.Width && Bullets[i].Y + Bullets[i].Img.Height > ActiveSprites[j].Y && Bullets[i].Y < ActiveSprites[j].Y + ActiveSprites[j].Img.Height)
                    {
                        ActiveSprites[j].Health -= 1;
                        Bullets.RemoveAt(i);
                        if (i > Bullets.Count - 1)
                        {
                            break;
                        }
                        if (ActiveSprites[j].Health <= 0)
                        {
                            AnimateSpriteDeath(j);
                        }
                    }
                }
            }
            for (int i = 0; i < TailBullets.Count; i++)
            {
                for (int j = 0; j < ActiveSprites.Count; j++)
                {
                    if (TailBullets[i].X + TailBullets[i].Img.Width > ActiveSprites[j].X && TailBullets[i].X < ActiveSprites[j].X + ActiveSprites[j].Img.Width && TailBullets[i].Y + TailBullets[i].Img.Height > ActiveSprites[j].Y && TailBullets[i].Y < ActiveSprites[j].Y + ActiveSprites[j].Img.Height)
                    {
                        ActiveSprites[j].Health -= 1;
                        TailBullets.RemoveAt(i);
                        if (i > TailBullets.Count - 1)
                        {
                            break;
                        }
                        if (ActiveSprites[j].Health <= 0)
                        {
                            AnimateSpriteDeath(j);
                        }
                    }
                }
            }
        }
        void AnimateSprites()
        {
            for (int i = 0; i < ActiveSprites.Count; i++)
            {
                if (ActiveSprites[i].Type == "hiveWhale")
                {
                    if (ActiveSprites[i].ImageNo == 39)
                    {
                        ActiveSprites[i].ImageNo = 1;
                    }
                    else
                    {
                        ActiveSprites[i].ImageNo++;
                    }
                    ActiveSprites[i].Img = HiveWhaleImages[ActiveSprites[i].ImageNo - 1];
                }
                else if (ActiveSprites[i].Type == "lucky1")
                {
                    if (ActiveSprites[i].ImageNo == 39)
                    {
                        ActiveSprites[i].ImageNo = 1;
                    }
                    else
                    {
                        ActiveSprites[i].ImageNo++;
                    }
                    ActiveSprites[i].Img = Lucky1[ActiveSprites[i].ImageNo - 1];
                }
                else if (ActiveSprites[i].Type == "lucky2")
                {
                    if (ActiveSprites[i].ImageNo == 39)
                    {
                        ActiveSprites[i].ImageNo = 1;
                    }
                    else
                    {
                        ActiveSprites[i].ImageNo++;
                    }
                    ActiveSprites[i].Img = Lucky2[ActiveSprites[i].ImageNo - 1];
                }
                else if (ActiveSprites[i].Type == "Smoke")
                {
                    if (ActiveSprites[i].ImageNo == 8)
                    {
                        ActiveSprites.RemoveAt(i);
                        continue;
                    }
                    else
                    {
                        ActiveSprites[i].ImageNo++;
                        ActiveSprites[i].Img = SmokeImages[ActiveSprites[i].ImageNo - 1];
                    }
                }
                else if (ActiveSprites[i].Type == "Fire")
                {
                    if (ActiveSprites[i].ImageNo == 8)
                    {
                        ActiveSprites.RemoveAt(i);
                        continue;
                    }
                    else
                    {
                        ActiveSprites[i].ImageNo++;
                        ActiveSprites[i].Img = FireImages[ActiveSprites[i].ImageNo - 1];
                    }
                }
            }
        }
        void MoveSprites()
        {
            for(int i=0;i<ActiveSprites.Count;i++)
            {
                ActiveSprites[i].X -= 2;
                if (ActiveSprites[i].X < -ActiveSprites[i].Img.Width)
                {
                    ActiveSprites.RemoveAt(i);
                }
            }
        }
        void SpawnSprite(string Type)
        {
            if (Type == "hiveWhale")
            {
                ActiveSprites.Add(new Sprite(new Bitmap(Type + "/" + Type + " (1).png"), this.ClientSize.Width, R.Next(30, this.ClientSize.Height - 100 - HiveWhaleImages[0].Height), Type, 20));
            }
            else if (Type == "lucky1")
            {
                ActiveSprites.Add(new Sprite(new Bitmap("lucky/" + Type + " (1).png"), this.ClientSize.Width, R.Next(30, this.ClientSize.Height - 100 - Lucky1[0].Height), Type, 1));
            }
            else if (Type == "lucky2")
            {
                ActiveSprites.Add(new Sprite(new Bitmap("lucky/" + Type + " (1).png"), this.ClientSize.Width, R.Next(30, this.ClientSize.Height - 100 - Lucky2[0].Height), Type, 1));
            }
        }
        void IncrementCounter()
        {
            HiveWhaleTickCounter++;
            if (HiveWhaleTickCounter == HiveWhaleTickObjective)
            {
                HiveWhaleGenerateRand = true;
                HiveWhaleTickCounter = 0;
                SpawnSprite("hiveWhale");
            }
            LuckyTickCounter++;
            if (LuckyTickCounter == LuckyTickObjective)
            {
                LuckyGenerateRand = true;
                LuckyTickCounter = 0;
                SpawnSprite("lucky"+R.Next(1,2).ToString());
            }
        }
        void GenerateNumber()
        {
            if (HiveWhaleGenerateRand)
            {
                HiveWhaleTickObjective = R.Next(0, 500);
                HiveWhaleGenerateRand = false;
            }
            if(LuckyGenerateRand)
            {
                LuckyTickObjective = R.Next(0, 500);
                LuckyGenerateRand = false;
            }
        }
        void AnimatePlayer()
        {
            if (Player.ImageNo == 38)
            {
                Player.ImageNo = 0;
            }
            else
            {
                Player.ImageNo++;
            }
            if (Player.super)
            {
                Player.Img = PlayerSuperImages[Player.ImageNo];
            }
            else
            {
                Player.Img = PlayerImages[Player.ImageNo];
            }
            if (Player.super)
            {
                if(Player.SuperTimer > 0)
                {
                    Player.SuperTimer--;
                }
                else
                {
                    Player.super = false;
                }
            }
        }
        void MoveBullet()
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets[i].X += 10;
            }
            for (int i = 0; i < TailBullets.Count; i++)
            {
                TailBullets[i].X += 10;
            }
        }
        void MoveLayers()
        {
            if (Layer3.StartNext)
            {
                if (Layer3.X > -500)
                {
                    Layer3.X -= 7;
                    Layer3.NextX -= 7;
                }
                else
                {
                    Layer3.NextX = Layer3.X + Layer3.Img.Width;
                    Layer3.StartNext = false;
                }
            }
            else
            {
                if (Layer3.NextX > -500)
                {
                    Layer3.NextX -= 5;
                    Layer3.X -= 5;
                }
                else
                {
                    Layer3.X = Layer3.NextX + Layer3.Img.Width;
                    Layer3.StartNext = true;
                }
            }
            if (Layer1.StartNext)
            {
                if (Layer1.X > -950)
                {
                    Layer1.X -= 1;
                    Layer1.NextX -= 1;
                }
                else
                {
                    Layer1.NextX = Layer1.X + Layer1.Img.Width;
                    Layer1.StartNext = false;
                }
            }
            else
            {
                if (Layer1.NextX > -950)
                {
                    Layer1.NextX -= 1;
                    Layer1.X -= 1;
                }
                else
                {
                    Layer1.X = Layer1.NextX + Layer1.Img.Width;
                    Layer1.StartNext = true;
                }
            }
            if (Layer2.StartNext)
            {
                if (Layer2.X < 500)
                {
                    Layer2.X += 1;
                    Layer2.NextX += 1;
                }
                else
                {
                    Layer2.NextX = Layer2.X - Layer2.Img.Width;
                    Layer2.StartNext = false;
                }
            }
            else
            {
                if (Layer2.NextX < 500)
                {
                    Layer2.NextX += 1;
                    Layer2.X += 1;
                }
                else
                {
                    Layer2.X = Layer2.NextX - Layer2.Img.Width;
                    Layer2.StartNext = true;
                }
            }
        }
        void PreventSpam()
        {
            if (Bullets.Count > 0)
            {
                if (Bullets[Bullets.Count-1].CoolDown == 0)
                {
                    BulletOk = true;
                }
                else
                {
                    Bullets[Bullets.Count-1].CoolDown--;
                }
            }
        }
    }
}