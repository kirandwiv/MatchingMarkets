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

            var preference_set = new PreferenceSet();
            var students = new List<Student>();
            var hospitals = new List<Hospital>();

            var solution = preference_set.GenerateRandomPreferences(number_students, number_hospitals, depth_of_list);
            students = solution.StudentList;
            hospitals = solution.HospitalList;
            Console.WriteLine(string.Join(',', hospitals[4999].preferences));

            var deferred_acceptance = new DA();
            var DAoutcomes = deferred_acceptance.solveDA(students, hospitals, depth_of_list);
        }
    }
}