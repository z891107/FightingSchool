using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Card_45 : PieceChangeAttr {
	public Card_45(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
        int communicationPoint = UnityEngine.Random.Range(-2, 3);

        Array values = Enum.GetValues(typeof(AttackType));
        AttackType attackType = (AttackType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        int attackPoint = UnityEngine.Random.Range(-2, 3);

        piece.AddAttackPower(AttackType.Richness, 5);
        piece.AddAttackPower(AttackType.Communication, communicationPoint);
		piece.AddAttackPower(attackType, attackPoint);
	}
}