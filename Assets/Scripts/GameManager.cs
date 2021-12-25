using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public event EventHandler GoBackButtonPressed;

    public Board mBoard;
    public PieceManager mPieceManager;
    public TurnHandler mTurnHandler;
    public ResourceHandler mResourceHandler;
    public CardManager mCardManager;
    public UIHandler mUIHandler;

    void Start()
    {
        mBoard.Create();        //把 16x16 的棋盤 生成出來

        mPieceManager.Setup(mBoard);    //生成初始 8隻角色

        mTurnHandler.GameStart();       //管理遊戲回合數

        mResourceHandler.Init();        //管理資源

        mCardManager.Init();        //管理卡片，生成初始 200張卡

        mUIHandler.AdaptScreenResolution();     //自適應解析度
        mUIHandler.ResetGUI();      //初始化 UI
    }

    void Update() {
        // Global Input Controll
        if (Mouse.current.rightButton.IsPressed()) {
            GoBackButtonPressed(this, EventArgs.Empty);
        }
    }

    public void Quit() {
        Application.Quit();
    }
}
