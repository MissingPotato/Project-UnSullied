using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour 
{
	#region Variables

	private List<Transform> spawnPoints = new List<Transform>();

	public List<GameObject> worldStatic;

	public List<GameObject> worldResources;

	private int randomIndex;
	private int randomChance;

	#endregion

	void Awake () 
	{

		#region GetSpawnPoints

		for (int i = 0; i < transform.childCount; i++)
		{

			if ( transform.GetChild(i).GetComponent<SpawnPoint>() )
				spawnPoints.Add(transform.GetChild(i));
		}

		#endregion

		GenerateChunk();

	}
	
	/// <summary>
	/// This function is going to generate the chunk with the designated models.
	/// </summary>
	public void GenerateChunk()
	{
		if ( worldStatic.Count == 0 )
			return;

		if ( spawnPoints.Count == 0 )
			return;

		for (int i = 0; i < spawnPoints.Count; i++)
		{
			
			randomChance = Random.Range(0, 100); // Randomize the random Chance

			if ( randomChance < 25 ) // Spawn World Resources
			{
				randomIndex = Random.Range(0, worldResources.Count); // Randomize the random index
				GameObject worldSpawn = Instantiate(worldResources[randomIndex], spawnPoints[i]);

				worldSpawn.transform.rotation = Quaternion.Euler(new Vector3(worldSpawn.transform.rotation.x, worldSpawn.transform.rotation.y + Random.Range(0f, 180f), worldSpawn.transform.rotation.z));
				worldSpawn.transform.position += new Vector3 (Random.Range(0f, .5f), 0, Random.Range(0f, .5f) );

			}
			
			else if ( randomChance >= 25 && randomChance < 95 ) // Spawn World Decoration Assets

			{	
				randomIndex = Random.Range(0, worldStatic.Count); // Randomize the random index
				GameObject worldSpawn = Instantiate(worldStatic[randomIndex], spawnPoints[i]);

				worldSpawn.transform.rotation = Quaternion.Euler(new Vector3(worldSpawn.transform.rotation.x, worldSpawn.transform.rotation.y + Random.Range(0f, 180f), worldSpawn.transform.rotation.z));
				worldSpawn.transform.position += new Vector3 (Random.Range(0f, 1.5f), 0, Random.Range(0f, 1.5f) );


			}

		}

	}
}
