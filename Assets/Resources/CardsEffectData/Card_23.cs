using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_23 : PieceChangeAttr {
	public Card_23(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mCommunication += 1;
		piece.mRichness = 0;
	}
}