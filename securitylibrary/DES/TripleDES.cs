using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class TripleDES : ICryptographicTechnique<string, List<string>>
    {
        DES des;
        public TripleDES()
        {
            des = new DES();
        }
        public string Decrypt(string cipherText, List<string> key)
        {
            cipherText = des.Decrypt(cipherText, key[0]);
            cipherText = des.Encrypt(cipherText, key[1]);
            string plain = des.Decrypt(cipherText, key[0]);
            return plain;
        }

        public string Encrypt(string plainText, List<string> key)
        {
            plainText = des.Encrypt(plainText, key[0]);
            plainText = des.Decrypt(plainText, key[1]);
            string cipher = des.Encrypt(plainText, key[0]);
            Console.WriteLine(cipher);
            return cipher;
        }

        public List<string> Analyse(string plainText,string cipherText)
        {
            throw new NotSupportedException();
        }

    }
}
