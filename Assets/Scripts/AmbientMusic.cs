using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
//using UnityEngine.UIElements;

/* Controls Ambient Music on each level */
public class AmbientMusic : MonoBehaviour
{
    public AudioSource ambientMusic; // Reference to the audio source for ambient/background music
    private GameManager gameManager; // Reference to the GameManager

    public Toggle musicToggle; // Reference to the toggle (in settings menu) for enabling/disabling ambient music
    public Slider ambientMusicSlider; // Reference to the slider (in settings menu) for adjusting the volume of ambient music


    // Start is called before the first frame update
    void Start()
    {
        ambientMusic = GetComponent<AudioSource>(); // Getting the Audio Source component of the Main Camera
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // Finding the GameManager and getting the GameManager script from it


        if (DataManager.Instance.isMusicEnabled) // Checking stored setting/value for toggle in DataManager
        {
            musicToggle.isOn = true; // Enabling it if the stored value is true
        }
        if (!DataManager.Instance.isMusicEnabled) // Checking stored setting/value for toggle in DataManager
        {
            musicToggle.isOn = false; // Disabling it if the stored value is false
        }
         
        ambientMusicSlider.value = DataManager.Instance.ambientMusicVolume; // Assigning the value stored in DataManager to the volume slider
        ambientMusic.volume = DataManager.Instance.ambientMusicVolume; // Assigning the value stored in DataManager to the ambient volume

    }

    // Update is called once per frame
    void Update()
    {
        if (musicToggle.isOn) // checking if the ambient music toggle is on
        {
            DataManager.Instance.isMusicEnabled = true; // if it is storing value "true" in DataManager
            ambientMusic.enabled = true; // enabling the relevant audio source
        }
        if (!musicToggle.isOn) // checking if the ambient music toggle is off
        {
            DataManager.Instance.isMusicEnabled = false; // if it is storing value "false" in DataManager
            ambientMusic.enabled = false; // disabling relevant audio source
        }

        DataManager.Instance.ambientMusicVolume = ambientMusicSlider.value; // storing the value of the volume slider into DataManager
        ambientMusic.volume = ambientMusicSlider.value; // adjusting the value of volume according to the volume slider value


        if (gameManager.isGameActive) // Checking whether the game is active
        {
            ambientMusic.UnPause(); // Play the music if the game is active
        }
        if (!gameManager.isGameActive) // Checks if the game is inactive
        {
            ambientMusic.Pause(); // Pause the music if the game is inactive
        }
    }

}
