// CivOne
//
// To the extent possible under law, the person who associated CC0 with
// CivOne has waived all copyright and related or neighboring rights
// to CivOne.
//
// You should have received a copy of the CC0 legalcode along with this
// work. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.

using System.Drawing;
using CivOne.Enums;
using CivOne.Interfaces;

namespace CivOne.GFX
{
	internal static class TileResources
	{
		private static Resources Res
		{
			get
			{
				return Resources.Instance;
			}
		}
		
		private static void DrawOceanBorderNorth(ref Picture output, ITile tile)
		{
			if (tile.GetBorderType(Direction.North) == Terrain.Ocean) return;
			if (tile.GetBorderType(Direction.West) == Terrain.Ocean)
			{
				if (tile.GetBorderType(Direction.NorthWest) == Terrain.Ocean)
					output.AddLayer(Res.GetPart("TER257", 64, 176, 8, 8), 0, 0);
				else
					output.AddLayer(Res.GetPart("TER257", 96, 176, 8, 8), 0, 0);
			}
			else
			{
				output.AddLayer(Res.GetPart("TER257", 80, 176, 8, 8), 0, 0);
			}
			if (tile.GetBorderType(Direction.East) == Terrain.Ocean)
			{
				if (tile.GetBorderType(Direction.NorthEast) == Terrain.Ocean)
					output.AddLayer(Res.GetPart("TER257", 24, 176, 8, 8), 8, 0);
				else
					output.AddLayer(Res.GetPart("TER257", 56, 176, 8, 8), 8, 0);
			}
			else
			{
				output.AddLayer(Res.GetPart("TER257", 88, 176, 8, 8), 8, 0);
			}
		}
		
		private static void DrawOceanBorderEast(ref Picture output, ITile tile)
		{
			if (tile.GetBorderType(Direction.East) == Terrain.Ocean) return;
			if (tile.GetBorderType(Direction.North) == Terrain.Ocean)
			{
				if (tile.GetBorderType(Direction.NorthEast) == Terrain.Ocean)
					output.AddLayer(Res.GetPart("TER257", 72, 176, 8, 8), 8, 0);
				else
					output.AddLayer(Res.GetPart("TER257", 104, 176, 8, 8), 8, 0);
			}
			else
			{
				output.AddLayer(Res.GetPart("TER257", 88, 176, 8, 8), 8, 0);
			}
			if (tile.GetBorderType(Direction.South) == Terrain.Ocean)
			{
				if (tile.GetBorderType(Direction.SouthEast) == Terrain.Ocean)
					output.AddLayer(Res.GetPart("TER257", 24, 184, 8, 8), 8, 8);
				else
					output.AddLayer(Res.GetPart("TER257", 56, 184, 8, 8), 8, 8);
			}
			else
			{
				output.AddLayer(Res.GetPart("TER257", 88, 184, 8, 8), 8, 8);
			}
		}
		
		private static void DrawOceanBorderSouth(ref Picture output, ITile tile)
		{
			if (tile.GetBorderType(Direction.South) == Terrain.Ocean) return;
			if (tile.GetBorderType(Direction.West) == Terrain.Ocean)
			{
				if (tile.GetBorderType(Direction.SouthWest) == Terrain.Ocean)
					output.AddLayer(Res.GetPart("TER257", 16, 184, 8, 8), 0, 8);
				else
					output.AddLayer(Res.GetPart("TER257", 48, 184, 8, 8), 0, 8);
			}
			else
			{
				output.AddLayer(Res.GetPart("TER257", 80, 184, 8, 8), 0, 8);
			}
			if (tile.GetBorderType(Direction.East) == Terrain.Ocean)
			{
				if (tile.GetBorderType(Direction.SouthEast) == Terrain.Ocean)
					output.AddLayer(Res.GetPart("TER257", 72, 184, 8, 8), 8, 8);
				else
					output.AddLayer(Res.GetPart("TER257", 104, 184, 8, 8), 8, 8);
			}
			else
			{
				output.AddLayer(Res.GetPart("TER257", 88, 184, 8, 8), 8, 8);
			}
		}
		
		private static void DrawOceanBorderWest(ref Picture output, ITile tile)
		{
			if (tile.GetBorderType(Direction.West) == Terrain.Ocean) return;
			if (tile.GetBorderType(Direction.North) == Terrain.Ocean)
			{
				if (tile.GetBorderType(Direction.NorthWest) == Terrain.Ocean)
					output.AddLayer(Res.GetPart("TER257", 16, 176, 8, 8), 0, 0);
				else
					output.AddLayer(Res.GetPart("TER257", 48, 176, 8, 8), 0, 0);
			}
			else
			{
				output.AddLayer(Res.GetPart("TER257", 80, 176, 8, 8), 0, 0);
			}
			if (tile.GetBorderType(Direction.South) == Terrain.Ocean)
			{
				if (tile.GetBorderType(Direction.SouthWest) == Terrain.Ocean)
					output.AddLayer(Res.GetPart("TER257", 64, 184, 8, 8), 0, 8);
				else
					output.AddLayer(Res.GetPart("TER257", 96, 184, 8, 8), 0, 8);
			}
			else
			{
				output.AddLayer(Res.GetPart("TER257", 80, 184, 8, 8), 0, 8);
			}
		}
		
