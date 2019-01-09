using System;
using Environment;
using Managers;
using UnityEngine;

namespace PlayerBehaviour.Movement
{
    public enum PlayerDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    
    public class PlayerMovement : MonoBehaviour
    {
        [NonSerialized] public float Speed;
        private Transform mTransform;
        private Room target;
        private Room nextTarget;
        private Room currentPosition;
        private PlayerDirection direction;
        
        public Room CurrentPosition
        {
            get { return currentPosition; }
        }        
        
        public bool MovementActive
        {
            get; set;
        }
        
        
        public Action<Vector2> EnterTheRoom;
        public Action EnterTheNewRoom;
        
        private void Awake()
        {
            InitMovement(WorldGrid.Instance.StartRoom);
        }

        private void InitMovement(Room room)
        {
            MovementActive = true;
            mTransform = transform;
            currentPosition = room;
            target = room;
            nextTarget = room;
            MovementTheRoom();
            target.FullShowRoom();
        }

        public void TeleportTo(Room room)
        {
            InitMovement(room);
            mTransform.position = room.WorldPosition;
        }

        void Update()
        {
            if (MovementActive)
            {
                if (target != null && (Vector2) mTransform.position != target.WorldPosition)
                {
                    MovementTo(target.WorldPosition);
                }
#if UNITY_EDITOR || UNITY_WEBG || UNITY_WEBGL || UNITY_WEBGL_API
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    nextTarget = WorldGrid.Instance.CheckMovementRoom(target, 0, 1);
                    direction = PlayerDirection.Up;
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    nextTarget = WorldGrid.Instance.CheckMovementRoom(target, 0, -1);
                    direction = PlayerDirection.Down;
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    nextTarget = WorldGrid.Instance.CheckMovementRoom(target, 1, 0);
                    direction = PlayerDirection.Right;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    nextTarget = WorldGrid.Instance.CheckMovementRoom(target, -1, 0);
                    direction = PlayerDirection.Left;
                }
#else
                if (SwipeManager.IsSwipingUp())
                {
                    nextTarget = WorldGrid.Instance.CheckMovementRoom(currentPosition, 0, 1);
                    direction = PlayerDirection.Up;
                }
                
                if (SwipeManager.IsSwipingDown())
                {
                    nextTarget = WorldGrid.Instance.CheckMovementRoom(currentPosition, 0, -1);
                    direction = PlayerDirection.Down;
                }
        
                if (SwipeManager.IsSwipingRight())
                {
                    nextTarget = WorldGrid.Instance.CheckMovementRoom(currentPosition, 1, 0);
                    direction = PlayerDirection.Right;
                }
                
                if (SwipeManager.IsSwipingLeft())
                {
                    nextTarget = WorldGrid.Instance.CheckMovementRoom(currentPosition, -1, 0);
                    direction = PlayerDirection.Left;
                }
#endif
                if (nextTarget != null && nextTarget != target && (Vector2) mTransform.position == target.WorldPosition)
                {
                   if(currentPosition.Edges[(int) direction])
                        target = nextTarget;
                }
            }
        }

        private void MovementTo(Vector2 position)
        {
            mTransform.position = Vector2.MoveTowards(mTransform.position, position, Speed * Time.deltaTime);

            if (target.CheckEnterTheCell(mTransform.position) && currentPosition != target)
            {
                MovementTheRoom();
            }
        }

        private void MovementTheRoom()
        {
            WorldGrid.Instance.ShowNeighbors(target.GridX, target.GridY);
            target.FullShowRoom();
                
            currentPosition = target;
                
            if(EnterTheRoom!=null)
                EnterTheRoom(currentPosition.WorldPosition);
                
            target.PlayerEnterTheRoom();

            if (EnterTheNewRoom != null && target.IsNewRoom)
            {
                EnterTheNewRoom();
            }

            if(currentPosition.RoomType == RoomType.End) GameManager.Instance.ExitFromDungeon();
        }
    }
}
