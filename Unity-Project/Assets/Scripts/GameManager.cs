using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	// -------------------------------------------- //
	// arrays to store all level chunks
	// name corresponds with entrance/exit layout
	// unity doesn't display multidimensional arrays
	//	in the inspector without explicitly declaring
	//	so each level chunk type will have its own array
	public GameObject[] oneW = new GameObject[1];
	public GameObject[] oneN = new GameObject[1];
	public GameObject[] oneE = new GameObject[1];
	public GameObject[] oneS = new GameObject[1];
	public GameObject[] twoWN = new GameObject[1];
	public GameObject[] twoWE = new GameObject[1];
	public GameObject[] twoWS = new GameObject[1];
	public GameObject[] twoNE = new GameObject[1];
	public GameObject[] twoNS = new GameObject[1];
	public GameObject[] twoES = new GameObject[1];
	public GameObject[] threeWNE = new GameObject[1];
	public GameObject[] threeWNS = new GameObject[1];
	public GameObject[] threeWES = new GameObject[1];
	public GameObject[] threeNES = new GameObject[1];
	public GameObject[] four = new GameObject[1];
	// -------------------------------------------- //
    public GameObject[] antiques = new GameObject[1];
	
	void Start()
	{
		Map tempMap = new Map(10, 100);
        for(int i = 0; i < Map.chunkNum; i++)
        {
            Map.CalcChunkOffset(tempMap.coordList[i]);
            DrawChunk(tempMap.coordList[i].x,
                      tempMap.coordList[i].y,
                      DetermineChunkType(tempMap.coordList[i]));
        }
        Debug.Log("furthest chunk: " + Map.furthestChunk.x + ", " + Map.furthestChunk.y);
        Instantiate(antiques[0], CalcChunkPos(Map.furthestChunk.x, Map.furthestChunk.y), Quaternion.identity);
	}
	
	private int DetermineChunkType(Map.Tuple tup)
    {
        int tempChunkType = 0;
		
		if((tup.x - 1 >= 0) && Map.mapCalc[tup.x - 1, tup.y] == true)
            tempChunkType += 1000;
		if((tup.y + 1 < Map.gridSize) && Map.mapCalc[tup.x, tup.y + 1] == true)
            tempChunkType += 100;
		if((tup.x + 1 < Map.gridSize) && Map.mapCalc[tup.x + 1, tup.y] == true)
            tempChunkType += 10;
		if((tup.y - 1 >= 0) && Map.mapCalc[tup.x, tup.y - 1] == true)
            tempChunkType += 1;
		
		return tempChunkType;
    }
	
    public void DrawChunk(int coordX, int coordY, int chunkType)
	{
		switch (chunkType)
        {
            case 1000:
                Instantiate(oneW[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
                break;
			case 100:
                Instantiate(oneN[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			case 10:
                Instantiate(oneE[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			case 1:
                Instantiate(oneS[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			case 1100:
                Instantiate(twoWN[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			case 1010:
                Instantiate(twoWE[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			case 1001:
                Instantiate(twoWS[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			case 110:
                Instantiate(twoNE[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			case 101:
                Instantiate(twoNS[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			case 11:
                Instantiate(twoES[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			case 1110:
                Instantiate(threeWNE[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			case 1101:
                Instantiate(threeWNS[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			case 1011:
                Instantiate(threeWES[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			case 111:
                Instantiate(threeNES[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			case 1111:
                Instantiate(four[0], CalcChunkPos(coordX, coordY), Quaternion.identity);
				break;
			default:
				break;
		}
	}
	
	// returns pixel position to place chunk
	private Vector2 CalcChunkPos(int x, int y)
	{
		return new Vector2 ((float)x * Map.chunkResSize, (float)y * Map.chunkResSize);
	}
}