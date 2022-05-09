namespace SecurityLibrary {
    public class AutokeyVigenere : ICryptographicTechnique<string, string> {
        private static char[,] paTable;
        public AutokeyVigenere() {
            paTable = new char[26, 26];
            for (int i = 0; i < 26; i++) {
                char c = (char)('a' + i);
                for (int j = 0; j < 26; j++) {
                    paTable[i, j] = c++;
                    c = c > 'z' ? 'a' : c;
                }
            }
        }
        public string Analyse(string plainText, string cipherText) {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            string key = "";
            for (int i = 0; i < plainText.Length; i++) {
                int currentPlainRow = plainText[i] - 'a';
                key += findKeyChar(cipherText[i], currentPlainRow);
            }
            key = getBaseKey(plainText, key);
            return key;
        }

        public string Decrypt(string cipherText, string key) {
            cipherText = cipherText.ToLower();
            key = key.ToLower();

            string plain = "";
            int i = 0;
            while (plain.Length != cipherText.Length) {
                string partialplain = "";
                for (; i < key.Length; i++) {
                    int currentKeyCol = key[i] - 'a';
                    char plainChar = findPlainChar(cipherText[i], currentKeyCol);
                    partialplain += plainChar;
                }
                plain += partialplain;
                if (key.Length + partialplain.Length <= cipherText.Length)
                    key += partialplain;
                else {
                    int j = 0;
                    while (key.Length < cipherText.Length)
                        key += partialplain[j++];
                }
            }
            return plain;
        }

        public string Encrypt(string plainText, string key) {
            plainText = plainText.ToLower();
            key = key.ToLower();

            key = getFullKey(plainText, key);
            string cipher = "";
            for (int i = 0; i < plainText.Length; i++) {
                int a = plainText[i] - 'a',
                    b = key[i] - 'a';
                cipher += paTable[a, b];
            }
            return cipher;
        }

        private string getFullKey(string plainText, string key) {
            int idx = 0;
            int plainLength = plainText.Length;

            while (key.Length < plainLength) {
                key += plainText[idx++];
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
        private char findPlainChar(char cipherChar, int col) {
            int row = 0;
            for (int i = 0; i < 26; i++) {
                if (paTable[i, col] == cipherChar) {
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
        private char findKeyChar(char cipherChar, int row) {
            int col = 0;
            for (int i = 0; i < 26; i++) {
                if (paTable[row, i] == cipherChar) {
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
        private string getBaseKey(string plainText, string longKey) {
            string subStr = "";
            for (int i = 0; i < longKey.Length; i++) {
                subStr = "";
                if (longKey[i] == plainText[0]) {
                    int j = 1,
                        tmpi = i + 1;
                    subStr += plainText[0];

                    while (tmpi < longKey.Length) {
                        if (longKey[tmpi] == plainText[j]) {
                            subStr += plainText[j];
                            tmpi++;
                            j++;
                        }
                        else break;
                    }
                    if (tmpi == longKey.Length)
                        break;
                }
            }
            string key = longKey.Replace(subStr, "");
            return key;
        }


    }
}
