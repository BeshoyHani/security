using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        string alphabet = "abcdefghijklmnopqrstuvwxyz";

        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            throw new NotImplementedException();
        }

        private char[ , ] keyTable(string key)
        {
            char[ , ] table = new char[5, 5];
            bool[] isTaken = new bool[26];

            int i = 0, j = 0;
            foreach(char c in key)
            {
                if (isTaken[c - 'a'] != true)
                {
                    table[i, j] = c;
                    j = (j + 1) % 5;
                    if (j == 0)
                        i++;
                }
            }
            foreach(char c in alphabet)
            {
                if (isTaken[c - 'a'] != true)
                {
                    table[i, j] = c;
                    j = (j + 1) % 5;
                    if (j == 0)
                        i++;
                }
            }
            return table;
        }
    }
}
