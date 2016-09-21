using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour 
{
	public Transform player;

	public Vector2 gridWorldSize;
	public float nodeRadius;
	public LayerMask unwalkableMask;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	node[,] grid;
	public List<node> path;

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,1,gridWorldSize.y));

		if(grid!=null)
		{
			foreach(node n in grid)
			{
				Gizmos.color = (n.walkable)?Color.white:Color.red;

				if(path != null)
				{
					if(path.Contains(n))
					{
						Gizmos.color = Color.blue;
					}
				}

				Gizmos.DrawCube(n.worldPosition,Vector3.one * (nodeDiameter - .1f));
			}
		}
	}

	void DrawPath()
	{

		Material m = new Material(Shader.Find("Standard"));

		if(grid!=null)
		{
			foreach(node n in grid)
			{
				m.color = (n.walkable)?Color.white:Color.red;

				if(path != null)
				{
					if(path.Contains(n))
					{
						m.color = Color.blue;
					}

				}

				if(n.cube != null)
				{
					n.cube.GetComponent<MeshRenderer>().material.color = m.color;
				}
			}
		}
	}

	void updateWalkable()
	{
		foreach(node n in grid)
		{
			n.walkable = !(Physics.CheckSphere(n.worldPosition,nodeRadius, unwalkableMask));
		}
	}

	void DrawNodes()
	{
		Material m = new Material(Shader.Find("Standard"));
		GameObject cube;

		if(grid!=null)
		{
			foreach(node n in grid)
			{
				cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.position = n.worldPosition;
				cube.transform.localScale = (Vector3.one * (nodeDiameter - .1f));
				n.cube = cube;
			}
		}
	}

	// Use this for initialization
	void Start () 
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		createGrid();
		DrawNodes();
	}
	
	// Update is called once per frame
	void Update () 
	{
		DrawPath();
		updateWalkable();
	}

	public node nodeFromWorldPoint(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;

		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

		return grid[x,y];
	}

	public void createGrid()
	{
		grid = new node[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

		for(int x = 0; x < gridSizeX; x++)
		{
			for(int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius, unwalkableMask));
				grid[x,y] = new node(walkable, worldPoint, x, y);
			}
		}
	}

	public List<node> getNeighbors(node node)
	{
		List<node> neighbors = new List<node>();

		for(int x = -1; x <= 1; x++)
		{
			for(int y = -1; y <= 1; y++)
			{
				if(x == 0 && y == 0)
				{
					//skip
					continue;
				}

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if((checkX >= 0 && checkX < gridSizeX) && (checkY >= 0 && checkY < gridSizeY))
				{
					neighbors.Add(grid[checkX,checkY]);
				}
			}
		}
		return neighbors;
	}
}
