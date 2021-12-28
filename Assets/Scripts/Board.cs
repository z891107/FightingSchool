using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public enum CellState {
	None,
	Friendly,
	Enemy,
	Obstacle,
	Free,
	OutOfBounds
}

public class TeamBaseCrashedEventArgs : EventArgs {
	public int mTeamNum;
}

public class Board : MonoBehaviour {
	public event EventHandler OutlineCellClicked;
	public event EventHandler<TeamBaseCrashedEventArgs> TeamBaseCrashed;

	public GameObject mCellPrefab;

	public GameManager mGameManager;
	public PieceManager mPicecManager;
	public TurnHandler mTurnHandler;
	public CardManager mCardManager;
	public UIHandler mUIHandler;

	[HideInInspector]
	public Cell[,] mCells = new Cell[16, 16];

	List<Cell> mOutlineCells = new List<Cell>();

	[SerializeField]
	GameObject[] mTeamsBase = new GameObject[4];
	Vector2Int[][] mTeamsBaseArea = {
	    new []{ new Vector2Int(0, 0)  , new Vector2Int(1, 1)   },
	    new []{ new Vector2Int(0, 14) , new Vector2Int(1, 15)  },
	    new []{ new Vector2Int(14, 14), new Vector2Int(15, 15) },
	    new []{ new Vector2Int(14, 0) , new Vector2Int(15, 1)  },
    };

	void Awake() {
		mGameManager.GoBackButtonPressed += OnGoBackButtonPressed;
		mTurnHandler.NextTurn += OnNextTurn;
		mTurnHandler.NextRound += OnNextRound;
		mCardManager.CardUsed += OnCardUsed;
		mUIHandler.GUIReseted += OnGUIReseted;
	}

	public void Create() {
		for (int y = 0; y < 16; y++) {
			for (int x = 0; x < 16; x++) {
				Vector2Int newCellPosition = new Vector2Int(x, y);
				GameObject newCell = Instantiate(mCellPrefab, transform); // remove transform?

				RectTransform newCellRectTransform = newCell.GetComponent<RectTransform>();
				newCellRectTransform.anchoredPosition = newCellPosition * 64;

				mCells[x, y] = newCell.GetComponent<Cell>();
				mCells[x, y].Setup(newCellPosition, this);

				mCells[x, y].CellClicked += OnCellClicked;
			}
		}
	}

	public CellState ValidateCell(Vector2Int position, BasePiece checkingPiece) {
		try {
			Cell targetCell = mCells[position.x, position.y];

			if (targetCell.mCurrentPiece != null) {
				if (targetCell.mCurrentPiece.mInvisible) {
					return CellState.Free;
				}

				if (targetCell.mCurrentPiece.mTeamNum == checkingPiece.mTeamNum) {
					return CellState.Friendly;
				} else {
					return CellState.Enemy;
				}
			}

			return CellState.Free;
		} catch (IndexOutOfRangeException) {
			return CellState.OutOfBounds;
		}
	}

	public CellState ValidateCell(Vector2Int position) {
		try {
			Cell targetCell = mCells[position.x, position.y];

			if (targetCell.mCurrentPiece != null) {
				if (targetCell.mCurrentPiece.mInvisible) {
					return CellState.Free;
				} else {
					return CellState.Obstacle;
				}
			}

			return CellState.Free;
		} catch (IndexOutOfRangeException) {
			return CellState.OutOfBounds;
		}
	}

