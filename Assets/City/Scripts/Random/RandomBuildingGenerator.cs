using UnityEngine;

[System.Serializable]
public class ObjectInformationHolder
{
    public GameObject houseObject = null;
    [Range(0f, 100f)] public int changeOfSpawning = 100;
    [HideInInspector] public int _weight;
}

public class RandomBuildingGenerator : MonoBehaviour
{
    private int accumulatedWeights;
    private System.Random rand = new System.Random();

    private bool valuesSet = false;

    [Header("Houses")]
    [SerializeField] private ObjectInformationHolder[] houseTypes = null;


    private void Awake()
    {
        valuesSet = false;

        if (!valuesSet)
        {
            rand = FindObjectOfType<RandomGenerator>().GetRandom();

            if (rand == null)
                rand = new System.Random();

            CalculateWeights();

            valuesSet = true;
        }
    }

    public void SetValueSetState(bool pState)
    {
        valuesSet = pState;
    }

    public GameObject GetRandomHouse()
    {
        GameObject randomHouse = ReturnRandomHouse();
        return randomHouse;
    }

    private bool CheckForBug(GameObject pRandomHouse)
    {
        foreach (ObjectInformationHolder informationHolder in houseTypes)
        {
            if (informationHolder.houseObject.name == pRandomHouse.name)
            {
                if (informationHolder.changeOfSpawning >= 0)
                    return true;

                return false;
            }
        }

        return false;
    }

    private GameObject ReturnRandomHouse()
    {
        if (!valuesSet)
        {
            rand = FindObjectOfType<RandomGenerator>().GetRandom();
            CalculateWeights();
            valuesSet = true;
        }

        GameObject randomHouse = houseTypes[GetRandomObjectIndex()].houseObject;

        //bool sameHouse = true;
        //while (sameHouse)
        //    sameHouse = CheckForBug(randomHouse);

        return randomHouse;
    }

    public void CalculateWeights()
    {
        accumulatedWeights = 0;

        foreach (ObjectInformationHolder _object in houseTypes)
        {
            accumulatedWeights += _object.changeOfSpawning;
            _object._weight = accumulatedWeights;
        }
    }

    private int GetRandomObjectIndex()
    {
        int r = (int)(rand.NextDouble() * accumulatedWeights);

        for (int i = 0; i < houseTypes.Length; i++)
            if (houseTypes[i]._weight >= r)
                return i;

        return 0;
    }
}
