using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Manages the sound that should be played when the player hits the bomb or collects a star/diamond */
public class CollisionSound : MonoBehaviour
{

    public AudioClip AudioClip1; // Reference to the explosion sound
    public AudioClip AudioClip2; // Reference to the pickup sound

    public AudioSource collisionAudio; // Reference to the audio source of CollisionSoundManager
    public AudioSource playerSound; // Reference to the audio source of the player

    public Toggle sfxToggle; // Reference to the toggle (in settings menu) for enabling/disabling SFX
    public Slider sfxSlider; // Reference to the slide (in settings menu) for adjusting the volume of SFX

    // Start is called before the first frame update
    void Start()
    {
        collisionAudio = GetComponent<AudioSource>(); // Reference to the Audio Source component of CollisionSoundManager Game Object

        playerSound = GameObject.Find("Player").GetComponent<AudioSource>(); // Finding the Player and its audio source component

        if (DataManager.Instance.isSfxEnabled) // Checking stored setting/value for toggle in DataManager
        {
            sfxToggle.isOn = true; // Enabling it if the stored value is true
        }
        if (!DataManager.Instance.isSfxEnabled) // Checking stored setting/value for toggle in DataManager
        {
            sfxToggle.isOn = false; // Disabling it if the stored value is false
        }

        sfxSlider.value = DataManager.Instance.sfxVolume; // Assigning the value stored in DataManager to the SFX slider
        collisionAudio.volume = DataManager.Instance.sfxVolume; // Assigning the value stored in DataManager to the collision volume
        playerSound.volume = DataManager.Instance.sfxVolume; // Assigning the value stored in DataManager to the player volume
    }

    // Update is called once per frame
    void Update()
    {
        if (sfxToggle.isOn) // checking if the SFX toggle is on
        {
            DataManager.Instance.isSfxEnabled = true; // if it is storing value "true" in DataManager
            collisionAudio.enabled = true; // enabling the relevant audio source
            playerSound.enabled = true; // enabling the relevant audio source
        }
        if (!sfxToggle.isOn) // checking if the SFX toggle is off
        {
            DataManager.Instance.isSfxEnabled = false; // if it is storing value "false" in DataManager
            collisionAudio.enabled = false; // disabling relevant audio source
            playerSound.enabled = false; // disabling relevant audio source
        }

        DataManager.Instance.sfxVolume = sfxSlider.value; // storing the value of the SFX slider into DataManager
        collisionAudio.volume = sfxSlider.value; // adjusting the value of collision volume according to the sfx volume slider value
        playerSound.volume = sfxSlider.value; // adjusting the value of player volume according to the sfx volume slider value
    }

    public void PlayExplosionSound() // Method that will me called when the player collides with the object with "Bomb" tag 
    {
        collisionAudio.PlayOneShot(AudioClip1, 1.0f); // Play the explosion sound when the player hits the bomb
    }
    public void PlayCollectableSound() // Method that will me called when the player collides with the object with these tags: "Star, "Gem"
    {
        collisionAudio.PlayOneShot(AudioClip2, 1.0f); // Play the pickup sound when the player collects a collectable
    }
}
