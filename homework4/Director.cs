using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneControl
{
    void LoadResources();
}

public enum GameState { ROUND_START, ROUND_FINISH, RUNNING, PAUSE, START }

public interface IUserAction
{
    void GameOver();
    GameState getGameState();
    void setGameState(GameState gs);
    int GetScore();
    void hit(Vector3 pos);
}


public class Director : System.Object
{


    public ISceneControl currentSceneControl { get; set; }

    private static Director director;

    private Director()
    {

    }

    public static Director getInstance()
    {
        if (director == null)
        {
            director = new Director();
        }
        return director;
    }
}