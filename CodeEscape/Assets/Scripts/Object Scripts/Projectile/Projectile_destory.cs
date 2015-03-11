using UnityEngine;
using System.Collections;

public class Projectile_destory : MonoBehaviour {


	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Untagged")
			Destroy(gameObject);
	}
}
