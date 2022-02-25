using System.Net.Http.Headers;
using Arkitektum.Cesium.Ion.RestApiSharp.Models;
using Arkitektum.Cesium.Ion.RestApiSharp.Services;
using Arkitektum.Cesium.Ion.RestApiSharp.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Arkitektum.Cesium.Ion.RestApiSharp;

// https://github.com/CesiumGS/cesium-ion-rest-api-examples/blob/main/tutorials/rest-api/index.js
public class Client : IDisposable
{
    private readonly HttpClient _client;
    private const string CesiumIonAssets = "assets/";

    public Client(string accessToken)
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://api.cesium.com/v1/"),
            DefaultRequestHeaders = { Authorization = AuthenticationHeaderValue.Parse($"Bearer {accessToken}")}
        };
    }

    public async Task<string> GetAssetListAsync()
    {
        return await _client.GetStringAsync(CesiumIonAssets);
    }

    public async Task<string> GetAssetAsync(int id)
    {
        return await _client.GetStringAsync(CesiumIonAssets + id);
    }

    public async Task<HttpResponseMessage> DeleteAssetAsync(int id)
    {
        return await _client.DeleteAsync(CesiumIonAssets + id);
    }

    public async Task<int?> UploadAssetAsStreamASync(Asset asset, Stream assetStream)
    {
        var uploadRequestContent = AssetUploadHelper.CreateUploadRequestContent(asset);

        using var responseMessage = await _client.PostAsync(CesiumIonAssets, uploadRequestContent);

        var jsonResponse = await responseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

        var response = JsonConvert.DeserializeObject<Response>(jsonResponse, new StringEnumConverter());

        if (response == null)
            return null;

        await AssetUploadHelper.UploadAsset(response.UploadLocation, asset.Name, assetStream);

        var updateContent = AssetUploadHelper.CreateUpdateUploadContent(response.OnComplete);

        using var updateResponseMessage = await _client.PostAsync(CesiumIonAssets, updateContent);

        return response.AssetMetadata.Id;
    }

    public void WhileUploading()
    {

    }

    public bool UploadCompleted(int id)
    {
        var jsonResponse = GetAssetAsync(id).Result;
        var response = JsonConvert.DeserializeObject<Response>(jsonResponse, new StringEnumConverter());

        if (response == null)
            return false;

        return response.AssetMetadata.Status == AssetStatus.COMPLETE;
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    private bool UploadIsReady(int assetId, out string message)
    {
        var jsonResponse = GetAssetAsync(assetId).Result;
        var response = JsonConvert.DeserializeObject<Response>(jsonResponse, new StringEnumConverter());

        if (response == null)
        {
            message = $"Could not get asset with id: {assetId}";
            return false;
        }

        switch (response.AssetMetadata.Status)
        {
            case AssetStatus.AWAITING_FILES:
                message = "Awaiting files...";
                return false;
            case AssetStatus.COMPLETE:
                message = "Asset tiled successfully\n" +
                          $"View in ion: https://cesium.com/ion/assets/{assetId}";
                return true;
            case AssetStatus.DATA_ERROR:
                message = "Ion detected a problem with the uploaded data";
                return false;
            case AssetStatus.ERROR:
                message = "An unknown tiling error occurred, please contact support@cesium.com.";
                return false;
            case AssetStatus.NOT_STARTED:
                message = "Tiling pipeline initializing.";
                return false;
            default:
                message = $"Asset is {response.AssetMetadata.PercentComplete}% complete";
                return false;
        }
    }
}