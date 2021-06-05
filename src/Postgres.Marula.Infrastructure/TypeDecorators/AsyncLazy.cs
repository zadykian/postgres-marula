using System;
using System.Threading;
using System.Threading.Tasks;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Provides lazy asynchronous initialization. 
	/// </summary>
	public class AsyncLazy<T> : Lazy<Task<T>>
	{
		/// <inheritdoc cref="Lazy{T}(Func{T}, LazyThreadSafetyMode)"/>
		public AsyncLazy(
			Func<Task<T>> factory,
			LazyThreadSafetyMode threadSafetyMode = LazyThreadSafetyMode.PublicationOnly)
			: base(factory, threadSafetyMode)
		{
		}
	}
}