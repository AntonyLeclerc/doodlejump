using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

 public enum GAMESTATE{ menu, play, paused ,  gameOver }
public class GameManager : MonoBehaviour
{
    private GAMESTATE gamestate = GAMESTATE.menu;
    //UI references
    public GameObject startMenu;
    public GameObject gameOverMenu;

    //

    // Start is called before the first frame update
    void Start()
    {
        
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
            case GAMESTATE.gameOver:
                displayGameOver();
                break;
        }
        
    }

    private void displayGameOver()
    {
        gameOverMenu.SetActive(true);
    }

    private void playBehaviour()
    {
        // Générer les plateformes ici 
        throw new NotImplementedException();
    }

    private void displayMenu()
    {
        startMenu.SetActive(true);
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
    public void gameOver()
    {
        gamestate = GAMESTATE.gameOver;
    }

    public void backToMenu()
    {
        gamestate = GAMESTATE.menu;
        SceneManager.LoadScene(0);
    }
}
