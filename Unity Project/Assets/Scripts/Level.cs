using UnityEngine;
using System.Collections.Generic;

public class Level
{
    // 2D array to store map
    public LevelChunk[,] levelMap = new LevelChunk[20,20];

    // Static helper lists to assist map calculation
    public static List<LevelChunk> levelChunkList = new List<LevelChunk>();
    public static List<LevelChunk> artifactPossChunks = new List<LevelChunk>();
    private static List<int> possChunks = new List<int>();

    // weather 0 for day, 1 for dayRain, 2 for night, 2 for nightRain
    public int environment;

    public int artifact;

    private int numOfChunks;
    
    private int chunkCounter = 0;
    private int currentPosX;
    private int currentPosY;

    private LevelChunk prevChunk = null;
    private LevelChunk currChunk = null;

    public Level(int _numOfChunks, int _environ, int _artifact)
    {
        levelChunkList.Clear();
        possChunks.Clear();
        artifactPossChunks.Clear();

        environment = _environ;
        artifact = _artifact;

        numOfChunks = _numOfChunks;
        CalculateMapLayout();
    }

    public Level(int _numOfChunks, int _artifact)
    {
        levelChunkList.Clear();
        possChunks.Clear();
        artifactPossChunks.Clear();

        environment = Random.Range(0, 4);
        artifact = _artifact;

        numOfChunks = _numOfChunks;
        CalculateMapLayout();
    }

    private void CalculateMapLayout()
    {
        currentPosX = 0;
        currentPosY = 10;

        while(chunkCounter < numOfChunks)
        {
            if(chunkCounter == 0)
            {
                GenerateAndFillChunk(currentPosX, currentPosY);
            }
            else
            {
                if(HasFoundNextChunkSpace())
                {
                    GenerateAndFillChunk(currentPosX, currentPosY);
                }
                else
                {
                    ResetToRandomChunk();
                }
            }
        }

        // randomly replace chunks with tall chunks
        AddTallChunks();

        // choose artifact chunk
        ChooseArtifactChunk();
    }

    private void GenerateAndFillChunk(int posX, int posY)
    {
        LevelChunk newChunk = new LevelChunk(posX, posY);

        currChunk = newChunk;

        if(prevChunk != null)
        {
            SetChunkLayoutType();
        }
        else
        {
            currChunk.layoutType += 1000;
        }

        prevChunk = newChunk;

        levelMap[posX, posY] = newChunk;
        levelChunkList.Add(newChunk);

        // add 1 to keep count of how many chunks have been generated
        chunkCounter++;
    }

    private bool HasFoundNextChunkSpace()
    {
        LookForSpaceAroundChunk();

        if(possChunks.Count > 0)
        {
            int nextChunk = possChunks[Random.Range(0, possChunks.Count)];

            switch(nextChunk)
            {
                case 1:
                    currentPosX--;
                    break;
                case 2:
                    currentPosY++;
                    break;
                case 3:
                    currentPosX++;
                    break;
                case 4:
                    currentPosY--;
                    break;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    private void LookForSpaceAroundChunk()
    {
        // clear possChunk array
        possChunks.Clear();
        // is there space West?
        if((currentPosX - 1 >= 0) && (levelMap[currentPosX - 1, currentPosY] == null))
        {
            possChunks.Add(1);
        }
        // is there space North?
        if((currentPosY + 1 < 20) && (levelMap[currentPosX, currentPosY + 1] == null))
        {
            possChunks.Add(2);
        }
        // is there space East?
        if((currentPosX + 1 < 20) && (levelMap[currentPosX + 1, currentPosY] == null))
        {
            possChunks.Add(3);
        }
        // is there space South?
        if((currentPosY - 1 >= 0) && (levelMap[currentPosX, currentPosY - 1] == null))
        {
            possChunks.Add(4);
        }
    }

    // if there are no routes found for next chunk (e.g. stuck in a corner of grid, or in a spiral)
    // this function chooses a random chunk with space around that has already been generated and starts from there
    private void ResetToRandomChunk()
    {
        int randomChunk = Random.Range(0, levelChunkList.Count);

        prevChunk = levelChunkList[randomChunk];

        currentPosX = levelChunkList[randomChunk].xCoord;
        currentPosY = levelChunkList[randomChunk].yCoord;
    }

    private void SetChunkLayoutType()
    {
        int layoutNum = 0;

        if(currChunk.xCoord == prevChunk.xCoord)
        {
            layoutNum = currChunk.yCoord - prevChunk.yCoord;
        }
        else
        {
            layoutNum = (currChunk.xCoord - prevChunk.xCoord) * 10;
        }

        switch(layoutNum)
        {
            case -1:
                currChunk.layoutType += 100;
                prevChunk.layoutType += 1;
                break;
            case 1:
                currChunk.layoutType += 1;
                prevChunk.layoutType += 100;
                break;
            case -10:
                currChunk.layoutType += 10;
                prevChunk.layoutType += 1000;
                break;
            case 10:
                currChunk.layoutType += 1000;
                prevChunk.layoutType += 10;
                break;
        }
    }

    private void AddTallChunks()
    {
        foreach(LevelChunk chunk in levelChunkList)
        {
            if(chunk.layoutType == 1010 && (chunk.yCoord + 2 < 20))
            {
                if((levelMap[chunk.xCoord, chunk.yCoord + 1] == null) && (levelMap[chunk.xCoord, chunk.yCoord + 2] == null))
                {
                    if(Random.Range(1, 3) == 1)
                    {
                        chunk.layoutType = 2;
                    }
                }
            }
        }
    }

    private void ChooseArtifactChunk()
    {
        foreach(LevelChunk chunk in levelChunkList)
        {
            if(chunk.layoutType == 1000 || chunk.layoutType == 100 || chunk.layoutType == 10 || chunk.layoutType == 1)
            {
                artifactPossChunks.Add(chunk);
            }
        }

        LevelChunk artChunk = artifactPossChunks[Random.Range(0, artifactPossChunks.Count)];

        artChunk.hasArtifact = true;
    }
}
