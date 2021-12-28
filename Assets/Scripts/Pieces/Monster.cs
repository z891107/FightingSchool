using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Monster : BasePiece {
    String mSpritePathPrefix = "UI/Bears/bear_";

    public override void Setup(int newTeamNum, int newId, AttackRange newAttackRange, PieceManager newPieceManager) {
        base.Setup(newTeamNum, newId, newAttackRange, newPieceManager);
        
        mStrength = 1;
        mCharm = 1;
        mCommunication = 1;
        mRichness = 1;
        mSpeed = 0;
        mEnergy = 0;
        mCurrentEnergy = 0;
    }

    protected override bool LevelUp() {
        mLevel++;
        if (mLevel <= 26) {
            GetComponent<Image>().sprite = Resources.Load<Sprite>(mSpritePathPrefix + mLevel);
        }

        Array values = Enum.GetValues(typeof(AttackType));

        AttackType attackType = (AttackType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        AddAttackPower(attackType);

        return true;
    }
}
