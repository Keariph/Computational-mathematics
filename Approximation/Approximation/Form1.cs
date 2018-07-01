using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace Approximation
{
    public partial class Form1 : Form
    {
        int N;
        List<double> x = new List<double> { 0, 1, 2, 3, 4, 5 };
        List<double> y = new List<double> { 0, 3, 4, 7, 8, 10 };
        List<double> B = new List<double>();
        List<double> A = new List<double>();
        List<List<double>> C = new List<List<double>>();
        public Form1()
        {
            InitializeComponent();
        }
        //public void FileRead()//Чтение с файла
        //{
        //    string str;
        //    FileStream file = new FileStream("C:\\Users\\Evil Spirit\\source\\repos\\Gauss\\Approximation\\FileXY.txt", FileMode.Open, FileAccess.Read);
        //    StreamReader reader = new StreamReader(file);
        //    str = reader.ReadLine();
        //    while (str != null) //пока читается
        //    {
        //        string[] buf = str.Split(' ');
        //        x.Add(Convert.ToDouble(buf[0]));
        //        y.Add(Convert.ToDouble(buf[1]));
        //        str = reader.ReadLine(); ;
        //    }
        //    reader.ReadLine();
        //}
        public double G(double x, int i)
        {
            double g = 0;
            switch (i)
            {
                case 0:
                    {
                        g = 1;
                        break;
                    }
                case 1:
                    {
                        g = x;
                        break;
                    }
                case 2:
                    {
                       g = Math.Pow(x, 2);
                        break;
                    }
                case 3:
                    {
                        g = Math.Pow(x, 3);
                        break;
                    }
                case 4:
                    {
                        g = Math.Pow(x, 4);
                        break;
                    }
                case 5:
                    {
                        g = Math.Pow(x, 5);
                        break;
                    }
                case 6:
                    {
                        g = Math.Pow(x, 6);
                        break;
                    }
                case 7:
                    {
                        g = Math.Pow(x, 7);
                        break;
                    }
                case 8:
                    {
                        g = Math.Pow(x, 8);
                        break;
                    }
            }
            return g;
        }
        public void MatC()
        {
            //for (int i = 0; i < N; i++)
            //{
            //    if (N > x.Count)
            //        N = x.Count;
            //    C.Add(new List<double>());
            //    for (int j = 0; j < N; j++)
            //    {
            //        C[i].Add(0);
            //            for (int k = 0; k < x.Count; k++)
            //            {
            //                C[i][j] += Math.Pow(x[k], i+j) /*(G(x[k], i) * G(x[k], j))*/;
            //            }
            //    }
            //}
            if (N > x.Count)
                N = x.Count;
            for (int i = 0; i < N; i++)
            {
                C.Add(new List<double>());
                for (int j = 0; j < N; j++)
                {
                    C[i].Add(0);
                    for (int k = 0; k < x.Count; k++)
                        C[i][j] += Math.Pow(x[k], i + j);
                }
            }
            for (int i = 0; i < N; i++)
            {
                B.Add(0);
                for (int k = 0; k < x.Count; k++)
                {
                    B[i] += y[k] * Math.Pow(x[k], i);
                }
            }
        }
        //public void VecB()
        //{
        //    for (int i = 0; i < N; i++)
        //    {
        //        B.Add(0);
        //        for (int k = 0; k < x.Count; k++)
        //        {
        //            B[i] += y[k] */* G(x[k], i)*/Math.Pow(x[k], i);
        //        }
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            N = Convert.ToInt32(textBox1.Text);
            B.Clear();
            for(int i = 0; i < C.Count; i++)
            {
                C[i].Clear();
            }
            C.Clear();
            A.Clear();
           // FileRead();
            MatC();
            //VecB();
            A = GaussMethodMOD(ref C, ref B);
            PrintGraphic();
        }
        private double Polynom(double x)
        {
            double result = 0;
            for (int i = 0; i < N - 1; i++)
            {
                result += A[i] * G(x, i);
            }
            return result;
        }
        public void PrintGraphic()
        {
            CH.Series[0].Points.Clear();
            CH.Series[1].Points.Clear();
            CH.ChartAreas[0].AxisX.ScaleView.Zoom(0, x[x.Count - 1]); // -15 <= x <= 2
            CH.ChartAreas[0].CursorX.IsUserEnabled = true;
            CH.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            CH.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            CH.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            CH.ChartAreas[0].AxisY.ScaleView.Zoom(0, y[y.Count - 1] ); // -15 <= x <= 2
            CH.ChartAreas[0].CursorY.IsUserEnabled = true;
            CH.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            CH.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            CH.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            for (int i = 0; i < y.Count; i++)
            {
                CH.Series[1].Points.AddXY(x[i], y[i]);
            }
            for (double i = 0; i < x.Count; i += 0.1)
            {
                CH.Series[0].Points.AddXY(i, Polynom(i));
            }
            for(int i = 0; i <N;i++)
            {
                dataGridViewApp.Rows.Add(x[i], y[i], A[i]);
            }
        }
        private List<double> GaussMethodMOD(ref List<List<double>> Array, ref List<double> VEC)
        {
            for (int i = 0; i < VEC.Count; i++)
            {
                NormalizeString(ref Array, ref VEC, VEC.Count, i, i);
                NormalizeKOEF(ref Array, ref VEC, VEC.Count, i);
                Subtraction(ref Array, ref VEC, i, VEC.Count);
            }
            return BackwardSubstitution(ref Array, ref VEC, VEC.Count);
        }
        private void NormalizeString(ref List<List<double>> Array,ref List<double> VEC, int size, int k, int m)
        {
            double max = Math.Abs(Array[m][k]);
            int f = -1;
            int z = m;
            while (z + 1 < size)
                if (max < Math.Abs(Array[z + 1][k]))
                {
                    max = Array[z + 1][k];
                    z++;
                    f = z;
                }
                else
                {
                    z++;
                }
            swapString(ref Array, ref VEC, k, f, size);
            if (max < 0.00001)
            {
                return;
            }
        }
        private void NormalizeKOEF(ref List<List<double>> Array, ref List<double> VEC, int size, int k)
        {
            double temp;
            for (int i = k; i < size; i++)
            {
                temp = Array[i][k];
                if (temp != 0)
                {
                    for (int j = k; j < size; j++)
                    {
                        Array[i][j] = Array[i][j] / temp;
                    }
                    VEC[i] = VEC[i] / temp;
                }
            }
        }
        private void Subtraction(ref List<List<double>> Array, ref List<double> VEC, int k, int size)
        {
            for (int i = k + 1; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Array[i][j] -= Array[k][j];
                }
                VEC[i] -= VEC[k];
            }
        }
        private void swapString(ref List<List<double>> Array, ref List<double> VEC, int k, int m, int size)
        {
            double buffer;
            if (m == -1)
                return;
            for (int i = 0; i < size; i++)
            {
                buffer = Array[k][i];
                Array[k][i] = Array[m][i];
                Array[m][i] = buffer;
            }
            buffer = VEC[k];
            VEC[k] = VEC[m];
            VEC[m] = buffer;
        }
        private List<double> BackwardSubstitution(ref List<List<double>> Array, ref List<double> VEC, int size)
        {
            int k = 0;
            List<double> x = new List<double>();
            for (int i = size - 1; i >= 0; i--)
            {
                x.Add(0);
                x[k] = VEC[i];
                for (int j = 0; j < i; j++)
                    VEC[j] = VEC[j] - Array[j][i] * x[k];
                k++;
            }
            return x;
        }
    }
}
