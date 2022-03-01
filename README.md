# Arkitektum.Cesium.Ion.RestApiSharp
C#-library for easier interaction with Cesium Ion Assets, by wrapping the Ion REST API: https://cesium.com/learn/ion/rest-api/

## Table of contents
* [Examples](#examples)

## Examples

### Get all assets
```C#
var accessToken = "myAccessToken";

var listOfAssetMetadata = await new CesiumIonClient(acessToken).GetAssetListAsync();
```
### Get specific asset
```C#
var accessToken = "myAccessToken";
var assetId = 1;

var cesiumWorldTerrainAssetMetadata = await new CesiumIonClient(acessToken).GetAssetAsync(assetId);
```
### Delete asset
```C#
var accessToken = "myAccessToken";
var assetId = <myAssetId>;

var httpResponseMessage = await new CesiumIonClient(acessToken).DeleteAssetAsync(assetId);
```
### Upload asset
```C#
var accessToken = "myAccessToken";

var assetName = "myAsset";
var myAsset = AssetFactory.Create(assetName, AssetType.TERRAIN, SourceType.RASTER_TERRAIN);

var assetFilePath = @"C:/Full/file/path/to/asset.extension";
using var myAssetStream = new FileStream(assetFilePath, FileMode.Open, FileAccess.Read);

var myAssetId = await new CesiumIonClient(acessToken).UploadAssetAsync(myAsset, myAssetStream);
```
