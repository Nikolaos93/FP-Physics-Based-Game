using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables for player movement
    public GameObject player;
    public Rigidbody playerRb;
    public float speed = 2.0f;
    private float horizontalInput;
    public float forwardInput;

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

    private float leftShift = 2.25f; // Constant amount for instanteinously shifting to the lane on the left
    private float rightShift = 2.25f; // Constant amount for instanteinously shifting to the lane on the right

    public bool isOnGround = true; // Checking whether the player is touching the ground to prevent double jumping (jumping mid-air)
    public float jumpForce; // Force that will be applied when the player triggers jump
    public AudioClip jumpSound; // Sound that will be played once when the player jumps

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>(); // Getting the Rigidbody component from the player object
        playerAs.GetComponent<AudioSource>(); // Getting the AudioSource component from the player object
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

            if (Input.GetKeyDown(KeyCode.Space) && speed < 10 && checkpointReached > 0) 
            {
                speed++; // Incrementing speed if Space is pressed, 1st checkpoint is passed and speed is less than 10m/s
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) && speed > 0 && checkpointReached > 0)
            {
                speed--; // Decrementing speed if LCtrl is pressed, 1st checkpoint is passed and speed is greater than 0m/s
            }

            // Move the player forward
            transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput); // Translating the player forward at constant speed/velocity 

            // Move the player right(left)
            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && player.transform.position.x != -2.25) // Checking whether player wants to move to the left and if the player is not in the left lane
            {
                player.transform.position = new Vector3(player.transform.position.x - leftShift, player.transform.position.y, player.transform.position.z); // Moving the player 1 lane to the left
            }
            if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && player.transform.position.x != 2.25) // Checking whether player wants to move to the right and if the player is not in the right lane
            {
                player.transform.position = new Vector3(player.transform.position.x + rightShift, player.transform.position.y, player.transform.position.z); // Moving the player 1 lane to the right
            }
            if (player.transform.position.x < -2.25) // Monitoring whether an error has occured and player was moved more to the left than the center of the left lane  
            {
                player.transform.position = new Vector3(-2.25f, player.transform.position.y, player.transform.position.z); // Correcting the player's position (back to the center of the left lane)
            }
            if (player.transform.position.x > 2.25) // Monitoring whether an error has occured and player was moved more to the right than the center of the right lane
            {
                player.transform.position = new Vector3(2.25f, player.transform.position.y, player.transform.position.z); // Correcting the player's position (back to the center of the right lane)
            }

            if (Input.GetKeyDown(KeyCode.W) && !playerAs.loop) // Checking whether the player wants to move forward and if the rolling sound is not looping
            {
                playerAs.loop = true; // Setting looping of the rolling sound to true
                playerAs.Play(); // Playing the rolling sound
            }
            else if (Input.GetKeyUp(KeyCode.W) && playerAs.loop) // Checking whether the player wants to stop moving forward and if the rolling sound is looping
            {
                playerAs.Stop(); // Stopping the rolling sound
                playerAs.loop = false; // Setting looping of the rolling sound to false
            }

            // Checking whether the player wants to jump by: checking if the "F" key is pressed, if player is touching the ground, if player is not located in the middle lane
            if (Input.GetKeyDown(KeyCode.F) && isOnGround && (transform.position.x > 0.5 || transform.position.x < -0.5))
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Adding jump force to the player's rigidbody component
                isOnGround = false; // Flagging the player as not on the ground 
                playerAs.PlayOneShot(jumpSound, 1.0f); // Playing the jumping sound
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
    }

    private void playerValues() // This method will display the player's stats on the screen
    {
        velocityText.text = "Velocity: " +(int)speed + "[m/s]"; // Displaying player's velocity on the screen
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

        if (other.CompareTag("Checkpoint")) // If the player collides with an object that has the tag "Checkpoint", then one of the checkpoints has been reached
        {
            checkpointReached++; // Incrementing the counter of checkpoints
            gameManager.CheckpointReached(); // Call the CheckpointReached method from the GameManager script  
            //Debug.Log("Checkpoint Reached: " + checkpointReached);
        }
    }

    private void OnCollisionEnter(Collision collision) // Checking whether the player is touching ground (in order to prevent double jumping)
    {
        if (collision.gameObject.CompareTag("Ground")) // Checking if the player is touching the object with the tag "Ground"
        {
            isOnGround = true; // Setting to true if touching the ground is detected
        }
    }
}
