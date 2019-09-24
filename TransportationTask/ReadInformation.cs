using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace TransportationTask
{
    /* Класс для  чтения задачи из файла и с формы, */
    class ReadInformation
    {
         /* Чтение  задачи  из  формы, */
        public static int ReadTaskFromForm(
            DataGridView dataGridA, DataGridView dataGridB,
            DataGridView dataGridC,  out Task t)
        {
            int u1 = 1;
            List<List<int>> a = new List<List<int>>();
            List<int> b = new List<int>();
            List<int> c = new List<int>();
            u1 = ReadFromDataGridView1(dataGridA, ref b) +
                ReadFromDataGridView1(dataGridB, ref c) +
                ReadFromDataGridView(dataGridC, ref a);
            u1 = (u1 == 3) ? 1 : 0;
            t = new Task(b, c, a);
            return u1;
        }

        /*    Чтение  списка чисел из  элемента управления 
         * DataGridView  на  форме, */
        private  static int ReadFromDataGridView1(
             DataGridView dataGridA, ref List<int> a)
        {
            int n = dataGridA.RowCount;
            int a1 = 0;
            int u1 = 1;
            a = new List<int>();
            for (int i = 0; i < n; i++)
            {
                if (int.TryParse(dataGridA[0, i].Value.ToString(), out a1)
                    && a1>=0)
                {
                    a.Add(a1);
                }
                else
                {
                    u1 = 0;
                    break;
                }
            }
            return u1;
        }

        /*    Чтение  матрицы чисел из  элемента управления 
        * DataGridView  на  форме, */
        private static int ReadFromDataGridView(
             DataGridView dataGridA, ref List<List<int>> a)
        {
            int n = dataGridA.RowCount;
            int m=dataGridA.ColumnCount;
            int a1 = 0;
            int u1 = 1;
            List<int> u = new List<int>();
            a = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                u = new List<int>();
                for (int j = 0; j < m; j++)
                {
                    if (int.TryParse(dataGridA[j, i].Value.ToString(),
                                           out a1) && a1>=0)
                    {
                        u.Add(a1);
                    }
                    else
                    {
                        u1 = 0;
                        break;
                    }
                }
                a.Add(u);
            }
            return u1;
        }


         /* Чтение задачи из файла, */
        public static int ReadTaskFile(string file,
            out Task t)
        {
            int u1 = 1;
            List<int> mA = new List<int>();
            List<int> mB = new List<int>();
            List<List<int>> a = new List<List<int>>();
            u1 = ReadFromFile(file, ref mA, ref mB, ref a);
            t = new Task(mA, mB, a);
            return u1;
        }


         /*  Чтение  данных для задачи  из  файла,  */
        private static int ReadFromFile(string file,
                    ref List<int> mA, ref List<int> mB,
                    ref List<List<int>> a)
        {
            int u1 = 1;
            StreamReader f1 = new StreamReader(file, Encoding.Default);
            string s;
            int a1;
            List<int> numbers = new List<int>();
            List<int> u = new List<int>();
            mA = new List<int>();
            mB = new List<int>();
            a = new List<List<int>>();
            int n = 0;
            int m = 0;
            s = f1.ReadToEnd();
            f1.Close();
            while (s.Contains("\n") || s.Contains("\r") ||
                 s.Contains("\t"))
            {
                s = s.Replace("\n", "  ");
                s = s.Replace("\r", "  ");
                s = s.Replace("\t", "  ");
            }
            while (s.Contains("  "))
            {
                s = s.Replace("  ", " ");
            }
            s = s.Trim();
            string[] words = s.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (int.TryParse(words[i], out a1) && a1>=0)
                {
                    numbers.Add(a1);
                }
                else
                {
                    u1 = 0;
                    break;
                }
            }
            n = numbers[0];
            m = numbers[1];
            if (numbers.Count != (n * m + m + n + 2))
            {
                u1 = 0;
            }
            if (u1 == 1)
            {
                for (int i = 0; i < n; i++)
                {
                    mA.Add(numbers[i+2]);
                }
                for (int i = 0; i < m; i++)
                {
                    mB.Add(numbers[i+n+2]);
                }
                for (int i = 0; i < n; i++)
                {
                    u = new List<int>();
                    for (int j = 0; j < m; j++)
                    {
                        u.Add(numbers[j+i*m+n+m+2]);
                    }
                    a.Add(u);
                //    MessageBox.Show(string.Format(
                //"строка {0}, {1}, {2}, {3}, {4}, {5},",
                //      i, a[i][0], a[i][1], a[i][2], a[i][3], a[i][4]
                //      ));

                }
            }
            return u1;
        }
    }
}
