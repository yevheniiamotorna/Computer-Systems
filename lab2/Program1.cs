using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        /// <summary>
        /// Return Sum of bits: A, B and CarryIn(full adder)
        /// </summary>
        /// <param name="A">value of first bit</param>
        /// <param name="B">value of second bit</param>
        /// <param name="CarryIn"></param>
        /// <param name="carryOut"></param>
        /// <returns></returns>
        static int Sum(int A, int B, int CarryIn, out int carryOut)
        {
            bool a = Convert.ToBoolean(A), b = Convert.ToBoolean(B), carryIn = Convert.ToBoolean(CarryIn);
            carryOut = Convert.ToInt32(a && b || a && carryIn || b && carryIn);
            return Convert.ToInt32(!a && !b && carryIn || !a && b && !carryIn || a && !b && !carryIn || a && b && carryIn);
        }

        static string AddNZerosAtBegin(string bin, int AmountOfZeros)
        {
            string newBin = bin;
            for (int i = 0; i < AmountOfZeros; i++)
                newBin = "0" + newBin;
            return newBin;
        }

        /// <summary>
        /// Adder. Add two binary values. Doesn't return carryOut
        /// </summary>
        /// <param name="A">first binary value</param>
        /// <param name="B">second binary value</param>
        /// <returns></returns>
        static string NSum(string A, string B)
        {
            int a = 0, b = 0, carryIn = 0;
            string sum = "", newA, newB;
            newB = AddNZerosAtBegin(B, A.Length - B.Length);
            newA = AddNZerosAtBegin(A, B.Length - A.Length);
            for (int i = newA.Length - 1; i >= 0; i--)
            {
                a = Convert.ToInt32(newA.Substring(i, 1));
                b = Convert.ToInt32(newB.Substring(i, 1));
                sum = Sum(a, b, carryIn, out carryIn) + sum;
            }
            return sum;
        }

        static string GetComplementaryCode(string bin)
        {
            string compl = "";
            for (int i = 0; i < bin.Length; i++)
                compl += Convert.ToInt32(!Convert.ToBoolean(Convert.ToInt32(bin.Substring(i, 1)))).ToString();
            compl = NSum(compl, "1");
            return compl;
        }
        //multiplies two binary values with But's algorithm
        static string ButsAlgorithm(string A, string B)
        {
            string res = "", sub = GetComplementaryCode(A), add = A, nop = AddNZerosAtBegin("", A.Length), temp = "";
            res = AddNZerosAtBegin(B, A.Length) + "0";
            for (int i = res.Length - 2; i >= res.Length - B.Length - 1; i--)
            {
                Console.WriteLine(res.Substring(res.Length - 2, 2));
                if (res.Substring(res.Length - 2, 2) == "10")
                {
                    Console.WriteLine("sub");
                    temp = sub;
                }
                else
                    if (res.Substring(res.Length - 2, 2) == "01")
                {
                    Console.WriteLine("add");
                    temp = add;
                }
                else//nop случай, когда "00" или "11"
                {
                    Console.WriteLine("nop");
                    temp = null;
                }
                if (temp != null)
                {
                    temp = NSum(res.Substring(0, A.Length), temp);
                    res = res.Replace(res.Substring(0, A.Length), temp);
                    res = temp[0] + res;
                }
                else
                    res = res[0] + res;//в случае nop просто  +1 бит впереди
                res = res.Substring(0, res.Length - 1);//"отрезаем" последний бит(типо побитовый сдвиг)
                Console.WriteLine(res+"\n");
            }
            return res = res.Substring(0, A.Length + B.Length);
        }
        static void Main(string[] args)
        {
            string A, B;
            try
            {
                A = Console.ReadLine();
                B = Console.ReadLine();
                Console.WriteLine("-----------------");
                Console.WriteLine("result: " + ButsAlgorithm(A, B));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
