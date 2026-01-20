using ConvertToLua.Extensions;
using NetP3DLib.Numerics;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using NetP3DLib.P3D.Enums;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Text;

namespace ConvertToLua.Helpers;

internal static class ChunkMap
{
    private static readonly Dictionary<Type, LuaChunkMapping> _mappings = [];

    static ChunkMap()
    {
        Register<AnimatedObjectAnimationChunk>(new()
        {
            LuaClassName = "AnimatedObjectAnimation",
            PropertyOrder =
            {
                "Version",
                "Name",
                "FrameRate",
                "NumOldFrameControllers",
            }
        });

        Register<AnimatedObjectChunk>(new()
        {
            LuaClassName = "AnimatedObject",
            PropertyOrder =
            {
                "Version",
                "Name",
                "FactoryName",
                "StartingAnimation",
            }
        });

        Register<AnimatedObjectFactoryChunk>(new()
        {
            LuaClassName = "AnimatedObjectFactory",
            PropertyOrder =
            {
                "Version",
                "Name",
                "BaseAnimation",
                "NumAnimations",
            }
        });

        Register<AnimationChannelCountChunk>(new()
        {
            LuaClassName = "AnimationChannelCount",
            PropertyOrder =
            {
                "Version",
                "ChannelChunkID",
                "NumKeys",
            }
        });

        Register<AnimationChunk>(new()
        {
            LuaClassName = "Animation",
            PropertyOrder =
            {
                "Version",
                "Name",
                "AnimationType",
                "NumFrames",
                "FrameRate",
                "Cyclic",
            }
        });

        Register<AnimationGroupChunk>(new()
        {
            LuaClassName = "AnimationGroup",
            PropertyOrder =
            {
                "Version",
                "Name",
                "GroupID",
            }
        });

        Register<AnimationGroupListChunk>(new()
        {
            LuaClassName = "AnimationGroupList",
            PropertyOrder =
            {
                "Version",
            }
        });

        Register<AnimationHeaderChunk>(new()
        {
            LuaClassName = "AnimationHeader",
            PropertyOrder =
            {
                "Version",
                "NumGroups",
            }
        });

        Register<AnimationSizeChunk>(new()
        {
            LuaClassName = "AnimationSize",
            PropertyOrder =
            {
                "Version",
                "PC",
                "PS2",
                "XBOX",
                "GC",
            }
        });

        // TODO: AnimationSyncFrame - Missing matching Lua file

        Register<AnimChunk>(new()
        {
            LuaClassName = "Anim",
            PropertyOrder =
            {
                "Name",
                "Version",
                "HasAlpha",
            }
        });

        Register<AnimCollChunk>(new()
        {
            LuaClassName = "AnimColl",
            PropertyOrder =
            {
                "Name",
                "Version",
                "HasAlpha",
            }
        });

        Register<AnimDynaPhysChunk>(new()
        {
            LuaClassName = "AnimDynaPhys",
            PropertyOrder =
            {
                "Name",
                "Version",
                "HasAlpha",
            }
        });

        Register<AnimDynaPhysWrapperChunk>(new()
        {
            LuaClassName = "AnimDynaPhysWrapper",
            PropertyOrder =
            {
                "Name",
                "Version",
                "HasAlpha",
            }
        });

        Register<AnimObjWrapperChunk>(new()
        {
            LuaClassName = "AnimObjWrapper",
            PropertyOrder =
            {
                "Name",
                "Version",
                "HasAlpha",
            }
        });

        Register<ATCChunk>(new()
        {
            LuaClassName = "ATC",
            PropertyOrder =
            {
                "Entries",
            }
        });

        // TODO: BillboardQuad - Missing matching Lua file

        // TODO: BillboardQuadGroup - Missing matching Lua file

        // TODO: BillboardTextureUV - Missing matching Lua file

        // TODO: BillboardTransform - Missing matching Lua file

        // TODO: BinormalList - Missing matching Lua file

        // TODO: BlackMagic - Missing matching Lua file

        Register<BooleanChannelChunk>(new()
        {
            LuaClassName = "BooleanChannel",
            PropertyOrder =
            {
                "Version",
                "Param",
                "StartState",
                "Values",
            }
        });

        Register<BoundingBoxChunk>(new()
        {
            LuaClassName = "BoundingBox",
            PropertyOrder =
            {
                "Low",
                "High",
            }
        });

        Register<BoundingSphereChunk>(new()
        {
            LuaClassName = "BoundingSphere",
            PropertyOrder =
            {
                "Centre",
                "Radius",
            }
        });

        Register<BreakableObjectChunk>(new()
        {
            LuaClassName = "BreakableObject",
            PropertyOrder =
            {
                "Index",
                "MaxInstances",
            }
        });

        Register<CameraChunk>(new()
        {
            LuaClassName = "Camera",
            PropertyOrder =
            {
                "Name",
                "Version",
                "FOV",
                "AspectRatio",
                "NearClip",
                "FarClip",
                "Position",
                "Look",
                "Up",
            }
        });

        Register<ChannelInterpolationModeChunk>(new()
        {
            LuaClassName = "ChannelInterpolationMode",
            PropertyOrder =
            {
                "Version",
                "Interpolate",
            }
        });

        Register<CollisionAxisAlignedBoundingBoxChunk>(new()
        {
            LuaClassName = "CollisionAxisAlignedBoundingBox",
            PropertyOrder =
            {
                "Nothing",
            }
        });

        Register<CollisionCylinderChunk>(new()
        {
            LuaClassName = "CollisionCylinder",
            PropertyOrder =
            {
                "Radius",
                "HalfLength",
                "FlatEnd",
            }
        });

        Register<CollisionEffectChunk>(new()
        {
            LuaClassName = "CollisionEffect",
            PropertyOrder =
            {
                "ClassType",
                "PhysPropID",
                "SoundResourceDataName",
            }
        });

        // TODO: CollisionMeshTree - Missing matching Lua file

        // TODO: CollisionMeshTriangleList - Missing matching Lua file

        // TODO: CollisionMeshVectorList - Missing matching Lua file

        // TODO: CollisionMetaData - Missing matching Lua file

        // TODO: CollisionMetaDataShortChannel - Missing matching Lua file

        // TODO: CollisionMetaDataVectorChannel - Missing matching Lua file

        Register<CollisionObjectAttributeChunk>(new()
        {
            LuaClassName = "CollisionObjectAttribute",
            PropertyOrder =
            {
                "IsStatic",
                "DefaultArea",
                "CanRoll",
                "CanSlide",
                "CanSpin",
                "CanBounce",
                "ExtraAttribute1",
                "ExtraAttribute2",
                "ExtraAttribute3",
            }
        });

        Register<CollisionObjectChunk>(new()
        {
            LuaClassName = "CollisionObject",
            PropertyOrder =
            {
                "Name",
                "Version",
                "MaterialName",
                "NumSubObjects",
            }
        });

        Register<CollisionOrientedBoundingBoxChunk>(new()
        {
            LuaClassName = "CollisionOrientedBoundingBox",
            PropertyOrder =
            {
                "HalfExtents",
            }
        });

        Register<CollisionSphereChunk>(new()
        {
            LuaClassName = "CollisionSphere",
            PropertyOrder =
            {
                "Radius",
            }
        });

        Register<CollisionVectorChunk>(new()
        {
            LuaClassName = "CollisionVector",
            PropertyOrder =
            {
                "Vector",
            }
        });

        Register<CollisionVolumeChunk>(new()
        {
            LuaClassName = "CollisionVolume",
            PropertyOrder =
            {
                "ObjectReferenceIndex",
                "OwnerIndex",
            }
        });

        Register<CollisionVolumeOwnerChunk>(new()
        {
            LuaClassName = "CollisionVolumeOwner",
            PropertyOrder = []
        });

        Register<CollisionVolumeOwnerNameChunk>(new()
        {
            LuaClassName = "CollisionVolumeOwnerName",
            PropertyOrder =
            {
                "Name",
            }
        });

        Register<CollisionWallChunk>(new()
        {
            LuaClassName = "CollisionWall",
            PropertyOrder = []
        });

        Register<ColourChannelChunk>(new()
        {
            LuaClassName = "ColourChannel",
            PropertyOrder =
            {
                "Version",
                "Param",
                "Frames",
                "Values",
            }
        });

        Register<ColourListChunk>(new()
        {
            LuaClassName = "ColourList",
            PropertyOrder =
            {
                "Colours",
            }
        });

        // TODO: CompositeDrawable2 - Missing matching Lua file

        Register<CompositeDrawableChunk>(new()
        {
            LuaClassName = "CompositeDrawable",
            PropertyOrder =
            {
                "Name",
                "SkeletonName",
            }
        });

        Register<CompositeDrawableEffectChunk>(new()
        {
            LuaClassName = "CompositeDrawableEffect",
            PropertyOrder =
            {
                "Name",
                "IsTranslucent",
                "SkeletonJointId",
            }
        });

        Register<CompositeDrawableEffectListChunk>(new()
        {
            LuaClassName = "CompositeDrawableEffectList",
            PropertyOrder = []
        });

        // TODO: CompositeDrawablePrimitive - Missing matching Lua file

        Register<CompositeDrawablePropChunk>(new()
        {
            LuaClassName = "CompositeDrawableProp",
            PropertyOrder =
            {
                "Name",
                "IsTranslucent",
                "SkeletonJointId",
            }
        });

        Register<CompositeDrawablePropListChunk>(new()
        {
            LuaClassName = "CompositeDrawablePropList",
            PropertyOrder = []
        });

        Register<CompositeDrawableSkinChunk>(new()
        {
            LuaClassName = "CompositeDrawableSkin",
            PropertyOrder =
            {
                "Name",
                "IsTranslucent",
            }
        });

        Register<CompositeDrawableSkinListChunk>(new()
        {
            LuaClassName = "CompositeDrawableSkinList",
            PropertyOrder = []
        });

        Register<CompositeDrawableSortOrderChunk>(new()
        {
            LuaClassName = "CompositeDrawableSortOrder",
            PropertyOrder =
            {
                "SortOrder",
            }
        });

        Register<CompressedQuaternionChannel2Chunk>(new()
        {
            LuaClassName = "CompressedQuaternionChannel2",
            PropertyOrder =
            {
                "Version",
                "Param",
                "Frames",
                "Values",
            }
        });

        Register<CompressedQuaternionChannelChunk>(new()
        {
            LuaClassName = "CompressedQuaternionChannel",
            PropertyOrder =
            {
                "Version",
                "Param",
                "Frames",
                "Values",
            }
        });

        Register<DynaPhysChunk>(new()
        {
            LuaClassName = "DynaPhys",
            PropertyOrder =
            {
                "Name",
                "Version",
                "HasAlpha",
            }
        });

        Register<EntityChannelChunk>(new()
        {
            LuaClassName = "EntityChannel",
            PropertyOrder =
            {
                "Version",
                "Param",
                "Frames",
                "Values",
            }
        });

        Register<ExportInfoChunk>(new()
        {
            LuaClassName = "ExportInfo",
            PropertyOrder =
            {
                "Name",
            }
        });

        Register<ExportInfoNamedIntegerChunk>(new()
        {
            LuaClassName = "ExportInfoNamedInteger",
            PropertyOrder =
            {
                "Name",
                "Value",
            }
        });

        Register<ExportInfoNamedStringChunk>(new()
        {
            LuaClassName = "ExportInfoNamedString",
            PropertyOrder =
            {
                "Name",
                "Value",
            }
        });

        Register<ExpressionChunk>(new()
        {
            LuaClassName = "Expression",
            PropertyOrder =
            {
                "Version",
                "Name",
                "Keys",
                "Indices",
            }
        });

        Register<ExpressionGroupChunk>(new()
        {
            LuaClassName = "ExpressionGroup",
            PropertyOrder =
            {
                "Version",
                "Name",
                "TargetName",
                "Stages",
            }
        });

        Register<ExpressionMixerChunk>(new()
        {
            LuaClassName = "ExpressionMixer",
            PropertyOrder =
            {
                "Version",
                "Name",
                "Type",
                "TargetName",
                "ExpressionGroupName",
            }
        });

        Register<FenceChunk>(new()
        {
            LuaClassName = "Fence",
            PropertyOrder = []
        });

        // TODO: Fenceline - Missing matching Lua file

        Register<Float1ChannelChunk>(new()
        {
            LuaClassName = "Float1Channel",
            PropertyOrder =
            {
                "Version",
                "Param",
                "Frames",
                "Values",
            }
        });

        Register<Float2ChannelChunk>(new()
        {
            LuaClassName = "Float2Channel",
            PropertyOrder =
            {
                "Version",
                "Param",
                "Frames",
                "Values",
            }
        });

        Register<FollowCameraDataChunk>(new()
        {
            LuaClassName = "FollowCameraData",
            PropertyOrder =
            {
                "Index",
                "Rotation",
                "Elevation",
                "Magnitude",
                "TargetOffset",
            }
        });

        Register<FrameControllerChunk>(new()
        {
            LuaClassName = "FrameController",
            PropertyOrder =
            {
                "Version",
                "Name",
                "Type",
                "CycleMode",
                "NumCycles",
                "InfiniteCycle",
                "HierarchyName",
                "AnimationName",
            }
        });

        // TODO: FrameControllerList - Missing matching Lua file

        // TODO: FrontendGroup2 - Missing matching Lua file

        Register<FrontendGroupChunk>(new()
        {
            LuaClassName = "FrontendGroup",
            PropertyOrder =
            {
                "Name",
                "Version",
                "Alpha",
            }
        });

        Register<FrontendImageResourceChunk>(new()
        {
            LuaClassName = "FrontendImageResource",
            PropertyOrder =
            {
                "Name",
                "Version",
                "Filename",
            }
        });

        Register<FrontendLanguageChunk>(new()
        {
            LuaClassName = "FrontendLanguage",
            PropertyOrder =
            {
                "Name",
                "Language",
                "Modulo",
                "Entries",
            }
        });

        // TODO: FrontendLayer2 - Missing matching Lua file

        Register<FrontendLayerChunk>(new()
        {
            LuaClassName = "FrontendLayer",
            PropertyOrder =
            {
                "Name",
                "Version",
                "Visible",
                "Editable",
                "Alpha",
            }
        });

        Register<FrontendMultiSpriteChunk>(new()
        {
            LuaClassName = "FrontendMultiSprite",
            CustomOverride = static chunk =>
            {
                var frontendMultiSpriteChunk = (FrontendMultiSpriteChunk)chunk;
                return $"{FormatLuaValue(frontendMultiSpriteChunk.Name)}, {FormatLuaValue(frontendMultiSpriteChunk.Version)}, {{X = {FormatLuaValue(frontendMultiSpriteChunk.PositionX)}, Y = {FormatLuaValue(frontendMultiSpriteChunk.PositionY)}}}, {{X = {FormatLuaValue(frontendMultiSpriteChunk.DimensionX)}, Y = {FormatLuaValue(frontendMultiSpriteChunk.DimensionY)}}}, {{X = {FormatLuaValue(frontendMultiSpriteChunk.JustificationX)}, Y = {FormatLuaValue(frontendMultiSpriteChunk.JustificationY)}}}, {FormatLuaValue(frontendMultiSpriteChunk.Colour)}, {FormatLuaValue(frontendMultiSpriteChunk.Translucency)}, {FormatLuaValue(frontendMultiSpriteChunk.RotationValue)}, {FormatLuaValue(frontendMultiSpriteChunk.ImageNames)}";
            }
        });

        Register<FrontendMultiTextChunk>(new()
        {
            LuaClassName = "FrontendMultiText",
            CustomOverride = static chunk =>
            {
                var frontendMultiTextChunk = (FrontendMultiTextChunk)chunk;
                return $"{FormatLuaValue(frontendMultiTextChunk.Name)}, {FormatLuaValue(frontendMultiTextChunk.Version)}, {{X = {FormatLuaValue(frontendMultiTextChunk.PositionX)}, Y = {FormatLuaValue(frontendMultiTextChunk.PositionY)}}}, {{X = {FormatLuaValue(frontendMultiTextChunk.DimensionX)}, Y = {FormatLuaValue(frontendMultiTextChunk.DimensionY)}}}, {{X = {FormatLuaValue(frontendMultiTextChunk.JustificationX)}, Y = {FormatLuaValue(frontendMultiTextChunk.JustificationY)}}}, {FormatLuaValue(frontendMultiTextChunk.Colour)}, {FormatLuaValue(frontendMultiTextChunk.Translucency)}, {FormatLuaValue(frontendMultiTextChunk.RotationValue)}, {FormatLuaValue(frontendMultiTextChunk.TextStyleName)}, {FormatLuaValue(frontendMultiTextChunk.ShadowEnabled)}, {FormatLuaValue(frontendMultiTextChunk.ShadowColour)}, {{X = {FormatLuaValue(frontendMultiTextChunk.ShadowOffsetX)}, Y = {FormatLuaValue(frontendMultiTextChunk.ShadowOffsetY)}}}, {FormatLuaValue(frontendMultiTextChunk.CurrentText)}";
            }
        });

        Register<FrontendPageChunk>(new()
        {
            LuaClassName = "FrontendPage",
            CustomOverride = static chunk =>
            {
                var frontendPageChunk = (FrontendPageChunk)chunk;
                return $"{FormatLuaValue(frontendPageChunk.Name)}, {FormatLuaValue(frontendPageChunk.Version)}, {{X = {FormatLuaValue(frontendPageChunk.ResolutionX)}, Y = {FormatLuaValue(frontendPageChunk.ResolutionY)}}}";
            }
        });

        Register<FrontendPolygonChunk>(new()
        {
            LuaClassName = "FrontendPolygon",
            PropertyOrder =
            {
                "Name",
                "Version",
                "Translucency",
                "Points",
                "Colours",
            }
        });

        Register<FrontendProjectChunk>(new()
        {
            LuaClassName = "FrontendProject",
            CustomOverride = static chunk =>
            {
                var frontendProjectChunk = (FrontendProjectChunk)chunk;
                return $"{FormatLuaValue(frontendProjectChunk.Name)}, {FormatLuaValue(frontendProjectChunk.Version)}, {{X = {FormatLuaValue(frontendProjectChunk.ResolutionX)}, Y = {FormatLuaValue(frontendProjectChunk.ResolutionY)}}}, {FormatLuaValue(frontendProjectChunk.Platform)}, {FormatLuaValue(frontendProjectChunk.PagePath)}, {FormatLuaValue(frontendProjectChunk.ResourcePath)}, {FormatLuaValue(frontendProjectChunk.ScreenPath)}";
            }
        });

        Register<FrontendPure3DObjectChunk>(new()
        {
            LuaClassName = "FrontendPure3DObject",
            CustomOverride = static chunk =>
            {
                var frontendPure3DObjectChunk = (FrontendPure3DObjectChunk)chunk;
                return $"{FormatLuaValue(frontendPure3DObjectChunk.Name)}, {FormatLuaValue(frontendPure3DObjectChunk.Version)}, {{X = {FormatLuaValue(frontendPure3DObjectChunk.PositionX)}, Y = {FormatLuaValue(frontendPure3DObjectChunk.PositionY)}}}, {{X = {FormatLuaValue(frontendPure3DObjectChunk.DimensionX)}, Y = {FormatLuaValue(frontendPure3DObjectChunk.DimensionY)}}}, {{X = {FormatLuaValue(frontendPure3DObjectChunk.JustificationX)}, Y = {FormatLuaValue(frontendPure3DObjectChunk.JustificationY)}}}, {FormatLuaValue(frontendPure3DObjectChunk.Colour)}, {FormatLuaValue(frontendPure3DObjectChunk.Translucency)}, {FormatLuaValue(frontendPure3DObjectChunk.RotationValue)}, {FormatLuaValue(frontendPure3DObjectChunk.Pure3DFilename)}";
            }
        });

        Register<FrontendPure3DResourceChunk>(new()
        {
            LuaClassName = "FrontendPure3DResource",
            PropertyOrder =
            {
                "Name",
                "Version",
                "Filename",
                "InventoryName",
                "CameraName",
                "AnimationName",
            }
        });

        Register<FrontendScreenChunk>(new()
        {
            LuaClassName = "FrontendScreen",
            PropertyOrder =
            {
                "Name",
                "Version",
                "PageNames",
            }
        });

        Register<FrontendStringHardCodedChunk>(new()
        {
            LuaClassName = "FrontendStringHardCoded",
            PropertyOrder =
            {
                "String",
            }
        });

        Register<FrontendStringTextBibleChunk>(new()
        {
            LuaClassName = "FrontendStringTextBible",
            PropertyOrder =
            {
                "BibleName",
                "StringID",
            }
        });

        Register<FrontendTextBibleChunk>(new()
        {
            LuaClassName = "FrontendTextBible",
            PropertyOrder =
            {
                "Name",
            }
        });

        Register<FrontendTextBibleResourceChunk>(new()
        {
            LuaClassName = "FrontendTextBibleResource",
            PropertyOrder =
            {
                "Name",
                "Version",
                "Filename",
                "InventoryName",
            }
        });

        Register<FrontendTextStyleResourceChunk>(new()
        {
            LuaClassName = "FrontendTextStyleResource",
            PropertyOrder =
            {
                "Name",
                "Version",
                "Filename",
                "InventoryName",
            }
        });

        Register<GameAttrChunk>(new()
        {
            LuaClassName = "GameAttr",
            PropertyOrder =
            {
                "Name",
                "Version",
            }
        });

        Register<GameAttributeColourParameterChunk>(new()
        {
            LuaClassName = "GameAttributeColourParameter",
            PropertyOrder =
            {
                "Name",
                "Value",
            }
        });

        Register<GameAttributeFloatParameterChunk>(new()
        {
            LuaClassName = "GameAttributeFloatParameter",
            PropertyOrder =
            {
                "Name",
                "Value",
            }
        });

        Register<GameAttributeIntegerParameterChunk>(new()
        {
            LuaClassName = "GameAttributeIntegerParameter",
            PropertyOrder =
            {
                "Name",
                "Value",
            }
        });

        Register<GameAttributeMatrixParameterChunk>(new()
        {
            LuaClassName = "GameAttributeMatrixParameter",
            PropertyOrder =
            {
                "Name",
                "Value",
            }
        });

        Register<GameAttributeVectorParameterChunk>(new()
        {
            LuaClassName = "GameAttributeVectorParameter",
            PropertyOrder =
            {
                "Name",
                "Value",
            }
        });

        Register<HistoryChunk>(new()
        {
            LuaClassName = "History",
            PropertyOrder =
            {
                "History",
            }
        });

        Register<ImageChunk>(new()
        {
            LuaClassName = "Image",
            PropertyOrder =
            {
                "Name",
                "Version",
                "Width",
                "Height",
                "Bpp",
                "Palettized",
                "HasAlpha",
                "Format",
            }
        });

        Register<ImageDataChunk>(new()
        {
            LuaClassName = "ImageData",
            CustomOverride = static chunk =>
            {
                var imageDataChunk = (ImageDataChunk)chunk;

                if (imageDataChunk.ImageData.Length == 0)
                    return "\"\"";

                var sb = new StringBuilder();
                sb.Append('"');
                foreach (var b in imageDataChunk.ImageData)
                    _ = sb.AppendFormat(@"\x{0:X2}", b);
                sb.Append('"');

                return sb.ToString();
            }
        });

        // TODO: ImageFont - Missing matching Lua file

        // TODO: ImageGlyphList - Missing matching Lua file

        Register<IndexListChunk>(new()
        {
            LuaClassName = "IndexList",
            PropertyOrder =
            {
                "Indices",
            }
        });

        Register<InstanceListChunk>(new()
        {
            LuaClassName = "InstanceList",
            PropertyOrder =
            {
                "Name",
            }
        });

        Register<InstParticleSystemChunk>(new()
        {
            LuaClassName = "InstParticleSystem",
            PropertyOrder =
            {
                "ParticleType",
                "MaxInstances",
            }
        });

        Register<InstStatEntityChunk>(new()
        {
            LuaClassName = "InstStatEntity",
            PropertyOrder =
            {
                "Name",
                "Version",
                "HasAlpha",
            }
        });

        Register<InstStatPhysChunk>(new()
        {
            LuaClassName = "InstStatPhys",
            PropertyOrder =
            {
                "Name",
                "Version",
                "HasAlpha",
            }
        });

        Register<IntegerChannelChunk>(new()
        {
            LuaClassName = "IntegerChannel",
            PropertyOrder =
            {
                "Version",
                "Param",
                "Frames",
                "Values",
            }
        });

        Register<IntersectChunk>(new()
        {
            LuaClassName = "Intersect",
            PropertyOrder =
            {
                "Indices",
                "Positions",
                "Normals",
            }
        });

        Register<IntersectionChunk>(new()
        {
            LuaClassName = "Intersection",
            PropertyOrder =
            {
                "Name",
                "Position",
                "Radius",
                "TrafficBehaviour",
            }
        });

        Register<IntersectMesh2Chunk>(new()
        {
            LuaClassName = "IntersectMesh2",
            PropertyOrder =
            {
                "SurfaceType",
            }
        });

        Register<IntersectMeshChunk>(new()
        {
            LuaClassName = "IntersectMesh",
            PropertyOrder =
            {
                "Name",
            }
        });

        Register<LensFlareChunk>(new()
        {
            LuaClassName = "LensFlare",
            PropertyOrder =
            {
                "Name",
                "Version",
            }
        });

        // TODO: LensFlareGroup - Missing matching Lua file

        Register<LightChunk>(new()
        {
            LuaClassName = "Light",
            PropertyOrder =
            {
                "Name",
                "Version",
                "Type",
                "Colour",
                "Constant",
                "Linear",
                "Squared",
                "Enabled",
            }
        });

        // TODO: LightConeParam - Missing matching Lua file

        Register<LightDirectionChunk>(new()
        {
            LuaClassName = "LightDirection",
            PropertyOrder =
            {
                "Direction",
            }
        });

        Register<LightGroupChunk>(new()
        {
            LuaClassName = "LightGroup",
            PropertyOrder =
            {
                "Name",
                "Lights",
            }
        });

        Register<LightIlluminationTypeChunk>(new()
        {
            LuaClassName = "LightIlluminationType",
            PropertyOrder =
            {
                "IlluminationType",
            }
        });

        Register<LightPositionChunk>(new()
        {
            LuaClassName = "LightPosition",
            PropertyOrder =
            {
                "Position",
            }
        });

        Register<LightShadowChunk>(new()
        {
            LuaClassName = "LightShadow",
            PropertyOrder =
            {
                "Shadow",
            }
        });

        Register<LocatorChunk>(new()
        {
            LuaClassName = "Locator",
            CustomOverride = chunk =>
            {
                var locatorChunk = (LocatorChunk)chunk;

                var sb = new StringBuilder();

                sb.Append($"{FormatLuaValue(locatorChunk.Name)}, {FormatLuaValue(locatorChunk.Position)}, {FormatLuaValue(locatorChunk.LocatorType)}");

                switch (locatorChunk.TypeData)
                {
                    case LocatorChunk.EventLocatorData eventData:
                        sb.Append($", {FormatLuaValue(eventData.Event)}");
                        if (eventData.Parameter.HasValue)
                            sb.Append($", {FormatLuaValue(eventData.Parameter.Value)}");
                        break;
                    case LocatorChunk.ScriptLocatorData scriptData:
                        sb.Append($", {FormatLuaValue(scriptData.Key)}");
                        break;
                    case LocatorChunk.GenericLocatorData genericData:
                        // No extra data
                        break;
                    case LocatorChunk.CarStartLocatorData carStartData:
                        sb.Append($", {FormatLuaValue(carStartData.Rotation)}");
                        if (carStartData.ParkedCar.HasValue)
                            sb.Append($", {FormatLuaValue(carStartData.ParkedCar.Value)}");
                        if (carStartData.FreeCar != null)
                            sb.Append($", {FormatLuaValue(carStartData.FreeCar)}");
                        break;
                    case LocatorChunk.SplineLocatorData splineData:
                        // No extra data
                        break;
                    case LocatorChunk.DynamicZoneLocatorData dynamicZoneData:
                        sb.Append($", {FormatLuaValue(dynamicZoneData.DynaLoadData)}");
                        break;
                    case LocatorChunk.OcclusionLocatorData occlusionData:
                        if (occlusionData.Occlusions.HasValue)
                            sb.Append($", {FormatLuaValue(occlusionData.Occlusions)}");
                        break;
                    case LocatorChunk.InteriorEntranceLocatorData interiorEntranceData:
                        sb.Append($", {FormatLuaValue(interiorEntranceData.InteriorName)}");
                        sb.Append($", {FormatLuaValue(interiorEntranceData.Right)}");
                        sb.Append($", {FormatLuaValue(interiorEntranceData.Up)}");
                        sb.Append($", {FormatLuaValue(interiorEntranceData.Front)}");
                        break;
                    case LocatorChunk.DirectionalLocatorData directionalData:
                        sb.Append($", {FormatLuaValue(directionalData.Right)}");
                        sb.Append($", {FormatLuaValue(directionalData.Up)}");
                        sb.Append($", {FormatLuaValue(directionalData.Front)}");
                        break;
                    case LocatorChunk.ActionLocatorData actionData:
                        sb.Append($", {FormatLuaValue(actionData.ObjectName)}");
                        sb.Append($", {FormatLuaValue(actionData.JointName)}");
                        sb.Append($", {FormatLuaValue(actionData.ActionName)}");
                        sb.Append($", {FormatLuaValue(actionData.ButtonInput)}");
                        sb.Append($", {FormatLuaValue(actionData.ShouldTransform)}");
                        break;
                    case LocatorChunk.FOVLocatorData fovData:
                        sb.Append($", {FormatLuaValue(fovData.FOV)}");
                        sb.Append($", {FormatLuaValue(fovData.Type)}");
                        sb.Append($", {FormatLuaValue(fovData.Rate)}");
                        break;
                    case LocatorChunk.BreakableCameraLocatorData breakableCameraData:
                        // No extra data
                        break;
                    case LocatorChunk.StaticCameraLocatorData staticCameraData:
                        sb.Append($", {FormatLuaValue(staticCameraData.TargetPosition)}");
                        sb.Append($", {FormatLuaValue(staticCameraData.FOV)}");
                        sb.Append($", {FormatLuaValue(staticCameraData.TargetLag)}");
                        sb.Append($", {FormatLuaValue(staticCameraData.FollowPlayer)}");
                        if (!staticCameraData.TransitionTargetRate.HasValue)
                            break;
                        sb.Append($", {FormatLuaValue(staticCameraData.TransitionTargetRate.Value)}");
                        if (!staticCameraData.Flags.HasValue)
                            break;
                        sb.Append($", {FormatLuaValue(staticCameraData.Flags.Value)}");
                        if (!staticCameraData.CutInOut.HasValue || !staticCameraData.Data.HasValue)
                            break;
                        sb.Append($", {FormatLuaValue(staticCameraData.CutInOut.Value)}");
                        sb.Append($", {FormatLuaValue(staticCameraData.Data.Value)}");
                        break;
                    case LocatorChunk.PedGroupLocatorData pedGroupData:
                        sb.Append($", {FormatLuaValue(pedGroupData.GroupNum)}");
                        break;
                    case LocatorChunk.CoinLocatorData coinData:
                        // No extra data
                        break;
                    case LocatorChunk.UnknownLocatorData unknownData:
                        sb.Append($", \"");
                        foreach (var b in unknownData.DataArray)
                            _ = sb.AppendFormat(@"\x{0:X2}", b);
                        sb.Append('"');
                        break;
                }

                return sb.ToString();
            }
        });

        Register<LocatorMatrixChunk>(new()
        {
            LuaClassName = "LocatorMatrix",
            PropertyOrder =
            {
                "Matrix",
            }
        });

        Register<MatrixListChunk>(new()
        {
            LuaClassName = "MatrixList",
            PropertyOrder =
            {
                "Matrices",
            }
        });

        Register<MatrixPaletteChunk>(new()
        {
            LuaClassName = "MatrixPalette",
            PropertyOrder =
            {
                "Matrices",
            }
        });

        // TODO: MemoryImageIndexList - Missing matching Lua file

        // TODO: MemoryImageVertexDescription - Missing matching Lua file

        // TODO: MemoryImageVertexList - Missing matching Lua file

        Register<MeshChunk>(new()
        {
            LuaClassName = "Mesh",
            PropertyOrder =
            {
                "Name",
                "Version",
            }
        });

        // TODO: MeshStats - Missing matching Lua file

        // TODO: MultiColourList - Missing matching Lua file

        Register<MultiController2Chunk>(new()
        {
            LuaClassName = "MultiController2",
            PropertyOrder =
            {
                "Version",
                "Name",
                "CycleMode",
                "NumCycles",
                "InfiniteCycle",
                "NumFrames",
                "FrameRate",
            }
        });

        Register<MultiControllerChunk>(new()
        {
            LuaClassName = "MultiController",
            PropertyOrder =
            {
                "Name",
                "Version",
                "Length",
                "Framerate",
            }
        });

        Register<MultiControllerTrackChunk>(new()
        {
            LuaClassName = "MultiControllerTrack",
            PropertyOrder =
            {
                "Version",
                "Name",
                "Type",
            }
        });

        Register<MultiControllerTracksChunk>(new()
        {
            LuaClassName = "MultiControllerTracks",
            PropertyOrder =
            {
                "Tracks",
            }
        });

        Register<NormalListChunk>(new()
        {
            LuaClassName = "NormalList",
            PropertyOrder =
            {
                "Normals",
            }
        });

        Register<OldBaseEmitterChunk>(new()
        {
            LuaClassName = "OldBaseEmitter",
            PropertyOrder =
            {
                "Version",
                "Name",
                "ParticleType",
                "GeneratorType",
                "ZTest",
                "ZWrite",
                "Fog",
                "MaxParticles",
                "InfiniteLife",
                "RotationalCohesion",
                "TranslationCohesion",
            }
        });

        Register<OldBillboardDisplayInfoChunk>(new()
        {
            LuaClassName = "OldBillboardDisplayInfo",
            PropertyOrder =
            {
                "Version",
                "Rotation",
                "CutOffMode",
                "UVOffsetRange",
                "SourceRange",
                "EdgeRange",
            }
        });

        Register<OldBillboardPerspectiveInfoChunk>(new()
        {
            LuaClassName = "OldBillboardPerspectiveInfo",
            PropertyOrder =
            {
                "Version",
                "PerspectiveScale",
            }
        });

        Register<OldBillboardQuadChunk>(new()
        {
            LuaClassName = "OldBillboardQuad",
            PropertyOrder =
            {
                "Version",
                "Name",
                "BillboardMode",
                "Translation",
                "Colour",
                "UV0",
                "UV1",
                "UV2",
                "UV3",
                "Width",
                "Height",
                "Distance",
                "UVOffset",
            }
        });

        Register<OldBillboardQuadGroupChunk>(new()
        {
            LuaClassName = "OldBillboardQuadGroup",
            PropertyOrder =
            {
                "Version",
                "Name",
                "Shader",
                "ZTest",
                "ZWrite",
                "Occlusion",
            }
        });

        Register<OldColourOffsetListChunk>(new()
        {
            LuaClassName = "OldColourOffsetList",
            PropertyOrder =
            {
                "Version",
                "Offsets",
            }
        });

        Register<OldEmitterAnimationChunk>(new()
        {
            LuaClassName = "OldEmitterAnimation",
            PropertyOrder =
            {
                "Version",
            }
        });

        Register<OldExpressionOffsetsChunk>(new()
        {
            LuaClassName = "OldExpressionOffsets",
            PropertyOrder =
            {
                "PrimitiveGroupIndices",
            }
        });

        // TODO: OldFrameController2 - Missing matching Lua file

        Register<OldFrameControllerChunk>(new()
        {
            LuaClassName = "OldFrameController",
            PropertyOrder =
            {
                "Version",
                "Name",
                "Type",
                "FrameOffset",
                "HierarchyName",
                "AnimationName",
            }
        });

        Register<OldGeneratorAnimationChunk>(new()
        {
            LuaClassName = "OldGeneratorAnimation",
            PropertyOrder =
            {
                "Version",
            }
        });

        Register<OldIndexOffsetListChunk>(new()
        {
            LuaClassName = "OldIndexOffsetList",
            PropertyOrder =
            {
                "Version",
                "Offsets",
            }
        });

        Register<OldLocatorChunk>(new()
        {
            LuaClassName = "Locator3",
            PropertyOrder =
            {
                "Name",
                "Version",
                "Position",
            }
        });

        Register<OldOffsetListChunk>(new()
        {
            LuaClassName = "OldOffsetList",
            PropertyOrder =
            {
                "KeyIndex",
                "Offsets",
                "PrimGroupIndex",
            }
        });

        Register<OldParticleAnimationChunk>(new()
        {
            LuaClassName = "OldParticleAnimation",
            PropertyOrder =
            {
                "Version",
            }
        });

        Register<OldParticleInstancingInfoChunk>(new()
        {
            LuaClassName = "OldParticleInstancingInfo",
            PropertyOrder =
            {
                "Version",
                "MaxInstances",
            }
        });

        Register<OldPrimitiveGroupChunk>(new()
        {
            LuaClassName = "OldPrimitiveGroup",
            PropertyOrder =
            {
                "Version",
                "ShaderName",
                "PrimitiveType",
                "NumVertices",
                "NumIndices",
                "NumMatrices",
            }
        });

        Register<OldScenegraphBranchChunk>(new()
        {
            LuaClassName = "OldScenegraphBranch",
            PropertyOrder =
            {
                "Name",
            }
        });

        Register<OldScenegraphDrawableChunk>(new()
        {
            LuaClassName = "OldScenegraphDrawable",
            PropertyOrder =
            {
                "Name",
                "DrawableName",
                "IsTranslucent",
            }
        });

        Register<OldScenegraphLightGroupChunk>(new()
        {
            LuaClassName = "OldScenegraphLightGroup",
            PropertyOrder =
            {
                "Name",
                "LightGroupName",
            }
        });

        Register<OldScenegraphRootChunk>(new()
        {
            LuaClassName = "OldScenegraphRoot",
            PropertyOrder = []
        });

        Register<OldScenegraphSortOrderChunk>(new()
        {
            LuaClassName = "OldScenegraphSortOrder",
            PropertyOrder =
            {
                "SortOrder",
            }
        });

        Register<OldScenegraphTransformChunk>(new()
        {
            LuaClassName = "OldScenegraphTransform",
            PropertyOrder =
            {
                "Name",
                "Transform",
            }
        });

        Register<OldScenegraphVisibilityChunk>(new()
        {
            LuaClassName = "OldScenegraphVisibility",
            PropertyOrder =
            {
                "Name",
                "IsVisible",
            }
        });

        Register<OldSpriteEmitterChunk>(new()
        {
            LuaClassName = "OldSpriteEmitter",
            PropertyOrder =
            {
                "Version",
                "Name",
                "ShaderName",
                "AngleMode",
                "Angle",
                "TextureAnimMode",
                "NumTextureFrames",
                "TextureFrameRate",
            }
        });

        Register<OldVector2OffsetListChunk>(new()
        {
            LuaClassName = "OldVector2OffsetList",
            PropertyOrder =
            {
                "Version",
                "Offsets",
                "Param",
            }
        });

        Register<OldVectorOffsetListChunk>(new()
        {
            LuaClassName = "OldVectorOffsetList",
            PropertyOrder =
            {
                "Version",
                "Offsets",
                "Param",
            }
        });

        Register<OldVertexAnimKeyFrameChunk>(new()
        {
            LuaClassName = "OldVertexAnimKeyFrame",
            PropertyOrder =
            {
                "Version",
                "Name",
            }
        });

        Register<PackedNormalListChunk>(new()
        {
            LuaClassName = "PackedNormalList",
            PropertyOrder =
            {
                "Normals",
            }
        });

        // TODO: ParticlePlaneGenerator - Missing matching Lua file

        // TODO: ParticlePointGenerator - Missing matching Lua file

        Register<ParticleSystem2Chunk>(new()
        {
            LuaClassName = "ParticleSystem2",
            PropertyOrder =
            {
                "Version",
                "Name",
                "FactoryName",
            }
        });

        // TODO: ParticleSystem - Missing matching Lua file

        Register<ParticleSystemFactoryChunk>(new()
        {
            LuaClassName = "ParticleSystemFactory",
            PropertyOrder =
            {
                "Version",
                "Name",
                "FrameRate",
                "NumAnimFrames",
                "NumOLFrames",
                "CycleAnim",
                "EnableSorting",
            }
        });

        Register<PathChunk>(new()
        {
            LuaClassName = "Path",
            PropertyOrder =
            {
                "Positions",
            }
        });

        // TODO: PhotonMap - Missing matching Lua file

        Register<PhysicsInertiaMatrixChunk>(new()
        {
            LuaClassName = "PhysicsInertiaMatrix",
            PropertyOrder =
            {
                "Matrix",
            }
        });

        Register<PhysicsJointChunk>(new()
        {
            LuaClassName = "PhysicsJoint",
            PropertyOrder =
            {
                "Index",
                "Volume",
                "Stiffness",
                "MaxAngle",
                "MinAngle",
                "DegreesOfFreedom",
            }
        });

        Register<PhysicsObjectChunk>(new()
        {
            LuaClassName = "PhysicsObject",
            PropertyOrder =
            {
                "Name",
                "Version",
                "MaterialName",
                "NumJoints",
                "Volume",
                "RestingSensitivity",
            }
        });

        Register<PhysicsVectorChunk>(new()
        {
            LuaClassName = "PhysicsVector",
            PropertyOrder =
            {
                "Vector",
            }
        });

        Register<PositionListChunk>(new()
        {
            LuaClassName = "PositionList",
            PropertyOrder =
            {
                "Positions",
            }
        });

        // TODO: PrimitiveGroup - Missing matching Lua file

        Register<QuaternionChannelChunk>(new()
        {
            LuaClassName = "QuaternionChannel",
            PropertyOrder =
            {
                "Version",
                "Param",
                "Frames",
                "Values",
            }
        });

        Register<RailCamChunk>(new()
        {
            LuaClassName = "RailCam",
            PropertyOrder =
            {
                "Name",
                "Behaviour",
                "MinRadius",
                "MaxRadius",
                "TrackRail",
                "TrackDist",
                "ReverseSense",
                "FOV",
                "TargetOffset",
                "AxisPlay",
                "PositionLag",
                "TargetLag",
            }
        });

        Register<RenderStatusChunk>(new()
        {
            LuaClassName = "RenderStatus",
            PropertyOrder =
            {
                "CastShadow",
            }
        });

        Register<RoadChunk>(new()
        {
            LuaClassName = "Road",
            PropertyOrder =
            {
                "Name",
                "Type",
                "StartIntersection",
                "EndIntersection",
                "MaximumCars",
                "Speed",
                "Intelligence",
                "Shortcut",
            }
        });

        Register<RoadDataSegmentChunk>(new()
        {
            LuaClassName = "RoadDataSegment",
            PropertyOrder =
            {
                "Name",
                "Type",
                "Lanes",
                "HasShoulder",
                "Direction",
                "Top",
                "Bottom",
            }
        });

        Register<RoadSegmentChunk>(new()
        {
            LuaClassName = "RoadSegment",
            PropertyOrder =
            {
                "Name",
                "RoadDataSegment",
                "Transform",
                "Scale",
            }
        });

        // TODO: ScenegraphBranch - Missing matching Lua file

        Register<ScenegraphChunk>(new()
        {
            LuaClassName = "Scenegraph",
            PropertyOrder =
            {
                "Name",
                "Version",
            }
        });

        // TODO: ScenegraphRoot - Missing matching Lua file

        // TODO: ScenegraphTransform - Missing matching Lua file

        Register<SetChunk>(new()
        {
            LuaClassName = "Set",
            PropertyOrder =
            {
                "Name",
                "Version",
            }
        });

        Register<ShaderChunk>(new()
        {
            LuaClassName = "Shader",
            CustomOverride = chunk =>
            {
                var shaderChunk = (ShaderChunk)chunk;
                return $"{FormatLuaValue(shaderChunk.Name)}, {FormatLuaValue(shaderChunk.Version)}, {FormatLuaValue(shaderChunk.PddiShaderName)}, {FormatLuaValue(shaderChunk.HasTranslucency)}, {FormatLuaValue(shaderChunk.VertexNeeds)}, {FormatLuaValue(~shaderChunk.VertexMask)}";
            }
        });

        Register<ShaderColourParameterChunk>(new()
        {
            LuaClassName = "ShaderColourParameter",
            PropertyOrder =
            {
                "Param",
                "Value",
            }
        });

        // TODO: ShaderDefinition - Missing matching Lua file

        Register<ShaderFloatParameterChunk>(new()
        {
            LuaClassName = "ShaderFloatParameter",
            PropertyOrder =
            {
                "Param",
                "Value",
            }
        });

        Register<ShaderIntegerParameterChunk>(new()
        {
            LuaClassName = "ShaderIntegerParameter",
            PropertyOrder =
            {
                "Param",
                "Value",
            }
        });

        Register<ShaderTextureParameterChunk>(new()
        {
            LuaClassName = "ShaderTextureParameter",
            PropertyOrder =
            {
                "Param",
                "Value",
            }
        });

        // TODO: ShadowMesh - Missing matching Lua file

        // TODO: ShadowSkin - Missing matching Lua file

        // TODO: Skeleton2 - Missing matching Lua file

        Register<SkeletonChunk>(new()
        {
            LuaClassName = "Skeleton",
            PropertyOrder =
            {
                "Name",
                "Version",
            }
        });

        // TODO: SkeletonJoint2 - Missing matching Lua file

        Register<SkeletonJointBonePreserveChunk>(new()
        {
            LuaClassName = "SkeletonJointBonePreserve",
            PropertyOrder =
            {
                "PreserveBoneLengths",
            }
        });

        Register<SkeletonJointChunk>(new()
        {
            LuaClassName = "SkeletonJoint",
            PropertyOrder =
            {
                "Name",
                "Parent",
                "DOF",
                "FreeAxis",
                "PrimaryAxis",
                "SecondaryAxis",
                "TwistAxis",
                "RestPose",
            }
        });

        Register<SkeletonJointMirrorMapChunk>(new()
        {
            LuaClassName = "SkeletonJointMirrorMap",
            PropertyOrder =
            {
                "MappedJointIndex",
                "XAxisMap",
                "YAxisMap",
                "ZAxisMap",
            }
        });

        // TODO: SkeletonPartition - Missing matching Lua file

        Register<SkinChunk>(new()
        {
            LuaClassName = "Skin",
            PropertyOrder =
            {
                "Name",
                "Version",
                "SkeletonName",
            }
        });

        // TODO: SmartProp - Missing matching Lua file

        // TODO: SortOrder - Missing matching Lua file

        Register<SplineChunk>(new()
        {
            LuaClassName = "Spline",
            PropertyOrder =
            {
                "Name",
                "Positions",
            }
        });

        Register<SpriteChunk>(new()
        {
            LuaClassName = "Sprite",
            PropertyOrder =
            {
                "Name",
                "NativeX",
                "NativeY",
                "Shader",
                "ImageWidth",
                "ImageHeight",
                "BlitBorder",
            }
        });

        // TODO: SpriteParticleEmitter - Missing matching Lua file

        Register<StatePropCallbackDataChunk>(new()
        {
            LuaClassName = "StatePropCallbackData",
            PropertyOrder =
            {
                "Name",
                "Event",
                "OnFrame",
            }
        });

        Register<StatePropDataV1Chunk>(new()
        {
            LuaClassName = "StatePropDataV1",
            PropertyOrder =
            {
                "Version",
                "Name",
                "ObjectFactoryName",
            }
        });

        Register<StatePropEventDataChunk>(new()
        {
            LuaClassName = "StatePropEventData",
            PropertyOrder =
            {
                "Name",
                "ToState",
                "Event",
            }
        });

        Register<StatePropFrameControllerDataChunk>(new()
        {
            LuaClassName = "StatePropFrameControllerData",
            PropertyOrder =
            {
                "Name",
                "Cyclic",
                "NumCycles",
                "HoldFrame",
                "MinFrame",
                "MaxFrame",
                "RelativeSpeed",
            }
        });

        Register<StatePropStateDataV1Chunk>(new()
        {
            LuaClassName = "StatePropStateDataV1",
            PropertyOrder =
            {
                "Name",
                "AutoTransition",
                "OutState",
                "OutFrame",
            }
        });

        Register<StatePropVisibilitiesDataChunk>(new()
        {
            LuaClassName = "StatePropVisibilitiesData",
            PropertyOrder =
            {
                "Name",
                "IsVisible",
            }
        });

        Register<StaticEntityChunk>(new()
        {
            LuaClassName = "StaticEntity",
            PropertyOrder =
            {
                "Name",
                "Version",
                "HasAlpha",
            }
        });

        Register<StaticPhysChunk>(new()
        {
            LuaClassName = "StaticPhys",
            PropertyOrder =
            {
                "Name",
                "Version",
            }
        });

        // TODO: TangentList - Missing matching Lua file

        Register<TerrainTypeListChunk>(new()
        {
            LuaClassName = "SurfaceTypeList",
            PropertyOrder =
            {
                "Version",
                "Types",
            }
        });

        Register<TextureAnimationChunk>(new()
        {
            LuaClassName = "TextureAnimation",
            PropertyOrder =
            {
                "Name",
                "Version",
                "MaterialName",
                "NumFrames",
                "FrameRate",
                "Cyclic",
            }
        });

        Register<TextureChunk>(new()
        {
            LuaClassName = "Texture",
            PropertyOrder =
            {
                "Name",
                "Version",
                "Width",
                "Height",
                "Bpp",
                "AlphaDepth",
                "NumMipMaps",
                "TextureType",
                "UsageHint",
                "Priority",
            }
        });

        Register<TextureFontChunk>(new()
        {
            LuaClassName = "TextureFont",
            PropertyOrder =
            {
                "Version",
                "Name",
                "Shader",
                "FontSize",
                "FontWidth",
                "FontHeight",
                "FontBaseLine",
            }
        });

        Register<TextureGlyphListChunk>(new()
        {
            LuaClassName = "TextureGlyphList",
            PropertyOrder =
            {
                "Glyphs",
            }
        });

        // TODO: Topology - Missing matching Lua file

        Register<TreeChunk>(new()
        {
            LuaClassName = "Tree",
            PropertyOrder =
            {
                "Minimum",
                "Maximum",
            }
        });

        Register<TreeNode2Chunk>(new()
        {
            LuaClassName = "TreeNode2",
            PropertyOrder =
            {
                "SplitAxis",
                "SplitPosition",
                "StaticEntityLimit",
                "StaticPhysEntityLimit",
                "IntersectEntityLimit",
                "DynaPhysEntityLimit",
                "FenceEntityLimit",
                "RoadSegmentEntityLimit",
                "PathSegmentEntityLimit",
                "AnimEntityLimit",
            }
        });

        Register<TreeNodeChunk>(new()
        {
            LuaClassName = "TreeNode",
            PropertyOrder =
            {
                "NumChildren",
                "ParentOffset",
            }
        });

        Register<TriggerVolumeChunk>(new()
        {
            LuaClassName = "TriggerVolume",
            PropertyOrder =
            {
                "Name",
                "Type",
                "HalfExtents",
                "Matrix",
            }
        });

        Register<UVListChunk>(new()
        {
            LuaClassName = "UVList",
            PropertyOrder =
            {
                "Channel",
                "UVs",
            }
        });

        Register<Vector1DOFChannelChunk>(new()
        {
            LuaClassName = "Vector1DOFChannel",
            PropertyOrder =
            {
                "Version",
                "Param",
                "DynamicIndex",
                "Constants",
                "Frames",
                "Values",
            }
        });

        Register<Vector2DOFChannelChunk>(new()
        {
            LuaClassName = "Vector2DOFChannel",
            PropertyOrder =
            {
                "Version",
                "Param",
                "StaticIndex",
                "Constants",
                "Frames",
                "Values",
            }
        });

        // TODO: Vector2OffsetList - Missing matching Lua file

        Register<Vector3DOFChannelChunk>(new()
        {
            LuaClassName = "Vector3DOFChannel",
            PropertyOrder =
            {
                "Version",
                "Param",
                "Frames",
                "Values",
            }
        });

        // TODO: VertexAnimKeyFrame - Missing matching Lua file

        // TODO: VertexAnimKeyFrameList - Missing matching Lua file

        // TODO: VertexCompressionHint - Missing matching Lua file

        Register<VertexShaderChunk>(new()
        {
            LuaClassName = "VertexShader",
            PropertyOrder =
            {
                "Name",
            }
        });

        // TODO: VisibilityAnimChannel - Missing matching Lua file

        // TODO: VisibilityAnim - Missing matching Lua file

        Register<VolumeImageChunk>(new()
        {
            LuaClassName = "VolumeImage",
            PropertyOrder =
            {
                "Name",
                "Version",
                "Width",
                "Height",
                "Depth",
                "Bpp",
                "Palettized",
                "HasAlpha",
                "Format",
            }
        });

        Register<WalkerCameraDataChunk>(new()
        {
            LuaClassName = "WalkerCameraData",
            PropertyOrder =
            {
                "Index",
                "MinMagnitude",
                "MaxMagnitude",
                "Elevation",
                "TargetOffset",
            }
        });

        Register<WallChunk>(new()
        {
            LuaClassName = "Fence2",
            PropertyOrder =
            {
                "Start",
                "End",
                "Normal",
            }
        });

        Register<WeightListChunk>(new()
        {
            LuaClassName = "WeightList",
            PropertyOrder =
            {
                "Weights",
            }
        });

        // TODO: WorldCollisionObject - Missing matching Lua file

        Register<WorldSphereChunk>(new()
        {
            LuaClassName = "WorldSphere",
            PropertyOrder =
            {
                "Name",
                "Version",
            }
        });

        // TODO: Unknown7000008 - Missing matching Lua file
    }

