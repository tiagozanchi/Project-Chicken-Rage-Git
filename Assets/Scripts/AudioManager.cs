using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance = null;

	public AudioClip[] audios;
	public AudioClip[] BGMs;

	private AudioSource aSource;
	private AudioSource BGMSource;

	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		aSource = GetComponent<AudioSource> ();
		BGMSource = transform.GetChild (0).GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayOnce(int audioId, float volume = 1f)
	{
		aSource.PlayOneShot (audios [audioId],volume);
	}

	public void BattleBGM()
	{
		Invoke ("PlayBGM", 2f);
	}

	public void PlayBGM()
	{
		BGMSource.clip = BGMs [0];
		BGMSource.Play ();
	}
}
