using System;
using System.IO;
using Arkitektum.Cesium.Ion.RestApiSharp.Services;
using Arkitektum.Cesium.Ion.RestApiSharp.Util;
using Xunit;

namespace Arkitektum.Cesium.Ion.RestApiSharp.Tests;

public class ClientTest
{
    private const string AccessToken = @"yourTokenHere";

    [Fact]
    public void ShouldGetAssets()
    {
        var assets = new CesiumIonClient(AccessToken).GetAssetListAsync().Result;

        Assert.NotNull(assets);
        Assert.NotEmpty(assets);
    }

    [Fact]
    public void ShouldGetSpecificAsset()
    {
        var asset = new CesiumIonClient(AccessToken).GetAssetAsync(1).Result;

        Assert.NotNull(asset);
    }

    [Fact, Trait("Category", "Integration")]
    public void ShouldUploadTerrainAsset()
    {
        var assetName = "testAsset";
        var assetDescription = "Terrain data";
        var asset = AssetFactory.Create(assetName, AssetType.TERRAIN, SourceType.RASTER_TERRAIN,
            description: assetDescription);

        var client = new CesiumIonClient(AccessToken);

        const string fileName = @"C:\Users\LeifHalvorSunde\source\repos\DiBK.Plankart\DiBK.Plankart.Application\Resources\test.tiff";
        using var assetStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

        var newAssetId = client.UploadAssetAsync(asset, assetStream).Result;

        Assert.True(newAssetId.HasValue);

        var completedAsset = client.GetAssetAsync(newAssetId.Value).Result;

        Assert.NotNull(completedAsset);
        Assert.Equal(newAssetId, completedAsset.Id);
        Assert.Equal(assetName, completedAsset.Name);
        Assert.Equal(assetDescription, completedAsset.Description);
        Assert.Equal(string.Empty, completedAsset.Attribution);
        Assert.Equal(AssetType.TERRAIN, completedAsset.Type);

        var deleteResponse = client.DeleteAssetAsync(newAssetId.Value).Result;

        Assert.NotNull(deleteResponse);

        Assert.ThrowsAsync<AggregateException>(() => client.GetAssetAsync(newAssetId.Value));

        Assert.Equal(AssetStatus.COMPLETE, completedAsset.Status);
        Assert.Equal(100, completedAsset.PercentComplete);
    }
}