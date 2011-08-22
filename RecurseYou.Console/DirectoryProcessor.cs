using System;
using System.Diagnostics;
using System.IO;
using RecuseYou;

namespace RecurseYou
{
    public class DirectoryProcessor
    {
        private readonly FileProcessor _fileProcessor;
        private readonly CommandLineInterpreter _interpreter;
        private readonly IInvokeProcess _processInvoker;

        public DirectoryProcessor(CommandLineInterpreter interpreter, FileProcessor fileProcessor,
                                  IInvokeProcess processInvoker)
        {
            _interpreter = interpreter;
            _processInvoker = processInvoker;
            _fileProcessor = fileProcessor;
        }


        public void Process()
        {
            ProcessStartInfo process = _interpreter.ProcessToExecute;

            foreach (
                string directory in
                    Directory.EnumerateDirectories(_interpreter.StartDirectory, "*", SearchOption.AllDirectories))
            {
                try
                {
                    Console.WriteLine("Processing Directory " + directory);
                    Directory.SetCurrentDirectory(directory);

                    if (_interpreter.ProcessEachFileIndividually)
                    {
                        _fileProcessor.Process(directory, process, _interpreter.DirectoryWildcard);
                    }
                    else
                    {
                        _processInvoker.Invoke(process);
                    }
                }
                catch (Exception ex)
                {
                    if (_interpreter.ContinueOnError)
                    {
                        string message = ex.Message ?? "UNKNOWN";

                        Console.WriteLine("Continuing after error: {0} while processing directory {1}", message,
                                          directory);
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