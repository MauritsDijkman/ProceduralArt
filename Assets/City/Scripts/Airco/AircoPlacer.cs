using UnityEngine;

public class AircoPlacer : MonoBehaviour
{
    [Header("Airco Object")]
    [SerializeField] private GameObject[] airco = null;

    // Shape parameters
    private Vector2 position = new Vector2();
    private int width = 0;
    private int depth = 0;


    public void Initialize(Vector2 pPosition, int pWidth, int pDepth)
    {
        position = pPosition;
        width = pWidth;
        depth = pDepth;
    }

    public void Generate()
    {
        Execute();
    }

    private void Execute()
    {
        int randomNumber = Random.Range(0, airco.Length);

        float offset = 1f;

        float randomX = RandomFloat(0 + offset, width - offset);
        float randomY = RandomFloat(0, 0);
        float randomZ = RandomFloat(0 + offset, depth - offset);

        Debug.Log($"randomX: {0 + offset} and {width - offset} || randomX: {randomX}|| randomZ: {0 + offset} and {depth - offset} || randomZ: {randomZ}");

        Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);

        GameObject placedAirco = Instantiate(airco[randomNumber], new Vector3(0, 0, 0), Quaternion.identity);
        placedAirco.transform.parent = transform;
        placedAirco.transform.localPosition = spawnPosition;
    }

    private float RandomFloat(float pMin, float pMax)
    {
        System.Random rand = new System.Random();
        float range = pMax - pMin;
        double sample = rand.NextDouble();
        double scaled = (sample * range) + pMin;
        float f = (float)scaled;

        return f;
    }
}
