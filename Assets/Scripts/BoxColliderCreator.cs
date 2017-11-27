using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderCreator : MonoBehaviour {

	public MeshRenderer mr;
	public LayerMask constructionMask;

	// Use this for initialization
	void Start () {
		
		/* ESSA MANEIRA FUNCIONA MUITO BEM SE TUDO QUE ESTIVER NO MAPA
		 * FOR MULTIPLO DE 4
		 * 
		 **/ for (float x = mr.bounds.min.x; x < mr.bounds.max.x; x++) {

			for (float z = mr.bounds.min.z; z < mr.bounds.max.z; z++) {
				Vector3 boxCenter = new Vector3(x+2,mr.bounds.max.y,z+2);
				Vector3 boxSize = new Vector3 (4f, 1f, 4f);
				Collider[] objs = Physics.OverlapBox (boxCenter, new Vector3(1.8f,3f,1.8f),Quaternion.identity, constructionMask);

				if (objs.Length == 1) {
					GameObject tempBox = new GameObject("box"+x+z);
					tempBox.transform.position = boxCenter;
					tempBox.transform.SetParent (mr.transform, true);
					BoxCollider bcTemp = tempBox.gameObject.AddComponent<BoxCollider> ();
					//tempBox.transform.localPosition = Vector3.zero;
					tempBox.layer = mr.gameObject.layer;

					bcTemp.center = new Vector3(bcTemp.center.x, -0.5f, bcTemp.center.z);
					bcTemp.size = boxSize;

				}
				z += 3;
				//break;
			}
			x+= 3;
			//break;
		}

		Destroy(mr.gameObject.GetComponent<MeshCollider>());
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
