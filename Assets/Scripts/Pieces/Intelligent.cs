using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicative : BasePiece {
    public override void Setup(int newTeamNum, AttackRange newAttackRange, PieceManager newPieceManager) {
        base.Setup(newTeamNum, newAttackRange, newPieceManager);
        
        mCommunication = 4;
    }

    protected override void LevelUp() {
        base.LevelUp();

        AddAttackPower(AttackType.Communication);
    }
}
