using System.Diagnostics;

namespace Trees
{

    public class Program
    {
        public static void Main()
        {
            string[] dictionary = new string[] { "test", "try", "angle", "the", "code", "is", "isnt" };
            PrefixTree trie = new PrefixTree();

            var sw = new Stopwatch();
            foreach (var word in dictionary)
            {
                trie.Add(word);
            }

            Console.WriteLine(string.Join(", ", trie.Search("is")));

        }
    }

    public class PrefixTree
    {
        private PrefixTreeNode root;

        public PrefixTree()
        {
            root = new PrefixTreeNode(String.Empty);
        }

        public void Add(string word)
        {
            AddRecursive(root, word, String.Empty);
        }

        private void AddRecursive(PrefixTreeNode node, string remainingString, string currentString)
        {
            if (remainingString.Length <= 0)
            {
                return;
            }

            char prefix = remainingString[0];
            string substring = remainingString.Substring(1);
            if (!node.SubNodes.ContainsKey(prefix))
            {
                node.SubNodes.Add(prefix, new PrefixTreeNode(currentString + prefix));
            }

            if (substring.Length == 0)
            {
                node.SubNodes[prefix].IsWord = true;
                return;
            }
            else
            {
                AddRecursive(node.SubNodes[prefix], substring, currentString + prefix);
            }
        }


        public IEnumerable<string> Search(string searchString)
        {
            PrefixTreeNode node = root;
            foreach (var search in searchString)
            {
                if (!node.SubNodes.ContainsKey(search))
                {
                    return new string[0];
                }
                node = node.SubNodes[search];
            }

            return FindAllWordsRecursive(node);
        }

        private IEnumerable<string> FindAllWordsRecursive(PrefixTreeNode node)
        {
            if (node.IsWord)
            {
                yield return node.Word;
            }

            foreach (var subnode in node.SubNodes)
            {
                foreach (var result in FindAllWordsRecursive(subnode.Value))
                {
                    yield return result;
                }
            }
        }

        protected class PrefixTreeNode
        {
            private readonly Dictionary<char, PrefixTreeNode> subNodes;
            private bool isWord;
            private readonly string word;

            public PrefixTreeNode(string word)
            {
                subNodes = new Dictionary<char, PrefixTreeNode>();
                isWord = false;
                this.word = word;
            }

            public Dictionary<char, PrefixTreeNode> SubNodes { get { return subNodes; } }
            public bool IsWord { get { return isWord; } set { isWord = value; } }
            public string Word { get { return word; } }
        }
    }

}