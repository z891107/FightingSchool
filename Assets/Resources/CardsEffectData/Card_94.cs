using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_94 : PieceChange {
	public Card_94(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Strength, -3);
		piece.AddAttackPower(AttackType.Charm, -3);
		piece.AddAttackPower(AttackType.Communication, -3);
		piece.AddAttackPower(AttackType.Richness, -3);

		mTurnToInvokeDelayedCallback = 4;
        mDelayedCallback = (piece) => {
            piece.AddAttackPower(AttackType.Strength, 3);
			piece.AddAttackPower(AttackType.Charm, 3);
			piece.AddAttackPower(AttackType.Communication, 3);
			piece.AddAttackPower(AttackType.Richness, 3);
        };
	}
}