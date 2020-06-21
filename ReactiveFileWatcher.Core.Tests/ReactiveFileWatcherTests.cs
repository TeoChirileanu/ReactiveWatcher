using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace ReactiveFileWatcher.Core.Tests
{
    [TestClass]
    public class ReactiveFileWatcherTests
    {
        [TestMethod]
        public async Task ShouldWatchNewFilesAndDeleteThem()
        {
            // Arrange
            const string folderToWatch = "watch";
            const string newFile = "foo.txt";
            
            Directory.CreateDirectory(folderToWatch);
            using IReactiveFileWatcher watcher = new ReactiveFileWatcher(folderToWatch);

            // Act
            await watcher.StartWatchingForNewFiles();
            await File.WriteAllTextAsync(Path.Combine(folderToWatch, newFile), string.Empty);
            
            // Assert
            Check.That(File.Exists(newFile)).IsFalse();
            
            // Clean
            Directory.Delete(folderToWatch, true);
        }

        [TestMethod]
        public async Task ShouldFillNewFileWithSomeData()
        {
            // Arrange
            const string folderToWatch = "watch";
            var file = Path.Combine(folderToWatch, "foo.txt");
            var randomData = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());

            Directory.CreateDirectory(folderToWatch);
            using IReactiveFileWatcher watcher = new ReactiveFileWatcher(folderToWatch);

            // Act
            await watcher.StartWatchingForNewFiles();
            await File.WriteAllTextAsync(file, randomData);

            // Assert
            Check.That(File.Exists(file)).IsTrue();
            Check.That(await File.ReadAllTextAsync(file)).IsEqualTo(randomData);

            // Clean
            Directory.Delete(folderToWatch, true);
        }
    }
}