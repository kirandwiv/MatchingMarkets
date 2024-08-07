using MatchingTest.Models;
using MatchingTest.Machines;
using MatchingTest.Utilities;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.Reflection.Metadata;

namespace MatchingTest.Machines
{
    public class EADAM
    {
        
        public EADAM() {}

        public EADAMSolution solveEADAM(Dictionary<int, Student> students, Dictionary<int, Hospital> hospitals, int depth_of_student_preferences, string filename) 
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
            DASolution initial_matching = solver.SolveDAExpress(students, hospitals, depth_of_student_preferences);
            // Run GetMatching to get the matching list.
            solution.da_matching_list = MatchingUtils.GetMatching(initial_matching.students);;
            
            Console.WriteLine("Initial Matching: " + initial_matching.n_matched);
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
                        Student relevant_student = students_t[hospital.match];
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
                // Console.WriteLine("Hospitals/Agents Still in: " + to_keep_hospitals.Count);
                // We now have a list of hospitals and students that we want to run DA on again.
                DASolution matching_t = solver.SolveDAExpress(to_keep_students, to_keep_hospitals, depth_of_student_preferences);
                // Update the hospitals list in hospital_t
                hospitals_t = matching_t.hospitals;
                // Update the students list in students_t
                students_t = matching_t.students;
                // Update the number of iterations:
                solution.n_iterations += matching_t.n_iterations;
            }

            solution.students = students;
            solution.hospitals = hospitals;

            // Run GetMatching to get the matching list.
            solution.eadam_matching_list = MatchingUtils.GetMatching(students);

            return solution;
        }

        public ConcurrentBag<Dictionary<string, object>> solveEADAMParallel(int number_of_students, int number_of_hospitals, int depth_of_list,  int n_sims, string filename)
        {
            var results = new ConcurrentBag<Dictionary<string, object>>();
            // Set the minimum number of threads in the ThreadPool
            ThreadPool.SetMinThreads(Environment.ProcessorCount * 3, Environment.ProcessorCount * 3);

            // Create an instance of ParallelOptions
            var parallelOptions = new ParallelOptions
            {
                // Set the maximum degree of parallelism to the number of processors
                MaxDegreeOfParallelism = Environment.ProcessorCount*3
            };

            Parallel.For(0, n_sims, parallelOptions, i =>
            {
                var preference_set = new RandomPreferenceSet();
                var students = new Dictionary<int, Student>();
                var hospitals = new Dictionary<int, Hospital>();
                var preferences = new int[,] { };
                var to_be_saved = new Dictionary<string, object>();
                

                var solution = preference_set.GeneratePreferences(number_of_students, number_of_hospitals, depth_of_list);
                students = solution.StudentDict;
                hospitals = solution.HospitalDict;
                preferences = solution.Preferences.preference_array;

                var eadam = new EADAM();
                var eadam_solution = eadam.solveEADAM(students, hospitals, depth_of_list, filename);

                // Add relevant variables to the dictionary to be saved.
                to_be_saved.Add("eadam_matching", eadam_solution.eadam_matching_list);
                to_be_saved.Add("da_matching", eadam_solution.da_matching_list);
                // to_be_saved.Add("preference_array", preferences);

                results.Add(to_be_saved);
            });
            // Save results to JSON
            string path = "../../Data/" + filename + "_eadam" + ".json";
            //inefficient but simple
            //File.WriteAllText(path, JsonConvert.SerializeObject(results));
            //stream directly to file
            using (var stream = File.CreateText(path))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(stream, results);
            }
            return results;
        }
    }
}
