using UnityEngine;

public class CreateFrame : MonoBehaviour
{
    void Start()
    {
        MeshBuilder builder = new MeshBuilder();

        Vector3 vec1 = new Vector3(1, -1, -0.25f);
        Vector3 vec2 = new Vector3(1, -1.5f, -0.25f);
        Vector3 vec3 = new Vector3(-1.5f, -1, -0.25f);
        Vector3 vec4 = new Vector3(-1.5f, -1.5f, -0.25f);

        Vector3 vec5 = new Vector3(1, -1, 0.25f);
        Vector3 vec6 = new Vector3(-1.5f, -1, 0.25f);
        Vector3 vec7 = new Vector3(1, -1.5f, 0.25f);
        Vector3 vec8 = new Vector3(-1.5f, -1.5f, 0.25f);
        for (int i = 0; i < 4; i++)
        {
            //front
            int v1 = builder.AddVertex(transform.position + vec1);
            int v2 = builder.AddVertex(transform.position + vec2);
            int v3 = builder.AddVertex(transform.position + vec3);
            int v4 = builder.AddVertex(transform.position + vec4);

            //top
            int v5 = builder.AddVertex(transform.position + vec1);
            int v6 = builder.AddVertex(transform.position + vec3);
            int v7 = builder.AddVertex(transform.position + vec5);
            int v8 = builder.AddVertex(transform.position + vec6);

            //bottom
            int v9 = builder.AddVertex(transform.position + vec2);
            int v10 = builder.AddVertex(transform.position + vec4);
            int v11 = builder.AddVertex(transform.position + vec7);
            int v12 = builder.AddVertex(transform.position + vec8);

            //left
            int v13 = builder.AddVertex(transform.position + vec4);
            int v14 = builder.AddVertex(transform.position + vec3);
            int v15 = builder.AddVertex(transform.position + vec8);
            int v16 = builder.AddVertex(transform.position + vec6);

            //front
            builder.AddTriangle(v1, v2, v3);
            builder.AddTriangle(v4, v3, v2);

            //top
            builder.AddTriangle(v6, v8, v5);
            builder.AddTriangle(v7, v5, v8);

            //bottom
            builder.AddTriangle(v10, v9, v12);
            builder.AddTriangle(v11, v12, v9);

            //left
            builder.AddTriangle(v13, v15, v14);
            builder.AddTriangle(v16, v14, v15);
            Debug.Log(vec1);
            vec1 = RotateVector(vec1);
            vec2 = RotateVector(vec2);
            vec3 = RotateVector(vec3);
            vec4 = RotateVector(vec4);
            vec5 = RotateVector(vec5);
            vec6 = RotateVector(vec6);
            vec7 = RotateVector(vec7);
            vec8 = RotateVector(vec8);
        }

        GetComponent<MeshFilter>().mesh = builder.CreateMesh();
    }

    private Vector3 RotateVector(Vector3 v)
    {
        Vector3 rotatedvector = new Vector3(v.x * Mathf.Cos(90 * Mathf.Deg2Rad) - v.y * Mathf.Sin(90 * Mathf.Deg2Rad), v.x * Mathf.Sin(90 * Mathf.Deg2Rad) + v.y * Mathf.Cos(90 * Mathf.Deg2Rad), v.z);
        return rotatedvector;
    }
}
