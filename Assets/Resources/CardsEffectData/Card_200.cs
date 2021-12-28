using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_200 : PieceChangePosition {
	public Card_200(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece, Cell cell) {
		Vector2Int position;
		do {
			int x = UnityEngine.Random.Range(0, 16);
			int y = UnityEngine.Random.Range(0, 16);
			position = new Vector2Int(x, y);
		} while (mCardManager.mBoard.ValidateCell(position) != CellState.Free);

		cell = mCardManager.mBoard.mCells[position.x, position.y];
		piece.Place(cell);
	}

	public override bool ShowRange() {
		return false;
	}
}