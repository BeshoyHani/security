using System;
using System.Text;

namespace SecurityLibrary {
    public class RailFence : ICryptographicTechnique<string, int> {
        public int Analyse(string plainText, string cipherText) {
            int n = cipherText.Length;
            for (int key = 1; key <= n; key++) {
                string tmp = Encrypt(plainText, key);
                if (tmp.Equals(cipherText, StringComparison.InvariantCultureIgnoreCase)) {
                    return key;
                }
            }
            return 0;
        }

        public string Decrypt(string cipherText, int key) {
            int n = cipherText.Length;
            int mov = 0;
            string ans = "";
            for (int i = 0; i < n; i++) {
                ans += ' ';
            }
            int cnt = 1, x = 0;
            StringBuilder sb = new StringBuilder(ans);
            for (int i = 0; i < n; i++) {
                sb[x] = cipherText[i];
                x += key;
                if (x >= n) {
                    x = cnt++;
                }
            }
            ans = sb.ToString();

            return ans;

        }

        public string Encrypt(string plainText, int key) {
            int x = plainText.Length % key;

            int n = plainText.Length;
            string ans = "";
            for (int i = 0; i < key; i++) {
                for (int j = i; j < n; j += key) {
                    ans += plainText[j];
                }
            }
            return ans;
        }
    }
}
