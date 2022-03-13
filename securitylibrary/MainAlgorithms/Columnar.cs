using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        static List<int[]> permutations = new List<int[]>();
        public List<int> Analyse(string plainText, string cipherText)
        {
            Create_Perm();
            // Brute-Force Solution
            List<int> Key = new List<int>() ;
            for (int i = 0; i < permutations.Count; i++)
            {

                Key = permutations[i].ToList();
                if (plainText.ToLower() == Decrypt(cipherText.ToLower(), Key))
                {
                    break;
                }

            }

            return Key;
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            /* Notes:
              * Not Handling Missing Chars {Showing X if in cipher}
              * Tested on Lab Examples Status = PASSED!
              */

            string plainText = "";
            // Get Number of Columns
            // Max Number in List (OR Size of list)
            int Columns = key.Count();

            // Get Number of Rows 
            // plainText size divided by Number of columns -Rounded up 
            int Rows = (int)Math.Ceiling(Convert.ToDouble(cipherText.Length) / Columns);

            // Create Array to store Cipher Text
            char[,] array = new char[Rows, Columns];
            // Pointer to the first char in cipher text
            int currentChar = 0;
            for (int i = 0; i < Columns; i++)
            {
                int idx = findIndexOfColumn(i + 1, key);
                for (int j = 0; j < Rows; j++)
                {
                    // Reading Cipher by columns in same order as in Key List
                    array[j, idx]= currentChar < cipherText.Length?cipherText[currentChar]: '\0';
                    currentChar++;

                }
            }
            // Reading in right Order
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    // Filling array row by row 
                    plainText +=array[i, j];

                }
            }
            Console.WriteLine(plainText);
            return plainText;
        }

        public string Encrypt(string plainText, List<int> key)
        {
            /* Notes:
              * Handling White Space by Removing it 
              * Handling Missing chars in array with char "X"
              * Converting All Letters to Capital
              * Tested on Lab Examples Status = PASSED!
              */

            // Removing White Spaces
            plainText = string.Join("", plainText.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            string cipherText = "";

            // Get Number of Columns
            // Max Number in List (OR Size of list)
            int Columns = key.Count();

            // Get Number of Rows 
            // plainText size divided by Number of columns -Rounded up 
            int Rows = (int)Math.Ceiling(Convert.ToDouble(plainText.Length) / Columns);

            // Create Array to store plain Text
            char[,] array = new char[Rows, Columns];
            // Pointer to the first char in plain text
            int currentChar = 0;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (currentChar != plainText.Length)
                    {
                        // Filling array row by row 
                        array[i, j] = Char.ToUpper(plainText[currentChar]);
                        currentChar++;
                    }
                    else
                    {
                        // Filling empty slots with space
                        array[i, j] = 'X';
                    }
                }
            }
            for (int i = 0; i < Columns; i++)
            {
                //get the index of the ith column in the key
                int idx = findIndexOfColumn(i+1, key);
                for (int j = 0; j < Rows; j++)
                {
                    // Reading Cipher by columns in same order as in Key List
                    cipherText += array[j, idx];
                    
                }
            }

            Console.WriteLine(cipherText);
            return cipherText;
        }

        private int findIndexOfColumn(int idx, List<int> key)
        {
            int column = 0;
            for(int i=0; i<key.Count; i++)
                if (key[i] == idx)
                    column = i;
            return column;
        }
    
        private char[,] create_arr(string plainText, int Rows, int Columns)
        {
            char[,] array = new char[Rows, Columns];
            int currentChar = 0;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (currentChar != plainText.Length)
                    {
                        // Filling array row by row 
                        array[i, j] = Char.ToUpper(plainText[currentChar]);
                        currentChar++;
                    }
                    else
                    {
                        // Filling empty slots with space
                        array[i, j] = 'X';
                    }
                }
            }
            return array;
        }

        ////////////////////////////// Permutaion Start////////////////////
        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
        private void Permut(int[] list, int k, int m, int sz)
        {

            int i;
            if (k == m)
            {
                int[] arr = new int[sz];
                for (i = 0; i <= m; i++)
                    arr[i] = list[i];
                permutations.Add(arr);
            }
            else
                for (i = k; i <= m; i++)
                {
                    Swap(ref list[k], ref list[i]);
                    Permut(list, k + 1, m, sz);
                    Swap(ref list[k], ref list[i]);
                }
        }
        private void Create_Perm()
        {

            for (int i = 2; i <= 7; i++)
            {
                int[] arr = new int[i];
                for (int j = 1; j <= i; j++)
                {
                    arr[j - 1] = j;
                }
                Permut(arr, 0, i - 1, i);

            }
        }
        //////////////////// End Of Permutation*//////////////////

    }
}
