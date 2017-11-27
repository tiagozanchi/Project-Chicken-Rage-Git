using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Cameras;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public Transform spawnPosition;
    public GameObject[] enemies;
    public GameObject[] buildings;
    public LayerMask constructibleMask;
    public Text resourcesText;
    public Animator mainUI;
    public GameObject fpsCharacter;
    public Image houseHPImage;
	public Texture2D[] mouseCursor;
	public Animator sunAnim;
	public LevelManager lm;
	public Transform gameOverPos;

	public House houseScript;
    private int houseHP = 100;
    private int houseMaxHP;
    private int destructionLevel = 0;

	private GameObject menuFPSHide;
	private int currentMouse;
    private int currentBuildingCost;
    private int resources = 500;
    private GameObject tempBuilding;
    private Camera mainCamera;
	private GameObject mainCameraGameObject;
	private Camera mainFPSCamera;
    private bool FPSMode = false;
	private Transform fpsTowerSymbol;

	//
	private bool gameOver = false;
	private bool canConstruct = true;
	private int towersConstructed = 0;
	//

    private bool constructing = false;

	//
	private List<GameObject> enemiesSpawned = new List<GameObject>();
	//

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
		RenderSettings.skybox.SetColor("_Tint",new Color(0.1f,0.1f,0.6f));
        UpdateResources(0);

        Physics.IgnoreLayerCollision(10,10);
        Physics.IgnoreLayerCollision(11, 12);
        Physics.IgnoreLayerCollision(12, 13);
        Physics.IgnoreLayerCollision(14, 12);

		fpsCharacter.SetActive(false);
        mainCamera = Camera.main;
		mainCameraGameObject = mainCamera.transform.parent.parent.gameObject;
		mainFPSCamera = fpsCharacter.transform.GetChild (0).GetComponent<Camera> ();

        houseMaxHP = houseHP;
        UpdateHouseHP();
    }

    // Update is called once per frame
    void Update () {
		if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)) && FPSMode)
			{
				DeactivateFPSMode();
			}

		if (!gameOver) {
			if(Input.GetKeyDown(KeyCode.Q))
			{
				GameObject tempSpawn = PoolManager.instance.GetNormalChick ();
				tempSpawn.transform.position = spawnPosition.position;
				enemiesSpawned.Add(tempSpawn);
			}

			if(Input.GetKeyDown(KeyCode.E))
			{
				GameObject tempSpawn = PoolManager.instance.GetExplosiveChick ();
				tempSpawn.transform.position = spawnPosition.position;
				enemiesSpawned.Add(tempSpawn);
			}

			if(Input.GetKeyDown(KeyCode.R))
			{
				GameObject tempSpawn = PoolManager.instance.GetHealerChick ();
				tempSpawn.transform.position = spawnPosition.position;
				enemiesSpawned.Add(tempSpawn);
			}

			
			if (constructing)
			{
				if (currentMouse != 1) {
					SwitchMouseCursor (1);
				}

				RaycastHit hit;
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit, 100f, constructibleMask))
				{
					if (hit.transform.tag == "Construction")
					{
						tempBuilding.SetActive(false);
					}
					else
					{
						tempBuilding.SetActive(true);
						tempBuilding.transform.position = hit.transform.position;

						if (Input.GetMouseButtonDown(0) && resources >= currentBuildingCost && canConstruct)
						{
							UpdateResources(-currentBuildingCost);
							GameObject cBuilding = tempBuilding;
							tempBuilding = Instantiate(tempBuilding, hit.point, Quaternion.identity);
							cBuilding.SendMessage("Activate");
							towersConstructed++;
						}
					}

					if (Input.GetKeyDown(KeyCode.Escape))
					{
						Destroy(tempBuilding);
						constructing = false;
						SwitchMouseCursor (0);
					}
				}
				else
				{
					if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
					{
						Destroy(tempBuilding);
						constructing = false;
						SwitchMouseCursor (0);
					}

					tempBuilding.SetActive(false);
				}


			}
		}
	}

    public void Construct(int buildingIndex)
    {
        tempBuilding = Instantiate(buildings[buildingIndex], new Vector3(-1000f, 0,0), Quaternion.identity);
        tempBuilding.SetActive(false);
        constructing = true;
		SwitchMouseCursor (1);
    }

    public void SetBuildingCost(int cost)
    {
        currentBuildingCost = cost;
    }

    public void UpdateResources(int amount)
    {
        resources += (amount);
        resourcesText.text = resources.ToString();
    }

	public void ActivateFPSMode(Vector3 pos, Transform towerSymbol)
    {
		if (!gameOver) {
			fpsCharacter.transform.position = pos;
			fpsCharacter.SetActive(true);
			mainCameraGameObject.SetActive (false);
			FPSMode = true;
			//menuFPSHide.SetActive(false);
			fpsTowerSymbol = towerSymbol;
			fpsTowerSymbol.localScale /= 2;
			mainUI.SetBool ("fpsMode", FPSMode);
		}
    }

    public void DeactivateFPSMode()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        fpsCharacter.SetActive(false);
		mainCameraGameObject.SetActive(true);
        FPSMode = false;
		//menuFPSHide.SetActive(true);
		fpsTowerSymbol.localScale = Vector3.one;
		mainUI.SetBool ("fpsMode", FPSMode);
    }

    public bool IsInFPSMode()
    {
        return FPSMode;
    }

    public bool IsConstructing()
    {
        return constructing;
    }

    public void HouseTakeHit(int damage)
    {
        houseHP -= damage;
        float hpPercentage = ((float)houseHP / (float)houseMaxHP) * 100f;

        if(hpPercentage <= 75 && hpPercentage > 50 && destructionLevel == 0)
        {
            houseScript.ActivateDestroyParticle();
            destructionLevel++;
        }
        else if(hpPercentage <= 50 && hpPercentage > 25 && (destructionLevel == 0 || destructionLevel == 1))
        {
            if(destructionLevel == 0)
            {
                houseScript.ActivateDestroyParticle();
                destructionLevel++;
            }

            houseScript.ActivateDestroyParticle();
            houseScript.DestroyHouse();
            destructionLevel++;
        }
        else if(hpPercentage <= 25 && (destructionLevel == 0 || destructionLevel == 1 || destructionLevel == 2))
        {
            if (destructionLevel == 0)
            {
                houseScript.ActivateDestroyParticle();
                destructionLevel++;

                houseScript.ActivateDestroyParticle();
                houseScript.DestroyHouse();
                destructionLevel++;
            }

            if (destructionLevel == 1)
            {
                houseScript.ActivateDestroyParticle();
                houseScript.DestroyHouse();
                destructionLevel++;
            }

            houseScript.ActivateDestroyParticle();
        }

		if (houseHP <= 0 && !gameOver)
        {
			gameOver = true;
            houseHP = 0;
			StartCoroutine ("GameOver");
        }

        UpdateHouseHP();
    }

    void UpdateHouseHP()
    {
        houseHPImage.fillAmount = (float)houseHP / houseMaxHP;
    }

    public Transform GetDestructionPoint()
    {
        return houseScript.GetDestructionPoint();
    }

	public void SwitchMouseCursor(int cursorId)
	{
		Cursor.SetCursor (mouseCursor[cursorId], Vector2.zero, CursorMode.Auto);
		currentMouse = cursorId;
	}

	public int GetActiveCursor()
	{
		return currentMouse;
	}

	public int GetTowersConstructed()
	{
		return towersConstructed;
	}

	public void MinusOneTowerConstructed()
	{
		towersConstructed--;
	}

	public void SetCanConstruct(bool itcan)
	{
		canConstruct = itcan;

		if (!itcan) {
			constructing = itcan;
			if(tempBuilding)
				tempBuilding.SetActive(false);
		}
	}

	public bool GetCanConstruct()
	{
		return canConstruct;
	}

	public void SunRise()
	{
		sunAnim.SetTrigger ("sunrise");
		Invoke ("StartGame", 4f);
	}

	public void StartGame()
	{
		RenderSettings.skybox.SetColor("_Tint",Color.gray);
		DynamicGI.UpdateEnvironment ();
		AudioManager.instance.PlayOnce (0);
		AudioManager.instance.BattleBGM ();
		lm.StartWaves ();
	}

	IEnumerator GameOver()
	{
		if (IsInFPSMode ()) {
			DeactivateFPSMode ();
		}

		Transform camT = mainCamera.transform.parent.parent;
		Transform camTSon = mainCamera.transform;
		camT.GetComponent<FreeLookCam> ().enabled = false;
		camT.GetComponent<ProtectCameraFromWallClip> ().enabled = false;
		camT.GetComponent<CamController> ().enabled = false;

		mainUI.SetTrigger("gameOver");

		while (Vector3.Distance(camTSon.position, gameOverPos.position) > 1f) {
			camTSon.position = Vector3.MoveTowards (camTSon.position, gameOverPos.position, 0.25f);
			camTSon.rotation = Quaternion.Lerp (camTSon.rotation, gameOverPos.rotation, 0.01f);
			yield return new WaitForEndOfFrame ();
		}


		Debug.Log ("ok");
	}

	public void TutorialToggle(bool tut)
	{
		mainUI.SetBool ("tutorial", tut);
	}

	public void SetGameOver(bool value)
	{
		gameOver = value;
	}

	public void LevelSuccess()
	{
		gameOver = true;
		mainUI.SetTrigger("success");
	}

	public void GoToScene(int sceneId)
	{
		if(sceneId != -1)
		{
			SceneManager.LoadScene(sceneId);
		}
		else
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
