# Mapbox Unity SDK Overlay Imagery Examples

It's a demo project showing how to use a second set of imagery on top of the base as an overlay in Mapbox Unity SDK.

I created two demos, one using a Mapbox map (`Streets` by default) and second using a third party api (I used AerisWeather api as example url).    
Only change has done to Mapbox Unity SDK code is in `UnityTile.cs` file;
```
//MeshRenderer.sharedMaterial.mainTexture = _rasterData;
MeshRenderer.sharedMaterial.SetTexture("_MainTexture", _rasterData);
```

**Scenes**    

![mapbox demo scene](https://i.imgur.com/9gQNwAW.jpg)    

For the first scene, all you need is a Mapbox access token. Scene will load satellite imagery as base image and streets as 50% transparent overlay. `Streets` is hardcoded in script, opacity is set on shader/material.

![aerisweather demo scene](https://i.imgur.com/GxYMrQE.jpg)    

For the second scene, you'll obviously need a third party api link for tile imagery. I used AerisWeather for this, their map builder creates a url which you can copy&paste into the script directly.
