using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Charming : BasePiece {
    public override void Setup(int newTeamNum, AttackRange newAttackRange, PieceManager newPieceManager) {
        base.Setup(newTeamNum, newAttackRange, newPieceManager);
        
        mCharm = 4;
    }

    protected override void LevelUp() {
        base.LevelUp();

        AddAttackPower(AttackType.Charm);
    }
}
