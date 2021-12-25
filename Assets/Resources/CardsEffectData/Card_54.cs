using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_54 : PieceChangeAttr {
	public Card_54(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mSpeed += 1;
	}
}