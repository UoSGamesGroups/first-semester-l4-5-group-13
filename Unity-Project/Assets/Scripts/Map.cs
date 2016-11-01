using UnityEngine;
using System.Collections.Generic;

public class Map
{
	// -------------------------------------------- //
	// fixed grid size for map
	public const int gridSize = 40;
	// pixel size for level chunks
	public const float chunkResSize = 5.12f;
	// array to quickly generate basic layout with true and false vals
	public static bool[,] mapCalc = new bool[gridSize, gridSize];
	// array to find next chunk position
	public static  bool[] possChunkMap = new bool[4];
	// list used to store filled chunk coords
	public List<Tuple> coordList = new List<Tuple>();
	// current x & y positions in map calculation
	public static int currPosX;
	public static int currPosY;
	// checks whether level has finished loading
	public static bool levelLoaded = false;
	// number of level chunks to load into map
    public static int chunkNum = 0;
    public static int chunkCounter = 0;
    // holds chunk furthest away from start chunk
    public static Tuple furthestChunk = new Tuple(0, 0);
    // holds largest chunk offset
    public static int largestOffset = 0;
    // holds coords for starting chunk
    public static int startingYCoord;
	// holds artifact array number
	public int artifact;
	// stores the seed key from Random.InitState()
	private int mapSeed;
	// difficulty to base other numbers on
	public int difficulty;
	// class to hold x & y coordinates for chunk list
	public class Tuple
	{
		public Tuple(int a, int b)
		{
			x = a;
			y = b;
		}
		public int x;
		public int y;
	}
	// -------------------------------------------- //
	// -------------------------------------------- //


	// -------------------------------------------- //
	// Methods
	
	// FIRST PASS - Calculate map layout (no draw)
	// constructors for new maps
	public Map(int diff, int chunkAmount)
	{
		difficulty = diff;
		chunkNum = chunkAmount;

		mapSeed = System.DateTime.Now.Millisecond;
		Debug.Log ("mapseed: " + mapSeed);
		Random.InitState(mapSeed);
		CalculateMap();

		artifact = Random.Range (0, 18);
	}
	// overload to take seedkey for nonrandom maps
	public Map(int diff, int chunkAmount, int seed)
	{
		difficulty = diff;
		chunkNum = chunkAmount;
		mapSeed = seed;
		Random.InitState(seed);
		CalculateMap();

		artifact = Random.Range (0, 18);
	}
	// function to build the map
	private void CalculateMap()
	{
		currPosX = 0;
		// choose random starting chunk
		currPosY = Random.Range(0, gridSize);

        startingYCoord = currPosY;

		for(chunkCounter = chunkNum; chunkCounter > 0; chunkCounter--)
		{
			SetChunk(currPosX, currPosY);
			MoveToNextChunk();
		}
	}
	private void SetChunk(int x, int y)
	{
		mapCalc[x, y] = true;
		coordList.Add(new Tuple(x, y));
	}
	// finds next empty chunk to fill
	private void MoveToNextChunk()
	{
		possChunkMap[0] = SpaceAbove(currPosX, currPosY);
        possChunkMap[1] = SpaceRight(currPosX, currPosY);
        possChunkMap[2] = SpaceBelow(currPosX, currPosY);
        possChunkMap[3] = SpaceLeft(currPosX, currPosY);

		bool validChunk = false;

		for(int i = 0; i < 4; i++)
		{
			if(possChunkMap[i] == true)
			{
				validChunk = true;
				break;
			}
		}

		if (validChunk == false)
		{
            int resetChunk = Random.Range(0, chunkNum - chunkCounter - 1);
			currPosX = coordList[resetChunk].x;
			currPosY = coordList[resetChunk].y;
			MoveToNextChunk();
		}
		else
		{
			int index = Random.Range(0, 4);

			while (possChunkMap[index] == false)
			{
				index = Random.Range(0, 4);
			}

			switch (index)
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
	// functions to check if spaces are empty and available
    private bool SpaceAbove(int x, int y)
	{
		if((y + 1 < gridSize) && mapCalc[x, y + 1] == false)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
    private bool SpaceRight(int x, int y)
	{
		if((x + 1 < gridSize) && mapCalc[x + 1, y] == false)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
    private bool SpaceBelow(int x, int y)
	{
		if((y - 1 >= 0) && mapCalc[x, y - 1] == false)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
    private bool SpaceLeft(int x, int y)
	{
		if((x - 1 >= 0) && mapCalc[x - 1, y] == false)
		{
			return true;
		}
		else
		{
			return false;
		}
	}   
    // calculate chunk offset from starting chunk
    public static void CalcChunkOffset(Tuple tup)
    {
        int tempCurrCoordX = tup.x;
        int tempCurrCoordY = tup.y - startingYCoord;

        int tempOffset = tempCurrCoordX + tempCurrCoordY;

        if (tempOffset >= largestOffset)
        {
            largestOffset = tempOffset;
            furthestChunk.x = tup.x;
            furthestChunk.y = tup.y;
        }
    }
	// -------------------------------------------- //
}