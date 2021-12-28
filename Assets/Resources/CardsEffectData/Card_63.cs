using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_63 : PieceChange {
	public Card_63(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Strength, 1);
	}
}