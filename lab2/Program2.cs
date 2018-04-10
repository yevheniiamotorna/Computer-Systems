using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
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

        /// <summary>
        /// Adder. Add two binary values. Doesn't return carryOut
        /// </summary>
        /// <param name="A">first binary value</param>
        /// <param name="B">second binary value</param>
        /// <returns></returns>
        static string NSum(string A, string B,out string carryOut)
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
            carryOut = carryIn.ToString();
            return sum;
        }

        static string AddNZerosAtBegin(string bin, int AmountOfZeros)
        {
            string newBin = bin;
            for (int i = 0; i < AmountOfZeros; i++)
                newBin = "0" + newBin;
            return newBin;
        }

        static string GetComplementaryCode(string bin)
        {
            string compl = "", carryOut;
            
            for (int i = 0; i < bin.Length; i++)
                compl += Convert.ToInt32(!Convert.ToBoolean(Convert.ToInt32(bin.Substring(i, 1)))).ToString();
            compl = NSum(compl, "1",out carryOut);
            return compl;
        }
        //multiplies two binary values with But's algorithm

        static string Sub(string binA, string binB)
        {
            string carryOut;
            string newB = AddNZerosAtBegin(binB, binA.Length - binB.Length);
            string newA = AddNZerosAtBegin(binA, binB.Length - binA.Length);
            string subB = GetComplementaryCode(newB);
            return NSum(newA, subB, out carryOut);
        }
        /// <summary>
        /// Compare two binary values
        /// </summary>
        /// <param name="binA"></param>
        /// <param name="binB"></param>
        /// <returns>1 - first value is bigger, 0 - values are equal, -1 - second value is bigger</returns>
        static int BinaryCompare(string binA, string binB)
        {
            binA = CutZerosInFront(binA);
            binB = CutZerosInFront(binB);
            int i = 0;
            if (binA.Length > binB.Length)
                return 1;
            if (binB.Length > binA.Length)
                return -1;
            for (; i < binA.Length; i++)
                if (binA[i] != binB[i])
                {
                    if (Convert.ToInt32(binB[i]) > Convert.ToInt32(binA[i]))
                        return -1;
                    return 1;
                }
            return 0;
        }
        static string CutZerosInFront(string bin)
        {
            while (bin[0] == '0' && bin.Length > 1)
                bin = bin.Substring(1);
            return bin;
        }

        static string UnsignedDivide(string Divident, string Divisor, out string Remainder)
        {
            string newDivident = CutZerosInFront(Divident), newDivisor = CutZerosInFront(Divisor), quotient = "", temp = "";
            int i = 0;
            if (BinaryCompare(newDivident, newDivisor) == -1)
            {
                Remainder = Divident;
                return "0";
            }
            for (; i < newDivident.Length; i++)
            {
                temp += newDivident[i];
                if (BinaryCompare(temp, newDivisor) == -1)
                    quotient += "0";
                else
                {
                    if (i == newDivident.Length - 1 && BinaryCompare(temp, newDivisor) == -1)
                    {
                        Remainder = temp;
                        return CutZerosInFront(quotient);
                    }
                    Console.WriteLine("_" + temp);
                    Console.WriteLine(" " + GetComplementaryCode(newDivisor));
                    Console.WriteLine("___________________");
                    temp = Sub(temp, newDivisor);
                    temp = CutZerosInFront(temp);
                    quotient += "1";
                    Console.WriteLine(" " + temp + "  quotient: " + quotient + "\n------------------------");
                }
            }
            Remainder = temp;
            return CutZerosInFront(quotient);
        }

        static string Divide(string Divident, string Divisor, out string remainder)
        {
            string quotient = UnsignedDivide(Divident, Divisor, out remainder), carryOut, temp;
            if (Divident[0] == '1')
            {
                if (Divisor[0] == '1')
                {
                    quotient = UnsignedDivide(GetComplementaryCode(Divident), GetComplementaryCode(Divisor), out remainder);
                    remainder = "1" + GetComplementaryCode(remainder);
                }
                else
                {
                    quotient = UnsignedDivide(GetComplementaryCode(Divident), Divisor, out remainder);
                    temp = NSum(quotient, "01", out carryOut);
                    temp = carryOut + temp;
                    quotient = "1" + GetComplementaryCode(temp);
                    remainder = Sub(Divisor, remainder);
                }
            }
            else
            {
                if (Divisor[0] == '1')
                {
                    quotient = UnsignedDivide(Divident, GetComplementaryCode(Divisor), out remainder);
                    temp = NSum(quotient, "01", out carryOut);
                    temp = carryOut + temp;
                    quotient = "1" + GetComplementaryCode(temp);
                    remainder = "1" + GetComplementaryCode(Sub(GetComplementaryCode(Divisor), remainder));
                }
            }
            return quotient;
        }
        static void Main(string[] args)
        {
            string rem, A, B;
            while (true)
            {
                A = Console.ReadLine();
                B = Console.ReadLine();
                Console.WriteLine("{0} {1}", CutZerosInFront(Divide(A, B, out rem)), CutZerosInFront(rem));
                Console.WriteLine("==========================");
            }
        }
    }
}
