using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_41 : SelfUsing {
	public Card_41(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mHealth += 5;
	}
}