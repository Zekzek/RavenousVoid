using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Genesis : MonoBehaviour
{
    private const int NUM_RED_CUBE_PER_TILE = 1;
    private const int NUM_BLUE_CUBE_PER_TILE = 3;
    private const int NUM_GREEN_CUBE_PER_TILE = 8;
    private const int NUM_BASE_SHADOWS_PER_TILE = 4;
    private const int NUM_BASE_TREES_PER_TILE = 5;
    private const int NUM_BASE_ROCKS_PER_TILE = 5;
    private const int NUM_BASE_LOGS_PER_TILE = 5;
    private const int INSTANTIATE_DELAY = 100;
    private const int SQR_SAFE_RADIUS = 100;

    public GameObject floorTemplate;
    public GameObject redCubeTemplate;
    public GameObject blueCubeTemplate;
    public GameObject greenCubeTemplate;
    public GameObject shadowTemplate;
    public GameObject[] treeTemplates;
    public GameObject[] rockTemplates;
    public GameObject[] logTemplates;

    private static Genesis instance;
    private static Dictionary<string, GameObject> loadedTiles = new Dictionary<string, GameObject>();

    void Start()
    {
        instance = this;
        CheckAndGenerateNear(0, 0);
    }

    public static async Task CheckAndGenerateNear(int x, int z)
    {
        await instance.CheckAndGenerateAt(x, z);
        await instance.CheckAndGenerateAt(x + 1, z + 1);
        await instance.CheckAndGenerateAt(x + 1, z);
        await instance.CheckAndGenerateAt(x + 1, z - 1);
        await instance.CheckAndGenerateAt(x, z + 1);
        await instance.CheckAndGenerateAt(x, z - 1);
        await instance.CheckAndGenerateAt(x - 1, z + 1);
        await instance.CheckAndGenerateAt(x - 1, z);
        await instance.CheckAndGenerateAt(x - 1, z - 1);

        instance.EnableOnlyNear(x, z);
    }

    public static void Clear()
    {
        loadedTiles = new Dictionary<string, GameObject>();
    }

    private void EnableOnlyNear(int x, int z)
    {
        string[] nearbyIds = {
            (x+1) + "," + (z+1), (x+1) + "," + (z), (x+1) + "," + (z-1),
            (x) + "," + (z+1), (x) + "," + (z), (x) + "," + (z-1),
            (x-1) + "," + (z+1), (x-1) + "," + (z), (x-1) + "," + (z-1)
        };

        foreach (string id in loadedTiles.Keys)
        {
            loadedTiles[id].SetActive(System.Array.IndexOf(nearbyIds, id) > -1);
        }
    }

    private async Task CheckAndGenerateAt(int x, int z)
    {
        if (!loadedTiles.ContainsKey(x + "," + z))
        {
            await instance.GenerateAt(x, z);
        }
    }

    private async Task GenerateAt(int x, int z)
    {
        GameObject floor = Instantiate(floorTemplate, transform);
        loadedTiles.Add(x + "," + z, floor);

        floor.name = "Tile" + x + "_" + z;
        PlayerSensor playerSensor = floor.GetComponentInChildren<PlayerSensor>();
        playerSensor.xPos = x;
        playerSensor.zPos = z;

        float floorWidth = floor.transform.localScale.x;
        float floorDepth = floor.transform.localScale.z;

        floor.transform.position = new Vector3(x * floorWidth, transform.position.y, z * floorDepth);

        await GenerateCubes(floor, x, z, floorWidth, floorDepth);
        await GenerateShadows(floor, x, z, floorWidth, floorDepth, Mathf.Abs(x) + Mathf.Abs(z));
        await GenerateTrees(floor, x, z, floorWidth, floorDepth, x);
        await GenerateRocks(floor, x, z, floorWidth, floorDepth, -(x + z));
        await GenerateLogs(floor, x, z, floorWidth, floorDepth, z);
    }

    private async Task GenerateCubes(GameObject floor, int x, int z, float floorWidth, float floorDepth)
    {
        GameObject cubeWrapper = new GameObject("Cubes");
        cubeWrapper.transform.SetParent(floor.transform);

        for (int i = 0; i < NUM_RED_CUBE_PER_TILE; i++)
        {
            Instantiate(redCubeTemplate, GenerateRandomPosition(x, z, floorWidth, floorDepth, 1.5f), Quaternion.identity, cubeWrapper.transform);
            await Task.Delay(INSTANTIATE_DELAY);
        }
        for (int i = 0; i < NUM_BLUE_CUBE_PER_TILE; i++)
        {
            Instantiate(blueCubeTemplate, GenerateRandomPosition(x, z, floorWidth, floorDepth, 1.5f), Quaternion.identity, cubeWrapper.transform);
            await Task.Delay(INSTANTIATE_DELAY);
        }
        for (int i = 0; i < NUM_GREEN_CUBE_PER_TILE; i++)
        {
            Instantiate(greenCubeTemplate, GenerateRandomPosition(x, z, floorWidth, floorDepth, 1.5f), Quaternion.identity, cubeWrapper.transform);
            await Task.Delay(INSTANTIATE_DELAY);
        }
    }

    private async Task GenerateShadows(GameObject floor, int x, int z, float floorWidth, float floorDepth, int bonus)
    {
        GameObject shadowWrapper = new GameObject("Shadows");
        shadowWrapper.transform.SetParent(floor.transform);

        for (int i = 0; i < NUM_BASE_SHADOWS_PER_TILE + bonus; i++)
        {
            Instantiate(shadowTemplate, GenerateRandomPosition(x, z, floorWidth, floorDepth, 2.5f), Quaternion.identity, shadowWrapper.transform);
            await Task.Delay(INSTANTIATE_DELAY);
        }
    }

    private async Task GenerateTrees(GameObject floor, int x, int z, float floorWidth, float floorDepth, int bonus)
    {
        GameObject treeWrapper = new GameObject("Trees");
        treeWrapper.transform.SetParent(floor.transform);

        for (int i = 0; i < NUM_BASE_TREES_PER_TILE + bonus; i++)
        {
            Instantiate(treeTemplates[Random.Range(0, treeTemplates.Length)], GenerateRandomPosition(x, z, floorWidth, floorDepth, 1), Quaternion.identity, treeWrapper.transform);
            await Task.Delay(INSTANTIATE_DELAY);
        }
    }

    private async Task GenerateRocks(GameObject floor, int x, int z, float floorWidth, float floorDepth, int bonus)
    {
        GameObject rockWrapper = new GameObject("Rocks");
        rockWrapper.transform.SetParent(floor.transform);

        for (int i = 0; i < NUM_BASE_ROCKS_PER_TILE + bonus; i++)
        {
            Instantiate(rockTemplates[Random.Range(0, rockTemplates.Length)], GenerateRandomPosition(x, z, floorWidth, floorDepth, 1), Quaternion.identity, rockWrapper.transform);
            await Task.Delay(INSTANTIATE_DELAY);
        }
    }

    private async Task GenerateLogs(GameObject floor, int x, int z, float floorWidth, float floorDepth, int bonus)
    {
        GameObject logWrapper = new GameObject("Logs");
        logWrapper.transform.SetParent(floor.transform);

        for (int i = 0; i < NUM_BASE_LOGS_PER_TILE + bonus; i++)
        {
            Instantiate(logTemplates[Random.Range(0, logTemplates.Length)], GenerateRandomPosition(x, z, floorWidth, floorDepth, 0.5f), Quaternion.identity, logWrapper.transform);
            await Task.Delay(INSTANTIATE_DELAY);
        }
    }

    private Vector3 GenerateRandomPosition(int x, int z, float floorWidth, float floorHeight, float height)
    {
        Vector3 pos = Vector3.zero;

        while (pos.sqrMagnitude < SQR_SAFE_RADIUS)
            pos = new Vector3(
                x * floorWidth + floorWidth / 2 - Random.Range(0, floorWidth),
                transform.position.y + height,
                z * floorHeight + floorHeight / 2 - Random.Range(0, floorHeight));

        return pos;
    }
}
