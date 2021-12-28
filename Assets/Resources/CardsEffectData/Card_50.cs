using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_50 : PieceChange {
	public Card_50(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mEnergy -= 1;
		piece.AddAttackPower(AttackType.Strength, -3);
		piece.AddAttackPower(AttackType.Richness, 5);
	}
}