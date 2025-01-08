using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;

    private BackgroundMusic() { }

    private void Awake()
    {
        initSingleton();

        _audioSource.loop = true;
        _audioSource.Play();
    }

    private void initSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
