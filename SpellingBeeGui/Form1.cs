using SpellingBeeModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SpellingBeeGUI
{
    public partial class Form1 : Form
    {
        Button[] buttons;
        private Model model = new Model();
        private int numChars;
        private char[] letters;
        private int score;

        public Form1()
        {
            GetSetUpDataFromModel();

            InitializeComponent();

            InitializeComponent2();
            
        }

        private void GetSetUpDataFromModel()
        {
            letters = model.SetUpBoard();
            this.numChars = model.NumChars;
        }

        private void NewGame(object sender, EventArgs e)
        {
            GetSetUpDataFromModel();
            SetTextOfButtons();
            this.label1.Text = "";

            this.richTextBox1.Text = "You have found " + (0) + " words\n";
            UpdateScore(0);
        }

        private void InitializeComponent2()
        {
            this.buttons = new[] {this.button1, this.button2, this.button3, 
                this.button4, this.button5, this.button6, this.button7, };
            
            for (int i = 0; i < buttons.Length; i++)
            {
                //buttons[i].Click += new System.EventHandler(this.ClickLetter);
                buttons[0].BackColor = Color.White;
                buttons[i].Tag = i;
            }
            SetTextOfButtons();

            buttons[0].BackColor = Color.Yellow;
            this.richTextBox1.Text = "You have found " + (0) + " words\n";
        }

        private void SetTextOfButtons()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Text = ("" + model.Letters[i]).ToUpper();
            }
        }



        private void ClickLetter(object sender, EventArgs e)
        {
            Button b = (Button) sender;
            this.label1.Text = model.AddCharToWord(b.Text);
            
        }

        private void UpdateScore(int x)
        {
            score = x;
            label2.Text = "Points: " + score;
        }

        private void UpdateListOfFound(List<string> list)
        {
            this.richTextBox1.Text = "You have found " + list.Count + " words\n";
            foreach (string s in list)
            {
                this.richTextBox1.AppendText(s+"\n");
            }
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void SubmitWord(object sender, EventArgs e)
        {
            PackageOfInfoForPlayer pack = model.CommitWord(label1.Text);
            if (pack.kosher)
            {
                this.label1.Text = "";
                
            }
            UpdateScore(pack.score);
            UpdateListOfFound(pack.found);

        }

        private void DeleteLast(object sender, EventArgs e)
        {
            //Button b = (Button)sender;
            this.label1.Text = model.DeleteMostRecentCharFromWord();
        }
        private void Scramble(object sender, EventArgs e)
        {
            model.ScrambleLetters();
            SetTextOfButtons();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
