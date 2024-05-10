 using System;
using System.Runtime.CompilerServices;

namespace MatchingTest.Models
{

    public class Student
    {
        public List<int> preferences;
        public int match;
        public bool permanentely_unmatched;
        public int rejections;
        public int student_id;

        public Student()
        {
            rejections = 0;
            match = -1;
            permanentely_unmatched = false;
        }

        /// <summary>
        /// Returns true if the student has exhausted their preference list.
        /// </summary>
        /// <param name="depth_student_preferences"></param> The depth of the students preferences.
        /// <returns></returns>
        public bool IsUnmatched(int depth_student_preferences)
        {
            if (rejections < depth_student_preferences)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class Hospital
    {
        public List<int> preferences;
        public int match;
        public List<int> proposals;
        public int rejected;
        public int n_proposals_rd;
        public int hospital_id;

        public Hospital()
        {
            preferences = new List<int>();
            proposals = new List<int>();
        }

        /// <summary>
        /// Finds the Hospitals preference over the set of proposals it receives. 
        /// </summary>
        public void FindChoice()
        {
            int FirstChoice = preferences.Intersect(proposals).First();
            match = FirstChoice;
        }
    }
}
