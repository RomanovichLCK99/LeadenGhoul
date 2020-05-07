﻿using UnityEngine;
using MyUtils;


public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask = 0;

    [Space(10f)]
    [Header("Variables")]
    [SerializeField] float specialMultiplier = 0f;
    [SerializeField] private float weridConstant = 26.57f;
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 5f;
    [Space(10)]
    [Tooltip("Use with caution! Too many polygons will lag!")]
    [SerializeField] private int rayCount = 100;


    private Vector3 origin;
    private float startingAngle;
    private Mesh mesh;
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
        transform.position = Vector3.zero;
    }


    private void LateUpdate()
    {

        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        // Raycount plus one of the origing and plus one plus the zero;
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;


        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {

            Vector3 vertex;
            RaycastHit2D hit = Physics2D.Raycast(origin, Utility.GetVectorFromAngle(angle), viewDistance,layerMask);

            

            if (hit.collider == null)
            {
                //No hit
                // Place on the origin and move towars current angle;
                 vertex = origin + Utility.GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {

                //Hit Object
                //Place vertex exactly on where it hit

                Vector2 localDir;
                
                if (angle < 90f)
                {
                    localDir = Utility.GetVectorFromAngle(angle + weridConstant);
                } else
                {
                    localDir = Utility.GetVectorFromAngle(angle - weridConstant);
                }
                

                float upDot = Vector2.Dot(localDir, Vector2.up);
                float rightDot = Vector2.Dot(localDir, Vector2.right);

                float upPower = Mathf.Abs(upDot);
                float rightPower = Mathf.Abs(rightDot);

                Vector3 vertexPos;

                // Place Vertex in a Vertical Wall

                if (upDot > 0 && upPower > rightPower)
                {
                    vertexPos = hit.point + new Vector2(
                        (Mathf.Clamp( (1f / Vector2.Distance(origin, hit.point)) , 0.05f, 0.5f)) * 0.5f * Mathf.Cos(angle * Mathf.Deg2Rad),
                        Mathf.Clamp((viewDistance - Vector2.Distance(origin, hit.point)) * specialMultiplier * (1f / -Mathf.Sin(angle * Mathf.Deg2Rad) + 2f), 0f, 2f)
                        );

                    /** Mathf.Sin(angle * Mathf.Deg2Rad)*/
                }
                else
                {
                    vertexPos = hit.point;
                }

                vertex = vertexPos;

            }

            


            vertices[vertexIndex] = vertex;

            // Every triangle will go from the origin to the previous vertex to the current vertex
            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }


            vertexIndex++;
            angle -= angleIncrease;

        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);

    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = Utility.GetAngleFromVectorFloat(aimDirection) + fov - ( fov / 2f);
    }

}
