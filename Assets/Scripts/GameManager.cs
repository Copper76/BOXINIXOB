using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int latestLevel;
    public GameObject continueButton;
    public GameObject levelButton;
    public GameObject mainMenu;
    public GameObject levelMenu;

    private Scene nextScene;
    private AsyncOperation asyncLoad;
    private AsyncOperation asyncUnload;
    private float lastRestartTick;

    // Start is called before the first frame update
    void Start()
    {
        lastRestartTick = Time.timeSinceLevelLoad;
        latestLevel = PlayerPrefs.GetInt("latestLevel",0);
        if (latestLevel < 1)
        {
            continueButton.SetActive(false);
            levelButton.SetActive(false);
        }
    }

    public void Restart()
    {
        
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        StartCoroutine(NextLevel(SceneManager.GetActiveScene().name));
        asyncLoad.allowSceneActivation = true;
        
    }

    public void toggleMenu()
    {
        mainMenu.SetActive(!mainMenu.activeInHierarchy);
        levelMenu.SetActive(!levelMenu.activeInHierarchy);
    }

    public void turnPage()
    {

    }

    public void ResetLevels()
    {
        PlayerPrefs.SetInt("latestLevel", 0);
        continueButton.SetActive(false);
        levelButton.SetActive(false);
    }

    public void LoadLatestLevel()
    {
        StartCoroutine(NextLevel("Level"+latestLevel));
        asyncLoad.allowSceneActivation = true;
    }

    public void LoadLevelWritten(TextMeshProUGUI levelText)
    {
        StartCoroutine(NextLevel(levelText.text));
        asyncLoad.allowSceneActivation = true;
    }

    public void LoadLevel(string nextSceneName)
    {
        StartCoroutine(NextLevel(nextSceneName));
        asyncLoad.allowSceneActivation = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator NextLevel(string nextSceneName)
    {
        nextScene = SceneManager.GetSceneByName(nextSceneName);

        asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);

        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log("Loading scene " + " [][] Progress: " + asyncLoad.progress);
            yield return null;
        }
    }
}
