using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{ 
    private bool getDark = false;
    private float TimeToWait = 1500;
    private float TimeWaited;
    private string nextScene;

    private void Update()
    {
        float delta = Time.deltaTime * 1000;

        if (getDark)
        {
            TimeWaited += delta;
        }

        if (TimeWaited >= TimeToWait)
        {
            SceneManager.LoadScene(nextScene);
            getDark = false;
            TimeWaited = 0;

        }

    }

    public void goLevel1()
    {
        TransitionController.ChangeScene();
        getDark = true;
        nextScene = "Level1";
        TransitionController.ActiveLoadIcon();
    }

    public void goLevel2()
    {
        TransitionController.ChangeScene();
        getDark = true;
        nextScene = "Level2";
        TransitionController.ActiveLoadIcon();
    }

    public void goTutorial(){
        TransitionController.ChangeScene();
        getDark = true;
        nextScene = "Tutorial";
        TransitionController.ActiveLoadIcon();

    }

    public void goMainMenu()
    {
        Time.timeScale = 1;
        TransitionController.ChangeScene();
        getDark = true;
        nextScene = "MainMenu";
        TransitionController.ActiveLoadIcon();
    }

    public void goCredits()
    {
        TransitionController.ChangeScene();
        getDark = true;
        nextScene = "WinScene";
        TransitionController.ActiveLoadIcon();
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
        		Application.Quit();
        #endif
    }

    public  IEnumerator MissionComplete()
    {
        
        Time.timeScale = 1;
        TransitionController.ActiveLoadIcon();
        yield return new WaitForSeconds(2f);
        TransitionController.ChangeScene();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        getDark = true;

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            nextScene = "Level1";
        }
        else if (SceneManager.GetActiveScene().name == "Level1")
        {
            nextScene = "Level2";
        }
        else
        {
            nextScene = "MainMenu";
        }


    } 
}
