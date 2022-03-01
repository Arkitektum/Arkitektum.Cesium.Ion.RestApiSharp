using Arkitektum.Cesium.Ion.RestApiSharp.Util;

namespace Arkitektum.Cesium.Ion.RestApiSharp.Models;

public class Asset
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Attribution { get; set; }
    public AssetType Type { get; set; }
    public AssetOptions Options { get; set; }
    public AssetFrom? From { get; set; }
}