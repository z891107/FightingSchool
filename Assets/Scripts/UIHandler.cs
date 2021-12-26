using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public enum UIAlignment {
    LowerLeft = 0,
    UpperLeft,
    UpperRight,
    LowerRight,
}

public class UIHandler : MonoBehaviour
{
    public GameManager mGameManager;
    public PieceManager mPieceManager;
    public Board mBoard;
    public TurnHandler mTurnHandler;
    public CardManager mCardManager;

    public GameObject mSpawnPiecePanel;
    public GameObject mPieceActionPanel;
    public GameObject mMovingPanel;
    public GameObject mPlayCardPanel;
    public GameObject mBuildPanel;
    public GameObject mAttackTypePanel;
    public GameObject mAttackingPanel;
    public GameObject mSpawningPiecePanel;
    public GameObject mFunctionalPanel;
    public GameObject mSelectedPieceInfoPanel;
    public GameObject mMouseOverPieceInfoPanel;
    public GameObject mEmptyCardPanel;
    public GameObject mCardAndResourcePanel;
    public GameObject mChangePanel;

    public RectTransform mPlacingCardRectTransform;

    GameObject[] mPreviouslyDisplayedPanel = new GameObject[4];

    Vector3[] uiPositions = {
        new Vector3(28, 28, 0),
        new Vector3(28, 552, 0),
        new Vector3(1500, 552, 0),
        new Vector3(1500, 28, 0),
    };

    void Awake() {
        mGameManager.GoBackButtonPressed += OnGoBackButtonPressed;
        mTurnHandler.NextTurn += OnNextTurn;
        mTurnHandler.EndOfRound += OnEndOfRound;
        mCardManager.CardUsed += OnCardUsed;
    }

    void ClearGUI() {
        ClearGUI(UIAlignment.LowerLeft);
        ClearGUI(UIAlignment.UpperLeft);
        ClearGUI(UIAlignment.UpperRight);
        ClearGUI(UIAlignment.LowerRight);
    }

    void ClearGUI(UIAlignment alignment) {
        if (mPreviouslyDisplayedPanel[(int)alignment]) {
            mPreviouslyDisplayedPanel[(int)alignment].SetActive(false);
        }
    }

    void ActivateGUI(GameObject panel, UIAlignment alignment) {
        panel.GetComponent<RectTransform>().position = uiPositions[(int)alignment];

        panel.SetActive(true);
        mPreviouslyDisplayedPanel[(int)alignment] = panel;

        UIUpdater updater = panel.GetComponent<UIUpdater>();
        if (updater) {
            updater.UpdateUI();
        }
    }

    public void AdaptScreenResolution() {
        uiPositions[(int)UIAlignment.LowerLeft] = mEmptyCardPanel.transform.position;
        uiPositions[(int)UIAlignment.UpperLeft] = mFunctionalPanel.transform.position;
        uiPositions[(int)UIAlignment.UpperRight] = mCardAndResourcePanel.transform.position;
        uiPositions[(int)UIAlignment.LowerRight] = mSpawnPiecePanel.transform.position;
    }

    public void ResetGUI() {
        mChangePanel.SetActive(false);

        ShowGUI(mSpawnPiecePanel, UIAlignment.LowerRight);
        ShowGUI(mCardAndResourcePanel, UIAlignment.UpperRight);
        //mCardAndResourcePanel.GetComponent<RectTransform>().sizeDelta *= (float)1.25;
        ShowGUI(mFunctionalPanel, UIAlignment.UpperLeft);
        ShowGUI(mEmptyCardPanel, UIAlignment.LowerLeft);

        mSpawnPiecePanel.GetComponent<SpawnPiece>().UpdateSpawnPiece();
        mFunctionalPanel.GetComponent<Functional>().UpdateFunctional();
    }

    public void ShowGUI(GameObject showingPanel, UIAlignment alignment) {
        ClearGUI(alignment);
        ActivateGUI(showingPanel, alignment);
    }

