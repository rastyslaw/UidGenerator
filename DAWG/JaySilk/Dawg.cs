using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAWG.JaySilk
{
    public class Dawg
    {
        private readonly List<(Node, char, Node)> uncheckedNodes = new List<(Node, char, Node)>();
        private readonly HashSet<Node> minimizedNodes = new HashSet<Node>();
        private string previousWord = "";

        public Node Root { get; } = new Node(true);
        public int NodeCount => Nodes.Count();
        public IEnumerable<Node> Nodes => DepthFirstNodeIterator(Root);
        public int EdgeCount => Nodes.Sum(n => n.Children.Count);

        public void Insert(string word) {
            var commonPrefix = 0;
            foreach (var i in Enumerable.Range(0, Math.Min(word.Length, previousWord.Length))) {
                if (word[i] != previousWord[i]) break;
                commonPrefix++;
            }

            Minimize(commonPrefix);

            var node = (uncheckedNodes.Count() == 0) ? Root : uncheckedNodes[^1].Item3;
            foreach (var c in word.Substring(commonPrefix)) {
                var next = new Node();
                node.Children[c] = next;
                uncheckedNodes.Add((node, c, next));
                node = next;
            }

            node.EndOfWord = true;
            previousWord = word;
        }

        public void Finish() {
            Minimize(0);
        }

        private void Minimize(int downTo) {
            for (var i = uncheckedNodes.Count() - 1; i > downTo - 1; i--) {
                var (parent, letter, child) = uncheckedNodes[i];
                if (minimizedNodes.Contains(child)) {
                    minimizedNodes.TryGetValue(child, out Node val);
                    parent.Children[letter] = val;
                }
                else
                    minimizedNodes.Add(child);

                uncheckedNodes.Remove(uncheckedNodes[^1]);
            }
        }

        public bool Exists(string word) {
            var node = Root;
 
            foreach (var l in word)
                if (node.Children.ContainsKey(l)) 
                    node = node.Children[l];
                 else 
                    return false;

            return node.EndOfWord;  
        }

        public IEnumerable<string> MapToString(Node root) {
            var stack = new Stack<Node>();
            var done = new HashSet<int>();

            stack.Push(root);

            while (stack.Count() > 0) {
                var content = new StringBuilder();
                var node = stack.Pop();

                if (done.Contains(node.Id)) continue;

                content.AppendLine($"{node.Id,-5} {node.ToString()}");

                foreach (var (key, child) in node.Children.OrderByDescending(x => x.Key)) {
                    content.AppendLine($"{key,5} goto {child.Id}");
                    stack.Push(child);
                    done.Add(node.Id);
                }

                yield return content.ToString();
            }
        }

        private IEnumerable<Node> DepthFirstNodeIterator(Node root) {
            var stack = new Stack<Node>();
            var done = new HashSet<int>();

            stack.Push(root);

            while (stack.Count() > 0) {
                var node = stack.Pop();

                if (done.Contains(node.Id)) continue;

                yield return node;

                foreach (var (key, child) in node.Children) {
                    stack.Push(child);
                    done.Add(node.Id);
                }
            }
        }
    }
}