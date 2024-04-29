using WordFinderSolution.Interfaces;

namespace WordFinderSolution.Services
{
    public class WordFinder : IWordFinder
    {
        private IEnumerable<string> matrix;

        private int matrixHeight { get => matrix.Count(); }
        private int matrixWidth { get => matrix.First().Length; }

        public WordFinder(IEnumerable<string> matrix)
        {
            this.matrix = matrix;
        }

        private void TransposeMatrix()
        {
            var matrixTransposed = new List<string>();
            var rowToInsert = string.Empty;

            for (int column = 0; column < matrixWidth; column++)
            {
                for (int row = 0; row < matrixHeight; row++)
                {
                    rowToInsert = rowToInsert + matrix.ElementAt(row)[column];
                }
                matrixTransposed.Add(rowToInsert);
                rowToInsert = string.Empty;
            }

            matrix = matrixTransposed;
        }

        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            var wordsFound = new List<string>();

            FindHorizontally(wordstream, wordsFound);
            FindVertically(wordstream, wordsFound);

            return wordsFound;
        }

        private void FindVertically(IEnumerable<string> wordstream, List<string> wordsFound)
        {
            // TRANSPOSE MATRIX
            this.TransposeMatrix();

            // Find words horizontally
            FindHorizontally(wordstream, wordsFound);
        }

        private void FindHorizontally(IEnumerable<string> wordstream, List<string> wordsFound)
        { 
            var WordWasFound = false;   

            foreach (var word in wordstream)
            {
                // Find words horizontally
                for (int row = 0; row < matrixHeight; row++)
                {
                    for (int column = 0; column < matrixWidth; column++)
                    {
                        WordWasFound = IsWordFindHorizontally(row, column, word, wordsFound);
                        if (WordWasFound)
                            break;
                    }
                    if (WordWasFound)
                        break;
                }
 
                // Continue with the next word, each word should be found only once
                if (WordWasFound)
                {
                    WordWasFound = false;
                    continue;
                }
            }
        }

        private bool IsWordFindHorizontally(int currentRow, int currentColumn, string wordToFind, List<string> wordsFound)
        {
            // Segment is too short to find the word
            if (matrixWidth - currentColumn < wordToFind.Length)
                return false;

            // Find the word
            for (int column = 0; column < wordToFind.Length; column++)
            {
                if (matrix.ElementAt(currentRow)[currentColumn + column] != wordToFind[column])
                    return false;
            }
            wordsFound.Add(wordToFind);
            return true;
        }
    }
}
