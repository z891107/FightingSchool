using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using System.Reflection;
using Unity.VisualScripting.Dependencies.NCalc;
using System.Data;

public enum AttackRange {
    Melee,
    Ranged,
}

public enum AttackType {
    Strength,
    Charm,
    Communication,
    Richness,
}

public abstract class BasePiece : EventTrigger
{
    public event EventHandler AllyPieceSelected;
    public event EventHandler MouseOver;
    public event EventHandler MouseOut;
    public event EventHandler Killed;

    public Cell mCurrentCell = null;

    [HideInInspector]
    public int mId;
    public int mTeamNum;

    protected Animator mAnimator;
    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;
    protected TurnHandler mTurnHandler;

    int[] mExpOfLevelUpList = {0, 60, 70, 80, 90, 100, 100};
    Dictionary<AttackType, FieldInfo> mAttackTypeToFieldInfo = new Dictionary<AttackType, FieldInfo> {
        { AttackType.Strength    , typeof(BasePiece).GetField("mStrength") },
        { AttackType.Charm       , typeof(BasePiece).GetField("mCharm") },
        { AttackType.Communication, typeof(BasePiece).GetField("mCommunication") },
        { AttackType.Richness    , typeof(BasePiece).GetField("mRichness") },
    };

    public int mLevel = 1;
    public int mStrength = 3;
    public int mCommunication = 3;
    public int mCharm = 3;
    public int mRichness = 3;

    public int _mSpeed = 8;
    public int mSpeed {
        get { return _mSpeed; }
        set {
            if (value < 0) { _mSpeed = 0; }
            else { _mSpeed = value; }
        }
    }

    public int _mEnergy = 2;
    public int mEnergy {
        get { return _mEnergy; }
        set {
            if (value < 0) { _mEnergy = 0; }
            else { _mEnergy = value; }
        }
    }

    public int _mHealth = 10;
    public int mHealth {
        get { return _mHealth; }
        set {
            if (value < 0) { _mHealth = 0; }
            else { _mHealth = value; }
        }
    }
    
    public int mExpOfLevelUp {
        get {
            return mExpOfLevelUpList[mLevel];
        }
    }

    int _mCurrentEnergy = 2;
    public int mCurrentEnergy {
        get { return _mCurrentEnergy; }
        set {
            if (value >= mEnergy) {
                _mCurrentEnergy = mEnergy;
            }
            else if (value <= 0) {
                _mCurrentEnergy = 0;
                OutOfEnergy();
            }
            else {
                _mCurrentEnergy = value;
            }
        }
    }
    
    int _mCurrentHealth = 10;
    public int mCurrentHealth {
        get { return _mCurrentHealth; }
        set {
            if (value >= mHealth) {
                _mCurrentHealth = mHealth;
            }
            else if (value <= 0) {
                _mCurrentHealth = 0;
                Kill();
            }
            else {
                _mCurrentHealth = value;
            }
        }
    }

    int _mCurrentExp = 0;
    public int mCurrentExp {
        get { return _mCurrentExp; }
        set {
            if (value >= mExpOfLevelUp) {
                _mCurrentExp = value - mExpOfLevelUp;
                LevelUp();

                if (mLevel == 5) { _mCurrentExp = mExpOfLevelUpList[5]; }
            }
            else {
                _mCurrentExp = value;
            }
        }
    }

    public bool mInvisible = false;
    public bool mInactive = false;

    public AttackRange mAttackRange;

    public Card[] mCards = new Card[3];


    void Awake() {
        mAnimator = GetComponent<Animator>();
        mTurnHandler = GameObject.Find("PR_TurnHandler").GetComponent<TurnHandler>();
    }

    public BasePiece ShallowClone() {
        return (BasePiece)this.MemberwiseClone();
    }

    public virtual void Setup(int newTeamNum, int newId, AttackRange newAttackRange, PieceManager newPieceManager)
    {
        mTeamNum = newTeamNum;

        mId = newId;

        mAttackRange = newAttackRange;

        mPieceManager = newPieceManager;
        mRectTransform = GetComponent<RectTransform>();

        if (mAttackRange == AttackRange.Ranged) {
            mHealth -= 2;
            mCurrentHealth -= 2;
        }
    }

