 using System;
using System.Runtime.CompilerServices;

namespace MatchingTest.Models
{

    public class DASolution
    {
        public List<Student> students;
        public List<Hospital> hospitals;
        public int n_iterations;
        public int n_matched;

        public DASolution() { }

        public List<(int, int)> GetMatching(List<Student> students)
        {
            List<(int, int)> values = new List<(int, int)>();
            foreach (Student student in students)
            {
                if (student.match != -1)
                {
                    (int, int) match = (student.match, student.student_id);
                    values.Add(match);
                }
            }
            return values;
        }
    }
}
