using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CardManager : MonoBehaviour {
	public event EventHandler CardUsed;

	public GameObject mCardPrefab;

	public GameObject mCanvas;
	public RectTransform mPlacingCardRectTransform;
	public UIHandler mUIHandler;
	public GameManager mGameManager;
	public PieceManager mPieceManager;
	public Board mBoard;
	public TurnHandler mTurnHandler;

	List<CardEffectData> mAllCardsEffectData = new List<CardEffectData>();
	List<Card>[] mTeamCards = {
	new List<Card>(),
	new List<Card>(),
	new List<Card>(),
	new List<Card>()
    };
	public List<Card> mCards {
		get {
			return mTeamCards[mTurnHandler.mCurrentTeamNum];
		}
	}

	public Card mCurrentSelectedCard;
	public List<Cell> mCurrentSelectedCell;
	bool mIsUsingCard = false;

	Dictionary<Card, int> mTurnToInvokeCardsCallback = new Dictionary<Card, int>();

	void Awake() {
		mGameManager.GoBackButtonPressed += OnGoBackButtonPressed;
		mBoard.OutlineCellClicked += OnOutlineCellClicked;
		mTurnHandler.NextTurn += OnNextTurn;
        mTurnHandler.NextRound += OnNextRound;
	}

	void MakeCards() {
		for (int i = 1; i <= 500; i++) {
			Type cardType = Type.GetType("Card_" + i);

			if (cardType != null) {
				CardEffectData effectData = (CardEffectData)Activator.CreateInstance(cardType, new object[] { i - 1, this });
				mAllCardsEffectData.Add(effectData);
			}
		}
	}

	public void Init() {
		MakeCards();
	}

	public Card RandomlyGenerateCard() {
		GameObject newCardObject = Instantiate(mCardPrefab, mCanvas.transform);
        newCardObject.SetActive(false);

		Card newCard = newCardObject.AddComponent<Card>();

		newCard.MouseOver += mUIHandler.OnCardMouseOver;
		newCard.MouseOut += mUIHandler.OnCardMouseOut;
		newCard.EndDrag += mUIHandler.OnCardEndDrag;

        int cardId = UnityEngine.Random.Range(0, mAllCardsEffectData.Count);
		newCard.Setup(mAllCardsEffectData[cardId], this, mPlacingCardRectTransform);

        return newCard;
	}

	public void SelectCard(Card card) {
		mCurrentSelectedCard = card;
	}

	public void UnSelectCard(Card card) {
		if (card == mCurrentSelectedCard) {
			mCurrentSelectedCard = null;
		}
	}

	public void UseCard() {
		bool successful = mCurrentSelectedCard.Action();

		if (successful) {
			mIsUsingCard = false;

			if (mCurrentSelectedCard.mEffectData.mTurnToInvokeDelayedCallback > 0) {
				int round = mCurrentSelectedCard.mEffectData.mTurnToInvokeDelayedCallback;

				mTurnToInvokeCardsCallback.Add(mCurrentSelectedCard, round);
			}

            mTeamCards[mTurnHandler.mCurrentTeamNum].Remove(mCurrentSelectedCard);

			CardUsed(this, EventArgs.Empty);
		}
	}

	public void OnConfirmUsingCardButtonPressed() {
		if (mCurrentSelectedCard) {
			var needSelect = mCurrentSelectedCard.ShowRange();

			if (!needSelect) {
				UseCard();

				return;
			}

			mIsUsingCard = true;
			mCurrentSelectedCell.Clear();
		}
	}

	void OnOutlineCellClicked(object sender, EventArgs e) {
		Cell cell = sender as Cell;

		if (!mIsUsingCard) {
			return;
		}

		if (mCurrentSelectedCard.mEffectData as PieceChange == null ||
		cell.mCurrentPiece) {
			mCurrentSelectedCell.Add(cell);

			UseCard();
		}
	}

	void OnNextTurn(object sender, EventArgs e) {
		List<Card> cards = new List<Card>(mTurnToInvokeCardsCallback.Keys);
		foreach (var card in cards) {
			if (--mTurnToInvokeCardsCallback[card] <= 0) {
				card.DelayedAction();
				mTurnToInvokeCardsCallback.Remove(card);
			}
		}

        for (int i = 0; i < 4; i++) {
            foreach (var card in mTeamCards[i]) {
                card.gameObject.SetActive(i == mTurnHandler.mCurrentTeamNum);
            }
        }
	}

    void OnNextRound(object sender, EventArgs e) {
		for (int i = 0; i < 4; i++) {
            if (mTeamCards[i].Count < 10) {
                mTeamCards[i].Add(RandomlyGenerateCard());
            }
        }
	}

	void OnGoBackButtonPressed(object sender, EventArgs e) {
		UnSelectCard(mCurrentSelectedCard);
	}
}
