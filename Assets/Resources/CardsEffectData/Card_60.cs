using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_60 : PieceChange {
	public Card_60(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Strength, 6);
	}
}