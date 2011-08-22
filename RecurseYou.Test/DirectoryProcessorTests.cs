using System.Diagnostics;
using NUnit.Framework;
using RecuseYou;

namespace RecurseYou.Test
{
    [TestFixture]
    public class DirectoryProcessorTests
    {
        [Test]
        public void CanCreateInstance()
        {
            var commandLineArgs = new string[] {};
            var directoryProcessor = new DirectoryProcessor(new CommandLineInterpreter(commandLineArgs), new FileProcessor());
            Assert.That(directoryProcessor, Is.Not.Null);
        }
    }
}