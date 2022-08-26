using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //1800(0.5%)-G(6@200)
            //1800(Asv0.5%)-G(6@200)
            string r = ReplaceWords("1800(0.5%)-G(6@200)");
            string r1 = ReplaceWords("111");
            Console.WriteLine(r1);
            Console.ReadLine();
        }

        private static string ReplaceWords(string input)
        {
            if (input.Contains("%"))
            {
                string[] result = input.Split('-');
                string[] re = result[0].Split('(');
                string output = re[0] + "(" + "Asv" + re[1] + "-" + result[1];
                return output;
            }
            else
            {
                return "wrong";
            }
         
        }
        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="nums"></param>
        private static void Buble(int[] nums)
        {
            for (int i = 0; i < nums.Length - 1; i++)
            {
                for (int j = 0; j < nums.Length - 1 - i; j++)
                {
                    if (nums[j] > nums[j + 1])
                    {
                        swap(ref nums[j], ref nums[j + 1]);
                    }
                }
                Console.WriteLine("第{0}次排序结果：", i);
                WriteNum(nums);
            }
            Console.WriteLine("最终排序结果如下：");
            WriteNum(nums);
        }
        /// <summary>
        /// 交换两个数字的数值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        static void swap(ref int x, ref int y)
        {
            int temp = 0;
            temp = y;
            y = x;
            x = temp;
        }
        /// <summary>
        /// 输出数组
        /// </summary>
        /// <param name="nums"></param>
        static void WriteNum(int[] nums)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                Console.WriteLine(nums[i]);
            }
        }
        /// <summary>
        /// 求数组最小值
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        static int GetMin(int[] nums)
        {
            int min = nums[0];
            for (int i = 0; i < nums.Length; i++)
            {
                if (min > nums[i])
                {
                    min = nums[i];
                }
            }

            return min;
        }
    }
}
