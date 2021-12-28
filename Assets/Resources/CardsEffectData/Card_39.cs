using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_39 : PieceChange {
	public Card_39(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Charm, 3);
	}
}