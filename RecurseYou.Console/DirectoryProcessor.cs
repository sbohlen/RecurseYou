using System;
using System.IO;

namespace RecuseYou
{
    public class DirectoryProcessor
    {
        private readonly CommandLineInterpreter _interpreter;
        private readonly FileProcessor _fileProcessor;

        public DirectoryProcessor(CommandLineInterpreter interpreter, FileProcessor fileProcessor)
        {
            _interpreter = interpreter;
            _fileProcessor = fileProcessor;
        }


        public void Process()
        {
            string process = _interpreter.ProcessToExecute;

            foreach (string directory in Directory.EnumerateDirectories(_interpreter.StartDirectory, "*", SearchOption.AllDirectories))
            {
                try
                {
                    Directory.SetCurrentDirectory(directory);

                    if (_interpreter.ProcessEachFileIndividually)
                    {
                        _fileProcessor.Process(directory, process, "*.*");
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(process);
                    }

                }
                catch (Exception ex)
                {
                    if (_interpreter.ContinueOnError)
                    {
                        string message = ex.Message ?? "UNKNOWN";

                        Console.WriteLine("Continuing after error: {0} while processing directory {1}", message, directory);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}