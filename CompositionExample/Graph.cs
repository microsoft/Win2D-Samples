using System;
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CompositionExample
{
    public class Graph<T>
    {

        // implementation of an adjacency list for nodes along with node weight
        public List<GraphNode<T>>[] ConnectedNodes;
        public List<int>[] ConnectedNodesWeight;
        private int NodeCount;
        private GraphNode<T> rootNode;
        private Dictionary<string, int> TotalConnectedWeight;

        //private string RenderShape;
        //memory cache for calculating ccw rotation
        //private MemoryCache _cache;
        //private CacheItemPolicy _policy;
        private bool SearchEnabled;

        public Graph(int NodeMax)
        {
            NodeCount = 0;
            ConnectedNodes = new List<GraphNode<T>>[NodeMax];
            ConnectedNodesWeight = new List<int>[NodeMax];
            TotalConnectedWeight = new Dictionary<string, int>();
            SearchEnabled = false;
            //_policy = new CacheItemPolicy();
            //_cache = new MemoryCache("ccwCalculations");

        }
        public List<T> GetValues()
        {
            List<T> values = new List<T>();
            for (int i = 0; i < NodeCount; i++)
            {
                values.Add(ConnectedNodes[i].First().Value);
            }
            return values;
        }

        public IList<GraphNode<T>> QuickSortNodePoints(List<GraphNode<T>> nodes)
        {
            //Comparisons are calculated based on the total weight of connections
            //using recursive quicksort
            List<GraphNode<T>> L1 = new List<GraphNode<T>>();
            List<GraphNode<T>> L2 = new List<GraphNode<T>>();
            List<GraphNode<T>> L3 = new List<GraphNode<T>>();

            if (nodes.Count < 2)
                return nodes;

            else
            {
                GraphNode<T> CheckNode = nodes.First();
                L3.Add(CheckNode);
                for (int i = 1; i < nodes.Count; i++)
                {
                    if (ConnectedNodesWeight[CheckNode.NodeIndex].Sum() > ConnectedNodesWeight[nodes[i].NodeIndex].Sum())
                    {
                        L1.Add(nodes[i]);

                    }
                    else if (ConnectedNodesWeight[CheckNode.NodeIndex].Sum() < ConnectedNodesWeight[nodes[i].NodeIndex].Sum())
                    {
                        L2.Add(nodes[i]);
                    }
                    else
                    {
                        L3.Add(nodes[i]);
                        //if (CheckNode.yPoint < nodes[i].yPoint)
                        //{
                        //    //Smaller yPoint means the the check node is smaller
                        //    //  in an +x and -y coordinate plane quadrant IV
                        //    L2.Add(nodes[i]);

                        //}
                        //else if (CheckNode.yPoint > nodes[i].yPoint)
                        //{
                        //    L1.Add(nodes[i]);
                        //} else
                        //{

                        //}
                    }
                }
                //Sort lower nodes
                L1 = (List<GraphNode<T>>)QuickSortNodePoints(L1);

                //Sort upper nodes
                L2 = (List<GraphNode<T>>)QuickSortNodePoints(L2);


                return L1.Concat(L3).Concat(L2).ToList();
            }
        }



        public IList<GraphNode<T>> GetConvexHull()
        {
            //https://en.wikibooks.org/wiki/Algorithm_Implementation/Geometry/Convex_hull/Monotone_chain
            //Input: a list P of points in the plane.

            //Precondition: There must be at least 3 points.

            //Sort the points of P by x - coordinate(in case of a tie, sort by y - coordinate).

            if (NodeCount < 3)
                return new List<GraphNode<T>>();
            List<GraphNode<T>> nodes = new List<GraphNode<T>>();
            for (int i = 0; i < NodeCount; i++)
            {
                nodes.Add(ConnectedNodes[i].First());
            }

            nodes = (List<GraphNode<T>>)QuickSortNodePoints(nodes);


            //Initialize U and L as empty lists.
            //The lists will hold the vertices of upper and lower hulls respectively.
            List<GraphNode<T>> L = new List<GraphNode<T>>();
            List<GraphNode<T>> U = new List<GraphNode<T>>();


            //for i = 1, 2, ..., n:
            //    while L contains at least two points and the sequence of last two points
            //            of L and the point P[i] does not make a counter - clockwise turn:
            //            remove the last point from L
            //        append P[i] to L
            //L.Add(nodes[0]);
            for (int i = 0; i < nodes.Count; i++)
            {
                while (L.Count >= 2)// && ccw(L[L.Count - 2], L[L.Count - 1], nodes[i]) >= 1)
                {
                    L.RemoveAt(L.Count - 1);
                }
                L.Add(nodes[i]);



            }


            //U.Add(nodes.Last());
            for (int n = nodes.Count; n > 0; n--)
            {
                //for i = n, n - 1, ..., 1:
                //    while U contains at least two points and the sequence of last two points
                //            of U and the point P[i] does not make a counter - clockwise turn:
                //            remove the last point from U
                //        append P[i] to U

                while (U.Count >= 2)// && ccw(U[U.Count - 2], U[U.Count - 1], nodes[n - 1]) >= 1)
                {

                    U.RemoveAt(U.Count - 1);
                }
                U.Add(nodes[n - 1]);


            }

            //Remove the last point of each list(it's the same as the first point of the other list).
            U.RemoveAt(U.Count - 1);
            L.RemoveAt(L.Count - 1);




            //Concatenate L and U to obtain the convex hull of P.
            //Points in the result are listed in counter - clockwise order.
            return L.Concat(U).ToList();

        }

        //public int ccw(GraphNode<T> a, GraphNode<T> b, GraphNode<T> c)
        //{
        //    //https://algs4.cs.princeton.edu/91primitives/
        //    //reconfigured to support perspective of a windows form
        //    // Top left -> X = 0 & Y = 0
        //    var result = _cache.Get($"{a.Name} {b.Name} {c.Name}");
        //    if (result != null)
        //    {
        //        return (int)result;
        //    }
        //    else
        //    {
        //        result = ((b.xPoint - a.xPoint) * (-(c.yPoint) - -(a.yPoint))) - ((c.xPoint - a.xPoint) * (-(b.yPoint) - -(a.yPoint)));
        //        _cache.Set($"{a.Name} {b.Name} {c.Name}", result, _policy);
        //        return (int)result;
        //    }


        //}


        public int AddNode(string Name, T NodeValue, int xPoint = 0, int yPoint = 0)
        {

            //Add a new connected node list of edges
            ConnectedNodes[NodeCount] = new List<GraphNode<T>>();
            ConnectedNodes[NodeCount].Add(new GraphNode<T>(Name, NodeValue, NodeCount, xPoint, yPoint));
            ConnectedNodesWeight[NodeCount] = new List<int>();
            ConnectedNodesWeight[NodeCount].Add(0);

            //default weight of 0 for a node to travel to itself
            if (NodeCount == 0)
                rootNode = ConnectedNodes[NodeCount].First();

            NodeCount += 1;
            return NodeCount - 1;
        }

        public bool ConnectDirectToNode(string originNodeName, string destinationNodeName, int weight = 0)
        {
            //Each link has a weight associated with traveling
            // Weights are stored in parallel to the adjacent node list
            int originIndex = -1;
            int destinationIndex = -1;
            for (int i = 0; i < NodeCount; i++)
            {
                if (ConnectedNodes[i].First().Name == originNodeName)
                {
                    originIndex = i;
                    
                }
                else if (ConnectedNodes[i].First().Name == destinationNodeName)
                {
                    destinationIndex = i;
                }
                if (originIndex > -1 && destinationIndex > -1)
                {
                    ConnectedNodes[originIndex].Add(ConnectedNodes[destinationIndex].First());
                    ConnectedNodesWeight[originIndex].Add(weight);
                    
                    return true;
                }
            }
            return false;
        }

        public GraphNode<T> MostCommonParent(string NodeName, int parentalDepth)
        {
            GraphNode<T> node = BFS(NodeName);
            int CurrentMax = 0;
            GraphNode<T> MostCommonParent = node;

            Dictionary<string, int> ParentNodeCounts = new Dictionary<string, int>();
            for (int i = 1; i < ConnectedNodes[node.NodeIndex].Count; i++)
            {
                GraphNode<T> NodeParent = ConnectedNodes[ConnectedNodes[node.NodeIndex][i].NodeIndex][parentalDepth];
                if (!ParentNodeCounts.ContainsKey(NodeParent.Name))
                {
                    ParentNodeCounts.Add(NodeParent.Name, 1);
                }
                else
                {
                    ParentNodeCounts[NodeParent.Name] += 1;
                }
                if (ParentNodeCounts[NodeParent.Name] > CurrentMax)
                {
                    MostCommonParent = NodeParent;
                    CurrentMax = ParentNodeCounts[NodeParent.Name];
                }
            }
            return MostCommonParent;
        }

        public bool DisconnectToAndFromNode(string originNodeName, string destinationNodeName)
        {
            //Each link has a weight associated with traveling
            // Weights are stored in parallel to the adjacent node list
            GraphNode<T> origNode = BFS(originNodeName);
            GraphNode<T> destNode = BFS(destinationNodeName);
            for (int i = 1; i < ConnectedNodes[origNode.NodeIndex].Count; i++)
            {
                if (ConnectedNodes[origNode.NodeIndex][i].Name == destNode.Name)
                {
                    
                    ConnectedNodes[origNode.NodeIndex].RemoveAt(i);
                    ConnectedNodesWeight[origNode.NodeIndex].RemoveAt(i);
                }
            }
            for (int i = 1; i < ConnectedNodes[destNode.NodeIndex].Count; i++)
            {
                if (ConnectedNodes[destNode.NodeIndex][i].Name == origNode.Name)
                {
                    ConnectedNodes[destNode.NodeIndex].RemoveAt(i);
                    ConnectedNodesWeight[destNode.NodeIndex].RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public bool ConnectDirectToAndFromNode(string originNodeName, string destinationNodeName, int weight)
        {
            if(!SearchEnabled)
                SearchEnabled = true;
            bool connect1 = ConnectDirectToNode(originNodeName, destinationNodeName, weight);
            bool connect2 = ConnectDirectToNode(destinationNodeName, originNodeName, weight);
            if (connect1 || connect2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public GraphNode<T> BFS(string Name)
        {
            if (SearchEnabled)
            {


                //leverage a queue to determine the next connected node to check
                List<GraphNode<T>> queue = new List<GraphNode<T>>();
                queue.Add(rootNode);

                for (int i = 0; i < queue.Count; i++)
                {
                    //Console.WriteLine("Checking on all adjacent Nodes of: " + queue[i].Name);
                    foreach (GraphNode<T> queueNode in ConnectedNodes[queue[i].NodeIndex])
                    {

                        if (queueNode.Name == Name)
                        {
                            //Console.WriteLine("Found " + Name);
                            return queueNode;
                        }
                        else
                        {
                            if (queue[i].Name != queueNode.Name && queue.Contains(queueNode) == false)
                            {
                                //Console.WriteLine(string.Format("Added {0} to queue for adjacent checks", queueNode.Name));
                                queue.Add(queueNode);
                            }
                        }
                    }
                }
                //Nodes are disconnected at this point. Search the rest of the not null queues
                for (int i = 1; i < NodeCount; i++)
                {
                    if (ConnectedNodes[i].First().Name == Name)
                    {
                        return ConnectedNodes[i].First();
                    }
                }
            }
            //Return null if the node matching Name wasn't found
            return null;
        }

        private GraphNode<T> CheckDepth(ref List<GraphNode<T>> visited, GraphNode<T> CHECK, string Name)
        {
            //Recursive, check depth of connected nodes as they are found from the root
            if (visited.Contains(CHECK) == false)
            {
                if (CHECK.Name != Name)
                {
                    //visited.Add(CHECK);
                    foreach (GraphNode<T> graphNode in ConnectedNodes[CHECK.NodeIndex])
                    {

                        if (graphNode.Name != CHECK.Name && visited.Contains(graphNode) == false)
                        {

                            //Console.WriteLine("Checking further depth of " + graphNode.Name);
                            visited.Add(CHECK);
                            GraphNode<T> result = CheckDepth(ref visited, graphNode, Name);
                            if (result != null)
                            {
                                return result;
                            }
                        }
                    }
                }
                else
                {
                    //Console.WriteLine("Found " + CHECK.Name);
                    return CHECK;
                }

            }

            return null;
        }

        public GraphNode<T> DFS(string Name)
        {
            //leveraging the stack of check depth from the nodes connected to the root node
            List<GraphNode<T>> Visited = new List<GraphNode<T>>();

            foreach (GraphNode<T> graphNode in ConnectedNodes[rootNode.NodeIndex])
            {
                if (graphNode.Name == Name)
                {
                    //return matching node as it was directly connected to the root
                    //Console.WriteLine("Found " + Name);
                    return graphNode;
                }

                if (graphNode != rootNode)
                {
                    GraphNode<T> gt = CheckDepth(ref Visited, graphNode, Name);
                    if (gt != null)
                    {
                        return gt;
                    }
                }
                else
                {
                    Visited.Add(graphNode);
                }



            }

            return null;
        }

        public IList<GraphNode<T>> ShortestPath(GraphNode<T> start, GraphNode<T> end)
        {

            List<GraphNode<T>> queue = new List<GraphNode<T>>();
            int[] dist = new int[NodeCount];
            GraphNode<T>[] prev = new GraphNode<T>[NodeCount];
            for (int i = 0; i < NodeCount; i++)
            {
                dist[i] = int.MaxValue;
                prev[i] = null;
                queue.Add(ConnectedNodes[i].First());
            }
            //Start node dist is 0 from itself
            dist[start.NodeIndex] = 0;

            while (queue.Count > 0)
            {
                //Get the next min value to check
                int MinValue = int.MaxValue;
                int MinIndex = 0;
                int QueueMindex = 0;
                for (int i = 0; i < queue.Count; i++)
                {
                    if (dist[i] < MinValue)
                    {
                        MinValue = dist[queue[i].NodeIndex];
                        MinIndex = queue[i].NodeIndex;
                        QueueMindex = i;
                    }

                }

                queue.RemoveAt(QueueMindex);

                for (int i = 1; i < ConnectedNodes[MinIndex].Count; i++)
                {
                    GraphNode<T> v = ConnectedNodes[MinIndex][i];

                    int altDist = dist[MinIndex] + ConnectedNodesWeight[MinIndex][i];
                    if (altDist > 0 && altDist < dist[v.NodeIndex])
                    {
                        //Shorter path found, update table for vertex
                        dist[v.NodeIndex] = altDist;
                        prev[v.NodeIndex] = ConnectedNodes[MinIndex].First();
                    }
                }


            }

            //Path table is found, return list of nodes for shortest travel
            List<GraphNode<T>> ShortestPathList = new List<GraphNode<T>>();

            queue.Add(start);
            for (int i = 0; i < dist.Length; i++)
            {
                if (dist[i] < int.MaxValue)
                    queue.Add(prev[i]);
            }


            ShortestPathList.Add(ConnectedNodes[end.NodeIndex].First());
            while (prev[ShortestPathList.Last().NodeIndex].NodeIndex != start.NodeIndex)
            {
                ShortestPathList.Add(prev[ShortestPathList.Last().NodeIndex]);
            }
            ShortestPathList.Add(start);
            ShortestPathList.Reverse();

            return ShortestPathList;

        }


    }
}