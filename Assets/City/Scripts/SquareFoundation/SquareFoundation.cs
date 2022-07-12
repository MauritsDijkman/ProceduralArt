using System.Collections.Generic;
using UnityEngine;

public class SquareFoundation : MonoBehaviour
{
    [Header("Plane Object")]
    [SerializeField] private GameObject tree = null;
    [SerializeField] private GameObject lamppostHorizontal = null;
    [SerializeField] private GameObject lamppostVertical = null;

    [Header("Placement of trees and lampposts")]
    [SerializeField] private bool placeLampposts = false;
    [SerializeField] private bool placeTrees = false;
    [SerializeField] private bool chooseRandom = true;

    [Header("Lamppost")]
    [Range(0f, 20f)]
    [SerializeField] private int amountLamppostsTop = 10;
    [Range(0f, 20f)]
    [SerializeField] private int amountLamppostsBottom = 10;
    [Range(0f, 20f)]
    [SerializeField] private int amountLamppostsLeft = 10;
    [Range(0f, 20f)]
    [SerializeField] private int amountLamppostsRight = 10;

    [Header("Trees")]
    [SerializeField] private int minWidthForTrees = 15;
    [SerializeField] private int minHeightForTrees = 15;
    [Space(20)]
    [Range(0f, 20f)]
    [SerializeField] private int amountTreesTop = 10;
    [Range(0f, 20f)]
    [SerializeField] private int amountTreesBottom = 10;
    [Range(0f, 20f)]
    [SerializeField] private int amountTreesLeft = 10;
    [Range(0f, 20f)]
    [SerializeField] private int amountTreesRight = 10;

    // Current values
    private int width;
    private int height;
    private Vector2 position;

    //Lists
    private List<GameObject> spawnedTreesTop = new List<GameObject>();
    private List<GameObject> spawnedTreesBottom = new List<GameObject>();
    private List<GameObject> spawnedTreesLeft = new List<GameObject>();
    private List<GameObject> spawnedTreesRight = new List<GameObject>();

    private List<GameObject> spawnedLamppostsTop = new List<GameObject>();
    private List<GameObject> spawnedLamppostsBottom = new List<GameObject>();
    private List<GameObject> spawnedLamppostsLeft = new List<GameObject>();
    private List<GameObject> spawnedLamppostsRight = new List<GameObject>();


    public void Initialize(int pWidth, int pHeight, Vector2 pPosition)
    {
        width = pWidth;
        height = pHeight;
        position = pPosition;
    }

    public void Generate()
    {
        RemoveTrees();
        RemoveLampposts();

        if (chooseRandom)
        {
            System.Random random = FindObjectOfType<RandomGenerator>().GetRandom();

            int pRandomNumber = random.Next(0, 2);

            if (pRandomNumber == 0)
            {
                placeTrees = true;
                placeLampposts = false;
            }
            else if (pRandomNumber == 1)
            {
                placeTrees = false;
                placeLampposts = true;
            }
        }

        if (placeTrees)
            CheckForTrees();

        if (placeLampposts)
            CheckForLampposts();

        //Debug.Log($"isEditor: {Application.isEditor} || isPlaying: {Application.isPlaying}");
    }


    private void CheckForTrees()
    {
        if (width >= minWidthForTrees && height >= minHeightForTrees)
        {
            //Debug.Log($"Place trees on square with width {width} and height {height}");

            PlaceTreesTop();
            PlaceTreesBottom();
            PlaceTreesLeft();
            PlaceTreesRight();
        }
        else if (placeLampposts)
            CheckForLampposts();
    }


    private void CheckForLampposts()
    {
        PlaceLampTop();
        PlaceLampBottom();
        PlaceLampLeft();
        PlaceLampRight();
    }

    //----------------------------------- PLACEMENT -----------------------------------//

    private void PlaceTree(Vector3 pPosition, List<GameObject> spawnedTreeList)
    {
        GameObject spawnedTree = (GameObject)Instantiate(tree, pPosition, Quaternion.identity);
        spawnedTreeList.Add(spawnedTree);
        spawnedTree.transform.parent = transform;
    }

