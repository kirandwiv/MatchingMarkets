using MatchingTest.Models;
using MatchingTest.Machines;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingTest.Machines
{
    public class EADAM
    {
        public EADAM() { }

        public EADAMSolution solveEADAM(Dictionary<int, Student> students, Dictionary<int, Hospital> hospitals, int depth_of_student_preferences, Preferences_Result preferences) 
        {
            
            // 1. Preference List is an input.
            // 2. Run DA solver to obtain initial matching. Save initial matching. 
            // 3. Remove unmatched hospitals and unmatched students from the list. This leaves us with two lists.
            // 4. Check for underdemanded schools/hospitals. Remove these and the students matched to them from the list. 
            // 5. For all students/hospitals left reset the number of rejections + the matchings
            // 6. Run DA again.

            EADAMSolution solution = new EADAMSolution();
            solution.n_iterations = 0;
            solution.n_matched = 0;

            // Run DA Solver for initial matching
            DA solver = new DA();
            DASolution initial_matching = solver.SolveDAExpress(students, hospitals, depth_of_student_preferences, preferences);
            solution.initial_matching = initial_matching;

            Dictionary<int, Hospital> hospitals_t = initial_matching.hospitals;
            Dictionary<int, Student> students_t = initial_matching.students;

            while (hospitals_t.Count > 0)
            {
                // Remove Underdemanded Hospitals + Their Matchings. Keep only oversubscribed hospitals and their matchings.
                Dictionary<int, Hospital> to_keep_hospitals = new Dictionary<int, Hospital>();
                Dictionary<int, Student> to_keep_students = new Dictionary<int, Student>();
                foreach (var hospitalEntry in hospitals_t)
                {
                    Hospital hospital = hospitalEntry.Value;
                    int hospital_index = hospitalEntry.Key;
                    if (hospital.rejected > 0)
                    {
                        // Get the student matched to this hospital.
                        Student relevant_student = initial_matching.students[hospital.match];
                        // Reset the number of rejections for this hospital, as well as the number of rejections for the student. Essentially we want to start fresh but keep the preferences. 
                        hospital.rejected = 0;
                        hospital.match = -1;
                        // Reset the number of rejections for the student.
                        relevant_student.rejections = 0;
                        relevant_student.match = -1;

                        // Add the Hospital to the list of hospitals to keep.
                        to_keep_hospitals.Add(hospital_index, hospital);
                        // Add the students matched to this hospital to the list of students to keep.
                        to_keep_students.Add(relevant_student.student_id, relevant_student);
                    }
                }
                Console.WriteLine("Hospitals/Agents Still in: " + to_keep_hospitals.Count);
                // We now have a list of hospitals and students that we want to run DA on again.
                DASolution matching_t = solver.SolveDAExpress(to_keep_students, to_keep_hospitals, depth_of_student_preferences, preferences);
                // Update the hospitals list in hospital_t
                hospitals_t = matching_t.hospitals;
                // Update the number of iterations:
                solution.n_iterations += matching_t.n_iterations;
                // Clear the to_keep_hospitals and to_keep_students lists.
                to_keep_hospitals.Clear();
                to_keep_students.Clear();
            }

            solution.students = students;
            solution.hospitals = hospitals;

            return solution;
        }
    }
}
