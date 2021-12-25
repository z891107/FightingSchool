using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_1 : PieceChangeAttr
{
    public Card_1(int id, CardManager cardManager) : base(id, cardManager) {}

    public override void ActionCallback(BasePiece piece) {
        piece.AddAttackPower(AttackType.Charm, -3);
    }
}
