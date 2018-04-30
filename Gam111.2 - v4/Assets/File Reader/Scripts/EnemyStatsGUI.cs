using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatsGUI : MonoBehaviour {
    //the character's stats stored as integers in here for the purpose of destruction and ToString()-
    //in order: Health, Armor, Power, Energy.
    int hP;
    int aP;
    int pWR;
    int nRG;
    [SerializeField]
    Text healthPoints;
    [SerializeField]
    Text armorPoints;
    [SerializeField]
    Text power;
    [SerializeField]
    Text energy;
    Character character;
    [SerializeField]
    GameObject correctSpawn;
    GameObject[] correctEnemy;
    GameObject player;

    public void FindCorrectCharacter() {
        if (correctSpawn.name != "PlayerSpawn") {
            for (int i = 0; i < 4; i++) {
                correctEnemy = GameObject.FindGameObjectsWithTag("Enemy");
                if (correctEnemy != null) {
                    if (correctSpawn.name == correctEnemy[i].name) {
                        character = correctEnemy[i].GetComponent<Character>();
                    }
                }
                else {
                    Destroy(gameObject);
                }
            }
        }
        else {
            player = GameObject.FindGameObjectWithTag("Player");
            character = player.GetComponent<Character>();
        }
    }
    void Update() {
        if (character != null) {
            pWR = character.stats.power;
            nRG = character.stats.energy;
            hP = character.trueHealth;
            aP = character.temporaryHealth;
            healthPoints.text = hP.ToString();
            armorPoints.text = aP.ToString();
            power.text = pWR.ToString();
            energy.text = nRG.ToString();

            if (hP <= 0) {
                Destroy(gameObject);
            }
        }
    }
}