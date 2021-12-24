using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_36 : SelfUsing {
	public Card_36(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mStrength -= 2;
		piece.mCharm += 2;
	}
}