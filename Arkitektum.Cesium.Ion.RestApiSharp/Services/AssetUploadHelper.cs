using System.Net.Http.Headers;
using System.Text;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Arkitektum.Cesium.Ion.RestApiSharp.Models;
using Newtonsoft.Json.Serialization;

namespace Arkitektum.Cesium.Ion.RestApiSharp.Services;

internal static class AssetUploadHelper
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        Converters = { new StringEnumConverter() },
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
    };

    public static HttpContent CreateUploadRequestContent(Asset asset)
    {
        var json = JsonConvert.SerializeObject(asset, Formatting.Indented, SerializerSettings);
            
        return new StringContent(json, Encoding.UTF8)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };
    }

    public static AmazonS3Client CreateAmazonS3Client(UploadLocation uploadLocation)
    {
        var s3Credentials = new SessionAWSCredentials(
            uploadLocation.AccessKey,
            uploadLocation.SecretAccessKey,
            uploadLocation.SessionToken);

        var s3Config = new AmazonS3Config
        {
            ServiceURL = uploadLocation.Endpoint,
            SignatureVersion = "v4",
            RegionEndpoint = RegionEndpoint.GetBySystemName("us-east-1"),
        };

        return new AmazonS3Client(s3Credentials, s3Config);
    }

    public static UploadPartRequest CreateUploadRequest(UploadLocation uploadLocation, FileStream assetStream)
    {
        return new UploadPartRequest
        {
            BucketName = uploadLocation.Bucket,
            Key = $"{uploadLocation.Prefix}{Path.GetFileName(assetStream.Name)}",
            InputStream = assetStream,
        };
    }

    public static HttpContent CreateUpdateUploadContent(OnComplete onComplete)
    {
        var json = onComplete.Fields?.Fields != null
            ? JsonConvert.SerializeObject(onComplete.Fields.Fields, Formatting.Indented, new StringEnumConverter())
            : "{}";

        return new StringContent(json, Encoding.UTF8)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };
    }
}