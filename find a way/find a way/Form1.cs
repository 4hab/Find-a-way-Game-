using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace find_a_way
{
    public partial class Form1 : Form
    {
        //vars
        node[,] no = new find_a_way.node[10, 10];
        bool start_selected = false;
        char[,] graph = new char[10, 10];
        bool[,] v = new bool[10, 10];
        char[] steps = new char[100];
        int[] stepv = new int[100];
        int n, m;
        Point pan;

        void color(int x,int y,char c)
        {
            no[x, y].BackColor = Color.Red;
            Point path;
            path = pan;
            path.X += (40 * x);
            path.Y += (40 * y);
            int a = 9, b = 30;
            if (c == 'R')
            {
                path.Y += 25;
                path.X += 8;
            }
            else if(c=='L')
            {
                path.X += 8;
                path.Y -= 30;
            }
            else if(c=='U')
            {
                path.Y += 8;
                path.X -= 30;
                int t = a;
                a = b;
                b = t;
            }
            else if(c=='D')
            {
                path.Y += 8;
                path.X += 25;
                int t = a;
                a = b;
                b = t;
            }
            stripe s = new stripe();
            s.SetBounds(path.X, path.Y,a,b);
            s.BackColor = Color.Red;
            this.grid.Controls.Add(s);

        }
        void solve()
        {
            if(!start_selected)
            {
                MessageBox.Show("Select start node");
                return;
            }
            int x=0, y=0;
            for(int i=0;i<n;i++)
            {
                for(int j=0;j<m;j++)
                {
                    v[i, j] = false;
                    stepv[i * m + j] = 0;
                    if (no[i, j].BackColor == Color.Black)
                        graph[i, j] = '#';
                    else if (no[i, j].BackColor == Color.Red)
                    {
                        graph[i, j] = 'O';
                        x = i;
                        y = j;
                    }
                    else
                        graph[i, j] = 'o';
                }
            }
            int c = sol(x, y, 0);
            color(x, y, steps[0]);
            for(int i=0;i<100;i++)
            {
                if (steps[i] == 'R')
                    y++;
                else if (steps[i] == 'L')
                    y--;
                else if (steps[i] == 'U')
                    x--;
                else if (steps[i] == 'D')
                    x++;
                else
                    break;
                if (valid(x, y))
                    color(x, y,steps[i+1]);
                this.Refresh();
                int t = 0;
                while(t<95000000)
                {
                    t += 1;
                }
            }
        }
        int sol(int x,int y,int step)
        {
            if (!valid(x, y) || v[x, y] || graph[x, y] == '#')
                return 0;
            int mx = 0, R = 0;
            char c = 'a';// to store the best direction
            v[x,y] = true;// mark current node as visited
            R = sol(x, y + 1, step + 1);// try first direction (move to right)
            if (R > mx)
            {
                mx = R;
                c = 'R';
            }
            R = sol(x, y - 1, step + 1);// move to left
            if (R > mx)
            {
                mx = R;
                c = 'L';
            }
            R = sol(x - 1, y, step + 1);//move up
            if (R > mx)
            {
                mx = R;
                c = 'U';
            }
            R = sol(x + 1, y, step + 1);// move down
            if (R > mx)
            {
                mx = R;
                c = 'D';
            }// 1st condition check if we've got a direction
             // 2nd condition to check if we have better choice
            if (c != 'a' && mx > stepv[step])
            {
                steps[step] = c;//change direction
                stepv[step] = mx;// change max
            }
            v[x,y] = false;// mark this node as not visited
            return mx + 1;
        }
        bool valid(int x,int y)
        {
            return (x < n && x >= 0 && y < m && y >= 0) ;
        }
        public Form1()
        {
            InitializeComponent();
            

        }
        void click(object sender,EventArgs e)
        {
            if((sender as Button).BackColor==Color.Black)
            {
                (sender as Button).BackColor = Color.Gray;
            }
            else if ((sender as Button).BackColor == Color.Gray&&!start_selected)
            {
                (sender as Button).BackColor = Color.Red;
                start_selected = true;
            }
            else if ((sender as Button).BackColor == Color.Red)
            {
                (sender as Button).BackColor = Color.Black;
                start_selected = false;
            }
            else if(start_selected)
            {
                (sender as Button).BackColor = Color.Black;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            solve();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            grid.Controls.Clear();
            start_selected = false;
            Point p = new Point(0, 0);
            int h = int.Parse(comboBox1.Text);
            int w = int.Parse(comboBox2.Text);
            n = h;
            m = w;
            Point f = new Point(grid.Width,grid.Height);
            f.X -= (h * 40);f.X /= 2;
            f.Y -= (w * 40);f.Y /= 2;
            pan = f;
            p.X += f.X;
            p.Y += f.Y;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    no[i, j] = new node();
                    no[i, j].BackColor = Color.Gray;
                    no[i, j].Click += click;
                    this.grid.Controls.Add(no[i, j]);
                    no[i, j].SetBounds(p.X,p.Y, 25, 25);
                    p.Y += 40;

                }
                p.X += 40;
                p.Y = f.Y;
            }

        }
    }
}
