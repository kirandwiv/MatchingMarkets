using System.Collections.Concurrent;
using MatchingTest.Models;
using MatchingTest.Machines;
using System.Text.Json;

namespace MatchingTest
{
    internal class Program
    {
        static void NMain(string[] args)
        {
            int number_students = 300;
            int number_hospitals = 300;
            int depth_of_list = 15;

            int n_sims  = 2;
            var results = new ConcurrentBag<EADAMSolution>(); // Assuming EADAMSolution is the type of eadam_solution

            var eadam = new EADAM();

            results = eadam.solveEADAMParallel(number_students, number_hospitals, depth_of_list, n_sims, "results");

        }
    }
}