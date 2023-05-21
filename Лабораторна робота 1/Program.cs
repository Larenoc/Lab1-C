using System;
using System.Threading;

class Program
{
    static int numThreads = 10000; // Кількість потоків
    static int sequenceStep = 2; // Крок послідовності
    static int permissionDelay = 1000; // Проміжок часу для генерації дозволу

    static void Main()
    {
        Thread[] threads = new Thread[numThreads];
        ManualResetEvent[] threadPermissions = new ManualResetEvent[numThreads];

        for (int i = 0; i < numThreads; i++)
        {
            int threadNumber = i;
            threadPermissions[i] = new ManualResetEvent(false);

            threads[i] = new Thread(() =>
            {
                int sum = 0;
                int count = 0;
                for (int j = 0; j <= threadNumber; j++)
                {
                    sum += j * sequenceStep;
                    count++;
                }
                Console.WriteLine("Thread {0}: Sum = {1}, Count = {2}", threadNumber, sum, count);

                threadPermissions[threadNumber].Set(); // Встановлюємо дозвіл для керуючого потоку
            });

            threads[i].Start();
        }

        foreach (var permission in threadPermissions)
        {
            permission.WaitOne(permissionDelay); // Очікуємо дозволу з заданим проміжком часу
        }

        Console.WriteLine("All threads completed.");
    }
}