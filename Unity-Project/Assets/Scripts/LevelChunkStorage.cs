using UnityEngine;
using System.Collections;

public class LevelChunkStorage
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
}