using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public float scrollSpeed = 40.0f;

    private RectTransform rectTransform;

    public GameObject creditsScreen, welcomeScreen;

    private float startingTime;



    // Start is called before the first frame update
    void Start()
    {
        // Get the RectTransform component of the UI element
        rectTransform = GetComponent<RectTransform>();

        startingTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        // Move the text upwards over time
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        if (Input.anyKey || (Time.time - startingTime) >= 60)
        {
            rectTransform.anchoredPosition = new Vector2(0, -600);

            creditsScreen.SetActive(false);
            welcomeScreen.SetActive(true);

        }
    }
}
