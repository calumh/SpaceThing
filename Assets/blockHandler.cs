using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class blockHandler : MonoBehaviour {
	Vector3 current;
	public bool recurOnBlock = true;

	public Dictionary<Vector3, block> connectedWithFind;
	public bool connectedWithFindDone = false;
	public IEnumerator findConnected(block block, Dictionary<Vector3, block> list)
	{
		connectedWithFindDone = false;
		//if (recurOnBlock)
		//	return block.findConnected(list, new Dictionary<Vector3, block>());
		//else
		//{
		bool alreadyInside = false;
		if (list.ContainsKey(block.transform.localPosition))
			alreadyInside = true;
		if (!alreadyInside)
			list.Add(block.transform.localPosition, block);

		connectedWithFind = new Dictionary<Vector3, block>();
		i = 0;
		pushneighborsDone = false;
		StartCoroutine(pushneighbors(list, connectedWithFind, block.transform.localPosition));
		while (!pushneighborsDone)
		{
			yield return true;
		} //print("findConnectedgo " + u);
		//print(i + " " + connected.Count + " " + list.Count);
		if (!alreadyInside)
			list.Remove(block.transform.localPosition);
		connectedWithFindDone = true;
		//return connected;
		
	}
	Vector3 upV;
	Vector3 downV;
	Vector3 leftV;
	Vector3 rightV;
	//block block;
	int i = 0;
	bool pushneighborsDone = false;
	IEnumerator pushneighbors(Dictionary<Vector3, block> list, Dictionary<Vector3, block> connected, Vector3 vect)
	{
		i++;
		int j = i;
		

		//block = list[vect];
		if (!connected.ContainsKey(vect))
			connected.Add(vect, list[vect]);
		//Refractor Up
		upV = new Vector3(vect.x, vect.y + 1, 0);
		if (list.ContainsKey(upV))
			if (!connected.ContainsKey(upV))
				if (list[vect].up && list[upV].down)
					StartCoroutine(pushneighbors(list, connected, upV));

		//Refractor Down
		downV = new Vector3(vect.x, vect.y - 1, 0);
		if (list.ContainsKey(downV))
			if (!connected.ContainsKey(downV))
				if (list[vect].down && list[downV].up)
					StartCoroutine(pushneighbors(list, connected, downV));

		//Refractor Left
		leftV = new Vector3(vect.x - 1, vect.y, 0);
		if (list.ContainsKey(leftV))
			if (!connected.ContainsKey(leftV))
				if (list[vect].left && list[leftV].right)
					StartCoroutine(pushneighbors(list, connected, leftV));


		//Refractor Right
		rightV = new Vector3(vect.x + 1, vect.y, 0);
		if (list.ContainsKey(rightV))
			if (!connected.ContainsKey(rightV))
				if (list[vect].right && list[rightV].left)
					StartCoroutine(pushneighbors(list, connected, rightV));
		if (j == 1)
		{
			//print("ggooo");
			pushneighborsDone = true;
		}
		yield return true;
	}






	public Dictionary<Vector3, block> entityBlocks = new Dictionary<Vector3, block>();
	public block central;
	public bool editMode = false;
	public bool isScrap = false;
	public int speed = 0;
	public Vector3 centerOfMass = new Vector3();
	public int longestRadius;
	
	private bool isConnected(block block)
	{
		entityBlocks.Add(block.transform.localPosition, block);
		StartCoroutine(setWhatIsConnected());
		//wait for connection build
		while (!connectedReady ) {  }
		//print("isConnected " + u);
		entityBlocks.Remove(block.transform.localPosition);

		if (whatIsConnected.ContainsKey(block.transform.localPosition))
		{
			return true;
		}

		return false;
	}
	private Dictionary<Vector3, block> whatIsConnected;
	private bool connectedReady = false;
	public IEnumerator setWhatIsConnected()
	{
		connectedReady = false;
		//whatIsConnected = new Dictionary<Vector3, block>();
		if (central != null)
		{
			StartCoroutine(findConnected(central, entityBlocks));
			while (!connectedWithFindDone )
			{
				
				
				yield return true;
			}
			whatIsConnected = connectedWithFind;
			connectedReady = true;
		}
		else
		{
			connectedReady = true;
		}

		
		//return new Dictionary<Vector3, block>();
	}
	/*
	private Dictionary<Vector3, block> findConnected(block block, Dictionary<Vector3, block> list)
	{
		Dictionary<Vector3, block> conPrint = new Dictionary<Vector3, block>();
		return block.findConnected(list, conPrint);
	}
	*/
	public void reColorDisconnect()
	{
		int startTime = Environment.TickCount;
		int starttime1 = 0;
		StartCoroutine(setWhatIsConnected());
		//wait for connection build

		while (!connectedReady ) { }
		//print("reColorDisconnectgo " + u);
		//Dictionary<Vector3, block> connected = whatIsConnected();
		starttime1 = Environment.TickCount;
		foreach (KeyValuePair<Vector3, block> entry in entityBlocks)
		{
			if (!whatIsConnected.ContainsKey(entry.Key) && entry.Value.spriteRend != null)
			{
				//paint sprite red
				entry.Value.spriteRend.color = new Color32(255, 111, 111, 255);

			}
			else if (entry.Value.spriteRend != null)
			{
				//paint normal
				entry.Value.spriteRend.color = new Color32(255, 255, 255, 255);

			}

		}
		//print(((Environment.TickCount - startTime) / 1) + " WhatIs " + ((starttime1 - startTime) / 1) + " Rest " + ((Environment.TickCount - starttime1) / 1));

	}
	void recalcMass()
	{
		float mass = 0;
		foreach (KeyValuePair<Vector3, block> entry in entityBlocks)
		{
			mass = mass + entry.Value.mass;
		
		}
		rigidbody2D.mass = mass;
		centerOfMass = calcCenterOfMass();
	}
	public Vector3 convertLocalToWorld(Vector3 input)
	{
		Vector3 translate = transform.position;
		return input + translate;
		
	}
	private void UpdateKey(Dictionary<Vector3, GameObject> dic,
									  Vector3 fromKey, Vector3 toKey)
	{
		GameObject value = dic[fromKey];
		dic.Remove(fromKey);
		dic[toKey] = value;
	}
	public void newCentral()
	{
		if (entityBlocks.Count != 0)
		{
			Vector3 firstInList = new Vector3();
			foreach (KeyValuePair<Vector3, block> entry in entityBlocks)
			{
				firstInList = entry.Key;
				break;
			}
			central = entityBlocks[firstInList];
		}
	}
	public void enableColiders()
	{
		foreach (KeyValuePair<Vector3, block> entry in entityBlocks)
		{
			entry.Value.collider2D.enabled = true;
		}
		print("complete");
	}
	public void scrapper()
	{
		StartCoroutine(scrapSplits());
	}
	public int buildInt=0;
	public IEnumerator scrapSplits()
	{
		//print("Gooo");
		//int start = Environment.TickCount;
		//int start1 = 0;
		//int start2 = 0;
		//int start3 = 0;

		//start1 = Environment.TickCount;
		//Dictionary<Vector3, block> connected = whatIsConnected();
		StartCoroutine(setWhatIsConnected());
		//wait for connection build

		int n = 0;
		while (!connectedReady) { yield return true; n++; }
		print("scrapsplitsgo1 "+n);

		//start2 = Environment.TickCount;
		Dictionary<Vector3, block> disConList = new Dictionary<Vector3, block>(entityBlocks);
		Dictionary<Vector3, block> disConListCopy = new Dictionary<Vector3, block>(entityBlocks);
		
		foreach (KeyValuePair<Vector3, block> entry in disConListCopy)
		{
			if (whatIsConnected.ContainsKey(entry.Key))
				disConList.Remove(entry.Key);
		}
		int i = 0;
		while (disConList.Count != 0 && i<20)
		{
			i++;
			Vector3 firstInList = new Vector3();
			foreach (KeyValuePair<Vector3, block> entry in disConList)
			{
				firstInList=entry.Key;
				break;
			}
			StartCoroutine(findConnected(disConList[firstInList], disConList));
			//wait for connection build

			int m = 0;
			while (!connectedReady) { yield return true; m++; }
			print("scrapsplitsgo2 "+m);

			Dictionary<Vector3, block> newConnect = connectedWithFind;
			Transform newEntity = ((GameObject) Instantiate(Resources.Load("entity"), transform.position, transform.rotation)).transform;
			newEntity.transform.parent = transform.parent;
			blockHandler newbh = newEntity.GetComponent<blockHandler>();
			transform.parent.GetComponent<entityHandler>().entityList.Add(newEntity.transform.GetComponent<blockHandler>());
			newEntity.rigidbody2D.velocity = rigidbody2D.velocity;
			newEntity.rigidbody2D.angularVelocity = rigidbody2D.angularVelocity;
			newbh.isScrap = true;
			disConListCopy = new Dictionary<Vector3, block>(disConList);
			foreach (KeyValuePair<Vector3, block> entry in disConListCopy)
			{
				if (newConnect.ContainsKey(entry.Key))
				{
					Vector3 saveLPos = entry.Value.transform.localPosition;
					entry.Value.transform.parent = newEntity;
					entry.Value.transform.localPosition = saveLPos;
					entityBlocks.Remove(entry.Key);
					disConList.Remove(entry.Key);
					newbh.entityBlocks.Add(entry.Key, entry.Value);
				}
			}
			newbh.newCentral();
			newbh.recalcAttributes();
		}
		//start3 = Environment.TickCount;
		//print("Total Scrap " + (Environment.TickCount - start) + " WhatIsCon " + (start2 - start1) + " Remaining " + (start3 - start2));
	}
	Vector3 calcCenterOfMass()
	{
		float xtop = 0;
		float ytop = 0;
		float mass = 0;
		//x
		foreach (KeyValuePair<Vector3, block> entry in entityBlocks)
		{
			xtop = xtop + entry.Key.x * entry.Value.mass;
			ytop = ytop + entry.Key.y * entry.Value.mass;
			mass = mass + entry.Value.mass;

		}
		return new Vector3(xtop / mass, ytop / mass, 0);
	}
	void recalcMovementSpeed(){
		speed = 0;
		foreach (KeyValuePair<Vector3, block> entry in entityBlocks)
		{
			//print(entry.Value.thrusterSpeed);
			if(entry.Value.id == 7){
				speed = speed + entry.Value.thrusterSpeed;
			}
		}
	}
	public void fireWeapons()
	{
		//starta = Environment.TickCount;
		//check if direction if within firing direction
		int angleBound = 25;
		float radians = angleBound * Mathf.PI / 180;
		//print(compareVectors(transform.up, leftBound, false) + " Fire! " + compareVectors(transform.up, rightBound, true));
		foreach (KeyValuePair<Vector3, block> entry in entityBlocks)
		{
				
			Vector3 directionToTarget = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - entry.Value.transform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - entry.Value.transform.position.y, 0).normalized;

			Vector3 rightBound = new Vector3(directionToTarget.x * Mathf.Cos(-radians) - directionToTarget.y * Mathf.Sin(-radians),
											directionToTarget.x * Mathf.Sin(-radians) + directionToTarget.y * Mathf.Cos(-radians),
											0);
			Vector3 leftBound = new Vector3(directionToTarget.x * Mathf.Cos(radians) - directionToTarget.y * Mathf.Sin(radians),
											directionToTarget.x * Mathf.Sin(radians) + directionToTarget.y * Mathf.Cos(radians),
											0);
			//check if target is safe distance from gun
			if (betweenToVectors(transform.up, rightBound, leftBound) && distanceBTP(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) > 1)
			{
				//startb = Environment.TickCount;
				entry.Value.fireWeapon();
				//endb = Environment.TickCount;
			}
		}
		//enda = Environment.TickCount;
		//int total = enda-starta;
		//print(enda + " " + starta);
		//print("Fire time: " + (endb - startb) + " | Rest " + (total));
	}
	bool betweenToVectors(Vector3 a, Vector3 b, Vector2 c){

		return (a.y * c.x - a.x * c.y) < 0 && (a.y * b.x - a.x * b.y) * (a.y * c.x - a.x * c.y) < 0;

	}
	int distanceBTP(Vector3 a, Vector3 b)
	{
		return Mathf.FloorToInt(Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2)));
	}
	int radiusLongestFromCenter()
	{
		int radius = 0;
		foreach (KeyValuePair<Vector3, block> entry in entityBlocks)
		{
			if (distanceBTP(Vector3.zero, entry.Value.transform.localPosition) > radius)
			{
				radius = distanceBTP(Vector3.zero, entry.Value.transform.localPosition);
			}
		}
		return radius;
	}
	public void recalcAttributes(bool Mass = true, bool Color = false, bool Movement = false, bool Radius = true, bool Parents = true)
	{
		if(Mass)
			recalcMass();
		if(Color)
			reColorDisconnect();
		if(Movement)
			recalcMovementSpeed();
	    if(Radius)
			longestRadius=  radiusLongestFromCenter() + 1;
		if(Parents)
			foreach (KeyValuePair<Vector3, block> entry in entityBlocks)
				entry.Value.setParentBH(this);
	}/*
	void Update()
	{
		
		Debug.DrawLine(transform.position, new Vector3(transform.position.x + longestRadius, transform.position.y, 0));
		Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + longestRadius, 0));
		Debug.DrawLine(transform.position, new Vector3(transform.position.x - longestRadius, transform.position.y, 0));
		Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - longestRadius, 0));
		 
	}
	void FixedUpdate()
	{
		//reColorDisconnect();
	}*/

}
