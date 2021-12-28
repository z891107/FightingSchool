using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_150 : PieceChange {
	public Card_150(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Richness, -2);
	}
}