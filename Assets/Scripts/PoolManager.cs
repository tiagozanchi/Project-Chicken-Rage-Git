using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    public static PoolManager instance = null;

    public GameObject explosionPrefab;
    private List<GameObject> explosionList = new List<GameObject>();

    public GameObject normalChickenPrefab;
    private List<GameObject> normalChickenList = new List<GameObject>();

    public GameObject explosiveChickenPrefab;
    private List<GameObject> explosiveChickenList = new List<GameObject>();

	public GameObject healerChickenPrefab;
	private List<GameObject> healerChickenList = new List<GameObject>();

	public GameObject eggPrefab;
	private List<GameObject> eggList = new List<GameObject> ();

	public GameObject eggOrgPrefab;
	private List<GameObject> eggOrbList = new List<GameObject> ();

	public GameObject poisonParticlePrefab;
	private List<GameObject> poisonPartList = new List<GameObject> ();

	public GameObject damageTextPrefab;
	private List<GameObject> dmgTextList = new List<GameObject>();

	public GameObject towerDestroyPrefab;
	private List<GameObject> towerDestroyList = new List<GameObject> ();

	private Transform effectCanvas;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
		effectCanvas = GameObject.FindGameObjectWithTag ("EffectCanvas").transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadNormalChicken(int quantity)
    {

        for (int i = 0; i < quantity; i++)
        {
			GameObject normalChickTemp = Instantiate(normalChickenPrefab,transform.position, Quaternion.identity) as GameObject;
            normalChickenList.Add(normalChickTemp);
            normalChickTemp.SetActive(false);
        }
    }

    public void LoadExplosiveChicken(int quantity)
    {

        for (int i = 0; i < quantity; i++)
        {
			GameObject tempExplChick = Instantiate(explosiveChickenPrefab,transform.position, Quaternion.identity) as GameObject;
            explosiveChickenList.Add(tempExplChick);
            tempExplChick.SetActive(false);

            GameObject tempExplosion = Instantiate(explosionPrefab) as GameObject;
            explosionList.Add(tempExplosion);
            tempExplosion.SetActive(false);
        }
    }

	public void LoadHealerChicken(int quantity)
	{

		for (int i = 0; i < quantity; i++)
		{
			GameObject healerChickTemp = Instantiate(healerChickenPrefab,transform.position, Quaternion.identity) as GameObject;
			healerChickenList.Add(healerChickTemp);
			healerChickTemp.SetActive(false);
		}
	}


	public void LoadEggs()
	{
		for (int i = 0; i < 30; i++)
		{
			GameObject tempEgg = Instantiate(eggPrefab ,transform.position, Quaternion.identity) as GameObject;
			eggList.Add(tempEgg);
			tempEgg.SetActive(false);
		}
	}

	public void LoadEggsOrbs()
	{
		while (effectCanvas == null) {
			effectCanvas = GameObject.FindGameObjectWithTag ("EffectCanvas").transform;
		}

		for (int i = 0; i < 30; i++)
		{
			GameObject tempEggOrb = Instantiate(eggOrgPrefab ,Vector3.zero, Quaternion.identity, effectCanvas) as GameObject;
			eggOrbList.Add(tempEggOrb);
			tempEggOrb.SetActive(false);
		}
	}

	public void LoadPoisonParticles()
	{
		for (int i = 0; i < 15; i++)
		{
			GameObject tempPoisonPart = Instantiate(poisonParticlePrefab ,transform.position, poisonParticlePrefab.transform.rotation) as GameObject;
			poisonPartList.Add(tempPoisonPart);
			tempPoisonPart.SetActive(false);
		}
	}

	public void LoadDamageText()
	{
		for (int i = 0; i < 50; i++)
		{
			GameObject tempDmgText = Instantiate(damageTextPrefab ,transform.position, Quaternion.identity) as GameObject;
			dmgTextList.Add(tempDmgText);
			tempDmgText.SetActive(false);
		}
	}

	public void LoadTowerDestroy()
	{
		for (int i = 0; i < 2; i++)
		{
			GameObject tempTowerDestroy = Instantiate(towerDestroyPrefab ,transform.position, Quaternion.identity) as GameObject;
			towerDestroyList.Add(tempTowerDestroy);
			tempTowerDestroy.SetActive(false);
		}
	}

	public GameObject GetChick(int chickId)
	{
		if (chickId == 0) {
			return GetNormalChick ();
		} else if (chickId == 1) {
			return GetExplosiveChick ();
		} else if (chickId == 2) {
			return GetHealerChick ();
		} else {
			return new GameObject ();
		}
	}

    public GameObject GetNormalChick()
    {
        foreach (GameObject normalChick in normalChickenList)
        {
            if (!normalChick.activeInHierarchy)
            {
                normalChick.SetActive(true);
                return normalChick;
            }
        }

        return new GameObject();
    }

    public GameObject GetExplosiveChick()
    {
        foreach (GameObject explChick in explosiveChickenList)
        {
            if (!explChick.activeInHierarchy)
            {
                explChick.SetActive(true);
                return explChick;
            }
        }

        return new GameObject();
    }

	public GameObject GetHealerChick()
	{
		foreach (GameObject healChick in healerChickenList)
		{
			if (!healChick.activeInHierarchy)
			{
				healChick.SetActive(true);
				return healChick;
			}
		}

		return new GameObject();
	}

	public GameObject getEgg()
	{
		foreach (GameObject egg in eggList)
		{
			if (!egg.activeInHierarchy)
			{
				egg.SetActive(true);
				return egg;
			}
		}

		return new GameObject();
	}

	public GameObject getEggOrb()
	{
		foreach (GameObject eggOrb in eggOrbList)
		{
			if (!eggOrb.activeInHierarchy)
			{
				if (eggOrb.transform.parent == null)
					eggOrb.transform.SetParent(effectCanvas);
						
				eggOrb.SetActive(true);
				return eggOrb;
			}
		}

		return new GameObject();
	}


    public GameObject GetExplosion()
    {
        foreach(GameObject explosion in explosionList)
        {
            if(!explosion.activeInHierarchy)
            {
                explosion.SetActive(true);
                return explosion;
            }
        }

        return new GameObject();
    }

	public GameObject GetPoisonEffect()
	{
		foreach(GameObject poisonEffect in poisonPartList)
		{
			if(!poisonEffect.activeInHierarchy)
			{
				poisonEffect.SetActive(true);
				return poisonEffect;
			}
		}

		return new GameObject();
	}

	public GameObject GetTowerDestroy()
	{
		foreach(GameObject towerDestroy in towerDestroyList)
		{
			if(!towerDestroy.activeInHierarchy)
			{
				towerDestroy.SetActive(true);
				return towerDestroy;
			}
		}

		return new GameObject();
	}

	public GameObject GetDamageText(int damage)
	{
		GameObject tempDmgHealText = null;
		TextMesh tempTM = null;

		foreach(GameObject dmgText in dmgTextList)
		{
			if(!dmgText.activeInHierarchy)
			{
				tempDmgHealText = dmgText;
				tempTM = tempDmgHealText.GetComponent<TextMesh> ();
				break;
			}
		}

		if (damage > 0) {
			tempTM.color = Color.green;
		} else {
			tempTM.color = Color.red;
		}

		tempTM.text = damage.ToString();
		tempDmgHealText.SetActive (true);
		return tempDmgHealText;
	}

}
