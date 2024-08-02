using System.Collections.Concurrent;
using MatchingTest.Models;
using MatchingTest.Machines;
using Newtonsoft.Json;

namespace MatchingTest
{
    internal class Tests
    {
        static void Main(string[] args)
        {
            int number_students = 4;
            int number_hospitals = 4;
            int depth_of_list = 4;

            var results = new EADAMSolution(); 
            var eadam = new EADAM();


            Dictionary<int, Student> student_dict = new Dictionary<int, Student>();
            Dictionary<int, Hospital> hospital_dict = new Dictionary<int, Hospital>();

            Student student1 = new Student();
            student1.student_id = 0;
            student1.preferences = new List<int> { 0, 1, 2, 3 };
            student_dict.Add(0, student1);

            Student student2 = new Student();
            student2.student_id = 1;
            student2.preferences = new List<int> { 1, 2, 0, 3 };
            student_dict.Add(1, student2);

            Student student3 = new Student();
            student3.student_id = 2;
            student3.preferences = new List<int> { 2, 0, 1, 3 };
            student_dict.Add(2, student3);

            Student student4 = new Student();
            student4.student_id = 3;
            student4.preferences = new List<int> { 0, 3, 2, 1 };
            student_dict.Add(3, student4);

            Hospital hospital1 = new Hospital();
            hospital1.hospital_id = 0;
            hospital1.priority_map = new Dictionary<int, double> { { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 2 } };
            hospital_dict.Add(0, hospital1);

            Hospital hospital2 = new Hospital();
            hospital2.hospital_id = 1;
            hospital2.priority_map = new Dictionary<int, double> { { 0, 2 }, { 1, 1 }, { 2, 3 }, { 3, 2 } };
            hospital_dict.Add(1, hospital2);

            Hospital hospital3 = new Hospital();
            hospital3.hospital_id = 2;
            hospital3.priority_map = new Dictionary<int, double> { { 0, 3 }, { 1, 2 }, { 2, 1 }, { 3, 2 } };
            hospital_dict.Add(2, hospital3);

            Hospital hospital4 = new Hospital();
            hospital4.hospital_id = 3;
            hospital4.priority_map = new Dictionary<int, double> { { 0, 2 }, { 1, 2 }, { 2, 3 }, { 3, 1 } };
            hospital_dict.Add(3, hospital4);

            results = eadam.solveEADAM(student_dict, hospital_dict, depth_of_list, "test1");

            Console.WriteLine(JsonConvert.SerializeObject(results));

        }
    }
}