using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    internal class Orchestrator : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //throw new NotImplementedException();         

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    Console.WriteLine("Task 1");
            //    Console.WriteLine("Task 2");
            //    Console.WriteLine("Task 333");
            //    await Task.Delay(2, stoppingToken);
            //    Console.WriteLine("");
            //}

            var timer = new PeriodicTimer(TimeSpan.FromSeconds(3));

            while (await timer.WaitForNextTickAsync())
            {
                //Business logic
                Console.WriteLine("Task with PeriodicTimer 111");
                Console.WriteLine("Task with PeriodicTimer 222");
                Console.WriteLine("Task with PeriodicTimer 333");
                Console.WriteLine("");
            }
        }
    }
}
