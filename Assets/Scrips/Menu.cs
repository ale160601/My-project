using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public AudioSource SFXSource;
    public AudioClip SFXClip;
    public Slider masterSlider, bgmSlider, sfxSlider;
    public AudioMixer mixer;

    private void Update()
    {
        mixer.SetFloat("MasterVolume", masterSlider.value);
        mixer.SetFloat("BGMVolume", bgmSlider.value);
        mixer.SetFloat("SFXVolume", sfxSlider.value);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void PlaySound()
    {
        SFXSource.clip = SFXClip;
        SFXSource.Play();
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Saliste :)");
    }
}
