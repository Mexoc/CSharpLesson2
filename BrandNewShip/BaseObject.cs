using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace BrandNewShip
{
    abstract class BaseObject
    {
        public Point Pos; //координаты начальные
        public Point Dir;   // направление движения       
        public Size Size; //размер объекта

        protected BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }

        public abstract void Draw();        

        virtual public void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;            
        }        
    }
}
