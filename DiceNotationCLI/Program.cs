using System;
using DiceNotation;
using Serilog;

namespace DiceNotationCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            string user_input = "";
            Console.WriteLine("Enter an expression and it'll be evaluated (Enter \"exit\" to quit):");
            Console.Write("> ");
            user_input = Console.ReadLine();
            while (user_input != "exit")
            {
                try
                {
                    IDiceParser parser = new DiceParser();
                    DiceExpression dice = parser.Parse(user_input);
                    DiceResult result = dice.Roll();
                    Console.WriteLine(user_input + ": " + result.Value);
                    Log.Debug("Original input: {0}", user_input);
                    Log.Debug("Reconstructed input: {0}", dice.ToString());
                    Log.Debug("Roller used: {0}", result.RollerUsed);
                    Log.Debug("Results: {@Results}", result.Results);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                Console.Write("> ");
                user_input = Console.ReadLine();
            }
            System.Environment.Exit(1);
        }
    }
}
