using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp.yufatang
{
    public class User
    {
        /// <summary>
        /// 自动属性
        /// </summary>
        public string Name { get; set; }
        public string LoginName { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public string Password { get; set; }

        public string DeptId { get; set; }


        public User()
        {
        }

        //构造函数重载


        public User(string name, string password,int age, string deptId)
        {
            this.Name = name;
            this.Password = password;
            this.Age = age;
            this.DeptId = deptId;
        }

        /// <summary>
        /// 默认参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loginName"></param>
        /// <param name="age"></param>
        /// <param name="address"></param>
        /// <param name="password"></param>
        public User(string name, string loginName, int age, string address = "上海", string password = "1234")
        {
            this.Name = name;
            this.LoginName = loginName;
            this.Age = age;
            this.Address = address;
            this.Password = password;
        }


    }

}
