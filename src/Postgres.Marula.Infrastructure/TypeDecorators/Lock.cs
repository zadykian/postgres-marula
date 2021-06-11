using System;
using System.Threading;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Disposable exclusive lock.
	/// </summary>
	public sealed class Lock : IDisposable
	{
		private readonly object lockObject = new();

		public Lock(PositiveTimeSpan lockTimeout)
		{
			if (!Monitor.TryEnter(lockObject, lockTimeout))
				throw new TimeoutException($"Lock acquiring timeout exceeded ({lockTimeout}).");
		}

		/// <inheritdoc />
		void IDisposable.Dispose() => Monitor.Exit(lockObject);
	}
}