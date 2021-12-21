using System;

namespace Matrix
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите размерность матриц");
            var size = Convert.ToInt32(Console.ReadLine());
            var a = new Matrix(size, size);
            a.InitializeWithRand();
            var b = new Matrix(size, size);
            b.InitializeWithRand();
            a.ParallelMultiplyBy(b);
            a.ParallelByCellMultiplyBy(b);
            a.SyncMultiplyBy(b);
        }
    }
}