using UnityEngine;

namespace ScriptBoy
{
    [AddComponentMenu(" Script Boy/PolygonCollider2D Solver")]
    public class PolygonCollider2DSolver : MonoBehaviour
    {
        private PolygonCollider2D polygonCollider2D;
        private new Transform transform;

        private Vector2[][] paths;

        private bool isConvex
        {
            get
            {
                return polygonCollider2D.shapeCount <= 1;
            }
        }

        public bool cachePolygon = true;
        

        private void Awake()
        {
            transform = base.transform;
            polygonCollider2D = GetComponent<PolygonCollider2D>();

            if (cachePolygon)
            {
                CachePolygon();
            }

#if UNITY_EDITOR
            if (GetComponent<Rigidbody2D>() != null)
            {
                Debug.LogWarning("The PolygonCollider2DSolver does not work with Rigidbody2D :|", gameObject);
                Destroy(this);
            }
#endif
        }

        [ContextMenu("Cache Polygon")]
        public void CachePolygon()
        {
            if (!isConvex)
            {
                int pathCount = polygonCollider2D.pathCount;
                paths = new Vector2[pathCount][];
                for (int i = 0; i < pathCount; i++)
                {
                    paths[i] = polygonCollider2D.GetPath(i);
                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (isConvex) return;
          
            Rigidbody2D rigidbody = collision.rigidbody;
            Vector2 centerOfMass = rigidbody.worldCenterOfMass;

            if (polygonCollider2D.OverlapPoint(centerOfMass))
            {
                Vector2 closestPoint = ClosestPoint(centerOfMass);
                Vector2 dir = closestPoint - centerOfMass;
                Vector2 normal = dir.normalized;
                Vector2 b = centerOfMass - normal * 10000;
                float h = 10000 - Vector2.Distance(rigidbody.ClosestPoint(b) , b);
                Vector2 newCenterOfMass = centerOfMass + normal * (dir.magnitude + h);

#if UNITY_EDITOR
                //Debug.DrawLine(closestPoint, centerOfMass, Color.red, 1);
                //Debug.DrawLine(closestPoint, newCenterOfMass, Color.yellow, 1);
#endif

                Vector2 offset = rigidbody.position - centerOfMass;
                rigidbody.position = newCenterOfMass + offset;
            }
        }

        public Vector2 ClosestPoint(Vector2 point)
        {
            point = transform.InverseTransformPoint(point);

            int pathCount = polygonCollider2D.pathCount;

            if (cachePolygon)
            {
                if (paths == null || paths.Length != pathCount)
                {
                    CachePolygon();
                }
            }

            float minSqrDis = float.PositiveInfinity;
            float closestPointX = 0;
            float closestPointY = 0;

            for (int i = 0; i < pathCount; i++)
            {
                Vector2[] path = cachePolygon ? paths[i] : polygonCollider2D.GetPath(i);
                Vector2 closestPointOnPath = PolygonUtility.ClosestPoint(path, point);

                float deltaX = closestPointOnPath.x - point.x;
                float deltaY = closestPointOnPath.y - point.y;
                float sqrDis = deltaX * deltaX + deltaY * deltaY;

                if (sqrDis < minSqrDis)
                {
                    minSqrDis = sqrDis;
                    closestPointX = closestPointOnPath.x;
                    closestPointY = closestPointOnPath.y;
                }
            }

            Vector2 closestPoint;
            closestPoint.x = closestPointX;
            closestPoint.y = closestPointY;
            closestPoint = transform.TransformPoint(closestPoint);
            return closestPoint;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) return;
            if (!cachePolygon) return;
            if (paths == null) return;

            int pathCount = paths.Length;

            for (int j = 0; j < pathCount; j++)
            {
                Vector2[] path = paths[j];
                int length = path.Length;
                if (length < 2) continue;

                Vector2 a;
                Vector2 b = transform.TransformPoint(path[length - 1]);
                for (int i = 0; i < length; i++)
                {
                    a = b;
                    b = transform.TransformPoint(path[i]);
                    Gizmos.DrawLine(a, b);
                }
            }
        }
#endif
    }
}