using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIUpdater : MonoBehaviour
{
    public Transform mPanelTransform;

    public abstract void UpdateUI();

    protected void UpdateText(String name, String value) {
        Text text = mPanelTransform.Find(name).GetComponent<Text>();

        text.text = value;
    }

    protected void UpdateImage(String name, String path) {
        Image image = mPanelTransform.Find(name).GetComponent<Image>();

        image.sprite = Resources.Load<Sprite>(path);
        image.color = Color.white;
    }

    protected void ClearImage(String name) {
        Image image = mPanelTransform.Find(name).GetComponent<Image>();

        image.sprite = null;
        image.color = Color.clear;
    }

    protected void UpdateSlot(String name, GameObject obj) {
        Vector3 slotPosition = mPanelTransform.Find(name).transform.position;

        obj.transform.position = slotPosition;
        obj.transform.SetParent(mPanelTransform);
    }
}