	private List<Cell> CreateCellPath(BasePiece piece, Vector2Int position, int movement, Dictionary<Vector2Int, int> remainMovementOfCell) {
		List<Cell> result = new List<Cell>();

		if (remainMovementOfCell.ContainsKey(position)) {
			remainMovementOfCell[position] = movement;
		} else {
			remainMovementOfCell.Add(position, movement);
		}

		if (movement == 0) {
			return result;
		}

		// up
		var up = position + Vector2Int.up;
		var cellState = ValidateCell(up, piece);

		if (cellState == CellState.Friendly && movement >= 2) {
			result.AddRange(CreateCellPath(piece, up, movement - 1, remainMovementOfCell));
		}

		if (cellState == CellState.Free) {
			var upCell = mCells[up.x, up.y];

			if (!result.Contains(upCell)) {
				result.Add(upCell);
				result.AddRange(CreateCellPath(piece, up, movement - 1, remainMovementOfCell));
			} else if (remainMovementOfCell[up] < movement) {
				result.AddRange(CreateCellPath(piece, up, movement - 1, remainMovementOfCell));
			}
		}

		// down
		var down = position + Vector2Int.down;
		cellState = ValidateCell(down, piece);

		if (cellState == CellState.Friendly && movement >= 2) {
			result.AddRange(CreateCellPath(piece, down, movement - 1, remainMovementOfCell));
		}

		if (cellState == CellState.Free) {
			var downCell = mCells[down.x, down.y];

			if (!result.Contains(downCell)) {
				result.Add(downCell);
				result.AddRange(CreateCellPath(piece, down, movement - 1, remainMovementOfCell));
			} else if (remainMovementOfCell[down] < movement) {
				result.AddRange(CreateCellPath(piece, down, movement - 1, remainMovementOfCell));
			}
		}

		// left
		var left = position + Vector2Int.left;
		cellState = ValidateCell(left, piece);

		if (cellState == CellState.Friendly && movement >= 2) {
			result.AddRange(CreateCellPath(piece, left, movement - 1, remainMovementOfCell));
		}

		if (cellState == CellState.Free) {
			var leftCell = mCells[left.x, left.y];

			if (!result.Contains(leftCell)) {
				result.Add(leftCell);
				result.AddRange(CreateCellPath(piece, left, movement - 1, remainMovementOfCell));
			} else if (remainMovementOfCell[left] < movement) {
				result.AddRange(CreateCellPath(piece, left, movement - 1, remainMovementOfCell));
			}
		}

		// right
		var right = position + Vector2Int.right;
		cellState = ValidateCell(right, piece);

		if (cellState == CellState.Friendly && movement >= 2) {
			result.AddRange(CreateCellPath(piece, right, movement - 1, remainMovementOfCell));
		}

		if (cellState == CellState.Free) {
			var rightCell = mCells[right.x, right.y];

			if (!result.Contains(rightCell)) {
				result.Add(rightCell);
				result.AddRange(CreateCellPath(piece, right, movement - 1, remainMovementOfCell));
			} else if (remainMovementOfCell[right] < movement) {
				result.AddRange(CreateCellPath(piece, right, movement - 1, remainMovementOfCell));
			}
		}

		return result;
	}

	public List<Cell> GetAvailableMovingCells(BasePiece movingPiece, int distance) {
		List<Cell> availableMovingCells = new List<Cell>();

		availableMovingCells = CreateCellPath(movingPiece, movingPiece.mCurrentCell.mBoardPosition, distance, new Dictionary<Vector2Int, int>());

		availableMovingCells.Remove(movingPiece.mCurrentCell);

		return availableMovingCells;
	}

	public void ShowOutlineCells(Vector2Int start, Vector2Int end, bool isOnlyFreeCell) {
		for (int y = start.y; y <= end.y; y++) {
			for (int x = start.x; x <= end.x; x++) {
				ShowOutlineCell(new Vector2Int(x, y), isOnlyFreeCell);
			}
		}
	}

	public void ShowOutlineCellsInCross(Vector2Int center, bool isOnlyFreeCell) {
		ShowOutlineCells(new Vector2Int(center.x, 0), new Vector2Int(center.x, 15), isOnlyFreeCell);
		ShowOutlineCells(new Vector2Int(0, center.y), new Vector2Int(15, center.y), isOnlyFreeCell);
	}

	public void ShowOutlineCellsInAngledCross(Vector2Int center, bool isOnlyFreeCell) {
		for (int x = center.x - 15, y = center.y - 15; x <= 15 && y <= 15; x++, y++) {
			ShowOutlineCell(new Vector2Int(x, y), isOnlyFreeCell);
		}

		for (int x = center.x - 15, y = center.y + 15; x <= 15 && y >= 0; x++, y--) {
			ShowOutlineCell(new Vector2Int(x, y), isOnlyFreeCell);
		}
	}

	public void ShowOutlineCellsRoundAllPiece(Vector2Int center, bool isOnlyFreeCell) {
		foreach (BasePiece piece in mPicecManager.mPieces) {
			Vector2Int position = piece.mCurrentCell.mBoardPosition;

			ShowOutlineCell(position + Vector2Int.up, isOnlyFreeCell);
			ShowOutlineCell(position + Vector2Int.down, isOnlyFreeCell);
			ShowOutlineCell(position + Vector2Int.left, isOnlyFreeCell);
			ShowOutlineCell(position + Vector2Int.right, isOnlyFreeCell);
		}
	}


	public void ShowOutlineCells(List<Cell> outlineCells) {
		foreach (var outlineCell in outlineCells) {
			ShowOutlineCell(outlineCell);
		}
	}

