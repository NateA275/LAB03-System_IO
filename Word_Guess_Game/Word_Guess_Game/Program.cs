using System;
using System.IO;
using System.Text;

namespace Word_Guess_Game
{
    public class Program
    {
        public static string path = "../../../WordBank.txt";
        public static string[] allWords;

        /// <summary>
        /// Word Guessing Game! Users can play a Hangman-Style word guessing game and add or remove words from the associated library.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            bool runFlag = true;
            CreateFile();
            ReadFile();
            while (runFlag)
            {
                try
                {
                    Menu();
                    int userChoice = GetUserNum();
                    switch (userChoice)
                    {
                        case 1:
                            RunGame();
                            break;
                        case 2:
                            Console.Clear();
                            Console.Write("Enter word to add: ");
                            AddWord(GetUserString());
                            break;
                        case 3:
                            Console.Clear();
                            PrintWords();
                            Console.Write("Enter word to remove: ");
                            RemoveWord(GetUserString());
                            break;
                        case 4:
                            Console.Clear();
                            PrintWords();
                            Console.WriteLine("\nPress ENTER to Continue");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 5:
                            runFlag = false;
                            break;
                    }
                }
                catch (Exception)
                {
                }
            }
            Console.Clear();
            Console.WriteLine("Thanks for playing!");
            Console.ReadLine();
        }

        /// <summary>
        /// RunGame - Users can play the game using a randomly selected word from the library.
        /// </summary>
        public static void RunGame()
        {
            Console.Clear();
            string keyWord = RandomWord();
            StringBuilder wrongLetters = new StringBuilder();
            StringBuilder correctLetters = new StringBuilder();
            
            int numGuesses = 0;
            bool playGame = true;

            while (playGame)
            {
                try
                {
                    int placedLetters = 0;
                    for (int i = 0; i < keyWord.Length; i++)
                    {
                        if (correctLetters.ToString().Contains(keyWord[i]))
                        {
                            Console.Write($"{keyWord[i]} ");
                            placedLetters++;
                        }
                        else
                            Console.Write("_ ");
                    }
                    Console.WriteLine();
                    Console.WriteLine($"Guessed Letters: {wrongLetters.ToString()}");

                    if (placedLetters == keyWord.Length)
                    {
                        Console.Clear();
                        Console.WriteLine("You WON!");
                        playGame = false;
                        Console.WriteLine("Press ENTER to continue.");
                        Console.ReadLine();
                    }
                    else if (numGuesses == 5)
                    {
                        Console.Clear();
                        Console.WriteLine("You ran out of attempts.");
                        Console.WriteLine($"The word was '{keyWord}'");
                        playGame = false;
                        Console.WriteLine("Press ENTER to continue.");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.Write("Guess a letter: ");
                        char usersInput = GetUserChar();
                        Console.Clear();

                        if (keyWord.Contains(usersInput))
                            correctLetters.Append(usersInput);
                        else
                        {
                            wrongLetters.Append($"{usersInput} ");
                            numGuesses++;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Enter a single letter.");
                }
                finally
                {
                    Console.Clear();
                }
            }
        }

        /// <summary>
        /// AddWord - Users can add a word the the Game's library
        /// </summary>
        public static void AddWord(string newWord)
        {
            Console.Clear();
            try
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(newWord);
                }

                ReadFile();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Console.Clear();
            }
        }

        /// <summary>
        /// RemoveWord - Users can remove a word from the Game's library
        /// </summary>
        public static void RemoveWord(string oldWord)
        {
            try
            {
                string tempFile = Path.GetTempFileName();
                using (StreamReader sr = new StreamReader(path))
                using (StreamWriter sw = new StreamWriter(tempFile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line != oldWord)
                            sw.WriteLine(line);
                    }
                }
                File.Delete(path);
                File.Move(tempFile, path);
                ReadFile();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Console.Clear();
            }
        }

        /// <summary>
        /// GetUserString - Retrieves and validates a string input from the user
        /// </summary>
        /// <returns name="userWord"> string - user's input </returns>
        public static string GetUserString()
        {
            try
            {
                string userWord = Console.ReadLine().ToLower();

                return userWord;
            }
            catch (Exception)
            {
                Console.WriteLine("Your word must be all letters.");
                throw;
            }
        }

        /// <summary>
        /// GetUserChar - Retrieves and validates a character input from the user
        /// </summary>
        /// <returns name="letter"> char - user's entered character </returns>
        public static char GetUserChar()
        {
            try
            {
                char letter = char.Parse(Console.ReadLine().ToLower());
                if (!char.IsLetter(letter))
                {
                    Console.WriteLine("Please Enter an alphabetic character.");
                    letter = GetUserChar();
                }
                return letter;
            }
            catch (Exception)
            {
                Console.WriteLine("Enter an aplabetic character.");
                throw;
            }
        }

        /// <summary>
        /// GetUserNum - Retrieves and validates an integer input from the user
        /// </summary>
        /// <returns name="choice"> int - user's enter integer [1, 4] </returns>
        public static int GetUserNum()
        {
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice < 1 || choice > 5)
                {
                    Console.WriteLine("Try again.");
                    choice = GetUserNum();
                }
                return choice;
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Input");
                throw;
            }
        }

        /// <summary>
        /// CreateFile - Generates a Library file if one has not already been created.
        /// </summary>
        public static void CreateFile()
        {
            if (!File.Exists(path))
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("cat");
                }
            }
        }

        /// <summary>
        /// ReadFile - Reads lines from library file and assigns them to AllWords variable
        /// </summary>
        public static void ReadFile()
        {
            allWords = File.ReadAllLines(path);
        }

        /// <summary>
        /// PrintWords - Prints all current library words onto user's console
        /// </summary>
        public static void PrintWords()
        {
            foreach (string word in allWords)
            {
                Console.WriteLine(word);
            }
        }

        /// <summary>
        /// RandomWord - Returns a randomly selected word from library file
        /// </summary>
        /// <returns> string - randomly selected word from library file </returns>
        public static string RandomWord()
        {
            try
            {
                Random n = new Random();
                return allWords[n.Next(0, allWords.Length)];
            }
            catch (Exception)
            {
                Console.WriteLine("Library is empty. Add words to begin.");
                throw;
            }

        }

        /// <summary>
        /// Menu - Presents the user with main menu options
        /// </summary>
        public static void Menu()
        {
            Console.WriteLine("1. Start New Game");
            Console.WriteLine("2. Add New Word");
            Console.WriteLine("3. Remove Word");
            Console.WriteLine("4. View Word Bank");
            Console.WriteLine("5. Exit");
        }
    }
}