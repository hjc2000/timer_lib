using TimerLib;

int i = 0;
ScheduledTasks.ExecuteAfterCancel(1000, (cts) =>
{
	Console.WriteLine(i++);
	if (i >= 10)
	{
		cts.Cancel();
	}
});
Console.ReadLine();