using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSystem : MonoBehaviour 
{
	public List<Cube> cubesTypes = new List<Cube> ();
	public List<int> quantities = new List<int> ();
	public int currentId;
	
	Ray r;
	RaycastHit rh;

	void Update () 
	{

		if(Input.GetKeyDown(KeyCode.JoystickButton4))
		{
			r = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (r, out rh)) {
				if (rh.collider != null) {
					var c = rh.collider.GetComponent<Cube> ();

					if (rh.collider.GetComponent<Cube> () != null) {
						StartCoroutine(DelayedDestruction(rh.collider, 
							c.destructionTime,
							c.id
						));				
					}				
				}			
			}
		}	
		if (Input.GetKeyUp (KeyCode.JoystickButton4)) {
			StopAllCoroutines ();
		}

		if (Input.GetKeyDown (KeyCode.JoystickButton5)) {			
			r = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast (r, out rh)) {
				if (rh.collider != null) {
					if (quantities [currentId] >= 1) {
						var go = Instantiate (cubesTypes[currentId].gameObject);
						go.transform.position = rh.collider.transform.position + rh.normal;	
						quantities [currentId]--;
					}
								
				}			
			}

		}
	}

	IEnumerator DelayedDestruction(Collider col, int timeToDestroy, int id)
	{
		bool destroy = true;

		for (int i = 0; i < timeToDestroy; i++) 
		{
			r = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast (r, out rh)) {
				if (rh.collider != col) {
					destroy = false;
					break;
				}			
			} else {
				destroy = false;
				break;
			}
			yield return new WaitForSeconds (0.1f);	
		}

		if (destroy) {
			Destroy (col.gameObject);
			quantities [id]++;
		}
	}

}
