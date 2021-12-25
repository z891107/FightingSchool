using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_203 : PieceChangeAttr {
	public Card_203(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mCurrentHealth -= 5;
	}
}