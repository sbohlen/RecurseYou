using System.IO;

namespace RecuseYou
{
    public class FileProcessor
    {
        private const string SUBSTITUTION_PATTERN = "--filename--";
        private readonly IInvokeProcess _processInvoker;

        public FileProcessor(IInvokeProcess processInvoker)
        {
            _processInvoker = processInvoker;
        }

        public void Process(string directory, string process, string wildcard)
        {
            foreach (string file in Directory.EnumerateFiles(directory, wildcard, SearchOption.TopDirectoryOnly))
            {
                string fileProcess = process.Replace(SUBSTITUTION_PATTERN, file);
                _processInvoker.Invoke(fileProcess);
            }
        }
    }
}