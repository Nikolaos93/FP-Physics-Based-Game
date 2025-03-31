using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverManager : MonoBehaviour
{
    private Rigidbody silverRb;
    private GameManager gameManager; // This is the GameManager script that checks if the game is active
    public ParticleSystem silverExplosionParticle; // Reference to the particle system (child object of the silver cube)

    public bool silverCheck1 = false; // Bool variable for confirming if the silver cube is put on the right tray

    private PlayerController5 playerController5; // Reference to PlayerController5 script
    private BronzeManager bronzeManager; // Reference to BronzeManger script


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // Finding the GameManager and getting the GameManager script from it
        playerController5 = GameObject.Find("Player").GetComponent<PlayerController5>(); // Finding the Player and getting the PlayerController5 script from it
        bronzeManager = GameObject.Find("Bronze Cube (1)").GetComponent<BronzeManager>(); // Finding the silver cube from the 2nd stage of the level
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) // This method will be called when the player collides with another object (e.g. the finish line)
    {
        if (collision.gameObject.CompareTag("TrayS") && gameObject.CompareTag("CubeS")) // If the silver tray collides with the silver cube
        {
            playerController5.tray2 = true; //Setting the silver tray to true
            Instantiate(silverExplosionParticle, transform.position, silverExplosionParticle.transform.rotation); // Playing the particle effect once to indicate that cube is put in the right place
            //Debug.Log("Tray 2" + playerController5.tray2);
        }

        if (collision.gameObject.CompareTag("Tray4") && gameObject.CompareTag("CubeS")) // If the left tray (2nd stage) collides with the silver cube
        {
            silverCheck1 = true; // Marking that silver cube is on the correct tray
            Instantiate(silverExplosionParticle, transform.position, silverExplosionParticle.transform.rotation); // Playing the particle effect once to indicate that cube is put in the right place
            if (silverCheck1 && bronzeManager.bronzeCheck1) // Checicking if both silver and bronze cubes have been put on the same tray
            {
                playerController5.tray4 = true; // if both cubes are on the tray activating the tray
                //Debug.Log("Tray 4" + playerController5.tray4);
            }
        }
        if (collision.gameObject.CompareTag("Tray5") && gameObject.CompareTag("CubeS")) // If the right tray (2nd stage) collides with the silver cube
        {
            silverCheck1 = true; // Marking that silver cube is on the correct tray
            Instantiate(silverExplosionParticle, transform.position, silverExplosionParticle.transform.rotation); // Playing the particle effect once to indicate that cube is put in the right place
            if (silverCheck1 && bronzeManager.bronzeCheck1) // Checicking if both silver and bronze cubes have been put on the same tray
            {
                playerController5.tray5 = true; // if both cubes are on the tray activating the tray
                //Debug.Log("Tray 4" + playerController5.tray4);
            }
        }
    }

}
