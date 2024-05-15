using MatchingTest.Models;
using MatchingTest.Machines;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingTest.Machines
{
    public class EADAM
    {
        public EADAM() { }

        public (List<Student> StudentList, List<Hospital> HospitalList) solveEADAM(List<Student> students, List<Hospital> hospitals, int depth_of_student_preferences) 
        {
            

            foreach (Hospital hospital in hospitals)
            {

            }



            //Check for Unmatched Students
            // Remove these students from student list as well.
            // Save them to another list!

            // This should leave us with only schools that rejected at least one student and the students matched to these schools. 
            
            // Run DA again. Be sure to reset the key variables. So for example, we'll need to reset the number of rejections (for both hospital and student)

            

            return (students, hospitals);
        }
    }
}
