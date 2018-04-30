using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameController))]
public class InputController : MonoBehaviour {
    GameController gameController;

    private void Start() {
        gameController = GetComponent<GameController>();
    }

    public void SelectAttack(string playerAttack) {
        switch (playerAttack) {
            case "Attack":
                gameController.PlayerSelectAttack();
                break;
            case "Skill":
                gameController.PlayerSelectSkill();
                break;
            case "Defense":
                gameController.PlayerSelectDefense();
                break;
        }
    }
}
