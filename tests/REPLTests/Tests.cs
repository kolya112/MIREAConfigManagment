using MIREAConfigManagment;
using MIREAConfigManagment.Commands;

namespace REPLTests
{
    public class Tests
    {
        [Fact]
        public void RandomTextCommandType()
        {
            var standartOut = Console.Out;
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                var rnd = new Random();
                string input = rnd.GetHexString(rnd.Next(50));

                CommandsEngine.Parse(input);

                Assert.Equal($"Command not found", stringWriter.ToString().Replace(Environment.NewLine, ""));
            }
            finally
            {
                Console.SetOut(standartOut);
            }
        }

        [Theory]
        [InlineData("cd", "cd")]
        [InlineData("cd --test-param", "cd --test-param")]
        [InlineData("cd --test-param1 --test-param2", "cd --test-param1 --test-param2")]
        public void CheckCdCommandResult(string input, string output)
        {
            var standartOut = Console.Out;
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                CommandsEngine.Parse(input);

                Assert.Equal(output, stringWriter.ToString().Replace(Environment.NewLine, ""));
            }
            finally
            {
                Console.SetOut(standartOut);
            }
        }

        [Theory]
        [InlineData("ls", "ls")]
        [InlineData("ls --test-param", "ls --test-param")]
        [InlineData("ls --test-param1 --test-param2", "ls --test-param1 --test-param2")]
        public void CheckLsCommandResult(string input, string output)
        {
            var standartOut = Console.Out;
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                CommandsEngine.Parse(input);

                Assert.Equal(output, stringWriter.ToString().Replace(Environment.NewLine, ""));
            }
            finally
            {
                Console.SetOut(standartOut);
            }
        }

        [Theory]
        [InlineData("echo $TEST1", "echo 12345")]
        [InlineData("echo params $TEST1", "echo params 12345")]
        [InlineData("echo $TEST2 params test", "echo 54321 params test")]
        [InlineData("echo params $TEST2 check test", "echo params 54321 check test")]
        public void TestEnvVars(string input, string output)
        {
            Environment.SetEnvironmentVariable("TEST1", "12345");
            Environment.SetEnvironmentVariable("TEST2", "54321");

            string result = Terminal.ResolveEnvVariables(input);

            Assert.Equal(output, result);
        }
    }
}
