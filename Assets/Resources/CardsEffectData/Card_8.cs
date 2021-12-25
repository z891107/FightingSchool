using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_8 : PieceChangeAttr {
	public Card_8(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mEnergy += 1;
		piece.mCurrentEnergy += 1;
		piece.AddAttackPower(AttackType.Charm, -piece.mCharm);
	}
}