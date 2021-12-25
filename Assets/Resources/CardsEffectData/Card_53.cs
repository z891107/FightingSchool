using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_53 : PieceChangeAttr {
	public Card_53(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Strength, -1);
		piece.AddAttackPower(AttackType.Charm, -1);
		piece.AddAttackPower(AttackType.Communication, -1);
		piece.AddAttackPower(AttackType.Richness, -1);
	}
}