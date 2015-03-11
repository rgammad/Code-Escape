using UnityEngine;
using System.Collections;

public class Platform_2_Trigger : MonoBehaviour {
	
	public GameObject platform;

	void Start(){}

	void OnTriggerEnter(Collider collision) {
		if(collision.gameObject.tag == "move_2"){
			platform.animation.Play();
			Destroy (gameObject);
			Destroy(collision.gameObject);

		}
	}
}
