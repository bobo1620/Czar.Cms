using Polly;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace PolicyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            #region 降级 不带参数  Fallback
            /*
            Policy policy = Policy.Handle<ArgumentException>()
                .Fallback(() =>{
                    Console.WriteLine("降级");
               });

            policy.Execute(() =>
            {
                Console.WriteLine("任务开始执行");
                throw new ArgumentException();
                Console.WriteLine("任务结束"); ;
            });
            */
            #endregion

            #region 降级带参数 Fallback
            /*
            Policy<string> policy = Policy<string>.Handle<ArgumentException>()
             .Fallback(() => {
                 Console.WriteLine("降级");
                 return "降级操作";
             });

            var result =  policy.Execute(() =>
            {
                Console.WriteLine("任务开始执行");
                throw new ArgumentException();
                Console.WriteLine("任务结束"); ;
                return "正常值"; 
            });
            Console.WriteLine(result);
            */
            #endregion

            #region 重试 retry
            /*
            //RetryForever 一直重试  Retry 只试一次 Retry(3)执行重试多少次
            try
            {
                Policy policy = Policy.Handle<Exception>()
                    //.Retry(3);
                    .WaitAndRetry(10, i=>TimeSpan.FromSeconds(i)); //等一等在重试
                policy.Execute(() =>
                {
                    Console.WriteLine("任务开始执行");
                    if (DateTime.Now.Second % 10 != 0)
                    {
                        throw new Exception();
                    }
                    Console.WriteLine("结束！");

                });
            }
            catch (Exception ex) { }
            */
            #endregion

            #region 短路保护  出现N次连续错误 则熔断 等待一段时间，这段时间内如果Execute 则直接抛错。等待时间过去之后再执行execute的时候又出现错误（仅一次）则继续熔断，否则恢复正常。
            /*
            Policy policy = Policy.Handle<Exception>()
                   .CircuitBreaker(3, TimeSpan.FromSeconds(5));//连续3次出现错误之后熔断5秒 （不会执行业务代码） 测试未成功

            while (true)
            {
                Console.WriteLine("开始Execute");
                try
                {
                    policy.Execute(() =>
                    {
                        Console.WriteLine("开始renw");
                        throw new Exception("错误");
                        Console.WriteLine("结束");

                    });
                }
                catch (Exception ex) {
                    Console.WriteLine("捕获异常"+ex);
                }

                Thread.Sleep(500);
            }
            */
            #endregion

            #region 策略封装  多个policy一起执行
            /*
            Policy retryPolicy = Policy.Handle<Exception>().Retry(3);
            Policy fallbackPoilicy = Policy.Handle<Exception>().Fallback(() => {
                Console.WriteLine("降级");
            });

            //Wrap 是有顺序的
            Policy policy3 = fallbackPoilicy.Wrap(retryPolicy);  //fallbackPoilicy 包含retryPolicy 则是retryPolicy先执行
            policy3.Execute(()=> {
                Console.WriteLine("任务开始执行");
                if (DateTime.Now.Second % 10 != 0)
                {
                    throw new Exception();
                }
                Console.WriteLine("结束！");

            });
            */
            #endregion

            #region 超时
            Policy timeoutPolicy = Policy.Timeout(2, TimeoutStrategy.Pessimistic);
            Policy fullbackPolicy = Policy.Handle<TimeoutRejectedException>().Fallback(() => {
                Console.WriteLine("降级");
            });

            Policy policy5 = fullbackPolicy.Wrap(timeoutPolicy);
            policy5.Execute(()=>{
                Console.WriteLine("开执行");
                Thread.Sleep(5000);
                Console.WriteLine("任务结束");
            });
            #endregion
           
            Console.ReadLine();    
        }
    }
}
