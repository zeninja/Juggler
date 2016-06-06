using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public static class Achievements {
	[DllImport("__Internal")]
	private static extern void _ReportAchievement( string achievementID, float progress );
}
