using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
	//fields set in Unity Inspector pane
	[Header("Set in Inspector")]
	public GameObject 	prefabProjectile;
	public float 		velocityMult = 8f;
	
	// fields set dynamically
	[Header("Set dynamically")]
    public GameObject 	launchPoint;
	public Vector3 		launchPos;
	public GameObject 	projectile;
	public bool			aimingMode;
	private Rigidbody	projectileRigidbody;
	
	void Awake(){
		Transform launchPointTrans = transform.Find("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive( false );
		launchPos = launchPointTrans.position;
	}
    void OnMouseEnter() {
		launchPoint.SetActive( true );
	}
	
	void OnMouseExit() {
		launchPoint.SetActive( false );
	}
	void OnMouseDown() {
		//Player pressed button while over slingshot
		aimingMode = true;
		
		//Instantiate a projectile
		projectile = Instantiate( prefabProjectile ) as GameObject;
		
		//start at launchpoint
		projectile.transform.position = launchPos;
		
		//set it to isKinematic for now
		projectileRigidbody = projectile.GetComponent<Rigidbody>();
		projectileRigidbody.isKinematic = true;
	}
	
	void Update() {
		//if slingshot not in aiming mode, cancel
		if (!aimingMode) return;
		
		//Get current mouse position in 2d screen coordinates
		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
		
		//Find the delta from the launchPos to the mousePos3D
		Vector3 mouseDelta = mousePos3D - launchPos;
		
		//Limit mouse delta to the radius of the Slingshot SphereCollider
		float maxMagnitude = this.GetComponent<SphereCollider>().radius;
		if (mouseDelta.magnitude > maxMagnitude) {
			mouseDelta.Normalize();
			mouseDelta *= maxMagnitude;
		}
		
		//Move the projectile to the new Position
		Vector3 projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;
		
		if ( Input.GetMouseButtonUp(0)) {
			//The mouse has been released
			aimingMode = false;
			projectileRigidbody.isKinematic = false;
			projectileRigidbody.velocity = -mouseDelta * velocityMult;
			projectile = null;
		}
		
		
		
		
	}
}
