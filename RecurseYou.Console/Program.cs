using RecuseYou;

namespace RecurseYou
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var interpreter = new CommandLineInterpreter(args);
            var invoker = new ProcessInvoker();
            var fileProcessor = new FileProcessor(invoker);

            var processor = new DirectoryProcessor(interpreter, fileProcessor, invoker);
            processor.Process();
        }
    }
}