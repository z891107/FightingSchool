using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_4 : PieceChange
{
    public Card_4(int id, CardManager cardManager) : base(id, cardManager) {}

    public override void ActionCallback(BasePiece piece) {
        piece.mCurrentHealth += 2;
        piece.AddAttackPower(AttackType.Richness, 3);
    }
}