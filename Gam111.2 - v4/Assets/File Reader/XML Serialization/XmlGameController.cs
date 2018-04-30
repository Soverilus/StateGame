using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XmlGameController : MonoBehaviour {
    GameObject[] enemySpawns;
	[SerializeField]
	EnemyStatsGUI[] enemyStatsGUI;
	[SerializeField]
	EnemyStatsGUI playerStatsGUI;
	[SerializeField]
	GameObject[] enemyPrefabs;
    GameObject playerSpawn;
    GameController gameController;
	[SerializeField]
	GameObject playerPrefab;
    void Start() {
        gameController = GetComponent<GameController>();
        enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
        CharacterStatsContainer characterStatsContainer = new CharacterStatsContainer();

        //the player classes; Knight, Barbarian, Mage
        CharacterStats character1 = new CharacterStats();
        character1.name = "Knight";
        character1.maxHealth = 80;
        character1.power = 6;
        character1.energy = 4;
        characterStatsContainer.characterStatsList.Add(character1);

        /*CharacterStats character2 = new CharacterStats();
        character2.name = "Barbarian";
        character2.maxHealth = 60;
        character2.power = 8;
        character2.energy = 2;
        characterStatsContainer.characterStatsList.Add(character2);

        CharacterStats character3 = new CharacterStats();
        character3.name = "Mage";
        character3.maxHealth = 40;
        character3.power = 6;
        character3.energy = 8;
        characterStatsContainer.characterStatsList.Add(character3);*/

        CharacterStatsContainer enemyStatsContainer = new CharacterStatsContainer();

        //the enemy classes: Bat, Rabbit, Ghost, Slime. These are multiplied by level.
        CharacterStats enemy1 = new CharacterStats();
        enemy1.name = "Bat";
        enemy1.maxHealth = 6;
        enemy1.power = 3;
        enemy1.energy = 3;
        enemyStatsContainer.characterStatsList.Add(enemy1);

        CharacterStats enemy2 = new CharacterStats();
        enemy2.name = "Rabbit";
        enemy2.maxHealth = 9;
        enemy2.power = 5;
        enemy2.energy = 1;
        enemyStatsContainer.characterStatsList.Add(enemy2);

        CharacterStats enemy3 = new CharacterStats();
        enemy3.name = "Ghost";
        enemy3.maxHealth = 12;
        enemy3.power = 1;
        enemy3.energy = 5;
        enemyStatsContainer.characterStatsList.Add(enemy3);

        /*CharacterStats enemy4 = new CharacterStats();
        enemy4.name = "Slime";
        enemy4.maxHealth = 8;
        enemy4.power = 4;
        enemy4.energy = 3;
        enemyStatsContainer.characterStatsList.Add(enemy4);*/

        characterStatsContainer.Save(GetPath("characterStats.xml"));
        enemyStatsContainer.Save(GetPath("enemyStats.xml"));

		characterStatsContainer.Load(GetPath("characterStats.xml"));
        enemyStatsContainer.Load(GetPath("enemyStats.xml"));

		//Debug.Log(characterStatsContainer.characterStatsList[0].name);
        //Debug.Log(enemyStatsContainer.characterStatsList.Count);
        //Debug.Log(enemy3.energy);

		//affect player, then
		int playerType = 0;
		int playerLevel = 1;
		GameObject playeri = Instantiate (playerPrefab, playerSpawn.transform.position, playerSpawn.transform.rotation);
		Character player = playeri.GetComponent<Character> ();
		CharacterStats playerTypeStats = characterStatsContainer.characterStatsList [playerType];
		player.stats.name = playerTypeStats.name;
		player.stats.maxHealth = playerTypeStats.maxHealth;
		player.stats.energy = playerTypeStats.energy;
		player.stats.power = playerTypeStats.power;
		player.level = playerLevel;
		player.StatMult ();
        gameController.FindPlayer();
		playerStatsGUI.FindCorrectCharacter ();

		//script for instantiation enemies (or not) randomly.
		for (int i = 0; i < enemySpawns.Length; i++) {
			int enemyType = Random.Range (0, enemyStatsContainer.characterStatsList.Count);
			//Debug.Log (enemyStatsContainer.characterStatsList.Count);
			int enemyLevel = Random.Range (1, 4);
			//Debug.Log (enemyLevel);
			//a reference to our instantiated enemy
			if (enemyType >= 0) {
				GameObject enemyi = Instantiate (enemyPrefabs [enemyType], enemySpawns [i].transform.position, enemySpawns [i].transform.rotation);
				Character enemy = enemyi.GetComponent<Character> ();
				//an instance of the characterStatsList deserialized list.
				CharacterStats typeStats = enemyStatsContainer.characterStatsList [enemyType];
				enemy.transform.position = GameObject.Find ("Enemy" + i).transform.position;
				enemy.transform.rotation = GameObject.Find ("Enemy" + i).transform.rotation;
				enemy.name = "Enemy" + i;
				enemy.stats.name = typeStats.name;
				enemy.stats.maxHealth = typeStats.maxHealth;
				enemy.stats.energy = typeStats.energy;
				enemy.stats.power = typeStats.power;
				enemy.level = enemyLevel;
				enemy.StatMult ();
			}
		}
		//tells the gamecontroller to findenemies - I do this so it doesn't mistakenly find nothing, then spawn. (as would happen if I put the function in start)
		// - in a way, "FindEnemies()" and "FindPlayer()" both serve the same function as a Start function - only delayed.
        gameController.FindEnemies();
		for (int i = 0; i < enemyStatsGUI.Length; i++) {
			enemyStatsGUI [i].FindCorrectCharacter ();
		}
    }

    string GetPath(string fileName) {
        return Application.dataPath + "/" + fileName;
    }
}