using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Card_45 : SelfUsing {
	public Card_45(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
        int communicationPoint = UnityEngine.Random.Range(-2, 4);

        Array values = Enum.GetValues(typeof(AttackType));
        AttackType attackType = (AttackType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        int attackPoint = UnityEngine.Random.Range(-2, 4);

        piece.AddAttackPower(AttackType.Richness, 5);
        piece.AddAttackPower(AttackType.Communication, communicationPoint);
		piece.AddAttackPower(attackType, attackPoint);
	}
}