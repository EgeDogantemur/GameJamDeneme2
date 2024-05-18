using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Board m_board;
    //PlayerManager m_player;

    bool m_hasLevelStarted = false;
    public bool HasLevelStarted { get => m_hasLevelStarted; set => m_hasLevelStarted = value; }

    bool m_isGamePlaying = false;
    public bool IsGamePlaying { get => m_isGamePlaying; set => m_isGamePlaying = value; }

    bool m_isGameOver = false;
    public bool IsGameOver { get => m_isGameOver; set => m_isGameOver = value; }

    bool m_hasLevelFinished = false;
    public bool HasLevelFinished { get => m_hasLevelFinished; set => m_hasLevelFinished = value; }

    public float delay = 1f;

    private void Awake()
    {
        m_board = Object.FindObjectOfType<Board>();
        //m_player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine("RunGameLoop");

        /*if (m_player != null && m_board != null)
        {
            StartCoroutine("RunGameLoop");
        }

        else
        {
            Debug.LogWarning("GAMEMANAGER Error: no player or board found!");
        }*/
    }

    IEnumerator RunGameLoop() 
    {
        yield return StartCoroutine("StartLevelRoutine");
        yield return StartCoroutine("PlayLevelRoutine");
        yield return StartCoroutine("EndLevelRoutine");
    }

    IEnumerator StartLevelRoutine()
    {
        yield return null;
    }

    IEnumerator PlayLevelRoutine()
    {
        yield return null;
    }

    IEnumerator EndLevelRoutine()
    {
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
