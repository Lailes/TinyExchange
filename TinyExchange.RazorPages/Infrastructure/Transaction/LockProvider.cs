using System.Collections.Concurrent;

namespace TinyExchange.RazorPages.Infrastructure.Transaction;

public class LockProvider
{
    private readonly ConcurrentDictionary<string, object> _locks = new ();

    private readonly object _localLock = new();
    
    public DisposeLock GetLock(string id)
    {
        if (_locks.TryGetValue(id, out var lockObject))
        {
            Monitor.Enter(lockObject);
            return new DisposeLock(() => Monitor.Exit(lockObject));
        }

        lock (_locks)
        {
            lockObject = new object();
            _locks[id] = lockObject;
        }
        
        Monitor.Enter(lockObject);
        return new DisposeLock(() => Monitor.Exit(lockObject));
    }
}

public class DisposeLock : IDisposable
{
    private readonly Action _disposeAction;

    public DisposeLock(Action disposeAction) => _disposeAction = disposeAction;

    public void Dispose() => _disposeAction();
}