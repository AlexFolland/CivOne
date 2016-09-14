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
using CivOne.Units;

namespace CivOne.Tasks
{
	internal class Orders : GameTask
	{
		private City _city;
		private Player _player;
		private Settlers _settlers = null;
		private int _x, _y;
		private bool _newCity = false;

		/*
		IUnit unit = GetUnits(x, y).FirstOrDefault();
			if (discardSettlers && ActiveUnit != null && ActiveUnit.GetType() != typeof(Settlers))
			{
				unit = ActiveUnit;
			}
			else if (discardSettlers && (ActiveUnit == null || ActiveUnit.GetType() != typeof(Settlers)))
			{
				return;
			}
			
			if (unit == null) return;

			if (cityName == null)
			{
				if (discardSettlers && GetCity(x, y) != null)
				{
					if (GetCity(x, y).Size > 9)
					{
						// TODO Show message?
						return;
					}
					GetCity(x, y).Size++;
					DisbandUnit(Game.Instance.ActiveUnit);
					return;
				}

				Player player = _players[unit.Owner];
				ICivilization civilization = player.Civilization;
				int index = GetCityIndex(civilization);
				_cityNameUsed[index] = true;

				cityName = _cityNames[index];
				if (unit.Owner == PlayerNumber(HumanPlayer))
				{
					Common.AddScreen(new CityName(x, y, cityName, discardSettlers));
				}
				else
				{
					FoundCity(x, y, cityName);
				}
				return;
			}

			City city = new City()
			{
				X = (byte)x,
				Y = (byte)y,
				Owner = unit.Owner,
				Name = cityName,
				Size = 1
			};
			if (!_cities.Any(c => c.Owner == unit.Owner))
				city.AddBuilding(new Palace());
			_cities.Add(city);
			if (unit.Owner == PlayerNumber(HumanPlayer))
			{
				Common.AddScreen(new CityView(city, founded: true));
			}
			DisbandUnit(Game.Instance.ActiveUnit);
			
			if (!discardSettlers) return;
			DisbandUnit(unit);
		*/
		private void CityManagerClosed(object sender, EventArgs args)
		{
			if (_settlers != null)
			{
				Game.Instance.DisbandUnit(_settlers);
			}
			EndTask();
		}

		private void CityViewed(object sender, EventArgs args)
		{
			CityManager cityManager = new CityManager(_city);
			cityManager.Closed += CityManagerClosed;
			Common.AddScreen(cityManager);
		}

		private void CityFounded(object sender, EventArgs args)
		{
			CityView cityView = new CityView(_city, firstView: true);
			cityView.Closed += CityViewed;
			Common.AddScreen(cityView);
		}

		private void CityNameAccept(object sender, EventArgs args)
		{
			string name = (sender as CityName).Value;
			CreateCity(name);
			EndTask();
		}

		private void Cancel(object sender, EventArgs args)
		{
			EndTask();
		}

		private void CreateCity(string name)
		{
			_city = Game.Instance.AddCity(_player, name, _x, _y); 
			if (_city != null)
			{
				if (_player.Human)
				{
					CityView cityView = new CityView(_city, founded: true);
					cityView.Closed += CityFounded;
					cityView.Skipped += CityViewed;
					Common.AddScreen(cityView);
					return;
				}
				if (_settlers != null)
				{
					Game.Instance.DisbandUnit(_settlers);
				}
			}
			EndTask();
		}

		private void CreateCity(Player player, int x, int y)
		{
			string name = Game.Instance.CityName(player);
			if (player.Human)
			{
				CityName cityName = new CityName(name);
				cityName.Accept += CityNameAccept;
				cityName.Cancel += Cancel;
				Common.AddScreen(cityName);
				return;
			}
			
			CreateCity(name);
		}

		public override void Run()
		{
			//public void FoundCity(int x, int y, string cityName = null, bool discardSettlers = true)
			if (_newCity)
			{
				if (_settlers != null)
				{
					_player = _settlers.Player;
					_x = _settlers.X;
					_y = _settlers.Y;
				}

				if (Map.Instance[_x, _y].City != null)
				{
					// There is already a city here, abort!
					EndTask();
					return;
				}

				CreateCity(_player, _x, _y);
				return;
			}
		}

		public static Orders NewCity(Settlers settlers = null)
		{
			return new Orders()
			{
				_settlers = settlers,
				_newCity = true
			};
		}

		public static Orders NewCity(Player player, int x, int y)
		{
			return new Orders()
			{
				_player = player,
				_newCity = true,
				_x = x,
				_y = y
			};
		}

		private Orders()
		{
			
		}
	}
}