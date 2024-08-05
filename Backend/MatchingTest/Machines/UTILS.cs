
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
    }
}

