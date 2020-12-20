using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPOLab2
{
    static class Language
    {
        private static String[] IdentifierAlphabet = {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                                      "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
        private static String[] NumbersAlphabet = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F"};
        private static String[] OperationsAlphabet = {":=", "(", ")", "or", "xor", "and", "not"};
        private static String[] Keywords = { "begin", "bool", "if", "end"};
        private static String[] Terminals = {";"};
        public static String Comments = "//";

        public static bool isItLegalIdentifier(String a)
        {
            bool check = true;
            for (int i = 0; i < a.Length; i++)
            {
                if (!isItIdentifierSymbol(Convert.ToString(a[i]))){
                    if (!isItNumbersSymbol(Convert.ToString(a[i])) || (i == 0))
                    {
                        check = false;
                    }       
                }
            }
            return check;
        }

        public static bool isItLegalNumber(String a)
        {
            bool check = true;
            for (int i = 0; i < a.Length; i++)
            {
                if (!isItNumbersSymbol(Convert.ToString(a[i])))
                {
                    check = false;
                }
            }
            return check;
        }

        public static bool isItIdentifierSymbol(String a)
        {
            return isItInAlphabet(a, ref IdentifierAlphabet);
        }

        public static bool isItOperationsSymbol(String a)
        {
            return isItInAlphabet(a, ref OperationsAlphabet);
        }
        public static bool isItOperationsWord(String a)
        {
            return isItInAlphabet(a, ref OperationsAlphabet);
        }

        public static bool isItNumbersSymbol(String a)
        {
            return isItInAlphabet(a, ref NumbersAlphabet);
        }

        public static bool isItKeyword(String a)
        {
            return isItInAlphabet(a, ref Keywords);
        }

        public static bool isItTerminal(String a)
        {
            return isItInAlphabet(a, ref Terminals);
        }

        public static bool isItType(String a)
        {
            return isItInAlphabet(a, ref Terminals);
        }

        private static bool isItInAlphabet(String a, ref String[] alphabet)
        {
            bool answer = false;
            for (int i = 0; i < alphabet.Length; i++)
            {
                if (a == alphabet[i])
                {
                    answer = true;
                    return answer;
                }
            }
            return answer;
        }

    }
}
