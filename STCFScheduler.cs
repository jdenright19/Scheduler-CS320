using System;
using System.Threading;
using System.Collections.Generic;

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
		
		List<String> jobOrdering = new List<String>();
		List<String> jobList = new List<String>();
		var jobDict = new Dictionary<int, dynamic>();
		string[] lines = System.IO.File.ReadAllLines(@"/home/jimmy/assign1-jdenright19-1/Scheduler-CS320/jobAssembler.txt");
		foreach (string line in lines)
		{
			jobList.Add(line);
			Console.WriteLine(line + "\n");
		}

		

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
			jobDict[i] = new {jobNum = jobNum1, pid = pid1, priority = priority1, arrivalTime = arrivalTime1, Duration = Duration1};
			//jobNum[i] = jobNum1;
			//pid[i] = pid1;
			//priority[i] = priority1;
			//arrivalTime[i] = arrivalTime1;
			//Duration[i] = Duration1;

			Console.WriteLine(jobNum1 + " " + pid1 + " " + priority1 + " " + arrivalTime1 + " " + Duration1 + ".");

		}
			int turnAroundTime = 0;
			int responseTime = 0;
			int waitTime = 0;
			int tempVal = 100;		
			int timeslice = 5;
			int tempIncrement = 0;
			int tempSize = jobDict.Count;
			for (int p = 0; p < tempSize; p++)
			{
				
				int tempNum = int.Parse(jobDict[p].Duration);
				
				if (tempVal > tempNum)
				{
							tempVal = tempNum;
							tempIncrement = p;
							tempVal -= 5;
							string newDuration = tempVal.ToString();
							jobDict[p].Duration = newDuration;
							turnAroundTime += int.Parse(jobDict[p].Duration);
							responseTime++;
							
				}
				if (p == tempSize - 1)
				{
					Console.WriteLine("Added increment " + tempIncrement + " with the shortest arrival time of" + tempVal);
					//jobDict.Remove(tempIncrement);
				}
			
			
			}
			Console.WriteLine( "old Count " + jobDict.Count);
			jobDict.Remove(tempVal);
			Console.WriteLine("Removed " + tempVal);
			Console.WriteLine( "new Count " + jobDict.Count);

			int tempVal1 = 100;		
			int tempIncrement1 = 0;
			int tempSize1 = jobDict.Count;
			for (int l = 0; l < tempSize1; l++)
			{
				if (l == tempIncrement)
				{

				}
				else
				{
					
				int tempNum1 = int.Parse(jobDict[l].arrivalTime);
				
				if (tempVal1 > tempNum1)
				{
							tempVal1 = tempNum1;
							tempIncrement1 = l;
							turnAroundTime += int.Parse(jobDict[l].Duration);
							responseTime++;
							//arrayIncrement++;
				}
				if (l == tempSize1 - 1)
				{
					Console.WriteLine("Added increment " + tempIncrement1 + " with the shortest arrival time of " + tempVal1);
					//jobDict.Remove(tempVal);
				}
				}
				
			
			}
			
			jobDict.Remove(tempIncrement1);
			Console.WriteLine("Removed " + tempVal1);

			tempVal1 = 100;		
			int tempIncrement2 = 0;
			tempSize1 = jobDict.Count;
			for (int l = 0; l < tempSize1; l++)
			{
				if (l == tempIncrement || l == tempIncrement1)
				{

				}
				else
				{
					
				int tempNum1 = int.Parse(jobDict[l].arrivalTime);
				
				if (tempVal1 > tempNum1)
				{
							tempVal1 = tempNum1;
							tempIncrement2 = l;
							turnAroundTime += int.Parse(jobDict[l].Duration);
							responseTime++;
							//arrayIncrement++;
				}
				if (l == tempSize1 - 1)
				{
					Console.WriteLine("Added increment " + tempIncrement2 + " with the shortest arrival time of" + tempVal1);
					//jobDict.Remove(tempVal);
				}
				}
				
			
			}
			
			jobDict.Remove(tempIncrement2);
			//Console.WriteLine("Removed " + tempVal1);
			Console.WriteLine("Total TurnAroundTime Was " + turnAroundTime);
			Console.WriteLine("Total Wait Time Was " + waitTime);
			Console.WriteLine("Total Response Time Was " + responseTime);
			Console.WriteLine("Average TurnAroundTime Was " + turnAroundTime / 4);
			Console.WriteLine("Average Wait Time Was " + waitTime / 4);
			Console.WriteLine("Average Response Time Was " + responseTime / 4);

	}
}