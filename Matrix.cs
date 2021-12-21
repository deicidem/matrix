using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    public class Matrix
    {
        public int[,] elements { get; set; }
        public TaskFactory Factory = new TaskFactory();
        public int rows { get; set; }
        public int cols { get; set; }
        public Matrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            elements = new int[rows, cols];
        }

        public void InitializeWithRand()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    elements[i, j] = new Random().Next(1, 9);
                }
            }
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sb.Append(elements[i,j] + "  ");
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }

        public Matrix SyncMultiplyBy(Matrix b)
        {
            var start = DateTime.Now;
            Matrix res = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var sum = 0;
                    for (int k = 0; k < cols; k++)
                    {
                        sum += elements[i, k] * b.elements[k, j];
                    }
                    res.elements[i, j] = sum;
                }
            }
            var ts = DateTime.Now - start;
            Console.WriteLine("Однопоточный метод - " + ts.TotalMilliseconds);
            return res;
        }

        public Matrix ParallelMultiplyBy(Matrix b)
        {
            var start = DateTime.Now;
            Matrix res = new Matrix(rows, cols);
            List<Task> tasks = new List<Task>(); 
            for (int i = 0; i < rows; i++)
            {
                var ind = i;
                tasks.Add(Factory.StartNew(() =>
                {
                    int rowId = ind;
                    for (int j = 0; j < cols; j++)
                    {
                        var sum = 0;
                        for (int k = 0; k < cols; k++)
                        {
                            sum += elements[rowId, k] * b.elements[k, j];
                        }

                        res.elements[rowId, j] = sum;
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            var ts = DateTime.Now - start;
            Console.WriteLine("Многопоточный метод (строки) - " + ts.TotalMilliseconds);
            return res;
        }
        public Matrix ParallelByCellMultiplyBy(Matrix b)
        {
            var start = DateTime.Now;
            Matrix res = new Matrix(rows, cols);
            List<Task> tasks = new List<Task>(); 
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var row = i;
                    var col = j;
                    tasks.Add(Factory.StartNew(() =>
                    {
                        var sum = 0;
                        for (int k = 0; k < cols; k++)
                        {
                            sum += elements[row, k] * b.elements[k, col];
                        }

                        res.elements[row,col] = sum;
                    }));
                    
                }
            }

            Task.WaitAll(tasks.ToArray());
            var ts = DateTime.Now - start;
            Console.WriteLine("Многопоточный метод (ячейки) - " + ts.TotalMilliseconds);
            return res;
        }
    }
}