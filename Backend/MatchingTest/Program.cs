using System;
using System.Reflection;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: Program <ClassName> [additional arguments]");
            return;
        }

        string className = args[0];
        string[] classArgs = args[1..];

        try
        {
            // Get the type of the class
            Type type = Type.GetType($"MatchingTest.{className}");
            if (type == null)
            {
                Console.WriteLine($"Class '{className}' not found.");
                return;
            }

            // Get the Main method of the class
            MethodInfo mainMethod = type.GetMethod("Main", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (mainMethod == null)
            {
                Console.WriteLine($"Main method not found in class '{className}'.");
                return;
            }

            // Invoke the Main method
            mainMethod.Invoke(null, new object[] { classArgs });
        }
        catch (TargetInvocationException ex)
        {
            // Display the inner exception details
            Console.WriteLine($"Error invoking Main method of class '{className}': {ex.InnerException?.Message}");
            Console.WriteLine($"Stack Trace: {ex.InnerException?.StackTrace}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error invoking Main method of class '{className}': {ex.Message}");
        }
    }
}
