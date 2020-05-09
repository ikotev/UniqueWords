using System;
using System.Threading;
using System.Threading.Tasks;

namespace UniqueWords.Application.LazyInitialization
{
    public class LazyValueFactory<T> where T : class
    {
        private volatile T _value;
        private SemaphoreSlim _signal;

        public LazyValueFactory()
        {
            _signal = new SemaphoreSlim(1, 1);
        }

        public async Task<T> GetValueAsync(Func<Task<T>> valueFactoryAsync)
        {
            if (_value == null)
            {
                await _signal.WaitAsync();

                try
                {
                    if (_value == null)
                    {
                        _value = await valueFactoryAsync();
                    }
                }
                finally
                {
                    _signal.Release();
                }
            }

            return _value;
        }
    }
}