using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rich : BasePiece {
    public override void Setup(int newTeamNum, int newId, AttackRange newAttackRange, PieceManager newPieceManager) {
        base.Setup(newTeamNum, newId, newAttackRange, newPieceManager);
        
        mRichness = 4;
    }

    protected override bool LevelUp() {
        if (base.LevelUp()) {
            AddAttackPower(AttackType.Richness);
        }

        return true;
    }
}
