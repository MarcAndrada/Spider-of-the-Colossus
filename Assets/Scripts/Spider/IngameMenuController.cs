using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject inGameMenu;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inGameMenu.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (Time.timeScale == 1)
            {
                PauseGame();
            }
            else if (Time.timeScale == 0)
            {
                // si la velocidad es 0
                ResumeGame();
            }
        }
    }



    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        inGameMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        inGameMenu.SetActive(false);// que la velocidad del juego regrese a 1
        Cursor.lockState = CursorLockMode.Locked;
        //sound.SaveValues();
    }

    public void GameToMainMenu() {
        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        inGameMenu.SetActive(false);
    }

}

