using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TransportationTask
{
      /*  Класс  для  работы  с  самой  задачей, */
    class Task
    {
        /* Вектор количеств единиц товара у поставщиков, */
        public List<int> mA = new List<int>();
        /*   Вектор потребностей в количестве единиц товара
         * у потребителей, */
        public List<int> mB = new List<int>();
        /*   Матрица  стоимости перевозки товаров 
         * от  поставщиков  к  потребителям,  */
        public List<List<int>> a = new List<List<int>>();

        /* Конструктор,  */
        public Task(List<int> mA1, List<int> mB1, 
            List<List<int>> a1)
        {
            mA = new List<int>(mA1);
            mB = new List<int>(mB1);
            a = new List<List<int>>(a1);
        }

         /* Конструктор,  */
        public Task(int n, int m)
        {
            Random r = new Random();
            int a1 = 0;
            int b1 = 0;
            int b;
            List<int> u = new List<int>();
            for (int i = 0; i < n - 1; i++)
            {
                mA.Add(r.Next(10, 100));
                a1 = a1 + mA[i];
            }
            for (int i = 0; i < m - 1; i++)
            {
                mB.Add(r.Next(10, 100));
                b1 = b1 + mB[i];
            }
            b = r.Next(10, 100);
            if (a1 < b1)
            {
                mA.Add(b + b1 - a1);
                mB.Add(b);
            }
            else
            {
                mA.Add(b);
                mB.Add(b + a1 - b1);
            }
            for (int i = 0; i < n; i++)
            {
                u = new List<int>();
                for (int j = 0; j < m; j++)
                {
                    u.Add(r.Next(10));
                }
                a.Add(u);
            }

        }

         /* Запись  информации  о  задаче  в  файл, */
        public void OutToFile(string fileName)
        {
            int n = mA.Count;
            int m = mB.Count;
            StreamWriter f1 = new StreamWriter(fileName, false,
                               Encoding.Default);
            string s;
            f1.WriteLine(string.Format(" {0}  {1}  ", n, m));
            s = "";
            for (int i = 0; i < n; i++)
            {
                s = string.Format("{0} {1} ", s, mA[i]);
            }
            f1.WriteLine(s);
            s = "";
            for (int i = 0; i < m; i++)
            {
                s = string.Format("{0} {1} ", s, mB[i]);
            }
            f1.WriteLine(s);
            for (int i = 0; i < n; i++)
            {
                s = "";
                for (int j = 0; j < m; j++)
                {
                    s = string.Format("{0} {1} ", s, a[i][j]);
                }
                f1.WriteLine(s);
            }
            f1.Close();
        }

         /* Является  ли  задача  закрытой,  */
        public bool IsClose
        {
            get
            {
                return Sum(mA) == Sum(mB);
            }
        }

         /* Сумма  элементов  списка, */
        public int Sum(List<int> a1)
        {
            int sum = 0;
            for (int i = 0; i < a1.Count; i++)
            {
                sum += a1[i];
            }
            return sum;
        }

         /*  Привести  задачу  к  закрытой  задаче, */
        public void ToClose()
        {
            int a1 = Sum(mA);
            int b1 = Sum(mB);
            List<int> u = new List<int>(); 
            if (a1 < b1)
            {
                mA.Add(b1 - a1);
                for (int i = 0; i < mB.Count; i++)
                {
                    u.Add(0);
                }
                a.Add(u);
            }
            else if (a1>b1)
            {
                mB.Add(a1 - b1);
                for (int i = 0; i < mA.Count; i++)
                {
                    a[i].Add(0);
                }
            }
        }
        
        /* Вывод информации о  задаче  на форму,  */
        public void OutTaskToDataGridView(
            NumericUpDown numericA, NumericUpDown numericB,
            DataGridView dataGridA, DataGridView dataGridB, 
             DataGridView dataGridC )
        {
            int n = mA.Count;
            int m = mB.Count;
            if (n > 0 && m > 0)
            {
                numericA.Value = n;
                numericB.Value = m;
                dataGridA.RowCount = n;
                dataGridB.RowCount = m;
                dataGridC.RowCount = n;
                dataGridC.ColumnCount = m;
                for (int i = 0; i < mA.Count; i++)
                {
                    dataGridA[0, i].Value = mA[i];
                    dataGridA.Rows[i].HeaderCell.Value =
                        "A" + (i + 1).ToString();
                    dataGridC.Rows[i].HeaderCell.Value =
                            "A" + (i + 1).ToString();
                }
                for (int i = 0; i < mB.Count; i++)
                {
                    dataGridB[0, i].Value = mB[i];
                    dataGridC.Columns[i].Width = 50;
                    dataGridB.Rows[i].HeaderCell.Value =
                            "B" + (i + 1).ToString();
                    dataGridC.Columns[i].HeaderText =
                            "B" + (i + 1).ToString();
                }
                for (int i = 0; i < a.Count; i++)
                {
                    for (int j = 0; j < a[0].Count; j++)
                    {
                        dataGridC[j, i].Value = a[i][j];
                    }
                }
            }
        }

    }
}
