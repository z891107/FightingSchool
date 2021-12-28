using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_196 : PieceChangePosition {
	public Card_196(int id, CardManager cardManager) : base(id, cardManager) { }

	public override void ActionCallback(BasePiece piece, Cell cell) {
		piece.Place(cell);
	}

	public override bool ShowRange() {
		mCardManager.mPieceManager.OutlineTeamsSpawnCells(mCardManager.mPieceManager.mCurrentSelectedPiece.mTeamNum);
    
        return true;
    }
}