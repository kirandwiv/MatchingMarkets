using MatchingTest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingTest.Machines
{
    public class DA
    {
        public DA() { }

        public DASolution solveDA(List<Student> students, List<Hospital> hospitals, int depth_of_student_preferences)
        {
            bool exhausted = false;
            DASolution solution = new DASolution();

            int iteration = 0;
            int permanentely_unmatched = 0;
            int n_matched = 0;
            while (exhausted == false)
            {
                iteration++;
                Console.WriteLine(iteration);
                Console.WriteLine(n_matched);
                foreach (Student student in students)
                {
                    if (!(student.IsUnmatched(depth_of_student_preferences)))
                    {
                        // For student, find proposal made.
                        int proposes_to = student.preferences[student.rejections];
                        // Add that student to the hospitals list of proposals under consideration.
                        hospitals[proposes_to].proposals.Add(student.student_id);
                        // Keep track of number of proposals made to the hospital.
                        hospitals[proposes_to].n_proposals_rd += 1;
                    }
                    else
                    {
                        if (student.permanentely_unmatched == false)
                        {
                            student.permanentely_unmatched = true;
                            permanentely_unmatched++;
                        }
                    }
                }
                //We now have a list of hospitals, each with proposals (or none).
                foreach (Hospital hospital in hospitals)
                {
                    // Three possibilities: no proposals, one proposal, many proposals.

                    //1. No proposal: nothing to update, continue.
                    if (hospital.n_proposals_rd == 0)
                    {
                        continue;
                    }
                    //2. One offer: automatic accept of the one offer.
                    else if (hospital.n_proposals_rd == 1)
                    {
                        hospital.match = hospital.proposals[0];
                        students[hospital.proposals[0]].match = hospital.hospital_id;
                        n_matched++;
                        hospital.n_proposals_rd = 0;
                        hospital.proposals = new List<int>();
                    }
                    //3. >1 Offer: then have to find favorite and assign it. Remove others. 
                    else
                    {
                        n_matched++;
                        //Find the top choice among proposals for hospital.
                        hospital.FindChoice();
                        for (int i = 0; i < hospital.proposals.Count; i++)
                        {

                            if (hospital.proposals[i] != hospital.match)
                            {
                                students[hospital.proposals[i]].rejections += 1;
                                students[hospital.proposals[i]].match = -1;
                                hospital.rejected += 1;
                            }
                            else
                            {
                                students[hospital.proposals[i]].match = hospital.hospital_id;
                            }
                        }
                        hospital.n_proposals_rd = 0;
                        hospital.proposals = new List<int>();
                    }
                }
                Console.WriteLine(permanentely_unmatched);
                Console.WriteLine(n_matched);
                if (n_matched + permanentely_unmatched == students.Count)
                {
                    exhausted = true;
                }
                n_matched = 0;
            }
            solution.students = students;
            solution.hospitals = hospitals;
            solution.n_iterations = iteration;
            solution.n_matched = n_matched;
            return solution;
        }

        public DASolution SolveDAExpress(List<Student> students, List<Hospital> hospitals, int depth_of_student_preferences, Preferences_Result preferences)
        {
            // We create a list of tentatively unmatched students, and an empty list of permanently unmatched students.
            DASolution solution = new DASolution(); 
            List<Student> tentatively_unmatched = students;
            List<Student> permanently_unmatched = new List<Student>();
            int iteration = 0;
            int n_matched = 0;

            // While tentatively_unmatched is not empty, we proceed with a further round of proposals.
            while (!(tentatively_unmatched.Count() == 0))
            {
                iteration++;
                List<Student> temp_list = tentatively_unmatched.ToList();// Create a copy of our tentatively unmatched list from which we can remove. 

                foreach (Student student in tentatively_unmatched)
                {

                    temp_list.Remove(student); // Temporarily remove the student from the list.

                    if (student.rejections == depth_of_student_preferences)
                    {
                        permanently_unmatched.Add(student); // Since the student has exhausted his possibilities, we add them to the list of lost souls. 
                        student.permanentely_unmatched = true;
                        student.match = -1;
                    }
                    else
                    {
                        int proposes_to = student.preferences[student.rejections]; //Find hospital the student proposes to next. Could also do this using the preference_array.

                        Hospital relevant_hospital = hospitals[proposes_to];
                        relevant_hospital.n_proposals_rd += 1;

                        // Create priority score for student at relevant_hospital
                        // Here we'll need to check if he already exists in the mapping. 

                        Random random = new Random();
                        double priority_score = random.NextDouble();

                        // Enter this into the hospital's priority map:
                        relevant_hospital.priority_map.Add(student.student_id, priority_score);

                        if (relevant_hospital.match == -1) // Then the hospital is as of yet unmatched
                        {
                            relevant_hospital.match = student.student_id;
                        }
                        else
                        {
                            // Get current match priority
                            double competing_score = relevant_hospital.priority_map[relevant_hospital.match];
                            if (competing_score < priority_score)
                            {
                                // We start by adding the loser to the temp_list
                                var loser_student = students[relevant_hospital.match];
                                loser_student.rejections += 1;
                                temp_list.Add(students[relevant_hospital.match]);


                                // We update the hospital's match
                                relevant_hospital.match = student.student_id;
                            }
                            else
                            {
                                temp_list.Add(student); // Add the student back to the temp_list. 
                                student.rejections += 1;
                            }
                        }
                    }
                }
                // Now we update the tentatively unmatched list to be equal to the temp_list. 
                tentatively_unmatched = temp_list;
            }
            solution.hospitals = hospitals;
            solution.students = students;
            solution.n_iterations = iteration;
            solution.perma_unmatched = permanently_unmatched;
            solution.n_matched = students.Count() - permanently_unmatched.Count();

            return solution;
        }
    }
}
