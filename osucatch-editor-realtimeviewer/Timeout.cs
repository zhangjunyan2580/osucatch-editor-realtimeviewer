using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osucatch_editor_realtimeviewer
{
    public class TimeoutCheck
    {
        public static async Task DoOperationWithTimeout(Action operation, TimeSpan timeout)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var task = Task.Run(() =>
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    operation();
                }, cancellationTokenSource.Token);

                if (await Task.WhenAny(task, Task.Delay(timeout, cancellationTokenSource.Token)) == task)
                {
                    // Operation completed within the timeout.
                    await task;
                }
                else
                {
                    // Operation timed out.
                    cancellationTokenSource.Cancel();
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }
    }
}
