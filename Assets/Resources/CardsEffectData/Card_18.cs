using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_18 : PieceChange {
	public Card_18(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Communication, 3);
	}
}