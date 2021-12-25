using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_5 : PieceChangeAttr {
	public Card_5(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mSpeed += 1;
		piece.mEnergy += 1;
		piece.mCurrentEnergy += 1;
		piece.AddAttackPower(AttackType.Strength, -2);
		piece.AddAttackPower(AttackType.Charm, -2);
		piece.AddAttackPower(AttackType.Communication, -2);
		piece.AddAttackPower(AttackType.Richness, -2);
	}
}