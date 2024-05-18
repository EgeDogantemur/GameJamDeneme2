using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    Vector2 m_coordinate;
    public Vector2 Coordinate { get { return Utility.Vector2Round(m_coordinate); } }

    List<Node> m_neighborNodes = new List<Node>();
    public List<Node> NeighborNodes {  get { return m_neighborNodes; } }

    Board m_board;

    public GameObject geometry;
    public GameObject linkPrefab;
    public float scaleTime = 0.3f; //scale animation time
    public iTween.EaseType easeType = iTween.EaseType.easeInExpo;
    public bool autoRun; //does animation gets activated at the start
    public float delay = 1f; //before animation delay
    bool m_isInitialized = false;

    private void Awake()
    {
        m_board = Object.FindObjectOfType<Board>();
        m_coordinate = new Vector2(transform.position.x, transform.position.z);
    }

    void Start()
    {
        if (geometry != null) //hide the mesh by scaling 0
            geometry.transform.localScale = Vector3.zero;

        if (autoRun) //start scale animation
            InitNode();

        if (m_board != null)
            m_neighborNodes = FindNeighbors(m_board.AllNodes);
    }

    public void ShowGeometry()
    {
        if (geometry != null)
        {
            iTween.ScaleTo(geometry, iTween.Hash(
                "time", scaleTime,
                "scale", Vector3.one,
                "easetype", easeType,
                "delay", delay
                ));
        }

    }

    public List<Node> FindNeighbors(List<Node> nodes)
    {
        List<Node> nList = new List<Node>();

        foreach (Vector2 dir in Board.directions)
        {
            Node foundNeighbor = nodes.Find(n => n.Coordinate == m_coordinate + dir);
            if (foundNeighbor != null && !nList.Contains(foundNeighbor))
            {
                nList.Add(foundNeighbor);
            }
        }
        return nList;
    }

    public void InitNode()
    {
        if (!m_isInitialized)
        {
            ShowGeometry();
            InitNeighbors();
            m_isInitialized = true;
        }
    }

    void InitNeighbors()
    {
        StartCoroutine(InitNeighborsRoutine());
    }

    IEnumerator InitNeighborsRoutine()
    {
        yield return new WaitForSeconds(delay);
        foreach(Node n in m_neighborNodes)
        {
            LinkNode(n);
            n.InitNode();
        }
    }

    void LinkNode(Node targetNode)
    {
        if (linkPrefab != null)
        {
            GameObject linkInstance = Instantiate(linkPrefab, transform.position, Quaternion.identity);
            linkInstance.transform.parent = transform;

            Link link = linkInstance.GetComponent<Link>();
            if (link != null)
                link.DrawLink(transform.position, targetNode.transform.position);
        }
    }
}
