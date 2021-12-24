using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceInfo : MonoBehaviour
{
    public PieceManager mPieceManager;

    Transform mPanelTransform;

    String mBarPathPrefix = "UI/bar_";
    Dictionary<AttackRange, String> mAttackRangePath = new Dictionary<AttackRange, string> {
        { AttackRange.Melee,  "UI/melee" },
        { AttackRange.Ranged, "UI/ranged" },
    };

    Color32[] mTeamColors = {
        new Color32(163, 31, 52, 255),
        new Color32(26, 65, 136, 255),
        new Color32(197, 166, 54, 255),
        new Color32(114, 31, 110, 255),
        new Color32(255, 255, 255, 255),
    };

    List<String> mTeamTexts = new List<String> {
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
            UpdateText("LevelText", ((char) (64 + piece.mLevel)).ToString());

            mPanelTransform.Find("PieceImage").GetComponent<Animator>().runtimeAnimatorController = null;
            UpdateImage("PieceImage", piece.gameObject);
        }
        else {
            UpdateText("TeamText", mTeamTexts[piece.mTeamNum]);
            UpdateText("TypeText", mTypeTexts[piece.GetType()]);
            UpdateText("LevelText", mChineseNumber[piece.mLevel]);

            UpdateAnimation("PieceImage", piece.GetComponent<Animator>().runtimeAnimatorController, new Vector2(128, 128));
        }

        UpdateText("StrengthValue", piece.mStrength.ToString());
        UpdateText("CharmValue", piece.mCharm.ToString());
        UpdateText("CommunicationValue", piece.mCommunication.ToString());
        UpdateText("RichnessValue", piece.mRichness.ToString());
        UpdateText("ExpValue", piece.mCurrentExp.ToString());
        UpdateText("MaxExpValue", piece.mExpOfLevelUp.ToString());

        UpdateText("HPValue", piece.mCurrentHealth.ToString());
        UpdateText("MaxHPValue", piece.mHealth.ToString());
        UpdateText("EnergyValue", piece.mCurrentEnergy.ToString());
        UpdateText("MaxEnergyValue", piece.mEnergy.ToString());
        UpdateText("SpeedValue", piece.mSpeed.ToString());

        UpdateImage("StrengthBar", mBarPathPrefix + piece.mStrength.ToString());
        UpdateImage("CharmBar", mBarPathPrefix + piece.mCharm.ToString());
        UpdateImage("CommunicationBar", mBarPathPrefix + piece.mCommunication.ToString());
        UpdateImage("RichnessBar", mBarPathPrefix + piece.mRichness.ToString());

        UpdateImage("AttackRangeImage", mAttackRangePath[piece.mAttackRange]);

        UpdateOutlineColor(mTeamColors[piece.mTeamNum]);
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
}
