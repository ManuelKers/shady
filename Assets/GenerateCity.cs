using UnityEngine;
using System.Collections;


public struct Block {
	public Vector2 c1, c2, c3, c4;
	private bool _isInitialized;
	public Block(Vector2 corner1, Vector2 corner2) {		
		this._isInitialized = true;	
		this.c1 = corner1;
		this.c2 = new Vector2(corner1.x, corner2.y);
		this.c3 = corner2;
		this.c4 = new Vector2(corner2.x, corner1.y);
	}
	public Block(float x1, float y1, float x2, float y2){
		this._isInitialized = true;
		this.c1 = new Vector2(x1,y1);
		this.c2 = new Vector2(x1,y2);
		this.c3 = new Vector2(x2,y2);
		this.c4 = new Vector2(x2,y1);
	}

	public bool IsNull {
		get { return !this._isInitialized; }
	}

};

public class GenerateCity : MonoBehaviour {


	private void recursiveBlockDivision(Block block, ArrayList blocks, int loopsLeft) {
		if (loopsLeft < 0) {
			return;
		}
		Block[] newBlocks = subDivideBlock (block);
		if (newBlocks [0].IsNull) {
			blocks.Add (block);
		} else {
//			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//			Vector2 center = (block.c3 + block.c1) / 2;
//			cube.transform.position = new Vector3 (center.x, -3*loopsLeft, center.y);	
//			cube.transform.localScale = new Vector3 (block.c3.x - block.c1.x, 0.01f, block.c3.y - block.c1.y);
			if ((block.c3-block.c1).magnitude > 20f || Random.value <= 0.9) {
				recursiveBlockDivision (newBlocks [0], blocks, loopsLeft-1);
				recursiveBlockDivision (newBlocks [1], blocks, loopsLeft-1);
			}
		}
	}
		
	// Use this for initialization
	void Start () {
		float cityBorder = 150f;
		Block mainBlock = new Block(-cityBorder,-cityBorder, cityBorder,cityBorder);
		ArrayList blocksToFill = new ArrayList ();
		recursiveBlockDivision(mainBlock, blocksToFill, 30);
		Debug.Log (blocksToFill.Count);
		foreach (Block b in blocksToFill) {
			fillBlock (b);
		}
	}

	private Block[] subDivideBlock(Block originBlock) {
		float minimumBlockSize = 2f;
		float roadWidth = Random.Range(0.3f,1f);
	
		bool xSpace = Mathf.Abs(originBlock.c3.x-originBlock.c1.x) > minimumBlockSize * 2.1;
		bool ySpace = Mathf.Abs(originBlock.c3.y-originBlock.c1.y) > minimumBlockSize * 2.1;

		if (!xSpace && !ySpace) { 
			Block b1 = new Block ();
			Block b2 = new Block ();
			Debug.Assert (b1.IsNull);
			Debug.Assert (b2.IsNull);
			return new Block[2]{new Block(), new Block()}; 
		}

		bool xSplit = (xSpace && Random.value < 0.5f) || !ySpace;
		float splitPlace;
		if (xSplit) {
			splitPlace = Random.Range (originBlock.c1.x+minimumBlockSize, originBlock.c3.x-minimumBlockSize);
			Block block1 = new Block (originBlock.c1, new Vector2(splitPlace-roadWidth/2, originBlock.c3.y));
			Block block2 = new Block (new Vector2(splitPlace+roadWidth/2, originBlock.c1.y), originBlock.c3);
			return new Block[2]{block1,block2};
					
		} else {
			splitPlace = Random.Range (originBlock.c1.y+minimumBlockSize, originBlock.c3.y-minimumBlockSize);
			Block block1 = new Block (originBlock.c1, new Vector2(originBlock.c3.x, splitPlace-roadWidth/2));
			Block block2 = new Block (new Vector2(originBlock.c1.x, splitPlace+roadWidth/2), originBlock.c3);
			return new Block[2]{block1,block2};
		}
	}

	private void fillBlock(Block b) {
		for (int i=1; i<=1; i++) {
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.SetParent (transform);
			float buildingHeight = Random.Range (0.5f, 2f);
			Vector2 center = (b.c3 + b.c1) / 2 + 0.1f* Random.insideUnitCircle;
			cube.transform.position = new Vector3 (center.x, buildingHeight/2, center.y);	
			cube.transform.localScale = new Vector3 (b.c3.x - b.c1.x, buildingHeight, b.c3.y - b.c1.y);	
		};
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
