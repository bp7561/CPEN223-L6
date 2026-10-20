// Lab 6
// Student name: Brian Pham
// Student number: 24626509

using System;
using System.Collections.Generic;

Console.WriteLine("CPEN223 Lab 6");

// Example graph from Section 1 of the handout. You may use it in your tests.
Dictionary<string, Dictionary<string, double>> scaffold = new()
{
    ["A"] = new() { ["B"] = 2.0, ["C"] = 5.0 },
    ["B"] = new() { ["A"] = 2.0, ["C"] = 1.0, ["D"] = 4.0 },
    ["C"] = new() { ["A"] = 5.0, ["B"] = 1.0, ["D"] = 1.5 },
    ["D"] = new() { ["B"] = 4.0, ["C"] = 1.5, ["E"] = 3.0 },
    ["E"] = new() { ["D"] = 3.0 },
    ["F"] = new()
};

//Testing: Write test cases that exercise ShortestDistancesFrom
//(and ShortestPath, if you attempt the optional challenge).
//Compare finite distances using a small tolerance, and check unreachable
//vertices with double.IsPositiveInfinity.
//TO DO
Console.WriteLine("Testing ShortestDistancesFrom");
Dictionary<string, double> distances = GraphLibrary.ShortestDistancesFrom(scaffold, "A");
Console.WriteLine();

Console.WriteLine("Distances from A");
Console.WriteLine("A " + distances["A"]);
Console.WriteLine("B " + distances["B"]);
Console.WriteLine("C " + distances["C"]);
Console.WriteLine("D " + distances["D"]);
Console.WriteLine("E " + distances["E"]);
Console.WriteLine("F " + distances["F"]);
Console.WriteLine();

Console.WriteLine("Test Cases:");
Console.WriteLine("A matches 0.0: " + (distances["A"] == 0.0));
Console.WriteLine("B matches 2.0: " + (distances["B"] == 2.0));
Console.WriteLine("C matches 3.0: " + (distances["C"] == 3.0));
Console.WriteLine("D matches 4.5: " + (distances["D"] == 4.5));
Console.WriteLine("E matches 7.5: " + (distances["E"] == 7.5));
Console.WriteLine("F is Infinity: " + double.IsPositiveInfinity(distances["F"]));
Console.WriteLine();

Console.WriteLine("Testing ShortestPath");
List<string> path = GraphLibrary.ShortestPath(scaffold, "A", "E");
Console.WriteLine();

Console.WriteLine("Test Case: A to E");
Console.WriteLine("Expected: A -> B -> C -> D -> E");

Console.WriteLine("Step 0: " + path[0]);
Console.WriteLine("Step 1: " + path[1]);
Console.WriteLine("Step 2: " + path[2]);
Console.WriteLine("Step 3: " + path[3]);
Console.WriteLine("Step 4: " + path[4]);
Console.WriteLine();

Console.WriteLine("Steps from A:");
Console.WriteLine("Step 0 is A: " + (path[0] == "A"));
Console.WriteLine("Step 1 is B: " + (path[1] == "B"));
Console.WriteLine("Step 2 is C: " + (path[2] == "C"));
Console.WriteLine("Step 3 is D: " + (path[3] == "D"));
Console.WriteLine("Step 4 is E: " + (path[4] == "E"));
Console.WriteLine();
//end Testing code

//Do not change the program skeleton: keep the class name, method names,
//parameters, and return types exactly as given.
//Do not use LINQ or recursion, and do not use Console inside the GraphLibrary methods.
//You may add private helper methods.

public static class GraphLibrary
{
    /// <summary>
    /// Computes the minimum total edge cost from <paramref name="source"/> to every
    /// vertex of an undirected weighted graph, using the algorithm in Section 3
    /// of the handout (Dijkstra's algorithm).
    /// </summary>
    /// <param name="graph">
    /// The graph, represented as a nested mapping: each key is a vertex, and its
    /// associated dictionary maps each neighbour to the cost of the connecting edge.
    /// </param>
    /// <param name="source">
    /// The vertex from which distances are measured.
    /// </param>
    /// <returns>
    /// A new dictionary with exactly one entry for every vertex in <paramref name="graph"/>:
    /// <paramref name="source"/> maps to 0.0, each reachable vertex maps to its minimum
    /// total path cost, and each unreachable vertex maps to double.PositiveInfinity.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="source"/> is not a vertex in the graph.
    /// </exception>
    /// <remarks>
    /// Assumes that <paramref name="graph"/> is a valid undirected weighted graph:
    /// every neighbour appearing in an inner dictionary is also a key in the graph;
    /// if v lists w with cost c, then w lists v with the same cost c;
    /// every edge cost is finite and greater than or equal to 0;
    /// and there are no self-loops.
    /// The method does not modify the graph, including the inner dictionaries.
    ///
    /// Implementation requirements: use a <see cref="PriorityQueue{TElement, TPriority}"/>
    /// as the worklist and a <see cref="HashSet{T}"/> to track finalized vertices,
    /// as described in steps 4 and 5. Do not detect stale queue entries by comparing
    /// priorities to stored distances instead.
    /// </remarks>
    public static Dictionary<string, double> ShortestDistancesFrom(
        IReadOnlyDictionary<string, Dictionary<string, double>> graph,
        string source)
    {
		// validation
        if (graph == null || !graph.ContainsKey(source))
        {
            throw new ArgumentException("ERROR: null graph or graph does not contain source key");
        }

        // call helper function to compute Dijkstra and return dictionary of only distances
        var (distances, _) = Dijkstra(graph, source);
		//return distances
        return distances;
    }

	
    // ---------------------------------------------------------------------
    // Optional challenge (Section 7). Leave the NotImplementedException in
    // place if you do not attempt it.
    // ---------------------------------------------------------------------

