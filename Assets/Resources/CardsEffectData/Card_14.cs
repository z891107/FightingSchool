using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_14 : PieceChange {
	public Card_14(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Communication, 2);
	}
}