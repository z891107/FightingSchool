using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class CardEffectData
{
    protected CardManager mCardManager;

    public int mId;
    public int mTurnToInvokeDelayedCallback = 0;

    public CardEffectData(int id, CardManager cardManager) {
        mId = id;
        mCardManager = cardManager;
    }

    public abstract bool Action();
    public abstract void DelayedAction();
    public abstract void ShowRange();
}
