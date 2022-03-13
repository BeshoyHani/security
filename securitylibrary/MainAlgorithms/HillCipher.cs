using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecurityLibrary
{
    

    class Matrix
    {
      
        public int [,] mat ;
        public Matrix (int n , int m , bool identity = false)
        {
            mat = new int[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    mat[i, j] = Convert.ToInt32(((i == j) && identity));
        }

        public static Matrix operator * (Matrix a , Matrix b)
        {
            int n = a.mat.GetLength(0) , m = b.mat.GetLength(1);

            Matrix res = new Matrix(n , m);

            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < m; j++)
                {
                    for(int k = 0; k < a.mat.GetLength(1); k++)
                    {
                        res.mat[i, j] = (res.mat[i, j] + a.mat[i, k] * b.mat[k, j]) % 26;
                    }
                }
            }

            return res;
        }

        private static int ModInverse(int x, int mod)
        {
            for (int i = 1; i < mod; i++)
            {
                if (i * x % mod == 1)
                {
                    return i;
                }
            }
            return 0;
        }

        public Matrix inverse()
        {
            int n = mat.GetLength(0);
            if (n != mat.GetLength(1))
            {
                throw new Exception();
            }

            int[,] temp = new int[n, 2 * n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    temp[i, j] = mat[i, j];

                for (int j = n; j < 2 * n; j++)
                    temp[i, j] = 0;

                temp[i, n + i] = 1;
            }

            //--------------------------------------------
            for(int row = 0; row < n; row++)
            {

                bool found = false;
                for (int j=row;j<n; j++)
                {
                    if (temp[j, row] != 0 && ModInverse(temp[j,row],26)!=0)
                    {
                        for(int k = 0; k < 2 * n; k++)
                        {
                            int temp_val = temp[row, k];
                            temp[row, k] = temp[j,k];
                            temp[j, k] = temp_val;
                        }
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new Exception();
                }

                int inv_val = ModInverse(temp[row, row] , 26);
                for(int j = 0; j < 2 * n; j++)
                {
                    temp[row, j] = (temp[row, j] * inv_val) % 26;
                }

                for(int j = 0; j < n; j++)
                {
                    if (j == row) continue;
                    int c = 26-temp[j, row];

                    for(int k = 0; k < 2 * n; k++)
                    {
                        temp[j, k] = (temp[j, k] + c * temp[row, k]) % 26;
                    }
                }
            }

            Matrix res = new Matrix(n, n);
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    res.mat[i, j] = temp[i, j + n];
                }
            }
            return res;
        }
    }

    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher :  ICryptographicTechnique<List<int>, List<int>>
    {
       
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();

        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            int n = 1;
            while (n * n != key.Count) n++;

            List<int> res = new List<int>();
            Matrix key_mat = new Matrix(n, n);
            Matrix CT_mat = new Matrix(n, cipherText.Count / n);

            for (int i = 0; i < key.Count; i++)
            {
                key_mat.mat[i / n, i % n] = key[i];
            }
            key_mat = key_mat.inverse();

            for (int i = 0; i < cipherText.Count; i++)
            {
                CT_mat.mat[i % n, i / n] = cipherText[i];
            }


            Matrix PT_mat = key_mat * CT_mat;
            for (int i = 0; i < PT_mat.mat.GetLength(1); i++)
            {
                for (int j = 0; j < PT_mat.mat.GetLength(0); j++)
                {
                    res.Add(PT_mat.mat[j, i]);
                }
            }

            return res;

        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            int n = 1;
            while (n * n != key.Count) n++;

            List<int> res = new List<int>();
            Matrix key_mat = new Matrix(n, n);
            Matrix PT_mat = new Matrix(n, plainText.Count / n);

            for (int i = 0; i < key.Count; i++)
            {
                key_mat.mat[i / n, i % n] = key[i];
            }

            for (int i = 0; i < plainText.Count; i++)
            {
                PT_mat.mat[i % n, i / n] = plainText[i];
            }

            Matrix CT = key_mat * PT_mat;
            for (int i = 0; i < CT.mat.GetLength(1); i++)
            {
                for (int j = 0; j < CT.mat.GetLength(0); j++)
                {
                    res.Add(CT.mat[j, i]);
                }
            }

            return res;
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }

    }
}
