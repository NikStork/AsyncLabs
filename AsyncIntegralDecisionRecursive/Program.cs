
using AsyncIntegralDecisionRecursive;

namespace AsyncMatrixMultiplication;

// Выполнил: Филоненко Никита УВП-311

internal class Program
{
    static void Main(string[] args)
    {
        IntegralDecisionStructure matrixStructure = new IntegralDecisionStructure();
        
        Console.WriteLine("Press 'Enter' to start the program.");
        string keyForRunProgram = Console.ReadLine();
        if (keyForRunProgram.Equals(""))
        {
            matrixStructure.RunIntegralApplication();
        }
        else
        {
            Console.WriteLine("Try again.");
        }
    }
}