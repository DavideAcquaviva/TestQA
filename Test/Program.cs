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
            SetGlobalVariable(args);
            Console.ReadKey();
        }

        /// <summary>
        /// Function to set/reset the searching system.
        /// </summary>
        private static void SetGlobalVariable(string[] args)
        {
            if (args.Length == 3)
            {
                string name = "";
                int time = 0;
                int frequency = 0;

                if (args[0].Length != 0) name = args[0];
                if (args[1].Length != 0) time = Convert.ToInt32(args[1]);
                if (args[2].Length != 0) frequency = Convert.ToInt32(args[2]);

                StartSearching(name, time, frequency);
            }
            else 
            {
                Console.WriteLine("Error in Argument Input. \n Please insert Name, Time and frequency for the process.");
                args = Console.ReadLine().Split();
                SetGlobalVariable(args);
            }
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
                var process = IsProcessRunning(name);

                if (process != null)
                {
                    var startTime = TimeSpan.Zero;
                    var periodTime = TimeSpan.FromMinutes(frequency);

                    var timer = new System.Threading.Timer((e) => { StartTimer(process, time, name); }, null, startTime, periodTime);

                    canCheck = false;

                }
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
        static Process IsProcessRunning(string name)
        {
            Process[] pName = Process.GetProcessesByName(name);
            if (pName.Length != 0)
            {
                Console.WriteLine(name + " process has been founded. Please wait to kill process.");
                return pName[0];
            }

            Console.WriteLine(name + " process has not been founded.");
            return null;
        }
    }
}