using System;

namespace SecurityLibrary {
    public class Ceaser : ICryptographicTechnique<string, int> {
        public string Encrypt(string plainText, int key) {
            string cipher = "";
            plainText = plainText.ToLower();
            foreach (char c in plainText) {
                int idx = ((c - 'a') + key) % 26;
                cipher += (char)('a' + idx);
            }
            return cipher;

        }

        public string Decrypt(string cipherText, int key) {
            string plain = "";
            cipherText = cipherText.ToLower();
            foreach (char c in cipherText) {
                int idx = (c - 'a') - (key % 26);
                if (idx < 0)
                    idx += 26;
                plain += (char)('a' + idx);

            }
            Console.WriteLine(plain);
            return plain;
        }

        public int Analyse(string plainText, string cipherText) {
            char plain = plainText.ToLower()[0];
            char cipher = cipherText.ToLower()[0];
            int key = Math.Abs(plain - cipher);
            if (plain > cipher)
                key = 26 - key;
            return key;
        }
    }
}
