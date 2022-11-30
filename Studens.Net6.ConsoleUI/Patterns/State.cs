using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studens.Net6.ConsoleUI.Patterns
{
	internal static partial class Patterns
	{

		public static class State
		{
			public class Context
			{
				private AppState _state;

				public Context(AppState state)
				{
					SwitchTo(state);
				}

				public void SwitchTo(AppState state)
				{
					Console.WriteLine($"Switching state from {_state} to {state}");
					_state = state;
					_state.SetContext(this);
				}

				// The Context delegates part of its behavior to the current State
				// object.
				public void Request1()
				{
					this._state.Handle1();
				}

				public void Request2()
				{
					this._state.Handle2();
				}
			}

			public abstract class AppState
			{
				protected Context _context;

				public void SetContext(Context context)
				{
					_context = context;
				}

				public abstract void Handle1();

				public abstract void Handle2();
			}

			// Concrete States implement various behaviors, associated with a state of
			// the Context.
			public class ConcreteStateA : AppState
			{
				public override void Handle1()
				{
					Console.WriteLine("ConcreteStateA handles request1.");
					Console.WriteLine("ConcreteStateA wants to change the state of the context.");
					this._context.SwitchTo(new ConcreteStateB());
				}

				public override void Handle2()
				{
					Console.WriteLine("ConcreteStateA handles request2.");
				}
			}

			public class ConcreteStateB : AppState
			{
				public override void Handle1()
				{
					Console.Write("ConcreteStateB handles request1.");
				}

				public override void Handle2()
				{
					Console.WriteLine("ConcreteStateB handles request2.");
					Console.WriteLine("ConcreteStateB wants to change the state of the context.");
					this._context.SwitchTo(new ConcreteStateA());
				}
			}
		}
	}
}
