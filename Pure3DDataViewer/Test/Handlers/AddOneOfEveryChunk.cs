using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Extensions;
using Pure3DDataViewerPluginAPI.Interfaces;
using System.ComponentModel;
using System.Reflection;

namespace Test.Handlers;

internal class AddOneOfEveryChunk : IFileHandler
{
    public string Name => "Add One of Every Chunk";

    public Image? Image => null;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value) { }

    public FileCallbackResult Handle(P3DFile p3dFile)
    {
        foreach (var chunkType in ChunkLoader.ChunkTypes.Select(x => x.Value.Item1))
        {
            var constructor = chunkType.GetConstructors().FirstOrDefault(constructor =>
            {
                var parameters = constructor.GetParameters();
                return !(parameters.Length == 1 && parameters[0].ParameterType == typeof(BinaryReader));
            });
            if (constructor == null)
                continue;

            var constructorParameters = constructor.GetParameters();
            if (constructorParameters.Length == 0)
            {
                p3dFile.Chunks.Add((Chunk)constructor.Invoke([]));
                continue;
            }

            var parameters = new List<object?>(constructorParameters.Length);
            int locatorDataIndex = -1;
            for (int i = 0; i < constructorParameters.Length; i++)
            {
                var parameter = constructorParameters[i];

                bool locatorData = parameter.ParameterType == typeof(LocatorChunk.LocatorData);
                if (locatorData)
                {
                    parameters.Add(new());
                    locatorDataIndex = i;
                    continue;
                }

                if (parameter.Name == "name")
                {
                    parameters.Add(chunkType.Name);
                    continue;
                }    

                var defaultVal = parameter.ParameterType.GetDefault();

                var parameterProperty = constructor.DeclaringType!.GetProperty(parameter.Name!, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (parameterProperty != null)
                {
                    var defaultValueAttribute = parameterProperty.GetCustomAttribute<DefaultValueAttribute>();
                    if (defaultValueAttribute != null)
                        defaultVal = Convert.ChangeType(defaultValueAttribute.Value, parameter.ParameterType);
                }

                parameters.Add(defaultVal);
            }

            if (locatorDataIndex != -1)
            {
                var nameIndex = parameters.IndexOf(chunkType.Name);
                foreach (LocatorChunk.LocatorTypes locatorType in Enum.GetValues(typeof(LocatorChunk.LocatorTypes)))
                {
                    parameters[nameIndex] = $"{chunkType.Name} - {locatorType}";
                    parameters[locatorDataIndex] = CreateLocatorData(locatorType);
                    p3dFile.Chunks.Add((Chunk)constructor.Invoke([.. parameters]));
                }
            }
            else
            {
                p3dFile.Chunks.Add((Chunk)constructor.Invoke([.. parameters]));
            }
        }

        return FileCallbackResult.Modified;
    }

    public bool IsFileSupported(P3DFile p3dFile) => true;

    private LocatorChunk.LocatorData CreateLocatorData(LocatorChunk.LocatorTypes locatorType)
    {
        var type = locatorType switch
        {
            LocatorChunk.LocatorTypes.Event => typeof(LocatorChunk.EventLocatorData),
            LocatorChunk.LocatorTypes.Script => typeof(LocatorChunk.ScriptLocatorData),
            LocatorChunk.LocatorTypes.Generic => typeof(LocatorChunk.GenericLocatorData),
            LocatorChunk.LocatorTypes.CarStart => typeof(LocatorChunk.CarStartLocatorData),
            LocatorChunk.LocatorTypes.Spline => typeof(LocatorChunk.SplineLocatorData),
            LocatorChunk.LocatorTypes.DynamicZone => typeof(LocatorChunk.DynamicZoneLocatorData),
            LocatorChunk.LocatorTypes.Occlusion => typeof(LocatorChunk.OcclusionLocatorData),
            LocatorChunk.LocatorTypes.InteriorEntrance => typeof(LocatorChunk.InteriorEntranceLocatorData),
            LocatorChunk.LocatorTypes.Directional => typeof(LocatorChunk.DirectionalLocatorData),
            LocatorChunk.LocatorTypes.Action => typeof(LocatorChunk.ActionLocatorData),
            LocatorChunk.LocatorTypes.FOV => typeof(LocatorChunk.FOVLocatorData),
            LocatorChunk.LocatorTypes.BreakableCamera => typeof(LocatorChunk.BreakableCameraLocatorData),
            LocatorChunk.LocatorTypes.StaticCamera => typeof(LocatorChunk.StaticCameraLocatorData),
            LocatorChunk.LocatorTypes.PedGroup => typeof(LocatorChunk.PedGroupLocatorData),
            LocatorChunk.LocatorTypes.Coin => typeof(LocatorChunk.CoinLocatorData),
            _ => throw new Exception($"Unsupported Locator Type: {locatorType}.")
        };

        var constructor = type.GetConstructors().FirstOrDefault(constructor =>
        {
            var parameters = constructor.GetParameters();
            return !(parameters.Length == 1 && parameters[0].ParameterType == typeof(List<uint>));
        }) ?? throw new Exception($"No valid constructor found for Locator Type: {locatorType}.");

        return (LocatorChunk.LocatorData)constructor.Invoke([.. constructor.GetParameters().Select(x => x.ParameterType.GetDefault())]);
    }
}
