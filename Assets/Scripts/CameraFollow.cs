using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Controls the Main Camera behaviour, enables it to follow the player and to switch from 3rd person to 1st person */
public class CameraFollow : MonoBehaviour
{
    public GameObject player;  // Reference to the player object (ball/point/vehicle)
    private Vector3 offset = new Vector3(0, 5, -7); // offset the camera behind/above the player by adding to the player's position (3rd person)
    private Vector3 offset2 = new Vector3(0, 7, -14); // offset the camera behind/above the player by adding to the player's position (only used on Level 6) (3rd person)
    private Vector3 offsetFront = new Vector3(0, 2.25f, -0.5f); //offset the camera (1st person)

    private bool cameraClicked = false; // this is a flag to check if the camera button/switch has been clicked

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C) && !cameraClicked) // if the C key is pressed and the camera is not clicked/flagged, then set the cameraClicked flag to true
        {
            cameraClicked = true;
        }
        else if (Input.GetKeyDown(KeyCode.C) && cameraClicked) // if the C key is pressed and the camera is clicked/flagged, then set the cameraClicked flag to false
        {
            cameraClicked = false;
        }

        if (!cameraClicked) // if the camera is not clicked, then follow the player from behind/above (3rd person view)
        {  
            transform.position = player.transform.position + offset; // offset the camera behind/above the player by adding to the player's position

            if (SceneManager.GetActiveScene().buildIndex == 5) // Checking if the current level is Level 6
            {
                transform.position = player.transform.position + offset2; // offset the camera behind/above the player by adding to the player's position
            }
        }
        if (cameraClicked)  // if the camera is clicked, then follow the player from the front (1st person view)
        {
            transform.position = player.transform.position + offsetFront; // offset the camera through the "windshield"
        }
    }
}
