using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_13 : SelfUsing {
	public Card_13(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mCharm -= 4;
	}
}