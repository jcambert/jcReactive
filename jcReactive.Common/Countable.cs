using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace jcReactive.Common
{
    public class Countable
    {
        private int _count;
        public int Count { get { return _count; } }
        public IObservable<T> GetCountable<T>(IObservable<T> source)
        {
            return Observable.Create<T>(o =>
            {
                Interlocked.Increment(ref _count);
                var subscription = source.Subscribe(o);
                var decrement = Disposable.Create(() =>
                {
                    Interlocked.Decrement(ref _count);
                });
                return new CompositeDisposable(subscription, decrement);
            });
        }
    }
}