    /// <summary>
    /// Finds one minimum-cost route from <paramref name="source"/> to
    /// <paramref name="target"/> in an undirected weighted graph.
    /// </summary>
    /// <param name="graph">
    /// The graph, represented as in <see cref="ShortestDistancesFrom"/>.
    /// </param>
    /// <param name="source">The vertex where the route starts.</param>
    /// <param name="target">The vertex where the route ends.</param>
    /// <returns>
    /// The vertices of one minimum-cost route, in order from <paramref name="source"/>
    /// to <paramref name="target"/>, including both endpoints. If several minimum-cost
    /// routes exist, any one is acceptable. If <paramref name="source"/> equals
    /// <paramref name="target"/>, the list contains only <paramref name="source"/>.
    /// If <paramref name="target"/> is unreachable, the list is empty.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="source"/> or <paramref name="target"/> is not a vertex
    /// in the graph.
    /// </exception>
    /// <remarks>
    /// Same assumptions about <paramref name="graph"/> as <see cref="ShortestDistancesFrom"/>.
    /// The method does not modify the graph.
    /// Hint: to avoid duplicating Dijkstra's algorithm, write a private helper that
    /// computes both the distance and predecessor mappings.
    /// </remarks>
    public static List<string> ShortestPath(
        IReadOnlyDictionary<string, Dictionary<string, double>> graph,
        string source,
        string target)
    {
		// validation
        if (graph == null || !graph.ContainsKey(source) || !graph.ContainsKey(target))
        {
            throw new ArgumentException("ERROR: graph is null or graph does not contain source or target key");
        }

        // if the source is the target, return the source list
        if (source == target)
        {
            return new List<string> { source };
        }

        // call function helper to compute Dijkstra
        var (distances, predecessors) = Dijkstra(graph, source);

        // if there is no path connection (infinity), return the empty list
        if (double.IsPositiveInfinity(distances[target]))
        {
            return new List<string>();
        }

        // new Stack to rebuild path with correct forward order
        Stack<string> path = new Stack<string>();
		// current string gets target
        string curr = target;

		// if current string is not equal to null, process
        while (curr != null)
        {
			// push current string to Stack
            path.Push(curr);
			// update current pointer to point to its predecessor
            curr = predecessors[curr];
        }

        // new List to store ordered strings
        List<string> ordered = new List<string>();
		// add strings to List until there are none left
        while (path.Count > 0)
        {
            ordered.Add(path.Pop());
        }

		// return the ordered List
        return ordered;
    }

    /// helper function to compute Dijkstra algorithm
    private static (Dictionary<string, double> Distances, Dictionary<string, string?> Predecessors) Dijkstra(
        IReadOnlyDictionary<string, Dictionary<string, double>> graph,
        string source)
    {
		// new Dictionary to hold minimum distance values from source to any node
        Dictionary<string, double> distances = new Dictionary<string, double>();
		// new Dictionary to hold predecessor node names for currently shortest path, accounts for undiscovered paths as well
        Dictionary<string, string?> predecessors = new Dictionary<string, string?>();
		// new HashSet to hold processed nodes with the minimum distances
        HashSet<string> processed = new HashSet<string>();
		// new Priority Queue for a work queue, will hold node names and the current distance, also puts the shortest distance on top of the queue
        PriorityQueue<string, double> work = new PriorityQueue<string, double>();

        // iterate through each node in graph
        foreach (string node in graph.Keys)
        {
			// initialize shortest distance to infinity
            distances[node] = double.PositiveInfinity;
			// clear predecessor node pointers
            predecessors[node] = null;
        }
		
		// the distance from the source to itself is 0.0
        distances[source] = 0.0;

        // start work queue at source with priority 0.0 
        work.Enqueue(source, 0.0);

        // process nodes from worklist until there are none left
        while (work.Count > 0)
        {
			// process current node at the top of the queue
            string curr = work.Dequeue();

            // if current node has already been processed, skip loop
            if (processed.Contains(curr))
            {
                continue;
            }

            // add current node to HashSet of processed nodes
            processed.Add(curr);
			// uses Dictionary for neighbouring nodes of current node
            Dictionary<string, double> neighbours = graph[curr];

			// iterate through all edges in neighbouring nodes
            foreach (var edge in neighbours)
            {
				// neighbour gets Key and edge cost gets distance value
                string neighbour = edge.Key;
                double edgeCost = edge.Value;

                // if neighbour has already been processed, skip loop
                if (processed.Contains(neighbour))
                {
                    continue;
                }

				// establish a candidate with the shortest distance from current node and its edge cost
                double candidate = distances[curr] + edgeCost;

                // if candidate has shorter distance current shortest neighbour distance, update values
                if (candidate < distances[neighbour])
                {
					// new shortest distance
                    distances[neighbour] = candidate;
					// current node is the predecessor of the neighbour
                    predecessors[neighbour] = curr;
					// enqueue neighbour to work queue with candidate as its priority so it can be evaluated later 
                    work.Enqueue(neighbour, candidate);
                }
            }
        }

		// return dictionaries of distances and predecessors
        return (distances, predecessors);
    }
}
