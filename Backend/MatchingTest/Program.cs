using MatchingTest.Models;

namespace MatchingTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int number_students = 5000;
            int number_hospitals = 5000;
            int depth_of_list = 7;

            var preference_set = new PreferenceSet();
            var students = new List<Student>();
            var hospitals = new List<Hospital>();

            var solution = preference_set.GenerateRandomPreferences(number_students, number_hospitals, depth_of_list);
            students = solution.StudentList;
            hospitals = solution.HospitalList;
            Console.WriteLine(students[0].preferences[0]);
        }
    }
}