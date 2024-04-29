using WordFinder.Interfaces;

namespace WordFinder.Services
{
    public class WordFinder : IWordFinder
    {
        public WordFinder(IEnumerable<string> matrix)
        {
        }

        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
        }
    }
}
