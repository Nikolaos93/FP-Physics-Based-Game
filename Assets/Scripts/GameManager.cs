using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Texts (names are self explanatory) 
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerCountdownText;
    public TextMeshProUGUI levelCompleteText;
    public TextMeshProUGUI levelResultsText;
    public TextMeshProUGUI gameCompleteText;
    public TextMeshProUGUI gameResultsText;
    public TextMeshProUGUI pauseText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI lifeLostText;


    /*public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;*/

    public TextMeshProUGUI checkpointText;
    public TextMeshProUGUI checkpointAdviceText;
    public TextMeshProUGUI checkpointAdvice2Text;

    // Buttons (names are self explanatory)
    public Button nextLevelButton;
    public Button finishGameButton;
    public Button continueButton;
    public Button leaveButton;
    public Button restartButton;
    public Button quitButton;
    public Button resumeButton;
    public Button proceedButton;

    // Variables for tracking game stats and states
    private int score;
    public float timer;
    public bool isGameActive;
    public int hintClicks;

    // Game Object for in game Menus/UI/Images
    public GameObject titleScreen;
    public GameObject pauseScreen;
    public GameObject levelFinishedScreen;
    public GameObject gameFinishedScreen;
    public GameObject statsScreen;
    public GameObject gameOverScreen;
    public GameObject lifeLostScreen;
    public GameObject checkpointScreen;
    public GameObject life1, life2, life3;

    private int highscore;
    public TMP_Text highscoreText;

    public PlayerController playerController;
    public PlayerController3 playerController3;
    public PlayerController4 playerController4;
    public PlayerController5 playerController5;


    public Toggle dayNightToggle; // Reference to the toggle (in settings menu) for enabling/disabling day/night
    public Toggle vfxToggle;

    public int collectablePoints;
    private int scoreSoFar;

    public GameObject dayUpperDome;
    public GameObject dayLowerDome;
    public GameObject nightUpperDome;
    public GameObject nightLowerDome;

    public GameObject visualEffect;

    // Start is called before the first frame update
    void Start()
    {

        // Initialize the timer based on the current level
        if (SceneManager.GetActiveScene().name == "Level 5")
        {
            timer = 180; // timer is 180 seconds only on level 5
        }
        else
        {
            timer = 60; // timer is 60 seconds on all levels except Level 5
        }

        //Finding the Player and its PlayerController script
        //playerController = GameObject.Find("Player").GetComponent<PlayerController>(); 


        if (DataManager.Instance.dayNight) // Checking stored setting/value for toggle in DataManager
        {
            dayNightToggle.isOn = true; // Enabling it if the stored value is true
        }
        if (!DataManager.Instance.dayNight) // Checking stored setting/value for toggle in DataManager
        {
            dayNightToggle.isOn = false; // Disabling it if the stored value is false
        }

        Debug.Log(DataManager.Instance.vfxON);
        Debug.Log(vfxToggle.isOn);
        if (DataManager.Instance.vfxON == true) // Checking stored setting/value for toggle in DataManager
        {
            vfxToggle.isOn = true; // Enabling it if the stored value is true
        }
        if (DataManager.Instance.vfxON == false) // Checking stored setting/value for toggle in DataManager
        {
            vfxToggle.isOn = false; // Disabling it if the stored value is false
        }


        collectablePoints = 0;
        Debug.Log("Collectable Points: " + collectablePoints);
        scoreSoFar = DataManager.Instance.scoreOverall;
        Debug.Log("Score so far: " + scoreSoFar);
    }

    // Update is called once per frame
    void Update()
    {
        if (dayNightToggle.isOn) // checking if the day/night toggle is on
        {
            DataManager.Instance.dayNight = true; // if it is storing value "true" in DataManager
            dayUpperDome.SetActive(true); // enabling the upper dome for day
            dayLowerDome.SetActive(true); // enabling the lower dome for day
            nightUpperDome.SetActive(false); // disabling the upper dome for night
            nightLowerDome.SetActive(false); // disabling the lower dome for night
        }
        if (!dayNightToggle.isOn) // checking if the day/night toggle is off
        {
            DataManager.Instance.dayNight = false; // if it is storing value "false" in DataManager
            dayUpperDome.SetActive(false); // disabling the upper dome for day
            dayLowerDome.SetActive(false); // disabling the lower dome for day
            nightUpperDome.SetActive(true); // enabling the upper dome for night
            nightLowerDome.SetActive(true); // enabling the lower dome for night
        }

        if (vfxToggle.isOn == true) // checking if the VFX toggle is on
        {
            DataManager.Instance.vfxON = true; // if it is storing value "true" in DataManager
            visualEffect.SetActive(true); // enabling the visual effect
        }
        if (vfxToggle.isOn == false) // checking if the VFX toggle is off
        {
            DataManager.Instance.vfxON = false; // if it is storing value "false" in DataManager
            visualEffect.SetActive(false); // disabling the visual effect
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //checking if Esc was pressed
        {
            PauseGame(); // Calling PauseGame method
        }
        
        CountdownTimer(); // Calling the CountdownTimer method

        switch (DataManager.Instance.livesLeft) // Checking how many lives are left
        {
            case 2: // 2 lives left 
                life3.SetActive(false); //disable 3rd heart image
                break;
            case 1: // 1 life left
                life3.SetActive(false); //disable 3rd heart image
                life2.SetActive(false); //disable 2nd heart image
                break;
            case 0: // no lives left
                life3.SetActive(false); //disable 3rd heart image
                life2.SetActive(false); //disable 2nd heart image
                life1.SetActive(false); //disable 1st heart image
                break;
        }

    }

    public void StartGame() // This method will be called when the player clicks the "Play" button
    {
        isGameActive = true; // setting the state of the game as true/active

        if (DataManager.Instance != null)
        {
            score = DataManager.Instance.scoreOverall;
        } else
        {
            score = 0;
            DataManager.Instance.maxLives = 3;
            DataManager.Instance.livesLeft = 3;
        }

        // Initialize the timer based on the current level
        if (SceneManager.GetActiveScene().name == "Level 5")
        {
            timer = 180; // timer is 180 seconds only on level 5
        } 
        else
        {
            timer = 60; // timer is 60 seconds on all levels except Level 5
        }

        UpdateScore(0); // Setting the score to 0 at the start of the game

        titleScreen.gameObject.SetActive(false); // disabling MainMenu when the game starts

    }

    public void PauseGame() // Method for pausing the game
    {
        if (isGameActive) // checking whether the game is active
        {
            // Activating all relevant screens/texts/buttons for Pause Menu
            pauseScreen.gameObject.SetActive(true);
            pauseText.gameObject.SetActive(true);
            leaveButton.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(true);
            isGameActive = false; // Flagging the game state as inactive/false
        }
        else if (!isGameActive) // Checking if the game is inactive
        {
            // Disabling all relevant screens/texts/buttons for Pause Menu
            pauseScreen.gameObject.SetActive(false);
            pauseText.gameObject.SetActive(false);
            leaveButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(false);
            isGameActive = true; // Flagging the game state as active/true
        }
    }
    public void UnpauseGame() // This method will be called when the player clicks the "Continue" button
    {
        isGameActive = true; // Set the game state to active again
    }

    public void CheckpointReached() // This method will be called when the player reaches a checkpoint
    {
        if (isGameActive) // checking if the game is active
        {
            // Activating relevant screens/texts/buttons depending on the checkpoint
            checkpointScreen.gameObject.SetActive(true);
            checkpointText.gameObject.SetActive(true);
            if (playerController.checkpointReached == 1 || playerController3.checkpointReached == 1 || playerController4.checkpointReached == 1 || playerController5.checkpointReached == 1)
            {
                checkpointAdviceText.gameObject.SetActive(true);
            }
            else if (playerController.checkpointReached == 2 || playerController3.checkpointReached == 2 || playerController4.checkpointReached == 2 || playerController5.checkpointReached == 2)
            {
                checkpointAdviceText.gameObject.SetActive(false);
                checkpointAdvice2Text.gameObject.SetActive(true);
            }
            //checkpointAdviceText.gameObject.SetActive(true);
            proceedButton.gameObject.SetActive(true);
            isGameActive = false; // flagging the game state as inactive/false
        }
        else if (!isGameActive) // Checking if the game is not active
        {
            // Disabling relevant screens/texts/buttons depending on the checkpoint
            checkpointScreen.gameObject.SetActive(false);
            checkpointText.gameObject.SetActive(false);
            if (playerController.checkpointReached == 1 || playerController3.checkpointReached == 1 || playerController4.checkpointReached == 1 || playerController5.checkpointReached == 1)
            {
                checkpointAdviceText.gameObject.SetActive(false);
            }
            else if (playerController.checkpointReached == 2 || playerController3.checkpointReached == 2 || playerController4.checkpointReached == 2 || playerController5.checkpointReached == 2)
            {
                checkpointAdvice2Text.gameObject.SetActive(false);
            }
            //checkpointAdviceText.gameObject.SetActive(false);
            proceedButton.gameObject.SetActive(false);
            isGameActive = true; // flagging the game state as active/true
        }
    }

    public void LevelComplete() // This method will be called when the player reaches the finish line
    {

        //DataManager.Instance.scoreOverall = score; // Keeping the score for the next level

        // Activating all relevant screens/texts/buttons for Level Complete Menu
        if (SceneManager.GetActiveScene().name != "Level 6")
        {
            // Calculating the score at the end of the level based on: 100 for level completion + seconds left - 30 points for each hint used
            UpdateScore(100 + (int)timer + (90 - hintClicks * 30));
            DataManager.Instance.scoreOverall = score; // Keeping the score for the next level

            // Showing Level Finished Menu and calculating score
            levelFinishedScreen.gameObject.SetActive(true);
            nextLevelButton.gameObject.SetActive(true);
            levelCompleteText.gameObject.SetActive(true);
            levelResultsText.gameObject.SetActive(true);
            levelResultsText.text = "Score:" + Environment.NewLine + "so far (" + scoreSoFar + ")" + Environment.NewLine + "+" + Environment.NewLine + 
                                    "level completion (100)" + Environment.NewLine + "+" + Environment.NewLine + 
                                    "collectables (" + collectablePoints + ")" + Environment.NewLine + "+" + Environment.NewLine + 
                                    "timer (" + (int)timer + ")" + Environment.NewLine + " + " + Environment.NewLine + 
                                    "hints (90 - 30*" + hintClicks + " used)" + Environment.NewLine + 
                                    "=" + Environment.NewLine + score;
        }
        

        isGameActive = false; // flagging the game state as inactive/false

        // If the final level is complete updating the highscore if needed
        if (SceneManager.GetActiveScene().name == "Level 6") 
        {
            // Calculating the score at the end of the game based on: 200 per life left + 100 for level completion + seconds left - 30 points for each hint used
            UpdateScore(200*DataManager.Instance.livesLeft + 100 + (90 - hintClicks * 30));
            DataManager.Instance.scoreOverall = score; // Storing the overall score

            // Showing Game Finished Menu and calculating score
            gameFinishedScreen.gameObject.SetActive(true);
            finishGameButton.gameObject.SetActive(true);
            gameCompleteText.gameObject.SetActive(true);
            gameResultsText.gameObject.SetActive(true);
            gameResultsText.text = "Score:" + Environment.NewLine + "so far (" + scoreSoFar + ")" + Environment.NewLine + "+" + Environment.NewLine +
                                   "lives left (200*" + DataManager.Instance.livesLeft + ")" + Environment.NewLine + "+" + Environment.NewLine +
                                   "level completion (100)" + Environment.NewLine + "+" + Environment.NewLine +
                                   "collectables (" + collectablePoints + ")" + Environment.NewLine + "+" + Environment.NewLine +
                                   "hints (90 - 30*" + hintClicks + " used)" + Environment.NewLine +
                                   "=" + Environment.NewLine + score;

            ScoreManager.instance.HighScoreCheck(score); 
        }
    }

    public void UpdateScore(int scoreToAdd) // Method to update the score based on the passed value
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score; // displaying the updated score
    }

    public void GameOver() // This method will be called when the player loses all lives
    {
        if (DataManager.Instance.livesLeft != 1) // if it is not the last life that is lost
        {
            // Activating all relevant screens/texts/buttons for Life Lost Menu
            lifeLostScreen.gameObject.SetActive(true);
            lifeLostText.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
            resumeButton.gameObject.SetActive(true);
            isGameActive = false; // flagging the game state as inactive/false
        }
        else if (DataManager.Instance.livesLeft == 1) // if the last/final life is lost
        {
            ScoreManager.instance.HighScoreCheck(score); // updating the highscore if needed

            // Activating all relevant screens/texts/buttons for Game Over Menu 
            gameOverScreen.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            gameOverText.gameObject.SetActive(true);
            isGameActive = false; // flagging the game state as inactive/false
        }  
    }

    public void RestartGame() // This method will be called when the player clicks the "Restart" button
    {
        DataManager.Instance.scoreOverall = 0; // restarting score back to 0
        DataManager.Instance.maxLives = 3; // restarting maximum lives back to 3 
        DataManager.Instance.livesLeft = 3; // restarting number of lives back to 3
        SceneManager.LoadScene(0); // loading the first scene (Level 1)
    }

    public void ReloadCurrentLevel() // Called when player loses a life but wants to continue playing
    {
        DataManager.Instance.livesLeft--; // decrementing number of lives left

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reloading the current scene/level
    }

    public void LoadNextLevel() // This method will be called when the player clicks the "Next Level" button
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // incrementing the build index
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadLevel3()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadLevel4()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadLevel5()
    {
        SceneManager.LoadScene(4);
    }
    public void LoadLevel6()
    {
        SceneManager.LoadScene(5);
    }

    public void ExitGame() // This method will be called when the player clicks the "Exit" button
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void CountdownTimer() // This method will countdown the timer
    {
        if (isGameActive && timer > 0) // if the game is active and timer hasn't reached 0
        {
            timer -= Time.deltaTime; // decrementing timer
            timerCountdownText.text = "Time: " + Mathf.Round(timer); // Rounding the value for displaying whole numbers on screen
        }

        // if the timer has reached 0 on any level except the last level (Level 6)
        if (timer <= 0 && SceneManager.GetActiveScene().buildIndex != 5)
        {
            GameOver(); // Call the GameOver method when the timer reaches 0
        }
        
        // if the timer has reached 0 on the final level (Level 6); goal is to survive for 60 seconds
        if (timer <= 0 && SceneManager.GetActiveScene().buildIndex == 5 && isGameActive)
        {
            LevelComplete(); // Call the LevelComplete method when the timer reaches 0
        }
    }

    public void hintClicking() // Method for tracking the number of hints used
    {
        hintClicks++; // incrementing the number of hints used

        if (isGameActive) // checking whether the game is active
        {
            isGameActive = false; // Flagging the game state as inactive/false
        }
    }

}
