using System.Collections.Concurrent;
using MatchingTest.Models;
using MatchingTest.Machines;

namespace MatchingTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int number_students = 10000;
            int number_hospitals = 10000;
            int depth_of_list = 15;

            int n_sims  = 2;
            var results = new ConcurrentBag<EADAMSolution>(); // Assuming EADAMSolution is the type of eadam_solution

            Parallel.For(0, n_sims, i =>
            {
                var preference_set = new RandomPreferenceSet();
                var students = new Dictionary<int, Student>();
                var hospitals = new Dictionary<int, Hospital>();
                var preferences = new Preferences_Result();

                var solution = preference_set.GeneratePreferences(number_students, number_hospitals, depth_of_list);
                students = solution.StudentDict;
                hospitals = solution.HospitalDict;
                preferences = solution.Preferences;

                var eadam = new EADAM();
                var eadam_solution = eadam.solveEADAM(students, hospitals, depth_of_list, preferences);
                eadam_solution.GetMatching(eadam_solution.students);

                results.Add(eadam_solution);
            });

            // var preference_set = new RandomPreferenceSet();
            // var students = new Dictionary<int, Student>();
            // var hospitals = new Dictionary<int, Hospital>();
            // var preferences = new Preferences_Result();

            // var solution = preference_set.GeneratePreferences(number_students, number_hospitals, depth_of_list);
            // students = solution.StudentDict;
            // hospitals = solution.HospitalDict;
            // preferences = solution.Preferences;
            // Console.WriteLine(string.Join(", ", students[0].preferences));
            
            // // var solver = new DA();

            // // var matching = solver.SolveDAExpress(students, hospitals, depth_of_list, preferences);
            // // Console.WriteLine(matching.n_matched);
            // // Console.WriteLine(matching.n_iterations);
            // // Console.WriteLine(string.Join(", ", hospitals[0].match));


            // var eadam = new EADAM();
            // var eadam_solution = eadam.solveEADAM(students, hospitals, depth_of_list, preferences);
            // eadam_solution.GetMatching(eadam_solution.students);
            // // matching.GetMatching(matching.students);
        }
    }
}