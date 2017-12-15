using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour 
{

	//  --------Variables-------------------------------------------------------------------------------

	#region Variables

	

	#endregion

	//  --------Active Functions-------------------------------------------------------------------------------

	void OnTriggerStay(Collider other)
	{
		
		if ( other.GetComponent <ResourceScript> () && Input.GetMouseButtonDown (0) )
		{
			Debug.Log ( other.GetComponent<ResourceScript>().resourceName ) ;
		}

	}

	//  --------Sleeper Functions-------------------------------------------------------------------------------



}
