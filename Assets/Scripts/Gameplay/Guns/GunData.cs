using System;
using UnityEngine;

[Serializable]
public struct GunData
{
	[Header("Constant Values")]
	public LayerMask targetLayers;
	public float reloadDuration;
	[Tooltip("Shots per second")]  public float fireRate;
	public int damage;
	public float range;
	public int maxBackupAmmo;
	public int maxAmmo;
	[Header("Current Values")]
	public float reloadTimer;
	public float fireTimer;
	public int backupAmmo;
	public int ammo;
}