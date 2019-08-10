using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Builds the graph
/// </summary>
public class GraphBuilder : MonoBehaviour
{
    static Graph<Waypoint> graph;

    /// <summary>
    /// Awake is called before Start
    /// </summary>
    void Awake()
    {
        // add nodes (all waypoints, including start and end) to graph
        graph = new Graph<Waypoint>();
        graph.AddNode(GameObject.FindWithTag("Start").GetComponent<Waypoint>());
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject node in nodes)
        {
            graph.AddNode(node.GetComponent<Waypoint>());
        }
        graph.AddNode(GameObject.FindWithTag("End").GetComponent<Waypoint>());

        // add edges to graph
        foreach (GraphNode<Waypoint> node1 in graph.Nodes)
        {
            Vector2 node1Position = node1.Value.Position;
            foreach (GraphNode<Waypoint> node2 in graph.Nodes)
            {
                Vector2 node2Position = node2.Value.Position;
                if (node1 != node2 && 
                    Mathf.Abs(node2Position.x - node1Position.x) < 3.5f &&
                    Mathf.Abs(node2Position.y - node1Position.y) < 3f)
                {
                    int distance = (int)Mathf.Sqrt(
                        Mathf.Pow(node2Position.x - node1Position.x, 2) +
                        Mathf.Pow(node2Position.y - node1Position.y, 2));
                    graph.AddEdge(node1.Value, node2.Value, distance);
                }
            }
        }
    }

    /// <summary>
    /// Gets the graph
    /// </summary>
    /// <value>graph</value>
    public static Graph<Waypoint> Graph
    {
        get { return graph; }
    }
}
