using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    bool isFirst = true;
    // Use this for initialization  
    void Start()
    {
        action = Director.getInstance().currentSceneControl as IUserAction;

    }

    private void OnGUI()
    {
        if (Input.GetButtonDown("Fire1"))
        {

            Vector3 pos = Input.mousePosition;
            action.hit(pos);

        }

        GUI.Label(new Rect(10, 10, 400, 400), action.GetScore().ToString());

        if (isFirst && GUI.Button(new Rect(Screen.width / 2 - 45, 20, 90, 50), "Start"))
        {
            isFirst = false;
            action.setGameState(GameState.ROUND_START);
        }

        if (!isFirst && action.getGameState() == GameState.ROUND_FINISH && GUI.Button(new Rect(Screen.width / 2 - 45, 20, 90, 50), "Next Round"))
        {
            action.setGameState(GameState.ROUND_START);
        }

    }


}
