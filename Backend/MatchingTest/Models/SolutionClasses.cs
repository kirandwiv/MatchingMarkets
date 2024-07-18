 using System;
using System.Runtime.CompilerServices;

namespace MatchingTest.Models
{

    public class DASolution
    {
        public Dictionary<int, Student> students;
        public Dictionary<int, Hospital> hospitals;
        public int n_iterations;
        public int n_matched;
        public Dictionary<int, Student> perma_unmatched;

        public DASolution() { }

        public List<(int, int)> GetMatching(Dictionary<int, Student> students)
        {
            List<(int, int)> values = new List<(int, int)>();
            foreach (KeyValuePair<int, Student> item in students)
            {
                if (item.Value.match != -1)
                {
                    (int, int) match = (item.Value.match, item.Value.student_id);
                    values.Add(match);
                }
            }
            return values;
        }
    }

    public class EADAMSolution
    {
        public Dictionary<int, Student> students;
        public Dictionary<int, Hospital> hospitals;
        public int n_iterations;
        public int n_matched;
        public DASolution initial_matching;

        public EADAMSolution() { }

        public List<(int, int)> GetMatching(Dictionary<int, Student> students)
        {
            List<(int, int)> values = new List<(int, int)>();
            foreach (KeyValuePair<int, Student> item in students)
            {
                if (item.Value.match != -1)
                {
                    (int, int) match = (item.Value.match, item.Value.student_id);
                    values.Add(match);
                }
            }
            return values;
        }
    }
}
