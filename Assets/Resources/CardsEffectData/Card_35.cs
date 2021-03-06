using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_35 : PieceChange {
	public Card_35(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Communication, -2);
		piece.mCurrentHealth += 3;
	}
}