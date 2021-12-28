using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Functional : UIUpdater {
    public PieceManager mPieceManager;
    public TurnHandler mTurnHandler;

    Color32[] mTeamColors = {
        new Color32(163, 31, 52, 255),
        new Color32(26, 65, 136, 255),
        new Color32(197, 166, 54, 255),
        new Color32(114, 31, 110, 255),
    };

    public override void UpdateUI() {
        UpdateImageColor("CurrentTeam", mTeamColors[mTurnHandler.mCurrentTeamNum]);

        for (int i = 0; i < mTeamColors.Length; i++) {
            if (mTurnHandler.mCurrentTeamSequence[i] != -1) {
                UpdateImageColor("Team_" + (i + 1), mTeamColors[mTurnHandler.mCurrentTeamSequence[i]]);
            } else {
                UpdateImageColor("Team_" + (i + 1), Color.black);
            }
        }
    }
}
