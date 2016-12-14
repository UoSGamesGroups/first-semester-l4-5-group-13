using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    public LayerMask collisionMask;

    public const float skinWidth = 0.015f;
    public int horRayCount = 4;
    public int vertRayCount = 4;

    [HideInInspector]
    public float horRaySpacing;
    [HideInInspector]
    public float vertRaySpacing;

    [HideInInspector]
    public BoxCollider2D collider;
    public RaycastOrigins raycastOrigins;

    public virtual void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalcRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalcRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horRayCount = Mathf.Clamp(horRayCount, 2, int.MaxValue);
        vertRayCount = Mathf.Clamp(vertRayCount, 2, int.MaxValue);

        horRaySpacing = bounds.size.y / (horRayCount - 1);
        vertRaySpacing = bounds.size.x / (vertRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}