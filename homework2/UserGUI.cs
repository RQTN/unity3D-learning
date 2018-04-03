using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {

    private IUserAction action;
    public int GameStatus = 0;
	// Use this for initialization
	void Start () {
        action = SSDirector.getInstance().currentSceneController as IUserAction;
	}

    void OnGUI()
    {
        if(GameStatus == 1)
        {
            Debug.Log("GameStatus: 1");
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "GameOver!");
            if(GUI.Button(new Rect(Screen.width/2 - 70, Screen.height / 2, 140, 70), "Restart"))
            {
                GameStatus = 0;
                action.restart();
            }
        } else if(GameStatus == 2)
        {
            Debug.Log("GameStatus: 2");
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You win!");
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart"))
            {
                GameStatus = 0;
                action.restart();
            }
        }
    }
}
