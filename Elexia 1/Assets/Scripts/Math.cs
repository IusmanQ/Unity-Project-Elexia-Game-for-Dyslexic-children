using UnityEngine;
using System.Collections;

public static class Math {

	public static Quaternion Rotate (Vector2 point1, Vector2 point2) {
		return Quaternion.FromToRotation (Vector2.up, point1 - point2);
	}

    public static float DistanceXZ (Vector3 point1, Vector3 point2) {
		return Mathf.Sqrt ((point1.x - point2.x) * (point1.x - point2.x) + (point1.z - point2.z) * (point1.z - point2.z));
	}

	public static float GetSlope (Vector2 point1, Vector2 point2) {
        return (point2.y - point1.y) / (point2.x - point1.x);
	}

	public static float GetTheta (float slope) {
		return Mathf.Atan (slope) * 180 / Mathf.PI;
	}

    public static float GetTheta (Vector3 direction) {
		float angle = Mathf.Atan ((direction.z) / (direction.x)) * 180 / Mathf.PI;
        if (direction.x < 0) //for getting angle btw 0 and 360
            angle += 180;
        else if (direction.z < 0) //for getting angle btw 0 and 360
            angle += 360;
        return angle;
    }

    public static float GetTheta (Vector3 point1, Vector3 point2) {
		float angle = Mathf.Atan ((point2.z - point1.z) / (point2.x - point1.x)) * 180 / Mathf.PI;
        if (point2.x < point1.x) //for getting angle btw 0 and 360
            angle += 180;
        else if (point2.z < point1.z) //for getting angle btw 0 and 360
            angle += 360;
        return angle;
    }

    public static float StandardAngle (float angle) { // always return angle btw 0 to 360
        angle = angle % 360;
        if (angle < 0)
            angle += 360;
        return angle;
    }

    public static Vector3 GetDirectionFromTheta (float angle) { // return direction vector
		float slope = Mathf.Tan (angle * Mathf.PI / 180);
        return (new Vector3(1, slope, 0) * (angle >= 90 && angle < 270 ? -1 : 1)).normalized; // for correcting signs in I II III IV quadrants
    }

	public static float SolveQuadraticFormula(float a, float b, float c, int sign) {
		return (-b + sign * Mathf.Sqrt (b * b - 4 * a * c)) / (2 * a);
	}
    
	public static Vector2 GetNewVectorOfMagnitude(Vector2 oldVector, float newMagnitude) {  // newVector with old direction but new magnitude
		// using some Vector`s rules
		float oldMagnitude = Mathf.Sqrt (oldVector.x * oldVector.x + oldVector.y * oldVector.y); // old vector magnitude
		Vector2 direction = oldVector / oldMagnitude; 
		return direction * newMagnitude;
	}

	public static Vector2 GetDistanceSlopeRatio(float distance, float slope) {
		//equating distance formula and slope equation
		// d^2 = delta x ^2 + delta y^2 and m = delta y / delta x;
		Vector2 ratio;
		ratio.x = distance / Mathf.Sqrt (1 + slope * slope);
		ratio.y = slope * ratio.x;
		return ratio;
	}

	public static Vector2 GetTwoLinesIntersectionPoint(Vector2 point1, Vector2 point2, float slope1, float slope2) {
		//line1 eq = y - y1 = m1 (x - x1) & line2 eq = y - y2 = m2 (x - x2)
		//so equating m1(x - x1) + y1 & m2(x - x2) + y2, after simplify we get, x = ((m1x1 - m2x2) - (y1 - y2)) / (m1 -m2)
		Vector2 point;
		point.x = ((slope1 * point1.x - slope2 * point2.x) - (point1.y - point2.y)) / (slope1 - slope2);
		
		point.y = slope1 * (point.x - point1.x) + point1.y; // Using line`s eq => y - y1 = m1 (x - x1)
        return point;
	}

