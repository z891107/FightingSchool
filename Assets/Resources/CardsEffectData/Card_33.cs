using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_33 : PieceChangeAttr {
	public Card_33(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mCharm += 5;
	}
}