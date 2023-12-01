using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Transactions;
using Microsoft.VisualBasic.CompilerServices;
using WeCantSpell.Hunspell;

namespace SpellingBeeModel
{
    public class PackageOfInfoForPlayer
    {
        public PackageOfInfoForPlayer(bool b, List<string> l, int x)
        {
            this.kosher = b;
            this.found = l;
            this.score = x;
        }

        public readonly bool kosher;
        public readonly List<string> found;
        public readonly int score;
    }

    public interface IModel
    {
        string AddCharToWord(string c);

        /// <summary>
        /// tell model if kosher
        /// increment score
        /// send list of found words
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        PackageOfInfoForPlayer CommitWord(string s);
        int CalcPoints(string s);

        char[] SetUpBoard();
        char[] ScrambleLetters();

        string DeleteMostRecentCharFromWord();
        List<string> GetWordsFoundSorted();

    }
    public class Model : IModel
    {
        internal bool kosherWord;
        internal bool mandatoryLetterUsed;
        internal string playerWord = "";
        internal readonly string abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        internal char[] board;
        internal int numChars = 7;
        internal int numFound;
        internal int score;
        internal int minLength = 4;
        internal string starter;
        private WordList dictionary;
        private bool firstMove;
        


        internal List<string> wordsFound;

        private Random rand = new Random();


        public int NumChars => board.Length;
        public int NumFound => numFound;

        public int Score => score;


        public char[] Letters {
            get
            {
                char[] ret = new char[board.Length];
                int i = 0;
                foreach (var c in board)
                {
                    ret[i++] = c;
                }

                return ret;
            }
        }


        public Model()
        {
            dictionary = WordList.CreateFromFiles(@"English (American).dic");
            //GenerateStarterWord();

        }

        internal void GenerateStarterWord()
        {
            StreamReader sr = new StreamReader("C:\\Users\\BDK\\Source\\Repos\\spellingbee-DBKorn\\startWords.txt");
            string line = "";
            List<string> a = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                a.Add(line);
            }

            int r = rand.Next(a.Count);
            starter = a[r].ToUpper();
            a = null;
        }


        public char[] SetUpBoard()
        {
            numFound = 0;
            this.firstMove = true;
            score = 0;
            board = new char[7];
            this.playerWord = null;
            this.playerWord = "";
            wordsFound = new List<string>();
            GenerateStarterWord();
            for (int i = 0; i < board.Length; i++)
            {
                board[i] = starter[i];
            }
            ScrambleLetters();
            return board;
        }

        public string AddCharToWord(string c)
        {
            if ( (firstMove) && (playerWord.Length == 0))  //was having bugs with making a new game; playerWord wasn't being reset as empty. this patches up the bug
            {
                playerWord = "";
                firstMove = false;
            }
            if (IsALetter(c[0]))
            {
                this.playerWord += c;
            }
            return playerWord.ToUpper();
        }

        private bool IsALetter(char c)
        {
            return ( ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z') );
        }

        public PackageOfInfoForPlayer CommitWord(string s) 
        {
            

            if ( (s.Length >= minLength) && (IsInDictionary(s)) && (!IsFoundAlready(s)) && (s.Contains(Letters[0])) ) //not sure why it wasn't working without the () around each thingy
            {
                wordsFound.Add(s);
                CalcPoints(s);
                playerWord = "";
                return new PackageOfInfoForPlayer(true, GetWordsFoundSorted(), score);
            }

            //playerWord = "";
            return new PackageOfInfoForPlayer(false, GetWordsFoundSorted(), score);

        }

        internal Boolean IsInDictionary(string s)
        {
            return dictionary.Check(s);
        }

        internal bool IsFoundAlready(string s)
        {
            return wordsFound.Contains(s);
        }

        public int CalcPoints(string s)
        {
            
            HashSet<char> dupKiller = new HashSet<char>(s);
            if (dupKiller.Count >= board.Length)
            {
                score += board.Length;
            }

            score += s.Length - minLength + 1;

            return score;
        }

        public char[] ScrambleLetters()
        {
            for (int i = 1; i < board.Length; i++)
            {
                Swap(board, i, rand.Next(board.Length-1)+1);
            }

            return Letters;
        }

        public void Swap(char[] a, int x, int y)
        {
            char temp = a[x];
            a[x] = a[y];
            a[y] = temp;
        }

        public string DeleteMostRecentCharFromWord()
        {
            if (playerWord.Length > 1)
            {
                this.playerWord = playerWord.Substring(0, playerWord.Length - 1);
            } else if (playerWord.Length == 1)
            {
                this.playerWord = "";
            }
            
            return playerWord;
        }
        

        public List<string> GetWordsFoundSorted()
        {
            wordsFound.Sort();

            List<string> deepCopy = new List<string>(wordsFound);

            return deepCopy;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
