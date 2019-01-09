using System;
using System.Collections.Generic;
using Environment;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DungeonsGeneration
{
	public class DungeonGenerator
	{
		private const int MAX_ITERATION_SEARCH = 100;

		private Room[,] rooms;

		private List<Vector2> takenPositions = new List<Vector2>();

		private int numberOfRooms;

		private int gridSizeX, gridSizeY;

		public DungeonGenerator(int gridSizeX, int gridSizeY, int numberOfRooms)
		{
			this.numberOfRooms = numberOfRooms;
			this.gridSizeX = gridSizeX;
			this.gridSizeY = gridSizeY;
		}

		public Room[,] Generate()
		{
			takenPositions.Clear();

			CreateRooms();
			SetRoomDoors();

			return rooms;
		}

		private void CreateRooms()
		{
			rooms = new Room[gridSizeX, gridSizeY];

			rooms[gridSizeX/2, gridSizeY/2] = new Room(gridSizeX/2, gridSizeY/2, RoomType.Start);
			
			Vector2 start = new Vector2((int)gridSizeX/2, (int)gridSizeY/2);
			
			takenPositions.Add(start);
			Vector2 checkPos = start;

			float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;

			for (int i = 0; i < numberOfRooms - 1; i++)
			{
				float randomPerc = ((float) i / ((float) numberOfRooms - 1));

				randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);

				checkPos = NewPosition();

				if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
				{
					int iterations = 0;
					do
					{
						checkPos = SelectiveNewPosition();
						iterations++;

					} while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < MAX_ITERATION_SEARCH) ;
				}

				rooms[(int) checkPos.x, (int) checkPos.y] = new Room((int) checkPos.x, (int) checkPos.y);
				takenPositions.Insert(0, checkPos);
			}

		}

		private Vector2 NewPosition()
		{
			int x = 0, y = 0;

			Vector2 checkingPos = Vector2.zero;
			do
			{
				int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
				x = (int) takenPositions[index].x;
				y = (int) takenPositions[index].y;
				bool upDown = (Random.value < 0.5f);
				bool positive = (Random.value < 0.5f);

				if (upDown)
				{
					if (positive)
					{
						y += 1;
					}
					else
					{
						y -= 1;
					}
				}
				else
				{
					if (positive)
					{
						x += 1;
					}
					else
					{
						x -= 1;
					}
				}
				checkingPos = new Vector2(x, y);

			} while (takenPositions.Contains(checkingPos) || !PositionInGrid(x, y)); //делаем до тех пор, пока не найдем точку, которая находится в гриде и не содержится в занятых позициях

			return checkingPos;
		}

		private bool PositionInGrid(int x, int y)
		{
			if (x >= 1 && x <= gridSizeX-1 && y >= 1 && y <= gridSizeY-1)
				return true;
			return false;
		}
		
		private bool RoomInGrid(int x, int y)
		{
			if (x >= 0 && x <= gridSizeX && y >= 0 && y <= gridSizeY && rooms[x,y]!=null)
				return true;
			return false;
		}

		private Vector2 SelectiveNewPosition()
		{
			int index = 0, inc = 0;
			int x = 0, y = 0;

			Vector2 checkingPos = Vector2.zero;
			do
			{
				inc = 0;
				do
				{
					inc++;
					index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
				} while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1  && inc < MAX_ITERATION_SEARCH) ;

				x = (int) takenPositions[index].x;
				y = (int) takenPositions[index].y;

				bool upDown = (Random.value < 0.5f);
				bool positive = (Random.value < 0.5f);

				if (upDown)
				{
					if (positive)
					{
						y += 1;
					}
					else
					{
						y -= 1;
					}
				}
				else
				{
					if (positive)
					{
						x += 1;
					}
					else
					{
						x -= 1;
					}
				}
				checkingPos = new Vector2(x, y);

			} while (takenPositions.Contains(checkingPos) || !PositionInGrid(x, y));

			return checkingPos;
		}

		private int NumberOfNeighbors(Vector2 checkPos, List<Vector2> usedPostions)
		{
			int countNeighbors = 0;
			if (usedPostions.Contains(checkPos + Vector2.right))
			{
				countNeighbors++;
			}

			if (usedPostions.Contains(checkPos + Vector2.left))
			{
				countNeighbors++;
			}

			if (usedPostions.Contains(checkPos + Vector2.down))
			{
				countNeighbors++;
			}

			if (usedPostions.Contains(checkPos + Vector2.up))
			{
				countNeighbors++;
			}

			return countNeighbors;
		}

		private void SetRoomDoors()
		{	
			List<Room> deadEnds = new List<Room>();
			
			for(int x=0; x<gridSizeX; x++)
			{
				for (int y = 0; y < gridSizeY; y++)
				{
					if (rooms[x, y] == null)
					{
						continue;
					}

					if (!PositionInGrid(x, y+1)) //Up Edges
					{
						rooms[x,y].Edges[0] = false;
					}
					else
					{
						rooms[x,y].Edges[0] = (rooms[x, y + 1] != null);
					}

					if (!PositionInGrid(x,y - 1)) //Down Edges
					{
						rooms[x,y].Edges[1] = false;
					}
					else
					{
						rooms[x,y].Edges[1] = (rooms[x, y - 1] != null);
					}

					if (!PositionInGrid(x - 1,y)) //Left Edges
					{
						rooms[x,y].Edges[2] = false;
					}
					else
					{
						rooms[x,y].Edges[2] = (rooms[x - 1, y] != null);
					}

					if (!PositionInGrid(x + 1,y)) //Right Edges
					{
						rooms[x,y].Edges[3] = false;
					}
					else
					{
						rooms[x,y].Edges[3] = (rooms[x + 1, y] != null);
					}
					
					if(RoomIsDeadEnd(rooms[x,y]))
						deadEnds.Add(rooms[x,y]);
				}
			}

			Room minX = deadEnds[0], 
				minY = deadEnds[0], 
				maxX = deadEnds[0],
				maxY = deadEnds[0];
			
			foreach (var end in deadEnds)
			{
				if (maxX.GridX < end.GridX)
					maxX = end;
				if (maxY.GridY < end.GridY)
					maxY = end;

				if (minX.GridX > end.GridX)
					minX = end;;
				if (minY.GridY > end.GridY)
					minY = end;

				end.RoomType = RoomType.DeadEnd;
			}

			switch (Random.Range(0,4))
			{
				case 0:
					minX.RoomType = RoomType.End;
					break;				
				case 1:
					maxX.RoomType = RoomType.End;
					break;				
				case 2:
					maxY.RoomType = RoomType.End;
					break;				
				case 3:
					minY.RoomType = RoomType.End;
					break;
			}
		}
		
		private bool RoomIsDeadEnd(Room room)
		{
			if (room == null)
				return false;
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

	}
}
