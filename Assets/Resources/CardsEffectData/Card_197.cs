using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_197 : PieceChangePosition {
	public Card_197(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece, Cell cell) {
		piece.Place(cell);
	}

	public override bool ShowRange() {
		var position = mCardManager.mPieceManager.mCurrentSelectedPiece.mCurrentCell.mBoardPosition;
		mCardManager.mBoard.ShowOutlineCellsInCross(position, true);

		return true;
	}
}