	public static Vector2 GetLineCircleIntersectionPoint(Vector2 circleMid, float slope, float radius, int sign) { // Line Passing From Center of Cirlce
        //line eq = y - y1 = m (x - x1) & circle eq = (y - y1)^2 = r^2 - (x - x1)^2 
		//so equating (y - y1)^2, we get (m(x - x1))^2 = r^2 - (x - x1)^2 after simplify we get, x = sqrt( r^2 / (m^2 + 1) ) + x1
		//As line intersect at 2 points on circle, so find correct point using sign
		if (slope == 0)
            return circleMid + sign * Mathf.Sign(slope) * new Vector2(radius, 0); // add radius to center.x bcz horizontal line
        if (slope == Mathf.Infinity || slope == -Mathf.Infinity)
            return circleMid + sign * Mathf.Sign (slope) * new Vector2 (0, radius); // add radius to center.y bcz vertical line
        Vector2 point;
		point.x = sign * Mathf.Sqrt ((radius * radius) / (slope * slope + 1)) + circleMid.x;
		point.y = slope * (point.x - circleMid.x) + circleMid.y; // Using line`s eq => y - y1 = m (x - x1)
        return point;
	}

	public static Vector2 GetFarthestLineCircleIntersectionPoint(Vector2 circleMid, Vector2 point, float radius) { // Line Passing From Center of Cirlce
		float slope = Math.GetSlope (circleMid, point);
		return GetLineCircleIntersectionPoint(circleMid, slope, radius, circleMid.x > point.x ? 1 : -1);
	}

	public static Vector2 GetNearestLineCircleIntersectionPoint(Vector2 circleMid, Vector2 point, float radius) { // Line Passing From Center of Cirlce
		float slope = Math.GetSlope (circleMid, point);
		return GetLineCircleIntersectionPoint (circleMid, slope, radius, circleMid.x > point.x  ? -1 : 1);
	}

	public static Vector2 GetLineCircleMidPoint(Vector2 circlePoint, Vector2 point, float radius, int sign) { // Line Passing From Center of Cirlce
		//line eq = y - y1 = m (x - x1) & circle eq = (y - y1)^2 = r^2 - (x - x1)^2
        // so equating (y - y1)^2, we get (m(x - x1))^2 = r^2 - (x - x1)^2 after simplify we get x1 = x - sqrt( r^2 / (m^2 + 1) )
        float slope = Math.GetSlope (circlePoint, point);
        if (slope == 0)
            return circlePoint - sign * Mathf.Sign(slope) * new Vector2(radius, 0); // add radius to center.x bcz horizontal line
        if (slope == Mathf.Infinity || slope == -Mathf.Infinity)
                return circlePoint - sign * Mathf.Sign (slope) * new Vector2 (0, radius);
        float temp = Mathf.Sqrt ((radius * radius) / (slope * slope + 1));
		Vector2 center;
        center.x = circlePoint.x - sign * temp; //As line intersect at 2 points on circle, so find correct point
		center.y = slope * (center.x - circlePoint.x) + circlePoint.y; // Using line`s eq => y - y1 = m (x - x1)
        return center;
	}

