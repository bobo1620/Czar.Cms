using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp.yufatang
{
    /// <summary>
    /// 静态类
    /// </summary>
    public static class StringExt
    {
        /// <summary>
        /// this 表示针对this后面的类型进行扩展
        /// 第一次参数不能是静态类类型 只能是接口类型
        /// </summary>
        /// <param name="convert"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        static private Regex regexNumber = new Regex("\\d+");
        static public bool IsNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return regexNumber.IsMatch(input);
        }



      

    }
}
