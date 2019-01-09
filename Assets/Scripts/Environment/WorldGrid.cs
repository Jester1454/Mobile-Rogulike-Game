using System;
using System.Collections.Generic;
using DungeonsGeneration;
using ItemsSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Environment
{
	public class WorldGrid : MonoBehaviour
	{
		[NonSerialized] public static WorldGrid Instance;

		[NonSerialized] public int RandomSeed;
		[NonSerialized] public Vector2 SizeWorld = Vector2.zero;
		private int sizeGridX;
		private int sizeGridY;
		public float CellDiametr;

		[NonSerialized] public int NumbersOfRoom;
		private Room[,] rooms;
		private bool worldIsFinish = false;
		public float DoorSize = 0.1f;
		
		private Room startRoom = null;
		private Room endRoom = null;
		
		public Room StartRoom
		{
			get { return startRoom; }
		}
		
		public Room EndRoom
		{
			get { return startRoom; }
		}

		private void Awake()
		{
			if (Instance == null) 
			{ 
				Instance = this; 
			} 
			else if(Instance == this)
			{
				Destroy(gameObject);
			}
		}

		public void GenearateRooms(int randomSeed, int countRoom, Action onFinish = null)
		{
			RandomSeed = randomSeed;
			sizeGridX = countRoom/2;
			sizeGridY = countRoom/2;
			NumbersOfRoom = countRoom;

			Random.InitState(RandomSeed);
			
			worldIsFinish = false;

			DungeonGenerator generator = new DungeonGenerator(sizeGridX, sizeGridY, NumbersOfRoom);

			rooms = null;
			
			rooms = new Room[sizeGridX, sizeGridY];

			rooms =  generator.Generate();

			ItemGenerator itemGenerator = new ItemGenerator();
			for (int x = 0; x < sizeGridX; x++)
			{
				for (int y = 0; y < sizeGridY; y++)
				{
					if (rooms[x, y] != null)
					{
						switch (rooms[x, y].RoomType)
						{
							case RoomType.Start:
							{
								rooms[x, y].ShowRoom(CellDiametr, null);
								startRoom = rooms[x, y];
								break;
							}
							case RoomType.End:
							{
								rooms[x, y].ShowRoom(CellDiametr, null);
								endRoom = rooms[x, y];
								break;
							}
							default:
							{
								rooms[x, y].ShowRoom(CellDiametr,itemGenerator.GetRandomObject(rooms[x, y].RoomType));
								break;
							}
						}
					}
					else
					{
						rooms[x, y] = new Room(CellDiametr, x, y, false, false, false, false, RoomType.Empty);
					}
				}
			}

			SizeWorld = rooms[sizeGridX - 1, sizeGridY - 1].WorldPosition;
//			if(Math.Abs(SizeWorld.x % 10) < Mathf.Epsilon)
//				SizeWorld += Vector2.one * CellDiametr;
			
			worldIsFinish = true;
			
			if (onFinish != null)
				onFinish();			
		}

		public Room CheckMovementRoom(Room currentCell, int offsetX, int offsetY)
		{
			int newX = offsetX + currentCell.GridX;
			int newY = offsetY + currentCell.GridY;
			if (newX > sizeGridX-1 || newY > sizeGridY-1 || newX <0 || newY < 0)
			{
				return null;
			}
			else
			{
				if (rooms[newX, newY] != null)
				{
					return rooms[newX, newY];
				}
			}

			return null;
		}

		public Room GetRoom(Vector2 position)
		{
			for (int x = 0; x < sizeGridX; x++)
			{
				for (int y = 0; y < sizeGridY; y++)
				{
					if (rooms[x, y] != null)
					{
						if (rooms[x, y].CheckEnterTheCell(position))
						{
							return rooms[x, y];
						}
					}
				}
			}

			return null;
		}

		private List<Room> GetNeighbors(int x, int y, int deep = 1)
		{
			List<Room> neighbors = new List<Room>();

			for (int i = 1; i < deep+1; i++)
			{
				if (CheckCoordinates(x + i, y) && rooms[x + i, y] != null)
				{
					neighbors.Add(rooms[x + i, y]);
				}

				if (CheckCoordinates(x - i, y) && rooms[x - i, y] != null)
				{
					neighbors.Add(rooms[x - i, y]);
				}

				if (CheckCoordinates(x, y + i) && rooms[x, y + i] != null)
				{
					neighbors.Add(rooms[x, y + i]);
				}

				if (CheckCoordinates(x, y - i) && rooms[x, y - i] != null)
				{
					neighbors.Add(rooms[x, y - i]);
				}
			}

			return neighbors;
		}
		
		public void ShowNeighbors(int x, int y)
		{
			List<Room> neighbors = GetNeighbors(x, y);
			foreach (var neighbor in neighbors)
			{
				neighbor.HalfShowRoom();
			}
		}

		private bool CheckCoordinates(int x, int y)
		{
			if (x < 0 || y < 0 || x > sizeGridX-1 || y > sizeGridY-1)
			{
				return false;
			}

			return true;
		}

		private bool RoomIsDeadEnd(Room room)
		{
			int countNeighbors = 0;
			
			foreach (var edge in room.Edges)
			{
				if (edge)
					countNeighbors++;
				if (countNeighbors > 1)
					return false;
			}
						
			return true;
		}

		public Enemy.AbstractEnemy FindEnemy(int x, int y, int deep = 1)
		{
			List<Room> neighbors = GetNeighbors(x, y);

			foreach (var neighbor in neighbors)
			{
				if (!neighbor.IsEmpty)
				{
					if (!neighbor.IsEmpty)
					{
						if (neighbor.AbstractItem.ItemType == ItemType.Enemy)
							return (Enemy.AbstractEnemy) neighbor.AbstractItem;
					}
				}
			}

			return null;
		}
	}
}
