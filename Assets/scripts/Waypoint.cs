using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A waypoint
/// </summary>
public class Waypoint : MonoBehaviour
{
    [SerializeField]
    int id;

    [SerializeField]
    GameObject explosion;

    SpriteRenderer spriteRenderer;

    /// <summary>
    /// Start is called at the start of the game
    /// </summary>
    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        EventManager.AddPathTraversalCompleteListener(DestroyWaypoint);
    }

    /// <summary>
    /// Changes waypoint to green
    /// </summary>
    /// <param name="other">other collider</param>
    void OnTriggerEnter2D(Collider2D _)
    {
        if (gameObject.tag != "Start" && gameObject.tag != "End")
        {
            spriteRenderer.color = Color.green;
        }
    }

    public void DestroyWaypoint()
    {
        if (spriteRenderer.color == Color.green)
        {
            Instantiate<GameObject>(explosion);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Gets the position of the waypoint
    /// </summary>
    /// <value>position</value>
    public Vector2 Position
    {
        get { return transform.position; }
    }

    /// <summary>
    /// Gets the unique id for the waypoint
    /// </summary>
    /// <value>unique id</value>
    public int Id
    {
        get { return id; }
    }
}
