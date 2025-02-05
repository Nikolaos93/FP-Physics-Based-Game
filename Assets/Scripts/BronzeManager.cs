using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BronzeManager : MonoBehaviour
{
    private Rigidbody bronzeRb;
    private GameManager gameManager; // This is the GameManager script that checks if the game is active
    public ParticleSystem bronzeExplosionParticle; // Reference to the particle system (child object of the bronze cube)

    public bool bronzeCheck1 = false; // Bool variable for confirming if the bronze cube is put on the right tray

    private PlayerController5 playerController5; // Reference to PlayerController5 script
    private SilverManager silverManager; // Reference to SilverManger script


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // Finding the GameManager and getting the GameManager script from it
        playerController5 = GameObject.Find("Player").GetComponent<PlayerController5>(); // Finding the Player and getting the PlayerController5 script from it
        silverManager = GameObject.Find("Silver Cube (1)").GetComponent<SilverManager>(); // Finding the silver cube from the 2nd stage of the level
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) // This method will be called when the player collides with another object (e.g. the finish line)
    {
        if (collision.gameObject.CompareTag("TrayB") && gameObject.CompareTag("CubeB")) // If the bronze tray collides with the bronze cube
        {
            playerController5.tray3 = true; // Setting the bronze tray to true
            Instantiate(bronzeExplosionParticle, transform.position, bronzeExplosionParticle.transform.rotation); // Playing the particle effect once to indicate that cube is put in the right place
            //Debug.Log("Tray 3" + playerController5.tray3);
        }

        if (collision.gameObject.CompareTag("Tray4") && gameObject.CompareTag("CubeB")) // If the left tray (2nd stage) collides with the bronze cube
        {
            bronzeCheck1 = true; // Marking that bronze cube is on the correct tray
            Instantiate(bronzeExplosionParticle, transform.position, bronzeExplosionParticle.transform.rotation); // Playing the particle effect once to indicate that cube is put in the right place
            if (bronzeCheck1 && silverManager.silverCheck1) // Checicking if both bronze and silver cubes have been put on the same tray
            {
                playerController5.tray4 = true; // if both cubes are on the tray activating the tray
                //Debug.Log("Tray 4" + playerController5.tray4);
            }    
        }
    }
}
