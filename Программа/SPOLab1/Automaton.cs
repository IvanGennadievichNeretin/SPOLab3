using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPOLab2
{
    class Automaton
    {
        Dictionary<int, Dictionary<String, int>> TransitionTable;
        private const int STATE_START = 1;
        private const int STATE_WRITING_NUMBER = 2;
        private const int STATE_WRITING_IDENTIFIER = 3;
        private const int STATE_WRITING_OPERATION = 4;
        private const int STATE_OPERATION_EXPECTED = 5;
        private const int STATE_END = 6;
        private const int STATE_ERROR = 0;
        private const char TYPE_OPERATION = 'o';
        private const char TYPE_NUMBER = 'n';
        private const char TYPE_IDENTIFIER = 'a';
        private int currentState;
        private String newWord;
        private List<Lexeme> generatedLexemes;
        public List<String> Errors;
        public Automaton()
        {
            generatedLexemes = new List<Lexeme>();
            Errors = new List<String>();
            //инициализация таблицы переходов автоматона в соответствии с заданием
            TransitionTable = new Dictionary<int, Dictionary<String, int>>();

            Dictionary<String, int> state_start_transitions = new Dictionary<String, int>();
            state_start_transitions.Add(" ", STATE_START);
            state_start_transitions.Add("(", STATE_START);
            state_start_transitions.Add(")", STATE_START);
            state_start_transitions.Add("n", STATE_WRITING_NUMBER);
            state_start_transitions.Add("a", STATE_WRITING_IDENTIFIER);
            state_start_transitions.Add(";", STATE_END);
            TransitionTable.Add(STATE_START, state_start_transitions);

            Dictionary<String, int> state_writing_identifier = new Dictionary<String, int>();
            state_writing_identifier.Add("n", STATE_WRITING_IDENTIFIER);
            state_writing_identifier.Add("a", STATE_WRITING_IDENTIFIER);
            state_writing_identifier.Add("(", STATE_START);
            state_writing_identifier.Add(")", STATE_START);
            state_writing_identifier.Add(" ", STATE_START);
            state_writing_identifier.Add(";", STATE_END);
            TransitionTable.Add(STATE_WRITING_IDENTIFIER, state_writing_identifier);

            Dictionary<String, int> state_writing_number = new Dictionary<String, int>();
            state_writing_number.Add("(", STATE_START);
            state_writing_number.Add(")", STATE_START);
            state_writing_number.Add(" ", STATE_START);
            state_writing_number.Add("n", STATE_WRITING_NUMBER);
            state_writing_number.Add(";", STATE_END);
            TransitionTable.Add(STATE_WRITING_NUMBER, state_writing_number);

            Dictionary<String, int> state_error = new Dictionary<String, int>();
            state_error.Add("(", STATE_START);
            state_error.Add(")", STATE_START);
            state_error.Add(" ", STATE_START);
            state_error.Add(";", STATE_END);
            TransitionTable.Add(STATE_ERROR, state_error);
        }

        public List<Lexeme> analyseOneString(String a)  //analyse all before ";" symbol
        {
            char anotherSymbol;
            int i = 0;
            newWord = "";
            int nextState = STATE_START;
            Dictionary<String, int> bufer;
            bool nextStateIsGotten = true;

            currentState = STATE_START;
            while ((currentState != STATE_END) && (newWord != Language.Comments) && (i < a.Length)){
                anotherSymbol = a[i];
                
                 //узнать, какое состояние будет следующим
                TransitionTable.TryGetValue(currentState, out bufer);
                nextStateIsGotten = bufer.TryGetValue(Convert.ToString(convertToAutomatonCommand(anotherSymbol)), out nextState);
                if (!nextStateIsGotten)
                {
                    nextState = STATE_ERROR;
                }

                if (currentState != nextState)
                {
                    putWord(newWord); 
                    newWord = "";
                }
                currentState = nextState;
                newWord += anotherSymbol;
                i++;
            }
            putWord(newWord);
            return generatedLexemes;
        }

        private void putWord(String a)
        {
            Lexeme newLexeme;
            a = a.Replace(" ", "");
            if (a == "")
            {
                return;
            }

            if ((a.Contains("(") || a.Contains(")")) && a.Length > 1)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    putWord(Convert.ToString(a[i]));
                }
                return;
            }        
            if (a.Contains("("))
            {
                newLexeme = new Lexeme(a, "Левая скобка", a);
                generatedLexemes.Add(newLexeme);
                return;
            }
            if (a.Contains(")"))
            {
                newLexeme = new Lexeme(a, "Правая скобка", a);
                generatedLexemes.Add(newLexeme);
                return;
            }
            if (Language.isItKeyword(a))
            {
                newLexeme = new Lexeme(a, "Ключевое слово", a);
                generatedLexemes.Add(newLexeme);
                return;
            }
            if (Language.isItOperationsWord(a))
            {
                newLexeme = new Lexeme(a, "Операция", a);
                generatedLexemes.Add(newLexeme);
                return;
            }
            if (Language.isItTerminal(a))
            {
                newLexeme = new Lexeme(a, "Терминал", a);
                generatedLexemes.Add(newLexeme);
                return;
            }
            if (Language.isItLegalIdentifier(a))
            {
                newLexeme = new Lexeme(a, "Идентификатор", a);
                generatedLexemes.Add(newLexeme);
                return;
            }
            if (Language.isItLegalNumber(a))
            {
                newLexeme = new Lexeme(a, "Число", a);
                generatedLexemes.Add(newLexeme);
                return;
            }
            if (a == Language.Comments)
            {
                return;
            }
            if (currentState == STATE_ERROR)
            {
                reportError(a);
                return;
            }
        }

        private char convertToAutomatonCommand(char a)
        {
            char newChar = a;
            if (Language.isItIdentifierSymbol(Convert.ToString(a)))
            {
                newChar = TYPE_IDENTIFIER;
            }
            if (Language.isItNumbersSymbol(Convert.ToString(a)))
            {
                newChar = TYPE_NUMBER;
            }
            return newChar;
        }

        public void clear()
        {
            currentState = STATE_START;
            newWord = "";
            generatedLexemes.Clear();
            Errors.Clear();
        }

        void reportError(String symbol)
        {
            switch (currentState)
            {
                case STATE_START:
                    Errors.Add("Нелегальный идентификатор: '" + symbol + "'");
                    break;
                case STATE_WRITING_NUMBER:
                    Errors.Add("Ошибка в написании числа: '" + symbol + "'");
                    break;
                case STATE_WRITING_IDENTIFIER:
                    Errors.Add("'"+ symbol +"' - некорректный идентификатор");
                    break;
                case STATE_END:
                    Errors.Add("Не удалось завершить строку");
                    break;
                default:
                    Errors.Add("Нелегальный идентификатор: '" + symbol + "'");
                    break;
            }
            
        }
    }
}
