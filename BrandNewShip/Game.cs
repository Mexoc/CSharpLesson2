using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace BrandNewShip
{
    static class Game
    {
        public static BufferedGraphics buffer;
        public static BufferedGraphicsContext context;
        public static Graphics g;        
        private static int width;
        private static int height;
        private static Random rnd = new Random();
        private static BaseObject[] _objs;
        private static Bullet _bullet;
        private static Asteroid[] _asteroids;
        private static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(10, 10));

        public static int Width
        {
            get { return width; }
            set { width = value; }
        }

        public static int Height
        {
            get { return height; }
            set { height = value; }
        }

        public static void Init(Form form)
        {            
            context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();            
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            if (Width > 984 || Height > 962 || Width < 132 || Height < 38) throw new ArgumentOutOfRangeException("Размер превышен");
            form.KeyDown += Form_KeyDown;
            Timer timer1 = new Timer { Interval = 100 };            
            timer1.Start();
            timer1.Tick += Timer_Tick;
            
        }      
        
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) _bullet = new Bullet(new Point(_ship.rect.X + 10, _ship.rect.Y + 4), new Point(4, 0), new Size(4, 1));
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }

        //рисуем форму и закрашиваем её черным
        public static void DrawForm()
        {
            buffer = context.Allocate(g, new Rectangle(0, 0, Width, Height));
            buffer.Graphics.Clear(Color.Black);
        }

        //загружаем астероиды, звезды и пулю
        public static void Load()
        {
            _objs = new BaseObject[200];
            _bullet = new Bullet(new Point(0, 200), new Point(10, 0), new Size(4, 1));            
            _asteroids = new Asteroid[200];
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _objs[i] = new Star(new Point(70,70), new Point(-r, r), new Size(3, 3));                
                _objs[i] = new Star(new Point(rnd.Next(Game.Width, 1000), rnd.Next(Game.Height, 1000)), new Point(-r, r), new Size(3, 3));                
            }
            for (var i = 0; i < _asteroids.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _asteroids[i] = new Asteroid(new Point(rnd.Next(Game.Width, 1000), rnd.Next(Game.Height, 1000)), new Point(-r / 5, r), new Size(r, r)); 
            }            
        }
        
        //Рисуем астероиды, пули и звезды
        public static void Draw()
        {
            _bullet.Draw();
            foreach (Asteroid obj in _asteroids)
                obj.Draw();
            foreach (Star obj in _objs)
                obj.Draw();
            _ship.Draw();
        }   

        public static void Update()
        {
            foreach (Star obj in _objs)
                obj.Update();
            foreach (BaseObject obj in _asteroids)
            {
                obj.Update();
                if (obj.Collision(_bullet)) obj.Pos = new Point(rnd.Next(1000), rnd.Next(1000));
            }
                
            _bullet.Update();          
        }

        //таймер изменения состояний
        public static void Timer_Tick(object sender, EventArgs e)
        {
            
            DrawForm();
            Draw();
            Update();   
            buffer.Render();
            buffer.Dispose();
        }        
    }
}
