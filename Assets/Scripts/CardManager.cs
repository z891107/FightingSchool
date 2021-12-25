using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
	public List<Card> mCards;

	public Card mCurrentSelectedCard;
	public List<Cell> mCurrentSelectedCell;
	bool mIsUsingCard = false;

	Dictionary<Card, int> mTurnToInvokeCardsCallback = new Dictionary<Card, int>();

	void Awake() {
		mGameManager.GoBackButtonPressed += OnGoBackButtonPressed;
		mBoard.OutlineCellClicked += OnOutlineCellClicked;
		mTurnHandler.NextTurn += OnNextTurn;
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



        {
			GameObject newPieceObject = Instantiate(mCardPrefab, mCanvas.transform);

			Card newPiece = newPieceObject.AddComponent<Card>();

			newPiece.MouseOver += mUIHandler.OnCardMouseOver;
			newPiece.MouseOut += mUIHandler.OnCardMouseOut;
			newPiece.EndDrag += mUIHandler.OnCardEndDrag;

			mCards.Add(newPiece);

			newPiece.Setup(mAllCardsEffectData[mAllCardsEffectData.Count - 4], this, mPlacingCardRectTransform);		//[mAllCardsEffectData.Count - 4] => [36](所要的卡片)
		}

		{
			GameObject newPieceObject = Instantiate(mCardPrefab, mCanvas.transform);

			Card newPiece = newPieceObject.AddComponent<Card>();

			newPiece.MouseOver += mUIHandler.OnCardMouseOver;
			newPiece.MouseOut += mUIHandler.OnCardMouseOut;
			newPiece.EndDrag += mUIHandler.OnCardEndDrag;

			mCards.Add(newPiece);

			newPiece.Setup(mAllCardsEffectData[mAllCardsEffectData.Count - 3], this, mPlacingCardRectTransform);
		}
		{
			GameObject newPieceObject = Instantiate(mCardPrefab, mCanvas.transform);

			Card newPiece = newPieceObject.AddComponent<Card>();

			newPiece.MouseOver += mUIHandler.OnCardMouseOver;
			newPiece.MouseOut += mUIHandler.OnCardMouseOut;
			newPiece.EndDrag += mUIHandler.OnCardEndDrag;

			mCards.Add(newPiece);

			newPiece.Setup(mAllCardsEffectData[mAllCardsEffectData.Count - 2], this, mPlacingCardRectTransform);
		}
		{
			GameObject newPieceObject = Instantiate(mCardPrefab, mCanvas.transform);

			Card newPiece = newPieceObject.AddComponent<Card>();

			newPiece.MouseOver += mUIHandler.OnCardMouseOver;
			newPiece.MouseOut += mUIHandler.OnCardMouseOut;
			newPiece.EndDrag += mUIHandler.OnCardEndDrag;

			mCards.Add(newPiece);

			newPiece.Setup(mAllCardsEffectData[mAllCardsEffectData.Count - 1], this, mPlacingCardRectTransform);
		}
	}

	public void SelectCard(Card card) {
		mCurrentSelectedCard = card;
	}

	public void UnSelectCard(Card card) {
		if (card == mCurrentSelectedCard) {
			mCurrentSelectedCard = null;
		}
	}

	public void OnConfirmUsingCardButtonPressed() {
		if (mCurrentSelectedCard) {
			mCurrentSelectedCard.ShowRange();

			mIsUsingCard = true;
			mCurrentSelectedCell.Clear();
		}
	}

	void OnOutlineCellClicked(object sender, EventArgs e) {
		Cell cell = sender as Cell;

		if (!mIsUsingCard) {
			return;
		}

		if (mCurrentSelectedCard.mEffectData as PieceChangeAttr == null ||
		cell.mCurrentPiece) {
			mCurrentSelectedCell.Add(cell);

			bool successful = mCurrentSelectedCard.Action();

			if (successful) {
				mIsUsingCard = false;

				if (mCurrentSelectedCard.mEffectData.mTurnToInvokeDelayedCallback > 0) {
					int round = mCurrentSelectedCard.mEffectData.mTurnToInvokeDelayedCallback;

					mTurnToInvokeCardsCallback.Add(mCurrentSelectedCard, round);
				}

				CardUsed(this, EventArgs.Empty);
			}
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
	}

	void OnGoBackButtonPressed(object sender, EventArgs e) {
		UnSelectCard(mCurrentSelectedCard);
	}
}
