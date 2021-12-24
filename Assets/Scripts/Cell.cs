using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : EventTrigger
{
    public event EventHandler CellClicked;

    public Image mOutlineImage;

    [HideInInspector]
    public Vector2Int mBoardPosition = Vector2Int.zero;
    [HideInInspector]
    public Board mBoard = null;
    [HideInInspector]
    public RectTransform mRectTransform = null;

    [HideInInspector]
    public BasePiece mCurrentPiece = null;

    Color mPreviousColor = Color.clear;

    Color32 mWhiteColorInDay = new Color32(176, 148, 98, 255);
    Color32 mBlackColorInDay = new Color32(149, 114, 83, 255);
    Color32 mWhiteColorInNight = new Color32(125, 133, 154, 255);
    Color32 mBlackColorInNight = new Color32(107, 109, 134, 255);
    Color32 mWhiteOutlineColorInDay = new Color32(74, 62, 41, 255);
    Color32 mBlackOutlineColorInDay = new Color32(74, 56, 41, 255);
    Color32 mWhiteOutlineColorInNight = new Color32(62, 66, 77, 255);
    Color32 mBlackOutlineColorInNight = new Color32(51, 52, 64, 255);

    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;

        mRectTransform = GetComponent<RectTransform>();

        SetColor(false);
    }

    public void SetColor(bool isNight)
    {
        mPreviousColor = Color.clear;

        bool isWhite = (mBoardPosition.x + mBoardPosition.y) % 2 == 0;

        if (isNight && isWhite) {
            GetComponent<Image>().color = mWhiteColorInNight;
            transform.Find("Outline").GetComponent<Image>().color = mWhiteOutlineColorInNight;
        }
        else if (isNight && !isWhite) {
            GetComponent<Image>().color = mBlackColorInNight;
            transform.Find("Outline").GetComponent<Image>().color = mBlackOutlineColorInNight;
        }
        else if (!isNight && isWhite) {
            GetComponent<Image>().color = mWhiteColorInDay;
            transform.Find("Outline").GetComponent<Image>().color = mWhiteOutlineColorInDay;
        }
        else if (!isNight && !isWhite) {
            GetComponent<Image>().color = mBlackColorInDay;
            transform.Find("Outline").GetComponent<Image>().color = mBlackOutlineColorInDay;
        }
    }

    public void SetOutline(bool isOutline) {
        mOutlineImage.enabled = isOutline;
    }

    public void SetHighlight(bool isHighlight) {
        if (!isHighlight) {
            if (mPreviousColor != Color.clear) {
                GetComponent<Image>().color = mPreviousColor;
                mPreviousColor = Color.clear;
            }

            return;
        }

        float H, S, V;
        if (mPreviousColor == Color.clear) {
            mPreviousColor = GetComponent<Image>().color;
        }
        Color.RGBToHSV(mPreviousColor, out H, out S, out V);

        GetComponent<Image>().color = Color.HSVToRGB(H, S, 1);
    }

    public override void OnPointerClick(PointerEventData eventData) {
        base.OnPointerClick(eventData);

        if (eventData.button != PointerEventData.InputButton.Left) {
            return;
        }

        CellClicked(this, EventArgs.Empty);
    }

    public override void OnPointerEnter(PointerEventData eventData) {
        base.OnPointerEnter(eventData);

        SetHighlight(true);
    }

    public override void OnPointerExit(PointerEventData eventData) {
        base.OnPointerExit(eventData);

        SetHighlight(false);
    }
}
