using UnityEngine;
using Demo;

public class SkyScraper : Shape
{
    [Header("Size")]
    public int width = 0;
    public int depth = 0;
    private Vector2 position = new Vector2();
    private Vector3 localPosition = new Vector3();

    [Header("Chance")]
    [Range(0.0f, 0.9f)]
    public float stockContinueChance = 0.5f;
    [Range(0.0f, 0.9f)]
    public float roofContinueChance = 0.5f;

    [Header("Walls")]
    public GameObject[] wallStyle = null;
    public GameObject[] roofStyle = null;


    public void Initialize(Vector2 pPosition, int pWidth, int pDepth)
    {
        position = pPosition;
        width = pWidth;
        depth = pDepth;
    }

    public void RemoveExistingBuilding()
    {
        int childcount = transform.childCount;

        for (int i = childcount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (Application.isPlaying)
                Destroy(child);
            else if (Application.isEditor)
                DestroyImmediate(child);
        }
    }

    public void Generate()
    {
        RemoveExistingBuilding();
        Execute();
    }

    protected override void Execute()
    {
        // Create four walls
        for (int i = 0; i < 4; i++)
        {
            localPosition = new Vector3();
            switch (i)
            {
                case 0:
                    localPosition = new Vector3(-(width - 1) * 0.5f, 0, 0); // Left
                    break;
                case 1:
                    localPosition = new Vector3(0, 0, (depth - 1) * 0.5f);  // Back
                    break;
                case 2:
                    localPosition = new Vector3((width - 1) * 0.5f, 0, 0);  // Right
                    break;
                case 3:
                    localPosition = new Vector3(0, 0, -(depth - 1) * 0.5f); // Front
                    break;
            }

            //Debug.Log($"i: {i} || LocalPosition: {localPosition} || Width: {width} || Width / 2: {width / 2.0f} || Depth: {depth} || Depth / 2: {depth / 2.0f} || Adjusted position: {new Vector3(localPosition.x + width / 2, localPosition.y, localPosition.z + depth / 2)}");

            SimpleRow newRow = CreateSymbol<SimpleRow>("wall", new Vector3(localPosition.x + (width / 2.0f), localPosition.y, localPosition.z + (depth / 2.0f)), Quaternion.Euler(0, i * 90, 0));
            newRow.Initialize(-1, i % 2 == 1 ? width : depth, wallStyle);
            newRow.Generate();
        }

        // Continue with a stock or with a roof (random choice):
        float randomValue = RandomFloat();

        //Debug.Log($"randomValue: {randomValue} || stockContinueChance: {stockContinueChance}");

        if (randomValue < stockContinueChance)
        {
            SimpleStock nextStock = CreateSymbol<SimpleStock>("stock", new Vector3(width / 2.0f, 1, depth / 2.0f));
            nextStock.Initialize(width, depth, wallStyle, roofStyle, stockContinueChance, roofContinueChance, false);
            nextStock.Generate();
        }
        else
        {
            SimpleRoof nextRoof = CreateSymbol<SimpleRoof>("roof", new Vector3(width / 2.0f, 1, depth / 2.0f));
            nextRoof.Initialize(width, depth, roofStyle, wallStyle, stockContinueChance, roofContinueChance, false);
            nextRoof.Generate();
        }
    }
}
