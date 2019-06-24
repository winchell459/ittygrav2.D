using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject AboutPanel;
    public GameObject HowToPlay;

    public GameObject LoadingPanel;

    private bool loadingNextLevel;
    private string loadingLevel;
    private float loadingStart;
    private void Start()
    {
        HowToPlay.SetActive(false);
        AboutPanel.SetActive(false);
        LoadingPanel.SetActive(false);

        if (GameObject.FindGameObjectWithTag("LevelHandler")) Destroy(GameObject.FindGameObjectWithTag("LevelHandler"));
        if (GameObject.FindGameObjectWithTag("AudioSource")) Destroy(GameObject.FindGameObjectWithTag("AudioSource"));
    }

   

    public void LoadLevel(string level)
    {
        HowToPlay.SetActive(false);
        AboutPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        loadingStart = Time.fixedTime;
        
    }

    public void LoadFirstLevel()
    {
        HowToPlay.SetActive(false);
        AboutPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        
        SceneManager.LoadScene(1);
    }

    public void ToggleAboutPanel()
    {
        HowToPlay.SetActive(false);
        AboutPanel.SetActive(!AboutPanel.activeSelf);
    }

    public void ToggleHowToPlay()
    {
        AboutPanel.SetActive(false);
        HowToPlay.SetActive(!HowToPlay.activeSelf);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
