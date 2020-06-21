using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ReactiveFileWatcher.Core
{
    public class ReactiveFileWatcher : IReactiveFileWatcher
    {
        private readonly FileSystemWatcher _watcher = new FileSystemWatcher();

        public ReactiveFileWatcher(string folder, Func<string, Task<string>> action = null)
        {
            _watcher.Path = folder;
            var errFile = $@"{folder}\log.err";
            var logfile = $@"{folder}\log.txt";

            Observable.FromEventPattern(_watcher, nameof(_watcher.Created))
                .Select(data => ((FileSystemEventArgs) data.EventArgs).FullPath)
                .Select(action ?? DeleteFile)
                .Subscribe(
                    async contents => await File.AppendAllTextAsync(logfile, await contents),
                    async exception => await File.AppendAllTextAsync(errFile, exception.ToString()));
                // todo: delete files
            
            static async Task<string> DeleteFile(string f)
            {
                File.Delete(f);
                return await Task.FromResult(f);
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