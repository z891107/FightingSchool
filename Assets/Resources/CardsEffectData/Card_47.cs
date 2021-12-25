using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_47 : PieceChangeAttr {
	public Card_47(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Richness, -3);
		piece.mSpeed += 2;
	}
}