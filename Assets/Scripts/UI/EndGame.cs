using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] GameObject gameover;
    [SerializeField] GameObject victory;

    [Header("Audio")]
    [SerializeField] AudioSource fightMusic;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource victorySound;

    private void Awake()
    {
        gameover.SetActive(false);
        victory.SetActive(false);
    }

    private void Start()
    {
        EventManager.current.OnEndGame += ShowGameOver;
        EventManager.current.OnVictory += ShowVictory;
    }

    void ShowGameOver()
    {
        gameover.SetActive(true);
        fightMusic.Stop();
        deathSound.Play();
    }

    void ShowVictory()
    {
        victory.SetActive(true);
        fightMusic.Stop();
        victorySound.Play();
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
