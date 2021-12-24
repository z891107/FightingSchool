using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_29 : SelfUsing {
	public Card_29(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mRichness += 2;
	}
}