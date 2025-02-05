using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    // Variables for player movement
    public GameObject player;
    public Rigidbody playerRb;
    public float speed = 7.0f; // Initital speed/velocity of the player (ski-jet)
    private float turnSpeed = 25.0f; // Rotation speed of the player
    private float horizontalInput;
    private float forwardInput;

    public float speedOfRiver = 7.0f; // Initial speed/velocity of the river (it changes/varies)
    private float speedUpstream; // Resulting velocity of the player (result of vector addition of player's velocity and river's velocity)

    private GameManager gameManager; // This is the GameManager script that checks if the game is active

    // Variables for tracking player's distance/displacement
    private Vector3 lastPosition;
    public float totalDistance;

    // Variables for displaying player's stats
    public TextMeshProUGUI velocityText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI speedOfRiverText;

    public ParticleSystem waterSplashParticle; // Reference to the particle system (child object of the player)
    private AudioSource playerAudio; // Reference to the audio source attached to the player

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>(); // Getting the Rigidbody component from the player object
        playerAudio = GetComponent<AudioSource>(); // Getting the AudioSource component from the player object
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // Finding the GameManager and getting the GameManager script from it
        lastPosition = transform.position; // Set the last position of the player to the starting position

        speedUpstream = speed - speedOfRiver; // Calculating the resulting speed when the game starts (intentionally results to 0 at start)
    }

    // Update is called once per frame
    void Update()
    {

        speedUpstream = speed - speedOfRiver; // Continuosly calculating the resulting speed

        if (gameManager.isGameActive) // If the game is active, then the player can move
        {
            horizontalInput = Input.GetAxis("Horizontal"); // Getting horizontal input
            forwardInput = Input.GetAxis("Vertical"); // Getting vertical input

            if (forwardInput < 0) // checking if player is trying to go backwards
            {
                forwardInput = 0; //preventing ski-jet from going backwards
            }
            
            if (player.transform.position.x > 4.5) // preventing the player from going through the right cliff (double check, colliders also exist)
            {
                player.transform.position = new Vector3(4.5f, player.transform.position.y, player.transform.position.z); 
            }
            if (player.transform.position.x < -4.5) // preventing the player from going through the left cliff (double check, colliders also exist)
            {
                player.transform.position = new Vector3(-4.5f, player.transform.position.y, player.transform.position.z);
            }

            if (Input.GetKeyDown(KeyCode.Space) && speed < 10)
            {
                speed++; // Incrementing speed if Space is pressed and speed is less than 10m/s
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) && speed > 0)
            {
                speed--; // Decrementing speed if LCtrl is pressed and speed is greater than 0m/s
            }

            // Move the player forward
            if (transform.position.x > -1 && transform.position.x < 1) // Center/middle "lane" of the river
            {
                speedOfRiver = 7; // Flow of the river is the fastest here because it is in the center
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
                transform.Translate(-Vector3.forward * Time.deltaTime * speedOfRiver, Space.World); // Moving the player in the direction of the river, based on global coordinates (frame of reference)
            }
            else if (transform.position.x > -3 && transform.position.x < 3) // 1st lanes to the left and to the right of the middle lane
            {
                speedOfRiver = 5; // Flow of the river is a bit slower here than in the middle
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
                transform.Translate(-Vector3.forward * Time.deltaTime * speedOfRiver, Space.World); // Moving the player in the direction of the river, based on global coordinates (frame of reference)
            }
            else // Lanes by the river banks
            {
                speedOfRiver = 3; // Flow of the river is the slowest here (next to the river banks)
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
                transform.Translate(-Vector3.forward * Time.deltaTime * speedOfRiver, Space.World); // Moving the player in the direction of the river, based on global coordinates (frame of reference)
            }
            
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) // Checking whether the player wants to move forward
            {
                waterSplashParticle.Play(); // Playing the water splashing particle effect
                playerAudio.loop = true; // Setting looping of the jet-ski sound to true
                playerAudio.Play(); // Playing the jet-ski sound
            }
            else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) // Checking whether the player wants to stop moving forward
            {
                waterSplashParticle.Stop(); // Stoping the water splashing particle effect
                playerAudio.Stop(); // Stopping the jet-ski sound
                playerAudio.loop = false; // Setting looping of the jet-ski sound to false
            }
            

            // Move/rotate the player right(left)
            transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime); // Rotating the player based on turnSpeed variable and horizontal input

            float distance = Vector3.Distance(lastPosition, transform.position); // Distance between the last position and the current position
            totalDistance += distance; // Adds the distance to the total distance 
            lastPosition = transform.position; // Updates the last position to the current position
        }

        if (!gameManager.isGameActive) // Checking if the game is not active
        {
            waterSplashParticle.Stop(); // Stoping the water splashing particle effect
            playerAudio.Stop(); // Stoping the jet-ski sound if the game is not active
            playerAudio.loop = false; // Looping of the jet-ski sound set to false
        }

        playerValues(); // Calls the playerValues method to display the player's stats
    }

    private void playerValues() // This method will display the player's stats on the screen
    {
        velocityText.text = "Velocity: " + (int)speed + "[m/s]"; // Displaying player's velocity on the screen
        distanceText.text = "Distance: " + (int)totalDistance + "[m]"; // Displaying distance the player has covered on the screen 
        speedOfRiverText.text = "River: " + (int)speedOfRiver + "[m/s]"; // Displaying the speed of the river on the screen
    }

    private void OnTriggerEnter(Collider other) // This method will be called when the player collides with another object (e.g. the finish line)
    {
        if (other.CompareTag("Finish")) // If the player collides with an object that has the tag "Finish", then the level is complete
        {
            gameManager.LevelComplete(); // Call the LevelComplete method from the GameManager script
            //transform.position = new Vector3(0, 0.5f, 0); // Reset the player's position to the starting position
            //Debug.Log("Level Finished");
        }
    }

}
