using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_34 : PieceChange {
	public Card_34(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mCurrentHealth -= 7;
		piece.AddAttackPower(AttackType.Strength, 2);
		piece.AddAttackPower(AttackType.Charm, 2);
		piece.AddAttackPower(AttackType.Communication, 2);
		piece.AddAttackPower(AttackType.Richness, 2);
	}
}