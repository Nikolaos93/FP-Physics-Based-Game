using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController3 : MonoBehaviour
{
    // Variables for player movement
    public Rigidbody playerRb;
    public float speed = 5.0f; // Initital speed/velocity of the player (ski-jet)
    private float turnSpeed = 25.0f; // Rotation speed of the player
    private float horizontalInput;
    private float forwardInput;

    public float speedOfRiver = 5.0f; // Initial speed/velocity of the river (it changes/varies)
    private float speedUpstream; // Resulting velocity of the player (result of vector addition of player's velocity and river's velocity)

    private GameManager gameManager; // This is the GameManager script that checks if the game is active

    // Variables for tracking player's distance/displacement
    private Vector3 lastPosition;
    public float totalDistance;

    // Variables for displaying player's stats
    public TextMeshProUGUI velocityText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI speedOfRiverText;

    public int checkpointReached = 0; // Counter for number of reached checkpoints in the level

    public ParticleSystem waterSplashParticle; // Reference to the particle system (child component of the player)
    private AudioSource playerAudio; // Reference to the audio source attached to the player

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>(); // Getting the Rigidbody component from the player object
        playerAudio = GetComponent<AudioSource>(); // Getting the AudioSource component from the player object
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // Finding the GameManager and getting the GameManager script from it
        lastPosition = transform.position; // Set the last position of the player to the starting position
    }

    // Update is called once per frame
    void Update()
    {

        if (gameManager.isGameActive) // If the game is active, then the player can move
        {
            horizontalInput = Input.GetAxis("Horizontal"); // Getting horizontal input
            forwardInput = Input.GetAxis("Vertical"); // Getting vertical input
             
            if (forwardInput < 0) // checking if player is trying to go backwards
            {
                forwardInput = 0; //preventing ski-jet from going backwards
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
            if (transform.position.z > 2.5 && transform.position.z < 9.5) // If the player is located in the 1st lane of the 1st river
            {
                speedOfRiver = 3.0f; // Flow of the river is the slowest here (next to the river bank)
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
                transform.Translate(Vector3.left * Time.deltaTime * speedOfRiver, Space.World); // Moving the player in the direction of the river, based on global coordinates (frame of reference)
            }
            else if (transform.position.z > 9.5 && transform.position.z < 16.5) // If the player is located in the lane next to the middle/center lane of 1st the river
            {
                speedOfRiver = 5.0f; // River flows faster here but not as fast as in the middle/center
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
                transform.Translate(Vector3.left * Time.deltaTime * speedOfRiver, Space.World); // Moving the player in the direction of the river, based on global coordinates (frame of reference)
            }
            else if (transform.position.z > 16.5 && transform.position.z < 23.5) // If the player is in the middle/center of the 1st river
            {
                speedOfRiver = 7.0f; // River flows the fastest here
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
                transform.Translate(Vector3.left * Time.deltaTime * speedOfRiver, Space.World); // Moving the player in the direction of the river, based on global coordinates (frame of reference)
            }
            else if (transform.position.z > 23.5 && transform.position.z < 30.5) // If the player is located in the lane next to the middle/center lane of 1st the river
            {
                speedOfRiver = 5.0f; // River flows faster here but not as fast as in the middle/center
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
                transform.Translate(Vector3.left * Time.deltaTime * speedOfRiver, Space.World); // Moving the player in the direction of the river, based on global coordinates (frame of reference)
            }
            else if (transform.position.z > 30.5 && transform.position.z < 37.5) // If the player is located in the last lane of the 1st river
            {
                speedOfRiver = 3.0f; // Flow of the river is the slowest here (next to the river bank)
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
                transform.Translate(Vector3.left * Time.deltaTime * speedOfRiver, Space.World); // Moving the player in the direction of the river, based on global coordinates (frame of reference)
            }
            else if (transform.position.z > 47.5 && transform.position.z < 54.5) // If the player is located in the 1st lane of the 2nd river
            {
                speedOfRiver = 3.0f; // Flow of the river is the slowest here (next to the river bank)
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
                transform.Translate(Vector3.right * Time.deltaTime * speedOfRiver, Space.World); // Moving the player in the direction of the river, based on global coordinates (frame of reference)
            }
            else if (transform.position.z > 54.5 && transform.position.z < 61.5) // If the player is located in the lane next to the middle/center lane of 2nd the river
            {
                speedOfRiver = 5.0f; // River flows faster here but not as fast as in the middle/center
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
                transform.Translate(Vector3.right * Time.deltaTime * speedOfRiver, Space.World); // Moving the player in the direction of the river, based on global coordinates (frame of reference)
            }
            else if (transform.position.z > 61.5 && transform.position.z < 68.5) // If the player is in the middle/center of the 2nd river
            {
                speedOfRiver = 7.0f; // River flows the fastest here
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
                transform.Translate(Vector3.right * Time.deltaTime * speedOfRiver, Space.World); // Moving the player in the direction of the river, based on global coordinates (frame of reference)
            }
            else if (transform.position.z > 68.5 && transform.position.z < 75.5) // If the player is located in the lane next to the middle/center lane of 2nd the river
            {
                speedOfRiver = 5.0f; // River flows faster here but not as fast as in the middle/center
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
                transform.Translate(Vector3.right * Time.deltaTime * speedOfRiver, Space.World); // Moving the player in the direction of the river, based on global coordinates (frame of reference)
            }
            else if (transform.position.z > 75.5 && transform.position.z < 83.5) // If the player is located in the last lane of the 2nd river
            {
                speedOfRiver = 3.0f; // Flow of the river is the slowest here (next to the river bank)
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
                transform.Translate(Vector3.right * Time.deltaTime * speedOfRiver, Space.World); // Moving the player in the direction of the river, based on global coordinates (frame of reference)
            }
            else // Player is in stagnant waters 
            {
                speed = 5; // Default speed for stagnant waters
                transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Moving the player forward, based on local coordinates (frame of reference)
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

        if (other.CompareTag("Checkpoint")) // If the player collides with an object that has the tag "Checkpoint", then the the checkpoint has been reached
        {
            checkpointReached++; // Incrementing the counter of checkpoints
            gameManager.CheckpointReached(); // Call the CheckpointReached method from the GameManager script  
            //Debug.Log("Checkpoint Reached: " + checkpointReached);
        }
    }
}
