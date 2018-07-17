using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace AlgorithmsCollection
{
    /// <summary>
    /// Connection definition between two nodes
    /// </summary>
    public class GraphConnection : Tuple<int, int, double>
    {
        public GraphConnection(int node1, int node2, double edgeValue)
            : base(node1, node2, edgeValue)
        {
        }
    }

    public class Graph<T> // IEnumerable<T>, ICollection<T> not used knowingly
    {
        private enum NodeColor
        {
            White, // Unseen
            Gray, // Seen and not left
            Black // Seen and left
        }
        
        private class Node
        {
            private static long IdCounter = 0;

            public long Id { get; set; }
            public T Value { get; set; }

            public List<Edge> InEdges { get; set; } = new List<Edge>();
            public List<Edge> OutEdges { get; set; } = new List<Edge>();

            // Seach and traversing values
            public double Distance { get; set; } = double.MaxValue;
            public NodeColor Color { get; set; } = NodeColor.White;
            public Node PreviousNode { get; set; } = null;
            public object ProgramData { get; set; } = null;

            public Node(T value)
            {
                unchecked
                {
                    Id = IdCounter++;
                }
                Value = value;
            }
        }

        public class Edge
        {
            private static long IdCounter = 0;

            public long Id { get; }
            public double Length { get; }
            public ReadOnlyNode NodeFrom { get; }
            public ReadOnlyNode NodeTo { get; }

            public Edge(double length, ReadOnlyNode nodeFrom, ReadOnlyNode nodeTo)
            {
                unchecked
                {
                    Id = IdCounter++;
                }
                Length = length;
                NodeFrom = nodeFrom;
                NodeTo = nodeTo;
            }
        }

        /// <summary>
        /// Read only node wrapper to Graph's Node class
        /// </summary>
        public struct ReadOnlyNode
        {
            private Node node;
            
            public T Value
            {
                get { return node.Value; }
                set { node.Value = value; }
            }
            public ReadOnlyCollection<Edge> InEdges => new ReadOnlyCollection<Edge>(node.InEdges);
            public ReadOnlyCollection<Edge> OutEdges => new ReadOnlyCollection<Edge>(node.OutEdges);
            public double Distance => node.Distance;
            public ReadOnlyNode? PreviousNode => Create(node.PreviousNode);
            public object ThisNode => node; // We need to hide Node class

            public static ReadOnlyNode? Create(object node)
            {
                if (node != null)
                {
                    return new ReadOnlyNode { node = (Node)node };
                }
                return null;
            }
        }
        
        private List<Node> nodes = new List<Node>();

        public int Count => nodes.Count;
        public bool Traversed { get; private set; } = false;

        public Graph()
        {
        }

        public Graph(double[,] connectionMatrix, List<T> nodes)
        {
            if (connectionMatrix == null)
            {
                throw new ArgumentNullException("Connection matrix is null");
            }
            if (connectionMatrix.GetLength(0) != connectionMatrix.GetLength(1))
            {
                throw new ArgumentException("Connection matrices height and width are not equal");
            }
            if (nodes == null)
            {
                throw new ArgumentNullException("Nodes are null");
            }
            if (nodes.Count != connectionMatrix.GetLength(0))
            {
                throw new ArgumentException("Nodes are not compatible with connection matrix");
            }
            CreateFromConnectionMatrix(connectionMatrix, nodes);
        }

        public Graph(List<GraphConnection> connectionList, List<T> nodes)
        {
            if (connectionList == null)
            {
                throw new ArgumentNullException("Connection list is null");
            }
            if (nodes == null)
            {
                throw new ArgumentNullException("Nodes are null");
            }
            if (connectionList.Count > 0 && nodes.Count == 0)
            {
                throw new ArgumentException("Cannot create edges on empty graph");
            }
            CreateFromConnectionList(connectionList, nodes);
        }

        public ReadOnlyNode this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException("Index out of range");
                }
                return ReadOnlyNode.Create(nodes[index]).Value;
            }
        }

        public T ValueAt(int index) => this[index].Value;
        public int GetIndex(ReadOnlyNode readOnlyNode) => nodes.FindIndex(node => node.Id == (readOnlyNode.ThisNode as Node).Id);

        public int FindIndex(Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate is null");
            }
            return nodes.FindIndex(node => predicate(node.Value));
        }

        public ReadOnlyNode? FindNode(Predicate<T> predicate)
        {
            var index = FindIndex(predicate);
            if (index >= 0 && index < Count)
            {
                return this[index];
            }
            return null;
        }

        public ReadOnlyNode AddNode(T value)
        {
            var node = new Node(value);
            nodes.Add(node);
            return ReadOnlyNode.Create(node).Value;
        }

        public Edge AddEdge(int nodeFrom, int nodeTo, double length) => AddEdge(this[nodeFrom], this[nodeTo], length);

        public Edge AddEdge(ReadOnlyNode nodeFrom, ReadOnlyNode nodeTo, double length)
        {
            var edge = new Edge(length, nodeFrom, nodeTo);
            (nodeFrom.ThisNode as Node).OutEdges.Add(edge);
            (nodeTo.ThisNode as Node).InEdges.Add(edge);
            return edge;
        }
        
        public void Clear()
        {
            nodes.Clear();
        }

        public void RemoveNode(int index) => RemoveNode(this[index]);
        public void RemoveNode(ReadOnlyNode readOnlyNode) => RemoveNode(node => (node.ThisNode as Node).Id == (readOnlyNode.ThisNode as Node).Id);

        public void RemoveNode(Predicate<ReadOnlyNode> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate is null");
            }
            var index = nodes.FindIndex(node => predicate(ReadOnlyNode.Create(node).Value));
            if (index >= 0 && index < Count)
            {
                RemoveAllEdgesConnectedToNode(nodes[index]);
                nodes.RemoveAt(index);
            }
        }

        public void RemoveAllNodes(Predicate<ReadOnlyNode> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate is null");
            }
            nodes.RemoveAll(node => predicate(ReadOnlyNode.Create(node).Value));
        }
    
        public void RemoveEdge(Edge edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException("Edge is null");
            }
            var nodeFrom = edge.NodeFrom.ThisNode as Node;
            var nodeTo = edge.NodeTo.ThisNode as Node;
            // Search by edge's Id rather by reference...
            var indexFrom = nodeFrom.OutEdges.FindIndex(searchEdge => searchEdge.Id == edge.Id);
            var indexTo = nodeTo.InEdges.FindIndex(searchEdge => searchEdge.Id == edge.Id);
            if (indexFrom < 0 || indexTo < 0)
            {
                throw new InvalidOperationException("Unable to find provided edge in graph. Nodes might be corrupted.");
            }
            nodeFrom.OutEdges.RemoveAt(indexFrom);
            nodeTo.InEdges.RemoveAt(indexTo);
        }

        public int NumberOfConnectivityComponents()
        {
            PrepareForGraphTraverse();
            int componentsCounter = 0;
            foreach (var node in nodes)
            {
                if (node.Color == NodeColor.White)
                {
                    componentsCounter++;
                    BFS(ReadOnlyNode.Create(node).Value, null);
                }
            }
            ClearTraversingStats();
            return componentsCounter;
        }

        public bool ContainsCycle()
        {
            PrepareForGraphTraverse();
            bool cycleDetected = false;
            foreach (var node in nodes)
            {
                if (node.Color == NodeColor.White)
                {
                    DFS(ReadOnlyNode.Create(node).Value, (readOnlyNode) => {
                        cycleDetected = (readOnlyNode.ThisNode as Node).Color == NodeColor.Gray;
                    });
                    if (cycleDetected)
                    {
                        break;
                    }
                }
            }
            ClearTraversingStats();
            return cycleDetected;
        }

        public List<Edge> KruskalMinimalSpanningTree()
        {
            if (NumberOfConnectivityComponents() > 1)
            {
                throw new InvalidOperationException("Unable to perform Kruskal's algorithm on graph with more than one connectivity component");
            }
            int singleTreeCounter = 0;
            foreach (var node in nodes)
            {
                node.ProgramData = singleTreeCounter++;
            }
            return KruskalImpl(GetSortedEdgesByLength());
        }

        public void BellmanFord(ReadOnlyNode startNode)
        {
            if (NumberOfConnectivityComponents() > 1)
            {
                throw new InvalidOperationException("Unable to perform BellmanFord on graph with more than one connectivity component");
            }
            PrepareForGraphTraverse();
            BellmanFordImpl(startNode.ThisNode as Node);
        }

        public void Dijkstra(ReadOnlyNode startNode)
        {
            PrepareForGraphTraverse();
            var comparer = Comparer<Node>.Create((a, b) => {
                if (a.Distance < b.Distance) return -1;
                if (b.Distance < a.Distance) return 1;
                return 0;
            });
            var notifier = new BinaryHeap<Node>.ChangeIndexNotifier((node, newIndex) => {
                // We need to track indices
                node.ProgramData = newIndex;
            });
            DijkstraImpl(new BinaryHeap<Node>(comparer, notifier), startNode.ThisNode as Node);
        }

        public void DFS(ReadOnlyNode startNode, Action<ReadOnlyNode> map)
        {
            PrepareForGraphTraverse();
            var start = startNode.ThisNode as Node;
            start.Distance = 0;
            DFSRecursive(start, null, map);
        }

        public void BFS(ReadOnlyNode startNode, Action<ReadOnlyNode> map)
        {
            PrepareForGraphTraverse();
            BFSImpl(startNode, map);
        }

        public void ClearTraversingStats()
        {
            Traversed = false;
            foreach (var node in nodes)
            {
                node.Color = NodeColor.White;
                node.Distance = long.MaxValue;
                node.PreviousNode = null;
                node.ProgramData = null;
            }
        }

        public IEnumerable<T> Values()
        {
            foreach (var node in nodes)
            {
                yield return node.Value;
            }
        }
        
        private void CreateFromConnectionList(List<GraphConnection> connectionList, List<T> nodes)
        {
            foreach (var nodeValue in nodes)
            {
                AddNode(nodeValue);
            }
            foreach (var connection in connectionList)
            {
                AddEdge(connection.Item1, connection.Item2, connection.Item3);
            }
        }

        private void CreateFromConnectionMatrix(double[,] connectionMatrix, List<T> nodes)
        {
            foreach (var nodeValue in nodes)
            {
                AddNode(nodeValue);
            }
            for (int x = 0; x < connectionMatrix.GetLength(0); x++)
            {
                for (int y = 0; y < connectionMatrix.GetLength(1); y++)
                {
                    if ((double)(dynamic)connectionMatrix[x, y] != 0.0) // No need for NumericUtilities.DoubleCompare
                    {
                        AddEdge(x, y, connectionMatrix[x, y]);
                    }
                }
            }
        }

        private void RemoveAllEdgesConnectedToNode(Node node)
        {
            // Without ToList() we would not be able to remove during foreach cycle
            foreach (var edge in node.OutEdges.ToList())
            {
                RemoveEdge(edge);
            }
            foreach (var edge in node.InEdges.ToList())
            {
                RemoveEdge(edge);
            }
        }

        private bool EdgeRelax(Node node, Edge edge)
        {
            var target = edge.NodeTo.ThisNode as Node;
            if (target.Distance == long.MaxValue || (dynamic)node.Distance + edge.Length < target.Distance)
            {
                target.Distance = (dynamic)node.Distance + edge.Length;
                target.PreviousNode = node;
                return true;
            }
            return false;
        }

        private void PrepareForGraphTraverse()
        {
            if (Traversed)
            {
                ClearTraversingStats();
            }
            Traversed = true;
        }

        private void BellmanFordImpl(Node start)
        {
            start.Distance = 0;
            for (int i = 0; i < Count - 2; i++)
            {
                foreach (var node in nodes)
                {
                    if (node.Distance < long.MaxValue)
                    {
                        foreach (var edge in node.OutEdges)
                        {
                            EdgeRelax(node, edge);
                        }
                    }
                }
            }
            if (TestBellmanFordNegativeCycle())
            {
                throw new InvalidOperationException("Bellman-Ford detected negative cycle.");
            }
        }

        private bool TestBellmanFordNegativeCycle()
        {
            foreach (var node in nodes)
            {
                if (node.Distance < long.MaxValue)
                {
                    foreach (var edge in node.OutEdges)
                    {
                        if (EdgeRelax(node, edge)) // Should not perform on valid graph
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void DijkstraImpl(BinaryHeap<Node> heap, Node start)
        {
            start.Distance = 0;
            heap.PushRange(nodes);
            while (heap.Count > 0)
            {
                var node = heap.Pop();
                if (node.Distance == long.MaxValue)
                {
                    break; // cannot continue, not accessible
                }
                foreach (var edge in node.OutEdges)
                {
                    if (EdgeRelax(node, edge))
                    {
                        var target = edge.NodeTo.ThisNode as Node;
                        heap.ReplacedOnIndex((int)target.ProgramData);
                    }
                }
            }
        }

        private void DFSRecursive(Node node, Node previousNode, Action<ReadOnlyNode> map)
        {
            if (node.Color == NodeColor.White)
            {
                node.PreviousNode = previousNode;
                node.Distance = previousNode != null ? previousNode.Distance + 1 : 0;
            }
            // In case of cycle detection
            map?.Invoke(ReadOnlyNode.Create(node).Value);
            if (node.Color != NodeColor.White)
            {
                return;
            }
            node.Color = NodeColor.Gray;
            foreach (var edge in node.OutEdges)
            {
                DFSRecursive(edge.NodeTo.ThisNode as Node, node, map);
            }
            node.Color = NodeColor.Black;
        }

        private void BFSImpl(ReadOnlyNode startNode, Action<ReadOnlyNode> map)
        {
            var queue = new Queue<Node>();
            var start = startNode.ThisNode as Node;
            start.Distance = 0;
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                map?.Invoke(ReadOnlyNode.Create(node).Value);
                node.Color = NodeColor.Black; // seen and leaving
                foreach (var edge in node.OutEdges)
                {
                    var targetNode = edge.NodeTo.ThisNode as Node;
                    if (targetNode.Color == NodeColor.White)
                    {
                        targetNode.Color = NodeColor.Gray; // seen and has not left yet
                        targetNode.Distance = node.Distance + 1;
                        targetNode.PreviousNode = node;
                        queue.Enqueue(targetNode);
                    }
                }
            }
        }

        private List<Edge> KruskalImpl(List<Edge> edges)
        {
            var spanningTree = new List<Edge>();
            foreach (var edge in edges)
            {
                var node1 = edge.NodeFrom.ThisNode as Node;
                var node2 = edge.NodeTo.ThisNode as Node;
                var id1 = (int)node1.ProgramData;
                var id2 = (int)node2.ProgramData;
                if (id1 != id2) // Two different trees
                {
                    // Join them by first tree's id
                    foreach (var node in nodes)
                    {
                        if ((int)node.ProgramData == id2)
                        {
                            node.ProgramData = id1;
                        }
                    }
                    spanningTree.Add(edge);
                }
            }
            return spanningTree;
        }
        
        private List<Edge> GetSortedEdgesByLength()
        {
            var edges = new List<Edge>();
            foreach (var node in nodes)
            {
                foreach (var edge in node.OutEdges)
                {
                    edges.Add(edge);
                }
            }
            edges.Sort((edge1, edge2) => {
                if ((dynamic)edge1.Length < edge2.Length) return -1;
                return 1;
            });
            return edges;
        }
    }
}
