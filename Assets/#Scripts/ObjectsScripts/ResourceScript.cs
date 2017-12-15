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
			CheckForDeath(); // Check if the resource is depleated
			return returnResource; // Return the amount that was salvaged
		}

		return 0;
	}


    /// <summary>
    /// This function is going to destroy the resource node if it is depleated, run this at the end of any kind of function.
    /// </summary>
	public void CheckForDeath()
	{

		if (resourceAmount <= 0)
		{

            // Play destroy animation

            Destroy(gameObject, .5f);
		}

	}

}
