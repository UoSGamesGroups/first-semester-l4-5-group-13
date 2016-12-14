using UnityEngine;
using System.Collections;

public class Controller2D : RaycastController
{
    float maxSlopeAngle = 50;

    public CollisionInfo collisions;
    Vector2 playerInput;

    public override void Start()
    {
        base.Start();
        collisions.faceDir = 1;
    }

    public void Move(Vector3 velcoity, bool onPlatform)
    {
        Move(velcoity, Vector2.zero, onPlatform);
    }

    public void Move(Vector3 velocity, Vector2 input, bool onPlatform = false)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;
        playerInput = input;

        if (velocity.y < 0)
        {
            DescSlope(ref velocity);
        }

        if (velocity.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(velocity.x);
        }

        HorCollisions(ref velocity);

        if (velocity.y != 0)
        {
            VertCollisions(ref velocity);
        }

        transform.Translate(velocity);

        if (onPlatform)
        {
            collisions.below = true;
        }
    }

    void HorCollisions(ref Vector3 velocity)
    {
        float dirX = collisions.faceDir;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if (Mathf.Abs(velocity.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for(int i = 0; i < horRayCount; i++)
        {
            Vector2 rayOrigin = (dirX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dirX, rayLength, collisionMask);

            // Debug.DrawRay(rayOrigin, Vector2.right * dirX * rayLength, Color.green);

            if (hit)
            {
                if (hit.distance == 0)
                {
                    continue;
                }
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (collisions.descSlope)
                    {
                        collisions.descSlope = false;
                        velocity = collisions.velocityOld;
                    }
                    float distToSlope = 0;
                    if (slopeAngle != collisions.slopeAngleLastFrame)
                    {
                        distToSlope = hit.distance - skinWidth;
                        velocity.x -= distToSlope * dirX;
                    }
                    ClimbSlope(ref velocity, slopeAngle, hit.normal);
                    velocity.x += distToSlope * dirX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * dirX;
                    rayLength = hit.distance;

                    if(collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collisions.left = dirX == -1;
                    collisions.right = dirX == 1;
                }
            }
        }
    }

    void VertCollisions(ref Vector3 velocity)
    {
        float dirY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for(int i = 0; i < vertRayCount; i++)
        {
            Vector2 rayOrigin = (dirY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (vertRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * dirY, rayLength, collisionMask);

            // Debug.DrawRay(rayOrigin, Vector2.up * dirY * rayLength, Color.green);

            if (hit)
            {
                if (hit.collider.tag == "CanMoveThrough")
                {
                    if (dirY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if (collisions.fallingThroughPlatform)
                    {
                        continue;
                    }
                    if (playerInput.y == -1)
                    {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", 0.5f);
                        continue;
                    }
                }
                velocity.y = (hit.distance - skinWidth) * dirY;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = dirY == -1;
                collisions.above = dirY == 1;
            }
        }

        if (collisions.climbingSlope)
        {
            float dirX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((dirX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dirX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * dirX;
                    collisions.slopeAngle = slopeAngle;
                    collisions.slopeNormal = hit.normal;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle, Vector2 slopeNormal)
    {
        float moveDist = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDist;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDist * Mathf.Sign(velocity.x);
            collisions.below = collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
            collisions.slopeNormal = slopeNormal;
        }
    }

    void DescSlope(ref Vector3 velocity)
    {
        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs(velocity.y) + skinWidth, collisionMask);
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down, Mathf.Abs(velocity.y) + skinWidth, collisionMask);

        if (maxSlopeHitLeft ^ maxSlopeHitRight)
        {
            SlideDownMaxSlope(maxSlopeHitLeft, ref velocity);
            SlideDownMaxSlope(maxSlopeHitRight, ref velocity);
        }

        if (!collisions.slidingDownMaxSlope)
        {
            float dirX = Mathf.Sign(velocity.x);
            Vector2 rayOrigin = (dirX == -1 ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == dirX)
                    {
                        if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                        {
                            float moveDist = Mathf.Abs(velocity.x);
                            float DescVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDist;
                            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDist * Mathf.Sign(velocity.x);
                            velocity.y -= DescVelocityY;

                            collisions.slopeAngle = slopeAngle;
                            collisions.descSlope = true;
                            collisions.below = true;
                            collisions.slopeNormal = hit.normal;
                        }
                    }
                }
            }
        }
    }
    
    void SlideDownMaxSlope(RaycastHit2D hit, ref Vector3 velocity)
    {
        if(hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle > maxSlopeAngle)
            {
                velocity.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs(velocity.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                collisions.slopeAngle = slopeAngle;
                collisions.slidingDownMaxSlope = true;
                collisions.slopeNormal = hit.normal;
            }
        }
    }

    void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descSlope;
        public bool slidingDownMaxSlope;
        public float slopeAngle, slopeAngleLastFrame;
        public Vector2 slopeNormal;

        public Vector3 velocityOld;

        public int faceDir;

        public bool fallingThroughPlatform;

        public void Reset()
        {
            above = below = left = right = false;
            climbingSlope = descSlope = false;
            slidingDownMaxSlope = false;
            slopeAngle = slopeAngleLastFrame;
            slopeNormal = Vector2.zero;
            slopeAngle = 0;
        }
    }
}
