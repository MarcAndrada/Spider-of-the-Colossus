using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerController : MonoBehaviour
{
    private float timetoWait = 3000;
    private float timeWaited;
    private int MaxTimes = 1;
    private int CurrentTimes = 0;
    private bool IsStart = true;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        float delta = Time.deltaTime * 1000;

        timeWaited += delta;

        if (timeWaited > timetoWait && IsStart)
        {
            if (MaxTimes > CurrentTimes)
            {
                TransitionController.ChangeScene();
                CurrentTimes++;
            }
            if (timeWaited >= 4500)
            {
                SceneManager.LoadScene("MainMenu");
                IsStart = false;

            }

        }

    }
}
