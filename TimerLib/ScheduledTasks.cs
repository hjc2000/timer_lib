using System.Timers;

namespace TimerLib;
public class ScheduledTasks
{
	/// <summary>
	/// 以一定的间隔执行一个任务一段时间
	/// </summary>
	/// <param name="milliseconds">持续时间</param>
	/// <param name="interval">间隔</param>
	/// <param name="action"></param>
	/// <param name="param"></param>
	public static void ExecuteForAPeriodOfTime(int milliseconds, int interval, Action action)
	{
		System.Timers.Timer timer = new()
		{
			Interval = interval,
			AutoReset = true,
			Enabled = true,
		};

		DateTime startTime = DateTime.Now;

		timer.Elapsed += (object? source, ElapsedEventArgs e) =>
		{
			action.Invoke();
			if ((e.SignalTime - startTime).TotalMilliseconds >= milliseconds)
			{
				timer.Stop();
				timer.Dispose();
			}
		};
	}

	public static void ExecuteAfterAPeriodOfTime(int milliseconds, Action action)
	{
		System.Timers.Timer timer = new()
		{
			Interval = milliseconds,
		};

		DateTime startTime = DateTime.Now;

		timer.Elapsed += (object? source, ElapsedEventArgs e) =>
		{
			if ((e.SignalTime - startTime).TotalMilliseconds >= milliseconds)
			{
				action.Invoke();
				timer.Stop();
				timer.Dispose();
			}
		};

		timer.Start();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="interval">时间间隔，单位 ms</param>
	/// <param name="action"></param>
	public static void ExecuteAfterCancel(int interval, Action<CancellationTokenSource> action)
	{
		CancellationTokenSource cts = new CancellationTokenSource();
		CancellationToken token = cts.Token;
		System.Timers.Timer timer = new()
		{
			Interval = interval,
			AutoReset = true,
			Enabled = true,
		};
		timer.Elapsed += (object? source, ElapsedEventArgs e) =>
		{
			action.Invoke(cts);
			if (token.IsCancellationRequested)
			{
				timer.Stop();
				timer.Dispose();
			}
		};
	}
}
