using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {

    public List<Transform> destructionPoints;
    public GameObject niceHouse;
    public GameObject trashedHouse;
    public ParticleSystem[] destroyParticles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DestroyHouse()
    {
        niceHouse.SetActive(false);
        trashedHouse.SetActive(true);
    }

    public void ActivateDestroyParticle()
    {
        foreach(ParticleSystem part in destroyParticles)
        {
            if(!part.isPlaying)
            {
                part.Play();
                break;
            }
        }
    }

    public Transform GetDestructionPoint()
    {
        Transform dPoint;
        int randomTransformIndex = Random.Range(0, destructionPoints.Count);
        dPoint = destructionPoints[randomTransformIndex];
        destructionPoints.RemoveAt(randomTransformIndex);
        return dPoint;
    }
}
