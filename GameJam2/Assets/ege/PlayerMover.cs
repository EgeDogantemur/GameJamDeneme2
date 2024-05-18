using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    
    public List<Transform> nodes;

    
    public float moveSpeed = 1.5f;

    public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

    public float iTweenDelay = 0f;

    private bool isMoving = false;
    private Coroutine moveCoroutine;

    private int currentNodeIndex = 0;

    void Start()
    {
        if (nodes.Count > 0)
        {
            transform.position = nodes[0].position;
        }
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        
        if (horizontal > 0 && !isMoving) // Right
        {
            MoveToNextNode(Vector3.right);
        }
        else if (horizontal < 0 && !isMoving) // Left
        {
            MoveToNextNode(Vector3.left);
        }
        else if (vertical > 0 && !isMoving) // Up
        {
            MoveToNextNode(Vector3.forward);
        }
        else if (vertical < 0 && !isMoving) // Down
        {
            MoveToNextNode(Vector3.back);
        }
    }

    private void MoveToNextNode(Vector3 direction)
    {
        int nextNodeIndex = FindNextNodeIndex(direction);

        if (nextNodeIndex != -1)
        {
            currentNodeIndex = nextNodeIndex;
            Vector3 destination = nodes[currentNodeIndex].position;
            Move(destination);
        }
    }

    private int FindNextNodeIndex(Vector3 direction)
    {
        Vector3 desiredPosition = transform.position + direction * moveSpeed;

        float closestDistance = float.MaxValue;
        int closestNodeIndex = -1;

        for (int i = 0; i < nodes.Count; i++)
        {
            float distance = Vector3.Distance(desiredPosition, nodes[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNodeIndex = i;
            }
        }

        
        if (closestDistance < moveSpeed)
        {
            return closestNodeIndex;
        }

        return -1;
    }

    
    public void Move(Vector3 destinationPos, float delayTime = 0.25f)
    {
        
        if (isMoving && moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            iTween.Stop(gameObject);
        }

        moveCoroutine = StartCoroutine(MoveRoutine(destinationPos, delayTime));
    }

    
    IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
    {
        // We are moving
        isMoving = true;

        yield return new WaitForSeconds(delayTime);

        iTween.MoveTo(gameObject, iTween.Hash(
            "position", destinationPos,
            "delay", iTweenDelay,
            "easetype", easeType,
            "speed", moveSpeed,
            "oncomplete", "OnMoveComplete",
            "oncompletetarget", gameObject
        ));

        while (Vector3.Distance(destinationPos, transform.position) > 0.01f)
        {
            yield return null;
        }

        iTween.Stop(gameObject);

        transform.position = destinationPos;

        
        isMoving = false;
    }

   
    void OnMoveComplete()
    {
        // We are not moving
        isMoving = false;
    }
}