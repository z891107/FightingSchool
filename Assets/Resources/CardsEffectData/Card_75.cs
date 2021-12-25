using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_75 : PieceChangeAttr {
	public Card_75(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.AddAttackPower(AttackType.Communication, 1);
	}
}