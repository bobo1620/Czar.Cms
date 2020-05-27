using System;
using System.Runtime.InteropServices;

namespace 委托
{
    public delegate string DelSya(string name);
    class Program
    {

        static void Main(string[] args)
        {
            #region 委托调用
            /*
            //DelSya del = say;
            DelSya del = new DelSya(say); //等价 DelSya del = say;
            string rel =  del("aaa");

            Console.WriteLine("Hello World!"+ rel);
            Console.ReadKey();
            */
            #endregion

            #region 匿名函数 签名也和委托保持一致

            DelSya del = delegate (string name) { Console.WriteLine("aaa"); return ""; };

            DelSya del2 = (string name) => { Console.WriteLine("sss"); return ""; };

            Console.ReadKey();
            #endregion
        }

        public static string say(string name ) 
        {
            Console.WriteLine("方法执行"+name);
            return "hhaode";
        }

    }
}
