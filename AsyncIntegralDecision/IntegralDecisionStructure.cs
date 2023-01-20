using System.Diagnostics;

namespace AsyncIntegralDecision;

public class IntegralDecisionStructure
{
    public void RunIntegralApplication()
    {
        while (true)
        {
            Console.WriteLine("What do you want to choose?\n1) Integral decision\n\n\tPress '+' to finished the program.");
            string choice = Console.ReadLine();
            if (choice.Equals("1"))
            {
                Console.WriteLine("\nEnter the beginning and end of the segment: ");
                string[] segment = Console.ReadLine().Split(" ");
                Console.WriteLine("\nEnter the inaccuracy: ");
                double inaccuracy = Double.Parse(Console.ReadLine());
                IntegralProgramStructure(Int32.Parse(segment[0]), Int32.Parse(segment[1]), inaccuracy);
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

    public void DecionIntegral(Function functionIntegral, double a, double b, double inaccuracy)
    {
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Restart();
        var resultTwo = IntegralDecision(functionIntegral, a, b, inaccuracy);
        stopwatch.Stop();
        Console.BackgroundColor = ConsoleColor.Yellow; 
        Console.WriteLine($"Time is synchronous: {stopwatch.ElapsedMilliseconds} ms");
        Console.ResetColor();
        

        Console.WriteLine($"\nResult integral decision Synchronous: {resultTwo}");
        
        Console.WriteLine("\n\tPress 'Enter' to continue the program.");
        string finishedProgram = Console.ReadLine(); 
    }

    public void IntegralProgramStructure(double a, double b, double inaccuracy)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\nChoose integral function:\n1) f(X) = x^2\n2) f(X) = 1 / x^5\n3) f(X) = x * sin(x)\n4) f(X) = tg√x / √x\n5) f(X) = 1 / x√(x^2 + x + 1)\n\n\tPress '+' to finished the program.");
        Console.ResetColor();
        string choice = Console.ReadLine();
        
        switch (choice)
        {
            case "1":
                DecionIntegral(IntegralFunctionOne, a, b, inaccuracy);
                break;
            case "2":
                DecionIntegral(IntegralFunctionTwo, a, b, inaccuracy);
                break;
            case "3":
                DecionIntegral(IntegralFunctionThree, a, b, inaccuracy);
                break;
            case "4":
                DecionIntegral(IntegralFunctionFour, a, b, inaccuracy);
                break;
            case "5":
                DecionIntegral(IntegralFunctionFive, a, b, inaccuracy);
                break;
            case "+":
                break;
        }
    }
    
    public delegate double Function(double x);
    
    public double IntegralFunctionOne(double x)
    {
        return Math.Pow(x, 2);
    }
    
    public double IntegralFunctionTwo(double x)
    {
        return 1 / Math.Pow(x, 5);
    }
    
    public double IntegralFunctionThree(double x)
    {
        return x * Math.Sin(x);
    }
    
    public double IntegralFunctionFour(double x)
    {
        return (Math.Tan(Math.Sqrt(x))) / (Math.Sqrt(x));
    }
    
    public double IntegralFunctionFive(double x)
    {
        return 1 / (x * Math.Sqrt(Math.Pow(x, 2) + x + 1));
    }

    public double IntegralDecision(Function functionIntegral, double a, double b, double inaccuracy)
    {
        var resultIntegral = 0.0;
        
        var h = (b - a) / inaccuracy;
        
        for (int i = 0; i <= inaccuracy - 1; i++)
        {
            double x = a + i * h;
            resultIntegral += h * functionIntegral(x);
        }

        return resultIntegral;
    }
}