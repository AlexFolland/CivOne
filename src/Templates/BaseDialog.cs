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
using CivOne.Events;
using CivOne.GFX;

namespace CivOne.Templates
{
	internal abstract class BaseDialog : BaseScreen
	{
		private readonly int _left, _top;

		private bool _update = true;
		
		protected Picture DialogBox { get; private set; }
		
		protected Picture[] TextLines { get; private set; }

		protected int TextWidth
		{
			get
			{
				return TextLines.Max(x => x.Width);
			}
		}

		protected int TextHeight
		{
			get
			{
				return TextLines.Sum(x => x.Height);
			}
		}
		
		protected Picture Selection(int left, int top, int width, int height)
		{
			Picture background = DialogBox.GetPart(left, top, width, height);
			Picture.ReplaceColours(background, new byte[] { 7, 22 }, new byte[] { 11, 3 });
			return background;
		}

		protected virtual void Cancel(object sender = null, EventArgs args = null)
		{
			Destroy();
		}

		protected virtual void FirstUpdate()
		{
			// Override this function to add menus and/or expand the dialog
		} 

		public override bool HasUpdate(uint gameTick)
		{
			if (_update)
			{
				_update = false;
				_canvas.AddLayer(DialogBox, _left, _top);

				FirstUpdate();

				return true;
			}
			return false;
		}
		
		public override bool KeyDown(KeyboardEventArgs args)
		{
			Cancel();
			return true;
		}
		
		public override bool MouseDown(ScreenEventArgs args)
		{
			Cancel();
			return true;
		}

		private void Initialize(int left, int top, int width, int height)
		{
			Cursor = MouseCursor.Pointer;

			Picture background = Resources.Instance.GetPart("SP299", 288, 120, 32, 16);
			
			_canvas = new Picture(320, 200, Common.GamePlay.Palette);

			// We expand the size to add space for the black border
			left -= 1;
			top -= 1;
			width += 2;
			height += 2;
			
			DialogBox = new Picture(width, height);
			DialogBox.FillLayerTile(background, 1, 1);
			DialogBox.AddBorder(15, 8, 1, 1, width - 2, height - 2);
			DialogBox.AddBorder(5, 5, 0, 0, width, height);
		}

		public BaseDialog(int left, int top, int marginWidth, int marginHeight, string[] message)
		{
			_left = left;
			_top = top;
			TextLines = new Picture[message.Length];
			for (int i = 0; i < message.Length; i++)
				TextLines[i] = Resources.Instance.GetText(message[i], 0, 15);

			Initialize(left, top, TextWidth + marginWidth, TextHeight + marginHeight);
		}
		
		public BaseDialog(int left, int top, int width, int height)
		{
			_left = left;
			_top = top;

			Initialize(left, top, width, height);
		}
	}
}