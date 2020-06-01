using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace ReactiveFileWatcher
{
    internal static class Program
    {
        private static void Main()
        {
            var tempDir = Path.GetTempPath();
            var watcher = new FileSystemWatcher(tempDir) {EnableRaisingEvents = true};
            Console.WriteLine($"Watching {tempDir}...");

            Observable.FromEventPattern(watcher, nameof(watcher.Created))
                .Select(data => ((FileSystemEventArgs)data.EventArgs).FullPath)
                .Subscribe(file => Console.WriteLine($"Saw {file})"));

            Observable.FromEventPattern(watcher, nameof(watcher.Error))
                .Select(data => ((ErrorEventArgs)data.EventArgs).GetException())
                .Subscribe(exception => Console.WriteLine($"Got an error: {exception}"));

            foreach(var _ in Enumerable.Range(0, 10))
            {
                var tempFile = Path.GetTempFileName();
                File.Delete(tempFile);
            }

            watcher.Dispose();
            Console.WriteLine($"Stopped watching {tempDir}");
        }
    }
}