using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public enum UIAlignment {
	LowerLeft = 0,
	UpperLeft,
	UpperRight,
	LowerRight,
}

public class UIHandler : MonoBehaviour {
	public event EventHandler GUIReseted;

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
    public GameObject mPlayingCardPanel;
	public GameObject mSpawningPiecePanel;
	public GameObject mFunctionalPanel;
	public GameObject mSelectedPieceInfoPanel;
	public GameObject mMouseOverPieceInfoPanel;
	public GameObject mEmptyCardPanel;
	public GameObject mCardAndResourcePanel;
	public GameObject mChangePanel;
	public GameObject mHoverInfoPanel;
	public GameObject mAttackInfoPanel;
	public GameObject mBuildInfoPanel;
	public GameObject mChooseInfoPanel;
	public GameObject mDeckInfoPanel;
	public GameObject mMemberInfoPanel;
	public GameObject mMoveInfoPanel;
	public GameObject mPlayCardInfoPanel;
	public GameObject mRecruitInfoPanel;
	public GameObject mResourceInfoPanel;
	public GameObject mSleepInfoPanel;
    public GameObject mWinPanel;

	public RectTransform mPlacingCardRectTransform;

	GameObject[] mPreviouslyDisplayedPanel = new GameObject[4];
	GameObject[] mDefaultPanel = new GameObject[4];

	Vector3[] uiPositions = {
	new Vector3(28, 28, 0),
	new Vector3(28, 552, 0),
	new Vector3(1500, 552, 0),
	new Vector3(1500, 28, 0),
    };

