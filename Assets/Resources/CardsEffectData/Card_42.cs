using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_42 : SelfUsing {
	public Card_42(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mStrength += 2;
	}
}