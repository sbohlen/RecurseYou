using System;
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
        private string _startDirectory;

        public CommandLineInterpreter(string[] commandLineArgs)
        {
            _commandLineArgs = commandLineArgs;
            SetupStartDirectory();
        }

        protected bool SpecifiesStartPath
        {
            get { return _commandLineArgs.Any(arg => arg.ToLowerInvariant() == START_FOLDER_FLAG); }
        }

        public string StartDirectory
        {
            get { return _startDirectory; }
        }

        public bool ShouldContinueOnError
        {
            get { return _commandLineArgs.Any(arg => arg.ToLowerInvariant() == CONTINUE_ON_ERROR_FLAG); }
        }

        public string ProcessToExecute
        {
            get { return ConcatenateAllArgumentsAfter(GetProcessStartIndex()); }
        }

        public bool ShouldProcessEachFileIndividually
        {
            get { return _commandLineArgs.Any(args => args.ToLowerInvariant() == PROCESS_EACH_FILE_FLAG); }
        }

        private string ConcatenateAllArgumentsAfter(int index)
        {
            var commandLine = new StringBuilder();

            for (int i = index; i < _commandLineArgs.Length; i++)
            {
                commandLine.Append(_commandLineArgs[i] + " ");
            }

            return commandLine.ToString().Trim();
        }

        private int GetProcessStartIndex()
        {
            int index = 0;

            if (ShouldContinueOnError)
            {
                index++;
            }

            if (SpecifiesStartPath)
            {
                index = index + 2;
            }

            if (ShouldProcessEachFileIndividually)
            {
                index++;
            }

            return index;
        }

        private void SetupStartDirectory()
        {
            if (SpecifiesStartPath)
            {
                int index =
                    _commandLineArgs.Select((value, i) => new { Value = value, Index = i }).First(
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
                    string.Format("The specified {0} folder of {1} does not exist or is inaccessible!",
                                  START_FOLDER_FLAG, _startDirectory));
            }
        }
    }
}