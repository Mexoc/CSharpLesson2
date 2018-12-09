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
            Timer timer1 = new Timer { Interval = 100 };            
            timer1.Start();
            timer1.Tick += Timer_Tick;
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
        }

        //рисуем астероиды на форме
        public static void AsteroidUpdate()
        {
            foreach (BaseObject obj in _asteroids)
                obj.Update();
        }        

        //метод движения пули
        public static void BulletUpdate()
        {
            _bullet.Update();
        }

        //метод движения звезд
        public static void StarUpdate()
        {
            foreach (Star obj in _objs)
                obj.Update();
        }

        //Проверка столкновения пули и астероида, а также генерация нового астероида в случае столкновения
        static public void BulletCollision()
        {
            foreach (Asteroid a in _asteroids)
            {
                Rectangle rect = new Rectangle(a.Pos.X, a.Pos.Y, a.Size.Height,a.Size.Width);
                Rectangle bul = new Rectangle(_bullet.Pos.X, _bullet.Pos.Y, 4,1);
                if (rect.IntersectsWith(bul)) a.Pos = a.Pos = new Point(rnd.Next(1000), rnd.Next(1000)); 
            }
        }

        //таймер изменения состояний
        public static void Timer_Tick(object sender, EventArgs e)
        {
            
            DrawForm();
            Draw();
            AsteroidUpdate();
            BulletUpdate();
            BulletCollision();
            StarUpdate();    
            buffer.Render();
            buffer.Dispose();
        }        
    }
}
