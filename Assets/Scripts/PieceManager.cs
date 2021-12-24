﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum PieceControlState {
    Idle,
    Attacking,
    Moving,
    Spawning,
}

public class PieceManagerEventData : EventArgs {
    public Type mPieceType;
    public AttackRange mPieceRange;

    public PieceManagerEventData(Type type, AttackRange range) {
        mPieceType = type;
        mPieceRange = range;
    }
}

public class PieceManager : MonoBehaviour {
    public event EventHandler<PieceManagerEventData> Spawned;

    public GameObject[] mTeamsPiecePrefab = new GameObject[5];

    public GameManager mGameManager;
    public UIHandler mUIHandler;
    public TurnHandler mTurnHandler;
    public Board mBoard;

    List<BasePiece> mMonsterPieces = new List<BasePiece>();
    public List<BasePiece> mPieces = new List<BasePiece>();
    public Dictionary<BasePiece, int> mPiecesDeadline = new Dictionary<BasePiece, int>();
    public List<BasePiece> mSleepingPieces = new List<BasePiece>();

    // State
    public PieceControlState mPieceControlState = PieceControlState.Idle;
    [HideInInspector]
    public BasePiece mCurrentSelectedPiece = null;
    public AttackType mSelectedPieceAttackType;
    public Type mSpawnPieceType = null;
    public AttackRange mSpawnPieceAttackRange;

    /*Color32[] mTeamColors = {
        new Color32(163, 31, 52, 255),
        new Color32(26, 65, 136, 255),
        new Color32(197, 166, 54, 255),
        new Color32(114, 31, 110, 255),
        new Color32(255, 255, 255, 255),
    };*/
    Vector2Int[][] mTeamsRespawnPositions = {
        new []{ new Vector2Int(2, 1)  , new Vector2Int(1, 2)   },
        new []{ new Vector2Int(2, 14) , new Vector2Int(1, 13)  },
        new []{ new Vector2Int(13, 14), new Vector2Int(14, 13) },
        new []{ new Vector2Int(14, 2) , new Vector2Int(13, 1)  },
    };
    public Type[] mPieceTypes = {
        typeof(Powerful),
        typeof(Communicative),
        typeof(Charming),
        typeof(Rich),
        typeof(Speedy),
        typeof(Energetic),
        typeof(Healthy),
    };

    Vector2Int[][] mTeamsSpawnRectArea = {
        new []{ new Vector2Int(0, 0)  , new Vector2Int(3, 3)   },
        new []{ new Vector2Int(0, 12) , new Vector2Int(3, 15)  },
        new []{ new Vector2Int(12, 12), new Vector2Int(15, 15) },
        new []{ new Vector2Int(12, 0) , new Vector2Int(15, 3)  },
    };
    Vector2Int[][] mMonsterRespawnRectAreas = {
        new []{ new Vector2Int(4, 4), new Vector2Int(7, 7)   },
        new []{ new Vector2Int(4, 8), new Vector2Int(7, 11)  },
        new []{ new Vector2Int(8, 8), new Vector2Int(11, 11) },
        new []{ new Vector2Int(8, 4), new Vector2Int(11, 7)  },
    };

    void Awake() {
        mGameManager.GoBackButtonPressed += OnGoBackButtonPressed;
        mBoard.OutlineCellClicked += OnOutlineCellClicked;
        mTurnHandler.NextTurn += OnNextTurn;
        mTurnHandler.NextRound += OnNextRound;
    }

    public void Setup(Board board) {
        for (int teamNum = 0; teamNum < 4; teamNum++) {
            foreach (var position in mTeamsRespawnPositions[teamNum]) {
                var type = mPieceTypes[UnityEngine.Random.Range(0, 6)];

                var newPiece = CreatePiece(teamNum, AttackRange.Melee, type);
                PlacePiece(newPiece, position, board);

                mPieces.Add(newPiece);
            }
        }
    }

    public void GenerateMonster(Board board) {
        int ignoreRespawnAreaIndex = UnityEngine.Random.Range(0, mMonsterRespawnRectAreas.Length - 1);

        for (int index = 0; index < mMonsterRespawnRectAreas.Length; index++) {
            if (index == ignoreRespawnAreaIndex) {
                continue;
            }

            int randomTime = 0;
            Vector2Int newPosition = new Vector2Int();
            do {
                Vector2Int minRange = new Vector2Int(mMonsterRespawnRectAreas[index][0].x, mMonsterRespawnRectAreas[index][0].y);
                Vector2Int maxRange = new Vector2Int(mMonsterRespawnRectAreas[index][1].x, mMonsterRespawnRectAreas[index][1].y);

                newPosition.x = UnityEngine.Random.Range(minRange.x, maxRange.x + 1);
                newPosition.y = UnityEngine.Random.Range(minRange.y, maxRange.y + 1);

                if (++randomTime > 100) {
                    return;
                }
            } while(board.ValidateCell(newPosition) != CellState.Free);

            var newPiece = CreatePiece(4, AttackRange.Melee, typeof(Monster));
            newPiece.AddLevel(mTurnHandler.mCurrentRoundNum / 2 - 1);
            PlacePiece(newPiece, newPosition, board);

            mMonsterPieces.Add(newPiece);
        }
    }

    BasePiece CreatePiece(int teamNum, AttackRange attackRange, Type pieceType) {
        GameObject newPieceObject = Instantiate(mTeamsPiecePrefab[teamNum], transform);

        BasePiece newPiece = newPieceObject.AddComponent(pieceType) as BasePiece;
        
        newPiece.AllyPieceSelected += OnAllyPieceSelected;
        newPiece.AllyPieceSelected += mUIHandler.OnAllyPieceSelected;
        newPiece.MouseOver += mUIHandler.OnMouseOverPiece;
        newPiece.MouseOut += mUIHandler.OnMouseOutPiece;
        newPiece.Killed += OnPieceKilled;

        newPiece.Setup(teamNum, attackRange, this);

        return newPiece;
    }

