using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//                       0      1      2        3
 public enum GAMESTATE{ menu, play, paused,  gameOver }
public class GameManager : MonoBehaviour
{
    private GAMESTATE gamestate = GAMESTATE.menu;

    //UI references
    public GameObject startMenu;
    public GameObject gameOverMenu;
    public GameObject playMenu;
    public GameObject pauseMenu;

    public GameObject player;
    public TextMeshProUGUI gameOverScoreText;

    private bool is_game_paused = false;
    private float finalScore;
    //

    // Start is called before the first frame update
    void Start()
    {

        
        if (PlayerPrefs.HasKey("gameValuePrev"))
        {
            gamestate = (GAMESTATE)PlayerPrefs.GetInt("gameValuePrev");
            if (gamestate == GAMESTATE.play)
            {
                startGame();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (gamestate)
        {
            case GAMESTATE.menu:
                displayMenu();
                break;
            case GAMESTATE.play:
                playBehaviour();
                break;
            case GAMESTATE.paused:
                displayPauseMenu();
                break;
            case GAMESTATE.gameOver:
                displayGameOver();
                break;
        }

        
    }
    //Display Menus functions
    private void displayPauseMenu()
    {

        Debug.Log("Game paused !");
        pauseMenu.SetActive(true);
        //throw new NotImplementedException();
    }

    private void displayGameOver()
    {
        gameOverMenu.SetActive(true);
        playMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    private void playBehaviour()
    {
        //nbplateform = plateformCreator.getNbplateform();
        //if(nbplateform >)
        playMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    private void displayMenu()
    {
        startMenu.SetActive(true);
        playMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    public GAMESTATE getGameState()
    {
        return gamestate;
    }
    // Changing states functions
    public void menu()
    {
        gamestate = GAMESTATE.menu;
    }
    public void startGame() {
        Debug.Log("Starting the game");
        gamestate = GAMESTATE.play;
        startMenu.SetActive(false);

    }

    public void pauseGame()
    {
        if (!is_game_paused)
        {
            Debug.Log("Pausing the game");
            gamestate = GAMESTATE.paused;
            is_game_paused = !is_game_paused;
            //startMenu.SetActive(true);
        }

        else
        {
            Debug.Log("Unpausing the game");
            gamestate = GAMESTATE.play;
            is_game_paused = !is_game_paused;
            //startMenu.SetActive(false);

        }

    }
    public void gameOver()
    {
        gamestate = GAMESTATE.gameOver;
        
        PlayerController playerController = player.GetComponent<PlayerController>();
        finalScore = playerController.getCurrentScore();
        Debug.Log("Final score : " + finalScore.ToString());
        gameOverScoreText.text = "your score : " + ((int)(20 * finalScore)).ToString();
    }

    public void backToMenu()
    {
        gamestate = GAMESTATE.menu;
        PlayerPrefs.SetInt("gameValuePrev", (int)gamestate);
        SceneManager.LoadScene(0);
    }
    public void replayGame()
    {
        gamestate = GAMESTATE.play;
        PlayerPrefs.SetInt("gameValuePrev", (int)gamestate);
        SceneManager.LoadScene(0);
    }
   
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("gameValuePrev");
    }
}
