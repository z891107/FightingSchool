using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_2 : SelfUsing
{
    public Card_2(int id, CardManager cardManager) : base(id, cardManager) {}

    public override void ActionCallback(BasePiece piece) {
        piece.mHealth += 2;
    }
}
