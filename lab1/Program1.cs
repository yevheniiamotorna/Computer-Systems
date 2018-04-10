using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        const int bitsInBytes = 8, numberOfFiles = 3;
        static List<int> keys = new List<int>();
        static void FillDictionaryKeys(Dictionary<int, double> chars)
        {
            for (int i = 10; i <= 1111; i++)
                chars.Add(i, 0);
            if (keys.Count == 0)
                keys = chars.Keys.ToList();
        }
        static void CalculateCharactersAmount(Dictionary<int, double> chars, string path, ref double H, ref double amountOfInf)
        {
            int amountOfCharacters = 0;
            using (StreamReader read = new StreamReader(path))
            {
                int c;
                do
                {
                    c = Convert.ToInt32((char)(read.Read()));
                    if (chars.ContainsKey(c))
                    {
                        chars[c]++;
                        amountOfCharacters++;
                    }
                } while (!read.EndOfStream);
            }
            foreach (int k in keys)
                chars[k] /= amountOfCharacters;
            foreach (int k in keys)
            {
                if (chars[k] != 0)
                    H += chars[k] * Math.Log(1 / chars[k], 2);
            }
            amountOfInf = H * amountOfCharacters;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            int i = 0;
            string path = @"E:\!KNU\3kurs2semestr\CS\lab1\", temp = "";
            double[] H = new double[numberOfFiles], 
                H_b64txt = new double[numberOfFiles], 
                H_b64bz2 = new double[numberOfFiles],
                H_bz2 = new double[numberOfFiles],
                amOfInf = new double[numberOfFiles],
                amOfInf_b64txt = new double[numberOfFiles],
                amOfInf_b64bz2 = new double[numberOfFiles],
                amOfInf_bz2 = new double[numberOfFiles];
            Dictionary<int, double>[] chars = new Dictionary<int, double>[numberOfFiles],
                base64_txt = new Dictionary<int, double>[numberOfFiles],
                base64_bz2 = new Dictionary<int, double>[numberOfFiles],
                bz2 = new Dictionary<int, double>[numberOfFiles];
            try
            {
                for (; i < H.Length; i++)
                {
                    chars[i] = new Dictionary<int, double>();
                    base64_txt[i] = new Dictionary<int, double>();
                    base64_bz2[i] = new Dictionary<int, double>();
                    bz2[i] = new Dictionary<int, double>();
                    FillDictionaryKeys(chars[i]);
                    FillDictionaryKeys(base64_txt[i]);
                    FillDictionaryKeys(base64_bz2[i]);
                    FillDictionaryKeys(bz2[i]);
                    CalculateCharactersAmount(chars[i], path + (i + 1).ToString() + ".txt", ref H[i], ref amOfInf[i]);
                    CalculateCharactersAmount(base64_txt[i], path + (i + 1).ToString() + "Base64.txt", ref H_b64txt[i], ref amOfInf_b64txt[i]);
                    CalculateCharactersAmount(base64_bz2[i], path + (i + 1).ToString() + "Base64_bz2.txt", ref H_b64bz2[i], ref amOfInf_b64bz2[i]);
                    CalculateCharactersAmount(bz2[i], path + (i + 1).ToString() + ".bz2", ref H_bz2[i], ref amOfInf_bz2[i]);
                }
                foreach (int k in keys)
                    if (chars[0][k] != 0 || chars[1][k] != 0 || chars[2][k] != 0)
                        Console.WriteLine("{0} {1:F8} {2:F8} {3:f8}", (char)k, chars[0][k], chars[1][k], chars[2][k]);
                for (i = 0; i < numberOfFiles; i++)
                {
                    temp = path + (i + 1).ToString();
                    Console.WriteLine(i + 1 + ". H = {0:F4} Amount of inf.: {1:F4} B, file size: {2} B, zip: {3} B, rar: {4} B, gzip: {5} B, bzip2: {6} B, xz: {7} B", H[i], amOfInf[i] / bitsInBytes, new FileInfo(temp + ".txt").Length, new FileInfo(temp + ".zip").Length, new FileInfo(temp + ".rar").Length, new FileInfo(temp + ".gz").Length, new FileInfo(temp + ".bz2").Length, new FileInfo(temp + ".xz").Length);
                    Console.WriteLine(i + 1 + ". OLD(txt):  {0:F4} B,  Base64:  {1:F4} B", amOfInf[i] / bitsInBytes, amOfInf_b64txt[i] / bitsInBytes);
                    Console.WriteLine(i + 1 + ". OLD(bz2):  {0:F4} B,  Base64:  {1:F4} B", amOfInf_bz2[i] / bitsInBytes, amOfInf_b64bz2[i] / bitsInBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
