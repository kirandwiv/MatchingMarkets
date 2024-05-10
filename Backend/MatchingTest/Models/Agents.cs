 using System;

namespace MatchingTest.Models
{

    public class Student
    {
        public List<int> preferences;
        public int match;
        public int rejections;

    }

    public class Hospital
    {
        public List<int> preferences;
        public int match;
        public List<int> proposals;
        public int rejected;
    }
}
