using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource startSound;
    [SerializeField] GameObject commandPanel;

    private void Awake()
    {
        commandPanel.SetActive(false);
    }

    private void Start()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {                
        StartCoroutine(LaunchLoad());
    }

    public IEnumerator LaunchLoad()
    {
        music.Stop();
        startSound.Play();
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ActivatePanel()
    {
        commandPanel.SetActive(!commandPanel.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
