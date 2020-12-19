using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPOLab1
{
    public class binaryTree
    {
        public class node
        {
            public node LeftChild;
            public node RightChild;
            public String Value;
            public node(String Val, node leftChild, node rightChild)
            {
                LeftChild = leftChild;
                RightChild = rightChild;
                Value = Val;
            }
        }
        
        private node[] identifyers;
        private int massiveLength;
        private int count;

        public binaryTree(int size)
        {
            identifyers = new node[size+500];
            for (int i = 0; i < size+500; i++)
            {
                identifyers[i] = null;
            }
            massiveLength = size+500;
            count = 0;
        }

        private int getNumberLeftChild(int number)
        {
            return ((number + 1) * 2) - 1;
        }
        private int getNumberRightChild(int number)
        {
            return (number + 1) * 2;
        }
 
        public bool put(String Value, ref int operations)
        {
            ref node currentNode = ref identifyers[0];
            ref node previousNode = ref identifyers[0];
            int limitCheck = 0;
            while (limitCheck < massiveLength)
            {
                if (currentNode == null)
                {
                    currentNode = new node(Value, null, null);
                    count++;
                    operations++;
                    return true;                    
                }
                if (String.Compare(Value, currentNode.Value) < 0)
                {
                    currentNode = ref previousNode.LeftChild;
                }
                else
                {
                    currentNode = ref previousNode.RightChild;
                }
                operations++;
                previousNode = ref currentNode;
                limitCheck++;
            }
            return false;
        }

        public bool isItExist(String Value, ref int operations)
        {
            node currentNode = identifyers[0];
            int limitCheck = 0;
            while (limitCheck < massiveLength)
            {
                if (currentNode != null)
                {
                    operations++;
                    if (currentNode.Value.Equals(Value))
                    {
                        operations++;
                        return true;
                    }
                    if (String.Compare(Value, currentNode.Value) < 0)
                    {
                        currentNode = currentNode.LeftChild;
                    }
                    else
                    {
                        currentNode = currentNode.RightChild;
                    }
                    operations++;
                    limitCheck++;
                }
                else
                {
                    operations++;
                    return false;
                }
            }
            return false;
        }

        public static binaryTree StrListToTree(String[] list, ref int dupes)
        {
            int listSize = list.Length;
            binaryTree NewTree = new binaryTree(listSize);
            int operations = 0;
            for (int i = 0; i < listSize; i++)
            {
                if (NewTree.isItExist(list[i], ref operations))
                {
                    dupes++;
                }
                else
                {
                    NewTree.put(list[i], ref operations);
                }
            }
            return NewTree;
        }
    }
}
