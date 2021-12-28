using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceInfo : MonoBehaviour {
	public PieceManager mPieceManager;
	public TurnHandler mTurnHandler;

	Transform mPanelTransform;

	public List<BasePiece>[] mTeamsPiecesVisual = {
	new List<BasePiece>(),
	new List<BasePiece>(),
	new List<BasePiece>(),
	new List<BasePiece>(),
    };

	String mBarPathPrefix = "UI/bar_";
	Dictionary<AttackRange, String> mAttackRangePath = new Dictionary<AttackRange, string> {
	{ AttackRange.Melee,  "UI/melee" },
	{ AttackRange.Ranged, "UI/ranged" },
    };

	public Color32[] mTeamColors = {
	new Color32(163, 31, 52, 255),
	new Color32(26, 65, 136, 255),
	new Color32(197, 166, 54, 255),
	new Color32(114, 31, 110, 255),
	new Color32(255, 255, 255, 255),
    };

	public List<String> mTeamTexts = new List<String> {
	"成大帝國",
	"交大帝國",
	"台大帝國",
	"清大帝國",
    };
	Dictionary<Type, String> mTypeTexts = new Dictionary<Type, String> {
	{ typeof(Powerful),      "力量系" },
	{ typeof(Communicative), "溝通系" },
	{ typeof(Charming),      "魅力系" },
	{ typeof(Rich),          "有錢系" },
	{ typeof(Speedy),        "速度系" },
	{ typeof(Energetic),     "精神系" },
	{ typeof(Healthy),       "血量系" },
    };

	List<String> mChineseNumber = new List<String> {
	"零", "一", "二", "三", "四", "五"
    };

	void Awake() {
		mPanelTransform = transform.Find("Panel");
	}

	public void UpdatePieceInfo(BasePiece piece) {
		if (piece.GetType() == typeof(Monster)) {
			UpdateText("TeamText", "野生");
			UpdateText("TypeText", "野生");
			UpdateText("LevelText", ((char)(64 + piece.mLevel)).ToString());

			mPanelTransform.Find("PieceImage").GetComponent<Animator>().runtimeAnimatorController = null;
			UpdateImage("PieceImage", piece.gameObject);
		} else {
			UpdateText("TeamText", mTeamTexts[piece.mTeamNum]);
			UpdateText("TypeText", mTypeTexts[piece.GetType()]);

			UpdateAnimation("PieceImage", piece.GetComponent<Animator>().runtimeAnimatorController, new Vector2(128, 128));
		}

		UpdateImage("AttackRangeImage", mAttackRangePath[piece.mAttackRange]);
		UpdateOutlineColor(mTeamColors[piece.mTeamNum]);

		BasePiece pieceVisual = mTeamsPiecesVisual[mTurnHandler.mCurrentTeamNum].Find(p => p.mId == piece.mId);
		if (pieceVisual) {
            if (piece.GetType() != typeof(Monster)) {
                UpdateText("LevelText", mChineseNumber[pieceVisual.mLevel]);
            }
			UpdateText("StrengthValue", pieceVisual.mStrength.ToString());
			UpdateText("CharmValue", pieceVisual.mCharm.ToString());
			UpdateText("CommunicationValue", pieceVisual.mCommunication.ToString());
			UpdateText("RichnessValue", pieceVisual.mRichness.ToString());
			UpdateText("ExpValue", pieceVisual.mCurrentExp.ToString());
			UpdateText("MaxExpValue", pieceVisual.mExpOfLevelUp.ToString());

			UpdateText("HPValue", pieceVisual.mCurrentHealth.ToString());
			UpdateText("MaxHPValue", pieceVisual.mHealth.ToString());
			UpdateText("EnergyValue", pieceVisual.mCurrentEnergy.ToString());
			UpdateText("MaxEnergyValue", pieceVisual.mEnergy.ToString());
			UpdateText("SpeedValue", pieceVisual.mSpeed.ToString());

			UpdateImage("StrengthBar", mBarPathPrefix + pieceVisual.mStrength.ToString());
			UpdateImage("CharmBar", mBarPathPrefix + pieceVisual.mCharm.ToString());
			UpdateImage("CommunicationBar", mBarPathPrefix + pieceVisual.mCommunication.ToString());
			UpdateImage("RichnessBar", mBarPathPrefix + pieceVisual.mRichness.ToString());
		} else {
			UpdateText("StrengthValue", "?");
			UpdateText("CharmValue", "?");
			UpdateText("CommunicationValue", "?");
			UpdateText("RichnessValue", "?");
			UpdateText("ExpValue", "?");
			UpdateText("MaxExpValue", "?");

			UpdateText("HPValue", "?");
			UpdateText("MaxHPValue", "?");
			UpdateText("EnergyValue", "?");
			UpdateText("MaxEnergyValue", "?");
			UpdateText("SpeedValue", "?");

			UpdateImage("StrengthBar", mBarPathPrefix + "0");
			UpdateImage("CharmBar", mBarPathPrefix + "0");
			UpdateImage("CommunicationBar", mBarPathPrefix + "0");
			UpdateImage("RichnessBar", mBarPathPrefix + "0");
		}


	}

	void UpdateOutlineColor(Color color) {
		Image outline = mPanelTransform.GetComponent<Image>();

		outline.color = color;
	}

	void UpdateText(String name, String value) {
		Text text = mPanelTransform.Find(name).GetComponent<Text>();

		text.text = value;
	}

	void UpdateImage(String name, String path) {
		UpdateImage(name, Resources.Load<Sprite>(path));
	}

	void UpdateImage(String name, GameObject obj) {
		UpdateImage(name, obj.GetComponent<Image>().sprite);
	}

	void UpdateImage(String name, Sprite sprite) {
		Image image = mPanelTransform.Find(name).GetComponent<Image>();

		image.sprite = sprite;
	}

	void UpdateAnimation(String name, RuntimeAnimatorController controller, Vector2 vec) {
		Animator animator = mPanelTransform.Find("PieceImage").GetComponent<Animator>();
		RectTransform rectTransform = mPanelTransform.Find("PieceImage").GetComponent<RectTransform>();

		animator.runtimeAnimatorController = controller;

		rectTransform.sizeDelta = vec;
	}

	public void UpdatePieceVisual(BasePiece piece, BasePiece piece2) {
		if (piece.mTeamNum != 4) {
			List<BasePiece> pieceVisual = mTeamsPiecesVisual[piece.mTeamNum];

			pieceVisual.Remove(piece.ShallowClone());
			pieceVisual.Add(piece.ShallowClone());

			pieceVisual.Remove(piece2.ShallowClone());
			pieceVisual.Add(piece2.ShallowClone());
		}
		if (piece2.mTeamNum != 4) {
			List<BasePiece> piece2Visual = mTeamsPiecesVisual[piece2.mTeamNum];

			piece2Visual.Remove(piece.ShallowClone());
			piece2Visual.Add(piece.ShallowClone());

			piece2Visual.Remove(piece2.ShallowClone());
			piece2Visual.Add(piece2.ShallowClone());
		}

	}

	public void AddPieceVisual(BasePiece piece) {
		foreach (var pieceVisual in mTeamsPiecesVisual) {
			pieceVisual.Add(piece.ShallowClone());
		}
	}
}
