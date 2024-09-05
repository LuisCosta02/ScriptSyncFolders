using SynFolders_C_;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

class FolderSynchronizer
{
    static void Main(string[] args)
    {

        //This script was developed by Luís Costa
        //Github : LuisCosta02
        
        
        //Main class of my script
        //This class is responsible to support all other classes and all the code is executed here
        if (args.Length != 4)
        {
            Console.WriteLine("To run the script:  "+ Assembly.GetExecutingAssembly().GetName().Name + ".exe <sourceDir> <replicaDir> <syncIntervalInSeconds> <logFilePath>");
            return;
        }

        string sourceDirectory = args[0];
        string replicaDirectory = args[1];
        int syncInterval = int.Parse(args[2]);
        string logFilePath = args[3];

        //Object
        SynchronizeFolders synchronizer = new SynchronizeFolders(sourceDirectory, replicaDirectory, logFilePath, syncInterval);
        synchronizer.StartSync();
    }
}



