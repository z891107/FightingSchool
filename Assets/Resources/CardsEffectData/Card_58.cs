using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_58 : PieceChange {
	public Card_58(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Richness, -3);
	}
}