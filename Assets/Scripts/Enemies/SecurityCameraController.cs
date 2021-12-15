using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;

public class SecurityCameraController : MonoBehaviour
{
    [SerializeField]
    private AudioClip sound1;
    [SerializeField]
    private AudioClip sound2;
    [SerializeField]
    private AudioSource focusedAudioSource;
    [SerializeField]
    private AudioClip focusSound;

    private TriggerSensor sensorCone;
    private GameObject Player;
    private SpiderStateController spiderCont;
    private Animator animator;
    [SerializeField]
    private AudioSource audioSource;
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
            //Debug.Log("Me Ve la CAM");
            if (!animator.GetBool("WatchingPlayer"))
            { 
                animator.enabled = false;
            }
            if (!focusedAudioSource.isPlaying)
            {
                focusedAudioSource.PlayOneShot(focusSound);
            }

        }
        else if (sensorCone.GetDetectedByName("Player").Contains(Player) && spiderCont.IsInvisible())
        {
            //Debug.Log("Me ve la CAM pero soy INVISIBLE");
            animator.enabled = true;
            focusedAudioSource.Stop();
            

        }
        else
        {
            //Debug.Log("No te veo :(");
            animator.enabled = true;
            focusedAudioSource.Stop();
            
        }
    }


    public void SpiderOutOfVision()
    {
        spiderCont.IsntSeen();
        //Debug.Log("Ya no me ve :)");

    }

    public void PlaySound1() {
        audioSource.PlayOneShot(sound1);
    }

    public void PlaySound2()
    {
        audioSource.PlayOneShot(sound2);
    }

}
