using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pathFinding : MonoBehaviour 
{
	Grid grid;
	public Transform seeker;
	public Transform target;

	void Awake()
	{
		grid = GetComponent<Grid>();
	}

	void Update()
	{
		findPath(seeker.position, target.position);
	}

	void findPath(Vector3 startPos, Vector3 targetPos)
	{
		node startNode = grid.nodeFromWorldPoint(startPos);
		node targetNode = grid.nodeFromWorldPoint(targetPos);

		List<node> openSet = new List<node>();
		HashSet<node> closedSet = new HashSet<node>();

		//add our start node to our open set
		openSet.Add(startNode);

		while(openSet.Count > 0)
		{
			node currentNode = openSet[0];
			for(int i = 1; i < openSet.Count; i++)
			{
				if(openSet[i].fCost < currentNode.fCost 
					|| openSet[i].fCost == currentNode.fCost 
					&& openSet[i].hCost < currentNode.hCost)
				{
					currentNode = openSet[i];
				}
			}

			openSet.Remove(currentNode);
			closedSet.Add(currentNode);

			if(currentNode == targetNode)
			{
				retracePath(startNode, targetNode);
				return;
			}

			foreach(node neighbor in grid.getNeighbors(currentNode))
			{
				if(!neighbor.walkable || closedSet.Contains(neighbor))
				{
					continue;
				}

				int newMovementCostToNeighbor = currentNode.gCost + getDistance(currentNode,neighbor);

				if(newMovementCostToNeighbor < neighbor.gCost || !(openSet.Contains(neighbor)))
				{
					neighbor.gCost = newMovementCostToNeighbor;
					neighbor.hCost = getDistance(neighbor,targetNode);

					neighbor.parent = currentNode;

					if(!(openSet.Contains(neighbor)))
					{
						openSet.Add(neighbor);
					}
				}
			}
		}
	}

	void retracePath(node startNode, node endNode)
	{
		List<node> path = new List<node>();
		node currentNode = endNode;

		while(currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

		path.Reverse();

		grid.path = path;
	}


	int getDistance(node nodeA, node nodeB)
	{
		int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if(distX > distY)
		{
			return 14 * distY + 10 * (distX - distY);
		}
		else
		{
			return 14 * distX + 10 * (distY - distX);
		}

	}

}
