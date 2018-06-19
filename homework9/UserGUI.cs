using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {

    private string hint = "";
    private IUserAction action;
    public int GameStatus = 0;
    private GUIStyle hintStyle;
    private GUIStyle btnStyle;

    // Use this for initialization
    void Start () {
        action = SSDirector.getInstance().currentSceneController as IUserAction;
	}



    void OnGUI()
    {
        hintStyle = new GUIStyle
        {
            fontSize = 15,
            fontStyle = FontStyle.Normal
        };
        btnStyle = new GUIStyle("button")
        {
            fontSize = 15
        };
        GUI.Label(new Rect(Screen.width / 2 - 500, Screen.height / 2 - 210, 100, 50),
         hint, hintStyle);
        if (GUI.Button(new Rect(10, 10, 80, 30), "Hint", btnStyle))
        {
            //Debug.Log("StateRight: " + state.rightDevils + " " + state.rightPriests);
            //Debug.Log("StateLeft: " + state.leftDevils + " " + state.leftPriests);

            IState temp = IState.bfs(FirstController.state, FirstController.endState);
            //Debug.Log("NextRight: " + temp.rightDevils + " " + temp.rightPriests);
            //Debug.Log("NextLeft: " + temp.leftDevils + " " + temp.leftPriests);
            hint = "Hint:\n" + "Right:  Devils: " + temp.rightDevils + "   Priests: " + temp.rightPriests +
              "\nLeft:  Devils: " + temp.leftDevils + "   Priests: " + temp.leftPriests;
            //int priestsOffset = temp.leftPriests - state.leftPriests;
            //int devilsOffset = temp.leftDevils - state.leftDevils;
            //Debug.Log("offset: " + priestsOffset + " " + devilsOffset);
            //controller.AIMove(priestsOffset, devilsOffset);
        }
        if (GameStatus == 1)
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
