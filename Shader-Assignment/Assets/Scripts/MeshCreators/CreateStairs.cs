using UnityEngine;

namespace Handout
{
    public class CreateStairs : MonoBehaviour
    {
        public int numberOfSteps = 10;
        // The dimensions of a single step of the staircase:
        public float width = 3;
        public float height = 1;
        public float depth = 1;
        [Space(20)]
        public float degrees = 3;
        public float heightOffset = 1;
        public float depthOffset = 1;

        MeshBuilder builder;

        void Start()
        {
            builder = new MeshBuilder();
            CreateShape();
            GetComponent<MeshFilter>().mesh = builder.CreateMesh(true);
        }

        /// <summary>
        /// Creates a stairway shape in [builder].
        /// </summary>
        void CreateShape()
        {
            builder.Clear();

            /**
            // V1: single step, hard coded:
            // bottom:
            int v1 = builder.AddVertex(new Vector3(2, 0, 0), new Vector2(1, 0));
            int v2 = builder.AddVertex(new Vector3(-2, 0, 0), new Vector2(0, 0));
            // top front:
            int v3 = builder.AddVertex(new Vector3(2, 1, 0), new Vector2(1, 0.5f));
            int v4 = builder.AddVertex(new Vector3(-2, 1, 0), new Vector2(0, 0.5f));
            // top back:
            int v5 = builder.AddVertex(new Vector3(2, 1, 1), new Vector2(1, 1));
            int v6 = builder.AddVertex(new Vector3(-2, 1, 1), new Vector2(0, 1));

            // side R:
            int v7 = builder.AddVertex(new Vector3(2, 0, 0), new Vector2(0, 0));
            int v8 = builder.AddVertex(new Vector3(2, 1, 0), new Vector2(0, 1));
            int v9 = builder.AddVertex(new Vector3(2, 1, 1), new Vector2(1, 1));

            // side L:
            int v10 = builder.AddVertex(new Vector3(-2, 0, 0), new Vector2(1, 0));
            int v11 = builder.AddVertex(new Vector3(-2, 1, 0), new Vector2(1, 1));
            int v12 = builder.AddVertex(new Vector3(-2, 1, 1), new Vector2(0, 1));

            // front:
            builder.AddTriangle(v1, v2, v3);
            builder.AddTriangle(v4, v3, v2);
            // top:
            builder.AddTriangle(v3, v4, v5);
            builder.AddTriangle(v6, v5, v4);

            //back:
            builder.AddTriangle(v6, v2, v1);
            builder.AddTriangle(v1, v5, v6);

            // sides:
            builder.AddTriangle(v7, v8, v9);
            builder.AddTriangle(v12, v11, v10);
            /**/
            // V2, with for loop:
            for (int i = 0; i < numberOfSteps; i++)
            {
                var usedDepthOffset = depthOffset;
                Vector3 offset = new Vector3(0 - usedDepthOffset * Mathf.Sin(degrees * i * Mathf.Deg2Rad), heightOffset, usedDepthOffset * Mathf.Cos(degrees * i * Mathf.Deg2Rad));

                var savedWidth = width;
                var savedDepth = depth;
                //width = savedWidth * Mathf.Cos(3 * i * Mathf.Deg2Rad) - savedDepth * Mathf.Sin(3 * Mathf.Deg2Rad);
                //depth = savedWidth * Mathf.Sin(3 * Mathf.Deg2Rad) + savedDepth * Mathf.Cos(3 * Mathf.Deg2Rad);
                // bottom:
                int v1 = builder.AddVertex(offset * i + new Vector3(width, 0, 0), new Vector2(1, 0));
                int v2 = builder.AddVertex(offset * i + new Vector3(-width, 0, 0), new Vector2(0, 0));
                // top front:
                int v3 = builder.AddVertex(offset * i + new Vector3(width, height, 0), new Vector2(1, 0.5f));
                int v4 = builder.AddVertex(offset * i + new Vector3(-width, height, 0), new Vector2(0, 0.5f));
                // top back:
                int v5 = builder.AddVertex(offset * i + new Vector3(width, height, depth), new Vector2(1, 1));
                int v6 = builder.AddVertex(offset * i + new Vector3(-width, height, depth), new Vector2(0, 1));
                // side R:
                int v7 = builder.AddVertex(offset * i + new Vector3(width, 0, 0), new Vector2(0, 0));
                int v8 = builder.AddVertex(offset * i + new Vector3(width, height, 0), new Vector2(0, 1));
                int v9 = builder.AddVertex(offset * i + new Vector3(width, height, depth), new Vector2(1, 1));

                // side L:
                int v10 = builder.AddVertex(offset * i + new Vector3(-width, 0, 0), new Vector2(1, 0));
                int v11 = builder.AddVertex(offset * i + new Vector3(-width, height, 0), new Vector2(1, 1));
                int v12 = builder.AddVertex(offset * i + new Vector3(-width, height, depth), new Vector2(0, 1));

                //back:
                int v13 = builder.AddVertex(offset * i + new Vector3(width, 0, 0), new Vector2(1, 0));
                int v14 = builder.AddVertex(offset * i + new Vector3(-width, 0, 0), new Vector2(0, 0));
                int v15 = builder.AddVertex(offset * i + new Vector3(width, height, depth), new Vector2(1, 1));
                int v16 = builder.AddVertex(offset * i + new Vector3(-width, height, depth), new Vector2(0, 1));

                // TODO 2: Fix the winding order (everything clockwise):
                // front:
                builder.AddTriangle(v1, v2, v3);
                builder.AddTriangle(v4, v3, v2);
                // top:
                builder.AddTriangle(v3, v4, v5);
                builder.AddTriangle(v6, v5, v4);

                //back:
                builder.AddTriangle(v16, v14, v13);
                builder.AddTriangle(v13, v15, v16);

                // sides:
                builder.AddTriangle(v7, v8, v9);
                builder.AddTriangle(v12, v11, v10);

                // TODO 3: make the mesh solid by adding left, right and back side.

                // TODO 5: Fix the normals by *not* reusing a single vertex in multiple triangles with different normals (solve it by creating more vertices at the same position)
            }

        }

    }
}