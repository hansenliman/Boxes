using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        GetComponent<ParticleSystem>().Play();
        Destroy(gameObject, 2f);
    }
}
