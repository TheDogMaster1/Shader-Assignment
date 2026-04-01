using UnityEngine;

namespace Handout
{
    public class CreateSpiralStair : MonoBehaviour
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

            //front
            Vector3 vec1 = new Vector3(0.15f, 0, 0);
            Vector3 vec2 = new Vector3(0.15f + width, 0, 0);
            Vector3 vec3 = new Vector3(0.15f, height, 0);
            Vector3 vec4 = new Vector3(0.15f + width, height, 0);

            //back
            Vector3 vec5 = new Vector3(0.15f, 0, depth);
            Vector3 vec6 = new Vector3(0.15f + width, 0, depth);
            Vector3 vec7 = new Vector3(0.15f, height, depth);
            Vector3 vec8 = new Vector3(0.15f + width, height, depth);

            for (int i = 0; i < numberOfSteps; i++)
            {
                Vector3 offset = new Vector3(0, heightOffset * i, 0);
                vec5 = RotateVector(vec5);
                vec6 = RotateVector(vec6);
                vec7 = RotateVector(vec7);
                vec8 = RotateVector(vec8);

                //front
                int v1 = builder.AddVertex(offset + vec1, new Vector2(0, 0));
                int v2 = builder.AddVertex(offset + vec2, new Vector2(1, 0));
                int v3 = builder.AddVertex(offset + vec3, new Vector2(0, 0.5f));
                int v4 = builder.AddVertex(offset + vec4, new Vector2(1, 0.5f));

                builder.AddTriangle(v1, v3, v2);
                builder.AddTriangle(v4, v2, v3);

                //top
                int v5 = builder.AddVertex(offset + vec3, new Vector2(0, 0));
                int v6 = builder.AddVertex(offset + vec4, new Vector2(1, 0));
                int v7 = builder.AddVertex(offset + vec7, new Vector2(0, 0.2f));
                int v8 = builder.AddVertex(offset + vec8, new Vector2(1, 1));

                builder.AddTriangle(v5, v7, v6);
                builder.AddTriangle(v8, v6, v7);

                //back
                int v9 = builder.AddVertex(offset + vec5, new Vector2(1, 0));
                int v10 = builder.AddVertex(offset + vec6, new Vector2(0, 0));
                int v11 = builder.AddVertex(offset + vec7, new Vector2(1, 0.5f));
                int v12 = builder.AddVertex(offset + vec8, new Vector2(0, 0.5f));

                builder.AddTriangle(v9, v10, v11);
                builder.AddTriangle(v12, v11, v10);

                //bottom
                int v13 = builder.AddVertex(offset + vec1);
                int v14 = builder.AddVertex(offset + vec2);
                int v15 = builder.AddVertex(offset + vec5);
                int v16 = builder.AddVertex(offset + vec6);

                builder.AddTriangle(v13, v14, v15);
                builder.AddTriangle(v16, v15, v14);

                //right
                int v17 = builder.AddVertex(offset + vec2);
                int v18 = builder.AddVertex(offset + vec6);
                int v19 = builder.AddVertex(offset + vec4);
                int v20 = builder.AddVertex(offset + vec8);

                builder.AddTriangle(v17, v19, v18);
                builder.AddTriangle(v20, v18, v19);

                vec1 = RotateVector(vec1);
                vec2 = RotateVector(vec2);
                vec3 = RotateVector(vec3);
                vec4 = RotateVector(vec4);
            }

        }
        private Vector3 RotateVector(Vector3 v)
        {
            Vector3 rotatedvector = new Vector3(v.x * Mathf.Cos(degrees * Mathf.Deg2Rad) - v.z * Mathf.Sin(degrees * Mathf.Deg2Rad), v.y, v.x * Mathf.Sin(degrees * Mathf.Deg2Rad) + v.z * Mathf.Cos(degrees * Mathf.Deg2Rad));
            return rotatedvector;
        }

    }
}