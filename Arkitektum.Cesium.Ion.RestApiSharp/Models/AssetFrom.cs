using Arkitektum.Cesium.Ion.RestApiSharp.Util;

namespace Arkitektum.Cesium.Ion.RestApiSharp.Models;

internal class AssetFrom
{
    public AssetFromType Type { get; set; }
    public string Bucket { get; set; }
    public Credentials Credentials { get; set; }
    public KeyList? Keys { get; set; }
    public PrefixList? Prefixes { get; set; }
}

internal class Credentials
{
    public string? AccessKey { get; set; }
    public string? SecretAccessKey { get; set; }
}

internal class KeyList
{
    public List<string>? Keys { get; set; }
}

internal class PrefixList
{
    public List<string>? Prefixes { get; set; }
}