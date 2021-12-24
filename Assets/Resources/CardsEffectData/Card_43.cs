using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_43 : SelfUsing {
	public Card_43(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mStrength += 4;
	}
}