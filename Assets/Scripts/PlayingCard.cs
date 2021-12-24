using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCard : UIUpdater
{
    public CardManager mCardManager;

    String mCardImagePathPrefix = "UI/Cards/Card_";

    public override void UpdateUI() {
        Card currentSelectedCard = mCardManager.mCurrentSelectedCard;
        if (currentSelectedCard) {
            UpdateImage("CardImage", mCardImagePathPrefix + (currentSelectedCard.mEffectData.mId + 1).ToString());
            UpdateSlot("CardSlot", currentSelectedCard.gameObject);
        }
        else {
            ClearImage("CardImage");
        }
    }
}
