using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function that clears the player prefs (high score)
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
