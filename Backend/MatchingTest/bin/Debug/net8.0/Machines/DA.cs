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

        public DASolution solveDA(Dictionary<int, Student> students, Dictionary<int, Hospital> hospitals, int depth_of_student_preferences)
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
                foreach (var studentEntry in students)
                {
                    Student student = studentEntry.Value;
                    if (!student.IsUnmatched(depth_of_student_preferences))
                    {
                        // For student, find proposal made.1
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
                foreach (var hospitalEntry in hospitals)
                {
                    Hospital hospital = hospitalEntry.Value;
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

        public DASolution SolveDAExpress(Dictionary<int, Student> students, Dictionary<int, Hospital> hospitals, int depth_of_student_preferences)
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
                        double priority_score = 0;

                        // Check if the hospital already has proposal from student, if not create score:
                        if (!relevant_hospital.priority_map.ContainsKey(student.student_id))
                        {
                            Random random = new Random();
                            priority_score = random.NextDouble();
                            relevant_hospital.priority_map.Add(student.student_id, priority_score);
                        }
                        else
                        {
                            priority_score = relevant_hospital.priority_map[student.student_id];
                        }
                        // Check if the hospital has a match, if not assign student to hospital.
                        if (relevant_hospital.match == -1)
                        {
                            relevant_hospital.match = student.student_id;
                            student.match = relevant_hospital.hospital_id;
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
                                student.match = relevant_hospital.hospital_id;
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

            solution.hospitals = hospitals;
            solution.students = students;
            solution.n_iterations = iteration;
            solution.perma_unmatched = permanently_unmatched;
            solution.n_matched = students.Count - permanently_unmatched.Count;

            return solution;
        }
    }
}
