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
    /// Connection definition between two nodes. Used for graph creation.
    /// </summary>
    public class GraphConnection : Tuple<int, int, double>
    {
        public GraphConnection(int node1, int node2, double edgeValue)
            : base(node1, node2, edgeValue)
        {
        }
    }

    /// <summary>
    /// Generic oriented and weighted graph data structure.
    /// </summary>
    /// <typeparam name="T">Type of value</typeparam>
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

        /// <summary>
        /// Graph's Edge class representation (source, target, length).
        /// </summary>
        public class Edge
        {
            private static long IdCounter = 0;

            /// <summary>
            /// Unique id for all edges.
            /// </summary>
            public long Id { get; }

            /// <summary>
            /// Length (weight) of this edge.
            /// </summary>
            public double Length { get; }

            /// <summary>
            /// Source node.
            /// </summary>
            public ReadOnlyNode NodeFrom { get; }

            /// <summary>
            /// Target node.
            /// </summary>
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
            
            /// <summary>
            /// Node's value.
            /// </summary>
            public T Value
            {
                get { return node.Value; }
                set { node.Value = value; }
            }

            /// <summary>
            /// Collection of edges ingoing to this node.
            /// </summary>
            public ReadOnlyCollection<Edge> InEdges => new ReadOnlyCollection<Edge>(node.InEdges);

            /// <summary>
            /// Collection of edges outgoing from this node.
            /// </summary>
            public ReadOnlyCollection<Edge> OutEdges => new ReadOnlyCollection<Edge>(node.OutEdges);

            /// <summary>
            /// Node's distance from starting node. Only if BFS/DFS/Dijkstra/Bellman-Ford algorithm has ran.
            /// </summary>
            public double Distance => node.Distance;

            /// <summary>
            /// Reference to previous (parent) node during BFS/DFS/Dijkstra/Bellman-Ford algorithm.
            /// </summary>
            public ReadOnlyNode? PreviousNode => Create(node.PreviousNode);
            public object ThisNode => node;

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

        /// <summary>
        /// Number of nodes in graph.
        /// </summary>
        public int Count => nodes.Count;

        /// <summary>
        /// If graph has been already searched (traversed).
        /// </summary>
        public bool Traversed { get; private set; } = false;

        /// <summary>
        /// Default empty graph constructor.
        /// </summary>
        public Graph()
        {
        }

        /// <summary>
        /// Create graph from connection matrix and list of node's values.
        /// </summary>
        /// <param name="connectionMatrix">Matrix of edges in graph. Zero value means no edge.</param>
        /// <param name="nodes">List of node's values. Must be sorted according to position in connection matrix.</param>
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

        /// <summary>
        /// Create graph from connection list and list of node's values.
        /// </summary>
        /// <param name="connectionList">List of connections between nodes</param>
        /// <param name="nodes">List of node's values</param>
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

        /// <summary>
        /// Get node at given index.
        /// </summary>
        /// <param name="index">Node's index</param>
        /// <returns>Node at given index</returns>
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

        /// <summary>
        /// Get value from node at given index.
        /// </summary>
        /// <param name="index">Node's index</param>
        /// <returns>Values from node at given index</returns>
        public T ValueAt(int index) => this[index].Value;

        /// <summary>
        /// Return index of given node in graph.
        /// </summary>
        /// <param name="readOnlyNode">Node to find out an index in graph</param>
        /// <returns>Index of given node</returns>
        public int GetIndex(ReadOnlyNode readOnlyNode) => nodes.FindIndex(node => node.Id == (readOnlyNode.ThisNode as Node).Id);

        /// <summary>
        /// Find index of node's value that match given predicate.
        /// </summary>
        /// <param name="predicate">Predicate to match</param>
        /// <returns>Index of node's value that match predicate, otherwise -1 if no node is found</returns>
        public int FindIndex(Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate is null");
            }
            return nodes.FindIndex(node => predicate(node.Value));
        }

        /// <summary>
        /// Find node that it's value match given predicate.
        /// </summary>
        /// <param name="predicate">Predicate to match</param>
        /// <returns>Node that it's value match given predicate, otherwise null if no node is found</returns>
        public ReadOnlyNode? FindNode(Predicate<T> predicate)
        {
            var index = FindIndex(predicate);
            if (index >= 0 && index < Count)
            {
                return this[index];
            }
            return null;
        }

        /// <summary>
        /// Add value to graph and create node for it.
        /// </summary>
        /// <param name="value">Value to add</param>
        /// <returns>Node created for this value</returns>
        public ReadOnlyNode AddNode(T value)
        {
            var node = new Node(value);
            nodes.Add(node);
            return ReadOnlyNode.Create(node).Value;
        }

        /// <summary>
        /// Add edge to graph.
        /// </summary>
        /// <param name="nodeFrom">Index of source node</param>
        /// <param name="nodeTo">Index of target node</param>
        /// <param name="length">Length (weight) of edge</param>
        /// <returns>Edge added to the graph</returns>
        public Edge AddEdge(int nodeFrom, int nodeTo, double length) => AddEdge(this[nodeFrom], this[nodeTo], length);

        /// <summary>
        /// Add edge to graph.
        /// </summary>
        /// <param name="nodeFrom">Source node</param>
        /// <param name="nodeTo">Target node</param>
        /// <param name="length">Length (weight) of node</param>
        /// <returns>Edge added to the graph</returns>
        public Edge AddEdge(ReadOnlyNode nodeFrom, ReadOnlyNode nodeTo, double length)
        {
            var edge = new Edge(length, nodeFrom, nodeTo);
            (nodeFrom.ThisNode as Node).OutEdges.Add(edge);
            (nodeTo.ThisNode as Node).InEdges.Add(edge);
            return edge;
        }
        
        /// <summary>
        /// Remove all nodes and edges from graph.
        /// </summary>
        public void Clear()
        {
            nodes.Clear();
        }

        /// <summary>
        /// Remove node at given index with all it's in/out going edges.
        /// </summary>
        /// <param name="index">Index of node to remove</param>
        public void RemoveNode(int index) => RemoveNode(this[index]);

        /// <summary>
        /// Remove given node from graph with all it's in/out going edges.
        /// </summary>
        /// <param name="readOnlyNode">Node to remove</param>
        public void RemoveNode(ReadOnlyNode readOnlyNode) => RemoveNode(node => (node.ThisNode as Node).Id == (readOnlyNode.ThisNode as Node).Id);

        /// <summary>
        /// Remove first node that match given predicate.
        /// </summary>
        /// <param name="predicate">Predicate to match</param>
        /// <returns>True if any node was removed, false otherwise</returns>
        public bool RemoveNode(Predicate<ReadOnlyNode> predicate)
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
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove all nodes that match given predicate.
        /// </summary>
        /// <param name="predicate">Predicate to match</param>
        public void RemoveAllNodes(Predicate<ReadOnlyNode> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate is null");
            }
            nodes.RemoveAll(node => predicate(ReadOnlyNode.Create(node).Value));
        }

        /// <summary>
        /// Remove edge from graph.
        /// </summary>
        /// <param name="edge">Edge to remove</param>
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

        /// <summary>
        /// Count number connectivity components in graph using BFS.
        /// </summary>
        /// <returns>Number of connectivity components in graph</returns>
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

        /// <summary>
        /// Check whether graph contains a cycle (using DFS).
        /// </summary>
        /// <returns>True whether graph containst cycle, false otherwise</returns>
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

        /// <summary>
        /// Count minimum spanning tree using Jarnik (Prim) algorithm.
        /// </summary>
        /// <returns>List of edges that make minimum spanning tree</returns>
        public List<Edge> JarnikMinimumSpannigTree()
        {
            if (NumberOfConnectivityComponents() > 1)
            {
                throw new InvalidOperationException("Unable to perform Jarnik's algorithm on graph with more than one connectivity component");
            }
            if (Count == 0)
            {
                throw new InvalidOperationException("Unable to perform Jarnik's algorithm on graph with no nodes");
            }
            return JarnikImpl();
        }

        /// <summary>
        /// Run Bellman-Ford algorithm and count minimum distance from given node.
        /// </summary>
        /// <param name="startNode">Starting node</param>
        public void BellmanFord(ReadOnlyNode startNode)
        {
            if (NumberOfConnectivityComponents() > 1)
            {
                throw new InvalidOperationException("Unable to perform BellmanFord on graph with more than one connectivity component");
            }
            PrepareForGraphTraverse();
            BellmanFordImpl(startNode.ThisNode as Node);
        }

        /// <summary>
        /// Run Dijkstra algorithm and count minimum distance from givennode
        /// </summary>
        /// <param name="startNode">Starting node</param>
        public void Dijkstra(ReadOnlyNode startNode)
        {
            PrepareForGraphTraverse();
            DijkstraImpl(startNode.ThisNode as Node, EdgeRelax);
        }

        /// <summary>
        /// Run DFS (Depth first search) algorithm.
        /// </summary>
        /// <param name="startNode">Starting node</param>
        /// <param name="map">Node's mapping function</param>
        public void DFS(ReadOnlyNode startNode, Action<ReadOnlyNode> map)
        {
            PrepareForGraphTraverse();
            var start = startNode.ThisNode as Node;
            start.Distance = 0;
            DFSRecursive(start, null, map);
        }

        /// <summary>
        /// Run BFS (Breadth first search) algorithm.
        /// </summary>
        /// <param name="startNode">Starting node</param>
        /// <param name="map">Node's mapping function</param>
        public void BFS(ReadOnlyNode startNode, Action<ReadOnlyNode> map)
        {
            PrepareForGraphTraverse();
            BFSImpl(startNode, map);
        }

        /// <summary>
        /// Clear traversing statistics (Distance, PreviousNode properties etc.).
        /// </summary>
        public void ClearTraversingStats()
        {
            Traversed = false;
            foreach (var node in nodes)
            {
                node.Color = NodeColor.White;
                node.Distance = double.MaxValue;
                node.PreviousNode = null;
                node.ProgramData = null;
            }
        }

        /// <summary>
        /// Get all values from graph.
        /// </summary>
        /// <returns>Values enumerable</returns>
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
            if ((dynamic)node.Distance + edge.Length < target.Distance)
            {
                target.Distance = (dynamic)node.Distance + edge.Length;
                target.PreviousNode = node;
                return true;
            }
            return false;
        }

        private bool EdgeRelaxJarnik(Node node, Edge edge)
        {
            var target = edge.NodeTo.ThisNode as Node;
            if (edge.Length < target.Distance)
            {
                target.Distance = edge.Length;
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
                    if (node.Distance < double.MaxValue)
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
                if (node.Distance < double.MaxValue)
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

        private void DijkstraImpl(Node start, Func<Node, Edge, bool> relaxationFunc)
        {
            var heap = new BinaryHeap<Node>(CreateNodeDistanceComparer(), CreateBinaryHeapChangeIndexNotifier());
            start.Distance = 0;
            heap.PushRange(nodes);
            while (heap.Count > 0)
            {
                var node = heap.Pop();
                if (node.Distance == double.MaxValue)
                {
                    break; // cannot continue, not accessible
                }
                foreach (var edge in node.OutEdges)
                {
                    if (relaxationFunc.Invoke(node, edge))
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
        
        private List<Edge> JarnikImpl()
        {
            PrepareForGraphTraverse();
            DijkstraImpl(nodes[0], EdgeRelaxJarnik);
            var spanningTree = new List<Edge>();
            foreach (var node in nodes)
            {
                if (node.PreviousNode == null)
                {
                    continue;
                }
                var from = ReadOnlyNode.Create(node.PreviousNode).Value;
                var to = ReadOnlyNode.Create(node).Value;
                spanningTree.Add(new Edge(node.Distance, from, to));
            }
            return spanningTree;
        }

        private static IComparer<Node> CreateNodeDistanceComparer()
        {
            return Comparer<Node>.Create((a, b) => {
                if (a.Distance < b.Distance) return -1;
                if (b.Distance < a.Distance) return 1;
                return 0;
            });
        }

        private static BinaryHeap<Node>.ChangeIndexNotifier CreateBinaryHeapChangeIndexNotifier() =>
            new BinaryHeap<Node>.ChangeIndexNotifier((node, newIndex) => node.ProgramData = newIndex);
    }
}
