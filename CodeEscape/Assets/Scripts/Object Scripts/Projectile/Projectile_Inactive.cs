using UnityEngine;
using System.Collections;

public class Projectile_Inactive : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag == "Untagged")
			Destroy(gameObject);
	}
	
}