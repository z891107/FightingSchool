using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_26 : PieceChangeAttr {
	public Card_26(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mRichness -= 3;
	}
}