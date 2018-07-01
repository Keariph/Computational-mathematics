using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Dolbanny_kyrsach
{
    public partial class Form1 : Form
    {
        double a, b, y_0, yy, y_1, h, E;
        public Form1()
        {
            InitializeComponent();
        }
        public List<double> F(double x, List<double> y)//функция
        {
            List<double> Y = new List<double>();
            Y.Add(y[1]);
            Y.Add(((y[1] + Math.Exp(x))/2));
            return Y;
        }
        public List<double> Rangechut(double y0, double y1, double h, int wr)//Рангекут
        {
            List<double> y_0 = new List<double> { y0, y1 };
            List<double> y_05 = new List<double>() { 0, 0 };
            List<double> y = new List<double>() { 0, 0 };
            dataGridView1.Rows.Clear();
            double j;
            for (j = a; j <= b; j += h)
            {
                for (int i = 0; i < y_0.Count; i++)
                {
                    y_05[i] = y_0[i] + ((h / 2) * F(j, y_0)[i]);
                }
                for (int i = 0; i < y_0.Count; i++)
                {
                    y[i] = y_0[i] + (h * F((j + h / 2), y_05)[i]);
                }
                for (int i = 0; i < y_0.Count; i++)
                {
                    y_0[i] = y[i];
                }
                if (wr != 0)
                {
                    dataGridView1.Rows.Add(j, y[0], y[1]);
                }
            }            
            if (wr != 0)
            {
                dataGridView1.Rows.Add(j, y[0], y[1]);
            }
            if (wr != 0)
            {
                label14.Text = Convert.ToString(h);
                label14.Visible = true;

            }
            return y;
        }
        public double Counting(double y0, double y1, double E, double h, int k, int wr)//двойной пересчет
        {
            double s = h;
            double I1 = 1, I2 =0 ;
            while (Math.Abs(I1 - I2) > 3*E)
            {
                I1 = Rangechut(y0, y1, s, 0)[k];
                I2 = Rangechut(y0, y1, s / 2, 0)[k];               
                s = s / 2;          
            }
            Rangechut(y0, y1, s, wr);
            return I2;
        }
        public void Boundare(int k)//Краевая задача без производной
        {
            double x1 = b, c;
            c = Counting(y_0, x1, E, h, k, 0);
            if (b > 0)
            {
                while (c > 0)
                {
                    x1--;
                    c = Counting(y_0, x1, E, h, k, 0);
                }
            }
            else
            {
                while (c < 0) 
                {
                    x1++;
                    c = Counting(y_0, x1, E, h, k, 0);
                }
            }
            if (b + x1 == 0 && b > 0)
                x1 -= 10;
            else
                x1 += 10;

            yy = HalfDivision(x1, b*10, k);
            Counting(y_0, yy, E, h, k, 1);
        }
        public double HalfDivision(double y0, double y1, int k)//МПД без производной
        {
            double y2 = 0, fA = 1, fC = 0, fAC = 0;
            dataGridView2.Rows.Clear();
            while (Math.Abs(fA) > E || Math.Abs(fC) > E)
            {
                y2 = ((y0 + y1) / 2);
                fA = Counting(y_0, y0, E, h, k, 0) - y_1;
                fC = Counting(y_0, y2, E, h, k, 0) - y_1;
                fAC = fA * fC;
                if (fAC < 0)
                    y1 = y2;
                else
                    y0 = y2;
                dataGridView2.Rows.Add(y0, y1, y2);
            }
            label16.Text = Convert.ToString(y2);
            label16.Visible = true;
            return y2;
        }       
        private void button1_Click(object sender, EventArgs e)
        {
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            tb3.Visible = true;
            tb4.Visible = true;
            tb5.Visible = true;
            tb6.Visible = true;
            /*----------------------*/
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            tb7.Visible = false;
            tb8.Visible = false;
            tb9.Visible = false;
            tb10.Visible = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            label12.Visible = true;
            tb7.Visible = true;
            tb8.Visible = true;
            tb9.Visible = true;
            tb10.Visible = true;
            /*----------------------*/
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            tb3.Visible = false;
            tb4.Visible = false;
            tb5.Visible = false;
            tb6.Visible = false;          
        }
        private void button3_Click(object sender, EventArgs e)
        {
            h = Convert.ToDouble(tb1.Text);
            E = Convert.ToDouble(tb2.Text);
            if (tb3.Text != "") a = Convert.ToDouble(tb3.Text);
            if (tb5.Text != "") b = Convert.ToDouble(tb5.Text);
            if (tb4.Text != "") y_0 = Convert.ToDouble(tb4.Text);
            if (tb6.Text != "") { y_1 = Convert.ToDouble(tb6.Text); yy = 0; button3.Text = "Загрузка";  Boundare(1); button3.Text = "Рассчитать"; }
            if (tb7.Text != "") a = Convert.ToDouble(tb7.Text);
            if (tb9.Text != "") b = Convert.ToDouble(tb9.Text);
            if (tb8.Text != "") y_0 = Convert.ToDouble(tb8.Text);
            if (tb10.Text != "") { y_1 = Convert.ToDouble(tb10.Text); yy = 0; button3.Text = "Загрузка"; Boundare(0); button3.Text = "Рассчитать"; }
            
        }
    }
}