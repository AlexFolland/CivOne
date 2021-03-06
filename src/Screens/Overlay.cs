// CivOne
//
// To the extent possible under law, the person who associated CC0 with
// CivOne has waived all copyright and related or neighboring rights
// to CivOne.
//
// You should have received a copy of the CC0 legalcode along with this
// work. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.

using System.Collections.Generic;
using System.Linq;
using CivOne.Enums;
using CivOne.Events;
using CivOne.Interfaces;
using CivOne.GFX;
using CivOne.Templates;

namespace CivOne.Screens
{
	internal class Overlay : BaseScreen
	{
		private struct HelpLabel
		{
			public int X, Y;
			public int PointX, PointY;
			public string Text;
			public HelpLabel(string text, int x, int y, int pointX, int pointY)
			{
				Text = text;
				X = x;
				Y = y;
				PointX = pointX;
				PointY = pointY;
			}
		}

		private bool _update = true;
		private bool _interfaceHelp = false;
		private bool _showTerrain = false;

		private int _x, _y;

		private bool _closing = false;

		private IEnumerable<HelpLabel> HelpLabels
		{
			get
			{
				IUnit startUnit = Game.GetUnits().First(x => Game.Human == x.Owner);
				IUnit activeUnit = Game.ActiveUnit;
				((GamePlay)Common.Screens.First(x => x is GamePlay)).HasUpdate(0);
				int offset = 0;
				if (Settings.RightSideBar)
				{
					yield return new HelpLabel("Map Window", 148, 24, 272, 32);
					yield return new HelpLabel("Menu Bar", 61, 16, 160, 6);
					yield return new HelpLabel("Active Unit", 158, 170, 280, 128);
					offset = -80;
				}
				else
				{
					yield return new HelpLabel("Map Window", 88, 24, 48, 32);
					yield return new HelpLabel("Menu Bar", 201, 16, 160, 6);
					yield return new HelpLabel("Active Unit", 88, 170, 40, 128);
				}
				
				for (int yy = -1; yy <= 1; yy++)
				for (int xx = -1; xx <= 1; xx++)
				{
					if (xx == 0 && yy == 0) continue;
					string text = string.Empty;
					ITile tile = startUnit.Tile[xx, yy];
					switch (tile.Type)
					{
						case Terrain.Desert: text = (tile.Special ? "Oasis" : "Desert"); break;
						case Terrain.Plains: text = (tile.Special ? "Horses" : "Plains"); break;
						case Terrain.Forest: text = (tile.Special ? "Game" : "Desert"); break;
						case Terrain.Hills: text = (tile.Special ? "Coal" : "Hills"); break;
						case Terrain.Mountains: text = (tile.Special ? "Gold" : "Mountains"); break;
						case Terrain.Tundra: text = (tile.Special ? "Game" : "Tundra"); break;
						case Terrain.Arctic: text = (tile.Special ? "Seals" : "Arctic"); break;
						case Terrain.Swamp: text = (tile.Special ? "Oil" : "Swamp"); break;
						case Terrain.Jungle: text = (tile.Special ? "Gems" : "Jungle"); break;
						case Terrain.Ocean: text = (tile.Special ? "Fish" : "Ocean"); break;
						case Terrain.River: text = "River"; break;
						case Terrain.Grassland1:
						case Terrain.Grassland2: text = "Grassland"; break;
					}
					if (tile.Hut) text = "Village";
					yield return new HelpLabel(text, 170 + (65 * xx) + offset, 100 + (49 * yy), 200 + (xx * 16) + offset, 112 + (yy * 16));
				}
			}
		}
		
		public override bool HasUpdate(uint gameTick)
		{
			if (_closing)
			{
				if (!HandleScreenFadeOut()) Destroy();
				return true;
			}

			if (_update)
			{
				if (_interfaceHelp)
				{
					Picture background = Resources.Instance.GetPart("SP299", 288, 120, 32, 16);
					foreach (HelpLabel helpLabel in HelpLabels)
					{
						Size textSize = Resources.Instance.GetTextSize(0, helpLabel.Text);

						Picture label = new Picture(textSize.Width + 11, textSize.Height + 9);
						label.FillLayerTile(background);
						label.AddBorder(15, 8, 1, 1, label.Width - 2, label.Height - 2);
						label.AddBorder(5, 5, 0, 0, label.Width, label.Height);
						label.DrawText(helpLabel.Text, 0, 15, 5, 5);

						_canvas.AddLine(15, helpLabel.PointX, helpLabel.PointY, helpLabel.X + 5, helpLabel.Y + 6);
						AddLayer(label, helpLabel.X, helpLabel.Y);
					}
				}

				if (_showTerrain)
				{
					int cx = Settings.RightSideBar ? 0 : 80;
					int cy = 8;

					for (int yy = 0; yy < 12; yy++)
					for (int xx = 0; xx < 15; xx++)
					{
						ITile tile = Map[_x + xx, _y + yy];
						if (tile == null || !Human.Visible(tile))
						{
							_canvas.FillRectangle(5, cx + (xx * 16), cy + (yy * 16), 16, 16);
							continue;
						}
						AddLayer(Resources.Instance.GetTile(tile, improvements: false), cx + (xx * 16), cy + (yy * 16));
						
						if (!Human.Visible(tile, Direction.West)) AddLayer(Resources.Instance.GetFog(Direction.West), cx + (xx * 16), cy + (yy * 16));
						if (!Human.Visible(tile, Direction.North)) AddLayer(Resources.Instance.GetFog(Direction.North), cx + (xx * 16), cy + (yy * 16));
						if (!Human.Visible(tile, Direction.East)) AddLayer(Resources.Instance.GetFog(Direction.East), cx + (xx * 16), cy + (yy * 16));
						if (!Human.Visible(tile, Direction.South)) AddLayer(Resources.Instance.GetFog(Direction.South), cx + (xx * 16), cy + (yy * 16));
					}
				}

				_update = false;
				return true;
			}
			return false;
		}
		
		public override bool KeyDown(KeyboardEventArgs args)
		{
			if (_interfaceHelp)
			{
				_closing = true;
				return true;
			}
			Destroy();
			return true;
		}
		
		public override bool MouseDown(ScreenEventArgs args)
		{
			if (_interfaceHelp)
			{
				_closing = true;
				return true;
			}
			Destroy();
			return true;
		}

		public static Overlay Empty
		{
			get
			{
				return new Overlay();
			}
		}

		public static Overlay InterfaceHelp
		{
			get
			{
				return new Overlay()
				{
					_interfaceHelp = true
				};
			}
		}

		public static Overlay TerrainView(int x, int y)
		{
			return new Overlay()
			{
				_showTerrain = true,
				_x = x,
				_y = y
			};
		}

		private Overlay()
		{
			Cursor = MouseCursor.Pointer;
			
			_canvas = new Picture(320, 200, Common.TopScreen.Palette);
		}
	}
}