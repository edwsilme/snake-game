using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_v1
{
    public partial class Form1 : Form
    {
        int cols = 50, rows = 25, score = 0, dx = 0, dy = 0, front = 0, back = 0;

        Piece[] snake = new Piece[1250];
        List<int> available = new List<int>();
        bool[,] visit;

        Random rnd = new Random();
        Timer tm = new Timer();

        public Form1()
        {
            InitializeComponent();
            intial();
            launchTimer();
        }

        private void launchTimer()
        {
            //throw new NotImplementedException();

            tm.Interval = 100;
            tm.Tick += move;
            tm.Start();

        }

        private void move(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            int x = snake[front].Location.X, y = snake[front].Location.Y;

            if (dx == 0 && dy == 0) return;

            if (game_over(x + dx, y + dy))
            {
                tm.Stop();

                MessageBox.Show("Game Over");
                return;

            }
            
            if (collisionFood(x + dx, y + dy))
            {
                score += 1;
                lblScore.Text = "Score: " + score.ToString();

                if (hits((y + dy) / 20, (x + dx) / 20)) return;

                Piece head = new Piece(x + dx, y + dy);
                front = (front - 1 + 1250) % 1250;
                snake[front] = head;
                visit[head.Location.Y / 20, head.Location.X / 20] = true;
                Controls.Add(head);
                randomFood();
            }
            else
            {
                if (hits((y + dy) / 20, (x + dx) / 20)) return;

                visit[snake[back].Location.Y / 20, snake[back].Location.X / 20] = false;
                front = (front - 1 + 1250) % 1250;
                snake[front] = snake[back];
                snake[front].Location = new Point(x + dx, y + dy);
                back = (back - 1 + 1250) % 1250;
                visit[(y + dy) / 20, (x + dx) / 20] = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            dx = dy = 0;

            switch(e.KeyCode)
            {
                case Keys.Right:
                    dx = 20;
                    break;
                case Keys.Up:
                    dy = -20;
                    break;
                case Keys.Left:
                    dx = -20;
                    break;
                case Keys.Down:
                    dy = 20;
                    break;
            }
        }

        private void randomFood()
        {
            //throw new NotImplementedException();

            available.Clear();

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    if (!visit[i, j]) available.Add(i * cols + j);

            int idx = rnd.Next(available.Count) % available.Count;

            lblFood.Left = (available[idx] * 20) % Width;
            lblFood.Top = (available[idx] * 20) / Width * 20;
        }

        private bool hits(int x, int y)
        {
            //throw new NotImplementedException();

            if (visit[x, y])
            {
                tm.Stop();

                MessageBox.Show("Snake Hit his Body");
                return true;
            }
            return false;
        }

        private bool collisionFood(int x, int y)
        {
            //throw new NotImplementedException();

            return x == lblFood.Location.X && y == lblFood.Location.Y;
        }

        private bool game_over(int x, int y)
        {
            //throw new NotImplementedException();

            return x < 0 || y < 0 || x > 980 || y > 480; 
        }

        private void intial()
        {
            //throw new NotImplementedException();

            visit = new bool[rows, cols];
            Piece head = new Piece((rnd.Next() % cols) * 20, (rnd.Next() % rows) * 20);
            lblFood.Location = new Point((rnd.Next() % cols) * 20, (rnd.Next() % rows) * 20);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j > cols; j++)
                {
                    visit[i, j] = false;
                    available.Add(i * cols + j);
                }

            visit[head.Location.Y / 20, head.Location.X / 20] = true;
            available.Remove(head.Location.Y / 20 * cols + head.Location.X / 20);
            Controls.Add(head);
            snake[front] = head;
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
