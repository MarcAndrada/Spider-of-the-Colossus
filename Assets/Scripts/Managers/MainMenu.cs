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

    public void goServerRoom()
    {
        TransitionController.ChangeScene();
        getDark = true;
        nextScene = "ServerRoom";
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

        Application.Quit();
    }


}
