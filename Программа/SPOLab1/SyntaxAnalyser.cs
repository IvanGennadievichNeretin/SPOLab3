using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPOLab2;

namespace SPOLab3
{
    class SyntaxAnalyser
    {
        private Stack<string> stack;
        Dictionary<string, Dictionary<string, int>> precedenceTable;
        RuleTable ruleTable;
        public List<ErrorInfo> errorInfos;
        public List<string> ruleSequense;
        public List<string> stackHistory;
        int currentRow;
        public List<LexemeTree> lexemeTrees;

        public SyntaxAnalyser()
        {
            PrecedenceTable table = new PrecedenceTable();
            stack = new Stack<string>();
            ruleTable = new RuleTable();
            precedenceTable = table.getTable();
            errorInfos = new List<ErrorInfo>();
            ruleSequense = new List<string>();
            stackHistory = new List<string>();
            lexemeTrees = new List<LexemeTree>();           
        }

        public void analyse(List<Lexeme> listOfLexemes)
        {
            List<string> currentList = new List<string>();
            int i = 0;
            currentRow = 0;
            errorInfos.Clear();
            bool result;
            while (i < listOfLexemes.Count)
            {
                currentRow = listOfLexemes[i].stringNumber;
                while (i < listOfLexemes.Count)
                {
                    currentList.Add(listOfLexemes[i].value);
                    if (Language.isItTerminal(listOfLexemes[i].value)) break;
                    i++;
                }
                if (analyseThisString(currentList))
                {
                    //reportError("very", "good");
                    TreeConstructor constructor = new TreeConstructor();
                    lexemeTrees.Add(constructor.CreateTree(ruleSequense));
                }
                currentList.Clear();
                i++;
            }
        }

        private bool analyseThisString(List<string> entrySequence)
        {
            string anotherLexeme;
            string terminalFoundInStack;
            int stateGotten;
            Dictionary<string, int> PrecedenceTableCol;

            stack.Push(PrecedenceTable.START_SYMBOL);
            entrySequence.Add(PrecedenceTable.END_SYMBOL);
            for (int i = 0; i < entrySequence.Count;)
            {
                anotherLexeme = entrySequence[i];
                
                terminalFoundInStack = findNonTerminalFromStack();
                if ((anotherLexeme == PrecedenceTable.END_SYMBOL) && (terminalFoundInStack == PrecedenceTable.START_SYMBOL))
                {
                    return true;
                }
                if(precedenceTable.TryGetValue(terminalFoundInStack, out PrecedenceTableCol) != false)
                {
                    if (PrecedenceTableCol.TryGetValue(anotherLexeme, out stateGotten) == false)
                    {
                        reportError("Синтаксическая ошибка: '" + anotherLexeme + "' найдено в ненадлежащем месте. Символ в стэке: ", terminalFoundInStack);
                        return false;
                    }
                    else
                    {
                        if ((stateGotten == PrecedenceTable.STATE_EQUAL) || (stateGotten == PrecedenceTable.STATE_PREVIOUS))
                        {
                            //сдвиг
                            stack.Push(anotherLexeme);
                            i++;
                        }
                        if (stateGotten == PrecedenceTable.STATE_NEXT)
                        {
                            //свертка
                            //перемещение каретки не производится
                            List<string> chainGotten = getRelatedLexemesFromStack();
                            if (!convoluteLexemesIntoRule(chainGotten))
                            {
                                string chain = "";
                                for (int k = 0; k < chainGotten.Count; k++)
                                {
                                    chain += chainGotten[k] + " ";
                                }
                                reportError("Синтаксическая ошибка при формировании конструкции: '" + chain + "'", "");                                                              
                            }
                        }
                    }
                }
            }
            return false;
        }

        private string findNonTerminalFromStack()
        {
            Stack<string> currentStack = copyStack(stack);
            int i;
            while (currentStack.Count > 0)
            {
                if (itIsTerminal(currentStack.Peek()))
                {
                    return currentStack.Peek();
                }
                currentStack.Pop();
            }
            return null;
        }

        private bool itIsTerminal(string a)
        {
            if (Language.isItLegalIdentifier(a) || Language.isItLegalNumber(a) 
                || Language.isItKeyword(a) || Language.isItOperationsWord(a) 
                || Language.isItTerminal(a) || (a == PrecedenceTable.START_SYMBOL))
            {
                return true;
            }
            return false;
        }

        private void reportError(string wordFound, string terminalFoundInStack)
        {
            errorInfos.Add(new ErrorInfo(currentRow, wordFound + terminalFoundInStack));
        }

