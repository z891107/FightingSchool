using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : UIUpdater {
    public PieceManager mPieceManager;

    String mBuildButtonImagePathPrefix = "UI/Build/Build_";

    List<int> teamNumMapping = new List<int>{
        4, 3, 1, 2
    };

	public override void UpdateUI() {
        int teamNum = teamNumMapping[mPieceManager.mCurrentSelectedPiece.mTeamNum];

        UpdateImage("BuildingButton1", mBuildButtonImagePathPrefix + teamNum + "_1");
        UpdateImage("BuildingButton2", mBuildButtonImagePathPrefix + teamNum + "_2");
        UpdateImage("BuildingButton3", mBuildButtonImagePathPrefix + teamNum + "_3");
	}
}
