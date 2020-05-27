using ConsoleApp.yufatang;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp
{
    //https://www.cnblogs.com/dotnet261010/p/6052829.html 委托
    //https://www.cnblogs.com/dotnet261010/p/6055092.html  语法糖
    //https://www.cnblogs.com/LipeiNet/p/4694225.html   action  func
    //1：Action用于没有返回值的方法（参数可以根据自己情况进行传递）
    //2：Func恰恰相反用于有返回值的方法（同样参数根据自己情况情况）
    //3：记住无返回就用action，有返回就用Func

    class Program
    {
        #region  委托及方法声明
        //例子：public delegate void MyDelegate(int number);//定义了一个委托MyDelegate,它可以注册 返回void类型且有一个int作为参数的函数
        //不带参数委托
        private delegate void buybook1();
        public static void Book1()
        {
            Console.WriteLine("ok");
        }

        //带参数委托
        private delegate void buybook(string name);
        public static void Book(string name)
        {
            Console.WriteLine("ok"+name);
        }

        public delegate string MyDelegate(string name);
        #endregion




        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            //Console.WriteLine("Hello World!" + StringExt.IsNumber("aaa") );

            List<User> listUser = new List<User>()
            {
                new User(){Name="张三",Password="1234",Age=12,DeptId="0001"},
                new User(){Name="张四",Password="1234",Age=16,DeptId="0002"},
                new User(){Name="张五",Password="1234",Age=29,DeptId="0003"},
                new User(){Name="张六",Password="1234",Age=18,DeptId="0001"},
                new User(){Name="张七",Password="1234",Age=12,DeptId="0001"}
            };

            List<Dept> listDept = new List<Dept>()
            {
                new Dept(){DeptId="0001",DeptName="人事部",PepNum=10},
                new Dept(){DeptId="0002",DeptName="财务部",PepNum=7},
                new Dept(){DeptId="0003",DeptName="行政部",PepNum=15}
            };

            //扩展集合方法 使用自定义委托
            listUser.TEach(PrintUser);

            //使用内置泛型委托
            listUser.ForEach(PrintUser);

            //使用匿名方法
            listUser.ForEach(delegate (User u) { Console.WriteLine(u.Name + " " + u.Password); });

            //使用Lambda表达式
            listUser.ForEach(p => Console.WriteLine(p.Name + " " + p.Password ));

            listDept.ForEach(new Action<Dept>(delegate (Dept d)
            {
                Console.WriteLine(d.DeptId + " " + d.DeptName + " " + d.PepNum);
            }));



            #region Lambda表达式
            MyDelegate my1 = (string name) => { return "Lambda表达式:hello" + name; };

            
            #endregion



            //======================
            #region 委托调用及action简写
            buybook1 bk1 = new buybook1(Book1);
            bk1();

            buybook bk = new buybook(Book);
            bk("aaaa");



            //action 用法
            //这样使用可以避免声明委托，只需定义委托的方法 并且支持多参数
            Action action = new Action(Book1);
            action();

            Action<string> action2 = new Action<string>(Book);
            action2("bbbb");
            #endregion

            #region func
            //func 用法
            //Func 解释 封装一个不一定具有参数（也许没有）但却返回 TResult 参数指定的类型值的方法。

            //没有参数只有返回值的方法 ->FuncBook  Func<string> 返回类型<?>取决于FuncBook方法的返回类型
            Func<string> RetBook = new Func<string>(FuncBook);
            Console.WriteLine(RetBook);

            Func<int> RetBook2 = new Func<int>(FuncBook2);
            Console.WriteLine(RetBook2);

            Func<string, string> RetBook3 = new Func<string,string>(FuncBook3);
            Console.WriteLine(RetBook3("aaa"));

            Func<string> funcValue = delegate
            {
                return "我是即将传递的值3";
            };
            DisPlayValue(funcValue);
            #endregion

            Console.WriteLine("");
        }

         /// <summary>
         /// 打印一个用户信息
         /// </summary>
         /// <param name="u"></param>
         public static void PrintUser(User u)
         {
             Console.WriteLine(u.Name+" "+u.Password);
         }

    public static string FuncBook()
        {
            return "送书来了";
        }

        public static int FuncBook2()
        {
            return 0;
        }

        public static string FuncBook3(string BookName)
        {
            return BookName;
        }

        private static void DisPlayValue(Func<string> func)
        {
            string RetFunc = func();
            Console.WriteLine("我在测试一下传过来值：{0}", RetFunc);
        }

    }
}