    public static Vector2[] GetGeneralLineCircleIntersectionPoint(Vector2 circleMid, float radius, Vector2 point, float slope) { // Line Passing From Anywhere at circle
        // line eq = y - y2 = m(x - x2)   => y = mx - mx2 + y2 _______eq.1 
        // circle eq = (y - y1)^2 + (x - x1)^2 = r^2   => y^2 + y1^2 - 2yy1 + x^2 + x1^2 -2xx1 -r^2 = 0 ________ eq.2
        // by putting y from eq.1 in eq.2, we get
        //x^2 (m^2 + 1) + x (-2x1 - 2m^2x2 - 2my1 + 2my2) + (x1^2 + m^2x2^2 + 2mx2y1 - 2mx2y2 + y1^2 -2y1y2 + y2^2 - r^2 = 0)
        //x^2 (m^2 + 1) + x (-2(x1 + m(mx2 + y1 - y2) + (x1^2 + mx2(mx2 + 2(y1 - y2)) + y1(y1 - 2y2) + y2^2 - r^2 = 0)
        float a, b, c;
        Vector2[] points = new Vector2[2];
        if (slope == 0) {
            //eq.2 => x^2 + x(-2x1) + (y(y - 2y1) + x1^2 + y1^2 - r^2) = 0, now solve x
            a = 1;
            b = -2 * circleMid.x;
            c = circleMid.x * circleMid.x + circleMid.y * (circleMid.y - 2 * point.y) + point.y * point.y - radius * radius;
            points[0].y = points[1].y = point.y; //as we know intersection point's y is also given point y bcz of horizontal line
            points[0].x = SolveQuadraticFormula(a, b, c, +1);
            points[1].x = SolveQuadraticFormula(a, b, c, -1);
        }
        else if (slope == Mathf.Infinity || slope == -Mathf.Infinity) {
            //eq.2 => y^2 + y(-2y1) + (x(x - 2x1) + x1^2 + y1^2 - r^2) = 0, now solve y
            a = 1;
            b = -2 * circleMid.y;
            c = point.x * (point.x - 2 * circleMid.x) + circleMid.x * circleMid.x + circleMid.y * circleMid.y - radius * radius;
            points[0].x = points[1].x = point.x; //as we know intersection point's x is also given point x bcz of vertical line
            points[0].y = SolveQuadraticFormula(a, b, c, +1);
            points[1].y = SolveQuadraticFormula(a, b, c, -1);
        }
        else {
            a = slope * slope + 1;
            b = -2 * (circleMid.x + slope * (slope * point.x + circleMid.y - point.y));
            c = circleMid.x * circleMid.x + slope * point.x * (slope * point.x + 2 * (circleMid.y - point.y)) + circleMid.y * (circleMid.y - 2 * point.y) + point.y * point.y - radius * radius;
            points[0].x = SolveQuadraticFormula(a, b, c, +1); //solve x by QuadraticFormula
            points[1].x = SolveQuadraticFormula(a, b, c, -1); //solve x by QuadraticFormula
            points[0].y = slope * (points[0].x - point.x) + point.y; // using eq.1
            points[1].y = slope * (points[1].x - point.x) + point.y; // using eq.1
        }
        return points;
	}
	
    public static Vector2 GetCircleMidPointByCircleThreePoints(Vector2 p1, Vector2 p2, Vector2 p3) {
        //solve circle eq1 = (y - y1)^2 = r^2 - (x - x1)^2 circle eq2 = (y - y2)^2 = r^2 - (x - x2)^2 & circle eq3 = (y - y3)^2 = r^2 - (x - x3)^2
        Vector2 midPoint;
		midPoint.y = ((p1.x - p3.x) * ((p1.x * p1.x + p1.y * p1.y) - (p2.x * p2.x + p2.y * p2.y)) - (p1.x - p2.x) * ((p1.x * p1.x + p1.y * p1.y) - (p3.x * p3.x + p3.y * p3.y)))
						/ (2 * ((p3.y - p1.y) * (p1.x - p2.x) - (p3.x - p1.x) * (p1.y - p2.y)));
		midPoint.x = (p1.x * p1.x - p2.x * p2.x + p1.y * p1.y - p2.y * p2.y - 2 * midPoint.y * (p1.y - p2.y)) / (2 * (p1.x - p2.x));
		return midPoint;
    }

