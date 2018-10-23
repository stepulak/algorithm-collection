using System;
using System.Linq;
using System.Collections.Generic;
using AlgorithmsCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphConnectionMatrixNull()
        {
            double[,] matrix = null;
            new Graph<int>(matrix, new List<int>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GraphInvalidConnectionMatrix()
        {
            var matrix = new double[3, 2]
            {
                { 1, 0 },
                { 0, 3 },
                { 0, -2 },
            };
            new Graph<int>(matrix, new List<int>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphConnectionMatrixNodesNull()
        {
            var matrix = new double[1, 1] { { 1 } };
            new Graph<int>(matrix, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GraphConnectionMatrixNodesNotCompatible()
        {
            var matrix = new double[1, 1] { { 1 } };
            new Graph<int>(matrix, new List<int> { 2, 3, 4 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphConnectionListNull()
        {
            List<GraphConnection> connectionList = null;
            new Graph<int>(connectionList, new List<int>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphConnectionListNodesNull()
        {
            new Graph<int>(new List<GraphConnection>(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GraphConnectionListNotCompatible()
        {
            var connectionList = new List<GraphConnection>
            {
                new GraphConnection(0, 1, 50),
                new GraphConnection(1, 1, 0),
            };
            new Graph<int>(connectionList, new List<int>());
        }
        
        [TestMethod]
        public void GraphAddNode()
        {
            var graph = new Graph<int>();
            Assert.AreEqual(graph.AddNode(100).Value, 100);
            Assert.AreEqual(graph.AddNode(42).Value, 42);
            Assert.IsTrue(graph.Values().SequenceEqual(new List<int> { 100, 42 }));
            Assert.AreEqual(graph.Count, 2);
        }

        [TestMethod]
        public void GraphIndexer()
        {
            var graph = new Graph<int>();
            graph.AddNode(100);
            graph.AddNode(42);
            Assert.AreEqual(graph[0].Value, 100);
            Assert.AreEqual(graph.ValueAt(1), 42);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GraphIndexerInvalidLarge()
        {
            var graph = new Graph<int>();
            var node = graph[1];
            node.Value = 100;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GraphIndexerInvalidNegative()
        {
            var graph = new Graph<int>();
            var node = graph[-1];
            node.Value = 100;
        }

        [TestMethod]
        public void GraphGetIndex()
        {
            var graph = new Graph<int>();
            graph.AddNode(42);
            var node13 = graph.AddNode(13);
            graph.AddNode(22);
            var node66 = graph.AddNode(66);
            graph.AddNode(66);
            Assert.AreEqual(graph.GetIndex(node13), 1);
            Assert.AreEqual(graph.GetIndex(node66), 3);
        }

        [TestMethod]
        public void GraphClear()
        {
            var graph = new Graph<float>();
            graph.AddNode(42.5f);
            graph.AddNode(13.3f);
            graph.AddNode(7.4f);
            graph.AddEdge(1, 2, 55);
            graph.AddEdge(0, 2, 60);
            graph.Clear();
            Assert.AreEqual(graph.Count, 0);
            Assert.IsNull(graph.FindNode(value => 0 == Comparer<float>.Default.Compare(value, 42.5f)));
        }

        [TestMethod]
        public void GraphAddEdge()
        {
            var graph = new Graph<int>();
            graph.AddNode(42);
            graph.AddNode(13);
            var edge1 = graph.AddEdge(0, 1, 3);
            Assert.AreEqual(edge1.Length, 3);
            Assert.AreEqual(edge1.NodeFrom.ThisNode, graph[0].ThisNode);
            Assert.AreEqual(edge1.NodeTo.ThisNode, graph[1].ThisNode);
            var node42 = graph[0];
            var node13 = graph[1];
            Assert.AreEqual(node42.OutEdges.Count, 1);
            Assert.AreEqual(node42.InEdges.Count, 0);
            Assert.AreEqual(node13.InEdges.Count, 1);
            Assert.AreEqual(node13.OutEdges.Count, 0);
            Assert.AreEqual(node42.OutEdges[0].Id, edge1.Id);
            Assert.AreEqual(node13.InEdges[0].Id, edge1.Id);
            var edge2 = graph.AddEdge(graph[0], graph[0], 100);
            Assert.AreEqual(edge2.Length, 100);
            Assert.AreEqual(edge2.NodeFrom.ThisNode, edge2.NodeTo.ThisNode);
            Assert.AreNotEqual(edge2.Id, edge1.Id);
            Assert.AreEqual(node42.OutEdges.Count, 2);
            Assert.AreEqual(node42.InEdges.Count, 1);
            Assert.AreEqual(node42.InEdges[0].Id, edge2.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphFindIndexNullPredicate()
        {
            new Graph<int>().FindIndex(null);
        }

        [TestMethod]
        public void GraphFindIndex()
        {
            var graph = new Graph<int>();
            graph.AddNode(42);
            graph.AddNode(13);
            graph.AddNode(100);
            graph.AddNode(13);
            Assert.AreEqual(graph.FindIndex(value => value == 42), 0);
            Assert.AreEqual(graph.FindIndex(value => value == 13), 1);
            Assert.IsTrue(graph.FindIndex(value => value == 66) < 0);
        }

        [TestMethod]
        public void GraphFindNode()
        {
            var graph = new Graph<int>();
            graph.AddNode(42);
            graph.AddNode(13);
            graph.AddNode(100);
            graph.AddNode(13);
            var node1 = graph.FindNode(value => value == 100);
            Assert.IsTrue(node1.HasValue);
            Assert.AreEqual(node1.Value.Value, 100);
            var node2 = graph.FindNode(value => value == 666);
            Assert.IsNull(node2);
        }

        [TestMethod]
        public void GraphRemoveNodeIndex()
        {
            var graph = CreateSampleGraphTestRemoveNode();
            graph.RemoveNode(1);
            TestSampleGraphRemovedNode(graph);
        }

        [TestMethod]
        public void GraphRemoveNodeValue()
        {
            var graph = CreateSampleGraphTestRemoveNode();
            graph.RemoveNode(graph[1]);
            TestSampleGraphRemovedNode(graph);
        }

        [TestMethod]
        public void GraphRemoveNodePredicate()
        {
            var graph = CreateSampleGraphTestRemoveNode();
            Assert.IsTrue(graph.RemoveNode(node => node.Value == 2));
            TestSampleGraphRemovedNode(graph);
        }
        
        private Graph<int> CreateSampleGraphTestRemoveNode()
        {
            var graph = new Graph<int>();
            graph.AddNode(1);
            graph.AddNode(2);
            graph.AddNode(3);
            graph.AddEdge(0, 1, 42);
            graph.AddEdge(1, 2, 55);
            return graph;
        }

        private void TestSampleGraphRemovedNode(Graph<int> graph)
        {
            Assert.AreEqual(graph.Count, 2);
            var node1 = graph[0];
            var node3 = graph[1];
            Assert.AreEqual(node1.Value, 1);
            Assert.AreEqual(node3.Value, 3);
            Assert.AreEqual(node1.InEdges.Count, 0);
            Assert.AreEqual(node1.OutEdges.Count, 0);
            Assert.AreEqual(node3.InEdges.Count, 0);
            Assert.AreEqual(node3.OutEdges.Count, 0);
        }

        [TestMethod]
        public void GraphCreateFromConnectionMatrix()
        {
            TestSampleGraphConstructors(false);
        }

        [TestMethod]
        public void GraphCreateFromConnectionList()
        {
            TestSampleGraphConstructors(true);
        }

        private void TestSampleGraphConstructors(bool useConnectionList)
        {
            Graph<int> graph;
            var nodes = new List<int> { 42, 13, 22 };

            if (useConnectionList)
            {
                var connectionList = new List<GraphConnection>
                {
                    new GraphConnection(0, 1, 1),
                    new GraphConnection(0, 2, 5),
                    new GraphConnection(1, 2, 2),
                    new GraphConnection(2, 1, 3)
                };
                graph = new Graph<int>(connectionList, nodes);
            }
            else
            {
                var matrix = new double[3, 3]
                {
                    { 0, 1, 5 },
                    { 0, 0, 2 },
                    { 0, 3, 0 }
                };
                graph = new Graph<int>(matrix, nodes);
            }
            Assert.IsTrue(graph.Values().SequenceEqual(nodes));
            var node42 = graph.FindNode(value => value == 42).Value;
            var node13 = graph.FindNode(value => value == 13).Value;
            var node22 = graph.FindNode(value => value == 22).Value;
            Assert.AreEqual(node42.OutEdges.Count, 2);
            Assert.AreEqual(node42.InEdges.Count, 0);
            Assert.AreEqual(node13.OutEdges.Count, 1);
            Assert.AreEqual(node13.InEdges.Count, 2);
            Assert.AreEqual(node22.OutEdges.Count, 1);
            Assert.AreEqual(node22.InEdges.Count, 2);
            Assert.AreEqual(node42.OutEdges[0].Length, 1);
            Assert.AreEqual(node42.OutEdges[1].Length, 5);
            Assert.AreEqual(node13.OutEdges[0].Length, 2);
            Assert.AreEqual(node22.OutEdges[0].Length, 3);
            Assert.AreEqual(node13.InEdges[0].Length, 1);
            Assert.AreEqual(node13.InEdges[1].Length, 3);
            Assert.AreEqual(node22.InEdges[0].Length, 5);
            Assert.AreEqual(node22.InEdges[1].Length, 2);
            var edge1 = node42.OutEdges[0];
            var edge5 = node42.OutEdges[1];
            var edge2 = node13.OutEdges[0];
            var edge3 = node22.OutEdges[0];
            Assert.AreEqual(edge1.NodeFrom.ThisNode, node42.ThisNode);
            Assert.AreEqual(edge1.NodeTo.ThisNode, node13.ThisNode);
            Assert.AreEqual(edge5.NodeFrom.ThisNode, node42.ThisNode);
            Assert.AreEqual(edge5.NodeTo.ThisNode, node22.ThisNode);
            Assert.AreEqual(edge2.NodeFrom.ThisNode, node13.ThisNode);
            Assert.AreEqual(edge2.NodeTo.ThisNode, node22.ThisNode);
            Assert.AreEqual(edge3.NodeFrom.ThisNode, node22.ThisNode);
            Assert.AreEqual(edge3.NodeTo.ThisNode, node13.ThisNode);
        }

        [TestMethod]
        public void GraphRemoveEdge()
        {
            var graph = new Graph<int>();
            var node100 = graph.AddNode(100);
            var node200 = graph.AddNode(200);
            var edgeToRemove = graph.AddEdge(0, 1, 42);
            graph.AddEdge(1, 0, 7);
            Assert.AreEqual(graph.Count, 2);
            Assert.AreEqual(node100.OutEdges.Count, 1);
            Assert.AreEqual(node100.InEdges.Count, 1);
            Assert.AreEqual(node200.OutEdges.Count, 1);
            Assert.AreEqual(node200.InEdges.Count, 1);
            graph.RemoveEdge(edgeToRemove);
            Assert.AreEqual(node100.OutEdges.Count, 0);
            Assert.AreEqual(node100.InEdges.Count, 1);
            Assert.AreEqual(node200.InEdges.Count, 0);
            Assert.AreEqual(node200.OutEdges.Count, 1);
        }

        [TestMethod]
        public void GraphBFS()
        {
            var graph = CreateSampleGraphForTraversing();
            var visitedList = new List<Tuple<char, double>>();
            graph.BFS(graph[0], node => visitedList.Add(new Tuple<char, double>(node.Value, node.Distance)));
            Assert.IsTrue(graph.Traversed);
            int index = 0;
            foreach (var nodeName in new List<char> { 'a','b','c','d','e','f' })
            {
                Assert.AreEqual(visitedList[index++].Item1, nodeName);
            }
            Assert.AreEqual(visitedList[0].Item2, 0); // a
            Assert.AreEqual(visitedList[1].Item2, 1); // b
            Assert.AreEqual(visitedList[2].Item2, 1); // c
            Assert.AreEqual(visitedList[3].Item2, 2); // d
            Assert.AreEqual(visitedList[4].Item2, 2); // e
            Assert.AreEqual(visitedList[5].Item2, 3); // f
            Assert.IsFalse(graph[0].PreviousNode.HasValue);
            Assert.AreEqual(graph[1].PreviousNode.Value.ThisNode, graph[0].ThisNode);
            Assert.AreEqual(graph[2].PreviousNode.Value.ThisNode, graph[0].ThisNode);
            Assert.AreEqual(graph[3].PreviousNode.Value.ThisNode, graph[1].ThisNode);
            Assert.AreEqual(graph[4].PreviousNode.Value.ThisNode, graph[1].ThisNode);
            Assert.AreEqual(graph[5].PreviousNode.Value.ThisNode, graph[4].ThisNode);
        }

        [TestMethod]
        public void GraphDFS()
        {
            var graph = CreateSampleGraphForTraversing();
            var visitedList = new List<Tuple<char, double>>();
            graph.DFS(graph[0], node => visitedList.Add(new Tuple<char, double>(node.Value, node.Distance)));
            Assert.IsTrue(graph.Traversed);
            int index = 0;
            foreach (var nodeName in new List<char> { 'a', 'b', 'd', 'e', 'f', 'e', 'c', 'd' })
            {
                Assert.AreEqual(visitedList[index++].Item1, nodeName);
            }
            Assert.AreEqual(visitedList[0].Item2, 0); // a
            Assert.AreEqual(visitedList[1].Item2, 1); // b
            Assert.AreEqual(visitedList[2].Item2, 2); // d
            Assert.AreEqual(visitedList[3].Item2, 3); // e
            Assert.AreEqual(visitedList[4].Item2, 4); // f
            Assert.AreEqual(visitedList[5].Item2, 3); // e
            Assert.AreEqual(visitedList[6].Item2, 1); // c
            Assert.AreEqual(visitedList[7].Item2, 2); // d
            Assert.IsFalse(graph[0].PreviousNode.HasValue);
            Assert.AreEqual(graph[1].PreviousNode.Value.ThisNode, graph[0].ThisNode);
            Assert.AreEqual(graph[2].PreviousNode.Value.ThisNode, graph[0].ThisNode);
            Assert.AreEqual(graph[3].PreviousNode.Value.ThisNode, graph[1].ThisNode);
            Assert.AreEqual(graph[4].PreviousNode.Value.ThisNode, graph[3].ThisNode);
            Assert.AreEqual(graph[5].PreviousNode.Value.ThisNode, graph[4].ThisNode);
        }
        
        [TestMethod]
        public void GraphContainsCycle()
        {
            var graph = new Graph<bool>();
            graph.AddNode(true);
            graph.AddNode(false);
            graph.AddNode(true);
            graph.AddEdge(0, 1, 1);
            graph.AddEdge(1, 2, 0);
            graph.AddEdge(2, 0, -1);
            Assert.IsTrue(graph.ContainsCycle());
            Assert.IsFalse(graph.Traversed);
            graph.RemoveEdge(graph[0].InEdges.FirstOrDefault());
            Assert.IsFalse(graph.ContainsCycle());
            Assert.IsFalse(graph.Traversed);
            Assert.IsFalse(CreateSampleGraphForTraversing().ContainsCycle());
        }

        [TestMethod]
        public void GraphNumberOfConnectivityComponents()
        {
            var graph = new Graph<int>();
            graph.AddNode(1);
            graph.AddNode(2);
            graph.AddNode(3);
            graph.AddNode(4);
            graph.AddEdge(0, 1, 100);
            graph.AddEdge(1, 2, 300);
            graph.AddEdge(2, 0, -5);
            Assert.AreEqual(graph.NumberOfConnectivityComponents(), 2);
            Assert.IsFalse(graph.Traversed);
            Assert.AreEqual(CreateSampleGraphForTraversing().NumberOfConnectivityComponents(), 1);
        }

        [TestMethod]
        public void GraphClearTraversingStats()
        {
            var graph = CreateSampleGraphForTraversing();
            graph.BFS(graph[0], null);
            Assert.IsTrue(graph.Traversed);
            graph.ClearTraversingStats();
            Assert.IsFalse(graph.Traversed);
            for (int i = 0; i < graph.Count; i++)
            {
                Assert.IsFalse(graph[i].PreviousNode.HasValue);
                Assert.AreEqual(graph[i].Distance, double.MaxValue);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void JarnikMinimalSpanningTreeTwoConnectivityComponents()
        {
            var graph = new Graph<int>();
            var node5 = graph.AddNode(5);
            var node10 = graph.AddNode(10);
            var node20 = graph.AddNode(20);
            graph.AddEdge(node5, node10, 5);
            graph.JarnikMinimumSpannigTree();
        }

        [TestMethod]
        public void JarnikMinimalSpanningTree()
        {
            var graph = CreateSampleGraphForTraversing();
            var tree = graph.JarnikMinimumSpannigTree();
            tree.Sort((edge1, edge2) => 
            {
                if (edge1.Length < edge2.Length) return -1;
                else if (edge1.Length > edge2.Length) return 1;
                return 0;
            });
            Assert.AreEqual(tree[0].NodeFrom.Value, 'd');
            Assert.AreEqual(tree[0].NodeTo.Value, 'e');
            /*Assert.AreEqual(tree[1].NodeFrom.Value, 'a');
            Assert.AreEqual(tree[1].NodeTo.Value, 'b');
            Assert.AreEqual(tree[2].NodeFrom.Value, 'a');
            Assert.AreEqual(tree[2].NodeTo.Value, 'c');
            Assert.AreEqual(tree[3].NodeFrom.Value, 'b');
            Assert.AreEqual(tree[3].NodeTo.Value, 'e');
            Assert.AreEqual(tree[4].NodeFrom.Value, 'e');
            Assert.AreEqual(tree[4].NodeTo.Value, 'f');*/
        }

        [TestMethod]
        public void GraphDijkstra1()
        {
            var graph = CreateSampleGraphForTraversing();
            graph.AddEdge(3, 5, 1);
            graph.Dijkstra(graph[0]);
            Assert.IsTrue(graph.Traversed);
            Assert.IsFalse(graph[0].PreviousNode.HasValue);
            Assert.AreEqual(graph[1].PreviousNode.Value.ThisNode, graph[0].ThisNode);
            Assert.AreEqual(graph[2].PreviousNode.Value.ThisNode, graph[0].ThisNode);
            Assert.AreEqual(graph[3].PreviousNode.Value.ThisNode, graph[2].ThisNode);
            Assert.AreEqual(graph[4].PreviousNode.Value.ThisNode, graph[1].ThisNode);
            Assert.AreEqual(graph[5].PreviousNode.Value.ThisNode, graph[3].ThisNode);
            Assert.AreEqual(graph[0].Distance, 0);
            Assert.AreEqual(graph[1].Distance, 1);
            Assert.AreEqual(graph[2].Distance, 2);
            Assert.AreEqual(graph[3].Distance, 7);
            Assert.AreEqual(graph[4].Distance, 4);
            Assert.AreEqual(graph[5].Distance, 8);
        }

        [TestMethod]
        public void GraphDijkstra2()
        {
            var graph = new Graph<int>();
            graph.AddNode(13);
            graph.AddNode(12);
            graph.AddNode(11);
            graph.AddNode(10);
            graph.AddNode(9);
            graph.AddEdge(0, 1, 1);
            graph.AddEdge(0, 3, 2);
            graph.AddEdge(2, 1, 10);
            graph.AddEdge(2, 3, 5);
            graph.AddEdge(4, 3, 5);
            graph.Dijkstra(graph[0]);
            Assert.IsTrue(graph.Traversed);
            Assert.IsFalse(graph[0].PreviousNode.HasValue);
            Assert.AreEqual(graph[1].PreviousNode.Value.ThisNode, graph[0].ThisNode);
            Assert.AreEqual(graph[3].PreviousNode.Value.ThisNode, graph[0].ThisNode);
            Assert.IsFalse(graph[4].PreviousNode.HasValue);
            Assert.IsFalse(graph[2].PreviousNode.HasValue);
            Assert.AreEqual(graph[0].Distance, 0);
            Assert.AreEqual(graph[1].Distance, 1);
            Assert.AreEqual(graph[2].Distance, double.MaxValue);
            Assert.AreEqual(graph[3].Distance, 2);
            Assert.AreEqual(graph[4].Distance, double.MaxValue);
        }

        [TestMethod]
        public void GraphBellmanFord()
        {
            var graph = CreateSampleGraphForTraversing();
            graph.BellmanFord(graph[0]);
            Assert.IsTrue(graph.Traversed);
            Assert.IsFalse(graph[0].PreviousNode.HasValue);
            Assert.AreEqual(graph[1].PreviousNode.Value.ThisNode, graph[0].ThisNode);
            Assert.AreEqual(graph[2].PreviousNode.Value.ThisNode, graph[0].ThisNode);
            Assert.AreEqual(graph[3].PreviousNode.Value.ThisNode, graph[2].ThisNode);
            Assert.AreEqual(graph[4].PreviousNode.Value.ThisNode, graph[1].ThisNode);
            Assert.AreEqual(graph[5].PreviousNode.Value.ThisNode, graph[4].ThisNode);
            Assert.AreEqual(graph[0].Distance, 0);
            Assert.AreEqual(graph[1].Distance, 1);
            Assert.AreEqual(graph[2].Distance, 2);
            Assert.AreEqual(graph[3].Distance, 7);
            Assert.AreEqual(graph[4].Distance, 4);
            Assert.AreEqual(graph[5].Distance, 14);
        }
        
        [TestMethod]
        public void GraphBellmanFordNegativeEdges()
        {
            var graph = new Graph<char>();
            graph.AddNode('a');
            graph.AddNode('b');
            graph.AddNode('c');
            graph.AddNode('d');
            graph.AddEdge(0, 1, 1);
            graph.AddEdge(0, 2, -5);
            graph.AddEdge(2, 3, 6);
            graph.AddEdge(1, 3, 8);
            graph.AddEdge(1, 2, 7);
            graph.AddEdge(2, 1, -6);
            graph.BellmanFord(graph[0]);
            Assert.IsTrue(graph.Traversed);
            Assert.IsFalse(graph[0].PreviousNode.HasValue);
            Assert.AreEqual(graph[1].PreviousNode.Value.ThisNode, graph[2].ThisNode);
            Assert.AreEqual(graph[2].PreviousNode.Value.ThisNode, graph[0].ThisNode);
            Assert.AreEqual(graph[3].PreviousNode.Value.ThisNode, graph[1].ThisNode);
            Assert.AreEqual(graph[0].Distance, 0);
            Assert.AreEqual(graph[1].Distance, -11);
            Assert.AreEqual(graph[2].Distance, -5);
            Assert.AreEqual(graph[3].Distance, -3);
        }

        private Graph<char> CreateSampleGraphForTraversing()
        {
            var graph = new Graph<char>();
            graph.AddNode('a'); // 0
            graph.AddNode('b'); // 1
            graph.AddNode('c'); // 2
            graph.AddNode('d'); // 3
            graph.AddNode('e'); // 4
            graph.AddNode('f'); // 5
            graph.AddEdge(0, 1, 1);
            graph.AddEdge(0, 2, 2);
            graph.AddEdge(1, 3, 7);
            graph.AddEdge(1, 4, 3);
            graph.AddEdge(2, 3, 5);
            graph.AddEdge(3, 4, -2);
            graph.AddEdge(4, 5, 10);
            return graph;
        }
    }
}
