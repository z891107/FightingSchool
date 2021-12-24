using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Functional : MonoBehaviour {
    public PieceManager mPieceManager;
    public TurnHandler mTurnHandler;

    Color32[] mTeamColors = {
        new Color32(163, 31, 52, 255),
        new Color32(26, 65, 136, 255),
        new Color32(197, 166, 54, 255),
        new Color32(114, 31, 110, 255),
    };

    public Image mTeamColor;

    public Image[] mTeamSequence = new Image[4];

    public void UpdateFunctional() {
        mTeamColor.color = mTeamColors[mTurnHandler.mCurrentTeamNum];

        for (int i = 0; i < mTeamSequence.Length; i++) {
            mTeamSequence[i].color = mTeamColors[mTurnHandler.mCurrentTeamSequence[i]];
        }
    }
}
