using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthy : BasePiece {
    public override void Setup(int newTeamNum, int newId, AttackRange newAttackRange, PieceManager newPieceManager) {
        base.Setup(newTeamNum, newId, newAttackRange, newPieceManager);
        
        mHealth += 2;
        mCurrentHealth += 2;
    }
}
