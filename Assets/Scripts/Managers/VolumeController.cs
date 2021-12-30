using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    string volumeParameter;
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private Slider slider;

    private float multiplier = 20;

    // Start is called before the first frame update
    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
    }

    private void HandleSliderValueChanged(float value)
    {
        mixer.SetFloat(volumeParameter, Mathf.Log10(value) * multiplier);
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }


}
