using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPOLab2
{
    class Lexeme
    {
        public String name;
        public String type;
        public String value;
        public int stringNumber;
        public Lexeme(String name, String type, String value)
        {
            this.name = name;
            this.type = type;
            this.value = value;
            stringNumber = -1;
        }
        public void setStringNumber(int num)
        {
            stringNumber = num;
        }
    }
}
