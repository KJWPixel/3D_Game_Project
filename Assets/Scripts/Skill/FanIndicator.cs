using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FanIndicator : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;

    [SerializeField] private int segments = 30;
    private List<Vector3> directions = new List<Vector3>();
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangle = new List<int>();

    private float currentAngle;
    private void Awake()
    {
        mesh = new Mesh();
        mesh.name = "FanMesh";
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    public void Build(float angle)
    {
        Debug.Log($"Fan Build angle={angle}");
        mesh.Clear();
        vertices.Clear();
        triangle.Clear();

        currentAngle = angle;   
        // 중심점
        vertices.Add(Vector3.zero);
        directions.Add(Vector3.zero);

        float halfAngle = angle * 0.5f;
        float step = angle / segments;

        for(int i = 0; i <= segments; i++)
        {
            float currentAngel = -halfAngle + step * i;
            Vector3 dir = Quaternion.Euler(0f, currentAngel, 0f) * Vector3 .forward;

            //vertices.Add(dir * radius);
            directions.Add(dir.normalized);
            vertices.Add(Vector3.zero); // 처음엔 반지름 0
        }

        for(int i = 1; i < vertices.Count - 1; i++)
        {
            triangle.Add(0);
            triangle.Add(i);
            triangle.Add(i + 1);          
        }

     
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangle, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
    public void UpdateRadius(float radius)
    {
        Debug.Log($"UpdateRadius 호출됨 radius={radius}");

        for (int i = 1; i < vertices.Count; i++)
        {
            vertices[i] = directions[i] * radius;
        }

        mesh.SetVertices(vertices);
        mesh.RecalculateBounds();
    }
}
