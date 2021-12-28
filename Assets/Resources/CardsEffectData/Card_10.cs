using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_10 : PieceChange {
	public Card_10(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		mTurnToInvokeDelayedCallback = 4;
		mDelayedCallback = (piece) => {
			piece.AddAttackPower(AttackType.Richness, 5);
		};
	}
}