    public static void Register<TChunk>(LuaChunkMapping mapping) where TChunk : Chunk => _mappings[typeof(TChunk)] = mapping;

    public static string GetLuaConstructor(Chunk chunk)
    {
        var type = chunk.GetType();

        if (!_mappings.TryGetValue(type, out var mapping))
            throw new NotSupportedException($"Chunk {chunk} is not supported.");

        if (mapping.CustomOverride != null)
            return $"P3D.{mapping.LuaClassName}P3DChunk({mapping.CustomOverride(chunk)})";

        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(static p => p.Name, static p => p);

        object?[] args = new object[mapping.PropertyOrder.Count];

        for (int i = 0; i < mapping.PropertyOrder.Count; i++)
        {
            var prop = mapping.PropertyOrder[i];

            if (!properties.TryGetValue(prop, out var propertyInfo))
                throw new InvalidOperationException($"Could not find property with name {prop} in {chunk}.");

            args[i] = propertyInfo.GetValue(chunk);
        }

        var luaArgs = string.Join(", ", args.Select(FormatLuaValue));

        return $"P3D.{mapping.LuaClassName}P3DChunk({luaArgs})";
    }

    private static string FormatLuaValue(object? value)
    {
        if (value is null)
            return "nil";

        if (value is AnimationType animationType)
            return $"\"{EscapeLuaString(animationType.ToFourCC())}\"";

        if (value is Enum e)
        {
            var underlyingType = Enum.GetUnderlyingType(e.GetType());

            if (underlyingType == typeof(sbyte) || underlyingType == typeof(short) || underlyingType == typeof(int) || underlyingType == typeof(long))
                return Convert.ToInt64(e).ToString(CultureInfo.InvariantCulture);

            return Convert.ToUInt64(e).ToString(CultureInfo.InvariantCulture);
        }

        if (value is string str)
            return $"\"{EscapeLuaString(str)}\"";

        if (value is IEnumerable enumerable)
        {
            var elements = new List<string>();

            foreach (var item in enumerable)
                elements.Add(FormatLuaValue(item));

            return $"{{{string.Join(", ", elements)}}}";
        }

        return value switch
        {
            char c => $"\"{c}\"",
            byte b => b.ToString(CultureInfo.InvariantCulture),
            sbyte sb => sb.ToString(CultureInfo.InvariantCulture),
            short s => s.ToString(CultureInfo.InvariantCulture),
            ushort us => us.ToString(CultureInfo.InvariantCulture),
            int i => i.ToString(CultureInfo.InvariantCulture),
            uint ui => ui.ToString(CultureInfo.InvariantCulture),
            long l => l.ToString(CultureInfo.InvariantCulture),
            ulong ul => ul.ToString(CultureInfo.InvariantCulture),
            float f => f.ToString(CultureInfo.InvariantCulture),
            double d => d.ToString(CultureInfo.InvariantCulture),
            Vector2 v => FormattableString.Invariant($"P3D.Vector2({v.X}, {v.Y})"),
            Vector3 v => FormattableString.Invariant($"P3D.Vector3({v.X}, {v.Y}, {v.Z})"),
            Quaternion q => FormattableString.Invariant($"P3D.Quaternion({q.W}, {q.X}, {q.Y}, {q.Z})"),
            Matrix4x4 m => FormattableString.Invariant($"P3D.Matrix({m.M11}, {m.M12}, {m.M13}, {m.M14}, {m.M21}, {m.M22}, {m.M23}, {m.M24}, {m.M31}, {m.M32}, {m.M33}, {m.M34}, {m.M41}, {m.M42}, {m.M43}, {m.M44})"),
            SymmetricMatrix3x3 m => FormattableString.Invariant($"P3D.SymmetricMatrix3x3({m.XX}, {m.XY}, {m.XZ}, {m.YY}, {m.YZ}, {m.ZZ})"),
            Color c => FormattableString.Invariant($"P3D.Colour({c.R}, {c.G}, {c.B}, {c.A})"),
            bool b => b ? "1" : "0",
            ATCChunk.Entry entry => FormattableString.Invariant($"{{SoundResourceDataName = \"{EscapeLuaString(entry.SoundResourceDataName)}\", Particle = \"{EscapeLuaString(entry.Particle)}\", BreakableObject = \"{EscapeLuaString(entry.BreakableObject)}\", Friction = {entry.Friction}, Mass = {entry.Mass}, Elasticity = {entry.Elasticity}}}"),
            FrontendLanguageChunk.Entry entry => FormattableString.Invariant($"{{Hash = {entry.Hash}, Value = \"{EscapeLuaString(entry.Value)}\"}}"),
            MatrixListChunk.Matrix matrix => FormattableString.Invariant($"P3D.Colour({matrix.B}, {matrix.C}, {matrix.D}, {matrix.A})"),
            MultiControllerTracksChunk.Track track => FormattableString.Invariant($"{{Name = \"{EscapeLuaString(track.Name)}\", StartTime = {track.StartTime}, EndTime = {track.EndTime}, Scale = {track.Scale}}}"),
            OldOffsetListChunk.OffsetEntry entry => FormattableString.Invariant($"{{Index = {entry.Index}, Offset = {FormatLuaValue(entry.Offset)}}}"),
            TextureGlyphListChunk.Glyph glyph => FormattableString.Invariant($"{{TextureNum = {glyph.TextureNum}, BottomLeft = {FormatLuaValue(glyph.BottomLeft)}, TopRight = {FormatLuaValue(glyph.TopRight)}, LeftBearing = {glyph.LeftBearing}, RightBearing = {glyph.RightBearing}, Width = {glyph.Width}, Advance = {glyph.Advance}, Code = {glyph.Code}}}"),
            _ => throw new NotSupportedException($"Lua serialization not supported for {value.GetType().Name}")
        };
    }

