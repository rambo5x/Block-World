using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Block
{
	public int type;
	public bool vis;
	public Block(int t, bool v)
	{
		type = t;
		vis = v;
	}
}

public class GenerateLandscape : MonoBehaviour {

	public static int width = 128;
	public static int depth = 128;
	public static int height = 128;
	public int heightScale = 20;
	public float detailScale = 25.0f;

	public GameObject snowBlock;
	public GameObject grassBlock;
	public GameObject sandBlock;
	public GameObject dirtBlock;
	public GameObject stoneBlock;

	Block[,,] worldBlocks = new Block[width, height, depth];

	// Use this for initialization
	void Start ()
	{
        heightScale = (int)Random.Range(0f, 100f);
        detailScale = (int)Random.Range(0f, 100f);

        int seed = (int)Random.Range(0f, 99999f);
		for(int z = 0; z < depth; z++ )
		{
			for (int x = 0; x < width ; x++)
			{
				int y = (int)(Mathf.PerlinNoise((x+seed) / detailScale, (z+seed) / detailScale) * heightScale);
				Vector3 blockPos = new Vector3(x, y, z);

				CreateBlock(y, blockPos, true);
				while(y > 0)
				{
					y--;
					blockPos = new Vector3(x, y, z);
					CreateBlock(y, blockPos, false);
				}
			}
		}
	}

    private void FixedUpdate()
    {
        
    }

    public void GenerateNewTerrain()
    {
        Debug.Log("Is pressed");

      //  Destroy(gameObject);

        heightScale = (int)Random.Range(0f, 100f);
        detailScale = (int)Random.Range(0f, 100f);

        int seed = (int)Random.Range(0f, 99999f);
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int y = (int)(Mathf.PerlinNoise((x + seed) / detailScale, (z + seed) / detailScale) * heightScale);
                Vector3 blockPos = new Vector3(x, y, z);

                CreateBlock(y, blockPos, true);
                while (y > 0)
                {
                    y--;
                    blockPos = new Vector3(x, y, z);
                    CreateBlock(y, blockPos, false);
                }
            }
        }
    }

	void CreateBlock(int y, Vector3 blockPos, bool create)
	{
		if (y > 15)
		{
			if (create)
				Instantiate(snowBlock, blockPos, Quaternion.identity);
			worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(1, create);
		}
		else if (y > 10)
		{
			if (create)
				Instantiate(grassBlock, blockPos, Quaternion.identity);
			worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(2, create);
		}
		else if (y > 7)
		{
			if (create)
				Instantiate(sandBlock, blockPos, Quaternion.identity);
			worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(3, create);
		}
		else if (y > 5)
		{
			if (create)
				Instantiate(dirtBlock, blockPos, Quaternion.identity);
			worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(4, create);
		}
		else
		{
			if (create)
				Instantiate(stoneBlock, blockPos, Quaternion.identity);
			worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(5, create);
		}
	} 

	void DrawBlock(Vector3 blockPos)
	{
		if (blockPos.x < 0 || blockPos.x >= width || blockPos.y < 0 || blockPos.y >= height || blockPos.z < 0 || blockPos.z >= depth) return;
		if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null) return;
		if (!worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].vis)
		{

			worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].vis = true;
			if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type == 1)
				Instantiate(snowBlock, blockPos, Quaternion.identity);
			else if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type == 2)
				Instantiate(grassBlock, blockPos, Quaternion.identity);
			else if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type == 3)
				Instantiate(sandBlock, blockPos, Quaternion.identity);
			else if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type == 4)
				Instantiate(dirtBlock, blockPos, Quaternion.identity);
			else if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type == 5)
				Instantiate(stoneBlock, blockPos, Quaternion.identity);
			else
				worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].vis = false;
		}
	}


	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
			if (Physics.Raycast(ray, out hit, 10.0f))
			{
				Vector3 blockPos = hit.transform.position;
				//this is the bottom block. Dont delete it.
				if ((int)blockPos.y == 0) return;
				worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = null;

				Destroy(hit.transform.gameObject);

				for (int x = -1; x <= 1; x++)
					for (int y = -1; y <= 1; y++)
						for (int z = -1; z <= 1; z++)
						{
							if (!(x == 0 && y == 0 && z == 0))
							{
								Vector3 neighbour = new Vector3(blockPos.x + x, blockPos.y + y, blockPos.z + z);
								DrawBlock(neighbour);
							}
						}

			}
		}


	}
}﻿