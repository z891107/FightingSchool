using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_36 : PieceChange {
	public Card_36(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Strength, -2);
		piece.AddAttackPower(AttackType.Charm, 2);
	}
}