	public void ShowOutlineCells(Vector2Int center, int minDistance, int maxDistance) {
		for (int distance = minDistance; distance <= maxDistance; distance++) {

			// left side including center
			for (Vector2Int position = center + Vector2Int.left * distance;
			    position.x <= center.x;
			    position += Vector2Int.right + Vector2Int.up) {

				ShowOutlineCell(position);
			}

			for (Vector2Int position = center + Vector2Int.left * distance;
			    position.x <= center.x;
			    position += Vector2Int.right + Vector2Int.down) {

				ShowOutlineCell(position);
			}

			// right side 
			for (Vector2Int position = center + Vector2Int.right * distance;
			    position.x > center.x;
			    position += Vector2Int.left + Vector2Int.up) {

				ShowOutlineCell(position);
			}

			for (Vector2Int position = center + Vector2Int.right * distance;
			position.x > center.x;
			    position += Vector2Int.left + Vector2Int.down) {

				ShowOutlineCell(position);
			}
		}
	}

	public void ShowOutlineCells(Vector2Int position, AttackRange attackRange) {
		if (attackRange == AttackRange.Melee) {
			ShowOutlineCells(position, 1, 1);
		} else if (attackRange == AttackRange.Ranged) {
			ShowOutlineCells(position, 2, 2);
		}
	}

	public void ShowOutlineCell(Vector2Int position, bool isOnlyFreeCell) {
		if (!isOnlyFreeCell || ValidateCell(position) == CellState.Free) {
			ShowOutlineCell(position);
		}
	}

	public void ShowOutlineCell(Vector2Int position) {
		if (ValidateCell(position) == CellState.OutOfBounds) {
			return;
		}

		mCells[position.x, position.y].SetOutline(true);

		mOutlineCells.Add(mCells[position.x, position.y]);
	}

	public void ShowOutlineCell(Cell outlineCell) {
		outlineCell.SetOutline(true);

		mOutlineCells.Add(outlineCell);
	}

	public void ClearOutlineCells() {
		foreach (var cell in mOutlineCells) {
			cell.SetOutline(false);
		}

		mOutlineCells.Clear();
	}

	public void SetTime(bool isNight) {
		foreach (var cell in mCells) {
			cell.SetColor(isNight);
		}
	}

	public void OnNextTurn(object sender, EventArgs e) {
		ClearOutlineCells();
	}

	public void OnNextRound(object sender, TurnHandlerEventData e) {
		SetTime(e.mIsNight);
	}

	public void OnCellClicked(object sender, EventArgs e) {
		Cell clickedCell = sender as Cell;

		if (mOutlineCells.Contains(clickedCell)) {
			OutlineCellClicked(clickedCell, EventArgs.Empty);
		}
	}

	public void OnCardUsed(object sender, EventArgs e) {
		ClearOutlineCells();
	}

	public void OnGoBackButtonPressed(object sender, EventArgs e) {
		ClearOutlineCells();
	}

	public void OnGUIReseted(object sender, EventArgs e) {
		for (int i = 0; i < mTeamsBaseArea.Length; i++) {
			Vector2Int start = mTeamsBaseArea[i][0], end = mTeamsBaseArea[i][1];
			int pieceNum = 0;

			for (int y = start.y; y <= end.y; y++) {
				for (int x = start.x; x <= end.x; x++) {
					if (mCells[x, y].mCurrentPiece && mCells[x, y].mCurrentPiece.mTeamNum != i) {
						pieceNum++;
					}
				}
			}

			if (pieceNum == 4 && mTeamsBase[i]) {
				CrashTeamBase(i);
			}
		}

		for (int i = 0; i < 4; i++) {
			if (mTeamsBase[i] && mPicecManager.TeamPiecesNum(i) == 0) {
                CrashTeamBase(i);
			}
		}
	}

	public void CrashTeamBase(int teamNum) {
		Destroy(mTeamsBase[teamNum]);
		mTeamsBase[teamNum] = null;

        int winTeamNum = -1;
        for (int i = 0; i < 4; i++) {
            if (mTeamsBase[i] && winTeamNum != -1) {
                winTeamNum = -1;
                break;
            }
            else if (mTeamsBase[i]) {
                winTeamNum = i;
            }
        }

        if (winTeamNum != -1) {
            mUIHandler.ShowWinPanel(winTeamNum);
        }

		TeamBaseCrashedEventArgs args = new TeamBaseCrashedEventArgs();
		args.mTeamNum = teamNum;
		TeamBaseCrashed(this, args);
	}
}
