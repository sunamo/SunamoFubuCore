namespace SunamoFubuCore;

public class FileChangePollingWatcher : IDisposable
{
    private readonly IList<WatchedFile> _files = new List<WatchedFile>();
    private readonly object _locker = new object();
    private readonly System.Timers.Timer _timer = new System.Timers.Timer();


    public FileChangePollingWatcher()
    {
        _timer.Elapsed += (sender, args) =>
        {
            var actionList = new List<Action>();
            lock (_locker)
            {
                _files.Each(file => file.Update(actionList.Add));
            }

            actionList.Each(x =>
    {
        try
        {
            x();
        }
        catch (Exception)
        {
            // TODO do something here, or at least get visibility
        }
    });

            if (PollingCallback != null) PollingCallback();
        };
    }

    public Action PollingCallback { get; set; } = () => { };

    public void Dispose()
    {
        _timer.Stop();
    }


    public void WatchFile(string path, Action callback)
    {
        lock (_locker)
        {
            var file = new WatchedFile(path, callback);
            _files.Add(file);
        }
    }

    public void StartWatching(double milliseconds)
    {
        _timer.Interval = milliseconds;

        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    #region Nested type: WatchedFile

    public class WatchedFile
    {
        private readonly Action _callback;

        public WatchedFile(string path, Action callback)
        {
            Path = path;
            LastChanged = GetLastWriteTime();
            _callback = callback;
        }

        public string Path { get; }
        public DateTime LastChanged { get; private set; }

        public DateTime GetLastWriteTime()
        {
            return File.GetLastWriteTime(Path);
        }

        public void Update(Action<Action> queue)
        {
            var lastChanged = File.GetLastWriteTime(Path);
            if (lastChanged > LastChanged)
            {
                queue(_callback);
                LastChanged = lastChanged;
            }
        }
    }

    #endregion
}
