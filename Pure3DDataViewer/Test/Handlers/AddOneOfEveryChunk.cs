using NetP3DLib.P3D;
using NetP3DLib.P3D.Attributes;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Extensions;
using Pure3DDataViewerPluginAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            bool hasLocatorData = false;
            foreach (var parameter in constructorParameters)
            {
                bool locatorData = parameter.ParameterType == typeof(LocatorChunk.LocatorData);
                if (locatorData)
                {
                    hasLocatorData = true;
                    break;
                }

                if (parameter.Name == "name")
                {
                    parameters.Add($"{chunkType.Name}");
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

            if (hasLocatorData)
                continue;

            p3dFile.Chunks.Add((Chunk)constructor.Invoke([..parameters]));
        }

        return FileCallbackResult.Modified;
    }

    public bool IsFileSupported(P3DFile p3dFile) => true;
}
