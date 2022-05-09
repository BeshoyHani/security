using System;

namespace SecurityLibrary.RSA {
    class myMath {

        public long FastPower(long b, long p, long mod) {
            if (p == 0) return 1;

            long ans = FastPower(b * b % mod, p / 2, mod);

            return p % 2 == 1 ? ans * b % mod : ans;
        }
        void replace(ref long x , ref long y , long newX , long newY , long mod = long.MaxValue) {
            x = newX % mod;
            y = newY % mod;
        }

        public long ModInverse(long val , long mod) {
            // x * val + y * mod = 1
            long a = val, b = mod;
            long x = 1, y = 0;
            long x1 = 0, y1 = 1;

            while (b != 0) {
                long q = a / b;
                replace(ref x,ref x1, x1, x - q * x1 , mod);
                replace(ref y, ref y1, y1, y - q * y1 ,mod);
                replace(ref a, ref b, b, a - q * b);
            }
            return (x%mod + mod)%mod;
        }

    }

    public class RSA {
        myMath math = new myMath();

        public int Encrypt(int p, int q, int M, int e) {
            int phi = (p - 1) * (q - 1);
            int n = p * q;
            return Convert.ToInt32(math.FastPower(M, e, n));
        }

        public int Decrypt(int p, int q, int C, int e) {
            int phi = (p - 1) * (q - 1);
            int n = p * q;

            int d = Convert.ToInt32(math.ModInverse(e, phi));
            return Convert.ToInt32(math.FastPower(C, d, n));
        }
    }
}