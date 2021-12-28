using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerful : BasePiece {
    public override void Setup(int newTeamNum, int newId, AttackRange newAttackRange, PieceManager newPieceManager) {
        base.Setup(newTeamNum, newId, newAttackRange, newPieceManager);
        
        mStrength = 4;
    }

    protected override bool LevelUp() {
        if (base.LevelUp()) {
            AddAttackPower(AttackType.Strength);
        }

        return true;
    }
}
