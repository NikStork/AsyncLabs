
// Выполнил Филоненко Никита УВП-311

namespace AsyncPhilosopher
{
    internal class PhilosopherStructure
    {
        static int philosopherNumber = 5;
        static Task[] philosophers;
        private static Forks[] forks = new Forks[philosopherNumber];
        private static SemaphoreSlim[] philosopherHey = new SemaphoreSlim[philosopherNumber];
        private static SemaphoreSlim waiter;

        static void Main(string[] args)
        {
            philosophers = new Task[philosopherNumber];
            waiter = new SemaphoreSlim(1);
            
            for (int i = 0; i < philosopherNumber; i++)
            {
                forks[i] = new Forks();
                philosopherHey[i] = new SemaphoreSlim(1);
                philosophers[i] = activityPhilosopher(i);
            }
            
            Task.WaitAll(philosophers);
        }
        
        private static int Right(int i) => i;
        private static int LeftPhilosopher(int i) => (philosopherNumber + i - 1) % philosopherNumber;
        private static int Left(int i) => (i + 1) % philosopherNumber;
        private static int RightPhilosopher(int i) => (i + 1) % philosopherNumber;
        
        private static Task activityPhilosopher(int philosopherIndex)
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    TakeFork(philosopherIndex);
                    PutFork(philosopherIndex);
                    Thinking(philosopherIndex);
                }
            });
        }

        public static void Thinking(int index)
        {
            Console.WriteLine($"Philosopher {index + 1}: Thinking.");
            Task.Delay(TimeSpan.FromMilliseconds(new Random().Next(500, 1101)));
        }

        public static void TakeFork(int index)
        {
            waiter.Wait();
            if (forks[Left(index)].IsUsing == false && forks[Right(index)].IsUsing == false)
            {
                forks[Left(index)].IsUsing = true;
                forks[Right(index)].IsUsing = true;
                Console.WriteLine($"Philosopher {index + 1}: Take forks and started to eating.");
                waiter.Release();
                Task.Delay(TimeSpan.FromMilliseconds(new Random().Next(3500, 5111)));
            }
            else if (forks[Left(index)].IsUsing == true || forks[Right(index)].IsUsing == true)
            {
                waiter.Release();
                Task.Delay(TimeSpan.FromMilliseconds(new Random().Next(3500, 5111)));
            }
        }

        public static void PutFork(int index)
        {
            if (forks[Left(index)].IsUsing == true && forks[Right(index)].IsUsing == true)
            {
                waiter.Wait();
                forks[Left(index)].IsUsing = false;
                forks[Right(index)].IsUsing = false;
                Console.WriteLine($"Philosopher {index + 1}: Put down forks and back started to thinking.");
                waiter.Release();
            }
        }
    }
}