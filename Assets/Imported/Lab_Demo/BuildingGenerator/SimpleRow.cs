using UnityEngine;

namespace Demo
{
    public class SimpleRow : Shape
    {
        int RowNumber = -1;

        [Header("Stats")]
        public float Amount;
        public Vector3 direction;

        [Header("Visual")]
        public GameObject[] prefabs = null;

        public void Initialize(int RowNumber, float Amount, GameObject[] prefabs, Vector3 dir = new Vector3())
        {
            this.RowNumber = RowNumber;
            this.Amount = Amount;
            this.prefabs = prefabs;

            if (dir.magnitude != 0)
                direction = dir;
            else
                direction = new Vector3(0, 0, 1);
        }

        protected override void Execute()
        {
            Debug.Log($"RowNumber: {RowNumber}");

            if (Amount <= 0)
                return;

            int index = RandomInt(prefabs.Length); // choose a random prefab index

            for (int i = 0; i < Amount; i++)
            {   // spawn the prefabs, randomly chosen

                if (RowNumber >= 0)
                {
                    if (i == 0 || i == Amount - 1)
                        index = 0;
                    else
                        index = 1;

                    if (RowNumber == 0)
                        index = 0;
                }

                SpawnPrefab(prefabs[index],
                    direction * (i - (Amount - 1) / 2f), // position offset from center
                    Quaternion.identity         // no rotation
                );
            }
        }

        public void Generate()
        {
            RemoveExistingBuilding();
            Execute();
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
    }
}
