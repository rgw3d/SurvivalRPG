using UnityEngine;
using System.Collections.Generic;

public static class AStar {

    private static MapTile[,] mapTiles;

	private static List<MapTile> closedList;
	private static List<MapTile> openList;
	private static List<Vector3> path;

	private static MapTile start;
	private static MapTile end;
	private static MapTile current;

	public static List<Vector3> findABPath(Vector3 startPos, Vector3 endPos){
        //instantiate new
        mapTiles = new MapTile[GenerateMap.Map.mapWidth,GenerateMap.Map.mapHeight];
        System.Array.Copy(GenerateMap.Map.mapTiles, 0, mapTiles, 0, GenerateMap.Map.mapTiles.Length); 
	    closedList = new List<MapTile>();
	    openList = new List<MapTile>();
	    path = new List<Vector3>();

	
		start = mapTiles[Mathf.FloorToInt(startPos.x),Mathf.FloorToInt(startPos.y)];
		end = mapTiles[Mathf.FloorToInt(endPos.x),Mathf.FloorToInt(endPos.y)];
		current = start;
		start.G = 0;
		List<MapTile> adjTiles = new List<MapTile>();

		addToOpenList(start);

		while(!closedList.Contains(end)){
			current = openList[0];
			Debug.Log (current.x + " " + current.y);
			closedList.Add(current);
			openList.Remove(current);
			adjTiles = findAdjacentTiles(current);
			foreach(MapTile tile in adjTiles){
				if(tile.tileType != MapTile.TileType.red || closedList.Contains(tile)){

				}
				else if(!openList.Contains(tile)){
					addToOpenList(tile);
				}
				else if(openList.Contains(tile)){
					if(calculateG(tile) < tile.G){
						tile.parent = current;
						calculateScores(tile);
					}
				}
				else{
					Debug.Log("something fucked up you dumbass");
				}
				openList.Sort(CompareTilesFScore);
			}
		}

		current = end;
		while(current != start){
			path.Insert(0,new Vector3(current.x,current.y));
			current = current.parent;
		}

		return path;
	}

	private static void addToOpenList(MapTile tile){
		setParent(tile);
		calculateScores(tile);
		Debug.Log("Tile at " + tile.x + "," + tile.y + " added, scores of GHF " + tile.G + " " + tile.H + " " + tile.F);

		openList.Add(tile);

		Debug.Log ("openList is now " + openList.Count);

	}

	private static void setParent(MapTile tile){
		tile.parent = current;
	}

	private static void calculateScores(MapTile tile){
		tile.G = calculateG(tile);
		tile.H = calculateH(tile);
		tile.F = calculateF(tile);
	}

	private static int calculateG(MapTile tile){
		int i = 0;
		if(tile.isDiagonalTo(current))
			i = 14;
		if(tile.isOrthogonalTo(current))
			i = 10;

		return current.G + i;
	}

	private static int calculateH(MapTile tile){
		return (Mathf.Abs(end.x - tile.x) + Mathf.Abs(end.y - tile.y)) * 10;
	}

	private static int calculateF(MapTile tile){
		return tile.G + tile.H;
	}

	private static List<MapTile> findAdjacentTiles(MapTile center){
		List<MapTile> tiles = new List<MapTile>();
		tiles.Add(mapTiles[center.x + 1, center.y]);
		tiles.Add(mapTiles[center.x + 1, center.y + 1]);
		tiles.Add(mapTiles[center.x, center.y + 1]);
		tiles.Add(mapTiles[center.x - 1, center.y + 1]);
		tiles.Add(mapTiles[center.x - 1, center.y]);
		tiles.Add(mapTiles[center.x - 1, center.y - 1]);
		tiles.Add(mapTiles[center.x, center.y - 1]);
		tiles.Add(mapTiles[center.x + 1, center.y - 1]);
		return tiles;
	}

	private static int CompareTilesFScore(MapTile tile1, MapTile tile2){
		if(tile1.F < tile2.F)
			return -1;
		else if(tile1.F > tile2.F)
			return 1;
		else if(tile1.F == tile2.F)
			return 0;
		else
			return 0;
	}
}
