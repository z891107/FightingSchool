using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_16 : PieceChange {
	public Card_16(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Strength, 1);
		piece.AddAttackPower(AttackType.Charm, 1);
		piece.AddAttackPower(AttackType.Communication, 1);
		piece.AddAttackPower(AttackType.Richness, 1);
	}
}