using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_74 : PieceChange {
	public Card_74(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Strength, -3);
		piece.AddAttackPower(AttackType.Charm, 3);
	}
}