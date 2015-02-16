using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class block : MonoBehaviour {
	public Transform shotPrefab;
    public float mass;
    public float volume;
    public float damage = 0;
	public float damageMax =1;
    public int id;
    public int type;
    public int size;
    public int conType;
    public bool isCockpit = false;
    public bool up = true;
    public bool down = true;
    public bool left = true;
    public bool right = true;
	public int xMod = 0;
	public int yMod = 0;
	public int thrusterSpeed = 0;
	public blockHandler parentBH;
	public SpriteRenderer spriteRend;
	public HashSet<block> mbConnected = new HashSet<block>();
	private bool canRun = false; //prevents run on cleanup


	bool destroyInitiated =false;
	public void Damage(int damageCount)
	{
		damage -= damageCount;

		if (damage <= 0)
		{

			canRun = true;
			Destroy(gameObject);
		}
	}
	public bool isPlayer = false;
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// Is this a shot?
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			if (!isPlayer)
			{
				Damage(shot.damage);
				// Destroy the shot
				Destroy(shot.gameObject);
			}
		}
	}

    void Start(){
		spriteRend = gameObject.GetComponent<SpriteRenderer>();
		//this.gameObject.collider2D.enabled = false;
        mbConnected.Add(this);
        switch (id)
        {
            case 1:
				Transform temp = ((GameObject) Instantiate(Resources.Load("2_0"))).transform;
				temp.GetComponent<block>().xMod = 1;
				temp.parent = transform.parent;
				mbConnected.Add(temp.GetComponent<block>());
				Transform temp1 = ((GameObject) Instantiate(Resources.Load("2_0"))).transform;
				temp1.GetComponent<block>().xMod = -1;
				mbConnected.Add(temp1.GetComponent<block>());
				temp1.parent = transform.parent;
				Transform temp2 = ((GameObject) Instantiate(Resources.Load("2_0"))).transform;
				temp2.GetComponent<block>().xMod = -1;
				temp2.GetComponent<block>().yMod = -1;
				mbConnected.Add(temp2.GetComponent<block>());
				temp2.parent = transform.parent;

				temp.GetComponent<block>().mbConnected.Add(this);
				temp1.GetComponent<block>().mbConnected.Add(this);
				temp2.GetComponent<block>().mbConnected.Add(this);

				temp1.GetComponent<block>().mbConnected.Add(temp.GetComponent<block>());
				temp2.GetComponent<block>().mbConnected.Add(temp.GetComponent<block>());

				temp.GetComponent<block>().mbConnected.Add(temp1.GetComponent<block>());
				temp2.GetComponent<block>().mbConnected.Add(temp1.GetComponent<block>());

				temp.GetComponent<block>().mbConnected.Add(temp2.GetComponent<block>());
				temp1.GetComponent<block>().mbConnected.Add(temp2.GetComponent<block>());
                break;
        }    
    }
	//make sure to remove multiblock connected blocks on destroy
	void OnDestroy(){

		//if multiblock, destroy other mb components
		foreach (block entry in mbConnected)
		{
			if (entry != this && entry != null) { 
				Destroy(entry.gameObject);
			
			}
		} 
		if (parentBH != null && canRun == true)
		{
			//int startTime = Environment.TickCount;
			//int starttime1 = 0;
			//int starttime2 = 0;
			//int starttime3 = 0;
			//remove destroyed from parent entity's block list
			if (parentBH.editMode == false)
			{
				parentBH.entityBlocks.Remove(transform.localPosition);
				parentBH.recalcAttributes();
				//starttime1 = Environment.TickCount;
			}

			//if this is the central block of the parent, set a new central block
			if (transform.localPosition == parentBH.central.transform.localPosition)
			{
				parentBH.newCentral();
				//starttime2 = Environment.TickCount;
			}
	
			if (parentBH.editMode == false)
			{
				parentBH.scrapper();
				//starttime3 = Environment.TickCount;

			}
			
			if (transform.parent.childCount == 1 && parentBH.editMode == false)
			{

				transform.parent.transform.parent.GetComponent<entityHandler>().entityList.Remove(transform.parent.GetComponent<blockHandler>());
				Destroy(transform.parent.gameObject);
			}
			//print(((Environment.TickCount - startTime) / 1) + " Recalc " + ((starttime1 - startTime) / 1) +" Scrap " + ((starttime3 - starttime1) / 1));
		}

	}
	public bool bLef = false;
	public bool bRig = false;
	public bool bUp = false;
	public bool bDown = false;
	


	public IEnumerator build(chance ch, Vector3 pos, Quaternion rot)
	{

		
		parentBH = transform.parent.GetComponent<blockHandler>();
		parentBH.buildInt++;
		int j = parentBH.buildInt;
		ch.reCalcChance(Mathf.FloorToInt(Mathf.Sqrt(Mathf.Pow(transform.localPosition.x - 0, 2) + Mathf.Pow(transform.localPosition.y - 0, 2))));
		chance chancer = new chance(ch.leftI, ch.rightI, ch.upI, ch.downI, ch.decrement);
		Dictionary<Vector3, block> list = parentBH.entityBlocks;
		var loaded = Resources.Load("2_0");
		yield return true;
		//Refractor Up
		//if  "Up" exists, then skip it
		Vector3 up = new Vector3(transform.localPosition.x, transform.localPosition.y + 1, 0);
		if (!list.ContainsKey(up) && ch.upB)
		{
			//otherwise findConnected on Up and add return result to connected
			Transform temp = ((GameObject)Instantiate(loaded, transform.position, rot)).transform;
			temp.parent = transform.parent;
			temp.localPosition = up;
			block tempB = temp.GetComponent<block>();
			list.Add(temp.localPosition, tempB);

			StartCoroutine(tempB.build(chancer, up, rot));
		}
		yield return true;
		//Refractor Down
		//if  "Down" exists, then skip it
		Vector3 down = new Vector3(transform.localPosition.x, transform.localPosition.y - 1, 0);
		if (!list.ContainsKey(down) && ch.downB)
		{
			//otherwise findConnected on Up and add return result to connected
			Transform temp = ((GameObject)Instantiate(loaded, transform.position, rot)).transform;
			temp.parent = transform.parent;
			temp.localPosition = down;
			block tempB = temp.GetComponent<block>();
			list.Add(temp.localPosition, tempB);

			StartCoroutine(tempB.build(chancer, down, rot));
		}
		yield return true;
		//Refractor Left
		//if  "Left" exists, then skip it
		Vector3 left = new Vector3(transform.localPosition.x - 1, transform.localPosition.y, 0);
		if (!list.ContainsKey(left) && ch.leftB)
		{
			//otherwise findConnected on Up and add return result to connected
			Transform temp = ((GameObject)Instantiate(loaded, transform.position, rot)).transform;
			temp.parent = transform.parent;
			temp.localPosition = left;
			block tempB = temp.GetComponent<block>();
			list.Add(temp.localPosition, tempB);

			StartCoroutine(tempB.build(chancer, left, rot));
		}
		yield return true;
		//Refractor Right
		//if  "Right" exists, then skip it
		Vector3 right = new Vector3(transform.localPosition.x + 1, transform.localPosition.y, 0);
		if (!list.ContainsKey(right) && ch.rightB)
		{
			//otherwise findConnected on Up and add return result to connected
			Transform temp = ((GameObject)Instantiate(loaded, transform.position, rot)).transform;
			temp.parent = transform.parent;
			temp.localPosition = right;
			block tempB = temp.GetComponent<block>();
			list.Add(temp.localPosition, tempB);

			StartCoroutine(tempB.build(chancer, right, rot));
		}
		/*
		if (j == 1)
		{
			print(this == parentBH.central);
			print("done");
			parentBH.enableColiders();
			parentBH.recalcAttributes();
		}*/
		//collider2D.enabled = true;
	}
	
	public Dictionary<Vector3, block> findConnected(Dictionary<Vector3, block> list, Dictionary<Vector3, block> connected)
    {
			if (!connected.ContainsKey(transform.localPosition))
				connected.Add(transform.localPosition, this);
            //Refractor Up
			Vector3 upV = new Vector3(transform.localPosition.x, transform.localPosition.y + 1, 0);
			if (list.ContainsKey(upV))
                //if connected already contains "Up", then skip it
				if (!connected.ContainsKey(upV))
                    //otherwise findConnected on Up and add return result to connected
                    //check first if this block and connect up and if the connecting block can connect back down
					if (up && list[upV].down)
						list[upV].findConnected(list, connected);

            //Refractor Down
			Vector3 downV = new Vector3(transform.localPosition.x, transform.localPosition.y - 1, 0);
			if (list.ContainsKey(downV))
                //if connected already contains "Down", then skip it
				if (!connected.ContainsKey(downV))
                    //otherwise findConnected on Down and add return result to connected
                    //check first if this block and connect down and if the connecting block can connect back up
					if (down && list[downV].up)
						list[downV].findConnected(list, connected);

            //Refractor Left
			Vector3 leftV =new Vector3(transform.localPosition.x - 1, transform.localPosition.y, 0) ;
            if (list.ContainsKey(leftV))
                //if connected already contains "Left", then skip it
				if (!connected.ContainsKey(leftV))
                    //otherwise findConnected on Left and add return result to connected
                    //check first if this block and connect left and if the connecting block can connect back right
                    if (left && list[leftV].right)
                        list[leftV].findConnected(list, connected);


            //Refractor Right
			Vector3 rightV = new Vector3(transform.localPosition.x + 1, transform.localPosition.y, 0);
            if (list.ContainsKey(rightV))
                //if connected already contains "Right", then skip it
				if (!connected.ContainsKey(rightV))
                    //otherwise findConnected on Right and add return result to connected
                    //check first if this block and connect right and if the connecting block can connect back left
                    if (right && list[rightV].left)
                        list[rightV].findConnected(list, connected);
        return connected;

    }

	public void fireWeapon(){
		if (shotPrefab != null)
		{
			// Create a new shot
			var shotTransform = ((Transform) Instantiate(shotPrefab));
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			// Assign position
			shotTransform.position = transform.position;
			shotTransform.rotation = Quaternion.LookRotation(transform.position - mousePos, Vector3.forward); ;
			// The is enemy property
			
			ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
			if (shot != null)
			{
				shot.isEnemyShot = false;
			}
			Vector3 moveDirection = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0).normalized;
			// Make the weapon shot always towards it
			MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
			if (move != null)
			{
				move.direction = moveDirection; // towards in 2D space is the right of the sprite
				//print(this.transform.up);
			}
		}
	}

	public void setParentBH(blockHandler bh)
	{
		parentBH = bh;
	}




    public class chance{
		public bool leftB =false;
		public bool rightB =false;
		public bool upB = false;
		public bool downB= false;

		public int leftI;
		public int rightI;
		public int upI;
		public int downI;
		public int decrement;
		public chance(int l, int r, int u, int d, int dec)
		{
			leftI = l;
			rightI = r;
			upI = u;
			downI = d;
			decrement = dec;
		}
		
		public chance reCalcChance(int numFromCenter){
			/*
			int tL = leftI - decrement * numFromCenter;
			int tR = rightI - decrement * numFromCenter;
			int tU = upI - decrement * numFromCenter;
			int tD = downI - decrement * numFromCenter;
		
			leftB = (tL >= 0) ? true : false;
			rightB = (tR >= 0) ? true : false;
			upB = (tU >= 0) ? true : false;
			downB = (tD >= 0) ? true : false;
			*/
			int tL = leftI - decrement * numFromCenter;
			int tR = rightI - decrement * numFromCenter;
			int tU = upI - decrement * numFromCenter;
			int tD = downI - decrement * numFromCenter;




			leftB = (UnityEngine.Random.Range(0, leftI*2) >= numFromCenter && (tL >= 0)) ? true : false;
			rightB = (UnityEngine.Random.Range(0, rightI * 2) >= numFromCenter && (tR >= 0)) ? true : false;
			upB = (UnityEngine.Random.Range(0, upI * 2) >= numFromCenter && (tU >= 0)) ? true : false;
			downB = (UnityEngine.Random.Range(0, downI * 2) >= numFromCenter && (tD >= 0)) ? true : false;

			return this;
		}

	}
}
