using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_10 : PieceChangeAttr {
	public Card_10(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		mTurnToInvokeDelayedCallback = 4;
		mDelayedCallback = (piece) => {
			piece.mRichness += 5;
		};
	}
}
