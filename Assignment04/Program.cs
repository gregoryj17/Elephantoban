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
        public static String map;
        public static Box winScreen = new Box(0, 0, 0, 0, 0);
        public static TextSprite winText = new TextSprite(0, 0, "       You Win!\n\nPress \"n\" for a new level.\nPress \"r\" to reset this level.");
        public static String[] levels = new String[] {Properties.Resources.level1, Properties.Resources.level2, Properties.Resources.level3 };
        public static int level = 0;


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
            if (e.KeyCode == Keys.R) {
                Reset();
            }
            if (e.KeyCode == Keys.N)
            {
                NewMap();
            }
            if (e.KeyCode == Keys.P)
            {
                jukebox.PlayLooping();
            }
            if (e.KeyCode == Keys.M)
            {
                jukebox.Stop();
            }
            elephant.TargetX = x * 100;
            elephant.TargetY = y * 100;
            setWinText();

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
            if (isFinished()) return false;
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
            canvas.X = (ClientSize.Width - (200 * width * canvas.Scale)) / 2;
            canvas.Y = (ClientSize.Height - (200 * height * canvas.Scale)) / 2;
        }

        protected Boolean isFinished()
        {
            for(int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (goals[i, j] != null ^ blocks[i, j] != null) return false;
                }
            }
            return true;
        }

        protected void setWinText()
        {
            if (isFinished())
            {
                winScreen.setDimensions(ClientSize.Width, ClientSize.Height);
                winScreen.setOpacity(200);
                winScreen.setVisibility(true);
                winText.changeLocation(ClientSize.Width/2 - 200, ClientSize.Height/2 - 80);
                if (winText.x < 50) winText.x = 50;
                winText.fontResize(ClientSize.Width, ClientSize.Height);
                winText.setVisibility(true);
            }
            else
            {
                winScreen.setVisibility(false);
                winText.setVisibility(false);
            }
            fixScale();
        }

        protected static void Setup()
        {
            map = levels[level];
            String[] lines = map.Split('\n');
            width = lines[0].Length-1;
            height = lines.Length;
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
            winScreen.setVisibility(false);
            winText.setVisibility(false);
            parent.add(winScreen);
            parent.add(winText);
        }

        protected static void Reset()
        {
            canvas.RemoveAll();
            parent.RemoveAll();
            Setup();
        }

        protected static void ChangeMap()
        {
            level = (level + 1) % levels.Length;
        }

        protected static void NewMap()
        {
            ChangeMap();
            Reset();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            map = Properties.Resources.level1;
            Setup();
            Application.Run(new Program());
        }
    }
}