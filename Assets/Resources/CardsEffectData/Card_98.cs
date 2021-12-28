using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Card_98 : PieceChange {
	public Card_98(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		Array values = Enum.GetValues(typeof(AttackType));
		AttackType subType = (AttackType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
		piece.AddAttackPower(subType, -1);

		piece.mEnergy += 1;
		piece.mCurrentEnergy += 1;
		
		mTurnToInvokeDelayedCallback = 4;
        mDelayedCallback = (piece) => {
            piece.mEnergy -= 1;
        };
	}
}