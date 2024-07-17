using MatchingTest.Models;
using MatchingTest.Machines;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// namespace MatchingTest.Machines
// {
//     public class EADAM
//     {
//         public EADAM() { }

//         public EADAMSolution solveEADAM(List<Student> students, List<Hospital> hospitals, int depth_of_student_preferences, Preferences_Result preferences) 
//         {
            
//             // 1. Preference List is an input.
//             // 2. Run DA solver to obtain initial matching. 
//             // 3. Remove unmatched hospitals and unmatched students from the list. This leaves us with two lists.
//             // 4. Check for underdemanded schools/hospitals. Remove these and the students matched to them from the list. 
//             // 5. For all students/hospitals left reset the number of rejections + the matchings
//             // 6. Run DA again.

//             EADAMSolution solution = new EADAMSolution();
//             solution.n_iterations = 0;
//             solution.n_matched = 0;

//             // Run DA Solver for initial matching
//             DA solver = new DA();
//             DASolution initial_matching = solver.SolveDAExpress(students, hospitals, depth_of_student_preferences, preferences);
//             solution.initial_matching = initial_matching;

//             List<Hospital> hospitals_t = initial_matching.hospitals;
//             List<Student> students_t = initial_matching.students;

//             while (hospitals_t.Count > 0)
//             {
//                 // Remove Underdemanded Hospitals + Their Matchings. Keep only oversubscribed hospitals and their matchings.
//                 List<Hospital> underdemanded_hospitals = new List<Hospital>();
//                 List<Hospital> to_keep_hospitals = new List<Hospital>();
//                 List<Student> to_keep_students = new List<Student>();
//                 foreach (Hospital hospital in hospitals_t)
//                 {
//                     if (hospital.rejected == 0)
//                     {
//                         // Add the Hospital to the list of underdemanded hospitals.
//                         underdemanded_hospitals.Add(hospital);
//                     }
//                     else
//                     {
//                         // Reset the number of rejections for this hospital, as well as the number of rejections for the student. Essentially we want to start fresh but keep the preferences. 
//                         hospital.rejected = 0;
//                         hospital.match = -1;

//                         Student relevant_student = initial_matching.students[hospital.match];
//                         relevant_student.rejections = 0;
//                         relevant_student.match = -1;

//                         // Add the Hospital to the list of hospitals to keep.
//                         to_keep_hospitals.Add(hospital);
//                         // Add the students matched to this hospital to the list of students to keep.
//                         to_keep_students.Add(initial_matching.students[hospital.match]);
//                     }
//                 }
//                 Console.WriteLine("Underdemanded Hospitals: " + underdemanded_hospitals.Count);
//                 // We now have a list of hospitals and students that we want to run DA on again.
//             }

//             return solution;
//         }
//     }
// }
