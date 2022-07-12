using UnityEngine;

namespace Demo
{
    public class SimpleStock : Shape
    {
        // grammar rule probabilities:
        private float stockContinueChance = 0.9f;
        private float roofContinueChance = 0.9f;

        // shape parameters:
        [SerializeField]
        public int Width;
        [SerializeField]
        public int Depth;

        // Build parameters
        private bool continueWalls = true;

        [SerializeField]
        GameObject[] wallStyle;
        [SerializeField]
        GameObject[] roofStyle;

        const float buildDelay = 0.1f;

        public void Initialize(int Width, int Depth, GameObject[] wallStyle, GameObject[] roofStyle, float stockContinueChance = 0.5f, float roofContinueChance = 0.5f, bool continueWalls = true)
        {
            this.Width = Width;
            this.Depth = Depth;

            if (wallStyle != null)
                this.wallStyle = wallStyle;

            if (roofStyle != null)
                this.roofStyle = roofStyle;

            this.stockContinueChance = stockContinueChance;
            this.roofContinueChance = roofContinueChance;

            this.continueWalls = continueWalls;
        }

        public void StartBuild()
        {
            Execute();
        }

        protected override void Execute()
        {
            // Create four walls:
            for (int i = 0; i < 4; i++)
            {
                Vector3 localPosition = new Vector3();
                switch (i)
                {
                    case 0:
                        localPosition = new Vector3(-(Width - 1) * 0.5f, 0, 0); // left
                        break;
                    case 1:
                        localPosition = new Vector3(0, 0, (Depth - 1) * 0.5f); // back
                        break;
                    case 2:
                        localPosition = new Vector3((Width - 1) * 0.5f, 0, 0); // right
                        break;
                    case 3:
                        localPosition = new Vector3(0, 0, -(Depth - 1) * 0.5f); // front
                        break;
                }
                SimpleRow newRow = CreateSymbol<SimpleRow>("wall", localPosition, Quaternion.Euler(0, i * 90, 0));
                newRow.Initialize(-1, i % 2 == 1 ? Width : Depth, wallStyle);
                newRow.Generate();
            }

            // Continue with a stock or with a roof (random choice):
            float randomValue = RandomFloat();
            if (randomValue < stockContinueChance)
            {
                SimpleStock nextStock = CreateSymbol<SimpleStock>("stock", new Vector3(0, 1, 0));
                nextStock.Initialize(Width, Depth, wallStyle, roofStyle, stockContinueChance, roofContinueChance, continueWalls);
                nextStock.Generate(buildDelay);
            }
            else
            {
                SimpleRoof nextRoof = CreateSymbol<SimpleRoof>("roof", new Vector3(0, 1, 0));
                nextRoof.Initialize(Width, Depth, roofStyle, wallStyle, stockContinueChance, roofContinueChance, continueWalls);
                nextRoof.Generate(buildDelay);
            }
        }
    }
}
