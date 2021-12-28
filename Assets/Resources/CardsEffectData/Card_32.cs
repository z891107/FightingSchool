using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_32 : PieceChange {
	public Card_32(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mCurrentHealth -= 4;
		piece.AddAttackPower(AttackType.Charm, 4);
	}
}