        private List<string> getRelatedLexemesFromStack()
        {
            List<string> chain = new List<string>();
            Stack<string> buferStack = new Stack<string>();
            List<string> LexemesInStack = new List<string>();
            List<int> removeThis = new List<int>();
            buferStack = copyStack(stack);
            int i;
            while (buferStack.Count > 0)
            {
                LexemesInStack.Add(buferStack.Pop());
            }

            string lastTerminal = "";
            string lastNonTerminal = "";
            string currentLexeme = "";
            bool EqualChainNotFound = true;

            for (i = 0; i < LexemesInStack.Count; i++)
            {
                currentLexeme = LexemesInStack[i];
                if (itIsTerminal(currentLexeme))
                {
                    if (lastTerminal == "")
                    {
                        chain.Add(currentLexeme);
                        removeThis.Add(i);
                        lastTerminal = currentLexeme;
                    }
                    else
                    {
                        if (!itHasStateInPrecedenceTable(currentLexeme, lastTerminal, PrecedenceTable.STATE_EQUAL))
                        {
                            break;
                        }
                        else
                        {
                            chain.Add(currentLexeme);
                            removeThis.Add(i);
                            EqualChainNotFound = false;
                        }
                        lastTerminal = currentLexeme;
                    }
                }
                else
                {
                    chain.Add(currentLexeme);
                    removeThis.Add(i);
                    lastNonTerminal = currentLexeme;
                }
            }

            //Составная цепочка не найдена
            if (EqualChainNotFound)
            {
                lastTerminal = "";
                chain.Clear();
                removeThis.Clear();
                for (i = 0; i < LexemesInStack.Count; i++)
                {
                    currentLexeme = LexemesInStack[i];
                    if (itIsTerminal(currentLexeme))
                    {
                        if (lastTerminal == "")
                        {
                            lastTerminal = currentLexeme;
                            chain.Add(currentLexeme);
                            removeThis.Add(i);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        chain.Add(currentLexeme);
                        removeThis.Add(i);
                    }
                }
            }

            LexemesInStack = removeIndexesFromList(removeThis, LexemesInStack);
            makeThisListAsCurrentStack(LexemesInStack);
            return revertList(chain);
        }

        private Stack<string> copyStack(Stack<string> originalStack)
        {
            Stack<string> buferStack = new Stack<string>();
            Stack<string> returnLater = new Stack<string>();
            Stack<string> currentStack = new Stack<string>();
            int i;
            while (originalStack.Count > 0)
            {
                buferStack.Push(originalStack.Pop());
                returnLater.Push(buferStack.Peek());
            }
            while (buferStack.Count > 0)
            {
                currentStack.Push(buferStack.Pop());
            }
            while (returnLater.Count > 0)
            {
                originalStack.Push(returnLater.Pop());
            }
            return currentStack;
        }

        private List<string> revertList(List<string> list)
        {
            string bufer;
            for (int i = 0; i < list.Count / 2; i++)
            {
                bufer = list[i];
                list[i] = list[list.Count - i - 1];
                list[list.Count - i - 1] = bufer;
            }
            return list;
        }

        private List<string> removeIndexesFromList(List<int> removeIndexes, List<string> fromList)
        {
            for (int i = removeIndexes.Count - 1; i >= 0; i--)
            {
                if (removeIndexes.Contains(i))
                {
                    removeIndexes.Remove(i);
                    fromList.RemoveAt(i);
                }
            }
            return fromList;
        }

        private void makeThisListAsCurrentStack(List<string> list)
        {
            stack.Clear();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                stack.Push(list[i]);
            }
        }

        private bool itHasStateInPrecedenceTable(string row, string col, int expected)
        {
            int valueGotten;
            Dictionary<string, int> rowGotten;
            if (precedenceTable.TryGetValue(row, out rowGotten))
            {
                if (rowGotten.TryGetValue(col, out valueGotten))
                {
                    if (valueGotten == expected)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool convoluteLexemesIntoRule(List<string> listOfLexemes)
        {
            string numberOfRuleGotten;
            string symbolGotten = ruleTable.convolution_getRuleSymbol(listOfLexemes, out numberOfRuleGotten);
            if (symbolGotten == null)
            {
                return false;
            }
            else
            {
                stack.Push(symbolGotten);
                ruleSequense.Add(numberOfRuleGotten);
                Stack<string> buferStack = copyStack(stack);
                string containedInStack = "";
                while (buferStack.Count > 0)
                {
                    containedInStack += buferStack.Pop() + " ";
                }
                stackHistory.Add(containedInStack);
                return true;
            }
            
        }
    }
}
