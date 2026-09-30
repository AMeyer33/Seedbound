using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class FollowLevel2 : MonoBehaviour
{
    [Header("Object To Follow")]
    public Transform objectToFollow;

    [Header("Movement")]
    public float speed = 30f;
    public float stoppingDistance = 5f;

    [Header("Obstacle Avoidance")]
    public LayerMask obstacleMask;

    [Tooltip("Distance between search points.")]
    public float searchStep = 10f;

    [Tooltip("How far the bee will search for a way around an obstacle.")]
    public float searchDistance = 500f;

    [Tooltip("How often the route is recalculated.")]
    public float repathInterval = 0.15f;

    [Tooltip("Extra space around the bee.")]
    public float obstaclePadding = 2f;

    private Rigidbody2D rb;
    private Collider2D beeCollider;

    private List<Vector2> path = new List<Vector2>();
    private int pathIndex = 0;

    private float repathTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        beeCollider = GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    private void FixedUpdate()
    {
        if (objectToFollow == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distance =
            Vector2.Distance(rb.position, objectToFollow.position);

        if (distance <= stoppingDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Recalculate the route frequently.
        repathTimer -= Time.fixedDeltaTime;

        if (repathTimer <= 0f)
        {
            FindPath();
            repathTimer = repathInterval;
        }

        FollowPath();
    }

    // =========================================================
    // FIND PATH
    // =========================================================

    private void FindPath()
    {
        Vector2 start = rb.position;
        Vector2 target = objectToFollow.position;

        // If the flower can be reached directly, fly straight to it.
        if (IsPathClear(start, target))
        {
            path.Clear();
            path.Add(target);
            pathIndex = 0;
            return;
        }

        // Direct route is blocked.
        // Search for a route around it.
        path = SearchForRoute(start, target);
        pathIndex = 0;
    }

    // =========================================================
    // FOLLOW PATH
    // =========================================================

    private void FollowPath()
    {
        if (path == null || path.Count == 0)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (pathIndex >= path.Count)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 waypoint = path[pathIndex];

        Vector2 direction =
            (waypoint - rb.position).normalized;

        rb.linearVelocity = direction * speed;

        if (Vector2.Distance(rb.position, waypoint) < searchStep)
        {
            pathIndex++;
        }
    }

    // =========================================================
    // SEARCH FOR ROUTE
    // =========================================================

    private List<Vector2> SearchForRoute(
        Vector2 start,
        Vector2 target)
    {
        List<SearchNode> open = new List<SearchNode>();
        HashSet<Vector2> closed = new HashSet<Vector2>();

        SearchNode startNode =
            new SearchNode(start);

        startNode.gCost = 0;
        startNode.hCost =
            Vector2.Distance(start, target);

        open.Add(startNode);

        int iterations = 0;

        // Prevent the search from running forever.
        int maxIterations = 5000;

        while (open.Count > 0 &&
               iterations < maxIterations)
        {
            iterations++;

            SearchNode current =
                GetBestNode(open);

            // We found a position from which the target
            // can be reached directly.
            if (IsPathClear(current.position, target))
            {
                List<Vector2> result =
                    BuildPath(current);

                result.Add(target);

                return result;
            }

            open.Remove(current);

            Vector2 key =
                RoundPosition(current.position);

            closed.Add(key);

            foreach (Vector2 neighbour
                     in GetNeighbours(current.position))
            {
                Vector2 neighbourKey =
                    RoundPosition(neighbour);

                if (closed.Contains(neighbourKey))
                    continue;

                // The bee itself must be able to fit here.
                if (!IsPositionOpen(neighbour))
                    continue;

                SearchNode existing =
                    open.Find(
                        n => Vector2.Distance(
                            n.position,
                            neighbour) < 0.01f
                    );

                float newCost =
                    current.gCost +
                    Vector2.Distance(
                        current.position,
                        neighbour);

                if (existing == null)
                {
                    SearchNode node =
                        new SearchNode(neighbour);

                    node.gCost = newCost;

                    node.hCost =
                        Vector2.Distance(
                            neighbour,
                            target);

                    node.parent = current;

                    open.Add(node);
                }
                else if (newCost < existing.gCost)
                {
                    existing.gCost = newCost;
                    existing.parent = current;
                }
            }
        }

        // If no route is found, stop instead of
        // flying through the obstacle.
        return new List<Vector2>();
    }

    // =========================================================
    // NEIGHBOURS
    // =========================================================

    private List<Vector2> GetNeighbours(
        Vector2 position)
    {
        List<Vector2> neighbours =
            new List<Vector2>();

        float s = searchStep;

        Vector2[] directions =
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right,

            new Vector2(1, 1).normalized,
            new Vector2(-1, 1).normalized,
            new Vector2(1, -1).normalized,
            new Vector2(-1, -1).normalized
        };

        foreach (Vector2 direction in directions)
        {
            Vector2 newPosition =
                position + direction * s;

            // Don't search infinitely far away.
            if (Vector2.Distance(
                    rb.position,
                    newPosition) <= searchDistance)
            {
                neighbours.Add(newPosition);
            }
        }

        return neighbours;
    }

    // =========================================================
    // COLLISION CHECKING
    // =========================================================

    private bool IsPathClear(
        Vector2 start,
        Vector2 target)
    {
        Vector2 direction =
            target - start;

        float distance =
            direction.magnitude;

        if (distance <= 0.01f)
            return true;

        direction.Normalize();

        RaycastHit2D hit =
            Physics2D.BoxCast(
                start,
                GetBeeSize(),
                0f,
                direction,
                distance,
                obstacleMask
            );

        return hit.collider == null;
    }

    private bool IsPositionOpen(
        Vector2 position)
    {
        Collider2D obstacle =
            Physics2D.OverlapBox(
                position,
                GetBeeSize(),
                0f,
                obstacleMask
            );

        return obstacle == null;
    }

    private Vector2 GetBeeSize()
    {
        if (beeCollider == null)
            return new Vector2(
                25f,
                25f
            );

        return beeCollider.bounds.size +
               new Vector3(
                   obstaclePadding,
                   obstaclePadding,
                   0f
               );
    }

    // =========================================================
    // PATH
    // =========================================================

    private List<Vector2> BuildPath(
        SearchNode node)
    {
        List<Vector2> result =
            new List<Vector2>();

        SearchNode current = node;

        while (current != null)
        {
            result.Add(current.position);
            current = current.parent;
        }

        result.Reverse();

        return result;
    }

    private SearchNode GetBestNode(
        List<SearchNode> nodes)
    {
        SearchNode best = nodes[0];

        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].FCost < best.FCost)
            {
                best = nodes[i];
            }
        }

        return best;
    }

    private Vector2 RoundPosition(
        Vector2 position)
    {
        return new Vector2(
            Mathf.Round(
                position.x / searchStep) * searchStep,

            Mathf.Round(
                position.y / searchStep) * searchStep
        );
    }

    // =========================================================
    // DEBUG
    // =========================================================

    private void OnDrawGizmosSelected()
    {
        if (path == null ||
            path.Count == 0)
            return;

        Gizmos.color = Color.green;

        for (int i = 0;
             i < path.Count - 1;
             i++)
        {
            Gizmos.DrawLine(
                path[i],
                path[i + 1]
            );
        }

        Gizmos.color = Color.yellow;

        foreach (Vector2 point in path)
        {
            Gizmos.DrawSphere(
                point,
                3f
            );
        }
    }

    // =========================================================
    // SEARCH NODE
    // =========================================================

    private class SearchNode
    {
        public Vector2 position;

        public float gCost;
        public float hCost;

        public SearchNode parent;

        public float FCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public SearchNode(
            Vector2 position)
        {
            this.position = position;
        }
    }
}