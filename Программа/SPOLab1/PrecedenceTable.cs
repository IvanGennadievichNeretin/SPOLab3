using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPOLab3
{
    class PrecedenceTable
    {
        public static int STATE_PREVIOUS = 1;
        public static int STATE_NEXT = 2;
        public static int STATE_EQUAL = 3;
        public static string START_SYMBOL = "START";
        public static string END_SYMBOL = "END";

        private Dictionary<string, Dictionary<string, int>> table;
        public PrecedenceTable()
        {
            table = new Dictionary<string, Dictionary<string, int>>();

            //a
            Dictionary<string, int> str1 = new Dictionary<string, int>();
            str1.Add("or", STATE_NEXT);
            str1.Add("xor", STATE_NEXT);
            str1.Add("and", STATE_NEXT);
            str1.Add(")", STATE_NEXT);
            str1.Add(":=", STATE_EQUAL);
            str1.Add(";", STATE_NEXT);

            //or
            Dictionary<string, int> str2 = new Dictionary<string, int>();
            str2.Add("a", STATE_PREVIOUS);
            str2.Add("or", STATE_NEXT);
            str2.Add("xor", STATE_NEXT);
            str2.Add("and", STATE_PREVIOUS);
            str2.Add("not", STATE_PREVIOUS);
            str2.Add("(", STATE_PREVIOUS);
            str2.Add(")", STATE_NEXT);
            str2.Add(";", STATE_NEXT);

            //xor
            Dictionary<string, int> str3 = new Dictionary<string, int>();
            str3.Add("a", STATE_PREVIOUS);
            str3.Add("or", STATE_NEXT);
            str3.Add("xor", STATE_NEXT);
            str3.Add("and", STATE_PREVIOUS);
            str3.Add("not", STATE_PREVIOUS);
            str3.Add("(", STATE_PREVIOUS);
            str3.Add(")", STATE_NEXT);
            str3.Add(";", STATE_NEXT);

            //and
            Dictionary<string, int> str4 = new Dictionary<string, int>();
            str4.Add("a", STATE_PREVIOUS);
            str4.Add("or", STATE_NEXT);
            str4.Add("xor", STATE_NEXT);
            str4.Add("and", STATE_NEXT);
            str4.Add("not", STATE_PREVIOUS);
            str4.Add("(", STATE_PREVIOUS);
            str4.Add(")", STATE_NEXT);
            str4.Add(";", STATE_NEXT);

            //not
            Dictionary<string, int> str5 = new Dictionary<string, int>();
            str5.Add("(", STATE_EQUAL);

            //(
            Dictionary<string, int> str6 = new Dictionary<string, int>();
            str6.Add("a", STATE_PREVIOUS);
            str6.Add("or", STATE_PREVIOUS);
            str6.Add("xor", STATE_PREVIOUS);
            str6.Add("and", STATE_PREVIOUS);
            str6.Add("not", STATE_PREVIOUS);
            str6.Add("(", STATE_PREVIOUS);
            str6.Add(")", STATE_EQUAL);

            //)
            Dictionary<string, int> str7 = new Dictionary<string, int>();
            str7.Add("or", STATE_NEXT);
            str7.Add("xor", STATE_NEXT);
            str7.Add("and", STATE_NEXT);
            str7.Add(")", STATE_NEXT);
            str7.Add(";", STATE_NEXT);

            //:=
            Dictionary<string, int> str8 = new Dictionary<string, int>();
            str8.Add("a", STATE_PREVIOUS);
            str8.Add("or", STATE_PREVIOUS);
            str8.Add("xor", STATE_PREVIOUS);
            str8.Add("and", STATE_PREVIOUS);
            str8.Add("not", STATE_PREVIOUS);
            str8.Add("(", STATE_PREVIOUS);
            str8.Add(";", STATE_EQUAL);

            //;
            Dictionary<string, int> str9 = new Dictionary<string, int>();
            str9.Add(END_SYMBOL, STATE_NEXT);

            //START
            Dictionary<string, int> str10 = new Dictionary<string, int>();
            str10.Add("a", STATE_PREVIOUS);

            table.Add("a", str1);
            table.Add("or", str2);
            table.Add("xor", str3);
            table.Add("and", str4);
            table.Add("not", str5);
            table.Add("(", str6);
            table.Add(")", str7);
            table.Add(":=", str8);
            table.Add(";", str9);
            table.Add(START_SYMBOL, str10);
        }

        public Dictionary<string, Dictionary<string, int>> getTable()
        {
            return table;
        }
    }
}
