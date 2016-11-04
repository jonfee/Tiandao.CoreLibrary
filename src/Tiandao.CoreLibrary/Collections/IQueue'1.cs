using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tiandao.Collections
{
    public interface IQueue<T> : IQueue
    {
		Task EnqueueAsync(object item, object settings = null);

		Task<int> EnqueueManyAsync<TItem>(IEnumerable<TItem> items, object settings = null);

		T Dequeue(object settings = null);

		IEnumerable<T> Dequeue(int count, object settings = null);

		Task<T> DequeueAsync(object settings = null);

		Task<IEnumerable<T>> DequeueAsync(int count, object settings = null);

		new T Peek();

		new IEnumerable<T> Peek(int count);

		Task<T> PeekAsync();

		Task<IEnumerable<T>> PeekAsync(int count);

		new T Take(int startOffset);

		new IEnumerable<T> Take(int startOffset, int count);

		Task<T> TakeAsync(int startOffset);

		Task<IEnumerable<T>> TakeAsync(int startOffset, int count);
	}
}