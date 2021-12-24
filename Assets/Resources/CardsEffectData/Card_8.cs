using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_8 : SelfUsing {
	public Card_8(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mEnergy += 1;
		piece.mCharm = 0;
	}
}