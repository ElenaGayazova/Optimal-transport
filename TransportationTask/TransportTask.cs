using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransportationTask
{
    /*  Класс для  решения  транспортной  задачи, */    
    class TransportTask
    {
        /* Количество  поставщиков, */
        int n = 0;
        /* Количество  потребителей, */
        int m = 0;
        /* Вектор количеств единиц товара у поставщиков, */
        List<int> mA = new List<int>();
        /*   Вектор потребностей в количестве единиц товара
         * у потребителей, */
        List<int> mB = new List<int>();
        /*   Матрица  стоимости перевозки товаров 
         * от  поставщиков  к  потребителям,  */
        List<List<int>> a = new List<List<int>>();
        /* План решения на данном шаге решения, */
        List<List<int>> plan = new List<List<int>>();
        /* Опорный план решения,  */
        List<List<int>> plan1 = new List<List<int>>();
        /* Цикл  для изменения плана, */
        List<Tuple<int, int>> cycle = new List<Tuple<int, int>>();
        /* Шаг  решения задачи, */
        Step step;
        /* Список  всех  шагов  решения задачи, */
        public List<Step> steps = new List<Step>();
        /* Информация о решении, */
        public string information = "";
        /* Информация о методе построения опорного плана, */
        public string supportPlanString = "";
        /* Информация о  ходе решения, */
        public List<string> information1 = new List<string>();
               
        /*   Конструктор, который принимает  задачу 
         * и вид метода построения опорного плана,  */
        public TransportTask(Task a1, int number)
        {
            if (!a1.IsClose)
            {
                a1.ToClose();
                information = 
                    "Задача приведена к закрытой задаче. \r\n";
            }
            mA =CopyElement.CopyVector(a1.mA);
            mB = CopyElement.CopyVector(a1.mB);
            a = CopyElement.CopyMatrix(a1.a);
            n = a.Count;
            m = a[0].Count;
            List<int> a2 = new List<int>();
            plan = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                a2 = new List<int>();
                for (int j = 0; j < m; j++)
                {
                    a2.Add(-1);
                }
                plan.Add(a2);
            }
            plan1 = CopyElement.CopyMatrix(plan);
            int i1 = 0;
            Random r = new Random();
            i1 = r.Next(10) % 3;
            if (number == 1 || number == 0 && i1 == 0)
            {
                SupportingPlanNW();
                supportPlanString = "Опорный план построен методом " +
                            "северо-западного угла, ";
            }
            else if (number == 2 || number == 0 && i1 == 1)
            {
                SupportingPlanMinElement();
                supportPlanString = "Опорный план построен методом " +
                           "минимального элемента, ";
            }
            else
            {
                SupportingPlanFogel();
                supportPlanString = "Опорный план построен методом " +
                            "аппроксимации  Фогеля, ";
            }
            information = string.Format("{0}{1}",
                              information, supportPlanString);
            plan1 = CopyElement.CopyMatrix(plan);
            step = new Step(a, plan);
            steps.Add(step);
            Solution();
        }

        /* Решение  транспортной  задачи,  */
        private void Solution()
        {
            Tuple<int, int> a1;
            string s = "";
            while (!step.IsOptimal())
            {
                a1 = step.MaxPotential();
                List<Tuple<int, int>> cycle1 = new List<Tuple<int, int>>();
                cycle1.Add(a1);
                Cycle(cycle1, a1.Item1, a1.Item2, 1);
                ChangePlan();
                step = new Step(a, plan);
                steps.Add(step);
                s = "План не является оптимальным, " +
                    "\r\nпостроен цикл, \r\n";
                for (int i = 0; i < cycle.Count; i++)
                {
                    s = string.Format("{0} ({1},{2}),", s,
                          cycle[i].Item1+1, cycle[i].Item2+1);
                }
                information1.Add(s);
            }
            information = string.Format("{0} \r\n"+
                          "количество шагов решения {1}, ",
                              information,steps.Count);
            information1.Add("План является оптимальным.");
        }

        /*   Изменение плана  после построения цикла, 
         * переход к новому  плану,   */
        private void ChangePlan()
        {
            int a1 = MinElementInCycle();
            plan[cycle[0].Item1][cycle[0].Item2] = 0;
            for (int i = 0; i < cycle.Count; i++)
            {
                plan[cycle[i].Item1][cycle[i].Item2] +=
                    (i % 2 == 0 ? 1 : -1) * a1;
            }
            for (int i = 1; i < cycle.Count; i++)
            {
                if (plan[cycle[i].Item1][cycle[i].Item2] == 0)
                {
                    plan[cycle[i].Item1][cycle[i].Item2] = -1;
                    break;
                }
            }
        }


        /*  Минимальное  значение для элементов цикла с минусом, */
        private int MinElementInCycle()
        {
            int a1 = 0;
            int u = 0;
            if (cycle.Count > 1)
            {
                a1 = plan[cycle[1].Item1][cycle[1].Item2];
                for (int i = 0; i < cycle.Count/2; i++)
                {
                    u = plan[cycle[2 * i + 1].Item1][cycle[2 * i + 1].Item2];
                    if (u < a1)
                    {
                        a1 = u;
                    }
                }
            }
            return a1;                
        }

        /*  Создание цикла  в плане  для  перехода к новому плану, */
        private void Cycle(List<Tuple<int, int>> cycle1,
              int i1, int j1, int s)
        {
            if (s < 0 && j1 == cycle1[0].Item2)
            {
                cycle = CopyCycle(cycle1);
            }
            else
            {
                if (s > 0)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (j != j1 && plan[i1][j] >= 0)
                        {
                            List<Tuple<int, int>> cycle2 =
                                 CopyCycle(cycle1);
                            cycle2.Add(Tuple.Create(i1, j));
                            Cycle(cycle2, i1, j, -1 * s);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (i != i1 && plan[i][j1] >= 0)
                        {
                            List<Tuple<int, int>> cycle2 =
                                CopyCycle(cycle1);
                            cycle2.Add(Tuple.Create(i, j1));
                            Cycle(cycle2, i,j1, -1 * s);
                        }
                    }
                }
            }
        }

        /*  Создание  копии цикла,  */
        private List<Tuple<int, int>> CopyCycle(
                 List<Tuple<int, int>> t)
        {
            List<Tuple<int, int>> t1 = new List<Tuple<int, int>>();
            for (int i = 0; i < t.Count; i++)
            {
                t1.Add(t[i]);
            }
            return t1;
        }


        /*    Построение опорного плана методом 
         * северо - западного угла,   */
        private void SupportingPlanNW()
        {
            int i1=0;
            int j1=0;
            List<int> mA1 = CopyElement.CopyVector(mA);
            List<int> mB1 = CopyElement.CopyVector(mB);
            
            int k = n + m - 1;
            int t = 0;
            for (int i = 0; i < k; i++)
            {
                t=Math.Min(mA1[i1],mB1[j1]);

                plan[i1][j1] = t;
                mA1[i1] -= t;
                mB1[j1] -= t;
                if (mA1[i1] >= mB1[j1] && j1 < m - 1)
                {
                    j1++;
                }
                else if (mA1[i1]<=mB1[j1] && i1<n-1)
                {
                    i1++;
                }
            }
        }

        /*    Построение опорного плана методом 
         * минимального элемента,   */
        private void SupportingPlanMinElement()
        {
            List<int> mA1 = CopyElement.CopyVector(mA);
            List<int> mB1 = CopyElement.CopyVector(mB);
            List<List<int>> a1 = CopyElement.CopyMatrix(a);
            
            List<int> a2 = new List<int>();
            plan = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                a2 = new List<int>();
                for (int j = 0; j < m; j++)
                {
                    a2.Add(-1);
                }
                plan.Add(a2);
            }
            int k = n + m - 1;
            int t = 0;
            Tuple<int, int> t1;
            int s = Max(a1)+10;
            for (int i = 0; i < k; i++)
            {
                t1 = Min(a1);
                t = Math.Min(mA1[t1.Item1], mB1[t1.Item2]);
                plan[t1.Item1][t1.Item2] = t;
                mA1[t1.Item1] -= t;
                mB1[t1.Item2] -= t;
                if (mA1[t1.Item1] >= mB1[t1.Item2])
                {
                    for (int j = 0; j < n; j++)
                    {
                        a1[j][t1.Item2] = s;
                    }
                }
                else 
                {
                    for (int j = 0; j < m; j++)
                    {
                        a1[t1.Item1][j] = s;
                    }
                }
            }
        }

         /*      Построение опорного плана методом 
          *   аппроксимации  Фогеля,               */
        private void SupportingPlanFogel()
        {
            List<int> mA1 = CopyElement.CopyVector(mA);
            List<int> mB1 = CopyElement.CopyVector(mB);
            List<int> mA2 = new List<int>();
            List<int> mB2 = new List<int>();
            List<List<int>> a1 = CopyElement.CopyMatrix(a);
            List<int> a2 = new List<int>();
            plan = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                a2 = new List<int>();
                for (int j = 0; j < m; j++)
                {
                    a2.Add(-1);
                }
                plan.Add(a2);
            }
            for (int i = 0; i < n; i++)
            {
                mA2.Add(1);
            }
            for (int i = 0; i < m; i++)
            {
                mB2.Add(1);
            }
            int k = n + m - 1;
            int t = 0;
            Tuple<int, int> t1;
            for (int i = 0; i < k; i++)
            {
                t1 = NewElement(a1, mA2, mB2);
                t = Math.Min(mA1[t1.Item1], mB1[t1.Item2]);
                plan[t1.Item1][t1.Item2] = t;
                mA1[t1.Item1] -= t;
                mB1[t1.Item2] -= t;
                if (mA1[t1.Item1] >= mB1[t1.Item2] && mB2[t1.Item2]>0)
                {
                    mB2[t1.Item2] = 0;
                }
                else 
                {
                    mA2[t1.Item1] = 0;
                }
            }
        }

        /*    Нахождение индексов элемента, который добавляется 
         * в опорный план, который находится по методу Фогеля,  */
        Tuple<int, int> NewElement(List<List<int>> b,
            List<int> mA2, List<int> mB2)
        {
            int i1 = 0;
            int j1 = 0;
            int m1 = 0;
            Tuple<int, int, int, int, int> a1; 
            Tuple<int, int> t = new Tuple<int, int>(0,0);
            List<Tuple<int, int, int, int, int>> t1 =
                new List<Tuple<int, int, int, int, int>>();
            for (int i = 0; i < n; i++)
            {
                if (mA2[i] > 0)
                {
                    t = MinElements(b, mA2, mB2, i, 1);
                    m1 = Math.Abs(b[i][t.Item2] - b[i][t.Item1]);
                    t1.Add(Tuple.Create(i, t.Item1, t.Item2, m1, 1));
                }
            }
            for (int i = 0; i < m; i++)
            {
                if (mB2[i]>0)
                {
                    t = MinElements(b, mA2, mB2, i, 2);
                    m1 = Math.Abs(b[t.Item2][i] - b[t.Item1][i]);
                    t1.Add(Tuple.Create(i, t.Item1, t.Item2, m1, 2));
                }
            }
            a1=t1[0];
            for(int i=0;i<t1.Count;i++)
            {
                if (a1.Item4<t1[i].Item4  ||
                    a1.Item4 == t1[i].Item4 &&
                    (a1.Item5 == 1 && plan[a1.Item1][a1.Item2] > -1 ||
                     a1.Item5 == 2 && plan[a1.Item2][a1.Item1] > -1))
                {
                    a1=t1[i];
                }
            }
            if (a1.Item5 == 1)
            {
                i1 = a1.Item1;
                j1 = a1.Item2;
            }
            else
            {
                j1 = a1.Item1;
                i1 = a1.Item2;
            }
            return Tuple.Create(i1,j1);
        }


        /*     Индексы двух  наименьших элементов матрицы 
         *  в  строке  или столбце, если u=1, 
         *  то поиск по строке  k, находятся индексы столбцов,
         *  если u=2, то  поиск по столбцу  k, 
         *  находятся индексы столбцов,       */
        private Tuple<int, int> MinElements(List<List<int>> b,
                  List<int> mA2, List<int> mB2,  
                  int k,int u=1)
        {
            int n = b.Count;
            int m = b[0].Count;
            int i1 = 0;
            int i2 = 0;
            int m1;
            int m2;
            List<Tuple<int,int>> b1 = new List<Tuple<int,int>>(); 
            /* Поиск по строке, */
            if (u == 1)
            {
                for (int i = 0; i < m; i++)
                {
                    if (mB2[i]>0)
                    {
                        b1.Add(Tuple.Create(i,b[k][i]));
                    }
                }
            }
            /* Поиск по столбцу, */
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (mA2[i]>0)
                    {
                        b1.Add(Tuple.Create(i,b[i][k]));
                    }
                }
            }
            if (b1.Count == 1)
            {
                i1 = b1[0].Item1;
                i2 = b1[0].Item1;
            }
            else if (b1.Count > 1)
            {
                i1 = (b1[0].Item2 < b1[1].Item2) ?
                     b1[0].Item1 : b1[1].Item1;
                i2 = (b1[0].Item2 < b1[1].Item2) ?
                     b1[1].Item1 : b1[0].Item1;
                m1 = (b1[0].Item2 < b1[1].Item2) ?
                     b1[0].Item2 : b1[1].Item2;
                m2 = (b1[0].Item2 < b1[1].Item2) ?
                     b1[1].Item2 : b1[0].Item2;
                for (int i = 2; i < b1.Count; i++)
                {
                    if (b1[i].Item2 < m1)
                    {
                        i2 = i1;
                        i1 = b1[i].Item1;
                        m2 = m1;
                        m1 = b1[i].Item2;
                    }
                    else if (b1[i].Item2 < m2)
                    {
                        i2 = b1[i].Item1;
                        m2 = b1[i].Item2;
                    }
                }
            }
            return Tuple.Create(i1, i2);
        }


        /* Вывод опорного плана   решения  на  форму,   */
        public void OutSupportingPlanToDataGridView(
                                   DataGridView dataGridA)
        {
            OutPlanToDataGridView(plan1,dataGridA);
        }

        /* Вывод оптимального плана   решения  на  форму,   */
        public void OutOptimalPlanToDataGridView(
                                   DataGridView dataGridA)
        {
            OutPlanToDataGridView(plan, dataGridA);
        }


        /* Вывод плана   решения  на  форму,   */
        private void OutPlanToDataGridView(List<List<int>> plan2,
            DataGridView dataGridA)
        {
            dataGridA.RowCount = n;
            dataGridA.ColumnCount = m;
            for (int i = 0; i < n; i++)
            {
                dataGridA.Rows[i].HeaderCell.Value =
                    mA[i].ToString();
            }
            for (int i = 0; i < m; i++)
            {
                dataGridA.Columns[i].Width = 50;
                dataGridA.Columns[i].HeaderText =mB[i].ToString();
            }
            for (int i = 0; i < n; i++)
            {
                dataGridA.Rows[i].Height = 40;
                for (int j = 0; j < m; j++)
                {
                    dataGridA[j, i].Value =
                    plan2[i][j] > -1 ? plan2[i][j].ToString() : "-";
                }
            }
        }


        /* Значение  наибольшего элемента  матрицы,  */
        private int Max(List<List<int>> b)
        {
            int n=b.Count;
            int m=b[0].Count;
            int i1 = 0;
            int j1 = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (b[i1][j1] < b[i][j])
                    {
                        i1 = i;
                        j1 = j;
                    }
                }
            }
            return b[i1][j1];
        }

        /* Индексы  наименьшего элемента  матрицы,  */
        private Tuple<int, int> Min(List<List<int>> b)
        {
            int n = b.Count;
            int m = b[0].Count;
            int i1 = 0;
            int j1 = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (b[i1][j1] > b[i][j])
                    {
                        i1 = i;
                        j1 = j;
                    }
                }
            }
            return Tuple.Create(i1, j1);
        }

    }
}
