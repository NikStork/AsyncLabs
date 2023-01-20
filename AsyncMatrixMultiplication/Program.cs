using System.Diagnostics;

namespace AsyncMatrixMultiplication;

// Выполнил: Филоненко Никита УВП-311

internal class Program
{
    static void Main(string[] args)
    {
        MultiplicationMatrixStructure matrixStructure = new MultiplicationMatrixStructure();
        
        Console.WriteLine("Press 'Enter' to start the program.");
        string keyForRunProgram = Console.ReadLine();
        if (keyForRunProgram.Equals(""))
        {
            matrixStructure.RunMatrixApplication();
        }
        else
        {
            Console.WriteLine("Try again.");
        }
    }
}





