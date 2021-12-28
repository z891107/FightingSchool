using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_198 : PieceChangePosition {
	public Card_198(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece, Cell cell) {
		piece.Place(cell);
	}

	public override bool ShowRange() {
		var position = mCardManager.mPieceManager.mCurrentSelectedPiece.mCurrentCell.mBoardPosition;
		mCardManager.mBoard.ShowOutlineCellsRoundAllPiece(position, true);

		return true;
	}
}