    private void PlaceLamppost(Vector3 pPosition, List<GameObject> spawnedLampPostsList, string pDirection)
    {
        if (pDirection == "Left")
        {
            GameObject spawnedLamppost = (GameObject)Instantiate(lamppostHorizontal, pPosition, Quaternion.identity);
            spawnedLampPostsList.Add(spawnedLamppost);
            spawnedLamppost.transform.parent = transform;
            spawnedLamppost.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (pDirection == "Right")
        {
            GameObject spawnedLamppost = (GameObject)Instantiate(lamppostHorizontal, pPosition, Quaternion.identity);
            spawnedLampPostsList.Add(spawnedLamppost);
            spawnedLamppost.transform.parent = transform;
            spawnedLamppost.transform.eulerAngles = new Vector3(0, 0, 0);
            spawnedLamppost.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (pDirection == "Top")
        {
            GameObject spawnedLamppost = (GameObject)Instantiate(lamppostVertical, pPosition, Quaternion.identity);
            spawnedLampPostsList.Add(spawnedLamppost);
            spawnedLamppost.transform.parent = transform;
            spawnedLamppost.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (pDirection == "Bottom")
        {
            GameObject spawnedLamppost = (GameObject)Instantiate(lamppostVertical, pPosition, Quaternion.identity);
            spawnedLampPostsList.Add(spawnedLamppost);
            spawnedLamppost.transform.parent = transform;
            spawnedLamppost.transform.eulerAngles = new Vector3(0, 0, 0);
            spawnedLamppost.transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }


    //----------------------------------- Lamppost PLACEMENT -----------------------------------//

    private void PlaceLampTop()
    {
        float distanceBetween = ((float)width / (amountLamppostsTop - 1)) - ((float)1 / (amountLamppostsTop - 1));

        float posX = position.x + 0.5f;
        float posY = position.y + height - 0.5f;

        for (int i = 0; i < amountLamppostsTop; i++)
        {
            Vector3 spawnPos = Vector3.zero;

            if (i == 0)
                spawnPos = new Vector3(posX, 0, posY);
            else
            {
                posX += distanceBetween;
                spawnPos = new Vector3(posX, 0, posY);
            }

            PlaceLamppost(spawnPos, spawnedLamppostsTop, "Top");
        }
    }

    private void PlaceLampBottom()
    {
        float distanceBetween = ((float)width / (amountLamppostsBottom - 1)) - ((float)1 / (amountLamppostsBottom - 1));

        float posX = position.x + 0.5f;
        float posY = position.y + 0.5f;

        for (int i = 0; i < amountLamppostsBottom; i++)
        {
            Vector3 spawnPos = Vector3.zero;

            if (i == 0)
                spawnPos = new Vector3(posX, 0, posY);
            else
            {
                posX += distanceBetween;
                spawnPos = new Vector3(posX, 0, posY);
            }

            PlaceLamppost(spawnPos, spawnedLamppostsBottom, "Bottom");
        }
    }

    private void PlaceLampLeft()
    {
        float distanceBetween = ((float)height / (amountLamppostsLeft - 1)) - ((float)1 / (amountLamppostsLeft - 1));

        float posX = position.x + 0.5f;
        float posY = position.y + 0.5f;

        for (int i = 0; i < amountLamppostsLeft; i++)
        {
            Vector3 spawnPos = Vector3.zero;

            if (i == 0)
                spawnPos = new Vector3(posX, 0, posY);
            else
            {
                posY += distanceBetween;
                spawnPos = new Vector3(posX, 0, posY);
            }

            PlaceLamppost(spawnPos, spawnedLamppostsLeft, "Left");
        }
    }

    private void PlaceLampRight()
    {
        float distanceBetween = ((float)height / (amountLamppostsRight - 1)) - ((float)1 / (amountLamppostsRight - 1));

        float posX = position.x + width - 0.5f;
        float posY = position.y + 0.5f;

        for (int i = 0; i < amountLamppostsRight; i++)
        {
            Vector3 spawnPos = Vector3.zero;

            if (i == 0)
                spawnPos = new Vector3(posX, 0, posY);
            else
            {
                posY += distanceBetween;
                spawnPos = new Vector3(posX, 0, posY);
            }

            PlaceLamppost(spawnPos, spawnedLamppostsRight, "Right");
        }
    }


    //----------------------------------- TREE PLACEMENT -----------------------------------//

    private void PlaceTreesTop()
    {
        float distanceBetween = ((float)width / (amountTreesTop - 1)) - ((float)1 / (amountTreesTop - 1));

        float posX = position.x + 0.5f;
        float posY = position.y + height - 0.5f;

        for (int i = 0; i < amountTreesTop; i++)
        {
            Vector3 spawnPos = Vector3.zero;

            if (i == 0)
                spawnPos = new Vector3(posX, 0, posY);
            else
            {
                posX += distanceBetween;
                spawnPos = new Vector3(posX, 0, posY);
            }

            PlaceTree(spawnPos, spawnedTreesTop);
        }
    }

    private void PlaceTreesBottom()
    {
        float distanceBetween = ((float)width / (amountTreesBottom - 1)) - ((float)1 / (amountTreesBottom - 1));

        float posX = position.x + 0.5f;
        float posY = position.y + 0.5f;

        for (int i = 0; i < amountTreesBottom; i++)
        {
            Vector3 spawnPos = Vector3.zero;

            if (i == 0)
                spawnPos = new Vector3(posX, 0, posY);
            else
            {
                posX += distanceBetween;
                spawnPos = new Vector3(posX, 0, posY);
            }

            PlaceTree(spawnPos, spawnedTreesBottom);
        }
    }

    private void PlaceTreesLeft()
    {
        float distanceBetween = ((float)height / (amountTreesLeft - 1)) - ((float)1 / (amountTreesLeft - 1));

        float posX = position.x + 0.5f;
        float posY = position.y + 0.5f;

        for (int i = 0; i < amountTreesLeft; i++)
        {
            Vector3 spawnPos = Vector3.zero;

            if (i == 0)
                spawnPos = new Vector3(posX, 0, posY);
            else
            {
                posY += distanceBetween;
                spawnPos = new Vector3(posX, 0, posY);
            }

            PlaceTree(spawnPos, spawnedTreesLeft);
        }
    }

    private void PlaceTreesRight()
    {
        float distanceBetween = ((float)height / (amountTreesRight - 1)) - ((float)1 / (amountTreesRight - 1));

        float posX = position.x + width - 0.5f;
        float posY = position.y + 0.5f;

        for (int i = 0; i < amountTreesRight; i++)
        {
            Vector3 spawnPos = Vector3.zero;

            if (i == 0)
                spawnPos = new Vector3(posX, 0, posY);
            else
            {
                posY += distanceBetween;
                spawnPos = new Vector3(posX, 0, posY);
            }

            PlaceTree(spawnPos, spawnedTreesRight);
        }
    }


    //----------------------------------- REMOVE FUNCTIONS -----------------------------------//

    private void RemoveLampposts()
    {
        RemoveLamppostsTop();
        RemoveLamppostsBottom();
        RemoveLamppostsLeft();
        RemoveLamppostsRight();

        Debug.Log("Lampost removed");
    }

    private void RemoveLamppostsTop()
    {
        foreach (GameObject lampPost in spawnedLamppostsTop)
        {


            if (Application.isPlaying)
                GameObject.Destroy(lampPost.gameObject);
            else if (Application.isEditor)
                GameObject.DestroyImmediate(lampPost.gameObject);
        }

        spawnedLamppostsTop.Clear();
    }

    private void RemoveLamppostsBottom()
    {
        foreach (GameObject lampPost in spawnedLamppostsBottom)
        {
            if (Application.isPlaying)
                GameObject.Destroy(lampPost.gameObject);
            else if (Application.isEditor)
                DestroyImmediate(lampPost.gameObject);
        }

        spawnedLamppostsBottom.Clear();
    }

    private void RemoveLamppostsLeft()
    {
        foreach (GameObject lampPost in spawnedLamppostsLeft)
        {
            if (Application.isPlaying)
                GameObject.Destroy(lampPost.gameObject);
            else if (Application.isEditor)
                GameObject.DestroyImmediate(lampPost.gameObject);
        }

        spawnedLamppostsLeft.Clear();
    }

    private void RemoveLamppostsRight()
    {
        foreach (GameObject lampPost in spawnedLamppostsRight)
        {
            if (Application.isPlaying)
                GameObject.Destroy(lampPost.gameObject);
            else if (Application.isEditor)
                GameObject.DestroyImmediate(lampPost.gameObject);
        }

        spawnedLamppostsRight.Clear();
    }

    private void RemoveTrees()
    {
        RemoveTreesTop();
        RemoveTreesBottom();
        RemoveTreesLeft();
        RemoveTreesRight();
    }

    private void RemoveTreesTop()
    {
        foreach (GameObject tree in spawnedTreesTop)
        {
            if (Application.isPlaying)
                GameObject.Destroy(tree.gameObject);
            else if (Application.isEditor)
                GameObject.DestroyImmediate(tree.gameObject);
        }

        spawnedTreesTop.Clear();
    }

    private void RemoveTreesBottom()
    {
        foreach (GameObject tree in spawnedTreesBottom)
        {
            if (Application.isPlaying)
                GameObject.Destroy(tree.gameObject);
            else if (Application.isEditor)
                DestroyImmediate(tree.gameObject);
        }

        spawnedTreesBottom.Clear();
    }

    private void RemoveTreesLeft()
    {
        foreach (GameObject tree in spawnedTreesLeft)
        {
            if (Application.isPlaying)
                GameObject.Destroy(tree.gameObject);
            else if (Application.isEditor)
                GameObject.DestroyImmediate(tree.gameObject);
        }

        spawnedTreesLeft.Clear();
    }

    private void RemoveTreesRight()
    {
        foreach (GameObject tree in spawnedTreesRight)
        {
            if (Application.isPlaying)
                GameObject.Destroy(tree.gameObject);
            else if (Application.isEditor)
                GameObject.DestroyImmediate(tree.gameObject);
        }

        spawnedTreesRight.Clear();
    }
}
