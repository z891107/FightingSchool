using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_152 : PieceChange {
	public Card_152(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mCurrentHealth -= 3;
	}
}