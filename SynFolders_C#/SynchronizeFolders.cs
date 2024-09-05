using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynFolders_C_
{
    //This class is responsible to run the principal methods in the script, he need both of the classes FileComparer and Logger, by requesting them
    class SynchronizeFolders
    {
        private string sourceDirectory;
        private string replicaDirectory;
        private string logFilePath;
        private int syncInterval;

        public SynchronizeFolders(string sourceDir, string replicaDir, string logPath, int interval)
        {
            sourceDirectory = sourceDir;
            replicaDirectory = replicaDir;
            logFilePath = logPath;
            syncInterval = interval;
        }

        public void StartSync()
        {
            while (true)
            {
                try
                {
                    Synchronize(sourceDirectory, replicaDirectory);
                    Console.WriteLine($"Synchronization completed at {DateTime.Now}. Next sync in {syncInterval} seconds.");
                    Thread.Sleep(syncInterval * 1000);
                }
                catch (Exception ex)
                {
                    WriteinFile.Log(logFilePath, $"An error occurred: {ex.Message}");
                }
            }
        }

        private void Synchronize(string sourceDir, string replicaDir)
        {
            Directory.CreateDirectory(replicaDir); // Ensure the replica directory exists

            SynchronizeDirectories(sourceDir, replicaDir);
            SynchronizeFiles(sourceDir, replicaDir);
        }

        private void SynchronizeDirectories(string sourceDir, string replicaDir)
        {
            DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(sourceDir);
            DirectoryInfo replicaDirectoryInfo = new DirectoryInfo(replicaDir);

            // Synchronize subdirectories
            foreach (DirectoryInfo sourceSubDir in sourceDirectoryInfo.GetDirectories())
            {
                string replicaSubDirPath = Path.Combine(replicaDir, sourceSubDir.Name);
                Synchronize(sourceSubDir.FullName, replicaSubDirPath);
            }

            // Delete subdirectories no longer present in source
            foreach (DirectoryInfo replicaSubDir in replicaDirectoryInfo.GetDirectories())
            {
                string sourceSubDirPath = Path.Combine(sourceDir, replicaSubDir.Name);
                if (!Directory.Exists(sourceSubDirPath))
                {
                    Directory.Delete(replicaSubDir.FullName, true);
                    WriteinFile.Log(logFilePath, $"Deleted directory: {replicaSubDir.FullName}");
                }
            }
        }

        private void SynchronizeFiles(string sourceDir, string replicaDir)
        {
            DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(sourceDir);
            DirectoryInfo replicaDirectoryInfo = new DirectoryInfo(replicaDir);

            // Copy or update files
            foreach (FileInfo sourceFile in sourceDirectoryInfo.GetFiles())
            {
                string replicaFilePath = Path.Combine(replicaDir, sourceFile.Name);
                if (!File.Exists(replicaFilePath) )
                {
                    sourceFile.CopyTo(replicaFilePath, true);
                    WriteinFile.Log(logFilePath, $"Copied file: {sourceFile.FullName} to {replicaFilePath}");
                } else if (!FileComparer.AreFilesEqual(sourceFile.FullName, replicaFilePath)) {
                    sourceFile.CopyTo(replicaFilePath, true);
                    WriteinFile.Log(logFilePath, $"Updated file: {sourceFile.FullName} to {replicaFilePath}");
                }
            }

            // Delete files no longer present in source
            foreach (FileInfo replicaFile in replicaDirectoryInfo.GetFiles())
            {
                string sourceFilePath = Path.Combine(sourceDir, replicaFile.Name);
                if (!File.Exists(sourceFilePath))
                {
                    File.Delete(replicaFile.FullName);
                    WriteinFile.Log(logFilePath, $"Deleted file: {replicaFile.FullName}");
                }
            }
        }
    }
}
