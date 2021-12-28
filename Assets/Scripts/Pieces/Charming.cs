using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Charming : BasePiece {
    public override void Setup(int newTeamNum, int newId, AttackRange newAttackRange, PieceManager newPieceManager) {
        base.Setup(newTeamNum, newId, newAttackRange, newPieceManager);
        
        mCharm = 4;
    }

    protected override bool LevelUp() {
        if (base.LevelUp()) {
            AddAttackPower(AttackType.Charm);
        }

        return true;
    }
}
