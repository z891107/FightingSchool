using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_9 : PieceChange {
	public Card_9(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Richness, 4);
	}
}