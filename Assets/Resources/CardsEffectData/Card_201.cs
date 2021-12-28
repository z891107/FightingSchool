using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_201 : PieceChangePosition {
	public Card_201(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece, Cell cell) {
		piece.Place(cell);
	}

	public override bool ShowRange() {
        var position = mCardManager.mPieceManager.mCurrentSelectedPiece.mCurrentCell.mBoardPosition;
		mCardManager.mBoard.ShowOutlineCellsInAngledCross(position, true);

        return true;
	}
}