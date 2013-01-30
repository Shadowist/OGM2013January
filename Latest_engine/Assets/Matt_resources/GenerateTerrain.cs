using System;
using UnityEngine;
using Object = UnityEngine.Object;


public class GenerateTerrain : MonoBehaviour
{
	public int mapWidth = 10;
	public int mapHeight = 10;
	public int octaves = 4;

	
	public Transform blockPrefab;
	public Transform detailPrefab;
	
	Texture2D heightMap;
	int[][] blockHeight;
	
	public Material material;
	
	void Start()
	{
		heightMap = new Texture2D(mapWidth, mapHeight,TextureFormat.RGB24, false);
		
		blockHeight = GetEmptyArray(mapWidth, mapHeight);
		
		GenerateHeightMap();
		GenerateBlocks();
	}

	public void GenerateHeightMap()
	{
		float[][] temp;
		PerlinNoise noise = new PerlinNoise();
		temp = noise.GeneratePerlinNoise(noise.GenerateWhiteNoise(mapWidth, mapHeight), octaves);
		
		for(int i = 0; i < mapWidth; i++)
		{
			for(int j = 0; j < mapHeight; j++)
			{
				double v = temp[i][j];
				Color pixelColor = new Color ((float)v,(float)v,(float)v,1.0f);
				heightMap.SetPixel(i,j,pixelColor);
			}
		}
		heightMap.Apply();
	}
	
	public void GenerateBlocks()
	{	
		float height = 0;
		int placedBlocks = 0;
			
		for(int i = 0; i < mapWidth; i++)
		{
			for(int j = 0; j < mapHeight; j++)
			{
				height = (Mathf.Round(heightMap.GetPixel(i, j).grayscale*10));
				Debug.Log(height);
				if((height >= 5) && (height < 8)){
					height = 1;	
					Vector3 location = new Vector3(i, height, j);	
					Instantiate(blockPrefab,location, Quaternion.identity);
					blockHeight[i][j] = (int)height;
					placedBlocks++;
				}
				if(height >= 8){
					height = 1;
					Vector3 location = new Vector3(i, height, j);	
					Instantiate(blockPrefab,location, Quaternion.identity);
					Instantiate(detailPrefab,(location + new Vector3(0,1,0)), Quaternion.identity);
					blockHeight[i][j] = (int)height;
					placedBlocks++;
				}
				if(height < 5){
					height = 0;
				}
			}
		}
		
		if(placedBlocks < ((mapWidth*mapHeight)/2))
		{
			Application.LoadLevel(0);
		}
	}
	
	public int[][] GetEmptyArray(int width, int height)
	{
		int[][] returnArray = new int[width][];
		for(int i = 0; i < returnArray.Length; i++)
		{
			returnArray[i] = new int[height];	
		}
		return returnArray;
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.F1)){
			Application.LoadLevel(0);	
		}	
	}
}


