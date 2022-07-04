using System.Net.Http.Headers;
using Arkitektum.Cesium.Ion.RestApiSharp.Models;
using Arkitektum.Cesium.Ion.RestApiSharp.Services;
using Arkitektum.Cesium.Ion.RestApiSharp.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Arkitektum.Cesium.Ion.RestApiSharp;

// https://github.com/CesiumGS/cesium-ion-rest-api-examples/blob/main/tutorials/rest-api/index.js
public class CesiumIonClient : IDisposable
{
    private readonly HttpClient _client;
    private const string CesiumIonAssetsUrl = "https://api.cesium.com/v1/assets/";

    public CesiumIonClient(string accessToken)
    {
        _client = new HttpClient
        {
            DefaultRequestHeaders = { Authorization = AuthenticationHeaderValue.Parse($"Bearer {accessToken}") }
        };
    }

    public async Task<List<AssetMetadata>?> GetAssetListAsync()
    {
        var jsonAssets = await _client.GetStringAsync(CesiumIonAssetsUrl);

        return JsonConvert.DeserializeObject<AssetList>(jsonAssets)?.Items;
    }

    public async Task<AssetMetadata?> GetAssetAsync(int id)
    {
        var jsonAsset = await _client.GetStringAsync(CesiumIonAssetsUrl + id);

        return JsonConvert.DeserializeObject<AssetMetadata>(jsonAsset);
    }

    public async Task<HttpResponseMessage> DeleteAssetAsync(int id)
    {
        return await _client.DeleteAsync(CesiumIonAssetsUrl + id);
    }

    public async Task<int?> UploadAssetAsync(Asset asset, FileStream assetStream)
    {
        var response = await PrepareUploadAsync(asset);

        if (response == null)
            return null;

        using var amazonS3Client = AssetUploadHelper.CreateAmazonS3Client(response.UploadLocation);

        var uploadPartRequest = AssetUploadHelper.CreateUploadRequest(response.UploadLocation, assetStream);

        var uploadPartResponse = await amazonS3Client.UploadPartAsync(uploadPartRequest);

        if (uploadPartResponse == null)
            return response.AssetMetadata.Id;

        using var updateUploadContent = AssetUploadHelper.CreateUpdateUploadContent(response.OnComplete);

        using var uploadResult = await _client.PostAsync(response.OnComplete.Url, updateUploadContent);

        return response.AssetMetadata.Id;
    }

    private async Task<Response?> PrepareUploadAsync(Asset asset)
    {
        using var uploadRequestContent = AssetUploadHelper.CreateUploadRequestContent(asset);

        using var responseMessage = await _client.PostAsync(CesiumIonAssetsUrl, uploadRequestContent);

        var jsonResponse = await responseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

        var response = JsonConvert.DeserializeObject<Response>(jsonResponse, new StringEnumConverter());

        return response;
    }

    public int GetUploadPercentProgress(int assetId)
    {
        var asset = GetAssetAsync(assetId).Result;

        if (asset == null)
            return 0;

        return asset.PercentComplete;
    }

    public async Task<bool> AssetIsCorrupt(int assetId)
    {
        var asset = await GetAssetAsync(assetId);

        return asset == null || asset.IsCorrupt();
    }

    public async Task<bool> AssetIsComplete(int assetId)
    {
        var asset = await GetAssetAsync(assetId);

        return asset != null && asset.IsComplete();
    }

    private bool UploadIsReady(int assetId, out string message, out bool error)
    {
        error = false;

        var asset = GetAssetAsync(assetId).Result;

        if (asset == null)
        {
            message = $"Could not get asset with id: {assetId}";
            error = true;
            return false;
        }

        switch (asset.Status)
        {
            case AssetStatus.AWAITING_FILES:
                message = "Awaiting files...";
                return false;
            case AssetStatus.COMPLETE:
                message = "Asset uploaded successfully\n" +
                          $"View in ion: https://cesium.com/ion/assets/{assetId}";
                return true;
            case AssetStatus.DATA_ERROR:
                message = "Ion detected a problem with the uploaded data";
                error = true;
                return false;
            case AssetStatus.ERROR:
                message = "An unknown tiling error occurred, please contact support@cesium.com.";
                error = true;
                return false;
            case AssetStatus.NOT_STARTED:
                message = "Tiling pipeline initializing.";
                return false;
            case AssetStatus.IN_PROGRESS:
                message = $"Asset is {asset.PercentComplete}% complete";
                return false;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}