using System;

namespace Cardbox.LexiconSearch
{
    public class LazyLoadingTrie : ILazyTrie
    {
        private readonly ILexiconRepository<TrieNode> _lexiconRepository;

        public LazyLoadingTrie(ILexiconRepository<TrieNode> lexiconRepository, IFileProcessor<TrieNode> fileProcessor)
        {
            _lexiconRepository = lexiconRepository;

            LazyLexicon = new Lazy<TrieNode>(() =>
            {
                _lexiconRepository.InitialiseRepository(new[] { fileProcessor });
                return _lexiconRepository[fileProcessor.Path];
            });
        }

        private Lazy<TrieNode> LazyLexicon { get; }

        public TrieNode Lexicon => LazyLexicon.Value;
    }
}