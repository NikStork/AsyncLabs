using System.Diagnostics;

namespace AsyncMatrixMultiplication;

public class MultiplicationMatrixStructure
{
    public void RunMatrixApplication()
    {
        while (true)
        {
            Console.WriteLine("What do you want to choose?\n1) Multiplication matrix\n\n\tPress '+' to finished the program.");
            string choice = Console.ReadLine();
            if (choice.Equals("1"))
            {
                Console.WriteLine("\nEnter the size of the matrix: ");
                int sizeRang = Int32.Parse(Console.ReadLine());
                double[,] matrixOne = CreateMatrix(sizeRang);
                double[,] matrixTwo = CreateMatrix(sizeRang);

                Stopwatch stopwatch = new Stopwatch();

                Console.WriteLine("\n\t\tMatrix multiplication results");
                stopwatch.Start();
                double[,] multiplicationParallelMatrix = ParallelMultiplicationMatrix(matrixOne, matrixTwo, sizeRang);
                stopwatch.Stop();
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nTime in parallel: {stopwatch.ElapsedMilliseconds} ms");
                Console.ResetColor();
        
                stopwatch.Restart();
                double[,] multiplicationSynchronousMatrix = SynchronousMultiplicationMatrix(matrixOne, matrixTwo, sizeRang);
                stopwatch.Stop();
                Console.BackgroundColor = ConsoleColor.Yellow; 
                Console.WriteLine($"Time is synchronous: {stopwatch.ElapsedMilliseconds} ms");
                Console.ResetColor();

                Console.WriteLine("\n\tPress 'Enter' to continue the program.");
                string finishedProgram = Console.ReadLine(); 
            }
            else if (choice.Equals("+"))
            {
                break;
            }
            else
            {
                Console.WriteLine("Try again.");
            }
        }
    }

    private double[,] CreateMatrix(int sizeRang)
    {
        double[,] matrixGenerated = new Double[sizeRang, sizeRang];

        for (int i = 0; i < sizeRang; i++)
        {
            for (int j = 0; j < sizeRang; j++)
            {
                matrixGenerated[i, j] = new Random().Next(1, 122);
            }
        }
        return matrixGenerated;
    }

    private double[,] ParallelMultiplicationMatrix(double[,] matrixOne, double[,] matrixTwo, int sizeRang)
    {
        Task[] tasksMultiplication = new Task[Environment.ProcessorCount];
        
        double[,] completeMultiplicationMatrix = new Double[sizeRang, sizeRang];

        var step = tasksMultiplication.Length;

        for (var i = 0; i < tasksMultiplication.Length; ++i)
        {
            var temp = i;

            tasksMultiplication[i] = new Task(() =>
            {
                for (var j = temp; j < completeMultiplicationMatrix.Length; j += step)
                {
                    var row = j / completeMultiplicationMatrix.GetLength(1);
                    var column = j % completeMultiplicationMatrix.GetLength(1);

                    for (var k = 0; k < matrixOne.GetLength(1); ++k)
                    {
                        completeMultiplicationMatrix[row, column] += matrixOne[row, k] * matrixTwo[k, column];
                    }
                }
            });
        }

        foreach (var task in tasksMultiplication)
        {
            task.Start();
        }

        Task.WaitAll(tasksMultiplication);
        
        return completeMultiplicationMatrix;
    }

    private double[,] SynchronousMultiplicationMatrix(double[,] matrixOne, double[,] matrixTwo, int sizeRang)
    {
        double[,] completeMultiplicationMatrix = new Double[sizeRang, sizeRang];
        
        for (int row = 0; row < matrixOne.GetLength(0); row++)
        {
            for (int column = 0; column < matrixTwo.GetLength(1); column++)
            {
                for (int k = 0; k < matrixTwo.GetLength(0); k++)
                {
                    completeMultiplicationMatrix[row, column] += matrixOne[row, k] * matrixTwo[k, column];
                }
            }
        }

        return completeMultiplicationMatrix;
    }
}





