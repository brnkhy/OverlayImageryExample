using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Map;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.MeshGeneration.Enums;
using Mapbox.Unity.Utilities;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.Networking;

public class AerisOverlayImagery : MonoBehaviour
{
	public AbstractMap Map;

	private const string OverlayImageTextureName = "_OverlayImage";
	private static readonly int OverlayImage = Shader.PropertyToID(OverlayImageTextureName);

	//This script won't work without a proper url
	//I marked where key goes in sample url below but I highly suggest not using that and creating a full url using AerisWeather online map creation tools
	[NonSerialized] private string _url = "https://maps.aerisapi.com/---KEY GOES HERE---/fsatellite/{0}/{1}/{2}/current.png";

	void Awake()
	{
		if (Map == null)
		{
			Map = FindObjectOfType<AbstractMap>();
		}
		Map.OnTileFinished += LoadCloudMaps;
	}

	private void LoadCloudMaps(UnityTile tile)
	{
		Runnable.Run(DownloadImage(tile));
	}

	private IEnumerator DownloadImage(UnityTile tile)
	{
		var fullUrl = string.Format(_url, tile.UnwrappedTileId.Z, tile.UnwrappedTileId.X, tile.UnwrappedTileId.Y);

		using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(fullUrl))
		{
			yield return uwr.SendWebRequest();

			if (uwr.isNetworkError || uwr.isHttpError)
			{
				Debug.Log(uwr.error);
			}
			else
			{
				var texture = DownloadHandlerTexture.GetContent(uwr);
				texture.wrapMode = TextureWrapMode.Clamp;
				tile.MeshRenderer.material.SetTexture(OverlayImage, texture);
			}
		}
	}
}