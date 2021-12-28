using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_29 : PieceChange {
	public Card_29(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Richness, 2);
	}
}