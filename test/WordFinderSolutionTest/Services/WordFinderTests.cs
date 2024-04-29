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
        public void Find_ThreeWordsFounds()
        {
            // Assert
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

            // Arrange
            Assert.AreEqual(3, result.Count());
        }
    }
}