using System;
using System.Diagnostics;

namespace Test
{
    class Program
    {
        /// <summary>
        /// Main Function.
        /// </summary>
        static void Main(string[] args)
        {
            SetGlobalVariable();
            Console.ReadKey();
        }

        /// <summary>
        /// Function to set/reset the searching system.
        /// </summary>
        private static void SetGlobalVariable()
        {
            Console.Clear();

            string[] arguments = new string[3];
            //Console.WriteLine("Insert name, time, frequancy: ");
            arguments = Console.ReadLine().Split();

            string name = "";
            int time = 0;
            int frequency = 0;

            if (arguments[0].Length != 0) name = arguments[0];
            else SetGlobalVariable();
            if (arguments[1].Length != 0) time = Convert.ToInt32(arguments[1]);
            else SetGlobalVariable();
            if (arguments[2].Length != 0) frequency = Convert.ToInt32(arguments[2]);
            else SetGlobalVariable();

            StartSearching(name, time, frequency);
        }

        /// <summary>
        /// Function to start searching if process exist.
        /// </summary>
        /// <param name="name"> Name of the process </param>
        /// <param name="time"> Time to control if lifetime of the process reach the MaxTime </param>
        /// <param name="frequency"> How many minutes have to pass to recall the function </param>
        static void StartSearching(string name, int time, int frequency)
        {
            bool canCheck = true;

            while (canCheck)
            {
                if (!IsRunning(name, out Process kill)) continue;

                var startTime = TimeSpan.Zero;
                var periodTime = TimeSpan.FromMinutes(frequency);

                var timer = new System.Threading.Timer((e) => { StartTimer(kill, time, name); }, null, startTime, periodTime);

                canCheck = false;
            }
        }

        /// <summary>
        /// Function for counting the life time of the process and kill.
        /// </summary>
        /// <param name="process"> Process to execute the controll and kill </param>
        /// <param name="time"> Time to control if lifetime of the process reach the MaxTime </param>
        /// <param name="name"> Name of the process (only for debug) </param>
        private static void StartTimer(Process process, int time, string name)
        {
            bool isControllingTime = true;

            while (isControllingTime)
            {
                var runtime = (DateTime.Now - process.StartTime).TotalMinutes;
                if (runtime >= time) isControllingTime = false;
            }

            Console.WriteLine(name + " has killed succesfully.");
            process.Kill();
        }

        /// <summary>
        /// Function to check if the inserted process is still running.
        /// </summary>
        /// <param name="name"> Name of process to find </param>
        /// <param name="kill"> Save info of the found Process </param>
        /// <returns> Return True if Process is started, False in opposite case </returns>
        static bool IsRunning(string name, out Process kill)
        {
            Process[] pName = Process.GetProcessesByName(name);
            kill = null;

            if (pName.Length != 0)
            {
                Console.WriteLine(name + " process has been founded. Please wait to kill process.");
                kill = pName[0];
                return true;
            }

            Console.WriteLine(name + " process has not been founded.");
            return false;
        }
    }
}