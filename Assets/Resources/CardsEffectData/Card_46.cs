using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_46 : PieceChange {
	public Card_46(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mCurrentHealth -= 2;
	}
}