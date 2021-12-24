using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedy : BasePiece {
    public override void Setup(int newTeamNum, AttackRange newAttackRange, PieceManager newPieceManager) {
        base.Setup(newTeamNum, newAttackRange, newPieceManager);
        
        mSpeed = 4;
    }
}
