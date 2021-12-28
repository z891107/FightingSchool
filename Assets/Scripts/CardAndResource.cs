using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CardAndResource : UIUpdater
{
    public ResourceHandler mResourceHandler;
    public CardManager mCardManager;

    String mCardImagePathPrefix = "UI/Cards/Card_";

    public override void UpdateUI() {
        UpdateText("FoodValue", mResourceHandler.mCurrentTeamResource.food.ToString());
        UpdateText("IronValue", mResourceHandler.mCurrentTeamResource.iron.ToString());
        UpdateText("BookValue", mResourceHandler.mCurrentTeamResource.book.ToString());

        List<Card> cards = mCardManager.mCards;
        for (int i = 0; i < cards.Count; i++) {
            UpdateSlot(new String[]{"Deck", "CardSlot_" + (i + 1)}, cards[i].gameObject);
        }
    }

    public void UpdateCardImage(CardEffectData cardEffectData) {
        UpdateImage("CardImage", mCardImagePathPrefix + (cardEffectData.mId + 1).ToString());
    }

    public void ClearCardImage() {
        ClearImage("CardImage");
    }
}
