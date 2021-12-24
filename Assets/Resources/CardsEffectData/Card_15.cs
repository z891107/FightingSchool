using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_15 : SelfUsing {
	public Card_15(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mStrength -= 2;
		piece.mCharm -= 2;
		piece.mCommunication -= 2;
		piece.mRichness -= 2;
	}
}