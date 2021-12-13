using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] footsteps;
    [SerializeField]
    private AudioClip turnInvisible;
    [SerializeField]
    private AudioClip turnVisible;
    [SerializeField]
    private AudioClip hack;
    [SerializeField]
    private AudioClip hackCompleted;
    [SerializeField]
    private AudioClip missionComplete;
    [SerializeField]
    private AudioClip missionFailed;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SoundRandomFootstep() {
        audioSource.PlayOneShot(footsteps[Random.Range(0,footsteps.Length - 1)]);
    }

    public void TurnInvisible()
    {
        audioSource.PlayOneShot(turnInvisible);
    }
    public void TurnVisible()
    {
        audioSource.PlayOneShot(turnVisible);
    }

    public void StartHack() {
        audioSource.PlayOneShot(hack);

    }

    public void StopHack() {
        audioSource.Stop();
    }

    public void HackCompleted()
    {
        audioSource.PlayOneShot(hackCompleted);
    }

    public void MissionComplete() {
        audioSource.PlayOneShot(missionComplete);
    }

    public void MissionFailed() {
        audioSource.PlayOneShot(missionFailed);
    }

}
