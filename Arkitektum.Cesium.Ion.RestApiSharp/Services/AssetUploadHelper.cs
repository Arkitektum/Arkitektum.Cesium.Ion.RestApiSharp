using System.Net.Http.Headers;
using System.Text;
using Amazon;
using Amazon.Runtime;
using Amazon.S3.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Arkitektum.Cesium.Ion.RestApiSharp.Models;

namespace Arkitektum.Cesium.Ion.RestApiSharp.Services;

internal static class AssetUploadHelper
{
    public static HttpContent CreateUploadRequestContent(Asset asset)
    {
        var json = JsonConvert.SerializeObject(asset, Formatting.Indented, new StringEnumConverter());
            
        return new StringContent(json, Encoding.UTF8)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };
    }

    public static async Task UploadAsset(UploadLocation uploadLocation, string assetName, Stream assetStream)
    {
        var s3Credentials = new SessionAWSCredentials(uploadLocation.AccessKey,
            uploadLocation.SecretAccessKey, uploadLocation.SessionToken);

        var s3Config = new Amazon.S3.AmazonS3Config
        {
            ServiceURL = uploadLocation.Endpoint,
            SignatureVersion = "v4",
            RegionEndpoint = RegionEndpoint.GetBySystemName("us-east-1"),
        };

        using var s3Client = new Amazon.S3.AmazonS3Client(s3Credentials, s3Config);

        var uploadPartRequest = new UploadPartRequest
        {
            BucketName = uploadLocation.Bucket,
            Key = $"{uploadLocation.Prefix}{assetName}",
            InputStream = assetStream
        };

        await s3Client.UploadPartAsync(uploadPartRequest);
    }

    public static HttpContent CreateUpdateUploadContent(OnComplete onComplete)
    {
        var json = JsonConvert.SerializeObject(onComplete.Fields, Formatting.Indented, new StringEnumConverter());

        return new StringContent(json, Encoding.UTF8)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };
    }
}