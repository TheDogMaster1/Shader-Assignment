using UnityEngine;

public class CreateFrame : MonoBehaviour
{
    void Start()
    {
        MeshBuilder builder = new MeshBuilder();
        /**
        //front bottom
        int v1 = builder.AddVertex(transform.position + new Vector3(1, -1, 0));
        int v2 = builder.AddVertex(transform.position + new Vector3(1, -1.5f, 0));
        int v3 = builder.AddVertex(transform.position + new Vector3(-1.5f, -1, 0));
        int v4 = builder.AddVertex(transform.position + new Vector3(-1.5f, -1.5f, 0));

        //front left
        int v5 = builder.AddVertex(transform.position + new Vector3(-1.5f, 1.5f, 0));
        int v6 = builder.AddVertex(transform.position + new Vector3(-1, 1.5f, 0));
        int v7 = builder.AddVertex(transform.position + new Vector3(-1, -1, 0));

        //front top

        int v8 = builder.AddVertex(transform.position + new Vector3(1.5f, 1.5f, 0));
        int v9 = builder.AddVertex(transform.position + new Vector3(1.5f, 1, 0));
        int v10 = builder.AddVertex(transform.position + new Vector3(-1, 1, 0));

        //front right

        int v11 = builder.AddVertex(transform.position + new Vector3(1.5f, -1.5f, 0));
        int v12 = builder.AddVertex(transform.position + new Vector3(1, 1, 0));

        builder.AddTriangle(v1, v2, v3);
        builder.AddTriangle(v4, v3, v2);

        builder.AddTriangle(v5, v6, v3);
        builder.AddTriangle(v7, v3, v6);

        builder.AddTriangle(v8, v9, v6);
        builder.AddTriangle(v10, v6, v9);

        builder.AddTriangle(v11, v2, v9);
        builder.AddTriangle(v12, v9, v2);
        /**/

        Vector3 vec1 = new Vector3(1, -1, 0);
        Vector3 vec2 = new Vector3(1, -1.5f, 0);
        Vector3 vec3 = new Vector3(-1.5f, -1, 0);
        Vector3 vec4 = new Vector3(-1.5f, -1.5f, 0);
        for (int i = 0; i < 4; i++)
        {
            vec1 = RotateVector(vec1);
            vec2 = RotateVector(vec2);
            vec3 = RotateVector(vec3);
            vec4 = RotateVector(vec4);

            int v1 = builder.AddVertex(transform.position + vec1);
            int v2 = builder.AddVertex(transform.position + vec2);
            int v3 = builder.AddVertex(transform.position + vec3);
            int v4 = builder.AddVertex(transform.position + vec4);

            builder.AddTriangle(v1, v2, v3);
            builder.AddTriangle(v4, v3, v2);
            Debug.Log(vec1);
        }

        GetComponent<MeshFilter>().mesh = builder.CreateMesh();
    }

    private Vector3 RotateVector(Vector3 v)
    {
        Vector3 rotatedvector = new Vector3(v.x * Mathf.Cos(90 * Mathf.Deg2Rad) - v.y * Mathf.Sin(90 * Mathf.Deg2Rad), v.x * Mathf.Sin(90 * Mathf.Deg2Rad) + v.y * Mathf.Cos(90 * Mathf.Deg2Rad), v.z);
        return rotatedvector;
    }
}
