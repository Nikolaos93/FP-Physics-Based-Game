using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController6 : MonoBehaviour
{
    // Variables for player movement
    public Rigidbody playerRb;
    public float speed = 5.0f;// Initital speed/velocity of the player
    private float horizontalInput;
    private float forwardInput;

    private GameManager gameManager; // Reference to the GameManager script that checks if the game is active

    // Variables for tracking player's distance/displacement
    private Vector3 lastPosition;
    public float totalDistance;

    // Variables for displaying player's stats
    public TextMeshProUGUI velocityText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI accelerationText;

    public AudioSource playerAs; // Reference to the audio source attached to the player
    public bool isOnPlatform; // flag for player touching the platform/ground

    private GameObject focalPoint; // focal point located at the center of the platform

    public GameObject[] collectables; // Array of collectables (stars and diamonds)
    public Transform[] spawningPoints; // Array of spawning points for collectables
    public int collectableCount; // Counter for number of collctables

    private Bomb bCollectable; // refference to Bomb.cs where number of collected pickups is tracked

    private int spawningPointCounter; // Counter for spawning points


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>(); // Getting the Rigidbody component from the player object
        playerAs.GetComponent<AudioSource>(); // Getting the AudioSource component from the player object
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // Finding the GameManager and getting the GameManager script from it
        lastPosition = transform.position; // Set the last position of the player to the starting position

        focalPoint = GameObject.Find("Focal Point"); // Finding the Focal Point in the scene

        collectableCount = 0; // Initial number of collectables is 0

        spawningPointCounter = 0; // Initial spawning point counter is 0
    }

    // Update is called once per frame
    void Update()
    {

        if (gameManager.isGameActive) // If the game is active, then the player can move
        {
            forwardInput = Input.GetAxis("Vertical"); // Getting horizontal input
            horizontalInput = Input.GetAxis("Horizontal"); // Getting vertical input

            // Moving the player 
            playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput); // Adding force to the player's rigid body component based on vertical input
            playerRb.AddForce(focalPoint.transform.right * speed * horizontalInput); // Adding force to the player's rigid body component based on horizontal input

            playerRb.constraints = RigidbodyConstraints.None; // Removing all constraints on rigidbody if the game is active


            float distance = Vector3.Distance(lastPosition, transform.position); // Distance between the last position and the current position
            totalDistance += distance; // Adds the distance to the total distance 
            lastPosition = transform.position; // Updates the last position to the current position
        }

        if (!gameManager.isGameActive) // Checking if the game is not active in order to freeze the rigidbody
        {
            playerRb.constraints = RigidbodyConstraints.FreezeAll;
        }

        playerValues(); // Calls the playerValues method to display the player's stats


        if (transform.position.y < -5) //Checking whether the player has fallen of the platform
        {
            gameManager.GameOver(); // If the player falls off the platform, then the game is over
        }

    }

    private void playerValues() // This method will display the player's stats (e.g. velocity, distance, acceleration...) on the screen
    { 
        velocityText.text = "Velocity: " + (int)speed + "[m/s]"; // Displaying player's velocity on the screen
        distanceText.text = "Distance: " + (int)totalDistance + "[m]"; // Displaying distance the player has covered on the screen 
        accelerationText.text = "Acceler: " + (int)2 + "[m/s^2]"; // Displaying player's acceleration on screen (important to show that there is no acceleration during uniform motion)
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
            //checkpointReached++;
            gameManager.CheckpointReached(); // Call the LevelComplete method from the GameManager script  
            //Debug.Log("Checkpoint Reached: " + checkpointReached);
        }
    }

    private void OnCollisionEnter(Collision collision) //Checking whether the player has collided with something (collectable on this level)
    {
        if (collision.gameObject.CompareTag("Gem") && gameObject.CompareTag("Player")) // Checking if the player has collided with the gem/diamond
        {
            //GameObject collectable = Instantiate(collectables[UnityEngine.Random.Range(0, collectables.Length)], spawningPoints[UnityEngine.Random.Range(0, spawningPoints.Length)]); // if it is spawn a new random collectable at a random spawning point
            GameObject collectable = Instantiate(collectables[UnityEngine.Random.Range(0, collectables.Length)], spawningPoints[spawningPointCounter%6]);
            spawningPointCounter++;
        }
        if (collision.gameObject.CompareTag("Star") && gameObject.CompareTag("Player")) // Checking if the player has collided with the star
        {
            //GameObject collectable = Instantiate(collectables[UnityEngine.Random.Range(0, collectables.Length)], spawningPoints[UnityEngine.Random.Range(0, spawningPoints.Length)]); // if it is spawn a new random collectable at a random spawning point
            GameObject collectable = Instantiate(collectables[UnityEngine.Random.Range(0, collectables.Length)], spawningPoints[spawningPointCounter%6]);
            spawningPointCounter++;
        }
    }

    private void OnCollisionStay(Collision collision) // Checking if the player is touching the paltform
    {
        if (collision.gameObject.CompareTag("Platform") && gameObject.CompareTag("Player"))
        {
            playerAs.UnPause(); // Playing the rolling sound
        }
    }
    private void OnCollisionExit(Collision collision) // Checcking if the player has stopped touching the platform
    {
        if (collision.gameObject.CompareTag("Platform") && gameObject.CompareTag("Player"))
        {
            playerAs.Pause(); // Pausing the rolling sound
        }
    }

}
