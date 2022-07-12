using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    [Header("Seed")]
    [SerializeField] private int seed = 0;

    private int copySeed = 0;

    private void Start()
    {
        SetNewRandomSeed();
    }

    public void SetNewRandomSeed()
    {
        copySeed = seed;

        if (copySeed <= 0)
            copySeed = Random.Range(0, 1000000001);
    }

    public System.Random GetRandom()
    {
        SetNewRandomSeed();
        return new System.Random(copySeed);
    }
}
