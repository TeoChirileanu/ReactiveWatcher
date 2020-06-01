using System;
using System.IO;

namespace ReactiveFileWatcher
{
    internal static class Program
    {
        private static void Main()
        {
            var watcher = new FileSystemWatcher(Path.GetTempPath());
            Console.WriteLine("Created watcher");
            
            watcher.Created += (_, args) => Console.WriteLine($"Saw {args.FullPath}");
            Console.WriteLine("Configured watcher to react to new files or directories");
            
            watcher.EnableRaisingEvents = true;
            Console.WriteLine($"Watching {Path.GetTempPath()}");
            
            var tempFile = Path.GetTempFileName();
            Console.WriteLine($"Created {tempFile}");
            
            File.Delete(tempFile);
            Console.WriteLine($"Deleted {tempFile}");
            
            watcher.Dispose();
            Console.WriteLine("Disposed watcher");
        }
    }
}