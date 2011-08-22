using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RecuseYou
{
    public class CommandLineInterpreter
    {
        private const string CONTINUE_ON_ERROR_FLAG = "-continueonerror";
        private const string START_FOLDER_FLAG = "-startpath";
        private const string PROCESS_EACH_FILE_FLAG = "-eachfile";

        private readonly string[] _commandLineArgs;
        private ProcessStartInfo _processToExecute;
        private string _startDirectory;

        public CommandLineInterpreter(string[] commandLineArgs)
        {
            _commandLineArgs = commandLineArgs;
        }

        protected bool StartPathSpecified
        {
            get { return _commandLineArgs.Any(arg => arg.ToLowerInvariant() == START_FOLDER_FLAG); }
        }

        public string StartDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_startDirectory))
                {
                    SetupStartDirectory();
                }

                return _startDirectory;
            }
        }

        public bool ContinueOnError
        {
            get { return _commandLineArgs.Any(arg => arg.ToLowerInvariant() == CONTINUE_ON_ERROR_FLAG); }
        }

        public ProcessStartInfo ProcessToExecute
        {
            get
            {
                if (_processToExecute == null)
                {
                    SetupProcessToExecute();
                }

                return _processToExecute;
            }
        }

        public bool ProcessEachFileIndividually
        {
            get { return _commandLineArgs.Any(args => args.ToLowerInvariant() == PROCESS_EACH_FILE_FLAG); }
        }

        public string DirectoryWildcard
        {
            //TODO: permit this to be passed in via command-line option at some future point
            get { return "*"; }
        }

        private string ConcatenateAllArgumentsAfter(int index)
        {
            var commandLine = new StringBuilder();

            for (int i = index + 1; i < _commandLineArgs.Length; i++)
            {
                commandLine.Append(_commandLineArgs[i] + " ");
            }

            return commandLine.ToString().Trim();
        }

        private int ProcessStartIndex()
        {
            int index = 0;

            if (ContinueOnError)
            {
                index++;
            }

            if (StartPathSpecified)
            {
                index = index + 2;
            }

            if (ProcessEachFileIndividually)
            {
                index++;
            }

            return index;
        }

        private void SetupProcessToExecute()
        {
            int processStartIndex = ProcessStartIndex();

            string processAsSpecified = _commandLineArgs[processStartIndex];
            string fullPathToProcess = Path.GetFullPath(processAsSpecified);

            _processToExecute = new ProcessStartInfo(fullPathToProcess)
                                    {
                                        Arguments = ConcatenateAllArgumentsAfter(processStartIndex)
                                    };
        }

        private void SetupStartDirectory()
        {
            if (StartPathSpecified)
            {
                int index =
                    _commandLineArgs.Select((value, i) => new {Value = value, Index = i}).First(
                        instance => instance.Value.ToLowerInvariant() == START_FOLDER_FLAG).Index;

                if (_commandLineArgs.Length <= index)
                {
                    throw new ArgumentException(
                        string.Format("You have specified the {0} argument but failed to provide a path argument!",
                                      START_FOLDER_FLAG));
                }

                _startDirectory = _commandLineArgs.ElementAt(index + 1);

                VerifyStartDirectoryExists();
            }
            else
            {
                _startDirectory = Environment.CurrentDirectory;
            }
        }

        private void VerifyStartDirectoryExists()
        {
            if (!Directory.Exists(_startDirectory))
            {
                throw new ArgumentException(
                    string.Format("The specified {0} folder of \"{1}\" does not exist or is inaccessible!",
                                  START_FOLDER_FLAG, _startDirectory));
            }
        }
    }
}