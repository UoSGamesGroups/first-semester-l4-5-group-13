using UnityEngine;
using System.Collections;

public class LevelChunkSpawnerScript : MonoBehaviour
{
	public GameObject[] levelChunks;
	public int levelSizeX = 5;
	public int levelSizeY = 5;

	void Start()
	{
		for(int y = 0; y < levelSizeY; y++)
		{
			for(int x = 0; x < levelSizeX; x++)
			{
				float offsetX = (float)x * 64 / 100;
				float offsetY = (float)y * 64 / 100;
				Vector2 offset = new Vector2(offsetX, offsetY);

				if(y == 0 && x == 0)
				{
					Instantiate(getLevelChunk(0), offset, Quaternion.identity);
				}
				else
				{
					Instantiate(getLevelChunk(), offset, Quaternion.identity);
				}
			}
		}
	}

	private GameObject getLevelChunk()
	{
		return levelChunks[Random.Range(0, levelChunks.Length)];
	}

	private GameObject getLevelChunk(int num)
	{
		return levelChunks[num];
	}
}