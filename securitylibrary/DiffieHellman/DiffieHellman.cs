using System;
using System.Collections.Generic;
using SecurityLibrary.RSA;

namespace SecurityLibrary.DiffieHellman {
    public class DiffieHellman {
        myMath math = new myMath();

        public List<int> GetKeys(int q, int alpha, int xa, int xb) {
            int Ya = Convert.ToInt32(math.FastPower(alpha, xa, q));
            int Yb = Convert.ToInt32(math.FastPower(alpha, xb, q));

            int Ka = Convert.ToInt32(math.FastPower(Yb, xa, q));
            int Kb = Convert.ToInt32(math.FastPower(Ya, xb, q));

            return new List<int> { Ka, Kb };
        }
    }
}
