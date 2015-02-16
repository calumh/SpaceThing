using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class entityHandler : MonoBehaviour
{

	public HashSet<blockHandler> entityList = new HashSet<blockHandler>();
	public bool intersectsEntity(int radius, Vector3 point)
	{
		foreach (blockHandler entry in entityList)
		{
			if (circleIntersection(entry, radius, point))
				return true;
		}
		return false;
	}
	bool circleIntersection(blockHandler a, int radius, Vector3 point)
	{
		int distance = distanceBTP(a.transform.position, point);
		if (distance > a.longestRadius + radius + 1)
			return false;
		else
			return true;
	}
	int distanceBTP(Vector3 a, Vector3 b)
	{
		return Mathf.FloorToInt(Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2)));
	}
}
