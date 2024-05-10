using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingTest.Models
{
    public class PreferenceSet
    {
        //public List<> GeneratePreferencesFrom();

        public PreferenceSet() { }

        public (List<Student> StudentList, List<Hospital> HospitalList) GenerateRandomPreferences(int number_of_students, int number_of_hospitals, int depth_student_preferences)
        {
            // First we create student preferences given the parameters
            List<Student> StudentList = new List<Student>();
            for (int i = 0;  i < number_of_students; i++)
            {
                // For each student, choose random, ordered list of hospitals
                List<int> numbers = Enumerable.Range(0, number_of_hospitals).ToList();
                Random rand = new Random();
                List<int> selectedNumbers = numbers.OrderBy(x => rand.Next()).Take(depth_student_preferences).ToList();

                // Initiate instance of student
                Student student = new Student();
                // Add preferences 
                student.preferences = selectedNumbers;
                //Add Student to our list of students
                StudentList.Add(student);
            }

            List<Hospital> HospitalList = new List<Hospital>();
            for (int i = 0; i < number_of_students; i++)
            {
                // For each hospital, choose random, ordered list of students
                List<int> numbers = Enumerable.Range(0, number_of_hospitals).ToList();
                Random rand = new Random();
                //Note that for hospitals we do not have a cut-off. 
                List<int> selectedNumbers = numbers.OrderBy(x => rand.Next()).ToList();

                // Create instance of hospital
                Hospital hospital = new Hospital();
                // Add Preference
                hospital.preferences = selectedNumbers;
            }
            return (StudentList, HospitalList);
        }
    }
}