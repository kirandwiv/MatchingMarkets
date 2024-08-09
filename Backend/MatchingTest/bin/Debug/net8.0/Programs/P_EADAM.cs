using System.Collections.Concurrent;
using MatchingTest.Models;
using MatchingTest.Machines;
using System.Text.Json;

namespace MatchingTest
{
    internal class P_EADAM
    {
        public static void Main(string[] args)
        {
            if (args.Length != 5)
            {
                Console.WriteLine("Usage: Program <number_students> <number_hospitals> <depth_of_list> <n_sims>");
                return;
            }
            if (!int.TryParse(args[0], out int number_students) ||
                !int.TryParse(args[1], out int number_hospitals) ||
                !int.TryParse(args[2], out int depth_of_list) ||
                !int.TryParse(args[3], out int n_sims))
            {
                Console.WriteLine("Invalid arguments. Please provide integers for all parameters. String for Filename");
                return;
            }

            string filename = args[4];
            
            var results = new List<List<int[,]>>();

            var eadam = new EADAM();

            results = eadam.Alternative(number_students, number_hospitals, depth_of_list, n_sims, filename);

        }
    }
}