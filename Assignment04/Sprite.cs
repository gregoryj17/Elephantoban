using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment04
{
    public class Sprite
    {
        private Sprite parent = null;

        //instance variable
        private float x = 0;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        private float y = 0;

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        private float scale = 1;

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        private float rotation = 0;

        /// <summary>
        /// The rotation in degrees.
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }


        public List<Sprite> children = new List<Sprite>();


        public void Kill()
        {
            parent.children.Remove(this);
        }

        //methods
        public void render(Graphics g)
        {
            Matrix original = g.Transform.Clone();
            g.TranslateTransform(x, y);
            g.ScaleTransform(scale, scale);
            g.RotateTransform(rotation);
            paint(g);
            foreach (Sprite s in children)
            {
                s.render(g);
            }
            g.Transform = original;
        }

        public void update()
        {
            act();
            foreach (Sprite s in children)
            {
                s.update();
            }
        }

        public virtual void paint(Graphics g)
        {

        }

        public virtual void act()
        {

        }

        public void add(Sprite s)
        {
            s.parent = this;
            children.Add(s);
        }

        public void RemoveAll()
        {
            children = new List<Sprite>();
        }

    }

    public class SlideSprite : Sprite
    {
        public int TargetX = 0;
        public int TargetY = 0;
        public int Velocity = 10;
        public Image Image;

        public SlideSprite(Image img)
        {
            Image = img;
            X = 0;
            Y = 0;
        }

        public SlideSprite(Image img, int X, int Y)
        {
            Image = img;
            this.X = X;
            TargetX = X;
            this.Y = Y;
            TargetY = Y;
        }

        public override void act()
        {
            if (X + Velocity < TargetX)
            {
                X += Velocity;
            }
            else if (X - Velocity > TargetX)
            {
                X -= Velocity;
            }
            else if (Math.Abs(X - TargetX) <= Velocity)
            {
                X = TargetX;
            }
            if (Y + Velocity < TargetY)
            {
                Y += Velocity;
            }
            else if (Y - Velocity > TargetY)
            {
                Y -= Velocity;
            }
            else if (Math.Abs(Y - TargetY) <= Velocity)
            {
                Y = TargetY;
            }

        }

        public override void paint(Graphics g)
        {
            //g.DrawImage(Image, X - (Image.Width / 2), Y - (Image.Height / 2));
            g.DrawImage(Image, X, Y);
        }
    }

    public class TextSprite : Sprite
    {
        public String text;
        public Boolean visible;
        public int x;
        public int y;
        public int font = 24;

        public TextSprite(int x, int y, String text)
        {
            this.x = x;
            this.y = y;
            this.text = text;
            visible = false;
        }

        public void changeLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void setVisibility(Boolean visible)
        {
            this.visible = visible;
        }

        public void fontResize(int width,int height)
        {
            font = (int)(12+(12*(Math.Min(width,height)/705)));
        }

        public override void paint(Graphics g)
        {
            //base.paint(g);
            if (visible) g.DrawString(text, new Font("Comic Sans MS", font), Brushes.Black, x, y);
        }
    }

    public class Box : Sprite
    {
        public int height;
        public int width;
        public Boolean visible;
        public int opacity;

        public Box(int x, int y, int width, int height, int opacity)
        {
            X = x;
            Y = y;
            this.height = height;
            this.width = width;
            this.opacity = opacity;
            visible = false;
        }

        public void setDimensions(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void setOpacity(int opacity)
        {
            this.opacity = opacity;
        }

        public void setVisibility(Boolean visible)
        {
            this.visible = visible;
        }

        public override void paint(Graphics g)
        {
            base.paint(g);
            if (visible) g.FillRectangle(new SolidBrush(Color.FromArgb(opacity, Color.LawnGreen)), X, Y, width, height);
        }
    }

}