using System.IO;

namespace RecuseYou
{
    public class FileProcessor
    {
        private const string SUBSTITUTION_PATTERN = "--filename--";

        public void Process(string directory, string process, string wildcard)
        {
            foreach (string file in Directory.EnumerateFiles(directory, wildcard, SearchOption.TopDirectoryOnly))
            {
                var fileProcess = process.Replace(SUBSTITUTION_PATTERN, file);
                System.Diagnostics.Process.Start(fileProcess);
            }
        }
    }
}