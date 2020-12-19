using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPOLab3
{
    class RuleTable
    {
        public static string S_NONTERMINAL_SYMBOL = "$S";
        public static string F_NONTERMINAL_SYMBOL = "$F";
        public static string T_NONTERMINAL_SYMBOL = "$T";
        public static string E_NONTERMINAL_SYMBOL = "$E";
        public static string SYMBOL_SEPARATOR = " ";
        Dictionary<string, string> ruleTable;
        List<string> NonTerminalSymbolsAllowed;

        public RuleTable()
        {
            ruleTable = new Dictionary<string, string>();
            //правило     |    символ-родитель
            //S
            ruleTable.Add("a" + SYMBOL_SEPARATOR + ":=" + SYMBOL_SEPARATOR + S_NONTERMINAL_SYMBOL + SYMBOL_SEPARATOR + ";", S_NONTERMINAL_SYMBOL + "1");
            ruleTable.Add(S_NONTERMINAL_SYMBOL + SYMBOL_SEPARATOR + "or" + SYMBOL_SEPARATOR + S_NONTERMINAL_SYMBOL, S_NONTERMINAL_SYMBOL + "2");
            ruleTable.Add(S_NONTERMINAL_SYMBOL + SYMBOL_SEPARATOR + "xor" + SYMBOL_SEPARATOR + S_NONTERMINAL_SYMBOL, S_NONTERMINAL_SYMBOL + "3");
            ruleTable.Add(S_NONTERMINAL_SYMBOL, S_NONTERMINAL_SYMBOL + "4");
            ruleTable.Add(S_NONTERMINAL_SYMBOL + SYMBOL_SEPARATOR + "and" + SYMBOL_SEPARATOR + S_NONTERMINAL_SYMBOL, S_NONTERMINAL_SYMBOL + "5");
            ruleTable.Add("(" + SYMBOL_SEPARATOR + S_NONTERMINAL_SYMBOL + SYMBOL_SEPARATOR + ")", S_NONTERMINAL_SYMBOL + "6");
            ruleTable.Add("not" + SYMBOL_SEPARATOR + "(" + SYMBOL_SEPARATOR + S_NONTERMINAL_SYMBOL + SYMBOL_SEPARATOR + ")", S_NONTERMINAL_SYMBOL + "7");
            ruleTable.Add("a", S_NONTERMINAL_SYMBOL + "8");

            NonTerminalSymbolsAllowed = new List<string>();
            NonTerminalSymbolsAllowed.Add(S_NONTERMINAL_SYMBOL);
            NonTerminalSymbolsAllowed.Add(SYMBOL_SEPARATOR);
        }

        public string convolution_getRuleSymbol(string ruleResultGotten, out string numberOfRule)
        {
            string answer;
            if (ruleTable.TryGetValue(ruleResultGotten, out answer))
            {
                answer = extractNumber(answer, out numberOfRule);
                return answer;
            }
            else
            {
                numberOfRule = "error";
                return null;
            }
        }

        public string convolution_getRuleSymbol(List<string> ruleResultGotten, out string numberOfRule)
        {
            return convolution_getRuleSymbol(prepareList(ruleResultGotten), out numberOfRule);
        }

        
        private string extractNumber(string rule, out string number)
        {
            number = "";
            for (int i = rule.Length - 1; i >= 0; i--)
            {
                if (itIsNumber(rule[i]))
                {
                    number += rule[i];
                    rule = rule.Remove(i, 1);
                }
            }
            number = revertString(number);
            return rule;
        }

        private bool itIsNumber(char number)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Convert.ToString(number) == Convert.ToString(i))
                {
                    return true;
                }
            }
            return false;
        }

        private string revertString(string str)
        {
            char bufer;
            char[] charArray = str.ToCharArray();
            for (int i = 0; i < charArray.Length / 2; i++)
            {
                bufer = charArray[i];
                charArray[i] = charArray[charArray.Length - 1 - i];
                charArray[charArray.Length - 1 - i] = bufer;
            }
            return new string(charArray);
        }

        private string prepareList(List<string> listOfLexemes)
        {
            string answer = "";
            if (listOfLexemes.Count < 1) return "";
            answer += listOfLexemes[0];
            for (int i = 1; i < listOfLexemes.Count; i++)
            {
                answer += SYMBOL_SEPARATOR + listOfLexemes[i];
            }
            return answer;
        }
    }
}
