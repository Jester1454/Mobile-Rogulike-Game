using ItemsSystem;
using Managers;
using UnityEngine;
using View.Dungeons;

namespace Environment
{
    public enum RoomType
    {
        Default,
        Start,
        End,
        DeadEnd,
        Empty
    }
    
    public class Room 
    {
        public int GridX;
        public int GridY;
        public Vector2 WorldPosition;
        public RoomView View;
        private float roomRadius;

        private int playerEnterRoomCount = 0;
        
        public bool IsNewRoom
        {
            get { return playerEnterRoomCount==1; }
        }
        
        public bool IsEmpty
        {
            get
            {
                return AbstractItem == null || RoomType==RoomType.Empty;
            }
        }
        
        public bool IsFullVisible
        {
            get { return isFullVisible; }
        }

        private bool isFullVisible = false;
        private bool isHalfVisible = false;
        
        public RoomType RoomType;

        public bool[] Edges;

        private int edgesSize = 4;

        public AbstractItem AbstractItem = null;
        
        public Room(int x, int y, RoomType type = RoomType.Default, int edgesSize = 4)
        {
            GridX = x;
            GridY = y;
            this.edgesSize = edgesSize;
            Edges = new bool[4];
            RoomType = type;
        }

        public void ShowRoom(float sizeCell, AbstractItem objectAbstractItemBehaviour = null)
        {
            WorldPosition = new Vector2(GridX*sizeCell, GridY*sizeCell);
            roomRadius = sizeCell / 2;
            
            View = GameObject.Instantiate
            (
                PrefabsManager.LoadPrefab(PrefabsManager.PrefabsList.DefaultRoom)
            ).GetComponent<RoomView>();
			
            View.InitView(WorldPosition, this);

            if (objectAbstractItemBehaviour != null)
            {
                AbstractItem = objectAbstractItemBehaviour;
                objectAbstractItemBehaviour.Show(this);
            }
        }

        public void PlayerEnterTheRoom()
        {
            if(!IsEmpty)
                AbstractItem.PlayerEnterTheRoom(this);
            
            playerEnterRoomCount++;
        }

        public void PlaceItemInRoom(AbstractItem obj)
        {
            AbstractItem = obj;
        }

        public void RemoveItemFromRoom()
        {
            AbstractItem = null;
        }
        
        public Room(float sizeCell, int x, int y, bool topDoor, bool doorBot, bool doorLeft, bool doorRight, RoomType type = RoomType.Default)
        {
            GridX = x;
            GridY = y;
            
            Edges = new bool[4];

            Edges[0] = topDoor;
            Edges[1] = doorBot;
            Edges[2] = doorLeft;
            Edges[3] = doorRight;
            RoomType = type;

            WorldPosition = new Vector2(GridX*sizeCell, GridY*sizeCell);
            roomRadius = sizeCell / 2;
            
            GameObject.Instantiate
            (
                PrefabsManager.LoadPrefab(PrefabsManager.PrefabsList.EmptyRoom)
            ).transform.position = WorldPosition;
        }
        
        public void HalfShowRoom()
        {
            if (!isHalfVisible && RoomType != RoomType.Empty)
            {
                isHalfVisible = true;
                View.HalfShowRoom();
            }
        }

        public void FullShowRoom()
        {
            if (!isFullVisible && RoomType != RoomType.Empty)
            {
                isFullVisible = true;
                View.FullShowRoom();
            }
        }
		
        public bool CheckEnterTheCell(Vector2 position)
        {
            if (position.x < WorldPosition.x + roomRadius && 
                position.x > WorldPosition.x - roomRadius
                && position.y < WorldPosition.y + roomRadius &&
                position.y > WorldPosition.y - roomRadius)
                return true;
            return false;
        }
    }
}
