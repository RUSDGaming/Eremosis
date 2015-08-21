using UnityEngine;
using System.Collections;


public class GameLogicUtils : MonoBehaviour
{
	


	//http://www.redblobgames.com/grids/hexagons/#range
	public static bool canHeroMoveToTile (HeroInfo heroInfo, TileInfo tileInfo)
	{
			
		if (heroInfo == null || tileInfo == null) {
			Debug.Log ("something was null");
			return false;
		}
		
		if (tileInfo.UnitOnTile != null) {
			Debug.Log ("something was on the tile");
			return false;
		}
		
		TileInfo heroTile = heroInfo.tile;
		
		int dX = tileInfo.x - heroTile.x;
		int dY = tileInfo.y - heroTile.y;
		int dZ = tileInfo.z - heroTile.z;
		
		float d = Mathf.Abs (dX) + Mathf.Abs (dY) + Mathf.Abs (dZ);
		
		d = d / 2;
		
		if (d <= heroInfo.move) {
			return true;
		}
		
		return false;
	}
	
	//http://www.redblobgames.com/grids/hexagons/#range
	public static bool canHeroAttackUnit (HeroInfo attacker, HeroInfo deffender)
	{
		
		if (deffender == null || attacker == null) {
			Debug.Log ("something was null");
			return false;
		}
		
		TileInfo attackerTile = attacker.tile;
		TileInfo deffenderTile = deffender.tile;
		
		int dX = attackerTile.x - deffenderTile.x;
		int dY = attackerTile.y - deffenderTile.y;
		int dZ = attackerTile.z - deffenderTile.z;
		
		float d = Mathf.Abs (dX) + Mathf.Abs (dY) + Mathf.Abs (dZ);
		
		d = d / 2;
		
		if (d <= attacker.range) {
			return true;
		}
		
		return false;
	}
}
