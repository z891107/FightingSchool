using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_76 : PieceChange {
	public Card_76(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Charm, -5);
		piece.AddAttackPower(AttackType.Strength, 5);
	}
}