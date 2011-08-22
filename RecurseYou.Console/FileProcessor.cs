using System;
using System.Diagnostics;
using System.IO;

namespace RecurseYou
{
    public class FileProcessor
    {
        private const string SUBSTITUTION_PATTERN = "--filename--";
        private readonly IInvokeProcess _processInvoker;

        public FileProcessor(IInvokeProcess processInvoker)
        {
            _processInvoker = processInvoker;
        }

        public void Process(string directory, ProcessStartInfo process, string wildcard)
        {
            foreach (string file in Directory.EnumerateFiles(directory, wildcard, SearchOption.TopDirectoryOnly))
            {
                Console.WriteLine("Processing file " + file);
                var fileProcess = new ProcessStartInfo(process.FileName);
                fileProcess.Arguments = process.Arguments.Replace(SUBSTITUTION_PATTERN, file);

                _processInvoker.Invoke(fileProcess);
            }
        }
    }
}