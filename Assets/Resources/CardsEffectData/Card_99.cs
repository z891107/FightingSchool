using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_99 : PieceChange {
	public Card_99(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mEnergy += 1;
		piece.mCurrentEnergy += 1;

		mTurnToInvokeDelayedCallback = 4;
        mDelayedCallback = (piece) => {
            piece.mEnergy -= 1;
        };
	}
}