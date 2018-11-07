using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\KNU\1semestr\Security\Lab5\1.txt", text, result;
            string[] A= Console.ReadLine().Split(' ');//first - mode, second - key, third - vector
            int mode = (A[0] == "d" ? 1 : 0);//mode = 1 - decrypt, 0 - encrypt
            byte[] key = Encoding.ASCII.GetBytes(A[1]);
            byte[] vector = Encoding.ASCII.GetBytes(A[2]);
            OpenSSL.Crypto.CipherContext ctx = new OpenSSL.Crypto.CipherContext(OpenSSL.Crypto.Cipher.AES_128_CTR);
            using (StreamReader sr = new StreamReader(path))
                text = sr.ReadToEnd();
            if (mode == 1)
            {
                byte[] msg = Convert.FromBase64String(text);
                byte[] decrypted = ctx.Decrypt(msg, key, vector);
                result = new string(System.Text.Encoding.ASCII.GetChars(decrypted));
            }
            else
            {
                byte[] msg = Encoding.ASCII.GetBytes(text);
                byte[] enc = ctx.Encrypt(msg, key, vector);
                result = Convert.ToBase64String(enc);
            }

            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
