using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Implementation of a queue with priority level, for ease of serch in the A* algorithm
/// </summary>
/// <typeparam name="P">The priority level</typeparam>
/// <typeparam name="V">The value for our queue</typeparam>
public class PriorityQueue<P, V>
{
    /// <summary>
    /// Sorted dictionary of all the queues for each priority value
    /// </summary>
    private SortedDictionary<P, Queue<V>> _list = new SortedDictionary<P, Queue<V>>();

    /// <summary>
    /// Add a value to the priority queue
    /// </summary>
    /// <param name="priority">The priority of our value</param>
    /// <param name="value">The value we want to add to our queue</param>
    public void Enqueue(P priority, V value)
    {
        if (!_list.TryGetValue(priority, out Queue<V> q))
        {
            q = new Queue<V>();
            _list.Add(priority, q);
        }
        q.Enqueue(value);
    }

    /// <summary>
    /// Provide the next value of the prioritize queue
    /// </summary>
    /// <returns>The next value in the queue</returns>
    public V Dequeue()
    {
        // will throw if there isn’t any first element!
        var pair = _list.First();
        var v = pair.Value.Dequeue();
        if (pair.Value.Count == 0) // nothing left of the top priority.
            _list.Remove(pair.Key);
        return v;
    }

    public bool IsEmpty
    {
        get { return !_list.Any(); }
    }
}