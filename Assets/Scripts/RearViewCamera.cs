using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Controls the Main Camera behaviour, enables it to follow the player and to switch from 3rd person to 1st person */
public class RearViewCamera : MonoBehaviour
{
    public GameObject player;  // Reference to the player object (ball/point/vehicle)
    private Vector3 offset = new Vector3(0, 4, 3); // offset the rearview camera behind/above the player by adding to the player's position (3rd person)

    public GameObject RearViewImage; // Reference to the rearview camera image
    public GameObject RearViewRawImage; // Reference to the rearview camera image
    public GameObject rearViewCamera; // Reference to the rearview camera


    private bool cameraClicked; // this is a flag to check if the camera button/switch has been clicked

    // Start is called before the first frame update
    void Start()
    {
        cameraClicked = true; // set the cameraClicked flag to false
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R) && cameraClicked == false)
        {
            rearViewCamera.SetActive(true); // enable the rearview camera
            RearViewImage.SetActive(true); // enable the rearview camera image
            RearViewRawImage.SetActive(true); // enable the rearview camera image
            cameraClicked = true; // set the cameraClicked flag to true
        }
        else if (Input.GetKeyDown(KeyCode.R) && cameraClicked)
        {
            //rearViewCamera.SetActive(false); // enable the rearview camera
            RearViewImage.SetActive(false); // enable the rearview camera image
            RearViewRawImage.SetActive(false); // enable the rearview camera image
            cameraClicked = false; // set the cameraClicked flag to true
        }

        transform.position = player.transform.position + offset; // offset the camera behind/above the player by adding to the player's position

    }
}
