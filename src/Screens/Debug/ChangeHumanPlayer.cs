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
using CivOne.Enums;
using CivOne.GFX;
using CivOne.Templates;

namespace CivOne.Screens.Debug
{
	internal class ChangeHumanPlayer : BaseScreen
	{
		private readonly Menu _civSelect;

		private Player _selectedPlayer = null;

		public string Value { get; private set; }

		public event EventHandler Accept, Cancel;

		private void ChangePlayer_Accept(object sender, EventArgs args)
		{
			_selectedPlayer = Game.GetPlayer((byte)_civSelect.ActiveItem);

			if (_selectedPlayer != Game.HumanPlayer)
			{
				Game.HumanPlayer = _selectedPlayer;
				Game.EndTurn();
			}

			if (Accept != null)
				Accept(this, null);
			Destroy();
		}

		private void ChangePlayer_Cancel(object sender, EventArgs args)
		{
			if (Cancel != null)
				Cancel(this, null);
			Destroy();
		}

		public override bool HasUpdate(uint gameTick)
		{
			if (_selectedPlayer == null && Common.TopScreen.GetType() != typeof(Menu))
			{
				AddMenu(_civSelect);
				return false;
			}
			return false;
		}

		public ChangeHumanPlayer()
		{
			Cursor = MouseCursor.Pointer;

			_canvas = new Picture(320, 200, Common.Screens.Last().Canvas.OriginalColours);

			int fontHeight = Resources.Instance.GetFontHeight(0);
			int hh = (fontHeight * (Game.Players.Count() + 1)) + 5;
			int ww = 108;

			int xx = (320 - ww) / 2;
			int yy = (200 - hh) / 2;

			Picture background = Resources.Instance.GetPart("SP299", 288, 120, 32, 16);
			Picture menuGfx = new Picture(ww, hh);
			menuGfx.FillLayerTile(background);
			menuGfx.AddBorder(15, 8, 0, 0, ww, hh);
			Picture menuBackground = menuGfx.GetPart(2, 11, ww - 4, hh - 11);
			Picture.ReplaceColours(menuBackground, new byte[] { 7, 22 }, new byte[] { 11, 3 });

			_canvas.FillRectangle(5, xx - 1, yy - 1, ww + 2, hh + 2);
			_canvas.AddLayer(menuGfx, xx, yy);
			_canvas.DrawText("Change Human Player...", 0, 15, xx + 8, yy + 3);

			_civSelect = new Menu(Canvas.Palette, menuBackground)
			{
				X = xx + 2,
				Y = yy + 11,
				Width = ww - 4,
				ActiveColour = 11,
				TextColour = 5,
				DisabledColour = 3,
				FontId = 0,
				Indent = 8
			};

			foreach (Player player in Game.Players)
			{
				_civSelect.Items.Add(new Menu.Item(player.TribeNamePlural));
				_civSelect.Items[_civSelect.Items.Count() - 1].Selected += ChangePlayer_Accept;
			}

			_civSelect.Cancel += ChangePlayer_Cancel;
			_civSelect.MissClick += ChangePlayer_Cancel;
			_civSelect.ActiveItem = Game.PlayerNumber(Human);
		}
	}
}