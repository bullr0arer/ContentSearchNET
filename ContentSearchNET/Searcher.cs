using System;
using System.Collections.Generic;
using System.IO;

namespace ContentSearchNET
{
    class Searcher
    {
        public Stack<FileInfo> fileMatches = new Stack<FileInfo>(0);


        public void searchDir(String queryDir, String queryTerm)
        {
            //Find the directory 
            EnumerationOptions options = new EnumerationOptions
            {
                IgnoreInaccessible = true
            };
            DirectoryInfo dirInfo = new DirectoryInfo(@queryDir);

            //Recursively access entire tree 
            //TODO: swap to breadth or depth first
            DirectoryInfo[] subDirs = dirInfo.GetDirectories("*", options);
            foreach (DirectoryInfo d in subDirs)
            {
                searchDir(d.FullName, queryTerm);
            }
            //Perform content search 
            FileInfo[] files = dirInfo.GetFiles("*.txt", options);
            foreach (FileInfo f in files)
            {
                string buffer = null;
                using (FileStream stream = new
                    FileStream(f.FullName, FileMode.Open,
                    FileAccess.Read, FileShare.None))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        buffer = reader.ReadToEnd();
                    }
                }
                if (buffer.Contains(queryTerm))
                {
                    this.fileMatches.Push(f);
                }
            }

        }
        public Stack<FileInfo> getMatches() {
            return fileMatches;
        }
    }
}
