using System;
using WordFinderSolution.Services;

namespace WordFinderSolution
{
    internal class Program
    {
        static void Main()
        {
            // Inputs 
            var wordStream = new string[] { "cold", "wind", "snow", "chill" };
            var matrix = new string[] { 
                "abcdc", 
                "fgwio", 
                "chill", 
                "pqnsd", 
                "uvdxy",
                "chill"
            };
            
            // WordFinder service
            var wordFinder = new WordFinder(matrix);
            var result = wordFinder.Find(wordStream);
            foreach (var word in result)
            {
                Console.WriteLine(word);
            }   
        }
    }
}