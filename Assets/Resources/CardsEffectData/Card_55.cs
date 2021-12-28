using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_55 : PieceChange {
	public Card_55(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mEnergy -= 1;
		piece.AddAttackPower(AttackType.Charm, 6);
	}
}