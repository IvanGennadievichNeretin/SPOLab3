using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPOLab2
{
    class LexicalAnalyser
    {
        private String[] strings;
        private List<Lexeme> listOfAllLexemes;
        public List<ErrorInfo> errorInfos;

        public LexicalAnalyser()
        {
            errorInfos = new List<ErrorInfo>();
        }
        
        private void setStrings(String[] newStrings)
        {
            strings = new string[newStrings.Length];
            for (int i = 0; i < newStrings.Length; i++)
            {
                strings[i] = newStrings[i];
            }
        }

        private void prepareStrings(String[] strs)
        {
            for (int i = 0; i < strs.Length; i++)
            {
                //удаление закомментированных строк
                if (strs[i].Contains(Language.Comments))
                {
                    strs[i].Remove(findComment(strs[i]));
                }

            }
        }

        private int findComment(String a)
        {
            int count = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == Language.Comments[count])
                {
                    count++;
                }
                else
                {
                    count = 0;
                }
                if (count >= Language.Comments.Length)
                {
                    return i;
                }
            }
            return -1;
        }

        private List<Lexeme> scanStringList()
        {
            
            Automaton stringScanner = new Automaton();
            listOfAllLexemes = new List<Lexeme>();
            List<Lexeme> buferListOfLexemes = new List<Lexeme>();
            int j = 0;
            int i = 0;
            for (i = 0; i < strings.Length; i++)
            {
                buferListOfLexemes = stringScanner.analyseOneString(strings[i]);
                if (buferListOfLexemes != null)
                {
                    for (j = 0; j < buferListOfLexemes.Count; j++)
                    {
                        buferListOfLexemes[j].setStringNumber(i);
                        listOfAllLexemes.Add(buferListOfLexemes[j]);
                    }
                    buferListOfLexemes.Clear();

                    for (j = 0; j < stringScanner.Errors.Count; j++)
                    {
                        errorInfos.Add(new ErrorInfo(i, stringScanner.Errors[j]));
                    }
                }
                stringScanner.clear();
            }

      
            return listOfAllLexemes;
        }

        private void Clear()
        {
            if (listOfAllLexemes != null)
            {
                listOfAllLexemes.Clear();
            }
            if (errorInfos != null)
            {
                errorInfos.Clear();
            }
        }

        public List<Lexeme> analyseThisText(String[] programStrings)
        {
            Clear();
            setStrings(programStrings);
            prepareStrings(strings);
            List<Lexeme> finalListOfLexemes = scanStringList();
            return finalListOfLexemes;
        }
    }
}
