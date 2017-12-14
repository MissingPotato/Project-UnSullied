using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour 
{
	#region Variables
	
	public int worldSize;
	public GameObject chunkReference;
	private Vector3 ogPoz;

	#endregion

	void Awake () 
	{
		ogPoz = transform.position;
		
		transform.position = new Vector3 ( ogPoz.x - worldSize, ogPoz.y , ogPoz.z - worldSize );

		for (int i = 0; i < worldSize; i++)
		{
			
			for (int j = 0; j < worldSize; j++)
			{
				transform.position = new Vector3 ( transform.position.x + 10, transform.position.y, transform.position.z ); // Move 1 chunk on the X
				Instantiate ( chunkReference, transform.position, transform.rotation );

			}

			transform.position = new Vector3 ( ogPoz.x - worldSize, transform.position.y, transform.position.z + 10 ); // Move 1 chunk on the Z

		}

	}
	
}
