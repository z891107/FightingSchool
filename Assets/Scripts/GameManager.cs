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
        mBoard.Create();

        mPieceManager.Setup(mBoard);

        mTurnHandler.GameStart();

        mResourceHandler.Init();

        mCardManager.Init();

        mUIHandler.AdaptScreenResolution();
        mUIHandler.ResetGUI();
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
