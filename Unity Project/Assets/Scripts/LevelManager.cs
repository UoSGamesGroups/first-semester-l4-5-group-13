using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // LEVEL CHUNK STORAGE ARRAYS
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
    public GameObject[] tall = new GameObject[1];

    public GameObject[] windows = new GameObject[4];

    public GameObject[] artifacts = new GameObject[8];

    public GameObject[] npcs = new GameObject[1];
    GameObject npcG;

    private const float spriteSize = 2.56f;

    public bool levelLoaded = false;
    public bool levelRequested = false;

    GameObject mapHolder;
    GameObject playerObject;
    Player player;

    void Start()
    {
        playerObject = GameObject.Find("TopHat");
        player = playerObject.GetComponent<Player>();

        mapHolder = new GameObject("Map Holder");

        npcG = Instantiate(npcs[0], new Vector2(npcs[0].transform.localPosition.x, npcs[0].transform.localPosition.y), Quaternion.identity) as GameObject;
        npcG.transform.parent = mapHolder.transform;

        StartCoroutine(MoveOverSeconds(npcG, new Vector2(-2.45f, 26.43f), 2f));

        Level tempLevel = new Level((int)((Player.difficulty + 1) * 1.5f), Random.Range(0, 7));

        foreach(LevelChunk chunk in Level.levelChunkList)
        {
            DrawChunk(chunk, tempLevel.environment, tempLevel.artifact);
        }

        GameObject gWindow;
        gWindow = Instantiate(windows[tempLevel.environment], new Vector2(-1.9f, 27.5f), Quaternion.identity) as GameObject;
        gWindow.transform.parent = mapHolder.transform;

        GameObject speechBubble;
        speechBubble = Instantiate(artifacts[7], new Vector2(artifacts[7].transform.localPosition.x, artifacts[7].transform.localPosition.y), Quaternion.identity) as GameObject;
        speechBubble.transform.parent = mapHolder.transform;

        SpriteRenderer speechRend = speechBubble.GetComponent<SpriteRenderer>();
        speechRend.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        GameObject speechBubbleArt;
        speechBubbleArt = Instantiate(artifacts[tempLevel.artifact], new Vector2(artifacts[tempLevel.artifact].transform.localPosition.x, artifacts[tempLevel.artifact].transform.localPosition.y), Quaternion.identity) as GameObject;
        speechBubbleArt.transform.parent = mapHolder.transform;

        SpriteRenderer speechArt = speechBubbleArt.GetComponent<SpriteRenderer>();
        speechArt.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        StartCoroutine(FadeTo(speechRend, 1.0f, 4f));
        StartCoroutine(FadeTo(speechArt, 1.0f, 4f));
    }

    //void update()
    //{
    //    if(input.getkey(keycode.m))
    //    {
    //        destroy(mapholder);

    //        levelloaded = false;
    //        levelrequested = false;
    //    }
    //    if(input.getkey(keycode.n) && levelloaded == false)
    //    {
    //        if(levelrequested == false)
    //        {
    //            mapholder = new gameobject("map holder");

    //            level newlevel = new level(random.range(3, 201), random.range(0, 7));

    //            levelrequested = true;

    //            foreach(levelchunk chunk in level.levelchunklist)
    //            {
    //                drawchunk(chunk, newlevel.environment, newlevel.artifact);
    //            }

    //            gameobject gwindow;
    //            gwindow = instantiate(windows[newlevel.environment], new vector2(-1.9f, 27.5f), quaternion.identity) as gameobject;
    //            gwindow.transform.parent = mapholder.transform;

    //            gameobject speechbubble;
    //            speechbubble = instantiate(artifacts[7], new vector2(artifacts[7].transform.localposition.x, artifacts[7].transform.localposition.y), quaternion.identity) as gameobject;
    //            speechbubble.transform.parent = mapholder.transform;
    //            gameobject speechbubbleart;

    //            speechbubbleart = instantiate(artifacts[newlevel.artifact], new vector2(artifacts[newlevel.artifact].transform.localposition.x, artifacts[newlevel.artifact].transform.localposition.y), quaternion.identity) as gameobject;
    //            speechbubbleart.transform.parent = mapholder.transform;

    //            levelloaded = true;
    //        }

    //    }

    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector2 end, float seconds)
    {
        float elapsedTime = 0;
        Vector2 startingPos = new Vector2(-3.7f, 26.43f);
        while(elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector2.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }

    public void DrawChunk(LevelChunk ch, int environ, int artifact)
    {
        switch(ch.layoutType)
        {
            case 1000:
                GameObject gOneW;
                gOneW = Instantiate(oneW[Random.Range(0, oneW.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gOneW.transform.parent = mapHolder.transform;

                if(ch.hasArtifact == true)
                {
                    GameObject artOneW;
                    artOneW = Instantiate(artifacts[artifact], CalcChunkPos(ch.xCoord, ch.yCoord) + new Vector2(0.66f, 1.53f), Quaternion.identity) as GameObject;
                    artOneW.transform.parent = mapHolder.transform;
                }

                if(ch.hasBackWindow)
                {
                    GameObject gWindow;
                    gWindow = Instantiate(windows[environ], CalcChunkPos(ch.xCoord, ch.yCoord) + new Vector2(-0.40f, 1.7f), Quaternion.identity) as GameObject;
                    gWindow.transform.parent = mapHolder.transform;
                }
                break;
            case 100:
                GameObject gOneN;
                gOneN = Instantiate(oneN[Random.Range(0, oneN.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gOneN.transform.parent = mapHolder.transform;

                if(ch.hasArtifact == true)
                {
                    GameObject artOneN;
                    artOneN = Instantiate(artifacts[artifact], CalcChunkPos(ch.xCoord, ch.yCoord) + new Vector2(0.63f, 1.06f), Quaternion.identity) as GameObject;
                    artOneN.transform.parent = mapHolder.transform;
                }
                break;
            case 10:
                GameObject gOneE;
                gOneE = Instantiate(oneE[Random.Range(0, oneE.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gOneE.transform.parent = mapHolder.transform;

                if(ch.hasArtifact == true)
                {
                    GameObject artOneE;
                    artOneE = Instantiate(artifacts[artifact], CalcChunkPos(ch.xCoord, ch.yCoord) + new Vector2(-0.92f, 1.1f), Quaternion.identity) as GameObject;
                    artOneE.transform.parent = mapHolder.transform;
                }
                break;
            case 1:
                GameObject gOneS;
                gOneS = Instantiate(oneS[Random.Range(0, oneS.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gOneS.transform.parent = mapHolder.transform;

                if(ch.hasArtifact == true)
                {
                    GameObject artOneS;
                    artOneS = Instantiate(artifacts[artifact], CalcChunkPos(ch.xCoord, ch.yCoord) + new Vector2(-0.98f, 1.6f), Quaternion.identity) as GameObject;
                    artOneS.transform.parent = mapHolder.transform;
                }
                break;
            case 1100:
                GameObject gTwoWN;
                gTwoWN = Instantiate(twoWN[Random.Range(0, twoWN.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gTwoWN.transform.parent = mapHolder.transform;
                break;
            case 1010:
                GameObject gTwoWE;
                gTwoWE = Instantiate(twoWE[Random.Range(0, twoWE.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gTwoWE.transform.parent = mapHolder.transform;
                break;
            case 1001:
                GameObject gTwoWS;
                gTwoWS = Instantiate(twoWS[Random.Range(0, twoWS.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gTwoWS.transform.parent = mapHolder.transform;
                break;
            case 110:
                GameObject gTwoNE;
                gTwoNE = Instantiate(twoNE[Random.Range(0, twoNE.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gTwoNE.transform.parent = mapHolder.transform;
                break;
            case 101:
                GameObject gTwoNS;
                gTwoNS = Instantiate(twoNS[Random.Range(0, twoNS.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gTwoNS.transform.parent = mapHolder.transform;
                break;
            case 11:
                GameObject gTwoES;
                gTwoES = Instantiate(twoES[Random.Range(0, twoES.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gTwoES.transform.parent = mapHolder.transform;
                break;
            case 1110:
                GameObject gThreeWNE;
                gThreeWNE = Instantiate(threeWNE[Random.Range(0, threeWNE.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gThreeWNE.transform.parent = mapHolder.transform;
                break;
            case 1101:
                GameObject gThreeWNS;
                gThreeWNS = Instantiate(threeWNS[Random.Range(0, threeWNS.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gThreeWNS.transform.parent = mapHolder.transform;
                break;
            case 1011:
                GameObject gThreeWES;
                gThreeWES = Instantiate(threeWES[Random.Range(0, threeWES.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gThreeWES.transform.parent = mapHolder.transform;
                break;
            case 111:
                GameObject gThreeNES;
                gThreeNES = Instantiate(threeNES[Random.Range(0, threeNES.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gThreeNES.transform.parent = mapHolder.transform;
                break;
            case 1111:
                GameObject gFour;
                gFour = Instantiate(four[Random.Range(0, four.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gFour.transform.parent = mapHolder.transform;
                break;
            case 2:
                GameObject gTall;
                gTall = Instantiate(tall[Random.Range(0, tall.Length)], CalcChunkPos(ch.xCoord, ch.yCoord), Quaternion.identity) as GameObject;
                gTall.transform.parent = mapHolder.transform;
                break;
            default:
                break;
        }
    }

    private Vector2 CalcChunkPos(int x, int y)
    {
        return new Vector2(x * spriteSize, y * spriteSize);
    }

    IEnumerator FadeTo(SpriteRenderer spr, float aValue, float aTime)
    {
        float alpha = spr.color.a;
        for(float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            spr.color = newColor;
            yield return null;
        }
    }
}
