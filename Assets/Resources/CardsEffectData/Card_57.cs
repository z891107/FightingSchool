using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Card_57 : PieceChange {
	public Card_57(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		Array values = Enum.GetValues(typeof(AttackType));
		AttackType addType = (AttackType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
		AttackType subType = (AttackType)values.GetValue(UnityEngine.Random.Range(0, values.Length));

		piece.AddAttackPower(addType, 1);
		piece.AddAttackPower(subType, -1);
	}
}