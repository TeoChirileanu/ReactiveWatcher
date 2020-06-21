using System;
using System.Threading.Tasks;

namespace ReactiveFileWatcher.Core
{
    public interface IReactiveFileWatcher : IDisposable
    {
        Task StartWatchingForNewFiles();
    }
}