using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour 
{

	public Rigidbody projectile; 
	public float speed;
	public int count = 0;
	
	


	void Start(){}

	void Update()
	{
		if(Input.GetMouseButtonDown(0)){
			Collider col = collider;
			print("mouse");
			Camera cam = Camera.main;
			Rigidbody instantiatedProjectile = Instantiate(projectile,transform.position,transform.rotation) as Rigidbody;
			instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0,speed));
			instantiatedProjectile.tag = "open(door)";
			count = count + 1;
			}
		if(count == 1){
			this.enabled = false;
			count = 0;		
		
		}
		
	}
	
			
}
