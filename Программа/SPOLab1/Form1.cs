using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SPOLab1
{
    public partial class Form1 : Form
    {
        //глобальные переменные
        String[] readedStrings;
        String currentFile;
        binaryTree Tree;
   

        public Form1()
        {
            InitializeComponent();
            currentFile = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                //выбор файла
                openFileDialog1.Filter = "Файлы txt|*.txt";
                String fileName = "";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog1.FileName;
                }
                label2.Text = fileName;
                currentFile = fileName;

                //чтение из файла
                if (fileName != "")
                {
                    StreamReader reader = new StreamReader(fileName);
                    int stringCount = 0;
                    String bufer;

                    //количество строк в файле
                    while ((bufer = reader.ReadLine()) != null)
                    {
                        stringCount++;
                    }
                    //создание новой коллекции строк
                    readedStrings = new string[stringCount];

                    //обнуление значений для считывания
                    reader.BaseStream.Position = 0;
                    stringCount = 0;
                    while ((bufer = reader.ReadLine()) != null){
                        readedStrings[stringCount] = bufer;
                        stringCount++;
                    }

                    //вывод содержимого файла
                    textBox1.Clear();
                    for (int i = 0; i < stringCount; i++)
                    {
                        textBox1.AppendText(readedStrings[i] + Environment.NewLine);
                    }

                    reader.Close();
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
            
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            String[] strings = new string[textBox1.Lines.Count()];
            for (int i = 0; i < textBox1.Lines.Count(); i++)
            {
                strings[i] = textBox1.Lines[i];
            }
            //лексический анализатор
            SPOLab2.LexicalAnalyser lexicalAnalyser = new SPOLab2.LexicalAnalyser();
            List<SPOLab2.Lexeme> lexemes = lexicalAnalyser.analyseThisText(strings);
            dataGridView1.Rows.Clear();
            for (int i = 0; i < lexemes.Count; i++)
            {
                dataGridView1.Rows.Add(lexemes[i].stringNumber, lexemes[i].name, lexemes[i].type, lexemes[i].value);
            }
            dataGridView2.Rows.Clear();
            for (int j = 0; j < lexicalAnalyser.errorInfos.Count; j++)
            {
                dataGridView2.Rows.Add(lexicalAnalyser.errorInfos[j].stringNumber, lexicalAnalyser.errorInfos[j].info);
            }

            //синтаксический анализатор
            SPOLab3.SyntaxAnalyser syntaxAnalyser = new SPOLab3.SyntaxAnalyser();
            syntaxAnalyser.analyse(lexemes);
            for (int k = 0; k < syntaxAnalyser.errorInfos.Count; k++)
            {
                dataGridView2.Rows.Add(syntaxAnalyser.errorInfos[k].stringNumber, syntaxAnalyser.errorInfos[k].info);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = ".txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String savedFile = saveFileDialog1.FileName;
                StreamWriter writer = new StreamWriter(savedFile);
                Random rng = new Random();
                int randomValue = 0;
                for (int i = 0; i < 1000000; i++)
                {
                    randomValue = rng.Next(0, 1000000000);
                    writer.WriteLine("id" + randomValue);
                }

                writer.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }
    }
}
