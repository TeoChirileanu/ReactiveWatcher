﻿using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace ReactiveFileWatcher.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var tempDir = Path.GetTempPath();
            var watcher = new FileSystemWatcher(tempDir) {EnableRaisingEvents = true};
            System.Console.WriteLine($"\nStart watching {tempDir}\n");

            var timeout = TimeSpan.FromSeconds(1);
            Observable.FromEventPattern(watcher, nameof(watcher.Created))
                .Select(data => ((FileSystemEventArgs) data.EventArgs).FullPath)
                .Buffer(timeout)
                .Subscribe(files => System.Console.WriteLine($"Saw {files.Count} files"));

            foreach (var _ in Enumerable.Range(0, 10))
            {
                var tempFile = Path.GetTempFileName();
                File.Delete(tempFile);
            }

            Thread.Sleep(timeout);
            watcher.Dispose();
            System.Console.WriteLine($"\nStopped watching {tempDir}\n");
        }
    }
}