using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        private static char[,] paTable;
        public RepeatingkeyVigenere()
        {
            paTable = new char[26, 26];
            for(int i=0; i<26; i++)
            {
                char c = (char)('a' + i);
                for(int j=0; j<26; j++)
                {
                    paTable[i, j] = c++;
                    c = c > 'z' ? 'a' : c;
                }
            }
        }

        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            string key = "";
            for(int i=0; i<plainText.Length; i++)
            {
                int currentPlainRow = plainText[i] - 'a';
                key += findKeyChar(cipherText[i], currentPlainRow);
            }
            key = getBaseKey(key);
            Console.WriteLine(key);
            return key;
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            key = key.ToLower();

            key = getFullKey(cipherText.Length, key);
            string plain = "";
            for(int i=0; i<cipherText.Length; i++)
            {
                int currentKeyCol = key[i] - 'a';
                char plainChar = findPlainChar(cipherText[i], currentKeyCol);
                plain += plainChar;
            }
            return plain;
        }

        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToLower();
            key = key.ToLower();

            key = getFullKey(plainText.Length, key);
            string cipher = "";
            for(int i=0; i<plainText.Length; i++)
            {
                int a = plainText[i] - 'a',
                    b = key[i] - 'a';
                cipher += paTable[a, b];
            }
            return cipher;
        }

        private string getFullKey(int textLen, string key)
        {
            int idx = 0;
            int baseLength = key.Length;
            while(key.Length < textLen)
            {
                key += key[idx];
                idx = (idx + 1) % baseLength;
            }
            return key;
        }
        /// <summary>
        /// Find plain character in paTable given the ciphered character and the index of the current
        /// key character(column)
        /// </summary>
        /// <param name="cipherChar">Ciphered Character</param>
        /// <param name="col">Column to search in</param>
        /// <returns>the plain character</returns>
        private char findPlainChar(char cipherChar, int col)
        {
            int row = 0;
            for(int i=0; i<26; i++)
            {
                if(paTable[i, col] == cipherChar)
                {
                    row = i;
                    break;
                }    
            }
            return (char)(row + 'a');
        }

        /// <summary>
        /// Find current key character in paTable given the ciphered character and the index of the current
        /// plain character(row)
        /// </summary>
        /// <param name="cipherChar">Ciphered Character</param>
        /// <param name="row">Row to search in</param>
        /// <returns>the current character of the key</returns>
        private char findKeyChar(char cipherChar, int row)
        {
            int col = 0;
            for (int i = 0; i < 26; i++)
            {
                if (paTable[row, i] == cipherChar)
                {
                    col = i;
                    break;
                }
            }
            return (char)(col + 'a');
        }
        /// <summary>
        /// Get Base Key of the repeated key
        /// </summary>
        /// <param name="longKey">The repeated key</param>
        /// <returns>Original key</returns>
        private string getBaseKey(string longKey)
        {
            string key = longKey[0] + "";
            for(int i=1; i<longKey.Length; i++)
            {
                if(longKey[i] == key[0])
                {
                    int j = 1,
                        tmpi = i + 1;
                    while(j<key.Length)
                    {
                        if (key[j] == longKey[tmpi])
                        {
                            tmpi++;
                            j++;
                        }
                        else break;
                    }
                    if (j == key.Length)
                        break;
                }
                key += longKey[i];
            }
            return key;
        }
    }
}