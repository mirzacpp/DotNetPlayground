namespace Simplicity.Commons.Threading
{
	/// <summary>
	/// This class is used to ensure running of a code block only once.
	/// It can be instantiated as a static object to ensure that the code block runs only once in the application lifetime.
	/// Source https://github.com/abpframework/abp/blob/420ddc360d7520c904a7490f7c820d8591b2899b/framework/src/Volo.Abp.Core/Volo/Abp/Threading/OneTimeRunner.cs
	/// </summary>
	public class OneTimeRunner
	{
		private volatile bool _hasRunBefore;

		public void Run(Action action)
		{
			if (_hasRunBefore)
			{
				return;
			}

			lock (this)
			{
				if (_hasRunBefore)
				{
					return;
				}

				action();

				_hasRunBefore = true;
			}
		}
	}
}