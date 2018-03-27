using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeBeh : MonoBehaviour
{
    private int playCount = 1;     // indicate is whose turn: playCount % 2 = 1 -> player 1(X)   
                                   //                         playCount % 2 = 0 -> player 2(O)
    private int[,] chessBoard = new int[3, 3];
                                    // chessBoard have 9 unit
                                    // unit value 0 -> can click
                                    // unit value 1 -> player 1's chess
                                    // unit value 2 -> player 2's chess
    private string gameMsg = "Click [Start / Reset] Button to start the game";
    private int chessBoardXPos = Screen.width / 2 - 90;
    private int chessBoardYPos = Screen.height / 2 - 90;
    private int chessUnitLen = 60;
    private bool isGameStart = false;
    public GUIStyle fontstyle;

    void resetGame()
    {
        gameMsg = "Click [Start / Reset] Button to start the game";
        playCount = 1;
        for(int i = 0; i < 3; ++i)
        {
            for(int j = 0; j < 3; ++j)
            {
                chessBoard[i, j] = 0;
            }
        }
    }

    void Start()
    {
        resetGame();    
    }

    void OnGUI()
    {
        
        GUI.Label(new Rect(chessBoardXPos - 120, chessBoardYPos - 50, 420, 30), gameMsg, fontstyle);
 
        if (GUI.Button(new Rect(Screen.width/2 - 60, Screen.height/2 + 130, 120, 30), "Start / Reset"))
        {
            resetGame();
            gameMsg = "player 1(X)'s turn";
            isGameStart = true;
        }
        int gameResult = checkWin();
        if (gameResult == 1) gameMsg = "player 1(X) Win";
        else if (gameResult == 2) gameMsg = "player 2(O) Win";
        else if (gameResult == -1) gameMsg = "game tie";



        for (int i = 0; i < 3; ++i)
        {

            for (int j = 0; j < 3; ++j)
            {
                if (chessBoard[i, j] == 0)
                {
                    if (GUI.Button(new Rect(chessBoardXPos + i * chessUnitLen, chessBoardYPos + j * chessUnitLen, chessUnitLen, chessUnitLen), "") && isGameStart && gameResult == 0)
                    {
                        if ((playCount++) % 2 == 0)
                        {
                            chessBoard[i, j] = 2;
                            gameMsg = "player 1(X)'s turn";
                        }
                        else
                        {
                            chessBoard[i, j] = 1;
                            gameMsg = "player 2(O)'s turn";
                        }
                    }
                }
                else if (chessBoard[i, j] == 1)
                {
                    GUI.Button(new Rect(chessBoardXPos + i * chessUnitLen, chessBoardYPos + j * chessUnitLen, chessUnitLen, chessUnitLen), "X");
                }
                else
                {
                    GUI.Button(new Rect(chessBoardXPos + i * chessUnitLen, chessBoardYPos + j * chessUnitLen, chessUnitLen, chessUnitLen), "O");
                }
            }
        }

    }

    // return 1 means player 1 win
    // return 2 means player 2 win
    // return 0 means game not finish
    // return -1 means game tie
    int checkWin()
    {
        // row
        for(int i = 0; i < 3; ++i)
        {
            if(chessBoard[i, 0] != 0 && chessBoard[i, 0] == chessBoard[i, 1] && chessBoard[i, 1] == chessBoard[i, 2])
            {
                return chessBoard[i, 0];
            }
        }

        // col
        for(int j = 0; j < 3; ++j)
        {
            if(chessBoard[0, j] != 0 && chessBoard[0, j] == chessBoard[1, j] && chessBoard[1, j] == chessBoard[2, j])
            {
                return chessBoard[0, j];
            }
        }

        // diagonal left up to right down
        if(chessBoard[0, 0] != 0 && chessBoard[0, 0] == chessBoard[1, 1] && chessBoard[1, 1] == chessBoard[2, 2])
        {
            return chessBoard[0, 0];
        }

        // diagonal right up to left down
        if(chessBoard[0, 2] != 0 && chessBoard[0, 2] == chessBoard[1, 1] && chessBoard[1, 1] == chessBoard[2, 0])
        {
            return chessBoard[0, 2];
        }

        // not finish
        for(int i = 0; i < 3; ++i)
        {
            for(int j = 0; j < 3; ++j)
            {
                if (chessBoard[i, j] == 0) return 0;
            }
        }

        // game tie
        return -1;
    }


}
