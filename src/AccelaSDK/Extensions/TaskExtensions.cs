using System;
using System.Threading.Tasks;

namespace Accela.WindowsStoreSDK
{
    static class TaskExtensions
    {
        public static Task<T2> Then<T1, T2>(this Task<T1> first, Func<T1, T2> next)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");

            var tcs = new TaskCompletionSource<T2>();
            first.ContinueWith(delegate
            {
                if (first.IsFaulted) tcs.TrySetException(first.Exception.InnerExceptions);
                else if (first.IsCanceled) tcs.TrySetCanceled();
                else
                {
                    try
                    {
                        var result = next(first.Result);
                        tcs.TrySetResult(result);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }
            });
            return tcs.Task;
        }
    }
}
