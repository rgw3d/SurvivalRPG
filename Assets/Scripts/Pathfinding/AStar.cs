﻿using UnityEngine;
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
			Debug.Log (_currentTile.x + " " + _currentTile.y);
			_closedList.Add(_currentTile);
			_openList.Remove(_currentTile);
			adjTiles = findAdjacentTiles(_currentTile);
			foreach(MapTile tile in adjTiles){
				if(tile.tileType != MapTile.TileType.red || _closedList.Contains(tile)){

				}
				else if(!_openList.Contains(tile)){
					addToOpenList(tile);
				}
				else if(_openList.Contains(tile)){
					if(calculateG(tile) < tile.G){
						tile.parent = _currentTile;
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
            _finalPath.Insert(0,new Vector3(_currentTile.x,_currentTile.y));
			_currentTile = _currentTile.parent;
		}

		return _finalPath;
	}

	private static void addToOpenList(MapTile tile){
		setParent(tile);
		calculateScores(tile);
		Debug.Log("Tile at " + tile.x + "," + tile.y + " added, scores of GHF " + tile.G + " " + tile.H + " " + tile.F);

		_openList.Add(tile);

		Debug.Log ("openList is now " + _openList.Count);

	}

	private static void setParent(MapTile tile){
		tile.parent = _currentTile;
	}

	private static void calculateScores(MapTile tile){
		tile.G = calculateG(tile);
		tile.H = calculateH(tile);
		tile.F = calculateF(tile);
	}

	private static int calculateG(MapTile tile){
		int i = 0;
		if(tile.isDiagonalTo(_currentTile))
			i = 14;
		if(tile.isOrthogonalTo(_currentTile))
			i = 10;

		return _currentTile.G + i;
	}

	private static int calculateH(MapTile tile){
		return (Mathf.Abs(_endTile.x - tile.x) + Mathf.Abs(_endTile.y - tile.y)) * 10;
	}

	private static int calculateF(MapTile tile){
		return tile.G + tile.H;
	}

	private static List<MapTile> findAdjacentTiles(MapTile center){
		List<MapTile> tiles = new List<MapTile>();
		tiles.Add(_mapTiles[center.x + 1, center.y]);
		tiles.Add(_mapTiles[center.x + 1, center.y + 1]);
		tiles.Add(_mapTiles[center.x, center.y + 1]);
		tiles.Add(_mapTiles[center.x - 1, center.y + 1]);
		tiles.Add(_mapTiles[center.x - 1, center.y]);
		tiles.Add(_mapTiles[center.x - 1, center.y - 1]);
		tiles.Add(_mapTiles[center.x, center.y - 1]);
		tiles.Add(_mapTiles[center.x + 1, center.y - 1]);
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
