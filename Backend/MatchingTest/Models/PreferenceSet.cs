using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingTest.Models
{
    public class FullPreferenceSet
    {
        //public List<> GeneratePreferencesFrom();

        public FullPreferenceSet() { }

        public (List<Student> StudentList, List<Hospital> HospitalList) GeneratePreferences(int number_of_students, int number_of_hospitals, int depth_student_preferences)
        {
            // First we create student preferences given the parameters
            List<Student> StudentList = new List<Student>();

            for (int i = 0; i < number_of_students; i++)
            {
                // For each student, choose random, ordered list of hospitals
                List<int> numbers = Enumerable.Range(0, number_of_hospitals).ToList();
                Random rand = new Random();
                List<int> selectedNumbers = numbers.OrderBy(x => rand.Next()).Take(depth_student_preferences).ToList();

                // Initiate instance of student
                Student student = new Student();
                student.student_id = i;
                // Add preferences 
                student.preferences = selectedNumbers;
                //Add Student to our list of students
                StudentList.Add(student);
            }

            List<Hospital> HospitalList = new List<Hospital>();
            for (int i = 0; i < number_of_hospitals; i++)
            {
                // For each hospital, choose random, ordered list of students
                List<int> numbers = Enumerable.Range(0, number_of_hospitals).ToList();
                Random rand = new Random();
                //Note that for hospitals we do not have a cut-off. 
                List<int> selectedNumbers = numbers.OrderBy(x => rand.Next()).ToList();

                // Create instance of hospital
                Hospital hospital = new Hospital();
                hospital.hospital_id = i;
                // Add Preference
                hospital.preferences = selectedNumbers;
                HospitalList.Add(hospital);
            }
            return (StudentList, HospitalList);
        }
    }

    public class Preferences_Result
    {
        public double[,] priority_array;
        public double[,] preference_array; 

        public Preferences_Result() { }
    }

    public class RandomPreferenceSet
    {
        //public List<> GeneratePreferencesFrom();

        public RandomPreferenceSet() { }

        public (Dictionary<int, Student> StudentDict, Dictionary<int, Hospital> HospitalDict, Preferences_Result Preferences) GeneratePreferences(int number_of_students, int number_of_hospitals, int depth_student_preferences)
        {
            // Initiate Student Dictionary and Hospital Dictionary
            Dictionary<int, Student> StudentDict = new Dictionary<int, Student>();
            Dictionary<int, Hospital> HospitalDict = new Dictionary<int, Hospital>();
            Preferences_Result Preferences = new Preferences_Result();

            //Initiate the Preference Array
            double[,] preference_array = new double[number_of_students, depth_student_preferences];

            for (int i = 0; i < number_of_students; i++)
            {
                List<int> numbers = Enumerable.Range(0, number_of_hospitals).ToList();
                Random rand = new Random();
                List<int> selectedNumbers = numbers.OrderBy(x => rand.Next()).Take(depth_student_preferences).ToList();

                for (int j = 0; j < depth_student_preferences; j++)
                {
                    preference_array[i, j] = selectedNumbers[j];
                }

                // Initiate instance of student and add to dictionary
                Student student = new Student
                {
                    student_id = i,
                    preferences = selectedNumbers
                };
                StudentDict.Add(i, student);
            }

            Preferences.preference_array = preference_array;

            // Create list of hospitals
            for (int i = 0; i < number_of_hospitals; i++)
            {
                List<int> numbers = Enumerable.Range(0, number_of_students).ToList(); // Corrected to range over students

                // Create instance of hospital and add to dictionary
                Hospital hospital = new Hospital
                {
                    hospital_id = i,
                };
                HospitalDict.Add(i, hospital);
            }

            return (StudentDict, HospitalDict, Preferences);
        }
    }
}