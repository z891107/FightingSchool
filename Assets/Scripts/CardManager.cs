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
    public List<BasePiece> mCurrentSelectedPiece;
    bool mIsUsingCard = false;

    Dictionary<Card, int> mTurnToInvokeCardsCallback = new Dictionary<Card, int>();

    void Awake() {
        mGameManager.GoBackButtonPressed += OnGoBackButtonPressed;
        mBoard.OutlineCellClicked += OnOutlineCellClicked;
        mTurnHandler.NextTurn += OnNextTurn;
    }
    
    void MakeCards() {
        for (int i = 1; i <= 6; i++) {
            CardEffectData effectData = (CardEffectData)Activator.CreateInstance(Type.GetType("Card_" + i), new object[] {i - 1, this});
            mAllCardsEffectData.Add(effectData);
        }

        CardEffectData effect = (CardEffectData)Activator.CreateInstance(Type.GetType("Card_10"), new object[] {9, this});
        mAllCardsEffectData.Add(effect);
        effect = (CardEffectData)Activator.CreateInstance(Type.GetType("Card_13"), new object[] {12, this});
        mAllCardsEffectData.Add(effect);
        effect = (CardEffectData)Activator.CreateInstance(Type.GetType("Card_16"), new object[] {15, this});
        mAllCardsEffectData.Add(effect);
    }

    public void Init() {
        MakeCards();

        for (int i = 0; i < 6; i++) {
        GameObject newPieceObject = Instantiate(mCardPrefab, mCanvas.transform);

        Card newPiece = newPieceObject.AddComponent<Card>();

        newPiece.MouseOver += mUIHandler.OnCardMouseOver;
        newPiece.MouseOut += mUIHandler.OnCardMouseOut;
        newPiece.EndDrag += mUIHandler.OnCardEndDrag;

        mCards.Add(newPiece);

        newPiece.Setup(mAllCardsEffectData[i], this, mPlacingCardRectTransform);
        }

        {
        GameObject newPieceObject = Instantiate(mCardPrefab, mCanvas.transform);

        Card newPiece = newPieceObject.AddComponent<Card>();

        newPiece.MouseOver += mUIHandler.OnCardMouseOver;
        newPiece.MouseOut += mUIHandler.OnCardMouseOut;
        newPiece.EndDrag += mUIHandler.OnCardEndDrag;

        mCards.Add(newPiece);

        newPiece.Setup(mAllCardsEffectData[6], this, mPlacingCardRectTransform);
        }
        {
        GameObject newPieceObject = Instantiate(mCardPrefab, mCanvas.transform);

        Card newPiece = newPieceObject.AddComponent<Card>();

        newPiece.MouseOver += mUIHandler.OnCardMouseOver;
        newPiece.MouseOut += mUIHandler.OnCardMouseOut;
        newPiece.EndDrag += mUIHandler.OnCardEndDrag;

        mCards.Add(newPiece);

        newPiece.Setup(mAllCardsEffectData[7], this, mPlacingCardRectTransform);
        }
        {
        GameObject newPieceObject = Instantiate(mCardPrefab, mCanvas.transform);

        Card newPiece = newPieceObject.AddComponent<Card>();

        newPiece.MouseOver += mUIHandler.OnCardMouseOver;
        newPiece.MouseOut += mUIHandler.OnCardMouseOut;
        newPiece.EndDrag += mUIHandler.OnCardEndDrag;

        mCards.Add(newPiece);

        newPiece.Setup(mAllCardsEffectData[8], this, mPlacingCardRectTransform);
        }
    }

    public void SelectCard(Card card) {
        mCurrentSelectedCard = card;
    }

    public void UnSelectCard(Card card) {
        if (card == mCurrentSelectedCard){
            mCurrentSelectedCard = null;
        }
    }

    public void OnConfirmUsingCardButtonPressed() {
        if (mCurrentSelectedCard) {
            var position = mPieceManager.mCurrentSelectedPiece.mCurrentCell.mBoardPosition;
            var attackRange = mPieceManager.mCurrentSelectedPiece.mAttackRange;
            mBoard.ShowOutlineCells(position, attackRange);

            mIsUsingCard = true;
            mCurrentSelectedPiece.Clear();
        }
    }

    void OnOutlineCellClicked(object sender, EventArgs e) {
        Cell cell = sender as Cell;

        if (mIsUsingCard && cell.mCurrentPiece) {
            mCurrentSelectedPiece.Add(cell.mCurrentPiece);

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