    public static Vector2[] GetTangentPoints(Vector2 circleMid, Vector2 point, float radius) {
        // using perpendicular distance point formula
        float a = radius * radius - (point.x - circleMid.x) * (point.x - circleMid.x);
        float b = 2 * (point.x - circleMid.x) * (point.y - circleMid.y);
        float c = radius * radius - (point.y - circleMid.y) * (point.y - circleMid.y);

        float slope1 = SolveQuadraticFormula(a, b, c, +1);
        float slope2 = SolveQuadraticFormula(a, b, c, -1);
        //As Normal Line is perpendicular to Tangent Line
        Vector2 point1 = GetLineCircleIntersectionPoint(circleMid, -1 / slope1, radius, +1);
        Vector2 point2 = GetLineCircleIntersectionPoint(circleMid, -1 / slope1, radius, -1);
        Vector2 point3 = GetLineCircleIntersectionPoint(circleMid, -1 / slope2, radius, +1);
        Vector2 point4 = GetLineCircleIntersectionPoint(circleMid, -1 / slope2, radius, -1);
        Vector2[] points = new Vector2[2];
        //check which is closest to given point as other point is other side of circle which is useless
        points[0] = (point1 - point).sqrMagnitude < (point2 - point).sqrMagnitude ? point1 : point2;
        points[1] = (point3 - point).sqrMagnitude < (point4 - point).sqrMagnitude ? point3 : point4;
        return points;
    }

    public static Vector2 GetNearestTangentPoint(Vector2 circleMid, Vector2 point, Vector2 targetPoint, float radius) {
        Vector2[] tangentPoints = GetTangentPoints(circleMid, point, radius);
        if ((targetPoint - tangentPoints[0]).sqrMagnitude < (targetPoint - tangentPoints[1]).sqrMagnitude)
			return tangentPoints[0];
		else
			return tangentPoints[1];
	}

    public static Vector2 GetFarthestTangentPoint(Vector2 circleMid, Vector2 point, Vector2 targetPoint, float radius) {
		Vector2[] tangentPoints = GetTangentPoints(circleMid, point, radius);
        if ((targetPoint - tangentPoints[0]).sqrMagnitude > (targetPoint - tangentPoints[1]).sqrMagnitude)
			return tangentPoints[0];
		else
			return tangentPoints[1];
	}

    public static float GetPerpendicularDistanceFromLineToPoint(Vector2 point, Vector2 p1, Vector2 p2) {
        //using formula d = |Ax + Bx + C| / Sqrt (A^2 + B^2)
        //AND y = mx + C, or mx - y + C = 0 so A = X, B = -1 and C = y - mx for another point on line 
        //and m is slope of line having p1 and p2
        float slope = Math.GetSlope(p1, p2);
        if (slope == Mathf.Infinity || slope == -Mathf.Infinity)
            return Mathf.Abs (point.y - p1.y);
        float distance = Mathf.Abs (slope * point.x - point.y + (p1.y - slope * p1.x)) / Mathf.Sqrt (slope * slope + 1);
		return distance;
	}

    public static Vector2 GetPerpendicularPointFromLineToPoint(Vector2 point, Vector2 p1, Vector2 p2) {
        //using m1 = y2 - y1 / x2 - x1 and m2 = y2 - y3 / x2 - x3
        // or y2 - y1 = m1x2 - m1x1 and y2 - y3 = m2x2 - m2x3
        //AND by comparing y2 from both sides, we get m1x2 - m2x2 = m1x1 - m2x3 - y1 + y3
        //or x2 = (m1x1 - m2x3 - y1 + y3) / (m1 - m2) and put in any one first line equation to get y2
        float slope1 = Math.GetSlope(p1, p2);
        if (slope1 == 0)
            return new Vector2(p1.x, point.y);
        if (slope1 == Mathf.Infinity || slope1 == -Mathf.Infinity)
            return new Vector2 (point.x, p1.y);
        float slope2 = -1 / slope1;
        Vector2 p;
        p.x = (slope1 * p1.x - slope2 * point.x - p1.y + point.y) / (slope1 - slope2);
        p.y = slope1 * (p.x - p1.x) + p1.y;
		return p;
    }

    public static bool IsPointWithInRect (Vector2 point, Vector2 p1, Vector2 p2) {
        Vector2 uCorner = new Vector2(p1.x > p2.x ? p1.x : p2.x, p1.y > p2.y ? p1.y : p2.y);
        Vector2 lCorner = new Vector2(p1.x < p2.x ? p1.x : p2.x, p1.y < p2.y ? p1.y : p2.y);
        return point.x < uCorner.x && point.x > lCorner.x && point.y < uCorner.y && point.y > lCorner.y;
    }
}
