using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    public event Action OnEndGame;
    public event Action OnVictory;
    public static event Action onWinScore;
    [SerializeField] GameObject tuto;
    [SerializeField] float tutoDuration = 5;

    private void Awake()
    {
        current = this;
        tuto.SetActive(true);
        Time.timeScale = 0;
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        OnEndGame?.Invoke();
    }

    public void GameWin()
    {
        OnVictory?.Invoke();
    }

    public void AddScore()
    {
        onWinScore?.Invoke();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }

        if (tuto.activeSelf && Input.anyKey)
        {
            tuto.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
