using System.Reflection;
using WordFinderSolution.Services;

namespace WordFinderSolutionTest.Services
{
    public class WordFinderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Constructor_MatrixIsEmptyException()
        {
            // Arrange
            var wordStream = new string[] { "cold", "wind", "snow", "chill" };
            var matrix = new string[] { };
            
            // Act
            var exception = Assert.Throws<ArgumentException>(() => new WordFinder(matrix));

            // Assert
            Assert.AreEqual("Matrix is empty (Parameter 'matrix')", exception.Message);
        }

        [Test]
        public void Constructor_MatrixIsTooBigException_ByRowsNumbers()
        {
            // Arrange
            var wordStream = new string[] { "cold", "wind", "snow", "chill" };

            var matrix = new string[65];
            for (int i = 0; i < 65; i++)
            {
                matrix[i] = "aaaaa";
            }

            // Act
            var exception = Assert.Throws<ArgumentException>(() => new WordFinder(matrix));

            // Assert
            Assert.AreEqual("Matrix is too big (Parameter 'matrix')", exception.Message);
        }

        [Test]
        public void Constructor_MatrixIsTooBigException_ByColumnNumbers()
        {
            // Arrange
            var wordStream = new string[] { "cold", "wind", "snow", "chill" };
            var matrix = new string[] { "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" };

            // Act
            var exception = Assert.Throws<ArgumentException>(() => new WordFinder(matrix));

            // Assert
            Assert.AreEqual("Matrix is too big (Parameter 'matrix')", exception.Message);
        }

        [Test]
        public void TransposeMatrix_Success()
        {
            // Arrange
            var wordStream = new string[] { "cold", "wind", "snow", "chill" };
            var matrix = new string[]
            {
                "abcdc",
                "fgwio",
                "chill",
                "pqnsd",
                "uvdxy",
                "chill"
            };
            var matrixTransposed = new string[]
            {
                "afcpuc",
                "bghqvh",
                "cwindi",
                "dilsxl",
                "coldyl"
            };

            var wordFinder = new WordFinder(matrix);
            var TransposeMatrixPrivateMethod = wordFinder.GetType().GetMethod("TransposeMatrix", BindingFlags.NonPublic | BindingFlags.Instance);
            var matrixPrivateVariable = wordFinder.GetType().GetField("matrix", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            TransposeMatrixPrivateMethod.Invoke(wordFinder, null);
            var matrixCreated = (IEnumerable<string>)matrixPrivateVariable.GetValue(wordFinder);

            // Assert
            Assert.AreEqual(matrixTransposed, matrixCreated);
        }

        [Test]
        public void Find_NoWordsFounds()
        {
            // Arrange
            var wordStream = new string[] { "bird", "hot", "snow", "cool" };
            var matrix = new string[] {
                "abcdc",
                "fgwio",
                "chill",
                "pqnsd",
                "uvdxy",
                "chill"
            };

            // Act
            var wordFinder = new WordFinder(matrix);
            var result = wordFinder.Find(wordStream);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void Find_ThreeWordsFounds()
        {
            // Arrange
            var wordStream = new string[] { "cold", "wind", "snow", "chill" };
            var matrix = new string[] {
                "abcdc",
                "fgwio",
                "chill",
                "pqnsd",
                "uvdxy",
                "chill"
            };

            // Act
            var wordFinder = new WordFinder(matrix);
            var result = wordFinder.Find(wordStream);

            // Assert
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void Find_TwelveWordsFounds_Get10MostRepeatedWords()
        {
            // Arrange
            var wordStream = new string[] { "cold", "wind", "dinner", "chill", "bird", 
                                            "hot", "snow", "cool", "hello", "sheep",
                                            "mouse", "cat"};

            var stringToAdd = "";
            var matrix = new List<string>();
            for (int i = 0; i < wordStream.Length; i++)
            {
                for (int r = 0; r < i+1; r++)
                {
                    if(stringToAdd.Length + wordStream[i].Length > 64)
                    {
                        matrix.Add(stringToAdd);
                        stringToAdd = "";
                    }
                    stringToAdd = stringToAdd + wordStream[i] + "x";
                }
                matrix.Add(stringToAdd);
                stringToAdd = "";
            }

            var maxLength = matrix.Max(x => x.Length);
            for (int i = 0; i < matrix.Count(); i++)
            {
                if (matrix[i].Length < maxLength)
                {
                    var filling = new string('a', maxLength - matrix[i].Length);
                    matrix[i] = matrix[i] + filling;
                }
            }

            // Act
            var wordFinder = new WordFinder(matrix);
            var result = wordFinder.Find(wordStream);

            // Assert
            Assert.AreEqual(10, result.Count());
            Assert.AreEqual("cat", result.ElementAt(0));
            Assert.AreEqual("mouse", result.ElementAt(1));
            Assert.AreEqual("sheep", result.ElementAt(2));
            Assert.AreEqual("hello", result.ElementAt(3));
            Assert.AreEqual("cool", result.ElementAt(4));
            Assert.AreEqual("snow", result.ElementAt(5));
            Assert.AreEqual("hot", result.ElementAt(6));
            Assert.AreEqual("bird", result.ElementAt(7));
            Assert.AreEqual("chill", result.ElementAt(8));
            Assert.AreEqual("dinner", result.ElementAt(9));
        }

        [Test]
        public void IsWordFindHorizontally_SegmentIsTooShortToFindWord()
        {
            // Arrange
            var wordStream = new string[] { "cold" };
            var matrix = new string[] {
                "abcol"
            };

            // Act
            var wordFinder = new WordFinder(matrix);
            var IsWordFindHorizontallyPrivateMethod = wordFinder.GetType().GetMethod("IsWordFindHorizontally", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            int currentRow = 0;
            int currentColumn = 2;
            string wordToFind = "cold";
            List<string> wordsFound = new List<string>();
            object[] parametros = new object[] { currentRow, currentColumn, wordToFind, wordsFound };

            var result = (bool)IsWordFindHorizontallyPrivateMethod.Invoke(wordFinder, parametros);
            
            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsWordFindHorizontally_WordNoFound()
        {
            // Arrange
            var wordStream = new string[] { "cold" };
            var matrix = new string[] {
                "abxyz"
            };

            // Act
            var wordFinder = new WordFinder(matrix);
            var IsWordFindHorizontallyPrivateMethod = wordFinder.GetType().GetMethod("IsWordFindHorizontally", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            int currentRow = 0;
            int currentColumn = 1;
            string wordToFind = "cold";
            List<string> wordsFound = new List<string>();
            object[] parametros = new object[] { currentRow, currentColumn, wordToFind, wordsFound };

            var result = (bool)IsWordFindHorizontallyPrivateMethod.Invoke(wordFinder, parametros);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsWordFindHorizontally_WordFound()
        {
            // Arrange
            var wordStream = new string[] { "cold" };
            var matrix = new string[] {
                "abccoldxyz"
            };

            // Act
            var wordFinder = new WordFinder(matrix);
            var IsWordFindHorizontallyPrivateMethod = wordFinder.GetType().GetMethod("IsWordFindHorizontally", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            int currentRow = 0;
            int currentColumn = 3;
            string wordToFind = "cold";
            List<string> wordsFound = new List<string>();
            object[] parametros = new object[] { currentRow, currentColumn, wordToFind, wordsFound };

            var result = (bool)IsWordFindHorizontallyPrivateMethod.Invoke(wordFinder, parametros);

            // Assert
            Assert.AreEqual(true, result);
        }
    }
}