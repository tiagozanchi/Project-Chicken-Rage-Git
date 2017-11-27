using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour {

    private LineRenderer lr;
    private Transform linePoint1;
    private Transform linePoint2;
    private Transform linePoint3;
    private Rect middleScreen;

    private bool arrowLoaded = false;
    private bool drawingArrow = false;
    private float arrowStrength;

    private int arrowIndex = 0;
    private GameObject[] arrows = new GameObject[10];
    private GameObject currentArrow;
    private Rigidbody cArrowRB;
    private BoxCollider cArrowBC;
	private AudioSource aSource;

    public int crosshairSize;
    public Texture crosshairTex;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public Transform arrowDrawPoint;
	public AudioClip[] bowSounds;

	// Use this for initialization
	void Start () {
		aSource = GetComponent<AudioSource> ();
        lr = GetComponent<LineRenderer>();
        linePoint1 = transform.GetChild(0).transform;
        linePoint3 = transform.GetChild(1).transform;

        middleScreen = new Rect((Screen.width / 2) - crosshairSize/2, (Screen.height / 2) - crosshairSize/2, crosshairSize, crosshairSize);
        middleScreen.center = new Vector2(Screen.width / 2, Screen.height / 2);

        for(int i = 0; i < 10; i++)
        {
            GameObject tempArrow = Instantiate(arrowPrefab);
            arrows[i] = tempArrow;
            tempArrow.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(!arrowLoaded)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, linePoint1.position);
            lr.SetPosition(1, linePoint3.position);
        }
        else
        {
            lr.positionCount = 3;
            lr.SetPosition(0, linePoint1.position);
            lr.SetPosition(1, linePoint2.position);
            lr.SetPosition(2, linePoint3.position);
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(arrowLoaded)
            {
                drawingArrow = true;
				aSource.clip = bowSounds [0];
				aSource.Play ();
                StartCoroutine(DrawArrow());
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(drawingArrow)
            {
				aSource.PlayOneShot (bowSounds [1]);
                middleScreen.width = middleScreen.height = crosshairSize;
                middleScreen.center = new Vector2(Screen.width / 2, Screen.height / 2);
                currentArrow.transform.parent = null;
                cArrowRB.useGravity = true;
                cArrowRB.AddRelativeForce(Vector3.forward * (arrowStrength*10));
                cArrowBC.enabled = true;
                drawingArrow = false;
                arrowLoaded = false;
                //currentArrow.SendMessage("Activate");
                SpawnArrow();
            }
        }
	}

    private void OnGUI()
    {
        GUI.DrawTexture(middleScreen, crosshairTex);
    }

    IEnumerator DrawArrow()
    {
        arrowStrength = 0;
		while (currentArrow.transform.localPosition != arrowDrawPoint.localPosition && drawingArrow && arrowStrength <= 200)
        {
            middleScreen.size *= 0.99f;
            middleScreen.center = new Vector2(Screen.width / 2, Screen.height / 2);
            arrowStrength += 2;
            currentArrow.transform.localPosition = Vector3.Lerp(currentArrow.transform.localPosition, arrowDrawPoint.localPosition, 0.05f);
            yield return new WaitForEndOfFrame();
        }

    }

    public void SpawnArrow()
    {
        if(arrowIndex > arrows.Length - 1)
        {
            arrowIndex = 0;   
        }

        currentArrow = arrows[arrowIndex];
        currentArrow.SetActive(false);
        currentArrow.transform.parent = arrowSpawnPoint;
        currentArrow.transform.localPosition = Vector3.zero;
        currentArrow.transform.localRotation = Quaternion.Euler(180f, 270f, 90f);
        arrowIndex++;
        currentArrow.SetActive(true);
        
        arrowLoaded = true;

        cArrowRB = currentArrow.GetComponent<Rigidbody>();
        cArrowBC = currentArrow.GetComponent<BoxCollider>();

        cArrowRB.velocity = Vector3.zero;
        cArrowRB.isKinematic = false;
        cArrowRB.useGravity = false;
        cArrowBC.enabled = false;
        linePoint2 = currentArrow.transform.GetChild(1).transform;
    }

    void OnDisable()
    {
        if(currentArrow)
        {
            currentArrow.SetActive(false);
        }
        
        arrowLoaded = false;
        drawingArrow = false;
        CancelInvoke();
    }

    private void OnEnable()
    {
        //lr.enabled = false;
        //lr.enabled = true;
        Invoke("SpawnArrow", 1f);
    }
}
