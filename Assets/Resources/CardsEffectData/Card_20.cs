using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_20 : PieceChangeAttr {
	public Card_20(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mEnergy -= 1;
		piece.AddAttackPower(AttackType.Richness, 8);
	}
}