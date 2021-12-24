using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energetic : BasePiece {
    public override void Setup(int newTeamNum, AttackRange newAttackRange, PieceManager newPieceManager) {
        base.Setup(newTeamNum, newAttackRange, newPieceManager);
        
        mEnergy = 3;
        mCurrentEnergy = 3;
    }
}
