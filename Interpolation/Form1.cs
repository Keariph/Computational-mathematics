using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Forms;

namespace Interpolation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            List<double> x1 = new List<double> { 3, 5, 7, 9 };
            List<double> y1 = new List<double>() { 1, 0, 1, 4 };
            double h = 2;
            for (double i = 1; i < 12; i += h)
            {
                x.Add(i);
                y.Add(f(i));
            }
            for (int n = 0; n < y.Count; n++)
            {
                dataGridView1.Rows.Add(x[n], y[n], Langrange(x, y, x[n]));
                dataGridView2.Rows.Add(x[n], y[n], Aitken(x, y, x[n]));
                dataGridView3.Rows.Add(x[n], y[n], Newton1(x, y, x[n], h));
                dataGridView4.Rows.Add(x[n], y[n], Newton2(x, y, x[n], h));
                dataGridView6.Rows.Add(x[n], y[n], Splines(x, y, x[n]));
                //dataGridView5.Rows.Add(x[n], y[n], Trig(x, y, x[n]));
            }
            GrafInit();
            GrafPrint(x, y, h);
            x.Clear();
            y.Clear();
            x.Add(0);
            h = Math.PI / 3;
            for (int i = 0; i < x1.Count; i++)
            { 
                dataGridView5.Rows.Add(x1[i], y1[i], Trig(x1, y1, x1[i]));
            }
            for (double i = 0; i < 10; i += 0.1)
            {
                /*--------------------------------------------*/
                Tgraf.Series[0].Points.AddXY(i, Trig(x1, y1, i).Real);
               // Tgraf.Series[1].Points.AddXY(i, F(i));
                /*--------------------------------------------*/
                TIgraf.Series[0].Points.AddXY(i, Trig(x1, y1, i).Imaginary);
                //TIgraf.Series[1].Points.AddXY(i, F(i));
            }
            Tgraf.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            Tgraf.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            TIgraf.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            TIgraf.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
        }
            public double F(double x)
        {
            return Math.Sin(x);
        }
        public double f(double x)
        {
            return Convert.ToDouble(Math.Sqrt(x));
        }
        public int Fac(int x)
        {
            int fac = 1;
            if (x == 1)
            {
                return 1;
            }
            else
            {
                for (int i = 1; i <= x; i++)
                {
                    fac *= i;
                }
                return fac;
            }
        }
        public List<double> Gauss(List<List<double>> C, List<double> d)
        {
            for (int i = 0; i < d.Count; i++)
            {
                NormalizeString(C, d, d.Count, i, i);
                NormalizeKOEF(C, d, d.Count, i);
                Subtraction(C, d, i, d.Count);
            }
            return BackwardSubstitution(C, d, d.Count);
        }
        public void NormalizeString(List<List<double>> C, List<double> d, int size, int k, int m)
        {
            double max = Math.Abs(C[m][k]);
            int f = -1;
            int z = m;
            while (z + 1 < size)
                if (max < Math.Abs(C[z + 1][k]))
                {
                    max = C[z + 1][k];
                    z++;
                    f = z;
                }
                else
                {
                    z++;
                }
            swapString(C, d, k, f, size);
            if (max < 0.00001)
            {
                // нет ненулевых диагональных элементов 
                return;
            }
        }
        public void NormalizeKOEF(List<List<double>> C, List<double> d, int size, int k)
        {
            double temp;
            for (int i = k; i < size; i++)
            {
                temp = C[i][k];
                if (temp != 0)
                {
                    for (int j = k; j < size; j++)
                    {
                        C[i][j] = C[i][j] / temp;
                    }
                    d[i] = d[i] / temp;
                }
            }
        }
        public void Subtraction(List<List<double>> C, List<double> d, int k, int size)
        {
            for (int i = k + 1; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {

                    C[i][j] -= C[k][j];
                }
                d[i] -= d[k];
            }
        }
        public void swapString(List<List<double>> C, List<double> d, int k, int m, int size)
        {
            double buffer;
            if (m == -1)
                return;
            for (int i = 0; i < size; i++)
            {
                buffer = C[k][i];
                C[k][i] = C[m][i];
                C[m][i] = buffer;
            }
            buffer = d[k];
            d[k] = d[m];
            d[m] = buffer;
        }
        public List<double> BackwardSubstitution(List<List<double>> C, List<double> d, int size)
        {
            List<double> x = new List<double>();
            for (int i = size - 1; i >= 0; i--)
            {
                x.Add(d[i]);
                for (int j = 0; j < i; j++)
                    d[j] = d[j] - C[j][i] * x[size - i - 1];
            }
            return x;
        }
        public double Langrange(List<double> x, List<double> y, double X)
        {
            List<double> L = new List<double>();
            double l = 0, num = 1, den = 1;
            for (int i = 0; i < x.Count; i++)
            {
                for (int j = 0; j < x.Count; j++)
                {
                    if (j != i)
                    {
                        num *= ((X) - x[j]);
                        den *= (x[i] - x[j]);
                    }
                }
                L.Add(num / den);
                num = 1;
                den = 1;
            }
            for (int i = 0; i < L.Count; i++)
            {
                l += L[i] * y[i];
            }
            return l;
        }
        public double Aitken(List<double> x, List<double> y, double X)
        {
            double P;
            List<List<double>> p = new List<List<double>>();
            int size = y.Count - 1;
            for (int i = 0; i < size; i++)
            {
                p.Add(new List<double>());
                for (int j = 0; j < size - i; j++)
                {
                    if (i == 0)
                    {
                        p[i].Add(((y[j] * (x[j + 1] - X)) - (y[j + 1] * (x[j] - X))) / (x[j + 1] - x[j]));
                    }
                    else
                    {
                        p[i].Add((p[i - 1][j] * (x[j + 1 + i] - X) - (p[i - 1][j + 1] * (x[j] - X))) / (x[j + i + 1] - x[j]));
                    }
                }
            }
            P = p[size - 1][0];
            return P;
        }
        public double Newton1(List<double> x, List<double> y, double X, double h)
        {
            int size = y.Count - 1;
            double N = 0, Q = 1, q;
            List<List<double>> dY = new List<List<double>>();
            for (int i = 0; i < y.Count; i++)
            {
                dY.Add(new List<double>());
                for (int j = 0; j < size - i; j++)
                {
                    if (i == 0)
                    {
                        dY[i].Add(y[j + 1] - y[j]);
                    }
                    else
                    {
                        dY[i].Add(dY[i - 1][j + 1] - dY[i - 1][j]);
                    }
                }
            }
            q = (X - x[0]) / h;
            N = y[0];
            for (int n = 0; n < size - 1; n++)
            {
                if (n == size - 1)
                {
                    Q *= q - n - 1;
                    N += (dY[n][0] / Fac(n)) * Q * (q - n);
                }
                else
                {
                    Q *= q - n;
                    N += (dY[n][0] / Fac(n + 1)) * Q;
                }
            }
            return N;
        }
        public double Newton2(List<double> x, List<double> y, double X, double h)
        {
            int size = y.Count - 1;
            double N, Q = 1, q;
            List<List<double>> dY = new List<List<double>>();
            for (int i = 0; i < y.Count; i++)
            {
                dY.Add(new List<double>());
                for (int j = 0; j < size - i; j++)
                {
                    if (i == 0)
                    {
                        dY[i].Add(y[j + 1] - y[j]);
                    }
                    else
                    {
                        dY[i].Add(dY[i - 1][j + 1] - dY[i - 1][j]);
                    }
                }
            }
            q = (X - x[size]) / h;
            N = y[size];
            for (int n = 0; n < size - 1; n++)
            {
                if (n == size - 1)
                {
                    Q *= q + n;
                    N += (dY[n][size - n - 1] / Fac(n + 1)) * Q * (q + n + 1);
                }
                else
                {
                    Q *= q + n;
                    N += (dY[n][size - n - 1] / Fac(n + 1)) * Q;
                }
            }
            return N;
        }
        public double Splines(List<double> x, List<double> y, double X)
        {
            int size = y.Count - 1, b = 0;
            List<double> h = new List<double>();
            List<double> d = new List<double>();
            List<double> M = new List<double>();
            List<double> S = new List<double>();
            List<List<double>> C = new List<List<double>>();
            //Нахождение h
            for (int i = 0; i < size; i++)
            {
                h.Add(x[i + 1] - x[i]);
            }
            //Нахождение матрицы C
            for (int i = 0; i < size - 1; i++)
            {
                C.Add(new List<double>());
                for (int j = 0; j < size - 1; j++)
                {
                    if (i == j)
                    {
                        C[i].Add((h[i] + h[i + 1]) / 3);
                    }
                    else
                    {
                        if (j == i + 1)
                        {
                            C[i].Add((h[i + 1]) / 6);
                        }
                        else
                        {
                            if (j == i - 1)
                            {
                                C[i].Add(h[i] / 6);
                            }
                            else
                            {
                                if (Math.Abs(i - j) > 1)
                                {
                                    C[i].Add(0);
                                }
                            }
                        }
                    }
                }
            }
            //нахождение d
            for (int i = 1; i < size - 1; i++)
            {
                d.Add(((y[i + 1] - y[i]) / h[i + 1]) - ((y[i] - y[i - 1]) / h[i]));
            }
            //Нахождение M
            List<double> buf = new List<double>();
            buf = Gauss(C, d);
            for (int i = 0; i < size - 1; i++)
            {
                if (i == 0)
                {
                    M.Add(0);
                }
                else
                {
                    M.Add(buf[i - 1]);
                }
            }
            M.Add(0);
            //Нахождение S
            for (int i = 1; i < size; i++)
            {
                S.Add((M[i - 1] *
                    ((Math.Pow((x[i] - X), 3)) / (6 * h[i]))) +
                    M[i] * ((Math.Pow((X - x[i - 1]), 3)) / (6 * h[i])) +
                    (y[i - 1] - ((M[i - 1] * Math.Pow(h[i], 2)) / 6)) * ((x[i] - X) / h[i]) +
                    (y[i] - ((M[i] * (Math.Pow(h[i], 2))) / 6)) * ((X - x[i - 1]) / h[i]));
            }
            for (int i = 0; i < size; i++)
            {
                if (X > x[i])
                {
                    b = i;
                }
            }
            if (b == 0)
            {
                return S[0];
            }
            else
            {
                if (b > (S.Count - 1))
                    return S[b - (b-S.Count) - 1];
                else return S[b];
            }
        }
        public Complex Trig(List<double> x, List<double> y, double X)
        {
            Complex i = new Complex();
            Complex Y = new Complex(0, 0);
            int Size = x.Count, m = 0;
            double T = x[1/*x.Count - 1*/] - x[0], b, a;
            List<Complex> A = new List<Complex>();
            for (int n = (Size / 2); n > -(Size / 2); n--)
            {
                if (Math.Abs(n*2) % 2 == 0)
                {
                        A.Add(0);
                    for (int j = 0; j <= Size-1; j++)
                    {
                        b = (-2 * Math.PI * n * j) / Size;
                        A[m] += y[j] * new Complex(Math.Round(Math.Cos(b),4), Math.Round(Math.Sin(b),4))/Size;
                    }
                        //A[m] /= Size;
                    a = 2 * Math.PI * n * ((X - x[0]) / (T * Size));
                    Y += A[m] *new Complex(Math.Round(Math.Cos(a), 4), Math.Round(Math.Sin(a),4));
                    m++;
                }
            }
            return Y;
        }
        public void GrafInit()
        {
            Lgraf.ChartAreas[0].AxisX.ScaleView.Zoom(0, 10);
            Lgraf.ChartAreas[0].CursorX.IsUserEnabled = true;
            Lgraf.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            Lgraf.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            Lgraf.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            Lgraf.ChartAreas[0].AxisY.ScaleView.Zoom(-10, 10);
            Lgraf.ChartAreas[0].CursorY.IsUserEnabled = true;
            Lgraf.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            Lgraf.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            Lgraf.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            /*-------------------------------------------------------------*/
            Agraf.ChartAreas[0].AxisX.ScaleView.Zoom(0, 10);
            Agraf.ChartAreas[0].CursorX.IsUserEnabled = true;
            Agraf.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            Agraf.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            Agraf.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            Agraf.ChartAreas[0].AxisY.ScaleView.Zoom(-10, 10);
            Agraf.ChartAreas[0].CursorY.IsUserEnabled = true;
            Agraf.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            Agraf.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            Agraf.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            /*------------------------------------------------------------*/
            grafN1.ChartAreas[0].AxisX.ScaleView.Zoom(0, 10);
            grafN1.ChartAreas[0].CursorX.IsUserEnabled = true;
            grafN1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            grafN1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            grafN1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            grafN1.ChartAreas[0].AxisY.ScaleView.Zoom(-10, 10);
            grafN1.ChartAreas[0].CursorY.IsUserEnabled = true;
            grafN1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            grafN1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            grafN1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            /*-------------------------------------------------------------*/
            Ngraf.ChartAreas[0].AxisX.ScaleView.Zoom(0, 10);
            Ngraf.ChartAreas[0].CursorX.IsUserEnabled = true;
            Ngraf.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            Ngraf.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            Ngraf.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            Ngraf.ChartAreas[0].AxisY.ScaleView.Zoom(-10, 10);
            Ngraf.ChartAreas[0].CursorY.IsUserEnabled = true;
            Ngraf.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            Ngraf.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            Ngraf.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            /*------------------------------------------------------------*/
            Sgraf.ChartAreas[0].AxisX.ScaleView.Zoom(0, 10);
            Sgraf.ChartAreas[0].CursorX.IsUserEnabled = true;
            Sgraf.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            Sgraf.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            Sgraf.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            Sgraf.ChartAreas[0].AxisY.ScaleView.Zoom(-10, 10);
            Sgraf.ChartAreas[0].CursorY.IsUserEnabled = true;
            Sgraf.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            Sgraf.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            Sgraf.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            /*------------------------------------------------------------*/
            Tgraf.ChartAreas[0].AxisX.ScaleView.Zoom(0, 10);
            Tgraf.ChartAreas[0].CursorX.IsUserEnabled = true;
            Tgraf.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            Tgraf.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            Tgraf.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            Tgraf.ChartAreas[0].AxisY.ScaleView.Zoom(-10, 10);
            Tgraf.ChartAreas[0].CursorY.IsUserEnabled = true;
            Tgraf.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            Tgraf.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            Tgraf.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            /*------------------------------------------------------------*/
            TIgraf.ChartAreas[0].AxisX.ScaleView.Zoom(0, 10);
            TIgraf.ChartAreas[0].CursorX.IsUserEnabled = true;
            TIgraf.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            TIgraf.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            TIgraf.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            TIgraf.ChartAreas[0].AxisY.ScaleView.Zoom(-10, 10);
            TIgraf.ChartAreas[0].CursorY.IsUserEnabled = true;
            TIgraf.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            TIgraf.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            TIgraf.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
        }
        public void GrafPrint(List<double> x, List<double> y, double h)
        {
            for (double i = 0; i < 10; i += 0.1)
            {
                Lgraf.Series[0].Points.AddXY(i, Langrange(x, y, i));
                Lgraf.Series[1].Points.AddXY(i, f(i));
                /*---------------------------------------*/
                Agraf.Series[0].Points.AddXY(i, Aitken(x, y, i));
                Agraf.Series[1].Points.AddXY(i, f(i));
                /*---------------------------------------*/
                grafN1.Series[0].Points.AddXY(i, Newton1(x, y, i, h));
                grafN1.Series[1].Points.AddXY(i, f(i));
                /*-----------------------------------------*/
                Ngraf.Series[0].Points.AddXY(i, Newton2(x, y, i, h));
                Ngraf.Series[1].Points.AddXY(i, f(i));
                /*--------------------------------------------*/
                Sgraf.Series[0].Points.AddXY(i, Splines(x, y, i));
                Sgraf.Series[1].Points.AddXY(i, f(i));
                /*--------------------------------------------
                Tgraf.Series[0].Points.AddXY(i, Trig(x, y, i).Real);
                Tgraf.Series[1].Points.AddXY(i, f(i));
                --------------------------------------------*/
            }

            Lgraf.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            Lgraf.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            Agraf.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            Agraf.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            grafN1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            grafN1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            Ngraf.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            Ngraf.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            Sgraf.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            Sgraf.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            Tgraf.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            Tgraf.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            int h = 2;
            for (int i = 1; i < 12; i += h)
            {
                x.Add(i);
                y.Add(f(i));
            }
            if (textBox1.Text != "")
            {
                label1.Text = Convert.ToString(Langrange(x, y, Convert.ToDouble(textBox1.Text)));
                label1.Enabled = true;
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            int h = 2;
            for (int i = 1; i < 12; i += h)
            {
                x.Add(i);
                y.Add(f(i));
            }
            if (textBox2.Text != "")
            {
                label4.Text = Convert.ToString(Aitken(x, y, Convert.ToDouble(textBox2.Text)));
                label4.Enabled = true;
            }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            int h = 2;
            for (int i = 1; i < 12; i += h)
            {
                x.Add(i);
                y.Add(f(i));
            }
            if (textBox3.Text != "")
            {
                label8.Text = Convert.ToString(Newton2(x, y, Convert.ToDouble(textBox3.Text), h));
                label8.Enabled = true;
            }
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            int h = 2;
            for (int i = 1; i < 12; i += h)
            {
                x.Add(i);
                y.Add(f(i));
            }
            if (textBox4.Text != "")
            {
                label5.Text = Convert.ToString(Newton1(x, y, Convert.ToDouble(textBox4.Text), h));
                label5.Enabled = true;
            }
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            int h = 2;
            for (int i = 1; i < 12; i += h)
            {
                x.Add(i);
                y.Add(f(i));
            }
            if (textBox5.Text != "")
            {
                label9.Text = Convert.ToString(Splines(x, y, Convert.ToDouble(textBox5.Text)));
                label9.Enabled = true;
            }
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            //x.Add(0);
            //double h = Math.PI / 3;
            //for (int i = 1; i < 5; i++)
            //{
            //    x.Add(x[0] + i * h);
            //}
            //for (int i = 0; i < 5; i++)
            //{
            //    y.Add(F(x[i]));
            //}
            int h = 2;
            for (int i = 1; i < 12; i += h)
            {
                x.Add(i);
                y.Add(f(i));
            }
            if (textBox6.Text != "")
            {
                //label12.Text = Convert.ToString(Langrange(x, y, Convert.ToDouble(textBox6.Text)));
                label12.Enabled = true;
            }
        }
    }
}