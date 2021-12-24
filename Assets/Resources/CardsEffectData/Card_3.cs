using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_3 : SelfUsing
{
    public Card_3(int id, CardManager cardManager) : base(id, cardManager) {}

    public override void ActionCallback(BasePiece piece) {
        piece.mRichness -= 1;

        mTurnToInvokeDelayedCallback = 4;
        mDelayedCallback = (piece) => {
            piece.mRichness += 3;
        };
    }
}
