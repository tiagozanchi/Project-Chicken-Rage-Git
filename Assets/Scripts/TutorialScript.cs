using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class TutorialScript : MonoBehaviour {

	public string[] tutorialText;
	public bool[] showNextText;
	public RectTransform[] highlightObj;
	public string[] arrowOri;
	public Animator tutorialBox;
	public Text tutorialTextBox;
	public GameObject HighlightObject;
	private RectTransform arrow;

	public FirstPersonController fpsController;
	public Bow bow;

	private int tutorialId;

	private RectTransform left;
	private RectTransform right;
	private RectTransform up;
	private RectTransform down;

	private Vector3 prevMousePos;

	private bool allower = false;

	// Use this for initialization
	void Start () {
		arrow = HighlightObject.transform.GetChild (0).GetComponent<RectTransform>();
		/*left = HighlightObject.transform.GetChild (1).GetComponent<RectTransform>();
		up = HighlightObject.transform.GetChild (2).GetComponent<RectTransform>();
		down = HighlightObject.transform.GetChild (3).GetComponent<RectTransform>();*/

		//PlayerPrefs.SetInt ("tutorial", 0);

		if(PlayerPrefs.HasKey("tutorial") && PlayerPrefs.GetInt("tutorial") == 1)
		{
			Destroy (gameObject);
		}
		else
		{
			GameController.instance.SetCanConstruct (false);
			Invoke ("ShowTutorial", 2.5f);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (tutorialId == 3  && !GameController.instance.GetCanConstruct () && GameController.instance.GetTowersConstructed () == 0) {
			GameController.instance.SetCanConstruct (true);
		}

		if (tutorialId == 3 && GameController.instance.GetTowersConstructed () > 0 && GameController.instance.GetCanConstruct ()) {
			GameController.instance.SetGameOver (true);
			GameController.instance.SetCanConstruct (false);
			ShowTutorial ();
		}

		if (tutorialId == 3 && Input.GetKeyDown (KeyCode.Escape)) {
			HideTutorial ();
		}

		if (tutorialId == 5 && !allower) {
			allower = true;
			GameController.instance.SetGameOver (false);
		}

		if (tutorialId == 5 && GameController.instance.IsInFPSMode () && bow.enabled) {
			GameController.instance.SetGameOver (true);
			ShowTutorial ();
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			fpsController.enabled = false;
			bow.enabled = false;
		}

		if (tutorialId == 7 && !bow.enabled) {
			fpsController.enabled = true;
			bow.enabled = true;
		}

		if (tutorialId == 7 && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))) {
			ShowTutorial ();
		}

		if (tutorialId == 8 && GameController.instance.GetTowersConstructed() == 0 && Input.GetKey(KeyCode.LeftShift)) {
			ShowTutorial ();
		}

		if ((tutorialId == 10) && Input.GetMouseButton (1) && Input.mousePosition != prevMousePos && allower) {
			GameController.instance.SetGameOver (false);

			HideTutorial ();
			Invoke ("ShowTutorial", 2f);
			allower = false;
		} else if (tutorialId == 10){
			prevMousePos = Input.mousePosition;
		}

		if ((tutorialId == 11) && !allower) {
			allower = true;
			GameController.instance.UpdateResources (100);
			GameController.instance.SetCanConstruct (true);
			GameController.instance.SetGameOver (false);
			GameController.instance.TutorialToggle (false);
			PlayerPrefs.SetInt ("tutorial", 1);
		}

	}

	void DisableTutorial()
	{
		tutorialBox.gameObject.SetActive (false);

		if (showNextText [tutorialId]) {
			tutorialId++;
			ShowTutorial ();
		} else {
			tutorialId++;
		}
	}

	void ShowTutorial()
	{
		//Debug.Log ("Dialog: " + tutorialId);
		GameController.instance.TutorialToggle (true);
		tutorialTextBox.text = tutorialText [tutorialId];
		tutorialBox.gameObject.SetActive (true);

		if(highlightObj[tutorialId] != null){
			Highlight ();
		}
	}

	public void HideTutorial()
	{
		if(tutorialId == 10)
		{
			allower = !allower;
		}

		tutorialBox.SetTrigger ("hide");
		HighlightObject.SetActive (false);
		Invoke ("DisableTutorial", 0.5f);
	}

	void Highlight()
	{
		HighlightObject.SetActive (true);
		Vector3 arrowPos;
		Quaternion arrowRot;

		if(arrowOri[tutorialId] == "down")
		{
			arrowPos = new Vector3(highlightObj[tutorialId].position.x, highlightObj[tutorialId].position.y + highlightObj[tutorialId].rect.yMin);
			arrowRot = Quaternion.Euler(0f,0f,90f);
		}
		else
		{
			arrowPos = new Vector3(highlightObj[tutorialId].position.x, highlightObj[tutorialId].position.y + highlightObj[tutorialId].rect.yMax);
			arrowRot = Quaternion.Euler(0f,0f,270f);
		}

		arrow.position = arrowPos;
		arrow.rotation = arrowRot;
		/*left.transform.position = new Vector2(target.position.x + target.rect.xMin, target.position.y);
		left.sizeDelta = new Vector2 (Mathf.Abs(left.transform.position.x), target.rect.size.y);

		right.transform.position = new Vector2(target.position.x + (target.pivot.x * target.rect.width), target.position.y);
		right.sizeDelta = new Vector2 (Screen.width - right.position.x, target.rect.size.y);

		up.transform.position = new Vector2(Screen.width/2, target.position.y + (target.pivot.y * target.rect.height));
		up.sizeDelta = new Vector2 (Screen.width, Screen.height - up.position.y);

		down.transform.position = new Vector2(Screen.width/2, target.position.y - (target.pivot.y * target.rect.height));
		down.sizeDelta = new Vector2 (Screen.width, Screen.height + down.position.y);*/
	}

	void CancelFPS()
	{
		ShowTutorial ();
		fpsController.enabled = false;
		bow.enabled = false;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}
