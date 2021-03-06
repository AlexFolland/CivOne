// CivOne
//
// To the extent possible under law, the person who associated CC0 with
// CivOne has waived all copyright and related or neighboring rights
// to CivOne.
//
// You should have received a copy of the CC0 legalcode along with this
// work. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.

using System;
using System.Linq;
using CivOne.Interfaces;
using CivOne.Screens;
using CivOne.Screens.Dialogs;

namespace CivOne.Tasks
{
	internal class Show : GameTask
	{
		private readonly IScreen _screen;

		public void Closed(object sender, EventArgs args)
		{
			EndTask();
		}

		public override void Run()
		{
			_screen.Closed += Closed;
			Common.AddScreen(_screen);
		}

		public static Show Empty
		{
			get
			{
				return new Show(Overlay.Empty);
			}
		}

		public static Show InterfaceHelp
		{
			get
			{
				return new Show(Overlay.InterfaceHelp);
			}
		}

		public static Show Terrain
		{
			get
			{
				GamePlay gamePlay = (GamePlay)Common.Screens.First(s => (s is GamePlay));
				return new Show(Overlay.TerrainView(gamePlay.X, gamePlay.Y));
			}
		}

		public static Show Goto
		{
			get
			{
				GamePlay gamePlay = (GamePlay)Common.Screens.First(s => (s is GamePlay));
				return new Show(new Goto(gamePlay.X, gamePlay.Y));
			}
		}

		public static Show TaxRate
		{
			get
			{
				return new Show(SetRate.Taxes);
			}
		}

		public static Show LuxuryRate
		{
			get
			{
				return new Show(SetRate.Luxuries);
			}
		}

		public static Show AutoSave
		{
			get
			{
				if (Game.GameTurn % 50 != 0) return null;
				int gameId = ((Game.GameTurn / 50) % 6) + 4;
				return new Show(new SaveGame(gameId));
			}
		}

		public static Show CityManager(City city)
		{
			return new Show(new CityManager(city));
		}

		public static Show UnitStack(int x, int y)
		{
			return new Show(new UnitStack(x, y));
		}

		public static Show Search
		{
			get
			{
				Search search = new Search();
				search.Accept += (s, a) =>
				{
					City city = (s as Search).City;
					if (city == null) return;
					GamePlay gamePlay = (GamePlay)Common.Screens.First(x => x.GetType() == typeof(GamePlay));
					gamePlay.CenterOnPoint(city.X, city.Y);
				};
				return new Show(search);
			}
		}

		public static Show ChooseGovernment
		{
			get
			{
				ChooseGovernment chooseGovernment = new ChooseGovernment();
				chooseGovernment.Closed += (s, a) => {
					Human.Government = (s as ChooseGovernment).Result;
					GameTask.Insert(Message.NewGoverment(null, $"{Human.TribeName} government", $"changed to {Human.Government.Name}!"));
				};
				return new Show(chooseGovernment);
			}
		}

		public static Show DestroyUnit(IUnit unit, bool stack)
		{
			return new Show(new DestroyUnit(unit, stack));
		}

		public static Show CaptureCity(City city)
		{
			return new Show(CityView.Capture(city));
		}

		public static Show Screen<T>() where T : IScreen, new()
		{
			return new Show(new T());
		}

		private Show(IScreen screen)
		{
			_screen = screen;
		}
	}
}