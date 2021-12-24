using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPiece : MonoBehaviour
{
    public PieceManager mPieceManager;
    public ResourceHandler mResourceHandler;

    public GameObject[] mPanelsOfButtons;

    public void UpdateSpawnPiece() {
        foreach (var panelOfButtons in mPanelsOfButtons) {
            Button[] buttons = panelOfButtons.GetComponentsInChildren<Button>(true);
            SpawnPieceButtonData panelData = panelOfButtons.GetComponentInChildren<SpawnPieceButtonData>(true);

            foreach (var button in buttons) {
                SpawnPieceButtonData data = button.GetComponent<SpawnPieceButtonData>();

                if (data) {
                    Type pieceType = mPieceManager.mPieceTypes[data.mSpawnTypeIndex];
                    AttackRange pieceRange = data.mAttackRange;

                    button.interactable = mResourceHandler.IsEnoughResourceToSpawn(pieceType, pieceRange);
                }
                else {
                    Type pieceType = mPieceManager.mPieceTypes[panelData.mSpawnTypeIndex];

                    button.interactable = mResourceHandler.IsEnoughResourceToSpawn(pieceType);
                }
            }
        }
    }
}
