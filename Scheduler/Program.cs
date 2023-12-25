
using Scheduler;

Scheduler.Scheduler scheduler = new Scheduler.Scheduler(3); 

MyProcess process1 = new MyProcess(1, "Process 1", 5, ProcessPriority.High);
MyProcess process2 = new MyProcess(2, "Process 2", 3, ProcessPriority.Medium);
MyProcess process3 = new MyProcess(3, "Process 3", 8, ProcessPriority.Low);
MyProcess process4 = new MyProcess(4, "Process 4", 15, ProcessPriority.High);
MyProcess process5 = new MyProcess(5, "Process 5", 20, ProcessPriority.High);
MyProcess process6 = new MyProcess(6, "Process 6", 30, ProcessPriority.High);

scheduler.AddProcess(process1);
scheduler.AddProcess(process2);
scheduler.AddProcess(process3);
scheduler.AddProcess(process4);
scheduler.AddProcess(process5);
scheduler.AddProcess(process6);

scheduler.Run();

Console.ReadLine();
