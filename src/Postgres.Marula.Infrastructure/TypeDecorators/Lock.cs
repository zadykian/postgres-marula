using System;
using System.Threading;
using System.Threading.Tasks;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Disposable exclusive lock.
	/// </summary>
	public sealed class Lock : IDisposable
	{
		private static readonly SemaphoreSlim semaphore = new(initialCount: 1, maxCount: 1);

		private Lock()
		{
		}

		/// <summary>
		/// Asynchronously acquire disposable lock. 
		/// </summary>
		public static async Task<Lock> AcquireAsync(PositiveTimeSpan lockTimeout)
		{
			var timeoutExceeded = !await semaphore.WaitAsync(lockTimeout);
			if (timeoutExceeded) throw new TimeoutException($"Lock acquiring timeout exceeded ({lockTimeout}).");
			return new Lock();
		}

		/// <inheritdoc />
		void IDisposable.Dispose() => semaphore.Release();
	}
}