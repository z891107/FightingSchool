using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_59 : PieceChangeAttr {
	public Card_59(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mSpeed += 2;
	}
}