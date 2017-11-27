using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [Header("Enemies To Load")]
    public bool normalChicken;
    public bool explosiveChicken;
	public bool healerChicken;

    [Header("Waves")]
    public int[] enemiesSequence;
	public Vector2[] enemiesAndDelay;

	private int waveNum = 0;
	private int enemiesSeq = 0;
	private int enemiesTo = 1;
	private List<GameObject> activeEnemies = new List<GameObject>();

    // Use this for initialization
    void Start () {
		PoolManager.instance.LoadEggs ();
		PoolManager.instance.LoadEggsOrbs ();
		PoolManager.instance.LoadPoisonParticles ();
		PoolManager.instance.LoadDamageText ();
		PoolManager.instance.LoadTowerDestroy ();

        if (normalChicken)
            PoolManager.instance.LoadNormalChicken(25);

        if(explosiveChicken)
            PoolManager.instance.LoadExplosiveChicken(25);

		if(healerChicken)
			PoolManager.instance.LoadHealerChicken(15);


		enemiesTo = (int)enemiesAndDelay [waveNum].x;
    }

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			StartWaves ();
		}
	}

	public void StartWaves()
	{
		StartCoroutine(SpawnHave());
	}


	IEnumerator SpawnHave()
	{
		for (int i = enemiesSeq; i < enemiesTo; i++) {
			activeEnemies.Add(PoolManager.instance.GetChick (enemiesSequence [i]));
			yield return new WaitForSeconds (0.5f);
		}


		if (waveNum+1 < enemiesAndDelay.Length) {
			
			Invoke ("StartWaves", enemiesAndDelay [waveNum].y);
			enemiesSeq = (int)enemiesAndDelay [waveNum].x;
			waveNum++;
			enemiesTo = (int)enemiesAndDelay [waveNum].x+enemiesSeq;
		}
		else
		{
			InvokeRepeating("CheckLevelFinish",0f,1f);
		}
	}

	void CheckLevelFinish()
	{
		foreach(GameObject enemy in activeEnemies)
		{
			if(enemy.activeInHierarchy)
			{
				return;
			}
		}
	
		CancelInvoke();
		GameController.instance.LevelSuccess();
	}
}