    void PlacePiece(BasePiece piece, Vector2Int boardPosition, Board board) {
        piece.Place(board.mCells[boardPosition.x, boardPosition.y]);
    }

    void PlacePiece(BasePiece piece, Cell cell) {
        piece.Place(cell);
    }

    void RemovePiece(BasePiece piece) {
        if (piece as Monster) {
            piece.mCurrentCell.mCurrentPiece = null;
            
            mMonsterPieces.Remove(piece);

            Destroy(piece.gameObject);
        }
        else {
            mPieces.Remove(piece);

            mPiecesDeadline.Add(piece, 8);
        }
    }

    public bool IsPieceSleeping(BasePiece piece) {
        return mSleepingPieces.Contains(piece);
    }

    public void OnAllyPieceSelected(object sender, EventArgs e) {
        BasePiece selectedPiece = sender as BasePiece;

        mBoard.ClearOutlineCells();
        mBoard.ShowOutlineCell(selectedPiece.mCurrentCell);

        mCurrentSelectedPiece = selectedPiece;
    }

    public void OnGoBackButtonPressed(object sender, EventArgs e) {
        ResetState();
    }

    public void ResetState() {
        mPieceControlState = PieceControlState.Idle;

        mCurrentSelectedPiece = null;
        mSpawnPieceType = null;
    }

    public void OnAttackButtonPressed(AttackButtonData data) {
        mPieceControlState = PieceControlState.Attacking;
        mSelectedPieceAttackType = data.mAttackType;

        mBoard.ClearOutlineCells();

        var position = mCurrentSelectedPiece.mCurrentCell.mBoardPosition;
        var attackRange = mCurrentSelectedPiece.mAttackRange;
        mBoard.ShowOutlineCells(position, attackRange);
    }

    public void OnMoveButtonPressed() {
        mPieceControlState = PieceControlState.Moving;

        mBoard.ClearOutlineCells();
    
        var outlineCells = mBoard.GetAvailableMovingCells  (mCurrentSelectedPiece, 
                                                            mCurrentSelectedPiece.mSpeed);
        mBoard.ShowOutlineCells(outlineCells);
    }

    public void OnSleepButtonPressed() {
        mSleepingPieces.Add(mCurrentSelectedPiece);

        ResetState();
        mBoard.ClearOutlineCells();
        mUIHandler.ResetGUI();
    }

    public void OnOutlineCellClicked(object sender, EventArgs e) {
        Cell clickedCell = sender as Cell;

        if (mPieceControlState == PieceControlState.Attacking) {
            BasePiece piece = clickedCell.mCurrentPiece;

            if (!piece) {
                return;
            }

            if (piece.mTeamNum == mCurrentSelectedPiece.mTeamNum) {
                return;
            }

            piece.BeAttackedBy(mCurrentSelectedPiece, mSelectedPieceAttackType);
            mUIHandler.UpdatePieceInfo(mCurrentSelectedPiece, piece);
        }

        if (mPieceControlState == PieceControlState.Moving) {
            mCurrentSelectedPiece.Move(clickedCell);
        }

        if (mPieceControlState == PieceControlState.Spawning) {
            var newPiece = CreatePiece (mTurnHandler.mCurrentTeamNum, 
                                        mSpawnPieceAttackRange, 
                                        mSpawnPieceType);
            PlacePiece(newPiece, clickedCell);

            mPieces.Add(newPiece);

            Spawned(this, new PieceManagerEventData(mSpawnPieceType, mSpawnPieceAttackRange));
        }

        ResetState();
        mBoard.ClearOutlineCells();
        mUIHandler.ResetGUI();
    }

    public void OnSpawnPieceButtonClicked(SpawnPieceButtonData data) {
        mPieceControlState = PieceControlState.Spawning;

        mSpawnPieceType = mPieceTypes[data.mSpawnTypeIndex];
        mSpawnPieceAttackRange = data.mAttackRange;

        mBoard.ShowOutlineCells(mTeamsSpawnRectArea[mTurnHandler.mCurrentTeamNum][0], 
                                mTeamsSpawnRectArea[mTurnHandler.mCurrentTeamNum][1], true);
    }

    public void OnPieceKilled(object sender, EventArgs e) {
        BasePiece piece = sender as BasePiece;

        RemovePiece(piece);
    }

    public void OnNextTurn(object sender, EventArgs e) {
        ResetState();

        foreach (var piece in mPieces) {
            piece.RestoreEnergy();
        }

        List<BasePiece> pieces = new List<BasePiece>(mPiecesDeadline.Keys);
        foreach (var piece in pieces) {
            if (--mPiecesDeadline[piece] <= 0) {
                Destroy(piece.gameObject);
                mPiecesDeadline.Remove(piece);
            }
        }

        for (int i = mSleepingPieces.Count - 1; i >= 0; i--) {
            var piece = mSleepingPieces[i];

            if (piece.mTeamNum == mTurnHandler.mCurrentTeamNum) {
                piece.mCurrentHealth += 2;
                piece.mCurrentEnergy = 0;

                mSleepingPieces.Remove(piece);
            }
        }
    }

    public void OnNextRound(object sender, TurnHandlerEventData e) {
        if (e.mIsNight) {
            GenerateMonster(mBoard);
        }
    }

    void SetInteractive(List<BasePiece> allPieces, bool value) {

    }

    public void SwitchSides(Color color) {

    }

    public void ResetPieces() {

    }
}
