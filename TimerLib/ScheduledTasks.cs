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
	public static void ExecuteForAPeriodOfTime(int milliseconds, int interval, Action<object?> action, object? param)
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
			action.Invoke(param);
			if ((e.SignalTime - startTime).TotalMilliseconds >= milliseconds)
			{
				timer.Stop();
				timer.Dispose();
			}
		};
	}

	public static void ExecuteAfterAPeriodOfTime(int milliseconds, Action<object?> action, object? param)
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
				action.Invoke(param);
				timer.Stop();
				timer.Dispose();
			}
		};

		timer.Start();
	}

	public static void ExecuteAfterCancel(int interval, Action<object?, CancellationTokenSource> action, object? param)
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
			action.Invoke(param, cts);
			if (token.IsCancellationRequested)
			{
				timer.Stop();
				timer.Dispose();
			}
		};
	}
}
