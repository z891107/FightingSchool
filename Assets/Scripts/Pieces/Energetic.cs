using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energetic : BasePiece {
    public override void Setup(int newTeamNum, int newId, AttackRange newAttackRange, PieceManager newPieceManager) {
        base.Setup(newTeamNum, newId, newAttackRange, newPieceManager);
        
        mEnergy = 3;
        mCurrentEnergy = 3;
    }
}
