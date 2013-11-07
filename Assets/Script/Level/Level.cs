using UnityEngine;
using System.Collections;

[RequireComponent(typeof(iTweenPath))]
public class Level : MonoBehaviour
{
	[HideInInspector]
	public string Path;
	public Texture2D Background;
	public Texture2D Cover;
}