		private static void DrawDiagonalCoast(ref Picture output, ITile tile)
		{
			bool north = (tile.GetBorderType(Direction.North) == Terrain.Ocean);
			bool east = (tile.GetBorderType(Direction.East) == Terrain.Ocean);
			bool south = (tile.GetBorderType(Direction.South) == Terrain.Ocean);
			bool west = (tile.GetBorderType(Direction.West) == Terrain.Ocean);
			
			if (north && west && (tile.GetBorderType(Direction.NorthWest) != Terrain.Ocean)) output.AddLayer(Res.GetPart("TER257", 32, 176, 8, 8), 0, 0);
			if (north && east && (tile.GetBorderType(Direction.NorthEast) != Terrain.Ocean)) output.AddLayer(Res.GetPart("TER257", 40, 176, 8, 8), 8, 0);
			if (south && west && (tile.GetBorderType(Direction.SouthWest) != Terrain.Ocean)) output.AddLayer(Res.GetPart("TER257", 32, 184, 8, 8), 0, 8);
			if (south && east && (tile.GetBorderType(Direction.SouthEast) != Terrain.Ocean)) output.AddLayer(Res.GetPart("TER257", 40, 184, 8, 8), 8, 8);
		}
		
		internal static Bitmap GetTile16(ITile tile)
		{
			Picture output = new Picture(16, 16);
			
			bool altTile = ((tile.X + tile.Y) % 2 == 1);
			
			// Set tile base
			if (tile.Type != Terrain.Ocean)
			{
				output.AddLayer(Res.GetPart("SPRITES", 0, 80, 16, 16));
			}
			
			// Add tile terrain
			switch (tile.Type)
			{
				case Terrain.Ocean:
				case Terrain.River:
					bool ocean = (tile.Type == Terrain.Ocean);
					output.AddLayer(Res.GetPart("SPRITES", tile.Borders * 16, (ocean ? 64 : 80), 16, 16));
					break;
				default:
					int terrainId = (int)tile.Type;
					if (tile.Type == Terrain.Grassland1) altTile = false;
					else if (tile.Type == Terrain.Grassland2) { altTile = true; terrainId = (int)Terrain.Grassland1; }
					output.AddLayer(Res.GetPart("SPRITES", terrainId * 16, (altTile ? 0 : 16), 16, 16));
					break;
			}
			
			// Add special resources
			if (tile.Special)
			{
				int terrainId = (int)tile.Type;
				Bitmap resource = Res.GetPart("SPRITES", terrainId * 16, 112, 16, 16);
				Picture.ReplaceColours(resource, 3, 0);
				output.AddLayer(resource);
			}
			
			return output.Image;
		}
		
		internal static Bitmap GetTile256(ITile tile)
		{
			Picture output = new Picture(16, 16);
			
			// Set tile base
			switch (tile.Type)
			{
				case Terrain.Ocean: output.AddLayer(Res.GetPart("TER257", 0, 160, 16, 16)); break;
				default: output.AddLayer(Res.GetPart("SP257", 0, 64, 16, 16)); break;
			}
			
			// Add tile terrain
			switch (tile.Type)
			{
				case Terrain.Ocean:
					DrawOceanBorderNorth(ref output, tile);
					DrawOceanBorderEast(ref output, tile);
					DrawOceanBorderSouth(ref output, tile);
					DrawOceanBorderWest(ref output, tile);
					DrawDiagonalCoast(ref output, tile);
					
					if (tile.GetBorderType(Direction.North) == Terrain.River) output.AddLayer(Res.GetPart("TER257", 128, 176, 16, 16));
					if (tile.GetBorderType(Direction.East) == Terrain.River) output.AddLayer(Res.GetPart("TER257", 144, 176, 16, 16));
					if (tile.GetBorderType(Direction.South) == Terrain.River) output.AddLayer(Res.GetPart("TER257", 160, 176, 16, 16));
					if (tile.GetBorderType(Direction.West) == Terrain.River) output.AddLayer(Res.GetPart("TER257", 176, 176, 16, 16));
					break;
				case Terrain.River:
					output.AddLayer(Res.GetPart("SP257", tile.Borders * 16, 80, 16, 16));
					break;
				default:
					int terrainId = (int)tile.Type;
					if (tile.Type == Terrain.Grassland2) { terrainId = (int)Terrain.Grassland1; }
					output.AddLayer(Res.GetPart("TER257", tile.Borders * 16, terrainId * 16, 16, 16));
					break;
			}
			
			// Add special resources
			if (tile.Special)
			{
				int terrainId = (int)tile.Type;
				Bitmap resource = Res.GetPart("SP257", terrainId * 16, 112, 16, 16);
				Picture.ReplaceColours(resource, 3, 0);
				output.AddLayer(resource);
			}
			else if (tile.Type == Terrain.Grassland2)
			{
				Bitmap resource = Res.GetPart("SP257", 152, 40, 8, 8);
				Picture.ReplaceColours(resource, 3, 0);
				output.AddLayer(resource, 4, 4);
			}
			
			return output.Image;
		}
	}
}