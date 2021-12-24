using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceAction : MonoBehaviour
{
    public Button AttackButton;
    public Button MoveButton;
    public Button BuildButton;
    public Button PlayCardButton;
    public Button SleepButton;

    public void UpdatePieceAction(BasePiece piece) {
        AttackButton.interactable = !piece.IsOutOfEnergy();
        MoveButton.interactable = !piece.IsOutOfEnergy();
        BuildButton.interactable = !piece.IsOutOfEnergy();
        PlayCardButton.interactable = !piece.IsOutOfEnergy();
    }
}
