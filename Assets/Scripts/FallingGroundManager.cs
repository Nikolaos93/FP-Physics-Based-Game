using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingGroundManager : MonoBehaviour
{
    public GameObject[] fallingGrounds1; // 1st stage of cubes in the middle lane
    public GameObject[] fallingGroundsA; // 1st stage of cubes in the left lane
    public GameObject[] fallingGroundsX; // 1st stage of cubes in the right lane
    public GameObject[] fallingGrounds2; // 2nd stage of cubes in the middle lane
    public GameObject[] fallingGroundsB; // 2nd stage of cubes in the left lane
    public GameObject[] fallingGroundsY; // 2nd stage of cubes in the right lane
    public GameObject[] fallingGrounds3; // 3rd stage of cubes in the middle lane
    public GameObject[] fallingGroundsC; // 3rd stage of cubes in the left lane
    public GameObject[] fallingGroundsZ; // 3rd stage of cubes in the right lane

    private GameManager gameManager;  // Referennce to the Game Manager
    private PlayerController playerController; // Reference to the Player Controller

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // Finding the GameManager object and get the GameManager script from it
        playerController = GameObject.Find("Player").GetComponent<PlayerController>(); // Finding the Player object and getting the PlayerController script from it
    }

    // Update is called once per frame
    void Update()
    {

        // 1st stage of the falling ground/cubes
        for (int i = 0; i < fallingGrounds1.Length - 1; i++) // Looping through the array of cubes (ground)
        {
            if (playerController.transform.position.z > fallingGrounds1[i].transform.position.z + 1) // Checking whether the player is ahead of the cube so that it can't be outrun
            {
                fallingGrounds1[i].GetComponent<Rigidbody>().isKinematic = false; // disabling kinematic state so that cube can fall
                fallingGroundsA[i].GetComponent<Rigidbody>().isKinematic = false; // disabling kinematic state so that cube can fall
                fallingGroundsX[i].GetComponent<Rigidbody>().isKinematic = false; // disabling kinematic state so that cube can fall
            }
        }

        // 2nd stage of the falling ground/cubes
        if (playerController.checkpointReached == 1) // Checking whether the player has reached the 1st checkpoint
        {
            for (int i = 0; i < fallingGrounds2.Length - 1; i++) // Looping through the array of cubes (ground)
            {
                if (gameManager.timer < 54 - i - 3) // Checking whether it is time to "drop" the cube
                {
                    fallingGrounds2[i].GetComponent<Rigidbody>().isKinematic = false; // disabling kinematic state so that cube can fall
                    fallingGroundsB[i].GetComponent<Rigidbody>().isKinematic = false; // disabling kinematic state so that cube can fall
                    fallingGroundsY[i].GetComponent<Rigidbody>().isKinematic = false; // disabling kinematic state so that cube can fall
                }
            }
        }

        // 3rd stage of the falling ground/cubes
        if (playerController.checkpointReached == 2) // Checking whether the player has reached the 2nd checkpoint
        {
            for (int i = 0; i < fallingGrounds3.Length; i++) // Looping through the array of cubes (ground)
            {
                if (playerController.transform.position.z > fallingGrounds3[i].transform.position.z - 1) // Checking when the player approaches the next cube
                {
                    fallingGrounds3[i].gameObject.SetActive(true); // Setting the state of the cube as "active" when the player is near/appraoching
                    fallingGroundsC[i].gameObject.SetActive(true); // Setting the state of the cube as "active" when the player is near/appraoching
                    fallingGroundsZ[i].gameObject.SetActive(true); // Setting the state of the cube as "active" when the player is near/appraoching
                }
                if (playerController.transform.position.z > fallingGrounds3[i].transform.position.z + 1) // Checking whether the player has passed the previous cube
                {
                    fallingGrounds3[i].GetComponent<Rigidbody>().isKinematic = false; // disabling kinematic state so that cube can fall
                    fallingGroundsC[i].GetComponent<Rigidbody>().isKinematic = false; // disabling kinematic state so that cube can fall
                    fallingGroundsZ[i].GetComponent<Rigidbody>().isKinematic = false; // disabling kinematic state so that cube can fall
                }
            }
        }
    }
}
