using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransportationTask
{
    public partial class Form1 : Form
    {

        /*   Переменная показывает, является ли задача корректной,
           u1=1, если задача корректна,
           u1=0, если задача не прочитана или 
         * не является корректной,          */
        int u1 = 0;
        /*  Задача, */
        Task task;
        /*   Объект  для  решения транспортной задачи, */
        TransportTask a;
         
         /* Действия  при  создании  формы, */
        public Form1()
        {
            InitializeComponent();
            
            listBox1.Items.Add("Любой метод,");
            listBox1.Items.Add("Метод северо-западного угла,");
            listBox1.Items.Add("Метод минимального элемента,");
            listBox1.Items.Add("Метод аппроксимации Фогеля,");
            
            listBox2.Items.Add("Любой метод,");
            listBox2.Items.Add("Метод северо-западного угла,");
            listBox2.Items.Add("Метод минимального элемента,");
            listBox2.Items.Add("Метод аппроксимации Фогеля,");
            listBox1.SelectedIndex = 0;
            listBox2.SelectedIndex = 0;

            dataGridA.RowCount = 1;
            dataGridB.RowCount = 1;
            dataGridC.RowCount = 1;
            dataGridC.ColumnCount = 1;
            dataGridA.RowHeadersWidth = 58;
            dataGridB.RowHeadersWidth = 58;
            dataGridC.RowHeadersWidth = 58;
            dataGridA.Columns[0].Width = 100;
            dataGridB.Columns[0].Width = 100;
            dataGridC.Columns[0].Width = 50;
            dataGridSupportingPlan.RowHeadersWidth = 80;
            dataGridSupportingPlan1.RowHeadersWidth = 80;
            dataGridOptimalPlan.RowHeadersWidth = 80;
            dataGridStepPlan.RowHeadersWidth = 100;
            dataGridStepPotential.RowHeadersWidth = 100;
            dataGridA[0, 0].Value = 0;
            dataGridB[0, 0].Value = 0;
            dataGridC[0, 0].Value = 0;

            dataGridA.Rows[0].HeaderCell.Value ="A1";
            dataGridB.Rows[0].HeaderCell.Value="B1";
            dataGridC.Rows[0].HeaderCell.Value = "A1";
            dataGridC.Columns[0].HeaderCell.Value="B1";
        }

         /*  Действия  при  изменении количества поставщиков, */
        private void numericA_ValueChanged(object sender, EventArgs e)
        {
            int n = (int)numericA.Value;
            dataGridA.RowCount = n;
            dataGridC.RowCount = n;
            dataGridA[0, n - 1].Value = 0;
            for (int i = 0; i < dataGridC.ColumnCount; i++)
            {
                dataGridC[i, n-1].Value = 0;
            }
            dataGridA.Rows[n - 1].HeaderCell.Value =
                    "A" + n.ToString();
            dataGridC.Rows[n - 1].HeaderCell.Value =
                    "A" + n.ToString();
        }

        /*  Действия  при  изменении количества потребителей, */
        private void numericB_ValueChanged(object sender, EventArgs e)
        {
            int m = (int)numericB.Value;
            dataGridB.RowCount = m;
            dataGridC.ColumnCount = m;
            dataGridC.Columns[m-1].Width = 50;
            dataGridB[0, m - 1].Value = 0;
            for (int i = 0; i < dataGridC.RowCount; i++)
            {
                dataGridC[m-1,i].Value = 0;
            }
            dataGridB.Rows[m - 1].HeaderCell.Value =
                    "B" + m.ToString();
            dataGridC.Columns[m-1].HeaderText =
                    "B" + m.ToString();
        }

        /*  Действия при изменении шага решения, */
        private void numericStep_ValueChanged(object sender, EventArgs e)
        {
            int i1 = (int)numericStep.Value;
            if (u1 == 1)
            {
                textBox3.Text = a.information1[i1 - 1];
                a.steps[i1 - 1].OutStepToDataGridView(
                        dataGridStepPlan,
                        dataGridStepPotential,
                        textStep);
            }
        }

        /*   Действия при изменении метода 
         * построения опорного плана  транспортной задачи,  */
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i1 = listBox1.SelectedIndex;
            if (listBox2.SelectedIndex != i1)
            {
                listBox2.SelectedIndex = i1;
            }
            if (u1 == 1)
            {
                OutSolutionToForm();
            }
        }

        /*   Действия при изменении  
         * выбраной строки в listBox2,  */
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i1 = listBox2.SelectedIndex;
            if (listBox1.SelectedIndex != i1)
            {
                listBox2.SelectedIndex = i1;
            }
            listBox1.SelectedIndex = i1;
        }
        
          /* Очистка данных задачи и решения,   */
        private void ClearForm()
        {
            DataGridView[] dataGrids =
            {
                dataGridSupportingPlan, dataGridSupportingPlan1,
                dataGridOptimalPlan, dataGridStepPlan,
                dataGridStepPotential
            };
            TextBox[] texts =
            {
                textSupportingPlan, 
                textOptimalPlan, textStep,
                textBox1,textBox2,textBox3
            };
            u1 = 0;
            label8.Text = "из ";
            numericA.Value = 1;
            numericB.Value = 1;
            dataGridA.RowCount = 1;
            dataGridB.RowCount = 1;
            dataGridC.RowCount = 1;
            dataGridC.ColumnCount = 1;
            dataGridA[0, 0].Value = 0;
            dataGridB[0, 0].Value = 0;
            dataGridC[0, 0].Value = 0;
            for (int i = 0; i < dataGrids.Length; i++)
            {
                dataGrids[i].Rows.Clear();
                dataGrids[i].Columns.Clear();
            }
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].Text = " ";
            }
        }


        /*  Нахождение решения и вывод его на форму,  */
        private void OutSolutionToForm()
        {
            a = new TransportTask(task,listBox1.SelectedIndex);
            a.OutSupportingPlanToDataGridView(dataGridSupportingPlan);
            a.OutSupportingPlanToDataGridView(dataGridSupportingPlan1);
            a.OutOptimalPlanToDataGridView(dataGridOptimalPlan);
            a.OutSupportingPlanToDataGridView(dataGridStepPlan);
            textSupportingPlan.Text = 
                a.steps[0].FunctionCost().ToString();
            textOptimalPlan.Text = a.steps[a.steps.Count-1].
                 FunctionCost().ToString();
            textBox1.Text = a.supportPlanString;
            textBox2.Text= a.information;
            dataGridStepPotential.Rows.Clear();
            dataGridStepPotential.Columns.Clear();
            a.steps[0].OutStepToDataGridView(
                dataGridStepPlan, dataGridStepPotential, 
                textStep);
            textBox3.Text =a.information1[0];
            label8.Text = string.Format("из {0}",
                   a.steps.Count);
            numericStep.Value = 1;
            numericStep.Maximum = a.steps.Count;
        }

        /*    Чтение задачи  из  файла  и решение задачи,
           сначала выводится решение, а потом - прочитанная задача,  
           так как возможно при решении  задача была приведена 
           к закрытой задаче,                  */
        private void задачаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string s = openFileDialog1.FileName;
                ClearForm();
                u1=ReadInformation.ReadTaskFile(s, out task);
                if (u1 == 1)
                {
                    task.OutTaskToDataGridView(
                        numericA, numericB,
                        dataGridA, dataGridB, dataGridC);
                    OutSolutionToForm();
                    task.OutTaskToDataGridView(
                        numericA, numericB,
                        dataGridA, dataGridB, dataGridC);
                }
                else
                {
                    MessageBox.Show(string.Format(
                     "Неправильные данные в файле {0}, ", s));
                }
            }
        }

          /*     Создание  закрытой транспортной  задачи  
           * на  основе  случайных  чисел  и  решение задачи, */
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int n = (int)numericA.Value;
            int m = (int)numericB.Value;
            u1 = 1;
            task = new Task(n, m);
            /*     Тут можно раньше задачу вывести , 
             * так как создается сразу закрытая задача, */
            task.OutTaskToDataGridView(numericA,
                numericB, dataGridA, dataGridB, dataGridC);
            OutSolutionToForm();
        }

        /*      Решение  задачи,  которая  есть  на форме,
          сначала выводится решение, а потом - прочитанная задача,  
          так как возможно при решении  задача была приведена 
          к закрытой задаче,                           */
        private void решениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            u1 = ReadInformation.ReadTaskFromForm(dataGridA,
                               dataGridB, dataGridC, out task);
            if (u1 == 1)
            {
                OutSolutionToForm();
                task.OutTaskToDataGridView(numericA,
                    numericB, dataGridA, dataGridB, dataGridC);
            }
            else
            {
                MessageBox.Show(
                 "Неправильные данные на форме, ");
            }
        }
         
          /* Очистка  формы,  */
        private void очисткаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
         
         /*   Сохранение условия  решенной  задачи в файл,
          * чтобы  созданную  задачу  можно  было  решить  позже, */
        private void сохранитьЗадачуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (u1 == 1)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string s = saveFileDialog1.FileName;
                    task.OutToFile(s);
                }
            }
            else
            {
                MessageBox.Show("Задача не была решена, ");
            }
        }

        

    }
}
