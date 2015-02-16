using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class GUIitemselect : MonoBehaviour
{
	private string tpr = "Place";
	private bool togglePR = true;
	private Rect box;
	public Transform entityHandlerTransform;
	entityHandler entityHandler;

	void OnGUI()
	{
		box = new Rect(Screen.width - Screen.width / 5, 0, Screen.width / 5, Screen.height);
		GUI.Box(box, "Inventory");
		if (GUI.Button(new Rect(Screen.width - Screen.width / 5 + 2, 2, Screen.width / 5, 20), tpr))
		{
			if (togglePR == true)
			{
				tpr = "Remove";
				togglePR = false;
			}
			else
			{
				tpr = "Place";
				togglePR = true;
			}

		}
		if (GUI.Button(new Rect(Screen.width - Screen.width / 5 + 2, 2+20, Screen.width / 5, 20), "Selection 1"))
		{
			selected = 1;
		}
		if (GUI.Button(new Rect(Screen.width - Screen.width / 5 + 2, 2+40, Screen.width / 5, 20), "MB 2"))
		{
			selected = 2;
		}
		if (GUI.Button(new Rect(Screen.width - Screen.width / 5 + 2, 2+60, Screen.width / 5, 20), "Selection 3"))
		{
			selected = 3;
		}
		if (GUI.Button(new Rect(Screen.width - Screen.width / 5 + 2, 2 + 80, Screen.width / 5, 20), "Selection 4"))
		{
			selected = 4;
		}
		if (GUI.Button(new Rect(Screen.width - Screen.width / 5 + 2, 2 + 100, Screen.width / 5, 20), "Selection 5"))
		{
			selected = 5;
		}
		if (GUI.Button(new Rect(Screen.width - Screen.width / 5 + 2, 2 + 120, Screen.width / 5, 20), "Weapon"))
		{
			selected = 6;
		}
		if (GUI.Button(new Rect(Screen.width - Screen.width / 5 + 2, 2 + 140, Screen.width / 5, 20), "Thruster"))
		{
			selected = 7;
		}
		if (GUI.Button(new Rect(Screen.width - Screen.width / 5 + 2, 2 + 160, Screen.width / 5, 20), "Gen Ast"))
		{
			//int y = 0;
			int startTime = Environment.TickCount;
			int starttime1 =0;
			int starttime2 =0;
			for (int j = 0; j < 1; j++)
			{
				int radius = UnityEngine.Random.Range(50, 50);
				Vector3 test;
				test.z = 0;
				test.x = UnityEngine.Random.Range(-1, 1);
				test.y = UnityEngine.Random.Range(-1, 1);
				
				//prevent intersection
				int i = 0;
				/*
				while (entityHandler.intersectsEntity(radius, test) && i < 50)
				{
					i++;
					//print("Intersection!!!");
					radius = UnityEngine.Random.Range(1, 100);
					test.z = 0;
					test.x = UnityEngine.Random.Range(-200, 200);
					test.y = UnityEngine.Random.Range(-200, 200);
				}*/
				if (i < 50)
				{
					//print("Create");
					//y++;
					Transform newEntity = ((GameObject)Instantiate(Resources.Load("entity"), test, new Quaternion())).transform;

					newEntity.parent = entity.parent;
					blockHandler bh = newEntity.GetComponent<blockHandler>();
				
					entity.parent.GetComponent<entityHandler>().entityList.Add(bh);
					block temp = ((GameObject)Instantiate(Resources.Load("2_0"), newEntity.position, new Quaternion())).GetComponent<block>();
					temp.transform.parent = newEntity;
					temp.transform.localPosition = new Vector3(0, 0, 0);
					bh.central = temp;
					bh.entityBlocks.Add(temp.transform.localPosition, temp);
					block.chance ch = new block.chance(radius, radius, radius, radius, 1);
					starttime1 = Environment.TickCount;
					
					StartCoroutine(temp.GetComponent<block>().build(ch, temp.transform.localPosition, new Quaternion()));
					starttime2 = Environment.TickCount;
					bh.recalcAttributes();
					bh.longestRadius = radius;
				}
			}
			//print(y);
			//print(((Environment.TickCount - startTime) / 1000) + " Build " + ((starttime2 - starttime1) / 1000) + " Recalc " + ((Environment.TickCount - starttime2) / 1000));
		}
		//display list of current small blocks (used for making the ship)
	}
	
	bool OverGui(){ // return true if in forbidden region
		var mouse = Input.mousePosition;
		return box.Contains(mouse);
	}


	//if item is clicked set selected to that item
	//Transform selected = <x>;
	public int selected =1;
	public Transform selected1;
	public Transform selected2;
	public Transform selected3;
	public Transform selected4;
	public Transform selected5;
	public Transform weapon;
	public Transform thruster;
	public Transform entity;

	//place 
	/*
	public void hover()
	{
		var selectedTransform = Instantiate(selected) as Transform;


		//checks if left mouse button is down
			
			//follow mouse
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = 0;
			selectedTransform.position = pos;
			

		
	}
	*/
	Vector3 getGridPos(Vector3 transf)
	{
		Vector3 gridPos = new Vector3();
		double floorX = System.Math.Floor(transf.x);
		double floorY = System.Math.Floor(transf.y);
		if (transf.x > floorX)
			gridPos.x = (float)floorX + (float).5;
		if (transf.x < floorX)
			gridPos.x = (float)floorX - (float).5;
		if (transf.y > floorY)
			gridPos.y = (float)floorY + (float).5;
		if (transf.y < floorY)
			gridPos.y = (float)floorY - (float).5;
		gridPos.z = 0;
		return gridPos;

	}

	//return true if block is connected, false if it isn't
	private bool isConnected(block block)
	{
		entityBlocks.Add(getGridPos(block.transform.localPosition), block);
		Dictionary<Vector3, block> conPrint = whatIsConnected();
		entityBlocks.Remove(getGridPos(block.transform.localPosition));

		if (conPrint.ContainsKey(getGridPos(block.transform.localPosition)))
		{
			return true;
		}

		return false;
	}
	private Dictionary<Vector3, block> whatIsConnected()
	{

		Dictionary<Vector3, block> conPrint = new Dictionary<Vector3, block>();
		if (central != null)
		{
			blockHandler bh = entity.GetComponent<blockHandler>();
			StartCoroutine(bh.findConnected(central, entityBlocks));
			//wait for connection build
			int i = 0;
			while (!bh.connectedWithFindDone) { }
			return bh.connectedWithFind;
		}
		else
			return conPrint;
	}
	/*
	private bool isConnected(Transform transf)
	{
		Vector3 temp = transf.position;
		transf.position = getGridPos(transf);
		Dictionary<Vector3, GameObject> conPrint = new Dictionary<Vector3,GameObject>();
		conPrint = transf.GetComponent<block>().findConnected(entityBlocks, conPrint);
		//restore correct position
		transf.position = temp;
		int i = 0;
		foreach (KeyValuePair<Vector3, GameObject> entry in conPrint)
		{
			i++;
			if(i>1)
				return true;
		 }
		return false;
	}    */





	private Transform selectedTransform;
	private bool holding = false;

	Vector3 pos;
	public Dictionary<Vector3, block> entityBlocks;
	block central;

	void Start()
	{
		entityBlocks = entity.GetComponent<blockHandler>().entityBlocks;
		entity.GetComponent<blockHandler>().editMode = true;
		transform.parent.GetComponent<entityHandler>().entityList.Add(entity.GetComponent<blockHandler>());
		entityHandler = entityHandlerTransform.GetComponent<entityHandler>();
		/*s
		for (int i = 0; i < 5; i++)
		{
			int radius = Random.Range(5, 10);
			Vector3 test;
			test.z = 0;
			test.x = Random.Range(-40, 40);
			test.y = Random.Range(-40, 40);
			Transform newEntity = ((GameObject)Instantiate(Resources.Load("entity"), test, new Quaternion())).transform;
			newEntity.parent = entity.parent;
			block temp = ((GameObject)Instantiate(Resources.Load("2_0"), newEntity.position, new Quaternion())).GetComponent<block>();
			temp.transform.parent = newEntity;
			temp.transform.localPosition = new Vector3(0, 0, 0);
			newEntity.GetComponent<blockHandler>().central = temp;
			newEntity.GetComponent<blockHandler>().entityBlocks.Add(temp.transform.localPosition, temp);
			block.chance ch = new block.chance(radius, radius, radius, radius, 1);
			temp.GetComponent<block>().build(ch, temp.transform.localPosition, new Quaternion());
		}
		 */
	}

	/*
	bool overlap(Transform t){
		foreach (KeyValuePair<Vector3, GameObject> entry in t.GetComponent<block>().mbconnect)
			{
				Vector3 curPos = getGridPos(selectedTransform.position);
				if (entityBlocks.ContainsKey(curPos))
					return false;
			}
		return true;
	}
	*/


	void Update()
	{

		//reColorDisconnect();

		if (togglePR) { 
			if (selectedTransform != null)
			{
				foreach (block entry in selectedTransform.GetComponent<block>().mbConnected)
				{
					Vector3 pos = getGridPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
					pos.x = entry.GetComponent<block>().xMod + pos.x;
					pos.y = entry.GetComponent<block>().yMod + pos.y;
					pos.z = 0;
					entry.transform.position = getGridPos(pos);
					entry.transform.localPosition = getGridPos(entry.transform.localPosition);
					entry.transform.rotation = entity.rotation;
					
				}
				//GUIitemselect test = GetComponent<GUIitemselect>();
				

				//check if overlapping or disconnected
				foreach (block entry in selectedTransform.GetComponent<block>().mbConnected)
				{
					if (entityBlocks.ContainsKey(getGridPos(entry.transform.localPosition)) || OverGui() || (!isConnected(entry) && entry != central))
					{
						//paint sprite red
						entry.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 111, 111, 140);

					}
					else
					{
						//set back to normal (plus transperency)
						entry.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 130);
					}

				}

			}
			if (!OverGui() && Input.GetMouseButtonDown(0))
			{
				if (holding == false)
				{

					holding = true;
				}
  
				switch(selected){
					case 1:
						selectedTransform = Instantiate(selected1, Input.mousePosition, new Quaternion()) as Transform;
						//correctsize
						//selectedTransform.localScale = new Vector3(3.2f, 3.2f, 0);
						break;
					case 2:
						selectedTransform = Instantiate(selected2, Input.mousePosition, new Quaternion()) as Transform;
						//correctsize
						//selectedTransform.localScale = new Vector3(3.2f, 3.2f, 0);
						break;
					case 3:
						selectedTransform = Instantiate(selected3, Input.mousePosition, new Quaternion()) as Transform;
						//correctsize
						//selectedTransform.localScale = new Vector3(3.2f, 3.2f, 0);
						break;
					case 4:
						selectedTransform = Instantiate(selected4, Input.mousePosition, new Quaternion()) as Transform;
						//correctsize
						//selectedTransform.localScale = new Vector3(3.2f, 3.2f, 0);
						break;
					case 5:
						selectedTransform = Instantiate(selected5, Input.mousePosition, new Quaternion()) as Transform;
						//correctsize
						//selectedTransform.localScale = new Vector3(1f, 1f, 0);
						break;
					case 6:
						selectedTransform = Instantiate(weapon, Input.mousePosition, new Quaternion()) as Transform;
						//correctsize
						//selectedTransform.localScale = new Vector3(1f, 1f, 0);
						break;
					case 7:
						selectedTransform = Instantiate(thruster, Input.mousePosition, new Quaternion()) as Transform;
						//correctsize
						//selectedTransform.localScale = new Vector3(1f, 1f, 0);
						break;
				}
				selectedTransform.GetComponent<block>().isPlayer = true;
				selectedTransform.parent = entity;
				selectedTransform.GetComponent<block>().setParentBH(transform.GetComponentInParent<blockHandler>());
				if (selectedTransform.GetComponent<block>().conType == 1) { central = selectedTransform.GetComponent<block>(); entity.GetComponent<blockHandler>().central = central; }
				
				//disable it's box colider until placed.
				selectedTransform.gameObject.collider2D.enabled = false;
				//TODO set mass, id, etc based on selected block
			

				/*
				//transparent
				foreach (Transform entry in selectedTransform.GetComponent<block>().mbConnected)
				{
					entry.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 130);
				}
				 */
				//follow mouse

				//TODO if selected connects to an already placed block, set that border green
				//TODO if selected covers an already placed block, overlay transparent red on said blocks
			}
			else if (Input.GetMouseButtonUp(0) && selectedTransform != null)
			{
				bool okayToPlace = false;
				if (!OverGui()){
					if (selectedTransform == central && !entityBlocks.ContainsKey(central.transform.position))
					{
						okayToPlace = true;
					}
					else
					{
						foreach (block entry in selectedTransform.GetComponent<block>().mbConnected)
						{
							//check for overlap, if there is on any of the blocks, break the loop and set okayToPlace to false
							if (entityBlocks.ContainsKey(getGridPos(entry.transform.localPosition)))
							{
								okayToPlace = false;
								break;
							}
							if (isConnected(entry))
							{
								okayToPlace = true;
							}

						}
					}
				}
				
				//don't place if it overlaps, is over the ui, or is disconnected
				//if (entityBlocks.ContainsKey(gridPos) || OverGui() || (isConnected(selectedTransform)==false))
				if (!okayToPlace)
				{
					//remove selectedTransform (do nothing)
					Destroy(selectedTransform.gameObject);

				}
				else
				{
					foreach (block entry in selectedTransform.GetComponent<block>().mbConnected)
					{
						//set block's up, left, down, right connected blocks
						//place object
						entry.transform.localPosition = getGridPos(entry.transform.localPosition);
						entry.transform.rotation = entity.rotation;
						//enable it's box colider
						entry.gameObject.collider2D.enabled = true;

						//add it's position tied to it's object to the dictionary
						entityBlocks.Add(entry.transform.localPosition, entry);
						
						holding = false;
						//if position of transformation intersects/is disconnected delete the object
						//else keep it and set transparency to normal


					}
					entity.GetComponent<blockHandler>().recalcAttributes(true,true,true,true,true);
					selectedTransform = null;
				}
			}
 
		}
		else
		{
			if (Input.GetMouseButtonUp(0))
			{
				//Remove selected block
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pos.z = 0;
				removeBlock(pos);
				reColorDisconnect();
			}
		}


	}
	void removeBlock(Vector3 pos)
	{
		if (entityBlocks.ContainsKey(getGridPos(pos)))
		{
			if (central != null)
				if (entityBlocks[getGridPos(pos)] == entityBlocks[getGridPos(central.transform.localPosition)])
					central = null;

			Destroy(entityBlocks[getGridPos(pos)].gameObject);
			entityBlocks.Remove(getGridPos(pos));
			entity.GetComponent<blockHandler>().recalcAttributes(true,true,true,true,true);
			//update colors



		}
	}
	void reColorDisconnect()
	{

		Dictionary<Vector3, block> connected = whatIsConnected();
		foreach (KeyValuePair<Vector3, block> entry in entityBlocks)
		{
			if (!connected.ContainsKey(entry.Key) && entry.Value !=central)
			{
				//paint sprite red
				entityBlocks[entry.Key].gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 111, 111, 255);

			}
			else
			{
				//paint normal
				entry.Value.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);

			}

		}
	}
}