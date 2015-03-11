using UnityEngine;
using System.Collections;

public class Platform_3_Trigger : MonoBehaviour {
	
	public GameObject platform;

	void Start(){}

	void OnTriggerEnter(Collider collision) {
		if(collision.gameObject.tag == "move_3"){
			platform.animation.Play();
			Destroy (gameObject);
			Destroy(collision.gameObject);

		}
	}
}
