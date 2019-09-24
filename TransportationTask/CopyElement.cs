using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportationTask
{
    /*  Класс содержит функции копирования векторов и матриц, */
    class CopyElement
    {
        /*  Создание  копии вектора,  */
        public static List<int> CopyVector(List<int> a)
        {
            List<int> a1 = new List<int>();
            for (int i = 0; i < a.Count; i++)
            {
                a1.Add(a[i]);
            }
            return a1;
        }

        /*  Создание  копии  матрицы,  */
        public static List<List<int>> CopyMatrix(List<List<int>> a)
        {
            List<List<int>> a1 = new List<List<int>>();
            List<int> a2 = new List<int>();
            if (a.Count > 0)
            {
                for (int i = 0; i < a.Count; i++)
                {
                    a2 = new List<int>();
                    for (int j = 0; j < a[0].Count; j++)
                    {
                        a2.Add(a[i][j]);
                    }
                    a1.Add(a2);
                }
            }
            return a1;
        }
    }
}
