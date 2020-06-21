using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ReactiveFileWatcher.Core
{
    public class ReactiveFileWatcher : IReactiveFileWatcher
    {
        private readonly FileSystemWatcher _watcher = new FileSystemWatcher();

        public ReactiveFileWatcher(string folderToWatch, Action<string> actionToExecute = null)
        {
            _watcher.Path = folderToWatch;

            Observable.FromEventPattern(_watcher, nameof(_watcher.Created))
                .Select(data => ((FileSystemEventArgs) data.EventArgs).FullPath)
                .Subscribe(actionToExecute ?? DeleteFile);

            static void DeleteFile(string file)
            {
                if (!File.Exists(file)) File.Delete(file);
            }
        }

        public async Task StartWatchingForNewFiles()
        {
            _watcher.EnableRaisingEvents = true;
            await Task.CompletedTask;
        }

        public void Dispose() => _watcher?.Dispose();
    }
}