using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public enum GAMESTATE{ menu, play, paused ,  gameOver }
public class GameManager : MonoBehaviour
{
    private GAMESTATE gamestate = GAMESTATE.menu;
    //UI references
    public GameObject startMenu;

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
        throw new NotImplementedException();
    }

    private void playBehaviour()
    {
        // Générer les plateformes ici 
        throw new NotImplementedException();
    }

    private void displayMenu()
    {
        throw new NotImplementedException();
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
}
