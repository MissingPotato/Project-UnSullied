using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour 
{
	#region Variables
	
	public string resourceName = "Unnamed Resource";

	public Item resourceType;

	public int resourceAmount = 0;

	#endregion

	public int Gather(int _amount)
	{
		
		if ( resourceAmount >= _amount )
		{
			resourceAmount -= _amount; // Update the amount
			CheckForDeath();
			return _amount; // Return the amount that has been taken away from the resource node
		}
		else if ( resourceAmount < _amount ) // If we are trying to take away more than we have, then we give the diference then kill the resource;
		{
			int returnResource = _amount - resourceAmount;
			resourceAmount = 0;
			CheckForDeath();
			return returnResource;
		}

		return 0;
	}

	public void CheckForDeath()
	{

	}

}
