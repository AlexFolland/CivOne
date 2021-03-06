// CivOne
//
// To the extent possible under law, the person who associated CC0 with
// CivOne has waived all copyright and related or neighboring rights
// to CivOne.
//
// You should have received a copy of the CC0 legalcode along with this
// work. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.

using CivOne.Leaders;
using CivOne.Templates;

namespace CivOne.Civilizations
{
	internal class Egyptian : BaseCivilization<Ramesses>
	{
		public Egyptian() : base(4, 4, "Egyptian", "Egyptians")
		{
			StartX = 41;
			StartY = 24;
			CityNames = new string[]
			{
				"Thebes",
				"Memphis",
				"Oryx",
				"Heliopolis",
				"Gaza",
				"Alexandria",
				"Byblos",
				"Cairo",
				"Coptos",
				"Edfu",
				"Pithom",
				"Busirus",
				"Athribus",
				"Mendes",
				"Tanis",
				"Abydos"
			};
		}
	}
}