using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_30 : PieceChangeAttr {
	public Card_30(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mStrength += 1;
	}
}