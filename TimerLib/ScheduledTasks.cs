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
	public static void ExecuteForAPeriodOfTime(int milliseconds, int interval, Action action)
	{
		System.Timers.Timer timer = new()
		{
			Interval = interval,
			AutoReset = true,
		};

		DateTime startTime = DateTime.Now;

		timer.Elapsed += (object? source, ElapsedEventArgs e) =>
		{
			try
			{
				if ((e.SignalTime - startTime).TotalMilliseconds >= milliseconds)
				{
					timer.Stop();
					timer.Dispose();
				}
				else
				{
					action.Invoke();
				}
			}
			catch
			{
				timer.Stop();
				timer.Dispose();
			}
		};
		timer.Start();
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
	/// 以一定的时间间隔回调一个函数，直到取消为止
	/// </summary>
	/// <param name="interval">定时间隔，单位 ms</param>
	/// <param name="action">时间到达后的回调函数</param>
	/// <returns>用于取消定时任务的令牌</returns>
	public static void ExecuteUntilCancel(int interval, Action action, CancellationToken token)
	{
		System.Timers.Timer timer = new()
		{
			Interval = interval,
			AutoReset = true,
		};

		timer.Elapsed += (object? source, ElapsedEventArgs e) =>
		{
			try
			{
				if (token.IsCancellationRequested)
				{
					timer.Stop();
					timer.Dispose();
				}
				else
				{
					action.Invoke();
				}
			}
			catch
			{
				timer.Stop();
				timer.Dispose();
			}
		};
		timer.Start();
	}
}
