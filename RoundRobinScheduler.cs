using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//namespace StreamReadWrite;
public class Queue //initialization of scheduler.
{
	public int front;
	public int rear = -1;
	public int numElements;
	Object[] element;
	int maxSize;

	public Queue(int maxSize)
	{
		element = new Object[maxSize];
		this.maxSize = maxSize;
	}
	public bool Empty { get {return numElements == 0;}}
	public bool Full { get { return numElements == maxSize;}}
	public int Size { get { return numElements; }}
	public void Insert(Object item) //inserts new jobs into the queue.
	{
		if (Full) throw new Exception("Queue is full");
		rear = (rear + 1) % maxSize;
		element[rear] = item;
		numElements++;
	}
	public Object Peek() //looks at the next job queued.
	{
		if (Empty) throw new Exception("Queue is empty");
		return element[front];
	}
	public void Remove() //removes specific item.
	{
		if (Empty) throw new Exception("Queue is empty");
		element[front] = null;
		front = (front + 1) % maxSize;
		numElements--;
	}
	public void Clear() { //clears item in queue.
		while (!Empty) Remove();
		}
	public override String ToString()
	{
		String ans = "";
		int traveler = front;
		int count = 0;
		while (count < numElements)
		{
			ans = ans + " " + element[traveler].ToString();
			traveler = (traveler + 1) % maxSize;
			count++;
		}
		return ans;
	}
}
public class RoundRobinScheduler
{
	private Queue queue;
	private int timeSlice;

	public RoundRobinScheduler(int quantum)
	{
		timeSlice = quantum; //sets timeSlice allowed for each program to run.
		queue = new Queue(5);
	}
	public void AddThread(Thread t)
	{
		t.Priority = ThreadPriority.Normal;
		queue.Insert(t); //insertion of new threads
	}
	public void RunRoundRobin()
	{
		Thread current; //thread scheduler looks at.
		//set the priority of the scheduler as highest priority
		Thread.CurrentThread.Priority = ThreadPriority.Highest;

		for (;;)
		{
			if (!queue.Empty)
			{
				current = (Thread)queue.Peek();
				queue.Remove();
				//raise the priority of current thread
				current.Priority = ThreadPriority.AboveNormal;
				Console.WriteLine(current.Name + " running");
				
				Thread.Sleep(timeSlice);
				//put scheduler to sleep for a set amount of time.
				AddThread(current);
				//puts thread back into the queue after it has ran.

			}
		}
	}
}
public class Job
{
	public void SpinWheels()
	{
		for (; ; ); //runs indefinitely
		
		
		

		
	}
}

public class TestScheduler
{
	

	public static void Main(String[] args)
	{
		Thread.CurrentThread.Priority = ThreadPriority.Highest;

		RoundRobinScheduler scheduler = new RoundRobinScheduler(1000);
		Thread schedThread = new Thread(new ThreadStart(scheduler.RunRoundRobin));
		schedThread.Start();
		int turnaroundTime = 0;
		while(true) 
		{
			List<String> jobList = new List<String>();
		var jobDict = new Dictionary<int, dynamic>();
		
		string[] lines = System.IO.File.ReadAllLines(@"/home/jimmy/assign1-jdenright19-1/Scheduler-CS320/jobAssembler.txt");
		foreach (string line in lines)
		{
			jobList.Add(line);
			Console.WriteLine(line + "\n");
		}
		int timeslice = 5;
		int waitTime = 0;
		int responseTime = 0;
		
		//string newLine;
		//int lineToReplace = 0;
		for (int i = 0; i < jobList.Count; i++)
		{
			//Console.WriteLine("test.");
			string[] jobSplit = jobList[i].Split(" ");
			string jobNum1 = jobSplit[0];
			string pid1 = jobSplit[1];
			string priority1 = jobSplit[2];
			string arrivalTime1 = jobSplit[3];
			string Duration1 = jobSplit[4];
			dynamic d1 = new System.Dynamic.ExpandoObject();
			jobDict[i] = d1;
			int newDuration = int.Parse(Duration1) - timeslice;
			waitTime += 5 * (jobList.Count - 1);
			responseTime += 5;
			Console.WriteLine("Current Total responseTime is: " + responseTime);
			Console.WriteLine("Current Total waitTime is: " + waitTime);
			Console.WriteLine("Current Average responseTime is: " + responseTime / jobList.Count);
			Console.WriteLine("Current Average waitTime is: " + waitTime / jobList.Count);
			if (newDuration < 0)
			{
				Console.WriteLine(jobNum1 + "Has finished running.");
				newDuration = 0;
				turnaroundTime -= 5;
				//Console.WriteLine("new count is " + jobDict.Count);
				//jobDict.Remove(i);
				//Console.WriteLine("new count is " + jobDict.Count);

			}
			if (newDuration != 0)
			{	Console.WriteLine();
				Console.WriteLine("Scheduling " + jobNum1 + " for 5 ms" + " time remaining = " + newDuration);
				Console.WriteLine();
				turnaroundTime += 5;
				Console.WriteLine("Current Total turnaroundTime is: " + turnaroundTime);
				Console.WriteLine("Current average turnaroundTime is: " + turnaroundTime / jobList.Count);
		

			}
			jobDict[i] = new {jobNum = jobNum1, pid = pid1, priority = priority1, arrivalTime = arrivalTime1, Duration = newDuration};
			

			
			//lineToReplace++;
			//jobNum[i] = jobNum1;
			//pid[i] = pid1;
			//priority[i] = priority1;
			//arrivalTime[i] = arrivalTime1;
			//Duration[i] = Duration1;
			//Console.WriteLine(jobDict[i]);
			//Console.WriteLine(jobNum1 + " " + pid1 + " " + priority1 + " " + arrivalTime1 + " " + newDuration + ".");
		}
		//for (int l = 0; l < jobDict.Count - 1; l++)
		//{
			
		//}
		using (StreamWriter writer = new StreamWriter(@"/home/jimmy/assign1-jdenright19-1/Scheduler-CS320/jobAssembler.txt"))
			{
				for (int i = 0; i <= lines.Length - 1; ++i)
				{
					
					writer.WriteLine(jobDict[i].jobNum + " " + jobDict[i].pid + " " + jobDict[i].priority + " " + jobDict[i].arrivalTime + " " + jobDict[i].Duration);
				}
			}
		}
		

		/*Job job1 = new Job();
		Thread t1 = new Thread(new ThreadStart(job1.SpinWheels));
		t1.Name = "Thread1";
		t1.Start();
		scheduler.AddThread(t1);

		Job job2 = new Job();
		Thread t2 = new Thread(new ThreadStart(job2.SpinWheels));
		t2.Name = "Thread2";
		t2.Start();
		scheduler.AddThread(t2);

		Job job3 = new Job();
		Thread t3 = new Thread(new ThreadStart(job3.SpinWheels));
		t3.Name = "Thread3";
		t3.Start();
		scheduler.AddThread(t3);*/

	}
}