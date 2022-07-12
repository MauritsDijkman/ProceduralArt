using Demo;
using UnityEngine;

public class SquareBuilding : Shape
{
    [Header("Height")]
    public int staticHeight = 5;
    public bool randomHeight = true;
    public int minHeight = 3;
    public int maxHeight = 8;
    private int chosenHeight = 0;

    // Wall position
    private Vector3 localPosition = new Vector3(0, 0, 0);

    [Header("Size")]
    public int width = 0;
    public int depth = 0;
    private Vector2 position = new Vector2();

    // Shape objects
    [SerializeField] private GameObject[] wallStyle;
    [SerializeField] private GameObject[] roofStyle;


    public void Initialize(Vector2 pPosition, int pWidth, int pDepth, GameObject[] pWallStyle = null, GameObject[] pRoofStyle = null)
    {
        position = pPosition;
        width = pWidth;
        depth = pDepth;

        if (pWallStyle != null)
            wallStyle = pWallStyle;

        if (pRoofStyle != null)
            roofStyle = pRoofStyle;
    }

    public void Generate()
    {
        Execute();
    }

    protected override void Execute()
    {
        RemoveExistingBuilding();

        if (!randomHeight)
            chosenHeight = staticHeight;
        else
        {
            chosenHeight = Random.Range(minHeight, maxHeight + 1);
            staticHeight = chosenHeight;
        }

        Debug.Log($"ChosenHeight: {chosenHeight}");

        localPosition = new Vector3(0, 0, 0);

        for (int i = 0; i < chosenHeight; i++)
            BuildRow(i);

        BuildRoof();
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

    private void BuildRow(int pRowNumber)
    {
        //Create four walls:
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    localPosition = new Vector3(-(width - 1) * 0.5f, localPosition.y, 0); // left
                    break;
                case 1:
                    localPosition = new Vector3(0, localPosition.y, (depth - 1) * 0.5f); // back
                    break;
                case 2:
                    localPosition = new Vector3((width - 1) * 0.5f, localPosition.y, 0); // right
                    break;
                case 3:
                    localPosition = new Vector3(0, localPosition.y, -(depth - 1) * 0.5f); // front
                    break;
            }
            SimpleRow newRow = CreateSymbol<SimpleRow>("wall", new Vector3(localPosition.x + (width / 2.0f), localPosition.y, localPosition.z + (depth / 2.0f)), Quaternion.Euler(0, i * 90, 0));
            newRow.Initialize(pRowNumber, i % 2 == 1 ? width : depth, wallStyle);
            newRow.Generate();
        }

        localPosition = new Vector3(0, localPosition.y + 1, 0);
    }

    private void BuildRoof()
    {
        SimpleRoof nextRoof = CreateSymbol<SimpleRoof>("roof", new Vector3(width / 2.0f, chosenHeight, depth / 2.0f));
        nextRoof.Initialize(width, depth, roofStyle, wallStyle);
        nextRoof.Generate();
    }
}
