using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

	public int length;
	int lifespan = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (lifespan > length) {
			Destroy (gameObject);
		}
		lifespan++;
	}
}
