using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameController : MonoBehaviour {
    enum AttackTypeEnum {
        Attack,
        Skill,
        Defense
    };
    AttackTypeEnum playerAction;
    AttackTypeEnum enemyAttack;
    // the different game states the game can be in
    enum GameState {
        PlayerSelect,
        PlayerTarget,
        PlayerAct,
        EnemySelect,
        EnemyAct
    };
    //a reference to the main audio source
    [SerializeField]
    AudioSource combatMusic;

    //a reference to my in combat UI and the win/lose screen
    [SerializeField]
    GameObject disableEnableUI;
    [SerializeField]
    GameObject loseWinPanel;
    WinLoseController winLose;
    Image winLoseEnable;
    bool fadeMusic = false;

    //the current state that the game is in.
    GameState currentState = GameState.PlayerSelect;

    // the previous state that the game was in for monitoring whether we just changed state.
    //GameState previousFramesState;

    //player character
    Character player;
    GameObject playerGO;

    // the enemy character
    public Character[] enemies;
    GameObject[] enemyGOs;

    // a checker to see if we'll win or not based on the amount of enemies on the scene:
    int enemiesAlive;

    [SerializeField]
    Button[] buttons;
    void Start() {
        winLose = loseWinPanel.GetComponent<WinLoseController>();
        winLoseEnable = loseWinPanel.GetComponent<Image>();
        winLoseEnable.enabled = false;
    }
    void EndGame(bool win) {
        fadeMusic = true;
        disableEnableUI.SetActive(false);
        winLoseEnable.enabled = true;
        loseWinPanel.SetActive(true);
        winLose.WinLose(win);
    }
    public void FindEnemies() {
        enemyGOs = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesAlive = 0;
        enemies = new Character[enemyGOs.Length];
        for (int i = 0; i < enemyGOs.Length; i++) {
            enemies[i] = enemyGOs[i].GetComponent<Character>();
            enemiesAlive++;
        }
    }

    public void FindPlayer() {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update() {
        if (fadeMusic) {
            combatMusic.volume -= 0.01f;
        }
        //game state logic
        switch (currentState) {
            case GameState.PlayerSelect:
                PlayerSelect();
                break;
            case GameState.PlayerTarget:
                PlayerTarget();
                break;
            case GameState.PlayerAct:
                PlayerAct();
                break;
            case GameState.EnemySelect:
                EnemySelect();
                break;
            case GameState.EnemyAct:
                EnemyAct();
                break;
            default:
                break;
        }
        //Update the previousFramesState to the current state
        //previousFramesState = currentState;
    }
    void EnemySelect() {
        EnemySelectAttack();
    }

    void PlayerSelect() {
        if (player.trueHealth == 0) {
            EndGame(false);
        }
        if (enemiesAlive == 0) {
            EndGame(true);
        }
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].interactable = true;
            buttons[i].gameObject.SetActive(true);
        }
    }

    void PlayerAct() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].interactable = false;
            buttons[i].gameObject.SetActive(false);
        }
        if (!IsInvoking("EnemySelectStateChange")) {
            Invoke("EnemySelectStateChange", 2f);
        }
    }
    void EnemyAct() {
        if (!IsInvoking("PlayerSelectStateChange")) {
            for (int i = 0; i < enemies.Length; i++) {
                if (enemies[i].isDead == true) {
                    if (!enemies[i].wasDead) {
                        enemiesAlive -= 1;
                        enemies[i].wasDead = true;
                    }
                    Debug.Log(enemiesAlive);
                }
            }
            if (player.trueHealth <= 0) { //checks if we've lost the game (we have lost all our health)
                EndGame(false);
            }
            else {
                Invoke("PlayerSelectStateChange", 2f);
            }

        }
    }
    public void PlayerSelectAttack() {
        playerAction = AttackTypeEnum.Attack;
        currentState = GameState.PlayerTarget;
    }
    public void PlayerSelectSkill() {
        if (player.skillUse >= 0) {
            playerAction = AttackTypeEnum.Skill;
            currentState = GameState.PlayerTarget;
        }
    }

    public void PlayerSelectDefense() {
        playerAction = AttackTypeEnum.Defense;
        currentState = GameState.PlayerTarget;
    }

    public void EnemySelectAttack() {
        for (int i = 0; i < enemies.Length; i++) {
            int attackRoll = Random.Range(0, 2);
            if (attackRoll == 0) {
                enemies[i].Attack(player);
            }
            else if (attackRoll == 1) {
                enemies[i].Defense();
            }
            else if (attackRoll == 2) {
                if (enemies[i].skillUse < 0) {
                    int actionRoll = Random.Range(0, 1);
                    if (actionRoll == 0) {
                        enemies[i].Attack(player);
                    }
                    else {
                        enemies[i].Defense();
                    }
                }
                else {

                    enemies[i].Skill();
                }
            }
            currentState = GameState.EnemyAct;
        }
    }

    void EnemySelectStateChange() {
        for (int i = 0; i < enemies.Length; i++) {
            enemies[i].OnTurnStart();
        }
        currentState = GameState.EnemySelect;
    }

    void PlayerSelectStateChange() {
        player.OnTurnStart();
        currentState = GameState.PlayerSelect;
    }
    //the script uses this later to determine whether or not the gameobject in question is the object last hit by the raycast we do to select our target
    GameObject lastHitByRay;
    void PlayerTarget() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].interactable = false;
            buttons[i].gameObject.SetActive(false);
        }
        if (playerAction == AttackTypeEnum.Skill) {
            player.Skill();
            currentState = GameState.PlayerAct;
        }
        if (playerAction == AttackTypeEnum.Defense) {
            player.Defense();
            currentState = GameState.PlayerAct;
        }
        else if (playerAction == AttackTypeEnum.Attack) {
            LayerMask enemyLayer;
            enemyLayer = LayerMask.NameToLayer("Enemy");
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << enemyLayer.value); // .value is important! LayerMask.NameToLayer only reports back as "LayerMask" in a debug. Value reports back the actual integer value of the layer.
            if (Input.GetKeyDown(KeyCode.Mouse1)) {
                currentState = GameState.PlayerSelect;
            }
            if (hit.collider != null) {

                /* GameObject example;
                 example = GameObject.FindGameObjectWithTag("Checkers");
                 example.transform.position = hit.point;*/

                GameObject nowHitByRay = hit.collider.gameObject;
                Character target = nowHitByRay.GetComponent<Character>();
                if (lastHitByRay && lastHitByRay != nowHitByRay) lastHitByRay.SendMessage("OnRaycastEnd");
                lastHitByRay = nowHitByRay;
                nowHitByRay.SendMessage("OnRaycastHit");

                if (Input.GetAxis("Fire1") > 0) {
                    currentState = GameState.PlayerAct;
                    if (playerAction == AttackTypeEnum.Attack) {
                        player.Attack(target);
                        nowHitByRay.SendMessage("OnRaycastEnd");
                    }
                    /*if (playerAction == AttackTypeEnum.Skill) {
                        player.Skill();
                    }*/
                }
            }
        }
    }
}