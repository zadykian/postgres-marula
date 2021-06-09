using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Provides lazy asynchronous initialization. 
	/// </summary>
	/// <remarks>
	/// Underlying <see cref="Lazy{T}"/> object
	/// configured with <see cref="LazyThreadSafetyMode.PublicationOnly"/> mode.
	/// </remarks>
	public class AsyncLazy<T>
	{
		private readonly object lockObject;
		private readonly Func<Task<T>> factoryWithRetry;
		private Lazy<Task<T>> instance;

		/// <inheritdoc cref="Lazy{T}(Func{T}, LazyThreadSafetyMode)"/>
		public AsyncLazy(Func<Task<T>> factory)
		{
			lockObject = new();
			factoryWithRetry = RetryOnFailure(factory);
			instance = New();
		}

		/// <summary>
		/// Get awaiter object. 
		/// </summary>
		public TaskAwaiter<T> GetAwaiter()
		{
			lock (lockObject) return instance.Value.GetAwaiter();
		}

		/// <summary>
		/// Create delegate which wraps <paramref name="factory"/> with try-catch-reset block. 
		/// </summary>
		private Func<Task<T>> RetryOnFailure(Func<Task<T>> factory)
			=> async () =>
			{
				try
				{
					return await factory();
				}
				catch
				{
					lock (lockObject) instance = New();
					throw;
				}
			};

		/// <summary>
		/// Create new <see cref="Lazy{T}"/> object. 
		/// </summary>
		private Lazy<Task<T>> New() => new(factoryWithRetry, LazyThreadSafetyMode.PublicationOnly);
	}
}