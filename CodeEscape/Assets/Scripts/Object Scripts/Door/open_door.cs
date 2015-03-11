using UnityEngine;
using System.Collections;

public class open_door : MonoBehaviour
{
	public GameObject Door;

	void Start(){}

	void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.tag == "open(door)")
		{
			Door.animation.Play();
			Destroy(gameObject);
			Destroy (collision.gameObject);
		}
		
	}
}