	void Awake() {
		mGameManager.GoBackButtonPressed += OnGoBackButtonPressed;
		mTurnHandler.NextTurn += OnNextTurn;
		mTurnHandler.RefreshTeamSequence += OnRefreshTeamSequence;
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

	void SetDefaultGUI(GameObject panel, UIAlignment alignment) {
		mDefaultPanel[(int)alignment] = panel;
	}

	void ShowDefaultGUI() {
		ShowDefaultGUI(UIAlignment.LowerLeft);
		ShowDefaultGUI(UIAlignment.UpperLeft);
		ShowDefaultGUI(UIAlignment.UpperRight);
		ShowDefaultGUI(UIAlignment.LowerRight);
	}

	public void ShowDefaultInfo() {
		ShowDefaultGUI(UIAlignment.LowerLeft);
	}

	public void ShowHoverInfoPanel() {
		ShowGUI(mHoverInfoPanel, UIAlignment.LowerLeft);
	}

	void ShowDefaultGUI(UIAlignment alignment) {
		ShowGUI(mDefaultPanel[(int)alignment], alignment);
	}

	void SetAndShowDefaultGUI(GameObject panel, UIAlignment alignment) {
		SetDefaultGUI(panel, alignment);
		ShowDefaultGUI(alignment);
	}

	public void AdaptScreenResolution() {
		uiPositions[(int)UIAlignment.LowerLeft] = mEmptyCardPanel.transform.position;
		uiPositions[(int)UIAlignment.UpperLeft] = mFunctionalPanel.transform.position;
		uiPositions[(int)UIAlignment.UpperRight] = mCardAndResourcePanel.transform.position;
		uiPositions[(int)UIAlignment.LowerRight] = mSpawnPiecePanel.transform.position;
	}

	public void ResetGUI() {
		SetDefaultGUI(mSpawnPiecePanel, UIAlignment.LowerRight);
		SetDefaultGUI(mCardAndResourcePanel, UIAlignment.UpperRight);
		SetDefaultGUI(mFunctionalPanel, UIAlignment.UpperLeft);
		SetDefaultGUI(mEmptyCardPanel, UIAlignment.LowerLeft);
		ShowDefaultGUI();

		mSpawnPiecePanel.GetComponent<SpawnPiece>().UpdateSpawnPiece();

		GUIReseted(this, EventArgs.Empty);
	}

	public void ShowGUI(GameObject showingPanel, UIAlignment alignment) {
		ClearGUI(alignment);
		ActivateGUI(showingPanel, alignment);
	}

    public void ShowBlackTransitions() {
        mChangePanel.SetActive(true);
        mFunctionalPanel.transform.position = new Vector3(763, 368, 0);
        mFunctionalPanel.transform.Find("EndGameButton").gameObject.SetActive(false);
        mFunctionalPanel.transform.Find("ElseFunctionButton").gameObject.SetActive(false);
    }

    public void ClearBlackTransitions() {
        mChangePanel.SetActive(false);
        ShowGUI(mFunctionalPanel, UIAlignment.UpperLeft);
        mFunctionalPanel.transform.Find("EndGameButton").gameObject.SetActive(true);
        mFunctionalPanel.transform.Find("ElseFunctionButton").gameObject.SetActive(true);
    }

    public void ShowWinPanel(int winTeamNum) {
        String color = UnityEngine.ColorUtility.ToHtmlStringRGBA(mSelectedPieceInfoPanel.GetComponent<PieceInfo>().mTeamColors[winTeamNum]);
        String name = mSelectedPieceInfoPanel.GetComponent<PieceInfo>().mTeamTexts[winTeamNum];

        mWinPanel.SetActive(true);
        mWinPanel.transform.Find("Text").GetComponent<Text>().text = "獲勝的是" + "<color=#" + color + ">" + name + "</color>";
    }

	public void UpdateData() {

	}

	public void OnMouseOverEmptyCardPanel() {
		ShowGUI(mHoverInfoPanel, UIAlignment.LowerLeft);
	}

	public void OnAllyPieceSelected(object sender, EventArgs e) {
		ShowGUI(mPieceActionPanel, UIAlignment.LowerRight);
		ShowGUI(mSelectedPieceInfoPanel, UIAlignment.UpperLeft);
		SetAndShowDefaultGUI(mMemberInfoPanel, UIAlignment.LowerLeft);

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
		ShowDefaultGUI(UIAlignment.LowerLeft);
	}

	public void OnGoBackButtonPressed(object sender, EventArgs e) {
		ResetGUI();
	}

	public void OnRefreshTeamSequence(object sender, EventArgs e) {
		ShowGUI(mFunctionalPanel, UIAlignment.UpperLeft);
	}
	public void OnNextTurn(object sender, EventArgs e) {
		ResetGUI();
		ShowBlackTransitions();
	}
	public void OnSwitchPlayerButtonPressed() {
		ClearBlackTransitions();
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
		SetAndShowDefaultGUI(mAttackInfoPanel, UIAlignment.LowerLeft);
	}

	public void OnAttackTypeButtonPressed() {
		ShowGUI(mAttackingPanel, UIAlignment.LowerRight);
	}

	public void OnMoveButtonPressed() {
		ShowGUI(mMovingPanel, UIAlignment.LowerRight);
		SetAndShowDefaultGUI(mMoveInfoPanel, UIAlignment.LowerLeft);
	}

	public void OnBuildButtonPressed() {
		ShowGUI(mBuildPanel, UIAlignment.LowerRight);
		SetAndShowDefaultGUI(mBuildInfoPanel, UIAlignment.LowerLeft);
	}

	public void OnPlayCardButtonPressed() {
		ShowGUI(mPlayCardPanel, UIAlignment.LowerRight);
		SetAndShowDefaultGUI(mPlayCardInfoPanel, UIAlignment.LowerLeft);
	}

    public void OnConfirmUsingCardButtonPressed() {
        ShowGUI(mPlayingCardPanel, UIAlignment.LowerRight);
    }

	public void OnSpawnPieceButtonClicked(GameObject sender) {
		GameObject parent = sender.transform.parent.gameObject;
		OnSpawnPieceButtonMouseLeave(parent);

		ShowGUI(mSpawningPiecePanel, UIAlignment.LowerRight);
		SetAndShowDefaultGUI(mRecruitInfoPanel, UIAlignment.LowerLeft);
	}

	public void OnMouseOverSelectedPieceInfoPanel() {
		ShowGUI(mChooseInfoPanel, UIAlignment.LowerLeft);
	}

	public void OnMouseOverDeck() {
		ShowGUI(mDeckInfoPanel, UIAlignment.LowerLeft);
	}

	public void OnMouseOverSpawnPiecePanel() {
		ShowGUI(mRecruitInfoPanel, UIAlignment.LowerLeft);
	}

	public void OnMouseOverResource() {
		ShowGUI(mResourceInfoPanel, UIAlignment.LowerLeft);
	}

	public void OnMouseOverSleepButton() {
		ShowGUI(mSleepInfoPanel, UIAlignment.LowerLeft);
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

    public void UpdatePieceVisual(BasePiece piece, BasePiece piece2) {
        PieceInfo[] pieceInfos = { mSelectedPieceInfoPanel.GetComponent<PieceInfo>(),
                                mMouseOverPieceInfoPanel.GetComponent<PieceInfo>() };

        for (int i = 0; i < pieceInfos.Length; i++) {
            PieceInfo pieceInfo = pieceInfos[i];

            pieceInfo.UpdatePieceVisual(piece, piece2);
        }
    }

    public void AddPieceVisual(BasePiece piece) {
        PieceInfo[] pieceInfos = { mSelectedPieceInfoPanel.GetComponent<PieceInfo>(),
                                mMouseOverPieceInfoPanel.GetComponent<PieceInfo>() };

        for (int i = 0; i < pieceInfos.Length; i++) {
            PieceInfo pieceInfo = pieceInfos[i];

            pieceInfo.AddPieceVisual(piece);
        }
    }
}
