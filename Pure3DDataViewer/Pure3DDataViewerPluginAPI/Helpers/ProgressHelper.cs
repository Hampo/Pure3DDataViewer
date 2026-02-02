using Pure3DDataViewerPluginAPI.Forms;
using System.ComponentModel;

namespace Pure3DDataViewerPluginAPI.Helpers;

public static class ProgressHelper
{
    /// <summary>
    /// Runs a long process with a progress form and cancellation support.
    /// </summary>
    /// <typeparam name="TResult">The type of result the process returns.</typeparam>
    /// <param name="title">Title displayed in the progress form.</param>
    /// <param name="work">The main work to execute. Accepts a function with two parameters:
    /// - Action<int> reportProgress: call with progress percentage (0-100)
    /// - Func<bool> isCancellationRequested: call to check if user requested cancellation
    /// Should return TResult as the result.</param>
    /// <param name="owner">Optional owner for the modal dialog.</param>
    /// <returns>Tuple: (cancelled, result)</returns>
    public static (bool Cancelled, TResult Result) Run<TResult>(string title, Func<Action<int>, Func<bool>, TResult> work, bool allowCancel = true, IWin32Window? owner = null)
    {
        TResult? result = default;
        bool cancelled = false;

        using var progressForm = new FrmProgress(title, allowCancel);
        using var completedEvent = new ManualResetEventSlim(false);
        BackgroundWorker worker = new()
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };

        worker.DoWork += (s, e) =>
        {
            var result = work(
                progress => worker.ReportProgress(progress),
                () => worker.CancellationPending
            );

            if (worker.CancellationPending)
            {
                e.Cancel = true; // <-- ensures e.Cancelled is true
                return;
            }

            e.Result = result;
        };

        worker.ProgressChanged += (s, e) =>
        {
            progressForm.UpdateProgress(e.ProgressPercentage);
        };

        worker.RunWorkerCompleted += (s, e) =>
        {
            if (e.Cancelled)
                cancelled = true;
            else if (e.Error == null)
                result = (TResult)e.Result!;

            completedEvent.Set();
            progressForm.Invoke(() => progressForm.Close()); // close form on UI thread
        };

        progressForm.FormClosing += (s, e) =>
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
                e.Cancel = true;
            }
        };

        worker.RunWorkerAsync();

        if (owner != null)
            progressForm.ShowDialog(owner);
        else
            progressForm.ShowDialog();

        completedEvent.Wait();

        return (cancelled, result!);
    }
}
