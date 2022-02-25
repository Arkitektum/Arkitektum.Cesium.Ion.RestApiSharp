using Newtonsoft.Json;

namespace Arkitektum.Cesium.Ion.RestApiSharp.Util;

[JsonConverter(typeof(AssetTypeEnumJsonConverter))]
public enum AssetType
{
    _3DTILES,
    GLTF,
    IMAGERY,
    TERRAIN,
    KML,
    CZML,
    GEOJSON,
}

[JsonConverter(typeof(AssetTypeEnumJsonConverter))]
public enum SourceType
{
    RASTER_IMAGERY,
    RASTER_TERRAIN,
    TERRAIN_DATABASE,
    CITYGML,
    KML,
    _3D_CAPTURE,
    _3D_MODEL,
    POINT_CLOUD,
    _3DTILES,
    CZML,
    GEOJSON,
}

public enum AssetStatus
{
    AWAITING_FILES,
    COMPLETE,
    DATA_ERROR,
    ERROR,
    NOT_STARTED,
}

internal enum AssetFromType
{
    S3,
}

internal enum HeightReference
{
    MEAN_SEA_LEVEL,
    WGS84,
}

internal enum GeometryCompression
{
    NONE,
    DRACO,
}