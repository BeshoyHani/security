using System;
using System.Collections.Generic;

namespace SecurityLibrary.DES {
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class DES : CryptographicTechnique {
        private Dictionary<char, string> map = new Dictionary<char, string>();
        private int[] key_ip1, key_ip2, text_ip, expansion_table, sbox_permutation, inverse_permutation;
        private int[,,] sbox;
        int[] key_shift_table;

        public DES() {
            // init Hexa to Binary Dictionary
            map.Add('0', "0000");
            map.Add('1', "0001");
            map.Add('2', "0010");
            map.Add('3', "0011");
            map.Add('4', "0100");
            map.Add('5', "0101");
            map.Add('6', "0110");
            map.Add('7', "0111");
            map.Add('8', "1000");
            map.Add('9', "1001");
            map.Add('A', "1010");
            map.Add('B', "1011");
            map.Add('C', "1100");
            map.Add('D', "1101");
            map.Add('E', "1110");
            map.Add('F', "1111");

            key_ip1 = new int[]{
                57,49,41,33,25,17,9,
                1,58,50,42,34,26,18,
                10,2,59,51,43,35,27,
                19,11,3,60,52,44,36,
                63,55,47,39,31,23,15,
                7,62,54,46,38,30,22,
                14,6,61,53,45,37,29,
                21,13,5,28,20,12,4};

            key_ip2 = new int[]{
                14,17,11,24,1,5,
                3,28,15,6,21,10,
                23,19,12,4,26,8,
                16,7,27,20,13,2,
                41,52,31,37,47,55,
                30,40,51,45,33,48,
                44,49,39,56,34,53,
                46,42,50,36,29,32};

            key_shift_table = new int[] {
                1, 1, 2, 2,
                2, 2, 2, 2,
                1, 2, 2, 2,
                2, 2, 2, 1};

            text_ip = new int[]{
                58,50,42,34,26,18,10,2,
                60,52,44,36,28,20,12,4,
                62,54,46,38,30,22,14,6,
                64,56,48,40,32,24,16,8,
                57,49,41,33,25,17,9,1,
                59,51,43,35,27,19,11,3,
                61,53,45,37,29,21,13,5,
                63,55,47,39,31,23,15,7
                };

            expansion_table = new int[]{
                32,1,2,3,4,5,4,5,
                6,7,8,9,8,9,10,11,
                12,13,12,13,14,15,16,17,
                16,17,18,19,20,21,20,21,
                22,23,24,25,24,25,26,27,
                28,29,28,29,30,31,32,1
                };

            sbox_permutation = new int[]{
                16,7,20,21,29,12,28,17,
                1,15,23,26,5,18,31,10,
                2,8,24,14,32,27,3,9,
                19,13,30,6,22,11,4,25
                };

            inverse_permutation = new int[]{
                40,8,48,16,56,24,64,32,
                39,7,47,15,55,23,63,31,
                38,6,46,14,54,22,62,30,
                37,5,45,13,53,21,61,29,
                36,4,44,12,52,20,60,28,
                35,3,43,11,51,19,59,27,
                34,2,42,10,50,18,58,26,
                33,1,41,9,49,17,57,25
                };

            sbox = new int[,,]
                {
                    {
                        { 14,4,13,1,2,15,11,8,3,10,6,12,5,9,0,7 },
                        { 0,15,7,4,14,2,13,1,10,6,12,11,9,5,3,8 },
                        { 4,1,14,8,13,6,2,11,15,12,9,7,3,10,5,0 },
                        { 15,12,8,2,4,9,1,7,5,11,3,14,10,0,6,13 }
                    },
                    {
                        { 15,1,8,14,6,11,3,4,9,7,2,13,12,0,5,10 },
                        { 3,13,4,7,15,2,8,14,12,0,1,10,6,9,11,5 },
                        { 0,14,7,11,10,4,13,1,5,8,12,6,9,3,2,15 },
                        { 13,8,10,1,3,15,4,2,11,6,7,12,0,5,14,9}
                    },
                    {
                        { 10,0,9,14,6,3,15,5,1,13,12,7,11,4,2,8 },
                        { 13,7,0,9,3,4,6,10,2,8,5,14,12,11,15,1 },
                        { 13,6,4,9,8,15,3,0,11,1,2,12,5,10,14,7 },
                        { 1,10,13,0,6,9,8,7,4,15,14,3,11,5,2,12 }
                    },
                    {
                        { 7,13,14,3,0,6,9,10,1,2,8,5,11,12,4,15 },
                        { 13,8,11,5,6,15,0,3,4,7,2,12,1,10,14,9 },
                        { 10,6,9,0,12,11,7,13,15,1,3,14,5,2,8,4 },
                        { 3,15,0,6,10,1,13,8,9,4,5,11,12,7,2,14 }
                    },
                    {
                        { 2,12,4,1,7,10,11,6,8,5,3,15,13,0,14,9 },
                        { 14,11,2,12,4,7,13,1,5,0,15,10,3,9,8,6 },
                        { 4,2,1,11,10,13,7,8,15,9,12,5,6,3,0,14 },
                        { 11,8,12,7,1,14,2,13,6,15,0,9,10,4,5,3 }
                    },
                    {
                        { 12,1,10,15,9,2,6,8,0,13,3,4,14,7,5,11 },
                        { 10,15,4,2,7,12,9,5,6,1,13,14,0,11,3,8 },
                        { 9,14,15,5,2,8,12,3,7,0,4,10,1,13,11,6 },
                        { 4,3,2,12,9,5,15,10,11,14,1,7,6,0,8,13 }
                    },
                    {
                        { 4,11,2,14,15,0,8,13,3,12,9,7,5,10,6,1},
                        { 13,0,11,7,4,9,1,10,14,3,5,12,2,15,8,6},
                        { 1,4,11,13,12,3,7,14,10,15,6,8,0,5,9,2},
                        { 6,11,13,8,1,4,10,7,9,5,0,15,14,2,3,12}
                    },
                    {
                        { 13,2,8,4,6,15,11,1,10,9,3,14,5,0,12,7 },
                        { 1,15,13,8,10,3,7,4,12,5,6,11,0,14,9,2 },
                        { 7,11,4,1,9,12,14,2,0,6,10,13,15,3,5,8 },
                        { 2,1,14,7,4,10,8,13,15,12,9,0,3,5,6,11 }
                    }
            };
        }

        public override string Decrypt(string cipherText, string key) {
            key = key.Remove(0, 2);
            key = ConvertHexatoBinary(key);
            key = ApplyInitialPermutation(key, key_ip1);

            string left_key = key.Substring(0, 28);
            string right_key = key.Substring(28, 28);

            List<string> keys = PerformLeftShift(left_key, right_key);
            keys.Reverse();

            string cipher = cipherText.Remove(0, 2);
            cipher = ConvertHexatoBinary(cipher);
            cipher = ApplyInitialPermutation(cipher, text_ip);

            string left_plain = cipher.Substring(0, 32);
            string right_plain = cipher.Substring(32, 32);

            string x = GenerateCipherText(left_plain, right_plain, keys);
            Console.WriteLine(x);
            return x;
        }

        public override string Encrypt(string plainText, string key) {
            key = key.Remove(0, 2);
            key = ConvertHexatoBinary(key);
            key = ApplyInitialPermutation(key, key_ip1);

            string left_key = key.Substring(0, 28);
            string right_key = key.Substring(28, 28);

            List<string> keys = PerformLeftShift(left_key, right_key);

            string plain = plainText.Remove(0, 2);
            plain = ConvertHexatoBinary(plain);
            plain = ApplyInitialPermutation(plain, text_ip);

            string left_plain = plain.Substring(0, 32);
            string right_plain = plain.Substring(32, 32);

            return GenerateCipherText(left_plain, right_plain, keys);
        }

        private string GenerateCipherText(string left_plain, string right_plain, List<string> keys) {
            for (int i = 0; i < 16; i++) {
                string expanded = ApplyExpansionTable(right_plain);
                string newRight = XOR(expanded, keys[i]);

                newRight = ApplySBox(newRight);
                newRight = ApplyInitialPermutation(newRight, sbox_permutation);
                newRight = XOR(newRight, left_plain);

                left_plain = right_plain;
                right_plain = newRight;

            }

            string finalBinaryString = right_plain + left_plain;
            finalBinaryString = ApplyInitialPermutation(finalBinaryString, inverse_permutation);


            string hexaString = "";
            for (int i = 0; i < finalBinaryString.Length; i += 4)
                hexaString += Convert.ToInt64(finalBinaryString.Substring(i, 4), 2).ToString("X");


            return "0x" + hexaString;
        }

        public string ApplyInitialPermutation(string key, int[] ip_table) {
            string newKey = "";
            foreach (int pos in ip_table)
                newKey += key[pos - 1];

            return newKey;
        }

        private string ConvertHexatoBinary(string hexa) {
            string binary = "";
            foreach (char c in hexa)
                binary += map[c];

            return binary;
        }

        private List<string> PerformLeftShift(string left_key, string right_key) {
            List<string> keys = new List<string>();
            for (int i = 0; i < 16; i++) {
                string shifted = left_key.Substring(0, key_shift_table[i]);
                string remaining = left_key.Remove(0, key_shift_table[i]);
                left_key = remaining + shifted;

                shifted = right_key.Substring(0, key_shift_table[i]);
                remaining = right_key.Remove(0, key_shift_table[i]);
                right_key = remaining + shifted;

                string newKey = left_key + right_key;
                newKey = ApplyInitialPermutation(newKey, key_ip2);// Perform IP2

                keys.Add(newKey);
            }
            return keys;
        }

        private string ApplyExpansionTable(string shortStr) {
            return ApplyInitialPermutation(shortStr, expansion_table);//Will Expand using expansion table
        }

        private string ApplySBox(string plain) {
            string newPlain = "";
            for (int i = 0, iteration = 0; i < plain.Length; i += 6, iteration++) {
                string subKey = plain.Substring(i, 6);
                string row_digits = subKey[0] + "" + subKey[subKey.Length - 1];
                string col_digits = subKey.Substring(1, 4);

                int row = Convert.ToInt32(row_digits, 2);
                int col = Convert.ToInt32(col_digits, 2);

                int value = sbox[iteration, row, col];
                string binary = Convert.ToString(value, 2);

                while (binary.Length < 4)
                    binary = "0" + binary;


                newPlain += binary;
            }
            return newPlain;
        }

        private string XOR(string one, string two) {
            string result = "";
            for (int j = 0; j < one.Length; j++) {
                if (one[j] == two[j])
                    result += "0";
                else result += "1";
            }

            return result;
        }
    }
}
