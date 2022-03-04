using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        public string Analyse(string plainText, string cipherText)
        {
            char[] key = new char[26];
            char[] alphabetArr = alphabet.ToCharArray();

            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            for(int i=0; i<plainText.Length; i++)
            {
                int idx = (plainText[i] - 'a');
                key[idx] = cipherText[i];
                int cipherIdx = cipherText[i] - 'a';
                alphabetArr[cipherIdx] = '0';
            }

            for(int i=0, j=0; i<26; i++)
            {
                if(key[i]<'a' || key[i] > 'z')
                {
                    while (alphabetArr[j] == '0')
                        j++;
                    key[i] = alphabetArr[j];
                    j++;
                }
            }
            return new String(key);
        }

        public string Decrypt(string cipherText, string key)
        {
            string plain = "";
            cipherText = cipherText.ToLower();
            foreach (char c in cipherText)
            {
                int idx = getCharIdx(c, key);
                plain += alphabet[idx];
            }
            return plain;
        }

        public string Encrypt(string plainText, string key)
        {
            string cipher = "";
            plainText = plainText.ToLower();
            foreach(char c in plainText)
            {
                int idx = c - 'a';
                cipher += key[idx];
            }
            return cipher;
        }

        private int getCharIdx(char c, string key)
        {
            for (int i = 0; i < key.Length; i++)
                if (key[i] == c)
                    return i;
            return -1;
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            float[] freq_info= {12.51f, 9.25f, 8.04f, 7.60f, 7.26f, 7.09f, 6.54f, 6.12f, 5.49f, 4.14f, 3.99f, 3.06f,
                                2.71f, 2.53f, 2.30f, 2.00f, 1.96f, 1.92f, 1.73f, 1.54f, 0.99f, 0.67f, 0.19f, 0.16f,
                                0.11f, 0.09f};
            string freq_ab = "etaoinsrhldcumfpgwybvkxjqz";

            char[] plain = new char[cipher.Length];
            float[] cipher_freq = new float[26];
            char[] key = new char[26];
            bool[] isTaken = new bool[26];
            cipher = cipher.ToLower();

            foreach(char c in alphabet)
            {
                cipher_freq[c - 'a'] = 0;
                foreach(char c2 in cipher)
                {
                    if (c2 == c)
                        cipher_freq[c - 'a']++;
                }

                cipher_freq[c - 'a'] /= cipher.Length;
                cipher_freq[c - 'a'] = (float)Math.Round(cipher_freq[c - 'a'] * 100.0f, 2);

            }
            for (int i=0; i<26; i++)
            Console.WriteLine(cipher_freq[i]);
            foreach(char c in alphabet)
            {
                int idx = c - 'a';
                if (cipher_freq[idx] == 0 || (key[idx] >='a' && key[idx]<='z') )
                    continue;
                float minFreq = cipher_freq[idx];
                int cipherIndex = 0;
                for (int i=0; i<26; i++)
                {
                    char newC = freq_ab[i];
                    float freq = Math.Abs(cipher_freq[idx] - freq_info[i]);
                    if(freq <= minFreq && isTaken[newC - 'a'] != true)
                    {
                        minFreq = freq;
                        cipherIndex = i;
                    }
                }

                isTaken[freq_ab[cipherIndex] - 'a'] = true;
                key[idx] = freq_ab[cipherIndex];
            }
            for(int i=0; i<cipher.Length; i++)
            {
                plain[i] = key[cipher[i] - 'a'];
            }
            Console.WriteLine(plain);
            return new String(plain);
        }
    }
}
