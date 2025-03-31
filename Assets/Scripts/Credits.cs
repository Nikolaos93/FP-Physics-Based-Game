using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public float scrollSpeed = 40.0f;

    private RectTransform rectTransform;

    public GameObject creditsScreen/*, welcomeScreen*/;

    private float startingTime;

    //private GameManager gameManager; // This is the GameManager script that checks if the game is active


    // Start is called before the first frame update
    void Start()
    {
        // Get the RectTransform component of the UI element
        rectTransform = GetComponent<RectTransform>();

        startingTime = Time.time;

        //gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // Finding the GameManager and getting the GameManager script from it

    }

    // Update is called once per frame
    void Update()
    {
        // Move the text upwards over time
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        if (Input.anyKey || (Time.time - startingTime) >= 60)
        {
            /*rectTransform.anchoredPosition = new Vector2(0, -600);

            creditsScreen.SetActive(false);
            welcomeScreen.SetActive(true);*/

            //gameManager.RestartGame();
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                creditsScreen.SetActive(false);
            }
            
        }
    }
}
