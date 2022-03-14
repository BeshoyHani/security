using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SecurityLibrary {


    class Matrix {
        private int[,] temp;
        private int ModInverse(int x, int mod) {
            for (int i = 1; i < mod; i++) {
                if (i * x % mod == 1) {
                    return i;
                }
            }
            return 0;
        }
        private void swap(ref int x, ref int y) {
            int temp = y;
            y = x;
            x = temp;
        }
        public void init(ref int[,] mat, int n, int m, bool identity = false) {
            mat = new int[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    mat[i, j] = Convert.ToInt32(((i == j) && identity));
        }

        public int[,] mul(int[,] a, int[,] b) {
            int n1 = a.GetLength(0), m1 = a.GetLength(1);
            int n2 = b.GetLength(0), m2 = b.GetLength(1);

            if (m1 != n2) throw new Exception();

            init(ref temp, n1, m2);

            for (int i = 0; i < n1; i++) {
                for (int j = 0; j < m2; j++) {
                    for (int k = 0; k < m1; k++) {
                        temp[i, j] = (temp[i, j] + a[i, k] * b[k, j]) % 26;
                    }
                }
            }

            return temp;
        }

        public int[,] inverse(int[,] mat) {
            int n = mat.GetLength(0);
            if (n != mat.GetLength(1)) {
                throw new Exception();
            }

            temp = new int[n, 2 * n];

            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++)
                    temp[i, j] = mat[i, j];

                for (int j = n; j < 2 * n; j++)
                    temp[i, j] = 0;

                temp[i, n + i] = 1;
            }

            //--------------------------------------------
            for (int row = 0; row < n; row++) {

                bool found = false;
                for (int j = row; j < n; j++) {
                    if (temp[j, row] != 0 && ModInverse(temp[j, row], 26) != 0) {
                        for (int k = 0; k < 2 * n; k++) {
                            swap(ref temp[row, k], ref temp[j, k]);
                        }
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    throw new Exception();
                }

                int inv_val = ModInverse(temp[row, row], 26);
                for (int j = 0; j < 2 * n; j++) {
                    temp[row, j] = (temp[row, j] * inv_val) % 26;
                }

                for (int j = 0; j < n; j++) {
                    if (j == row) continue;
                    int c = 26 - temp[j, row];

                    for (int k = 0; k < 2 * n; k++) {
                        temp[j, k] = (temp[j, k] + c * temp[row, k]) % 26;
                    }
                }
            }
            int[,] res = new int[n, n];

            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    res[i, j] = temp[i, j + n];
                }
            }
            return res;
        }
    }

    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>> {

        Matrix matrix = new Matrix();

        public List<int> Analyse(List<int> plainText, List<int> cipherText) {
            List<int> key = new List<int>() { 0, 0, 0, 0 };

            for (key[0] = 0; key[0] < 26; key[0]++) {
                for (key[1] = 0; key[1] < 26; key[1]++) {
                    for (key[2] = 0; key[2] < 26; key[2]++) {
                        for (key[3] = 0; key[3] < 26; key[3]++) {
                            List<int> CT = Encrypt(plainText, key);
                            bool flag = true;
                            for (int i = 0; i < cipherText.Count; i++) {
                                if (cipherText[i] != CT[i])
                                    flag = false;
                            }
                            if (flag)
                                return key;
                        }
                    }
                }
            }

            throw new InvalidAnlysisException();
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key) {
            int n = 1;
            while (n * n != key.Count) n++;

            int[,] K = new int[n, n];
            int[,] CT = new int[n, cipherText.Count / n];

            for (int i = 0; i < key.Count; i++) {
                K[i / n, i % n] = key[i];
            }

            for (int i = 0; i < cipherText.Count; i++) {
                CT[i % n, i / n] = cipherText[i];
            }

            int[,] KI = matrix.inverse(K);
            int[,] PT = matrix.mul(KI, CT);

            List<int> res = new List<int>();

            for (int i = 0; i < PT.GetLength(1); i++) {
                for (int j = 0; j < PT.GetLength(0); j++) {
                    res.Add(PT[j, i]);
                }
            }
            return res;
        }


        public List<int> Encrypt(List<int> plainText, List<int> key) {
            int n = 1;
            while (n * n != key.Count) n++;

            int[,] K = new int[n, n];
            int[,] PT = new int[n, plainText.Count / n];


            for (int i = 0; i < key.Count; i++) {
                K[i / n, i % n] = key[i];
            }

            for (int i = 0; i < plainText.Count; i++) {
                PT[i % n, i / n] = plainText[i];
            }

            int[,] CT = matrix.mul(K, PT);

            List<int> res = new List<int>();

            for (int i = 0; i < CT.GetLength(1); i++) {
                for (int j = 0; j < CT.GetLength(0); j++) {
                    res.Add(CT[j, i]);
                }
            }
            return res;
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText) {
            int[,] PT = new int[3, 3];
            int[,] CT = new int[3, 3];

            for (int i = 0; i < plainText.Count; i++) {
                PT[i % 3, i / 3] = plainText[i];
                CT[i % 3, i / 3] = cipherText[i];
            }

            int[,] PTI = matrix.inverse(PT);

            int[,] K = matrix.mul(CT, PTI);

            List<int> res = new List<int>();
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    res.Add(K[i, j]);
                }
            }

            return res;
        }


        List<int> stringToList(string s) {
            List<int> L = new List<int>();
            s = s.ToLower();

            for (int i = 0; i < s.Length; i++) {
                L.Add(s[i] - 'a');
            }
            return L;
        }
        string listToString(List<int> L) {
            string s = "";
            for (int i = 0; i < L.Count; i++) {
                s += (char)(L[i] + 'a');
            }
            return s;
        }
        public string Analyse(string plainText, string cipherText) {
            List<int> PT_list = stringToList(plainText);
            List<int> CT_list = stringToList(cipherText);
            return listToString(Analyse(PT_list, CT_list));
        }


        public string Decrypt(string cipherText, string key) {
            List<int> CT_list = stringToList(cipherText);
            List<int> K_list = stringToList(key);
            return listToString(Decrypt(CT_list, K_list));
        }


        public string Encrypt(string plainText, string key) {
            List<int> PT_list = stringToList(plainText);
            List<int> K_list = stringToList(key);
            return listToString(Encrypt(PT_list, K_list));
        }


        public string Analyse3By3Key(string plainText, string cipherText) {
            List<int> PT_list = stringToList(plainText);
            List<int> CT_list = stringToList(cipherText);
            return listToString(Analyse3By3Key(PT_list, CT_list));
        }

    }
}
