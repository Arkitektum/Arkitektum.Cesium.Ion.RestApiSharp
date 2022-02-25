using Arkitektum.Cesium.Ion.RestApiSharp.Util;

namespace Arkitektum.Cesium.Ion.RestApiSharp.Models;

internal class Response
{
    public AssetMetadata AssetMetadata { get; set; }
    public UploadLocation UploadLocation { get; set; }
    public OnComplete OnComplete { get; set; }
}

internal class AssetMetadata
{
    public int Id { get; set; }
    public AssetType Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Attribution { get; set; }
    public long Bytes { get; set; }
    public DateTime DateAdded { get; set; }
    public AssetStatus Status { get; set; }
    public int PercentComplete { get; set; }
}

internal class UploadLocation
{
    public string Endpoint { get; set; }
    public string AccessKey { get; set; }
    public string SecretAccessKey { get; set; }
    public string SessionToken { get; set; }
    public string Bucket { get; set; }
    public string Prefix { get; set; }
}

internal class OnComplete
{
    public string Method { get; set; }
    public string Url { get; set; }
    public Field? Fields { get; set; }
}

internal class Field
{
    public List<string>? Fields { get; set; }
}