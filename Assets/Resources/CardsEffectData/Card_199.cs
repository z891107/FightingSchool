using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_199 : PieceChangePosition {
	public Card_199(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece, Cell cell) {
		piece.Place(cell);
	}

	public override bool ShowRange() {
		mCardManager.mBoard.ShowOutlineCells(new Vector2Int(0, 0), new Vector2Int(15, 15), true);

		return true;
	}
}