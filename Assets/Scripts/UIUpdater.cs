using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

    protected void UpdateImageColor(String name, Color color) {
        Image image = mPanelTransform.Find(name).GetComponent<Image>();

        image.color = color;
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

    protected void UpdateSlot(String[] names, GameObject obj) {
        Transform parent = mPanelTransform.Find(names[0]);
        for (int i = 1; i < names.Length - 1; i++) {
            parent = parent.Find(names[i]);
        }

        Vector3 slotPosition = parent.Find(names[names.Length - 1]).transform.position;

        obj.transform.position = slotPosition;
        obj.transform.SetParent(mPanelTransform);
    }
}
