using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;

namespace ReactiveFileWatcher
{
    internal static class Program
    {
        private static void Main()
        {
            var tempDir = Path.GetTempPath();
            var watcher = new FileSystemWatcher(tempDir);
            Console.WriteLine("Created watcher");

            watcher.EnableRaisingEvents = true;
            Console.WriteLine($"Watching {tempDir}...");
            
            IObservable<EventPattern<object>> newFiles = Observable.FromEventPattern(watcher, nameof(watcher.Created));
            newFiles.Subscribe(data =>
            {
                var args = data.EventArgs as FileSystemEventArgs;
                Console.WriteLine($"Saw {args.FullPath}");
            });
            
            var tempFile = Path.GetTempFileName();
            Console.WriteLine($"Created {tempFile}");
            
            File.Delete(tempFile);
            Console.WriteLine($"Deleted {tempFile}");
            
            watcher.Dispose();
            Console.WriteLine("Disposed watcher");
        }
    }
}