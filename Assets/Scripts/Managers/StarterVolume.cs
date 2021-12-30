using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class StarterVolume : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private string masterParameter;
    [SerializeField]
    private string musicParameter;
    [SerializeField]
    private string sfxParameter;
    [SerializeField]
    private float multiplier;

    private float masterValue;
    private float musicVolume;
    private float sfxVolume;

    // Start is called before the first frame update
    void Start()
    {

        masterValue = PlayerPrefs.GetFloat(masterParameter);
        musicVolume = PlayerPrefs.GetFloat(musicParameter);
        sfxVolume = PlayerPrefs.GetFloat(sfxParameter);

        mixer.SetFloat(masterParameter, Mathf.Log10(masterValue) * multiplier);
        mixer.SetFloat(musicParameter, Mathf.Log10(musicVolume) * multiplier);
        mixer.SetFloat(sfxParameter, Mathf.Log10(sfxVolume) * multiplier);


    }

    // Update is called once per frame

}
