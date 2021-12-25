using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_17 : PieceChangeAttr {
	public Card_17(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mCharm += 10;
		piece.mCurrentHealth = 1;
	}
}