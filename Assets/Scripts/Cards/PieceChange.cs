using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceChange : CardEffectData {
    public PieceChange(int id, CardManager cardManager) : base(id, cardManager) {}

    BasePiece mPiece;

    public Action<BasePiece> mDelayedCallback;

    public abstract void ActionCallback(BasePiece piece);

    public override bool Action() {
        if (mCardManager.mCurrentSelectedCell.Count >= 1) {
            mPiece = mCardManager.mCurrentSelectedCell[0].mCurrentPiece;

            ActionCallback(mPiece);
        }

        return true;
    }

    public override void DelayedAction() {
        mDelayedCallback(mPiece);
    }

    public override bool ShowRange() {
        var position = mCardManager.mPieceManager.mCurrentSelectedPiece.mCurrentCell.mBoardPosition;
        var attackRange = mCardManager.mPieceManager.mCurrentSelectedPiece.mAttackRange;
        mCardManager.mBoard.ShowOutlineCells(position, attackRange);

        return true;
    }
}
