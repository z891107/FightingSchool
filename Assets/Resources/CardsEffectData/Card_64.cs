using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Card_64 : PieceChangeAttr {
	public Card_64(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Richness, -3);

		Array values = Enum.GetValues(typeof(AttackType));
		AttackType addType = (AttackType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
		piece.AddAttackPower(addType, 2);
	}
}