
using UnityEngine;

public class RigidbodySleep : MonoBehaviour {

	
	void Start () {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.Sleep();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
