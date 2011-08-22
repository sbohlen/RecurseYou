using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RecuseYou
{
    class Program
    {
        static void Main(string[] args)
        {
            var interpreter = new CommandLineInterpreter(args);
            var invoker = new ProcessInvoker();
            var fileProcessor = new FileProcessor(invoker);

            var processor = new DirectoryProcessor(interpreter, fileProcessor, invoker);
            processor.Process();

        }
    }
}
