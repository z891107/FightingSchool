using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Pool;

public enum CellState
{
    None,
    Friendly,
    Enemy,
    Obstacle,
    Free,
    OutOfBounds
}

public class Board : MonoBehaviour
{
    public event EventHandler OutlineCellClicked;

    public GameObject mCellPrefab;

    public GameManager mGameManager;
    public PieceManager mPicecManager;
    public TurnHandler mTurnHandler;

    [HideInInspector]
    public Cell[,] mCells = new Cell[16, 16];

    List<Cell> mOutlineCells = new List<Cell>();

    void Awake() {
        mGameManager.GoBackButtonPressed += OnGoBackButtonPressed;
        mTurnHandler.NextTurn += OnNextTurn;
        mTurnHandler.NextRound += OnNextRound;
    }

    public void Create()
    {
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
                if (targetCell.mCurrentPiece.mTeamNum == checkingPiece.mTeamNum) {
                    return CellState.Friendly;
                }
                else {
                    return CellState.Enemy;
                }
            }

            return CellState.Free;
        }
        catch (IndexOutOfRangeException) {
            return CellState.OutOfBounds;
        }
    }

    public CellState ValidateCell(Vector2Int position) {   
        try {
            Cell targetCell = mCells[position.x, position.y];

            if (targetCell.mCurrentPiece != null) {
                return CellState.Obstacle;
            }

            return CellState.Free;
        }
        catch (IndexOutOfRangeException) {
            return CellState.OutOfBounds;
        }
    }

    private List<Cell> CreateCellPath(BasePiece piece, Vector2Int position, int movement, Dictionary<Vector2Int, int> remainMovementOfCell)
    {
        List<Cell> result = new List<Cell>();

        if (remainMovementOfCell.ContainsKey(position)) {
            remainMovementOfCell[position] = movement;
        }
        else {
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
            }
            else if (remainMovementOfCell[up] < movement) {
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
            }
            else if (remainMovementOfCell[down] < movement) {
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
            }
            else if (remainMovementOfCell[left] < movement) {
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
            }
            else if (remainMovementOfCell[right] < movement) {
                result.AddRange(CreateCellPath(piece, right, movement - 1, remainMovementOfCell));
            }
        }

        return result;
    }

    public List<Cell> GetAvailableMovingCells(BasePiece movingPiece, int distance)
    {
        List<Cell> availableMovingCells = new List<Cell>();

        availableMovingCells = CreateCellPath(movingPiece, movingPiece.mCurrentCell.mBoardPosition, distance, new Dictionary<Vector2Int, int>());

        availableMovingCells.Remove(movingPiece.mCurrentCell);

        return availableMovingCells;
    }

    public void ShowOutlineCells(Vector2Int start, Vector2Int end, bool isOnlyFreeCell) {
        for (int y = start.y; y <= end.y; y++) {
            for (int x = start.x; x <= end.x; x++) {
                if (!isOnlyFreeCell || ValidateCell(new Vector2Int(x, y)) == CellState.Free) {
                    ShowOutlineCell(new Vector2Int(x, y));
                }
            }
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
        }
        else if (attackRange == AttackRange.Ranged) {
            ShowOutlineCells(position, 2, 2);
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

    public void OnGoBackButtonPressed(object sender, EventArgs e) {
        ClearOutlineCells();
    }
}
