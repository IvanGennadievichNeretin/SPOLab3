using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPOLab3
{
    class LexemeTree
    {
        public string contains;
        public List<LexemeTree> childs;
        private LexemeTree parent;
        public LexemeTree()
        {
            childs = new List<LexemeTree>();
        }
        public void addChild(LexemeTree newChild)
        {
            newChild.parent = this;
            childs.Add(newChild);
        }               
        public LexemeTree getParent()
        {
            return parent;
        }
        public void setParent(LexemeTree newParent)
        {
            parent = newParent;
        }

        public List<string> toLines()
        {
            List<string> result = new List<string>();
            int depth = 0;
            printBranch(ref result, this, depth);
            return result;
        }

        private void printBranch(ref List<string> list, LexemeTree node, int depth)
        {
            if (node == null) return;
            list.Add(indentViaDepth(depth) + node.contains);
            for (int i = 0; i < node.childs.Count; i++)
            {
                printBranch(ref list, node.childs[i], depth + 1);
            }
        }

        private string indentViaDepth(int depth)
        {
            string result = "";
            for (int i = 0; i <= depth; i++)
            {
                result += "  ";
            }
            return result;
        }
    }
}
