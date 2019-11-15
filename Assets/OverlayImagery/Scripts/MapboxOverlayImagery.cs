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

public class MapboxOverlayImagery : MonoBehaviour
{
	public AbstractMap Map;
	public string TilesetId;

	private ImageDataFetcher _imageDataFetcher;
	private const string OverlayImageTextureName = "_OverlayImage";
	private static readonly int OverlayImage = Shader.PropertyToID(OverlayImageTextureName);


	void Awake()
	{
		if (Map == null)
		{
			Map = FindObjectOfType<AbstractMap>();
		}
		Map.OnTileFinished += LoadOverlayImage;
		_imageDataFetcher = ScriptableObject.CreateInstance<ImageDataFetcher>();
		_imageDataFetcher.DataRecieved += DataReceived;
		_imageDataFetcher.FetchingError += FetchingError;
	}

	private void DataReceived(UnityTile unityTile, RasterTile tile)
	{
		var rasterData = new Texture2D(0, 0, TextureFormat.RGB24, false)
		{
			wrapMode = TextureWrapMode.Clamp
		};

		rasterData.LoadImage(tile.Data);
		rasterData.Compress(false);

		unityTile.MeshRenderer.material.SetTexture(OverlayImage, rasterData);
	}

	private void FetchingError(UnityTile unityTile, RasterTile tile, TileErrorEventArgs errors)
	{
		foreach (var exp in errors.Exceptions)
		{
			Debug.Log(exp.ToString());
		}
	}

	private void LoadOverlayImage(UnityTile tile)
	{
		_imageDataFetcher.FetchData(new ImageDataFetcherParameters()
		{
			canonicalTileId = tile.CanonicalTileId,
			tile = tile,
			tilesetId = TilesetId,
			useRetina = true
		});
	}
}