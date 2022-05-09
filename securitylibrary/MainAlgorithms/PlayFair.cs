namespace SecurityLibrary {
    public class PlayFair : ICryptographic_Technique<string, string> {
        string alphabet = "abcdefghijklmnopqrstuvwxyz";

        public string Decrypt(string cipherText, string key) {
            cipherText = cipherText.ToLower();
            char[,] table = keyTable(key);
            string plain = createStringFromTable(table, cipherText, 4); // offset = 4 to get the previous item in row/col

            string plainText = "" + plain[0];
            for (int i = 1; i < plain.Length; i++) // removing extra x's from the plain text
            {
                if (plain[i] == 'x' && (i == plain.Length - 1 || plain[i - 1] == plain[i + 1] && i % 2 != 0)) {
                    continue;
                }
                plainText += plain[i];
            }
            return plainText;
        }

        public string Encrypt(string plainText, string key) {
            char[,] table = keyTable(key);

            plainText = plainText.ToLower();
            string plain = "" + plainText[0];
            int cumIdx = 0; //Cumulative index in order to maintain type of indices after adding x in string 
            for (int i = 1; i < plainText.Length; i++) {
                if (plainText[i - 1] == plainText[i] && (i + cumIdx) % 2 != 0) {
                    plain += 'x';
                    cumIdx++;
                }
                plain += plainText[i];
            }
            if (plain.Length % 2 != 0)
                plain += 'x';

            string cipher = createStringFromTable(table, plain, 1); // offset = 1 to get the next item in row/col
            return cipher;
        }

        private char[,] keyTable(string key) {
            char[,] table = new char[5, 5];
            bool[] isTaken = new bool[26];

            int i = 0, j = 0;
            foreach (char c in key) {
                char letter = checkForIJ(c);
                if (isTaken[letter - 'a'] != true) {
                    table[i, j] = letter;
                    isTaken[letter - 'a'] = true;
                    j = (j + 1) % 5;
                    if (j == 0)
                        i++;
                }
            }
            foreach (char c in alphabet) {
                char letter = checkForIJ(c);
                if (isTaken[letter - 'a'] != true) {
                    table[i, j] = letter;
                    isTaken[letter - 'a'] = true;
                    j = (j + 1) % 5;
                    if (j == 0)
                        i++;
                }
            }
            return table;
        }
        /// <summary>
        /// Unify i and j in order to be in one cell
        /// </summary>
        /// <param name="c"></param>
        /// <returns>return i or j if c equals any else return c</returns>
        private char checkForIJ(char c) {
            if (c == 'i' || c == 'j')
                return 'i';
            return c;
        }
        /// <summary>
        /// Search for character in table and returns its row and col
        /// </summary>
        /// <param name="c"></param>
        /// <param name="table"></param>
        /// <param name="dim"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void getCharIdx(char c, char[,] table, int dim, ref int row, ref int col) {
            bool isFound = false;
            for (int i = 0; i < dim; i++) {
                for (int j = 0; j < dim; j++) {
                    if (table[i, j] == c) {
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
        /// <summary>
        /// Create string from playfair table given the text you want to encrypt/decrypt
        /// </summary>
        /// <param name="table"></param>
        /// <param name="text"></param>
        /// <param name="offset"></param>
        /// <returns>encrypted/decrypted text using playfair table</returns>
        private string createStringFromTable(char[,] table, string text, int offset) {
            string output = "";
            for (int i = 0; i < text.Length; i += 2) {
                int row1 = 0, col1 = 0, row2 = 0, col2 = 0;
                char letter1 = checkForIJ(text[i]),
                     letter2 = checkForIJ(text[i + 1]);
                getCharIdx(letter1, table, 5, ref row1, ref col1);
                getCharIdx(letter2, table, 5, ref row2, ref col2);

                if (row1 == row2) {
                    int newCol = (col1 + offset) % 5;
                    output += table[row1, newCol];

                    newCol = (col2 + offset) % 5;
                    output += table[row1, newCol];
                }

                else if (col1 == col2) {
                    int newRow = (row1 + offset) % 5;
                    output += table[newRow, col1];

                    newRow = (row2 + offset) % 5;
                    output += table[newRow, col1];
                }

                else {
                    output += table[row1, col2];
                    output += table[row2, col1];
                }
            }
            return output;
        }
    }
}
