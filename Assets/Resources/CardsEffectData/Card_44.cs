using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_44 : PieceChangeAttr {
	public Card_44(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mCharm -= 1;
		piece.mCurrentHealth += 3;
	}
}