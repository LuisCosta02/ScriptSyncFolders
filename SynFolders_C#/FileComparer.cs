using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SynFolders_C_
{
    //This class is responsible to compare all the files using a MD5 Algorithm
    class FileComparer
    {
        public static bool AreFilesEqual(string filePath1, string filePath2)
        {
            using (var hashAlg = MD5.Create())
            {
                byte[] file1Hash = hashAlg.ComputeHash(File.ReadAllBytes(filePath1));
                byte[] file2Hash = hashAlg.ComputeHash(File.ReadAllBytes(filePath2));
                return StructuralComparisons.StructuralEqualityComparer.Equals(file1Hash, file2Hash);
            }
        }
    }
}
