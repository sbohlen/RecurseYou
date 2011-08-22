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
            var processor = new DirectoryProcessor(new CommandLineInterpreter(args), new FileProcessor());
            processor.Process();

        }
    }
}
