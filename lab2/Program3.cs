using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class FloatingPoint
    {
        const int Capacity = 32, MantissaLength = 23, BiasLength = Capacity - MantissaLength - 1;
        readonly int ExpBias = (int)(Math.Pow(2, BiasLength - 1)) - 1;
        public string Sign { set; get; }
        public string Bias { set; get; }
        public string Mantissa { set; get; }

        public FloatingPoint() { }

        public FloatingPoint(double value)
        {
            Sign = (value < 0) ? "1" : "0";
            ToFloatingPoint(DoubleToBinary(value));
        }

        public FloatingPoint(string sign, string bias, string mantissa)
        {
            this.Sign = sign;
            this.Bias = bias;
            this.Mantissa = mantissa;
        }


        string DoubleToBinary(double value)
        {
            double fract = Math.Abs(value) - (int)Math.Truncate(Math.Abs(value));
            string binary = IntToBinary(value) + ".";
            for (int i = 0; i < MantissaLength; i++)
            {
                binary += ((fract * 2 >= 1) ? "1" : "0");
                fract = fract * 2 - Math.Truncate(fract * 2);
            }
            return binary;
        }

        string IntToBinary(double value)
        {
            string binary = "";
            int integer = (int)Math.Truncate(Math.Abs(value));
            do
            {
                binary = ((integer % 2 == 1) ? "1" : "0") + binary;
                integer /= 2;
            } while (integer >= 1);
            return binary;
        }

        void ToFloatingPoint(string binary)
        {
            string[] temp = binary.Split('.');
            int exponent = 0;
            if (temp[0] != "0")
            {
                exponent = temp[0].Length - 1;
                Mantissa = (temp[0].Substring(1) + temp[1]).Substring(0, MantissaLength);
            }
            else
            {
                while (temp[1][exponent] == '0')
                    exponent++;
                Mantissa = temp[1].Substring(exponent);
                while (Mantissa.Length <= MantissaLength)
                    Mantissa += "0";
                exponent = -exponent;
            }
            Bias = IntToBinary(ExpBias + exponent);
            while (Bias.Length < BiasLength)
                Bias = "0" + Bias;
        }

        static string AddNZerosAtBegin(string bin, int AmountOfZeros)
        {
            string newBin = bin;
            for (int i = 0; i < AmountOfZeros; i++)
                newBin = "0" + newBin;
            return newBin;
        }

        static string NSum(string A, string B, out string carryOut)
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
        
        static int Sum(int A, int B, int CarryIn, out int carryOut)
        {
            bool a = Convert.ToBoolean(A), b = Convert.ToBoolean(B), carryIn = Convert.ToBoolean(CarryIn);
            carryOut = Convert.ToInt32(a && b || a && carryIn || b && carryIn);
            return Convert.ToInt32(!a && !b && carryIn || !a && b && !carryIn || a && !b && !carryIn || a && b && carryIn);
        }
        
        static string Sub(string binA, string binB)
        {
            string carryOut;
            string newB = AddNZerosAtBegin(binB, binA.Length - binB.Length);
            string newA = AddNZerosAtBegin(binA, binB.Length - binA.Length);
            string subB = GetComplementaryCode(newB);
            return NSum(newA, subB, out carryOut);
        }

        static int BinaryCompare(string binA, string binB)
        {
            int i = 0;
            for (; i < binA.Length; i++)
                if (binA[i] != binB[i])
                {
                    if (Convert.ToInt32(binB[i]) > Convert.ToInt32(binA[i]))
                        return -1;
                    return 1;
                }
            return 0;
        }

        static string GetComplementaryCode(string bin)
        {
            string compl = "", carryOut;

            for (int i = 0; i < bin.Length; i++)
                compl += Convert.ToInt32(!Convert.ToBoolean(Convert.ToInt32(bin.Substring(i, 1)))).ToString();
            compl = NSum(compl, "1", out carryOut);
            return compl;
        }

        public override string ToString()
        {
            string WritenValue = Sign + "   ";
            int i = 0;
            for (; i < (Bias.Length / 4) * 4; i += 4)
                WritenValue += Bias.Substring(i, 4) + " ";
            WritenValue += Bias.Substring(i) + "  ";
            for(i = 0; i < (Mantissa.Length / 4) * 4; i += 4)
                WritenValue += Mantissa.Substring(i, 4) + " ";
            WritenValue += Mantissa.Substring(i);
            return WritenValue;
        }

        string ShiftToRight(string binary, int step)
        {
            if (step != 0)
            {
                binary = "1" + binary;
                binary = binary.Substring(0, binary.Length - 1);
                for (int i = 0; i < step; i++)
                    binary = "0" + binary;
                return binary.Substring(0, binary.Length - step + 1);
            }
            return binary;
        }

        int BinaryToInt(string Binary)
        {
            int res = 0;
            for (int i = 0; i < Binary.Length; i++)
                res += (Binary[i] == '1') ? (int)Math.Pow(2, Binary.Length - i - 1) : 0;
            return res;
        }

        public FloatingPoint AddValues(FloatingPoint f)
        {
            FloatingPoint max = null, min = null, result = new FloatingPoint();
            string carry;
            int shift = 0, temp =0;
            if (BinaryCompare(this.Bias, f.Bias) == -1)
            {
                max = f;
                min = this;
            }
            else if (BinaryCompare(this.Bias, f.Bias) == 1)
            {
                max = this;
                min = f;
            }
            else if (BinaryCompare(this.Mantissa, f.Mantissa) == -1)
            {
                max = f;
                min = this;
            }
            else
            {
                max = this;
                min = f;
            }
            max.Mantissa = "1" + max.Mantissa;
            if (BinaryCompare(max.Bias, min.Bias) == 0)
                min.Mantissa = "1" + min.Mantissa;
            result.Sign = max.Sign;
            shift = BinaryToInt(Sub(max.Bias, min.Bias));
            min.Mantissa = ShiftToRight(min.Mantissa, shift);
            min.Bias = max.Bias;
            if (max.Sign == min.Sign)
            {
                result.Mantissa = NSum(max.Mantissa, min.Mantissa, out carry);
                if (carry == "0")
                    result.Mantissa = result.Mantissa.Substring(1, MantissaLength);
                else
                    result.Mantissa = result.Mantissa.Substring(0, MantissaLength);
                result.Bias = NSum(max.Bias, carry, out carry);
            }
            else
            {
                result.Mantissa = Sub(max.Mantissa, min.Mantissa);
                if (result.Mantissa[0] == '0')
                    while (result.Mantissa[temp] != '1')
                        temp++;
                result.Mantissa = result.Mantissa.Substring(temp + 1);
                while (result.Mantissa.Length < MantissaLength)
                    result.Mantissa += "0";
                result.Bias = Sub(max.Bias, IntToBinary(temp));
            }
            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            FloatingPoint a = new FloatingPoint(11186.25);
            FloatingPoint b = new FloatingPoint(-11185.10);
            FloatingPoint c = new FloatingPoint(1.15);

            Console.WriteLine(a.ToString());
            Console.WriteLine(b.ToString());
            Console.WriteLine();
            Console.WriteLine(a.AddValues(b).ToString());
            Console.WriteLine(c.ToString());
        }
    }
}
