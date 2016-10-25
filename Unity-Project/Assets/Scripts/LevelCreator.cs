using UnityEngine;
using System.Collections;

public class LevelCreator : MonoBehaviour
{
	// limits potential map size 
	public static int gridSize = 100;
	// stores the different level chunks
	public GameObject[] levelChunkArr;
	// array to hold bool values to determine if a space is filled or not
	private static bool[,] currentMap = new bool[gridSize, gridSize];
	// array to find next chunk position
	private static bool[] possChunkMap = new bool[4];
	// number of level chunks that will spawn
	private int levelChunkCount;
	private int pieceCounter;
	// the length of side of chunk in pixels
	private static float chunkDims = 0.64f;
	// currentMap grid x & y values
	private static int currPosX;
	private static int currPosY;
	// checks whether level has finished loading
	// private bool levelLoaded = false;

	void Start()
	{
		// chooses random starting position in first column of level grid
		currPosY = 0;
		currPosX = 0;

		levelChunkCount = 100;

		// load chunks
		for(pieceCounter = 0; pieceCounter < levelChunkCount; pieceCounter++)
		{
			if(pieceCounter == 0)
			{
				createChunk(3, currPosX, currPosY);
				moveToNextChunk();
			}
			else
			{
				createChunk(currPosX, currPosY);
				moveToNextChunk();
			}
		}
	}
	// returns a random level chunk prefab
	GameObject getLevelChunk()
	{
		return levelChunkArr[Random.Range(0, levelChunkArr.Length)];
	}

	// returns a specific level chunk prefab
	GameObject getLevelChunk(int num)
	{
		return levelChunkArr[num];
	}

	// calculates the pixel position for the chunk
	Vector2 calcChunkPos(int x, int y)
	{
		return new Vector2((float)x * chunkDims, (float)y * chunkDims);
	}

	// creates a chunk with a random level chunk prefab
	void createChunk(int x, int y)
	{
		Instantiate(getLevelChunk(), calcChunkPos(x, y), Quaternion.identity);
		currentMap[x, y] = true;
	}

	// creates a chunk with a specific level chunk prefab
	void createChunk(int explicitChunk, int x, int y)
	{
		Instantiate(getLevelChunk(explicitChunk), calcChunkPos(x, y), Quaternion.identity);
		currentMap[x, y] = true;
	}

	void moveToNextChunk()
	{
		possChunkMap[0] = spaceAbove();
		possChunkMap[1] = spaceRight();
		possChunkMap[2] = spaceBelow();
		possChunkMap[3] = spaceLeft();

		bool validChunk = false;
		
		for(int i = 0; i < 4; i++)
		{
			if(possChunkMap[i] == true)
			{
				validChunk = true;
				break;
			}
		}
		
		if(validChunk == false)
		{
			Debug.Log("No possible routes found");
			pieceCounter = levelChunkCount;
			return;
		}
		else
		{
			int index = Random.Range(0, 4);
			
			while(possChunkMap[index] == false)
			{
				index = Random.Range(0, 4);
			}

			switch(index)
			{
				case 0:
					currPosY++;
					break;
				case 1:
					currPosX++;
					break;
				case 2:
					currPosY--;
					break;
				case 3:
					currPosX--;
					break;
			}
		}
	}

	bool spaceAbove()
	{
		if((currPosY + 1 < gridSize) && currentMap[currPosX, currPosY + 1] == false)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	bool spaceRight()
	{
		if((currPosX + 1 < gridSize) && currentMap[currPosX + 1, currPosY] == false)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	bool spaceBelow()
	{
		if((currPosY - 1 >= 0) && currentMap[currPosX, currPosY - 1] == false)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	bool spaceLeft()
	{
		if((currPosX - 1 >= 0) && currentMap[currPosX - 1, currPosY] == false)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}