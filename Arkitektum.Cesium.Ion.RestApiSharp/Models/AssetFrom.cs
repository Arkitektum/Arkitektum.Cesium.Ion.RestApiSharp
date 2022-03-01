using Arkitektum.Cesium.Ion.RestApiSharp.Util;

namespace Arkitektum.Cesium.Ion.RestApiSharp.Models;

public class AssetFrom
{
    public AssetFromType Type { get; set; }
    public string Bucket { get; set; }
    public Credentials Credentials { get; set; }
    public KeyList? Keys { get; set; }
    public PrefixList? Prefixes { get; set; }
}

public class Credentials
{
    public string? AccessKey { get; set; }
    public string? SecretAccessKey { get; set; }
}

public class KeyList
{
    public List<string>? Keys { get; set; }
}

public class PrefixList
{
    public List<string>? Prefixes { get; set; }
}