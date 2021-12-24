using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnHandlerEventData : EventArgs {
    public bool mIsNight;
}

public class TurnHandler : MonoBehaviour {
    public event EventHandler NextTurn;
    public event EventHandler<TurnHandlerEventData> NextRound;

    public Board mBoard;
    public PieceManager mPieceManager;

    [HideInInspector]
    public List<int> mCurrentTeamSequence = new List<int>();

    [HideInInspector]
    int mTotalTurnNum = 0;
    [HideInInspector]
    public int mCurrentRoundNum {
        get {
            return mTotalTurnNum / 4 + 1;
        }
    }
    [HideInInspector]
    public int mCurrentTeamNum {
        get {
            return mCurrentTeamSequence[mTotalTurnNum % 4];
        }
    }

    public void GameStart() {
        mCurrentTeamSequence = RandomlyGenerateTeamSequence();
    }

    public void OnNextTurnButtonClicked() {
        mTotalTurnNum++;


        if (mTotalTurnNum % 4 == 0) {
            mCurrentTeamSequence = RandomlyGenerateTeamSequence();

            TurnHandlerEventData data = new TurnHandlerEventData();
            data.mIsNight = mCurrentRoundNum % 2 == 0;

            NextRound(this, data);
        }

        NextTurn(this, EventArgs.Empty);
    }

    List<int> RandomlyGenerateTeamSequence() {
        List<int> result = new List<int>();

        List<int> randomList = Enumerable.Range(0, 4).ToList();

        while (randomList.Count != 0) {
            int randomListIndex = UnityEngine.Random.Range(0, randomList.Count);
            int randomNumber = randomList[randomListIndex];
            randomList.RemoveAt(randomListIndex);

            result.Add(randomNumber);
        }

        return result;
    }
}
