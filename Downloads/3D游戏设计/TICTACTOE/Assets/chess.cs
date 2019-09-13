using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chess : MonoBehaviour
{
    private int[,] chessBoard = new int[3, 3];//初始化井字棋棋盘
    private int turn = 1;

    // Use this for initialization
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Reset()
    {
        turn = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                chessBoard[i, j] = 0;
            }
        }
    }

    void OnGUI()
    {
        GUIStyle Style = new GUIStyle();
        Style.normal.background = null;
        Style.normal.textColor = new Color(0, 5, 1);
        Style.fontSize = 35;
        Style.fontStyle = FontStyle.Bold;
        //Type (Position, Content)
        //Rect() defines four properties: left-most position, top-most position, total width, total height.
        //You can use the Screen.width and Screen.height properties to get the total dimensions of the screen space available in the player.
        GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 250, 150, 40), "Tic Tac Toe");

        if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 200, 150, 50), "RESET GAME"))//displays a Box Control with the header text “RESET GAME”.
        {
            Reset();
        }

        //First judge the result.
        int State = check();
        //The second argument for a GUI Control is the actual content to be displayed with the Control. Most often you will want to display some text or an image on your Control.
        if (State == 2)
        {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 + 50, 50, 50), "X Win!", Style);
        }
        else if (State == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 + 50, 50, 50), "O Win!", Style);
        }
        else if (State == 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 + 50, 50, 50), "Equally", Style);
        }

        //The Game is not over,continue.
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                //If the number of the chessboard is 1, it represents O, else 2 represents X.
                if (chessBoard[i, j] == 1)
                {
                    GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), "O");
                }
                else if (chessBoard[i, j] == 2)
                {
                    GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), "X");
                }

                //Click operation.
                if (GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), ""))
                {
                    if (State == 3)
                    {
                        if (turn == 1)
                        {
                            chessBoard[i, j] = 1;
                        }
                        else if (turn == -1)
                        {
                            chessBoard[i, j] = 2;
                        }
                        turn = -turn;
                    }
                }
            }
        }
    }

    int check()
    {
        //Check the rows.
        for (int i = 0; i < 3; i++)
        {
            if (chessBoard[i, 0] == chessBoard[i, 1] && chessBoard[i, 0] == chessBoard[i, 2] && chessBoard[i, 0] != 0)
            {
                return chessBoard[i, 0]; //1
            }
        }

        //Check the columns.
        for (int j = 0; j < 3; j++)
        {
            if (chessBoard[0, j] == chessBoard[1, j] && chessBoard[0, j] == chessBoard[2, j] && chessBoard[0, j] != 0)
            {
                return chessBoard[0, j]; //2
            }
        }

        //Check the diagonals.
        if (chessBoard[0, 0] == chessBoard[1, 1] && chessBoard[0, 0] == chessBoard[2, 2] && chessBoard[0, 0] != 0) return chessBoard[0, 0];

        if (chessBoard[0, 2] == chessBoard[1, 1] && chessBoard[0, 2] == chessBoard[2, 0] && chessBoard[0, 2] != 0) return chessBoard[0, 2];

        //Cteate count to find out who win the game.
        //If count == 9, then nobody win the game, the game go equally.
        int count = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (chessBoard[i, j] != 0)// some player put chess on that button.
                {
                    count++;
                }
            }
        }
        if (count == 9)
        {
            return 0;
        }
        return 3; //3
    }

}