using System;
using System.Collections.Generic;
using SecurityLibrary.RSA;
namespace SecurityLibrary.ElGamal {
    public class ElGamal {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        /// 
        myMath math = new myMath();

        public List<long> Encrypt(int q, int alpha, int y, int k, int m) {
            long c1 = math.FastPower(alpha, k, q);
            long K = math.FastPower(y,k,q);
            long c2 = K * m % q;

            return new List<long>() { c1, c2 };
        }
        public int Decrypt(int c1, int c2, int x, int q) {
            long K=math.FastPower(c1,x,q);
            long M = c2 * math.ModInverse(K, q) % q;
            return Convert.ToInt32(M);
        }
    }
}
