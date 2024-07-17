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

        public DASolution SolveDAExpress(Dictionary<int, Student> students, Dictionary<int, Hospital> hospitals, int depth_of_student_preferences, Preferences_Result preferences)
        {
            DASolution solution = new DASolution();
            Dictionary<int, Student> tentatively_unmatched = new Dictionary<int, Student>(students);
            Dictionary<int, Student> permanently_unmatched = new Dictionary<int, Student>();
            int iteration = 0;

            while (tentatively_unmatched.Count > 0)
            {
                iteration++;
                Dictionary<int, Student> temp_list = new Dictionary<int, Student>(tentatively_unmatched);

                foreach (var studentEntry in tentatively_unmatched)
                {
                    Student student = studentEntry.Value;
                    temp_list.Remove(student.student_id);

                    if (student.rejections == depth_of_student_preferences)
                    {
                        permanently_unmatched.Add(student.student_id, student);
                        student.permanentely_unmatched = true;
                        student.match = -1;
                    }
                    else
                    {
                        int proposes_to = student.preferences[student.rejections];
                        Hospital relevant_hospital = hospitals[proposes_to];
                        relevant_hospital.n_proposals_rd += 1;

                        Random random = new Random();
                        double priority_score = random.NextDouble();
                        relevant_hospital.priority_map.Add(student.student_id, priority_score);

                        if (relevant_hospital.match == -1)
                        {
                            relevant_hospital.match = student.student_id;
                        }
                        else
                        {
                            double competing_score = relevant_hospital.priority_map[relevant_hospital.match];
                            if (competing_score < priority_score)
                            {
                                Student loser_student = students[relevant_hospital.match];
                                loser_student.rejections += 1;
                                temp_list.Add(loser_student.student_id, loser_student);

                                relevant_hospital.match = student.student_id;
                                relevant_hospital.rejected += 1;
                            }
                            else
                            {
                                temp_list.Add(student.student_id, student);
                                student.rejections += 1;
                                relevant_hospital.rejected += 1;
                            }
                        }
                    }
                }
                tentatively_unmatched = new Dictionary<int, Student>(temp_list);
            }

            solution.hospitals = hospitals.Values.ToList();
            solution.students = students.Values.ToList();
            solution.n_iterations = iteration;
            solution.perma_unmatched = permanently_unmatched.Values.ToList();
            solution.n_matched = students.Count - permanently_unmatched.Count;

            return solution;
        }
    }
}
