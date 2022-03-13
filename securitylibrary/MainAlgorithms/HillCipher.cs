using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    class Matrix
    {
        public int [,] mat ;
        public Matrix (int n , int m)
        {
            mat = new int[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    mat[i,j] = 0;
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
            throw new NotImplementedException();

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