    public virtual void Place(Cell newCell) {
        if (mCurrentCell) {
            mCurrentCell.mCurrentPiece = null;
        }

        mCurrentCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        transform.position = newCell.transform.position + new Vector3(32, 32, 0);
    }

    public void Move(Cell newCell) {
        Place(newCell);

        mCurrentEnergy -= 1;
    }

    public void BecameInvisible() {
        Image image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, (float)0.5);

        mCurrentCell.mCurrentPiece = null;

        mInvisible = true;
    }

    public void BecameVisible() {
        Image image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);

        mCurrentCell.mCurrentPiece = this;

        mInvisible = false;
    }

    public void RestoreEnergy() {
        if (mInactive) {
            return;
        }

        mCurrentEnergy = mEnergy;

        GetComponent<Image>().material = null;
    }

    public void OutOfEnergy() {
        GetComponent<Image>().material = Resources.Load<Material>("Shader/Gray");
    }

    public bool IsOutOfEnergy() {
        return mCurrentEnergy == 0;
    }

    public void Inactive() {
        mInactive = true;
        OutOfEnergy();
    }

    public bool IsSleeping() {
        return mPieceManager.IsPieceSleeping(this);
    }

    public virtual void Kill() {
        transform.Rotate(new Vector3(0, 0, 90));
        GetComponent<Image>().material = Resources.Load<Material>("Shader/Gray");
        mAnimator.SetBool("Killed", true);

        Killed(this, EventArgs.Empty);
    }

    public bool IsKilled() {
        return mCurrentHealth == 0;
    }

    public virtual void BeAttackedBy(BasePiece attackPiece, AttackType attackType) {
        FieldInfo field = mAttackTypeToFieldInfo[attackType];

        int attackPiecePower = (int)field.GetValue(attackPiece);
        int beAttackedPiecePower = (int)field.GetValue(this);
        int damage = attackPiecePower - beAttackedPiecePower;

        if (damage >= 0) {
            attackPiece.mAnimator.SetTrigger("Attack");
            this.mAnimator.SetTrigger("BeAttack");

            this.mCurrentHealth -= damage;
            attackPiece.mCurrentExp += 30;

            if (IsKilled()) {
                attackPiece.mCurrentExp += 30;
            }
        }
        else {
            attackPiece.mAnimator.SetTrigger("AttackButNotEnoughDamage");

            attackPiece.mCurrentHealth += (int)Math.Floor(damage / 2.0);
            attackPiece.mCurrentExp += 10;
        }

        attackPiece.mCurrentEnergy -= 1;

        mPieceManager.mUIHandler.UpdatePieceVisual(this, attackPiece);
    }

    public void AddLevel(int level) {
        for (int i = 0; i < level; i++) {
            LevelUp();
        }
    }

    protected virtual bool LevelUp() {
        if (mLevel == 5) {
            return false;
        }
        mLevel++;

        Array values = Enum.GetValues(typeof(AttackType));

        for (int i = 0; i < 2; i++) {
            AttackType attackType = (AttackType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
            AddAttackPower(attackType);
        }

        return true;
    }

    public void AddAttackPower(AttackType attackType) {
        AddAttackPower(attackType, 1);
    }

    public void AddAttackPower(AttackType attackType, int value) {
        FieldInfo field = mAttackTypeToFieldInfo[attackType];
        int newValue = (int)field.GetValue(this) + value;

        if (newValue < 0) {
            newValue = 0;
        }
        
        field.SetValue(this, newValue);
    }

    public bool IsAlly() {
        return mTeamNum == mTurnHandler.mCurrentTeamNum;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        mCurrentCell.OnPointerClick(eventData);

        if (eventData.button != PointerEventData.InputButton.Left) {
            return;
        }

        if (IsKilled() || mInvisible || !IsAlly()) {
            return;
        }

        AllyPieceSelected(this, EventArgs.Empty);
    }

    public override void OnPointerEnter(PointerEventData eventData) {
        base.OnPointerEnter(eventData);
        mCurrentCell.OnPointerEnter(eventData);

        MouseOver(this, EventArgs.Empty);
    }

    public override void OnPointerExit(PointerEventData eventData) {
        base.OnPointerExit(eventData);
        mCurrentCell.OnPointerExit(eventData);

        MouseOut(this, EventArgs.Empty);
    }
}
