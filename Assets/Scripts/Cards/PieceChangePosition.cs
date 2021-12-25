using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceChangePosition : CardEffectData {
	public PieceChangePosition(int id, CardManager cardManager) : base(id, cardManager) { }

	BasePiece mPiece;
	Cell mCell;

	public Action<BasePiece, Cell> mDelayedCallback;

	public abstract void ActionCallback(BasePiece piece, Cell position);

	public override bool Action() {
		if (mCardManager.mCurrentSelectedCell.Count >= 1) {
			mPiece = mCardManager.mPieceManager.mCurrentSelectedPiece;
			mCell = mCardManager.mCurrentSelectedCell[0];

			ActionCallback(mPiece, mCell);
		}

		return true;
	}

	public override void DelayedAction() {
		mDelayedCallback(mPiece, mCell);
	}
}
