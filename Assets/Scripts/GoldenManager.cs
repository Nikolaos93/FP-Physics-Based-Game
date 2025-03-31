using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenManager : MonoBehaviour
{
    private Rigidbody goldenRb;
    private GameManager gameManager; // This is the GameManager script that checks if the game is active
    public ParticleSystem goldenExplosionParticle; // Reference to the particle system (child object of the golden cube)

    private PlayerController5 playerController5; // Reference to PlayerController5 script

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // Finding the GameManager and getting the GameManager script from it
        playerController5 = GameObject.Find("Player").GetComponent<PlayerController5>(); // Finding the Player and getting the PlayerController5 script from it
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) // This method will be called when the player collides with another object (e.g. the finish line)
    {
        if (collision.gameObject.CompareTag("TrayG") && gameObject.CompareTag("CubeG")) // If the golden tray collides with the golden cube
        {
            playerController5.tray1 = true; // Setting the golden tray to true
            Instantiate(goldenExplosionParticle, transform.position, goldenExplosionParticle.transform.rotation); // Playing the particle effect once to indicate that cube is put in the right place
            //Debug.Log("Tray 1" + playerController5.tray1);
        }

        if (collision.gameObject.CompareTag("Tray5") && gameObject.CompareTag("CubeG")) // If the right tray (2nd stage) collides with the golden cube
        {
            playerController5.tray5 = true; // Marking that golden cube is on the correct tray
            Instantiate(goldenExplosionParticle, transform.position, goldenExplosionParticle.transform.rotation); // Playing the particle effect once to indicate that cube is put in the right place
            //Debug.Log("Tray 5" + playerController5.tray5);
        }
        if (collision.gameObject.CompareTag("Tray4") && gameObject.CompareTag("CubeG")) // If the right tray (2nd stage) collides with the golden cube
        {
            playerController5.tray4 = true; // Marking that golden cube is on the correct tray
            Instantiate(goldenExplosionParticle, transform.position, goldenExplosionParticle.transform.rotation); // Playing the particle effect once to indicate that cube is put in the right place
            //Debug.Log("Tray 5" + playerController5.tray5);
        }
    }

}
