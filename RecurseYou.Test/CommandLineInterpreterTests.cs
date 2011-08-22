using System;
using System.IO;
using NUnit.Framework;
using RecuseYou;

namespace RecurseYou.Test
{
    [TestFixture]
    public class CommandLineInterpreterTests
    {
        private static void AssertThat_Interpreter_IsSetupCorrectly(CommandLineInterpreter interpreter)
        {
            Assert.That(interpreter.ProcessToExecute.FileName, Is.EqualTo(@"c:\test\process.exe"));
            Assert.That(interpreter.ProcessToExecute.Arguments, Is.EqualTo("/s /e"));
        }

        [TestFixture]
        public class When_Continue_On_Error_Flag_Is_Set
        {
            [Test]
            [TestCase("-CONTINUEONERROR", "All UPPERCASE Argument Failed!")]
            [TestCase("-continueonerror", "All lowercase Argument Failed!")]
            [TestCase("-ContinueOnError", "PascalCased Argument Failed!")]
            public void Reports_Proper_Continue_On_Error_Status(string arg, string failureMessage)
            {
                var interpreter = new CommandLineInterpreter(new[] {arg});
                Assert.That(interpreter.ContinueOnError, Is.True, failureMessage);
            }
        }

        [TestFixture]
        public class When_Passing_Continue_On_Error_and_CommandLine
        {
            [Test]
            public void Can_Return_Expected_CommandLine()
            {
                var interpreter =
                    new CommandLineInterpreter(new[] {"-continueOnError", @"c:\test\process.exe", "/s", "/e"});
                AssertThat_Interpreter_IsSetupCorrectly(interpreter);
            }
        }

        [TestFixture]
        public class When_Passing_Continue_On_Error_and_Start_Path_and_Each_File_and_CommandLine
        {
            [Test]
            public void Can_Return_Expected_CommandLine()
            {
                var interpreter =
                    new CommandLineInterpreter(new[]
                                                   {
                                                       "-startPath", @"c:\", "-eachfile", "-continueOnError",
                                                       @"c:\test\process.exe", "/s", "/e"
                                                   });
                AssertThat_Interpreter_IsSetupCorrectly(interpreter);
            }
        }

        [TestFixture]
        public class When_Passing_Each_File_and_CommandLine
        {
            [Test]
            public void Can_Return_Expected_CommandLine()
            {
                var interpreter = new CommandLineInterpreter(new[] {"-eachFile", @"c:\test\process.exe", "/s", "/e"});
                AssertThat_Interpreter_IsSetupCorrectly(interpreter);
            }
        }

        [TestFixture]
        public class When_Passing_Process_CommandLine_Only
        {
            [Test]
            public void Can_Return_Expected_CommandLine()
            {
                var interpreter = new CommandLineInterpreter(new[] {@"c:\test\process.exe", "/s", "/e"});
                AssertThat_Interpreter_IsSetupCorrectly(interpreter);
            }
        }

        [TestFixture]
        public class When_Passing_Process_Start_Path_and_CommandLine
        {
            [Test]
            public void Can_Return_Expected_CommandLine()
            {
                var interpreter =
                    new CommandLineInterpreter(new[] {"-startpath", @"c:\", @"c:\test\process.exe", "/s", "/e"});
                AssertThat_Interpreter_IsSetupCorrectly(interpreter);
            }
        }

        [TestFixture]
        public class When_Start_Path_Flag_Is_Set_To_A_Valid_Folder
        {
            [Test]
            public void Reports_Proper_Start_Folder()
            {
                const string PATH = @"c:\";

                var interpreter = new CommandLineInterpreter(new[] {"-startpath", PATH});
                Assert.That(interpreter.StartDirectory, Is.EqualTo(PATH));
            }
        }

        [TestFixture]
        public class When_Start_Path_Flag_Is_Set_To_An_Invalid_Folder
        {
            [Test]
            public void Reports_Invalid_Folder()
            {
                const string PATH = @"c:\this_folder_does_not_exist";

                Assert.That(Directory.Exists(PATH), Is.False, "PRECONDITION: Directory " + PATH + " should not exist!");

                var interpreter = new CommandLineInterpreter(new[] {"-startpath", PATH});

                string temp;
                Assert.Throws<ArgumentException>(() => temp = interpreter.StartDirectory);
            }
        }

        [Test]
        public void CanCreateInstance()
        {
            var args = new string[] {};
            var interpreter = new CommandLineInterpreter(args);

            Assert.That(interpreter, Is.Not.Null);
        }
    }
}