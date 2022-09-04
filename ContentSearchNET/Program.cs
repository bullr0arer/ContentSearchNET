using System;
using System.Collections.Generic;
using System.IO;

namespace ContentSearchNET
{
    class Program
    {
        static void Main(string[] args)
        {
            int exit = 0;
            List<FileInfo> accessList = new List<FileInfo>();
            while (exit == 0)
            {
                accessList.Clear();
                Console.WriteLine("Specify directory path?");
                String queryDir = Console.ReadLine();
                if (queryDir.Equals("close")) { exit = 0; break; }
                Console.WriteLine("Term to search for?");
                String queryTerm = Console.ReadLine();

                DirectoryInfo dirInfo = new DirectoryInfo(@queryDir);
                if (!dirInfo.Exists)
                {
                    Console.WriteLine("*no such directory found, please try again*");
                    continue;
                }
                Searcher searcher = new Searcher();
                searcher.searchDir(queryDir, queryTerm);
                Stack<FileInfo> matches = searcher.getMatches();
                int minuend = matches.Count;
                while (matches.Count > 0) {
                    accessList.Add(matches.Pop());
                    int currIndex = minuend - matches.Count - 1;
                    FileInfo match = accessList[currIndex];
                    Console.WriteLine(currIndex.ToString() + ": " + match.FullName);
                }
                Retry:
                Console.WriteLine("Examine file? (input file index) Otherwise input \"n\" to start a new search");
                String resp = Console.ReadLine();
                if (resp.Equals("n")) { continue; }
                else if (int.TryParse(resp, out int selection))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(@accessList[selection].FullName) { UseShellExecute = true });
                    goto Retry;
                }
                else { goto Retry; }
            }

        }
    }
}

