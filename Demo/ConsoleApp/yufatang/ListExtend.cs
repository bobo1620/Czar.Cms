using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.yufatang
{
      /// <summary>
     /// List扩展方法
     /// </summary>
    public static class ListExtend
    {
        //声明自定义泛型委托
        public delegate void PrintT<T>(T t);

        public static void TEach<T>(this List<T> list, PrintT<T> pt)
        {
            foreach (T t in list)
            {
                pt(t);
            }
        }
    }
}
