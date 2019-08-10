using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A traveler
/// </summary>
public class Traveler : MonoBehaviour
{
    // events fired by class
    PathFoundEvent pathFoundEvent = new PathFoundEvent();
    PathTraversalCompleteEvent pathTraversalCompleteEvent = new PathTraversalCompleteEvent();

    Rigidbody2D rb2D;
    int speed = 60;
    LinkedList<Waypoint> path;
    GameObject endNode;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
	{
        // Adding invokers to event manager
        EventManager.AddPathFoundInvoker(this);
        EventManager.AddPathTraversalCompleteInvoker(this);

        // Move to start point
        gameObject.transform.position = GameObject.FindWithTag("Start").transform.position;
        path = Search();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        endNode = GameObject.FindWithTag("End");

    }
	
	/// <summary>
	/// Update is called once per frame
	/// </summary>
	void Update()
	{
		
	}

    Vector2 Force()
    {
        Vector2 force = new Vector2(
            path.First.Value.Position.x - transform.position.x,
            path.First.Value.Position.y - transform.position.y);
        return force.normalized;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject == endNode)
        {
            rb2D.velocity = Vector2.zero;
            path.RemoveFirst();
            pathTraversalCompleteEvent.Invoke();
        }
        else if (path.First.Value.Id == coll.gameObject.GetComponent<Waypoint>().Id)
        {
            rb2D.velocity = Vector2.zero;
            path.RemoveFirst();
            rb2D.AddForce(Force() * speed);
        }
 
    }
	
    /// <summary>
    /// Adds the given listener for the PathFoundEvent
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddPathFoundListener(UnityAction<float> listener)
    {
        pathFoundEvent.AddListener(listener);
    }

    /// <summary>
    /// Adds the given listener for the PathTraversalCompleteEvent
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddPathTraversalCompleteListener(UnityAction listener)
    {
        pathTraversalCompleteEvent.AddListener(listener);
    }

    /// <summary>
    /// Find shortest path from start to end
    /// </summary>
    public LinkedList<Waypoint> Search()
    {
        Graph<Waypoint> graph = GraphBuilder.Graph;
        Dictionary<GraphNode<Waypoint>, SearchNode<Waypoint>> searchDict = 
            new Dictionary<GraphNode<Waypoint>, SearchNode<Waypoint>>();
        SortedLinkedList<SearchNode<Waypoint>> searchList = new SortedLinkedList<SearchNode<Waypoint>>();
        GraphNode<Waypoint> startNode = graph.Nodes[0];
        GraphNode<Waypoint> endNode = graph.Nodes[graph.Nodes.Count - 1];

        foreach (GraphNode<Waypoint> graphNode in graph.Nodes)
        {
            SearchNode<Waypoint> searchNode = new SearchNode<Waypoint>(graphNode);
            if (graphNode == startNode)
            {
                searchNode.Distance = 0;
            }
            searchList.Add(searchNode);
            searchDict.Add(graphNode, searchNode);
        }

        while (searchList != null)
        {
            SearchNode<Waypoint> searchNode = searchList.First.Value;
            GraphNode<Waypoint> graphNode = searchNode.GraphNode;
            searchList.RemoveFirst();
            searchDict.Remove(graphNode);
            float distance;

            if (graphNode == endNode)
            {
                LinkedList<Waypoint> path = new LinkedList<Waypoint>();
                path.AddFirst(endNode.Value);
                float cost = searchNode.Distance;
                SearchNode<Waypoint> previous = searchNode.Previous;
                while (previous != null)
                {
                    cost += previous.Distance;
                    path.AddFirst(previous.GraphNode.Value);
                    previous = previous.Previous; 
                }
                pathFoundEvent.Invoke(cost);
                return path;
            }

            foreach (GraphNode<Waypoint> neighbor in graphNode.Neighbors)
            {
                if (searchDict.ContainsKey(neighbor))
                {
                    if (searchList.Contains(searchDict[neighbor]))
                    {
                        distance = graphNode.GetEdgeWeight(neighbor);
                        if (distance < searchDict[neighbor].Distance)
                        {
                            searchDict[neighbor].Distance = distance;
                            searchDict[neighbor].Previous = searchNode;
                            searchList.Reposition(searchDict[neighbor]);
                        }
                    }
                }
            }
        }
        throw new System.Exception("Error");
    }
}
