using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Start() and Update() methods deleted

    public static DataManager Instance;

    public int scoreOverall;
    public int maxLives;
    public int livesLeft;
    public bool isMusicEnabled;
    public bool isSfxEnabled;
    public float ambientMusicVolume;
    public float sfxVolume;
    public bool dayNight;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
