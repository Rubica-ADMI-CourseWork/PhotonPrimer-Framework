using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles playing the audio for the game.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip tankXplosionFX;
    public AudioClip shellFiringFX;

    public static AudioManager Instance;

    private void Start()
    {
        audioSource.GetComponent<AudioSource>();
    }
    public void PlayTankXplosionFX()
    {
        audioSource.PlayOneShot(tankXplosionFX);
    }

    public void PlayShellFiringFX()
    {
        audioSource.PlayOneShot(shellFiringFX);
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
