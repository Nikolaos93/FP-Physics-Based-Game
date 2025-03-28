using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    // Reference to the text component in the canvas/ui
    [SerializeField] private TMP_Text fpsText;

    // Configuration options
    [SerializeField] private bool showFPS = true;
    [SerializeField] private float updateInterval = 0.5f;

    // Internal variables
    private float deltaTime = 0.0f;
    private float lastUpdate = 0.0f;
    private int frameCount = 0;


    private bool _fpsEnabled;
    public Toggle fpsToggle; // Reference to the toggle (in settings menu) for enabling/disabling FPS counter

    public void EnableFPS(bool fpsEnabled)
    {
        _fpsEnabled = fpsEnabled;

        PlayerPrefs.SetInt("masterFPS", (_fpsEnabled ? 1 : 0));
        //fpsText.enabled = _fpsEnabled;
        if (_fpsEnabled == false)
        {
            fpsText.gameObject.SetActive(false);
        }
        else
        {
            fpsText.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("masterFPS") == 1)
        {
            fpsToggle.isOn = true; // Enabling it if the stored value is true
            fpsText.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("masterFPS") == 0)
        {
            //fpsToggle.isOn = false; // Disabling it if the stored value is false
            fpsText.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!showFPS || fpsText == null)
        {
            return;
        }

        // Accumulate frame time
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        frameCount++;

        // Update the FPS counter
        float elapsed = Time.time - lastUpdate;
        if (elapsed >= updateInterval)
        {
            float msec = deltaTime*1000.0f;
            float fps = frameCount/elapsed;

            fpsText.text = string.Format("FPS: {1:0.}" + System.Environment.NewLine + "{0:0.0} ms", msec, fps);

            // Reset the counters
            lastUpdate = Time.time;
            frameCount = 0;
        }


        if (PlayerPrefs.GetInt("masterFPS") == 1)
        {
            //fpsToggle.isOn = true; // Enabling it if the stored value is true
            fpsText.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("masterFPS") == 0)
        {
            fpsToggle.isOn = false; // Disabling it if the stored value is false
            fpsText.gameObject.SetActive(false);
        }
    }
}
