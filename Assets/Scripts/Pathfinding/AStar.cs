using UnityEngine;
using System.Collections.Generic;

public static class AStar {

    private static MapTile[,] _mapTiles;

	private static List<MapTile> _closedList;
	private static List<MapTile> _openList;
	private static List<Vector3> _finalPath;

	private static MapTile _startTile;
	private static MapTile _endTile;
	private static MapTile _currentTile;

	public static List<Vector3> findABPath(Vector3 startPos, Vector3 endPos){
        //instantiate new versions of the static variables
        _mapTiles = new MapTile[GenerateMap.Map.mapWidth,GenerateMap.Map.mapHeight];
        System.Array.Copy(GenerateMap.Map.mapTiles, 0, _mapTiles, 0, GenerateMap.Map.mapTiles.Length); 
	    _closedList = new List<MapTile>();
	    _openList = new List<MapTile>();
	    _finalPath = new List<Vector3>();

	
		_startTile = _mapTiles[Mathf.FloorToInt(startPos.x),Mathf.FloorToInt(startPos.y)];
		_endTile = _mapTiles[Mathf.FloorToInt(endPos.x),Mathf.FloorToInt(endPos.y)];
		_currentTile = _startTile;
		_startTile.G = 0;
		List<MapTile> adjTiles = new List<MapTile>();

		addToOpenList(_startTile);

		while(!_closedList.Contains(_endTile)){
			_currentTile = _openList[0];
			Debug.Log (_currentTile.X + " " + _currentTile.Y);
			_closedList.Add(_currentTile);
			_openList.Remove(_currentTile);
			adjTiles = FindAdjacentTiles(_currentTile);
			foreach(MapTile tile in adjTiles){
				if(tile._tileType != MapTile.TileType.red || _closedList.Contains(tile)){

				}
				else if(!_openList.Contains(tile)){
					addToOpenList(tile);
				}
				else if(_openList.Contains(tile)){
					if(calculateG(tile) < tile.G){
						tile.Parent = _currentTile;
						calculateScores(tile);
					}
				}
				else{
					Debug.Log("something fucked up you dumbass.... steven... that is harsh....");
				}
				_openList.Sort(CompareTilesFScore);
			}
		}

		_currentTile = _endTile;
		while(_currentTile != _startTile){
            _finalPath.Insert(0,new Vector3(_currentTile.X,_currentTile.Y));
			_currentTile = _currentTile.Parent;
		}

		return _finalPath;
	}

	private static void addToOpenList(MapTile tile){
		setParent(tile);
		calculateScores(tile);
		Debug.Log("Tile at " + tile.X + "," + tile.Y + " added, scores of GHF " + tile.G + " " + tile.H + " " + tile.F);

		_openList.Add(tile);

		Debug.Log ("openList is now " + _openList.Count);

	}

	private static void setParent(MapTile tile){
		tile.Parent = _currentTile;
	}

	private static void calculateScores(MapTile tile){
		tile.G = calculateG(tile);
		tile.H = CalculateH(tile);
		tile.F = CalculateF(tile);
	}

	private static int calculateG(MapTile tile){
		int i = 0;
		if(tile.IsDiagonalTo(_currentTile))
			i = 14;
		if(tile.IsOrthogonalTo(_currentTile))
			i = 10;

		return _currentTile.G + i;
	}

	private static int CalculateH(MapTile tile){
		return (Mathf.Abs(_endTile.X - tile.X) + Mathf.Abs(_endTile.Y - tile.Y)) * 10;
	}

	private static int CalculateF(MapTile tile){
		return tile.G + tile.H;
	}

	private static List<MapTile> FindAdjacentTiles(MapTile center){
		List<MapTile> adjacentTiles = new List<MapTile>();
		adjacentTiles.Add(_mapTiles[center.X + 1, center.Y]);
		adjacentTiles.Add(_mapTiles[center.X + 1, center.Y + 1]);
        adjacentTiles.Add(_mapTiles[center.X + 1, center.Y - 1]);
        adjacentTiles.Add(_mapTiles[center.X - 1, center.Y]);
		adjacentTiles.Add(_mapTiles[center.X - 1, center.Y + 1]);
		adjacentTiles.Add(_mapTiles[center.X - 1, center.Y - 1]);
		adjacentTiles.Add(_mapTiles[center.X, center.Y - 1]);
        adjacentTiles.Add(_mapTiles[center.X, center.Y + 1]);
		
		return adjacentTiles;
	}

	private static int CompareTilesFScore(MapTile tile1, MapTile tile2){
		if(tile1.F < tile2.F)
			return -1;
		else if(tile1.F > tile2.F)
			return 1;
		else if(tile1.F == tile2.F)
			return 0;
		return 0;
	}
}