    public void UpdateData() {
        
    }

    public void OnAllyPieceSelected(object sender, EventArgs e) {
        ShowGUI(mPieceActionPanel, UIAlignment.LowerRight);
        ShowGUI(mSelectedPieceInfoPanel, UIAlignment.UpperLeft);
        
        mPieceActionPanel.GetComponent<PieceAction>().UpdatePieceAction(sender as BasePiece);
        mSelectedPieceInfoPanel.GetComponent<PieceInfo>().UpdatePieceInfo(sender as BasePiece);
    }

    public void OnMouseOverPiece(object sender, EventArgs e) {
        ShowGUI(mMouseOverPieceInfoPanel, UIAlignment.LowerLeft);
        
        mMouseOverPieceInfoPanel.GetComponent<PieceInfo>().UpdatePieceInfo(sender as BasePiece);
    }

    public void UpdatePieceInfo(BasePiece selectedPiece, BasePiece mouseOverPiece) {
        mSelectedPieceInfoPanel.GetComponent<PieceInfo>().UpdatePieceInfo(selectedPiece);
        mMouseOverPieceInfoPanel.GetComponent<PieceInfo>().UpdatePieceInfo(mouseOverPiece);
    }

    public void OnMouseOutPiece(object sender, EventArgs e) {
        ShowGUI(mEmptyCardPanel, UIAlignment.LowerLeft);
    }

    public void OnGoBackButtonPressed(object sender, EventArgs e) {
        ResetGUI();
    }

    public void OnNextTurn(object sender, EventArgs e) {
        ResetGUI();
    }
    public void OnEndOfRound(object sender, EventArgs e) {
        mChangePanel.SetActive(true);
    }
    public void OnCardMouseOver(object sender, EventArgs e) {
        Card card = sender as Card;
        mCardAndResourcePanel.GetComponent<CardAndResource>().UpdateCardImage(card.mEffectData);
    }

    public void OnCardMouseOut(object sender, EventArgs e) {
        mCardAndResourcePanel.GetComponent<CardAndResource>().ClearCardImage();
    }

    public void OnCardEndDrag(object sender, EventArgs e) {
        mCardAndResourcePanel.GetComponent<UIUpdater>().UpdateUI();
        mPlayCardPanel.GetComponent<UIUpdater>().UpdateUI();
    }

    public void OnCardUsed(object sender, EventArgs e) {
        ResetGUI();
    }

    public void OnAttackButtonPressed() {
        ShowGUI(mAttackTypePanel, UIAlignment.LowerRight);
    }

    public void OnAttackTypeButtonPressed() {
        ShowGUI(mAttackingPanel, UIAlignment.LowerRight);
    }

    public void OnMoveButtonPressed() {
        ShowGUI(mMovingPanel, UIAlignment.LowerRight);
    }

    public void OnBuildButtonPressed() {
        ShowGUI(mBuildPanel, UIAlignment.LowerRight);
    }

    public void OnPlayCardButtonPressed() {
        ShowGUI(mPlayCardPanel, UIAlignment.LowerRight);
    }

    public void OnSpawnPieceButtonClicked(GameObject sender) {
        GameObject parent = sender.transform.parent.gameObject;
        OnSpawnPieceButtonMouseLeave(parent);

        ShowGUI(mSpawningPiecePanel, UIAlignment.LowerRight);
    }

    public void OnSpawnPieceButtonMouseOver(GameObject sender) {
        sender.transform.GetChild(0).gameObject.SetActive(false);
        sender.transform.GetChild(1).gameObject.SetActive(true);
        sender.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void OnSpawnPieceButtonMouseLeave(GameObject sender) {
        sender.transform.GetChild(0).gameObject.SetActive(true);
        sender.transform.GetChild(1).gameObject.SetActive(false);
        sender.transform.GetChild(2).gameObject.SetActive(false);
    }
}
