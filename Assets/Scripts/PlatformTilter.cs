using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTilter : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager script that checks if the game is active

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (gameManager.isGameActive) // If the game is active, then tilt the platform
        {   
            // Using PingPong method to tilt the platform on x-axis and z-axis
            transform.localEulerAngles = new Vector3(Mathf.PingPong(Time.time * 40, 30) - 15, 0, Mathf.PingPong(Time.time * 30, 40) - 20);
        }
        
    }
}
