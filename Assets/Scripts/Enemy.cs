using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public int hp;
    public float speed;
    public Image healthImage;
    public int damage;
    public Animator anim;
	public int eggsDrop;

    public bool attacking = false;
    private int maxHP;
	private NavMeshAgent navMesh;
    //private Rigidbody rb;
	private Vector3 destination;

	private Material mat;

	private bool alive = false;
	private bool poisoned = false;
	private int poisonDamage;
	private GameObject poisonEffect;
	private Vector3 spawnPos;

	// Use this for initialization
	void Start () {
		spawnPos = transform.position;
		mat = transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<MeshRenderer> ().material;
		navMesh = GetComponent<NavMeshAgent> ();
		//rb = GetComponent<Rigidbody>();
		destination = GameObject.FindGameObjectWithTag("EnemyDestination").transform.position;

		maxHP = hp;
		navMesh.speed = speed;
		navMesh.SetDestination (destination);
		alive = true;
	}

    // Update is called once per frame
    void Update() {
        if (navMesh.enabled)
        {
			if (navMesh.remainingDistance <= 0.5f && navMesh.hasPath)
            {
				ActionOnHouse ();
            }
        }
	}

    void TakeHit(int hit)
    {
        hp -= hit;

		GameObject dmgText = PoolManager.instance.GetDamageText (-hit);
		dmgText.transform.position = transform.position + (transform.up * 5f);

        if(hp <= 0)
        {
			Die();
        }
        else
        {
            UpdateHealth();
        }
    }

    public void HitKill()
    {
        TakeHit(hp);
    }

    void UpdateHealth()
    {
        healthImage.fillAmount = (float)hp / maxHP;
    }

	/*void OnDisable()
	{
		if (alive) {
			
		}
	}*/

	public virtual void OnEnable()
	{
		if (alive && navMesh.isOnNavMesh) {
			navMesh.SetDestination (destination);
		}
	}

	void Freeze(int damage)
	{
		TakeHit (damage);
		mat.color = new Color (0.35f, 0.6f, 1f);
		navMesh.speed = speed / 2;
		CancelInvoke ("Unfreeze");
		Invoke ("Unfreeze", 3f);
	}

	void Unfreeze()
	{
		if (poisoned)
			mat.color = new Color (0.19f, 0.58f, 0.22f);
		else
			mat.color = Color.white;
		
		navMesh.speed = speed;
	}

	void Poisoned(int damage)
	{
		if (!poisoned) {
			poisoned = true;
			poisonDamage = damage;
			mat.color = new Color (0.19f, 0.58f, 0.22f);
			GameObject poisonEffectTemp = PoolManager.instance.GetPoisonEffect ();
			poisonEffectTemp.transform.parent = transform;
			poisonEffectTemp.transform.localPosition = new Vector3 (0, 1.9f, 0.14f);
			poisonEffect = poisonEffectTemp;
			InvokeRepeating ("PoisonHit", 1f,1f);
		}
	}

	void PoisonHit()
	{
		TakeHit (poisonDamage);
	}

	public virtual void ActionOnHouse()
	{
		navMesh.enabled = false;
		//rb.isKinematic = true;
	}

	public virtual void Die()
	{
		for (int i = -1; i < eggsDrop; i++) {
			GameObject tempEgg = PoolManager.instance.getEgg ();
			tempEgg.transform.position = transform.position + (transform.right * i);
		}

		if (poisonEffect != null) {
			poisonEffect.transform.parent = null;
			poisonEffect.SetActive (false);
			poisonEffect = null;
		}

		hp = maxHP;
		UpdateHealth ();
		CancelInvoke ();
		poisoned = false;
		Unfreeze ();
		transform.position = spawnPos;
		gameObject.SetActive(false);
	}

	void Heal(int amount)
	{
		hp += amount;
	
		if (hp > maxHP)
			hp = maxHP;

		UpdateHealth ();
		GameObject healText = PoolManager.instance.GetDamageText (amount);
		healText.transform.position = transform.position + (transform.up * 5f);
	}
}
