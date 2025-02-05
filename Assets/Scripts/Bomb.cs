using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.Burst.CompilerServices;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class Bomb : MonoBehaviour
{
    private Rigidbody bombRb;
    private GameManager gameManager; // This is the GameManager script that checks if the game is active
    public ParticleSystem explosionParticle; // Reference to the particle system

    private CollisionSound collisionSound; // Reference to the CollisionSoundManager

    //private Transform pingPongOscillation;
    public float maxHeight1 = 1f;// max height of the object's movement 
    public float yCenter1 = 1f; // center around which object oscilates
    public float maxHeight2 = 0.5f;//max height of the object's movement 
    public float yCenter2 = 6f; // center around which object oscilates
    private Vector3 rotateAmount; // Angle by which object will rotate

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // Finding the GameManager and getting the GameManager script from it
        collisionSound = GameObject.Find("CollisionSoundManager").GetComponent<CollisionSound>(); // Finding the CollisionSoundManager and getting the CollisionSoundManager script from it

        rotateAmount = new Vector3(0, 60, 0); // initiating the angle of rotation to be 60 degrees on y-axis
    }

    // Update is called once per frame
    void Update()
    {
        // Behaviour of the "not bomb" (collectables) on all levels except on Level 1 and Level 4
        if (gameObject.tag != "Bomb" && (SceneManager.GetActiveScene().name != "Level 1" || SceneManager.GetActiveScene().name != "Level 4")) 
        {
            transform.position = new Vector3(transform.position.x, yCenter1 + Mathf.PingPong(Time.time * 0.1f, maxHeight1) - maxHeight1 / 2f, transform.position.z); // oscilating on y axis only
            transform.Rotate(rotateAmount * Time.deltaTime); // rotating by the predefined angle over time
        }
        // Behaviour of the "not bomb" (collectables) on Level 1 and Level 4
        if (gameObject.tag != "Bomb" && (SceneManager.GetActiveScene().name == "Level 1" || SceneManager.GetActiveScene().name == "Level 4"))
        {
            transform.position = new Vector3(transform.position.x, yCenter2 + Mathf.PingPong(Time.time * 0.1f, maxHeight2) - maxHeight2 / 2f, transform.position.z); // oscilating on y axis only
            transform.Rotate(rotateAmount * Time.deltaTime); // rotating by the predefined angle over time
        }
        // Behaviour of the bomb on all levels except on Level 1 and Level 4
        if (gameObject.tag == "Bomb" && (SceneManager.GetActiveScene().name != "Level 1" || SceneManager.GetActiveScene().name != "Level 4"))
        {
            transform.position = new Vector3(transform.position.x, 0.25f + Mathf.PingPong(Time.time * 0.1f, 0.25f) - 0.25f / 2f, transform.position.z); // oscilating on y axis only
        }
        // Behaviour of the bomb on Level 1 and Level 4
        if (gameObject.tag == "Bomb" && (SceneManager.GetActiveScene().name == "Level 1" || SceneManager.GetActiveScene().name == "Level 4"))
        {
            transform.position = new Vector3(transform.position.x, (0.25f + Mathf.PingPong(Time.time * 0.1f, 0.25f) - 0.25f / 2f) + 5.5f, transform.position.z); // oscilating on y axis only
        }
    }

    /*private void OnTriggerEnter(Collider other) // Checking if something has collided with the bomb
    {
        Destroy(gameObject); // Destroying the bomb in case of collision 
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation); // Playing the explosion particle effect
        gameManager.GameOver(); // Calling the GameOver method from GameManager
    }*/

    private void OnCollisionEnter(Collision collision) // Checking what collided with the bomb
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Bomb")) // checking if the player collided with bomb
        {
            //Debug.Log("Bomb triggered by me: ");
            collisionSound.PlayExplosionSound(); // Playing the explosion sound
            
            Destroy(gameObject); // Destroying the bomb in case of collision
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation); // Playing the explosion particle effect
            gameManager.GameOver(); // Calling the GameOver method from GameManager
        }
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Gem")) // checking if the player collided with the gem/diamond
        {
            //Debug.Log("Gem triggered by: ");
            
            collisionSound.PlayCollectableSound(); // Playing pickup sound
            
            Destroy(gameObject); // destroying the gem in case of collision 
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation); // Playing the particle effect for pickup 
            gameManager.collectablePoints += 10;
            gameManager.UpdateScore(10); // Calling the UpdateScore from GameManager and updating the score by 10
        }
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Star")) // Checking if the Player collided with the star
        {
            //Debug.Log("Star triggered by: ");
            
            collisionSound.PlayCollectableSound(); // Playing pickup sound
             
            Destroy(gameObject); // Destroying the star in case of collision
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation); // Playing the particle effect for pickup 
            gameManager.collectablePoints += 5;
            gameManager.UpdateScore(5); // Calling the UpdateScore from GameManager and updating the score by 5
        }

    }
}

