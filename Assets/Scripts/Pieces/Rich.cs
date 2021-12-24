using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rich : BasePiece {
    public override void Setup(int newTeamNum, AttackRange newAttackRange, PieceManager newPieceManager) {
        base.Setup(newTeamNum, newAttackRange, newPieceManager);
        
        mRichness = 4;
    }

    protected override void LevelUp() {
        base.LevelUp();

        AddAttackPower(AttackType.Richness);
    }
}
