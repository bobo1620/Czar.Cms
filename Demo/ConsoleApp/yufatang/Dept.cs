using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.yufatang
{
    public class Dept
    {
        public string DeptId { get; set; }

        public string DeptName { get; set; }

        public int PepNum { get; set; }

        public Dept() { }

        public Dept(string deptid, string deptName, int pepNum) 
        {
            this.DeptId = deptid;
            this.DeptName = deptName;
            this.PepNum = pepNum;
        }


    }
}
