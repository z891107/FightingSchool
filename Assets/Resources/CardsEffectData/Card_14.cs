using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_14 : PieceChangeAttr {
	public Card_14(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece) {
		piece.mCommunication += 2;
	}
}