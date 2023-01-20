using System.Diagnostics;

namespace AsyncIntegralDecisionRecursive;

public class IntegralDecisionStructure
{
    private Mutex _mutex = new Mutex();

    private static double resultIntegralSynchronous = 0.0;
    
    private static double resultIntegralParallel = 0.0;

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
                double eps = Double.Parse(Console.ReadLine());
                IntegralProgramStructure(Int32.Parse(segment[0]), Int32.Parse(segment[1]), eps);
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

    public void DecionIntegral(Function functionIntegral, double a, double b, double eps)
    {
        Stopwatch stopwatch = new Stopwatch();
        
        Console.WriteLine("\n\t\tIntegral decision results: ");
        stopwatch.Start();
        var resultOne = ParallelIntegralDecisionAsync(functionIntegral, a, b, eps);
        stopwatch.Stop();
        Console.BackgroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nTime in parallel: {stopwatch.ElapsedMilliseconds} ms");
        Console.ResetColor();

        stopwatch.Restart();
        var resultTwo = IntegralDecision(functionIntegral, a, b, eps);
        stopwatch.Stop();
        Console.BackgroundColor = ConsoleColor.Yellow; 
        Console.WriteLine($"Time is synchronous: {stopwatch.ElapsedMilliseconds} ms");
        Console.ResetColor();

        Console.WriteLine($"\nResult integral decision Parallel: {resultIntegralParallel}");

        Console.WriteLine($"\nResult integral decision Synchronous: {resultIntegralSynchronous}");
        
        Console.WriteLine("\n\tPress 'Enter' to continue the program.");
        string finishedProgram = Console.ReadLine(); 
    }

    public void IntegralProgramStructure(double a, double b, double eps)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\nChoose integral function:\n1) f(X) = x^2\n2) f(X) = 1 / x^5\n3) f(X) = x * sin(x)\n4) f(X) = tg√x / √x\n5) f(X) = 1 / x√(x^2 + x + 1)\n\n\tPress '+' to finished the program.");
        Console.ResetColor();
        string choice = Console.ReadLine();
        
        switch (choice)
        {
            case "1":
                DecionIntegral(IntegralFunctionOne, a, b, eps);
                break;
            case "2":
                DecionIntegral(IntegralFunctionTwo, a, b, eps);
                break;
            case "3":
                DecionIntegral(IntegralFunctionThree, a, b, eps);
                break;
            case "4":
                DecionIntegral(IntegralFunctionFour, a, b, eps);
                break;
            case "5":
                DecionIntegral(IntegralFunctionFive, a, b, eps);
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

    public async Task ParallelIntegralDecisionAsync(Function functionIntegral, double a, double b, double eps)
    {
        var distance = b - a;
        var stepsForThread = distance / Environment.ProcessorCount;
        var _tasks = new Task[Environment.ProcessorCount];

        double[] results = new double[Environment.ProcessorCount];
            
        for (int i = 0; i < _tasks.Length; i++)
        {
            int tempI = i;
            var _a = a + stepsForThread * i;
            var _b = a + stepsForThread * (i + 1);

            _tasks[i] = new Task(() =>
            { 
                ParallelIntegralDecisionStructureAsync(functionIntegral, _a, _b, eps, out results[tempI]);
            });
        }

        foreach (var task in _tasks)
        {
            task.Start();
        }
        
        Task.WaitAll(_tasks);

        resultIntegralParallel = results.Sum();
    }
    
    public void ParallelIntegralDecisionStructureAsync(Function functionIntegral, double a, double b, double eps, out double decision)
    {
        var resultIntegralOneFigure = 0.0;
        var resultIntegralTwoFigure = 0.0;
        
        var width = b - a;

        var center = (width / 2) + a;

        resultIntegralOneFigure += width * functionIntegral(a);
        
        resultIntegralTwoFigure += (((width / 2) * functionIntegral(a)) + ((width / 2) * functionIntegral((width / 2) + a)));

        if (Math.Abs(resultIntegralTwoFigure - resultIntegralOneFigure) < eps)
        {
            decision = (resultIntegralOneFigure + resultIntegralTwoFigure) / 2;
        }
        else
        {
            ParallelIntegralDecisionStructureAsync(functionIntegral, a, center, eps, out var decisionOne);

            ParallelIntegralDecisionStructureAsync(functionIntegral, center, b, eps, out var decisionTwo);
            
            decision = decisionOne + decisionTwo;
        }
    }
    
    public double IntegralDecision(Function functionIntegral, double a, double b, double eps)
    {
        var resultIntegralOneFigure = 0.0;
        var resultIntegralTwoFigure = 0.0;
        
        var width = b - a;

        resultIntegralOneFigure += width * functionIntegral(a); // Площадь большой фигуры
        
        resultIntegralTwoFigure += (((width / 2) * functionIntegral(a)) + ((width / 2) * functionIntegral((width / 2) + a)));   // Площадь двух маленьких фигур

        if (Math.Abs(resultIntegralTwoFigure - resultIntegralOneFigure) < eps)  // Если разность площадей меньше эпсилон(заданной точности), то суммируем результат найденного интеграла
        {
            _mutex.WaitOne();

            resultIntegralSynchronous += (resultIntegralOneFigure + resultIntegralTwoFigure) / 2;

            _mutex.ReleaseMutex();
        }
        
        else if (Math.Abs(resultIntegralOneFigure - resultIntegralTwoFigure) > eps) // Если разность площадей больше эпсилон(заданной точности), то делим два маленьких прямоугольника еще на два, вызвав рекурсивные функции
        {
            IntegralDecision(functionIntegral, a, ((b - a) / 2) + a, eps);
            IntegralDecision(functionIntegral, ((b - a) / 2) + a, a + (b - a), eps);
        }

        return resultIntegralSynchronous;
    }
    
}