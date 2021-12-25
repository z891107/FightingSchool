using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_16 : PieceChangeAttr {
	public Card_16(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mStrength += 1;
		piece.mCharm += 1;
		piece.mCommunication += 1;
		piece.mRichness += 1;
	}
}