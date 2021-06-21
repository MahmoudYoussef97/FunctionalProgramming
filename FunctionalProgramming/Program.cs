using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FunctionalProgramming
{
    internal class Program
    {
        private static Func<string, List<string>> getFilesPaths = GetFilesPaths;
        static void Main(string[] args)
        {
            InputArguments input = FormatReadingInput();
            GetFilesDetails(getFilesPaths, input.Path).AsParallel().WithDegreeOfParallelism(8)
                .Where(file => file.Length > input.FileSize * 1024 * 1024)
                .OrderByDescending(x => x.Length)
                .Take(input.NumberOfOutputFiles).ToList()
                .ForEach(x => Console.WriteLine(x.FullName + " : " + x.Length / 1024 / 1024 + "MB"));
        }
        private static InputArguments FormatReadingInput()
        {
            InputArguments input = new InputArguments();
            Console.WriteLine("-------------------------- Succpecious Drive: ----------------------------");
            input.Path = Console.ReadLine();
            Console.WriteLine("-------------------------- Number Of Output Files: ----------------------------");
            input.NumberOfOutputFiles = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("-------------------------- Filter Files Bigger Than in MegaBytes: ----------------------------");
            input.FileSize = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("-------------------------- Processing ----------------------------");
            return input;
        }
        private static List<string> GetFilesPaths(string directory)
        {                
            return Directory.GetFiles(directory, "*", new EnumerationOptions
            {
                RecurseSubdirectories = true
            }).ToList();
        }
        private static List<FileInfo> GetFilesDetails(Func<string, List<string>> getFilesPathOf, string directory)
        {
            List<FileInfo> fileInfoList = new List<FileInfo>();
            getFilesPathOf(directory).ForEach(x => fileInfoList.Add(new FileInfo(x)));
            return fileInfoList;
        }
    }
    public class InputArguments
    {
        public int NumberOfOutputFiles { get; set; }
        public int FileSize { get; set; }
        public string Path { get; set; }
    }
}
