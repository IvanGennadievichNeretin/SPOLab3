using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPOLab3
{
    class TreeConstructor
    {
        string[] numberToRuleResult;
        Stack<int> rules;

        public TreeConstructor()
        {
            numberToRuleResult = new string[9];
            numberToRuleResult[0] = "";
            numberToRuleResult[1] = "a := S;";
            numberToRuleResult[2] = "S or S";
            numberToRuleResult[3] = "S xor S";
            numberToRuleResult[4] = "S";
            numberToRuleResult[5] = "S and S";
            numberToRuleResult[6] = "(S)";
            numberToRuleResult[7] = "not(S)";
            numberToRuleResult[8] = "a";

            rules = new Stack<int>();
        }

        public LexemeTree CreateTree(List<string> ruleSequence)
        {
            for (int i = 0; i < ruleSequence.Count; i++)
            {
                rules.Push(Convert.ToInt32(ruleSequence[i]));
            }
            LexemeTree primalTree = new LexemeTree();
            primalTree.contains = "S";
            string StringGotten = CreateNewBranch(ref primalTree);
            return primalTree;
        }

        private string CreateNewBranch(ref LexemeTree tree)
        {
            LexemeTree newChild = new LexemeTree();
            tree.addChild(newChild);
            if (rules.Count == 0) return "";         
            string stringGottenFromRule = numberToRuleResult[rules.Pop()];
            newChild.contains = stringGottenFromRule;
            while (stringGottenFromRule.Contains('S'))
            {                
                int indexS = stringGottenFromRule.IndexOf('S');
                stringGottenFromRule = stringGottenFromRule.Remove(indexS, 1);
                stringGottenFromRule.Insert(indexS, CreateNewBranch(ref newChild));
            }

            return stringGottenFromRule;
        }

        
    }
}
