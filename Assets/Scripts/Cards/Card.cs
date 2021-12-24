using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : EventTrigger {
    public event EventHandler MouseOver;
    public event EventHandler MouseOut;
    public event EventHandler EndDrag;

    protected RectTransform mRectTransform;
    protected CardManager mCardManager;
    protected RectTransform mPlacingCardRectTransform;

    public CardEffectData mEffectData;

    void Awake() {
        mRectTransform = GetComponent<RectTransform>();
    }

    public void Setup(CardEffectData effectData, CardManager cardManager, RectTransform placingCardRectTransform) {
        mEffectData = effectData;
        mCardManager = cardManager;
        mPlacingCardRectTransform = placingCardRectTransform;
    }

    public bool Action() {
        return mEffectData.Action();
    }

    public void DelayedAction() {
        mEffectData.DelayedAction();
    }

    public override void OnPointerEnter(PointerEventData eventData) {
        base.OnPointerEnter(eventData);

        MouseOver(this, EventArgs.Empty);
    }

    public override void OnPointerExit(PointerEventData eventData) {
        base.OnPointerExit(eventData);

        MouseOut(this, EventArgs.Empty);
    }

    //public override void OnBeginDrag(PointerEventData eventData) {
    //    base.OnBeginDrag(eventData);
    //}

    public override void OnDrag(PointerEventData eventData) {
        base.OnDrag(eventData);

        transform.position += (Vector3)eventData.delta;
    }

    public override void OnEndDrag(PointerEventData eventData) {
        base.OnEndDrag(eventData);

        if (mRectTransform.Overlaps(mPlacingCardRectTransform)) {
            mCardManager.SelectCard(this);
        }
        else {
            mCardManager.UnSelectCard(this);
        }

        EndDrag(this, EventArgs.Empty);
    }
}
