using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SpellingBeeModel;


namespace SpellingBeeModelTest
{

    [TestClass]
    public class UnitTest1
    {
        private Model m = new Model();
        
        [TestMethod]
        public void TestSetUpBoard()
        {
           char[] a = m.SetUpBoard();
           foreach (char c in a)
           {
               Assert.IsTrue('A'<=c && c <= 'Z');
           }
        }

        [TestMethod]
        public void TestScrambleLetters()
        {
            char[] unchanged = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', };
            
            m.board = new []{'a', 'b', 'c', 'd', 'e', 'f', 'g', };
            char first = m.board[0];

            m.ScrambleLetters();

            Assert.AreEqual(first, m.board[0]);

            bool scrambled = false;
            for (int i = 1; i < unchanged.Length; i++)
            {
                if (unchanged[i] != m.board[i])
                {
                    scrambled = true;
                }
            }
            Assert.IsTrue(scrambled);


        }

        [TestMethod]
        public void TestAddCharToWordWorks()
        {
            m.playerWord = "abcdef";
            m.AddCharToWord("g");
            Assert.AreEqual(m.playerWord, "abcdefg");
        }

        [TestMethod]
        public void TestAddCharToWordDoesntWork()
        {
            m.playerWord = "abcdef";
            m.AddCharToWord("*");
            Assert.AreEqual(m.playerWord, "abcdef");
        }

        [TestMethod]
        public void TestDeleteLast()
        {
            m.playerWord = "abcdefg";
            m.DeleteMostRecentCharFromWord();
            Assert.AreEqual(m.playerWord, "abcdef");
        }

        [TestMethod]
        public void TestScore4Let()
        {
            m.playerWord = "abcd";
            m.SetUpBoard();
            int points = m.CalcPoints(m.playerWord);
            Assert.AreEqual(1, points);
        }

        [TestMethod]
        public void TestScore7LetNoPanagram()
        {
            m.playerWord = "abcdeff";
            m.SetUpBoard();
            int points = m.CalcPoints(m.playerWord);
            Assert.AreEqual(4, points);
        }

        [TestMethod]
        public void TestScorePanagram()
        {
            m.playerWord = "abcdefg";
            m.SetUpBoard();
            int points = m.CalcPoints(m.playerWord);
            Assert.AreEqual(11, points);
        }

        [TestMethod]
        public void TestGetAllWordsFoundSorted()
        {
            m.wordsFound = new List<string>();
            m.wordsFound.Add("banana");
            m.wordsFound.Add("apple");

            List<string> ret = m.GetWordsFoundSorted();

            Assert.AreEqual("apple", ret[0]);
            Assert.AreEqual("banana", ret[1]);

            ret[0] = "pear";
            Assert.AreEqual("apple", m.wordsFound[0]) ;
            Assert.AreNotEqual(m.wordsFound[0], ret[0]);
        }


    }
}
