using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_34 : PieceChangeAttr {
	public Card_34(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mCurrentHealth -= 7;
		piece.mStrength += 2;
		piece.mCharm += 2;
		piece.mCommunication += 2;
		piece.mRichness += 2;
	}
}