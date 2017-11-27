using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public Transform spawnPos;
    public GameObject projectile;
    public float projectileCadence;
    public GameObject projector;
	public int resourcesWhenDestroyed;
	public MeshRenderer[] towerMaterials;
	public Material normalMat;

	private Transform towerSimbol;
	[SerializeField]
    private Transform target;
    private SphereCollider sc;
    private BoxCollider bc;
    private MeshCollider mc;
	private GameObject blocker;
    private bool attacking = false;
    private bool active = false;

	// Use this for initialization
	void Start () {
        sc = GetComponent<SphereCollider>();
        bc = transform.GetChild(0).GetComponent<BoxCollider>();
        mc = transform.GetChild(0).GetComponent<MeshCollider>();
        bc.enabled = false;
		towerSimbol = transform.GetChild (3).transform;
		blocker = transform.GetChild (4).gameObject;
        //mc.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (target != null && !target.gameObject.activeInHierarchy) {
			target = null;
			LookForAnotherTarget();
		}
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && target == null && active)
        {
            attacking = true;
            target = other.transform;
		}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Enemy") && other.transform == target)
        {
			target = null;
			LookForAnotherTarget();
        }
    }

    void Attack()
    {
		if (attacking && target) {
			AudioManager.instance.PlayOnce (7,0.6f);
			GameObject tempProjectile = Instantiate (projectile, spawnPos.position, Quaternion.identity);
			tempProjectile.SendMessage ("SetTarget", target);
		}
    }

    void LookForAnotherTarget()
    {
        attacking = false;
        
        Collider[] hits = Physics.OverlapSphere(transform.position, sc.radius);

        foreach (Collider hit in hits)
        {
            if (hit.tag == "Enemy" && hit.transform != target)
            {
                attacking = true;
                target = hit.transform;
				break;
            }
        }
    }

    void ProjectorToggle(bool active)
    {
        projector.SetActive(active);

		if (active) {
			GameController.instance.SwitchMouseCursor (3);
		} else {
			GameController.instance.SwitchMouseCursor (0);
		}
    }

	private void OnMouseOver()
	{
		int aGameCursor = GameController.instance.GetActiveCursor ();

		if (aGameCursor == 3 && Input.GetKey (KeyCode.LeftShift)) {
			GameController.instance.SwitchMouseCursor (2);
		}

		if (aGameCursor == 2 && !Input.GetKey (KeyCode.LeftShift)) {
			GameController.instance.SwitchMouseCursor (3);
		}
	}

    private void OnMouseEnter()
    {
        if(!GameController.instance.IsInFPSMode())
            ProjectorToggle(true);
    }

    private void OnMouseDown()
    {
		if(!GameController.instance.IsInFPSMode() && !GameController.instance.IsConstructing() && !Input.GetKey(KeyCode.LeftShift))
        {
			GameController.instance.ActivateFPSMode(spawnPos.position, towerSimbol);
        }

		if(!GameController.instance.IsInFPSMode() && !GameController.instance.IsConstructing() && Input.GetKey(KeyCode.LeftShift))
		{
			AudioManager.instance.PlayOnce (6);
			PoolManager.instance.GetTowerDestroy ().transform.position = transform.position;
			GameController.instance.MinusOneTowerConstructed ();
			GameController.instance.SwitchMouseCursor (0);
			GameController.instance.UpdateResources (resourcesWhenDestroyed);
			Destroy (gameObject);
		}
    }

    private void OnMouseExit()
    {
        ProjectorToggle(false);
    }

    void Activate()
    {
		AudioManager.instance.PlayOnce (5);
        ProjectorToggle(false);
        bc.enabled = true;
        sc.enabled = true;
        active = true;
        mc.enabled = true;
		blocker.SetActive (true);
        InvokeRepeating("Attack", 0f, projectileCadence);

		foreach (MeshRenderer mat in towerMaterials)
		{
			mat.material = normalMat;
		}
    }
}