    private static string EscapeLuaString(string s)
    {
        var sb = new System.Text.StringBuilder();
        foreach (var c in s)
        {
            switch (c)
            {
                case '\\': sb.Append("\\\\"); break;
                case '\"': sb.Append("\\\""); break;
                case '\n': sb.Append("\\n"); break;
                case '\r': sb.Append("\\r"); break;
                case '\t': sb.Append("\\t"); break;
                case '\0': sb.Append("\\0"); break;
                default:
                    if (char.IsControl(c) || c > 127)
                        sb.AppendFormat("\\x{0:X2}", (int)c);
                    else
                        sb.Append(c);
                    break;
            }
        }
        return sb.ToString();
    }

    public static async Task ProcessChunksAsync(IProgress<int>? progress, StringBuilder sb, string parent, Collection<Chunk> chunks, int indent = 0)
    {
        foreach (var chunk in chunks)
        {
            try
            {
                var constructor = GetLuaConstructor(chunk);
                progress?.Report(1);
                await Task.Yield();

                if (chunk.Children.Count == 0)
                {
                    sb.AddIndent(indent);
                    sb.AppendLine($"{parent}:AddChunk({constructor})");
                    continue;
                }

                sb.AddIndent(indent++);
                sb.AppendLine("do");

                sb.AddIndent(indent);
                sb.AppendLine($"local Chunk{indent} = {constructor}");
                sb.AddIndent(indent);
                sb.AppendLine($"{parent}:AddChunk(Chunk{indent})");
                sb.AppendLine();

                await ProcessChunksAsync(progress, sb, $"Chunk{indent}", chunk.Children, indent);

                sb.AddIndent(--indent);
                sb.AppendLine("end");
            }
            catch (Exception ex)
            {
                sb.AddIndent(indent);
                sb.AppendLine($"-- Error in {chunk}: {ex.Message}");
            }
        }
    }
}

internal sealed class LuaChunkMapping
{
    public string LuaClassName { get; init; } = null!;
    public List<string> PropertyOrder { get; init; } = [];
    public Func<Chunk, string>? CustomOverride { get; init; }
}
