using System;
using System.Timers;

namespace NotesBlaze.Services
{
    public class ToastService : IDisposable
    {
        private string _toastMessage = string.Empty;
        private System.Timers.Timer _timer = new System.Timers.Timer();

        public event EventHandler<string>? ToasterChanged;

        public void SetToast(string message)
        {
            ToasterChanged?.Invoke(this, message);
            _timer.Interval = 10000;
            _timer.AutoReset = false;
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        public void ClearToast()
        {
            ToasterChanged?.Invoke(this, String.Empty);
        }

        private void TimerElapsed(object? sender, ElapsedEventArgs e)
        {
            ClearToast();
            _timer.Stop();
        }


        public void Dispose()
        {
                if (_timer is not null)
                {
                    _timer.Elapsed += TimerElapsed;
                    _timer.Dispose();
                }
        }
    }
}

