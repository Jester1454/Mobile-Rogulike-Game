namespace Utils
{
	public abstract class AbstractSingleton<T> where T : AbstractSingleton<T>, new()
	{
        private static readonly T instance = new T();
        public static T Instance
		{
			get
			{
				return instance;
			}
		}
	}
}
