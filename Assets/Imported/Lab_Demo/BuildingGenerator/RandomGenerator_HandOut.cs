using UnityEngine;

public class RandomGenerator_HandOut : MonoBehaviour
{
    public int seed;

    static System.Random rand = null;

    /// <summary>
    /// Returns a random integer between 0 and maxValue-1 (inclusive).
    /// </summary>
    public int Next(int maxValue)
    {
        return Rand.Next(maxValue);
    }

    public System.Random Rand
    {
        get
        {
            if (rand == null)
                ResetRandom();

            return rand;
        }
    }

    public void ResetRandom()
    {
        if (seed == 0)
            seed = Random.Range(0, 10000000);
        if (seed < 0)
            seed *= -1;

        rand = new System.Random(seed);
    }
}
