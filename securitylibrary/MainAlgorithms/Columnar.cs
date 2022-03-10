using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            /*
             * Found a brutforce Solution but needs time
             * Waiting for test package to see if it will pass
             * Expected Solution :
             * Naive solution is to get column count by observing both texts
             * then find rows count
             * then itrating each letter after (n) rows in cipher and 
             * see where it stands in plain text then add it to list 
             */
            throw new NotImplementedException();
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
    }
}
