
using MatchingTest.Models;
using MatchingTest.Machines;
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

namespace MatchingTest.Utilities
{
    public static class MatchingUtils
    {
        public static int[,] GetMatching(Dictionary<int, Student> students)
        {
            int[,] matching = new int[students.Count, 2];
            foreach (KeyValuePair<int, Student> student in students)
            {
                matching[student.Value.student_id, 1] = student.Value.match;
                matching[student.Value.student_id, 0] = student.Value.student_id;
            }
            return matching;
        }
        

        public static (int, int) NDifferences(int[,] matching1, int[,] matching2)
        {
            int n = 0;
            int k = 0;
            for (int i = 0; i < matching1.GetLength(0); i++)
            {
                if (matching1[i, 1] != matching2[i, 1])
                {
                    n++;
                }
                if (matching1[i, 1] == -1)
                {
                    k++;
                }
            }
            return (n, k);
        }

        public static int NUnmatched(int[,] matching)
        {
            int n = 0;
            for (int i = 0; i < matching.GetLength(0); i++)
            {
                if (matching[i, 1] == -1)
                {
                    n++;
                }
            }
            return n;
        }
    }
}

