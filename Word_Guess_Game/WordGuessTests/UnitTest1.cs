using System;
using Xunit;
using System.IO;
using Word_Guess_Game;

namespace WordGuessTests
{
    public class UnitTest1
    {
        [Fact]
        public void CanCreateFile()
        {
            //Arrange
            Program.path = "TESTFILE.txt";

            //Act
            Program.CreateFile();

            //Assert
            Assert.True(File.Exists(Program.path));
        }


        [Theory]
        [InlineData("TEST", "test")]
        [InlineData("cat", "dog")]
        [InlineData("blue", "red")]
        public void CanReadFile(string word1, string word2)
        {
            //Arrange
            Program.path = "TESTFILE.txt";
            using (StreamWriter sw = new StreamWriter(Program.path))
            {
                sw.WriteLine(word1);
                sw.WriteLine(word2);
            }

            //Act
            Program.ReadFile();

            //Assert
            Assert.Equal(word1, Program.allWords[0]);
            Assert.Equal(word2, Program.allWords[1]);
        }


        [Theory]
        [InlineData("testword")]
        [InlineData("secondTestWord")]
        [InlineData("thirdTestWord")]
        public void CanAddWordToFile(string word)
        {
            //Arrange
            Program.path = "TESTFILE.txt";

            //Act
            Program.AddWord(word);

            //Assert
            Assert.Equal(word, Program.allWords[Program.allWords.Length - 1]);
        }


        [Theory]
        [InlineData("thisTestWord")]
        [InlineData("orThisTestWord")]
        [InlineData("orMaybeThisTestWord")]
        public void CanRemoveWordFromFile(string word)
        {
            //Arrange
            Program.path = "TESTFILE.txt";
            Program.AddWord(word);

            //Act
            Program.RemoveWord(word);

            //Assert
            Assert.Equal(-1, Array.IndexOf(Program.allWords, word));
        }

        [Theory]
        [InlineData("cat", "bird", "dog", 3)]
        public void CanRetrieveAllWordsFromFile(string word1, string word2, string word3, int length)
        {
            //Arrange
            Program.path = "TESTFILE.txt";
            using (StreamWriter sw = new StreamWriter(Program.path))
            {
                sw.WriteLine(word1);
                sw.WriteLine(word2);
                sw.WriteLine(word3);
            }

            //Act
            Program.ReadFile();

            //Assert
            Assert.Equal(length, Program.allWords.Length);
        }
    }
}
