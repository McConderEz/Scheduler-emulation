using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler
{
    public enum ProcessState
    {
        Running,
        Waiting,
        Completed
    }


    public enum ProcessPriority
    {
        Low,
        Medium,
        High
    }


    public class MyProcess
    {
        public int PID { get; set; }
        public string ProcessName { get; set; }
        public int ExecutionTime { get; set; }
        public int RemainingTime { get; set; }
        public ProcessPriority Priority { get; set; }
        public ProcessState State { get; set; }

        public MyProcess(int pid, string processName, int executionTime, ProcessPriority priority)
        {
            PID = pid;
            ProcessName = processName;
            ExecutionTime = executionTime;
            RemainingTime = executionTime;
            Priority = priority;
            State = ProcessState.Waiting;
        }
    }

    public class Scheduler
    {
        private List<MyProcess> processes;
        private int timeQuantum;

        public Scheduler(int timeQuantum)
        {
            this.timeQuantum = timeQuantum;
            processes = new List<MyProcess>();
        }

        public void AddProcess(MyProcess process)
        {
            MyProcess existingProcess = processes.FirstOrDefault(p => p.PID == process.PID);
            if (existingProcess != null)
            {
                existingProcess.ProcessName = process.ProcessName;
                existingProcess.ExecutionTime = process.ExecutionTime;
                existingProcess.RemainingTime = process.RemainingTime;
                existingProcess.Priority = process.Priority;
            }
            else
            {
                processes.Add(process);
            }
        }

        public void Run()
        {
            Console.WriteLine("Starting Scheduler...");

            while (processes.Count > 0)
            {
                MyProcess currentProcess = GetNextProcess();
                if (currentProcess != null)
                {
                    if (currentProcess.State == ProcessState.Waiting)
                    {
                        RunProcess(currentProcess);
                    }

                    if (currentProcess.RemainingTime > 0)
                    {
                        if (currentProcess.State != ProcessState.Running)
                        {
                            processes.Remove(currentProcess);
                            processes.Add(currentProcess);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Process {currentProcess.PID} ({currentProcess.ProcessName}) completed.");
                        processes.Remove(currentProcess);
                    }
                }

                PrintProcessTable();
                Thread.Sleep(3000);
                Console.Clear();
            }

            Console.WriteLine("Scheduler finished.");
        }

        private MyProcess GetNextProcess()
        {
            if (processes.Count == 0)
            {
                return null;
            }

            processes.Sort((p1, p2) =>
            {
                int priorityCompare = p2.Priority.CompareTo(p1.Priority);
                if (priorityCompare != 0)
                {
                    return priorityCompare;
                }
                else
                {
                    return p1.RemainingTime.CompareTo(p2.RemainingTime);
                }
            });

            return processes[0];
        }

        private void RunProcess(MyProcess process)
        {
            Console.WriteLine($"Running process {process.PID} ({process.ProcessName}) with priority {process.Priority}...");

            process.State = ProcessState.Running;

            for (int i = 0; i < timeQuantum; i++)
            {
                Thread.Sleep(100); 
                process.RemainingTime--;
                if (process.RemainingTime <= 0)
                {
                    break;
                }
            }

            process.State = ProcessState.Waiting; 

            if (process.RemainingTime > 0)
            {
                int newPriority = (int)ProcessPriority.High - process.RemainingTime;
                if (newPriority < (int)ProcessPriority.Low)
                {
                    newPriority = (int)ProcessPriority.Low;
                }
                process.Priority = (ProcessPriority)newPriority;
            }
        }

        private int GetTimeQuantum(ProcessPriority priority)
        {
            switch (priority)
            {
                case ProcessPriority.High:
                    return 4;
                case ProcessPriority.Medium:
                    return 2;
                case ProcessPriority.Low:
                    return 1;
                default:
                    return 1;
            }
        }

        private ProcessPriority GetNextPriority(ProcessPriority priority)
        {
            switch (priority)
            {
                case ProcessPriority.High:
                    return ProcessPriority.Medium;
                case ProcessPriority.Medium:
                    return ProcessPriority.Low;
                case ProcessPriority.Low:
                    return ProcessPriority.Low;
                default:
                    return ProcessPriority.Low;
            }
        }

        private void PrintProcessTable()
        {
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("|  PID  |    Process Name    |  Execution Time  |  Remaining Time  |  Priority  |     State      |");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");

            foreach (var process in processes)
            {
                Console.WriteLine($"{process.PID,-5} |  {process.ProcessName,-18} |  {process.ExecutionTime,-15} |  {process.RemainingTime,-15} |  {process.Priority,-10} |  {process.State,-15} |");
            }

            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine();
        }
    }

}