using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_21 : PieceChangeAttr {
	public Card_21(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mStrength += 1;
	}
}