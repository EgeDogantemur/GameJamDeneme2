using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoverWithnodesbut : MonoBehaviour
{  // List of nodes (positions) the player can move between// List of nodes (positions) the player can move between/ List of nodes (positions) the player can move between
    public List<Transform> nodes;

    // How fast we move
    public float moveSpeed = 5f;

    private bool isMoving = false;
    private int currentNodeIndex = 0;

    void Start()
    {
        // Ensure the player starts at the first node
        if (nodes.Count > 0)
        {
            transform.position = nodes[0].position;
        }
    }

    void Update()
    {
        // Get input from the old input system
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Determine the direction of movement based on input
        if (horizontal > 0 && !isMoving) // Right
        {
            MoveToNextNode(Vector3.left); // Changed from right to left
        }
        else if (horizontal < 0 && !isMoving) // Left
        {
            MoveToNextNode(Vector3.right); // Changed from left to right
        }
        else if (vertical > 0 && !isMoving) // Up
        {
            MoveToNextNode(Vector3.back); // Changed from up to back
        }
        else if (vertical < 0 && !isMoving) // Down
        {
            MoveToNextNode(Vector3.forward); // Changed from down to forward
        }
    }

    private void MoveToNextNode(Vector3 direction)
    {
        // Calculate the desired position based on the current position and direction
        Vector3 desiredPosition = transform.position + direction * moveSpeed;

        // Check for valid node movement
        int nextNodeIndex = currentNodeIndex;
        float closestDistance = float.MaxValue;

        // Find the closest node in the given direction
        for (int i = 0; i < nodes.Count; i++)
        {
            if (i != currentNodeIndex)
            {
                Vector3 directionToNode = (nodes[i].position - transform.position).normalized;

                // Check if the node is in the desired direction
                if (Vector3.Dot(direction, directionToNode) > 0.9f) // Allow for some margin
                {
                    float distance = Vector3.Distance(transform.position, nodes[i].position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        nextNodeIndex = i;
                    }
                }
            }
        }

        // Ensure the closest node is within a reasonable distance and is a valid move
        if (nextNodeIndex != currentNodeIndex && closestDistance < moveSpeed * 1.5f)
        {
            currentNodeIndex = nextNodeIndex;
            Vector3 destination = nodes[currentNodeIndex].position;
            StartCoroutine(MoveToDestination(destination));
        }
    }

    private IEnumerator MoveToDestination(Vector3 destination)
    {
        isMoving = true;

        // Move towards the destination
        while (Vector3.Distance(transform.position, destination) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = destination;
        isMoving = false;
    }
}
