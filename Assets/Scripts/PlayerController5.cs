using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController5 : MonoBehaviour
{
    // Variables for player movement
    public Rigidbody playerRb;
    public float speed = 2.0f; // Initital speed/velocity of the player
    private float turnSpeed = 25.0f; // Rotation speed of the player
    private float horizontalInput;
    private float forwardInput;

    private GameManager gameManager; // This is the GameManager script that checks if the game is active

    // Variables for tracking player's distance/displacement
    private Vector3 lastPosition;
    public float totalDistance;

    // Variables for displaying player's stats
    public TextMeshProUGUI velocityText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI accelerationText;

    public int checkpointReached = 0; // Counter for number of reached checkpoints in the level
    public AudioSource playerAs; // Reference to the audio source attached to the player

    // Varaiables for checking whether specific tray has the required object on it
    public bool tray1 = false;
    public bool tray2 = false;
    public bool tray3 = false;
    public bool tray4 = false;
    public bool tray5 = false;

    private GameObject gate1; // Gate that opens after the 1st stage is complete 
    private GameObject gate2; // gate that opens after the 2nd stage is complete

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>(); // Getting the Rigidbody component from the player object
        playerAs.GetComponent<AudioSource>(); // Getting the AudioSource component from the player object
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // Finding the GameManager and getting the GameManager script from it
        lastPosition = transform.position; // Set the last position of the player to the starting position
        gate1 = GameObject.Find("Gate1"); // Assigning "Gate 1" game object
        gate2 = GameObject.Find("Gate2"); // Assigning "Gate 2" game object
    }

    // Update is called once per frame
    void Update()
    {

        if (gameManager.isGameActive) // If the game is active, then the player can move
        {
            horizontalInput = Input.GetAxis("Horizontal"); // Getting horizontal input
            forwardInput = Input.GetAxis("Vertical"); // Getting vertical input

            if (Input.GetKeyDown(KeyCode.Space) && speed < 10 && checkpointReached > 0)
            {
                speed++; // Incrementing speed if Space is pressed and speed is less than 10m/s
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) && speed > 0 && checkpointReached > 0)
            {
                speed--; // Decrementing speed if LCtrl is pressed and speed is greater than 0m/s
            }

            // Move the player forward
            transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput); // Translating the player forward at constant speed/velocity

            // Move/rotate the player right(left)
            transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime); // Rotating the player based on turnSpeed variable and horizontal input

            if (Input.GetKeyDown(KeyCode.W) && !playerAs.loop) // Checking whether the player wants to move forward and if the bulldozer sound is not looping
            {
                playerAs.loop = true; // Setting looping of the rolling sound to true
                playerAs.Play(); // Playing the rolling sound
            }
            else if (Input.GetKeyUp(KeyCode.W) && playerAs.loop) // Checking whether the player wants to stop moving forward and if the bulldozer sound is looping
            {
                playerAs.Stop(); // Stopping the rolling sound
                playerAs.loop = false; // Setting looping of the rolling sound to false
            }
            if (Input.GetKeyDown(KeyCode.S) && !playerAs.loop) // Checking whether the player wants to move backwards and if the bulldozer sound is not looping
            {
                playerAs.loop = true; // Setting looping of the rolling sound to true
                playerAs.Play(); // Playing the rolling sound
            }
            else if (Input.GetKeyUp(KeyCode.S) && playerAs.loop) // Checking whether the player wants to stop moving backwards and if the bulldozer sound is looping
            {
                playerAs.Stop(); // Stopping the rolling sound
                playerAs.loop = false; // Setting looping of the rolling sound to false
            }

            float distance = Vector3.Distance(lastPosition, transform.position); // Distance between the last position and the current position
            totalDistance += distance; // Adds the distance to the total distance 
            lastPosition = transform.position; // Updates the last position to the current position
        }

        if (!gameManager.isGameActive) // Checking if the game is not active
        {
            playerAs.Stop(); // Stoping the ambient music if the game is not active
            playerAs.loop = false; // Looping of the ambient music set to false 
        }

        playerValues(); // Calls the playerValues method to display the player's stats

        if (transform.position.y < 0) //Checking whether the player has fallen below the ground level
        {
            gameManager.GameOver(); // If the player falls off the platform, then the game is over
        }

        if (tray1 && tray2 && tray3) // Checking if all 3 trays (1st stage) have appropriate objects on them
        {
            gate1.SetActive(false); // Disabling/opening the 1st gate
        }
        if (tray4 && tray5) // Checking if both trays (2nd stage) have appropriate objects on them
        {
            gate2.SetActive(false); // Disabling/opening the 2nd gate
        }
    }

    private void playerValues() // This method will display the player's stats on the screen
    {
        velocityText.text = "Velocity: " + (int)speed + "[m/s]"; // Displaying player's velocity on the screen
        distanceText.text = "Distance: " + (int)totalDistance + "[m]"; // Displaying distance the player has covered on the screen 
        accelerationText.text = "Acceler: " + 0 + "[m/s^2]"; // Displaying player's acceleration on screen (important to show that there is no acceleration during uniform motion)
    }

    private void OnTriggerEnter(Collider other) // This method will be called when the player collides with another object (e.g. the finish line)
    {
        if (other.CompareTag("Finish")) // If the player collides with an object that has the tag "Finish", then the level is complete
        {
            gameManager.LevelComplete(); // Call the LevelComplete method from the GameManager script
            //transform.position = new Vector3(0, 0.5f, 0); // Reset the player's position to the starting position
            //Debug.Log("Level Finished");
        }

        if (other.CompareTag("Checkpoint")) // If the player collides with an object that has the tag "Checkpoint", then the checkpoint has been reached
        {
            checkpointReached++; // Incrementing the counter of checkpoints
            gameManager.CheckpointReached(); // Call the CheckpointReached method from the GameManager script  
            //Debug.Log("Checkpoint Reached: " + checkpointReached);
        }
    }
}
