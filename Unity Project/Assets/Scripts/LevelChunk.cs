using UnityEngine;
using System.Collections;

public class LevelChunk
{
    public int layoutType = 0;
    public bool hasArtifact = false;

    public bool hasBackWindow = false;
    public bool hasFrontWindow = false;

    public int xCoord;
    public int yCoord;

    public LevelChunk(int _xCoord, int _yCoord)
    {
        xCoord = _xCoord;
        yCoord = _yCoord;

        int i = Random.Range(0, 4);

        switch(i)
        {
            case 0:
                hasBackWindow = true;
                hasFrontWindow = true;
                break;
            case 1:
                hasBackWindow = true;
                hasFrontWindow = false;
                break;
            case 2:
                hasBackWindow = false;
                hasFrontWindow = true;
                break;
            case 3:
                hasBackWindow = false;
                hasFrontWindow = false;
                break;
        }
    }
}
