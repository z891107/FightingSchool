using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelfUsing : CardEffectData {
    public SelfUsing(int id, CardManager cardManager) : base(id, cardManager) {}

    BasePiece mPiece;

    public Action<BasePiece> mDelayedCallback;

    public abstract void ActionCallback(BasePiece piece);

    public override bool Action() {
        if (mCardManager.mCurrentSelectedPiece.Count >= 1) {
            ActionCallback(mCardManager.mCurrentSelectedPiece[0]);
        }

        mPiece = mCardManager.mCurrentSelectedPiece[0];
        
        return true;
    }

    public override void DelayedAction() {
        mDelayedCallback(mPiece);
    }
}
