using System;
using ItemsSystem;
using View.Enemy;

namespace Environment
{
	public interface IItemBehaviour : ICreated
	{
		void Show(Room room);
		void PlayerEnterTheRoom(Room room);
	}
}
