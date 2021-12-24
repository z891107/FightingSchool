using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_19 : SelfUsing {
	public Card_19(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mRichness += 2;
	}
}