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
            cipherText = cipherText.ToLower();
            string cipher = "";
            char[,] table = keyTable(key);
            string plain = "";

            for (int i = 0; i < cipherText.Length; i += 2)
            {
                int row1 = 0, col1 = 0, row2 = 0, col2 = 0;
                char letter1 = checkForIJ(cipherText[i]),
                     letter2 = checkForIJ(cipherText[i + 1]);
                getCharIdx(letter1, table, 5, ref row1, ref col1);
                getCharIdx(letter2, table, 5, ref row2, ref col2);

                if (row1 == row2)
                {
                    int newCol = (col1 + 4) % 5;
                    plain += table[row1, newCol];

                    newCol = (col2 + 4) % 5;
                    plain += table[row1, newCol];
                }

                else if (col1 == col2)
                {
                    int newRow = (row1 + 4) % 5;
                    plain += table[newRow, col1];

                    newRow = (row2 + 4) % 5;
                    plain += table[newRow, col1];
                }

                else
                {
                    plain += table[row1, col2];
                    plain += table[row2, col1];
                }
            }

            string plainText = "" + plain[0];
            for (int i = 1; i < plain.Length; i++)
            {
                if (plain[i] == 'x' && (i == plain.Length - 1 || plain[i - 1] == plain[i + 1] && i % 2 != 0))
                {
                    continue;
                }
                plainText += plain[i];
            }
            Console.WriteLine(cipherText);
            Console.WriteLine(plainText);
            return plainText;
        }

        public string Encrypt(string plainText, string key)
        {
            char[,] table = keyTable(key);

            for(int i=0; i<5; i++)
            {
                for (int j = 0; j < 5; j++)
                    Console.Write(table[i, j]);
                Console.WriteLine();
            }

            plainText = plainText.ToLower();
            string plain = "" + plainText[0];
            string cipher = "";
            int cumIdx = 0;
            for(int i=1; i<plainText.Length; i++)
            {
                if (plainText[i - 1] == plainText[i] && (i + cumIdx) % 2 != 0)
                {
                    plain += 'x';
                    cumIdx++;
                }
                plain += plainText[i];
            }
            if (plain.Length % 2 != 0)
                plain += 'x';

            Console.WriteLine(plain);
            for(int i=0; i<plain.Length; i+=2)
            {
                int row1=0, col1=0, row2=0, col2=0;
                char letter1 = checkForIJ(plain[i]),
                     letter2 = checkForIJ(plain[i + 1]); 
                getCharIdx(letter1, table, 5, ref row1, ref col1);
                getCharIdx(letter2, table, 5, ref row2, ref col2);

                if(row1 == row2)
                {
                    int newCol = (col1 + 1) % 5;
                    cipher += table[row1, newCol];

                    newCol = (col2 + 1) % 5;
                    cipher += table[row1, newCol];
                }

                else if(col1 == col2)
                {
                    int newRow = (row1 + 1) % 5;
                    cipher += table[newRow, col1];

                    newRow = (row2 + 1) % 5;
                    cipher += table[newRow, col1];
                }

                else
                {
                    cipher += table[row1, col2];
                    cipher += table[row2, col1];
                }
            }
            Console.WriteLine(cipher);
            return cipher;
        }

        private char[ , ] keyTable(string key)
        {
            char[ , ] table = new char[5, 5];
            bool[] isTaken = new bool[26];

            int i = 0, j = 0;
            foreach(char c in key)
            {
                char  letter= checkForIJ(c);
                if (isTaken[letter - 'a'] != true)
                {
                    table[i, j] = letter;
                    isTaken[letter - 'a'] = true;    
                    j = (j + 1) % 5;
                    if (j == 0)
                        i++;
                }
            }
            foreach(char c in alphabet)
            {
                char letter = checkForIJ(c);
                if (isTaken[letter - 'a'] != true)
                {
                    table[i, j] = letter;
                    isTaken[letter - 'a'] = true;
                    j = (j + 1) % 5;
                    if (j == 0)
                        i++;
                }
            }
            return table;
        }

        private char checkForIJ(char c)
        {
            if (c == 'i' || c == 'j')
                return 'i';
            return c;
        }

        private void getCharIdx(char c, char[ , ] table, int dim, ref int row, ref int col)
        {
            bool isFound = false;
            for(int i=0; i<dim; i++)
            {
                for(int j=0; j<dim; j++)
                {
                    if(table[i , j] == c)
                    {
                        row = i;
                        col = j;
                        isFound = true;
                        break;
                    }
                }
                if (isFound == true)
                    break;
            }
        }
    }
}
