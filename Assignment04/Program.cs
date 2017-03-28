using System;
using System.Windows.Forms;

namespace Assignment04
{
    public class Program : Engine
    {

        public static SlideSprite elephant;
        public static SlideSprite[,] goals;
        public static SlideSprite[,] walls;
        public static SlideSprite[,] blocks;
        public static int x;
        public static int y;
        public static int width;
        public static int height;


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                if (canMoveTo(x + 1, y, 1, 0)) x++;
                if (blocks[x, y] != null) moveBlock(x, y, 1, 0);
            }
            if (e.KeyCode == Keys.Left)
            {
                if (canMoveTo(x - 1, y, -1, 0)) x--;
                if (blocks[x, y] != null) moveBlock(x, y, -1, 0);
            }
            if (e.KeyCode == Keys.Up)
            {
                if (canMoveTo(x, y - 1, 0, -1)) y--;
                if (blocks[x, y] != null) moveBlock(x, y, 0, -1);
            }
            if (e.KeyCode == Keys.Down)
            {
                if (canMoveTo(x, y + 1, 0, 1)) y++;
                if (blocks[x, y] != null) moveBlock(x, y, 0, 1);
            }
            elephant.TargetX = x * 100;
            elephant.TargetY = y * 100;

        }

        public void moveBlock(int i, int j, int dx, int dy)
        {
            blocks[i + dx, j + dy] = blocks[i, j];
            blocks[i, j] = null;

            blocks[i + dx, j + dy].TargetX = (i + dx) * 100;
            blocks[i + dx, j + dy].TargetY = (j + dy) * 100;
            if (goals[i + dx, j + dy] != null) blocks[i + dx, j + dy].Image = Properties.Resources.final;
            else blocks[i + dx, j + dy].Image = Properties.Resources.box;

        }

        public Boolean canMoveTo(int i, int j, int dx, int dy)
        {

            if (walls[i, j] == null && blocks[i, j] == null) return true;
            if (walls[i, j] != null) return false;
            if (blocks[i, j] != null && blocks[i + dx, j + dy] == null && walls[i + dx, j + dy] == null) return true;
            return false;

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            fixScale();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            fixScale();
        }

        private void fixScale()
        {
            canvas.Scale = Math.Min(ClientSize.Width, ClientSize.Height) / (Math.Max(height,width)*200.0f);
            //more code here
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            String map = Properties.Resources.level1;
            String[] lines = map.Split('\n');
            width = 8;
            height = 9;
            goals = new SlideSprite[width, height];
            walls = new SlideSprite[width, height];
            blocks = new SlideSprite[width, height];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (lines[j][i] == 'g' || lines[j][i] == 'B')
                    {
                        goals[i, j] = new SlideSprite(Properties.Resources.goal, i * 100, j * 100);
                        Program.canvas.add(goals[i, j]);
                    }
                    if (lines[j][i] == 'w')
                    {
                        walls[i, j] = new SlideSprite(Properties.Resources.wall, i * 100, j * 100);
                        Program.canvas.add(walls[i, j]);
                    }
                    if (lines[j][i] == 'b' || lines[j][i] == 'B')
                    {
                        blocks[i, j] = new SlideSprite(Properties.Resources.box, i * 100, j * 100);
                        if (lines[j][i] == 'B') blocks[i, j].Image = Properties.Resources.final;

                    }
                    if (lines[j][i] == 'c')
                    {
                        elephant = new SlideSprite(Properties.Resources.elephant, i * 100, j * 100);

                        x = i;
                        y = j;

                    }

                }

            }
            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                    if (blocks[i, j] != null) Program.canvas.add(blocks[i, j]);
            Program.canvas.add(elephant);
            Application.Run(new Program());
        }
    }
}