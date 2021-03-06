using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_23 : PieceChange {
	public Card_23(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Communication, 1);
		piece.AddAttackPower(AttackType.Richness, -piece.mRichness);
	}
}