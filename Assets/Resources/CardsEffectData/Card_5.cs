using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_5 : SelfUsing {
	public Card_5(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mSpeed += 1;
		piece.mEnergy += 1;
		piece.mStrength -= 2;
		piece.mCharm -= 2;
		piece.mCommunication -= 2;
		piece.mRichness -= 2;
	}
}