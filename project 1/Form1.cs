namespace project_1
{
    public class BGLayers
    {
        public Bitmap Img;
        public int X;
        public int NextX;
        public bool StartNext = false;
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
        public Bullet(Bitmap img, int x, int y)
        {
            Img = img;
            X = x;
            Y = y;
        }
    }
    public partial class Form1 : Form
    {
        Bitmap offImage;
        System.Windows.Forms.Timer T = new System.Windows.Forms.Timer();
        BGLayers Layer1 = new BGLayers(new Bitmap("layer1.png"), 0, false);
        BGLayers Layer2 = new BGLayers(new Bitmap("layer2.png"), 0, false);
        BGLayers Layer3 = new BGLayers(new Bitmap("layer3.png"), 0, false);
        BGLayers Layer4 = new BGLayers(new Bitmap("layer4.png"), 0, false);
        PlayerSprite Player = new PlayerSprite(new Bitmap("player.png"), 0, 0);
        List<Bullet> Bullets = new List<Bullet>();
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
            if(e.KeyCode == Keys.Space)
            {
                Bullets.Add(new Bullet(new Bitmap("projectile.png"), Player.Img.Width - 20, Player.Y + 25));
            }
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            DoubleBuffer(e.Graphics);
        }

        private void T_Tick(object? sender, EventArgs e)
        {
            MoveLayers();
            MoveBullet();
            DoubleBuffer(this.CreateGraphics());
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            offImage = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            Layer1.Img.MakeTransparent();
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
        }
        void MoveBullet()
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets[i].X += 10;
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
                    Layer3.NextX -= 7;
                    Layer3.X -= 7;
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
                    Layer1.X -= 5;
                    Layer1.NextX -= 5;
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
                    Layer1.NextX -= 5;
                    Layer1.X -= 5;
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
                    Layer2.X += 2;
                    Layer2.NextX += 2;
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
                    Layer2.NextX += 2;
                    Layer2.X += 2;
                }
                else
                {
                    Layer2.X = Layer2.NextX - Layer2.Img.Width;
                    Layer2.StartNext = true;
                }
            }
        }
    }
}