using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptBoy
{
    public static class PolygonUtility
    {
        public static Vector2 ClosestPoint(Vector2[] polygon, Vector3 point)
        {
            int polygonLength = polygon.Length;
            int lastIndex = polygonLength - 1;

            float pointX = point.x;
            float pointY = point.y;

            float closestPointX = pointX;
            float closestPointY = pointY;

            float minDis = float.PositiveInfinity;

            float bX = polygon[lastIndex].x;
            float bY = polygon[lastIndex].y;

            for (int i = 0; i < polygonLength; i++)
            {
                float aX = bX;
                float aY = bY;

                bX = polygon[i].x;
                bY = polygon[i].y;

                float apX = pointX - aX;
                float apY = pointY - aY;

                float abX = bX - aX;
                float abY = bY - aY;

                float sqrMagnitudeAB = abX * abX + abY * abY;
                float dotProduct_ap_ab = apX * abX + apY * abY;
                float d = dotProduct_ap_ab / sqrMagnitudeAB;

                if (d > 0 && d < 1)
                {
                    float hPointX = aX + abX * d;
                    float hPointY = aY + abY * d;

                    float hpX = pointX - hPointX;
                    float hpY = pointY - hPointY;

                    float sqrMagnitudeHP = hpX * hpX + hpY * hpY;

                    if (sqrMagnitudeHP < minDis)
                    {

                        minDis = sqrMagnitudeHP;
                        closestPointX = hPointX;
                        closestPointY = hPointY;

                    }
                }
                else if (d <= 0)
                {
                    float sqrMagnitudeAP = apX * apX + apY * apY;

                    if (sqrMagnitudeAP < minDis)
                    {

                        minDis = sqrMagnitudeAP;
                        closestPointX = aX;
                        closestPointY = aY;

                    }
                }
            }

            Vector2 ClosestPoint;
            ClosestPoint.x = closestPointX;
            ClosestPoint.y = closestPointY;
            return ClosestPoint;
        }

        public static bool IsPointInsidePolygon(Vector2[] polygon, Vector2 point)
        {
            int polygonLength = polygon.Length;
            int lastIndex = polygonLength - 1;

            float pX = point.x;
            float pY = point.y;

            float bX = polygon[lastIndex].x;
            float bY = polygon[lastIndex].y;

            bool inside = false;
            for (int i = 0; i < polygonLength; i++)
            {
                float aX = bX;
                float aY = bY;

                bX = polygon[i].x;
                bY = polygon[i].y;

                inside ^= (bY > pY ^ aY > pY) && ((pX - bX) < (pY - bY) * (aX - bX) / (aY - bY));
            }
            return inside;
        }
    }
}