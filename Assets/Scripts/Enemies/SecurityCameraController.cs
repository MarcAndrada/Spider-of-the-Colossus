using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;

public class SecurityCameraController : MonoBehaviour
{
    private TriggerSensor sensorCone;
    private GameObject Player;
    private SpiderStateController spiderCont;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        sensorCone = GetComponentInChildren<TriggerSensor>();
        Player = GameObject.FindGameObjectWithTag("Player");
        spiderCont = Player.GetComponent<SpiderStateController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfSeeEnemy();

    }


    private void CheckIfSeeEnemy()
    {

        if (sensorCone.GetDetectedByName("Player").Contains(Player) && !spiderCont.IsInvisible())
        {
            spiderCont.IsSeen();
            Debug.Log("Me Ve la CAM");
            if (!animator.GetBool("WatchingPlayer"))
            { 
                animator.enabled = false;
            }

        }
        else if (sensorCone.GetDetectedByName("Player").Contains(Player) && spiderCont.IsInvisible())
        {
            Debug.Log("Me ve la CAM pero soy INVISIBLE UWU");
            animator.enabled = true;

        }
        else
        {

            Debug.Log("No te veo :(");
            animator.enabled = true;
        }
    }


    public void SpiderOutOfVision()
    {
        spiderCont.IsntSeen();
        Debug.Log("Ya no me ve :)");

    }



}
