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
            PropertyOrder = {
                nameof(AnimatedObjectAnimationChunk.Version),
                nameof(AnimatedObjectAnimationChunk.Name),
                nameof(AnimatedObjectAnimationChunk.FrameRate),
                nameof(AnimatedObjectAnimationChunk.NumOldFrameControllers),
            }
        });

        Register<AnimatedObjectChunk>(new()
        {
            LuaClassName = "AnimatedObject",
            PropertyOrder = {
                nameof(AnimatedObjectChunk.Version),
                nameof(AnimatedObjectChunk.Name),
                nameof(AnimatedObjectChunk.FactoryName),
                nameof(AnimatedObjectChunk.StartingAnimation),
            }
        });

        Register<AnimatedObjectFactoryChunk>(new()
        {
            LuaClassName = "AnimatedObjectFactory",
            PropertyOrder = {
                nameof(AnimatedObjectFactoryChunk.Version),
                nameof(AnimatedObjectFactoryChunk.Name),
                nameof(AnimatedObjectFactoryChunk.BaseAnimation),
                nameof(AnimatedObjectFactoryChunk.NumAnimations),
            }
        });

        Register<AnimationChannelCountChunk>(new()
        {
            LuaClassName = "AnimationChannelCount",
            PropertyOrder = {
                nameof(AnimationChannelCountChunk.Version),
                nameof(AnimationChannelCountChunk.ChannelChunkID),
                nameof(AnimationChannelCountChunk.NumKeys),
            }
        });

        Register<AnimationChunk>(new()
        {
            LuaClassName = "Animation",
            PropertyOrder = {
                nameof(AnimationChunk.Version),
                nameof(AnimationChunk.Name),
                nameof(AnimationChunk.AnimationType),
                nameof(AnimationChunk.NumFrames),
                nameof(AnimationChunk.FrameRate),
                nameof(AnimationChunk.Cyclic),
            }
        });

        Register<AnimationGroupChunk>(new()
        {
            LuaClassName = "AnimationGroup",
            PropertyOrder = {
                nameof(AnimationGroupChunk.Version),
                nameof(AnimationGroupChunk.Name),
                nameof(AnimationGroupChunk.GroupID),
            }
        });

        Register<AnimationGroupListChunk>(new()
        {
            LuaClassName = "AnimationGroupList",
            PropertyOrder = {
                nameof(AnimationGroupListChunk.Version),
            }
        });

        Register<AnimationHeaderChunk>(new()
        {
            LuaClassName = "AnimationHeader",
            PropertyOrder = {
                nameof(AnimationHeaderChunk.Version),
                nameof(AnimationHeaderChunk.NumGroups),
            }
        });

        Register<AnimationSizeChunk>(new()
        {
            LuaClassName = "AnimationSize",
            PropertyOrder = {
                nameof(AnimationSizeChunk.Version),
                nameof(AnimationSizeChunk.PC),
                nameof(AnimationSizeChunk.PS2),
                nameof(AnimationSizeChunk.XBOX),
                nameof(AnimationSizeChunk.GC),
            }
        });

        // TODO: AnimationSyncFrame - Missing matching Lua file

        Register<AnimChunk>(new()
        {
            LuaClassName = "Anim",
            PropertyOrder = {
                nameof(AnimChunk.Name),
                nameof(AnimChunk.Version),
                nameof(AnimChunk.HasAlpha),
            }
        });

        Register<AnimCollChunk>(new()
        {
            LuaClassName = "AnimColl",
            PropertyOrder = {
                nameof(AnimCollChunk.Name),
                nameof(AnimCollChunk.Version),
                nameof(AnimCollChunk.HasAlpha),
            }
        });

        Register<AnimDynaPhysChunk>(new()
        {
            LuaClassName = "AnimDynaPhys",
            PropertyOrder = {
                nameof(AnimDynaPhysChunk.Name),
                nameof(AnimDynaPhysChunk.Version),
                nameof(AnimDynaPhysChunk.HasAlpha),
            }
        });

        Register<AnimDynaPhysWrapperChunk>(new()
        {
            LuaClassName = "AnimDynaPhysWrapper",
            PropertyOrder = {
                nameof(AnimDynaPhysWrapperChunk.Name),
                nameof(AnimDynaPhysWrapperChunk.Version),
                nameof(AnimDynaPhysWrapperChunk.HasAlpha),
            }
        });

        Register<AnimObjWrapperChunk>(new()
        {
            LuaClassName = "AnimObjWrapper",
            PropertyOrder = {
                nameof(AnimObjWrapperChunk.Name),
                nameof(AnimObjWrapperChunk.Version),
                nameof(AnimObjWrapperChunk.HasAlpha),
            }
        });

        Register<ATCChunk>(new()
        {
            LuaClassName = "ATC",
            PropertyOrder = {
                nameof(ATCChunk.Entries),
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
            PropertyOrder = {
                nameof(BooleanChannelChunk.Version),
                nameof(BooleanChannelChunk.Param),
                nameof(BooleanChannelChunk.StartState),
                nameof(BooleanChannelChunk.Values),
            }
        });

        Register<BoundingBoxChunk>(new()
        {
            LuaClassName = "BoundingBox",
            PropertyOrder = {
                nameof(BoundingBoxChunk.Low),
                nameof(BoundingBoxChunk.High),
            }
        });

        Register<BoundingSphereChunk>(new()
        {
            LuaClassName = "BoundingSphere",
            PropertyOrder = {
                nameof(BoundingSphereChunk.Centre),
                nameof(BoundingSphereChunk.Radius),
            }
        });

        Register<BreakableObjectChunk>(new()
        {
            LuaClassName = "BreakableObject",
            PropertyOrder = {
                nameof(BreakableObjectChunk.Index),
                nameof(BreakableObjectChunk.MaxInstances),
            }
        });

        Register<CameraChunk>(new()
        {
            LuaClassName = "Camera",
            PropertyOrder = {
                nameof(CameraChunk.Name),
                nameof(CameraChunk.Version),
                nameof(CameraChunk.FOV),
                nameof(CameraChunk.AspectRatio),
                nameof(CameraChunk.NearClip),
                nameof(CameraChunk.FarClip),
                nameof(CameraChunk.Position),
                nameof(CameraChunk.Look),
                nameof(CameraChunk.Up),
            }
        });

        Register<ChannelInterpolationModeChunk>(new()
        {
            LuaClassName = "ChannelInterpolationMode",
            PropertyOrder = {
                nameof(ChannelInterpolationModeChunk.Version),
                nameof(ChannelInterpolationModeChunk.Interpolate),
            }
        });

        Register<CollisionAxisAlignedBoundingBoxChunk>(new()
        {
            LuaClassName = "CollisionAxisAlignedBoundingBox",
            PropertyOrder = {
                nameof(CollisionAxisAlignedBoundingBoxChunk.Dummy),
            }
        });

        Register<CollisionCylinderChunk>(new()
        {
            LuaClassName = "CollisionCylinder",
            PropertyOrder = {
                nameof(CollisionCylinderChunk.Radius),
                nameof(CollisionCylinderChunk.HalfLength),
                nameof(CollisionCylinderChunk.FlatEnd),
            }
        });

        Register<CollisionEffectChunk>(new()
        {
            LuaClassName = "CollisionEffect",
            PropertyOrder = {
                nameof(CollisionEffectChunk.ClassType),
                nameof(CollisionEffectChunk.PhysPropID),
                nameof(CollisionEffectChunk.SoundResourceDataName),
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
            PropertyOrder = {
                nameof(CollisionObjectAttributeChunk.IsStatic),
                nameof(CollisionObjectAttributeChunk.DefaultArea),
                nameof(CollisionObjectAttributeChunk.CanRoll),
                nameof(CollisionObjectAttributeChunk.CanSlide),
                nameof(CollisionObjectAttributeChunk.CanSpin),
                nameof(CollisionObjectAttributeChunk.CanBounce),
                nameof(CollisionObjectAttributeChunk.ExtraAttribute1),
                nameof(CollisionObjectAttributeChunk.ExtraAttribute2),
                nameof(CollisionObjectAttributeChunk.ExtraAttribute3),
            }
        });

        Register<CollisionObjectChunk>(new()
        {
            LuaClassName = "CollisionObject",
            PropertyOrder = {
                nameof(CollisionObjectChunk.Name),
                nameof(CollisionObjectChunk.Version),
                nameof(CollisionObjectChunk.MaterialName),
                nameof(CollisionObjectChunk.NumSubObjects),
            }
        });

        Register<CollisionOrientedBoundingBoxChunk>(new()
        {
            LuaClassName = "CollisionOrientedBoundingBox",
            PropertyOrder = {
                nameof(CollisionOrientedBoundingBoxChunk.HalfExtents),
            }
        });

        Register<CollisionSphereChunk>(new()
        {
            LuaClassName = "CollisionSphere",
            PropertyOrder = {
                nameof(CollisionSphereChunk.Radius),
            }
        });

        Register<CollisionVectorChunk>(new()
        {
            LuaClassName = "CollisionVector",
            PropertyOrder = {
                nameof(CollisionVectorChunk.Vector),
            }
        });

        Register<CollisionVolumeChunk>(new()
        {
            LuaClassName = "CollisionVolume",
            PropertyOrder = {
                nameof(CollisionVolumeChunk.ObjectReferenceIndex),
                nameof(CollisionVolumeChunk.OwnerIndex),
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
            PropertyOrder = {
                nameof(CollisionVolumeOwnerNameChunk.Name),
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
            PropertyOrder = {
                nameof(ColourChannelChunk.Version),
                nameof(ColourChannelChunk.Param),
                nameof(ColourChannelChunk.Frames),
                nameof(ColourChannelChunk.Values),
            }
        });

        Register<ColourListChunk>(new()
        {
            LuaClassName = "ColourList",
            PropertyOrder = {
                nameof(ColourListChunk.Colours),
            }
        });

        // TODO: CompositeDrawable2 - Missing matching Lua file

        Register<CompositeDrawableChunk>(new()
        {
            LuaClassName = "CompositeDrawable",
            PropertyOrder = {
                nameof(CompositeDrawableChunk.Name),
                nameof(CompositeDrawableChunk.SkeletonName),
            }
        });

        Register<CompositeDrawableEffectChunk>(new()
        {
            LuaClassName = "CompositeDrawableEffect",
            PropertyOrder = {
                nameof(CompositeDrawableEffectChunk.Name),
                nameof(CompositeDrawableEffectChunk.IsTranslucent),
                nameof(CompositeDrawableEffectChunk.SkeletonJointId),
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
            PropertyOrder = {
                nameof(CompositeDrawablePropChunk.Name),
                nameof(CompositeDrawablePropChunk.IsTranslucent),
                nameof(CompositeDrawablePropChunk.SkeletonJointId),
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
            PropertyOrder = {
                nameof(CompositeDrawableSkinChunk.Name),
                nameof(CompositeDrawableSkinChunk.IsTranslucent),
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
            PropertyOrder = {
                nameof(CompositeDrawableSortOrderChunk.SortOrder),
            }
        });

        Register<CompressedQuaternionChannel2Chunk>(new()
        {
            LuaClassName = "CompressedQuaternionChannel2",
            PropertyOrder = {
                nameof(CompressedQuaternionChannel2Chunk.Version),
                nameof(CompressedQuaternionChannel2Chunk.Param),
                nameof(CompressedQuaternionChannel2Chunk.Frames),
                nameof(CompressedQuaternionChannel2Chunk.Values),
            }
        });

        Register<CompressedQuaternionChannelChunk>(new()
        {
            LuaClassName = "CompressedQuaternionChannel",
            PropertyOrder = {
                nameof(CompressedQuaternionChannelChunk.Version),
                nameof(CompressedQuaternionChannelChunk.Param),
                nameof(CompressedQuaternionChannelChunk.Frames),
                nameof(CompressedQuaternionChannelChunk.Values),
            }
        });

        Register<DynaPhysChunk>(new()
        {
            LuaClassName = "DynaPhys",
            PropertyOrder = {
                nameof(DynaPhysChunk.Name),
                nameof(DynaPhysChunk.Version),
                nameof(DynaPhysChunk.HasAlpha),
            }
        });

        Register<EntityChannelChunk>(new()
        {
            LuaClassName = "EntityChannel",
            PropertyOrder = {
                nameof(EntityChannelChunk.Version),
                nameof(EntityChannelChunk.Param),
                nameof(EntityChannelChunk.Frames),
                nameof(EntityChannelChunk.Values),
            }
        });

        Register<ExportInfoChunk>(new()
        {
            LuaClassName = "ExportInfo",
            PropertyOrder = {
                nameof(ExportInfoChunk.Name),
            }
        });

        Register<ExportInfoNamedIntegerChunk>(new()
        {
            LuaClassName = "ExportInfoNamedInteger",
            PropertyOrder = {
                nameof(ExportInfoNamedIntegerChunk.Name),
                nameof(ExportInfoNamedIntegerChunk.Value),
            }
        });

        Register<ExportInfoNamedStringChunk>(new()
        {
            LuaClassName = "ExportInfoNamedString",
            PropertyOrder = {
                nameof(ExportInfoNamedStringChunk.Name),
                nameof(ExportInfoNamedStringChunk.Value),
            }
        });

        Register<ExpressionChunk>(new()
        {
            LuaClassName = "Expression",
            PropertyOrder = {
                nameof(ExpressionChunk.Version),
                nameof(ExpressionChunk.Name),
                nameof(ExpressionChunk.Keys),
                nameof(ExpressionChunk.Indices),
            }
        });

        Register<ExpressionGroupChunk>(new()
        {
            LuaClassName = "ExpressionGroup",
            PropertyOrder = {
                nameof(ExpressionGroupChunk.Version),
                nameof(ExpressionGroupChunk.Name),
                nameof(ExpressionGroupChunk.TargetName),
                nameof(ExpressionGroupChunk.Stages),
            }
        });

        Register<ExpressionMixerChunk>(new()
        {
            LuaClassName = "ExpressionMixer",
            PropertyOrder = {
                nameof(ExpressionMixerChunk.Version),
                nameof(ExpressionMixerChunk.Name),
                nameof(ExpressionMixerChunk.Type),
                nameof(ExpressionMixerChunk.TargetName),
                nameof(ExpressionMixerChunk.ExpressionGroupName),
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
            PropertyOrder = {
                nameof(Float1ChannelChunk.Version),
                nameof(Float1ChannelChunk.Param),
                nameof(Float1ChannelChunk.Frames),
                nameof(Float1ChannelChunk.Values),
            }
        });

        Register<Float2ChannelChunk>(new()
        {
            LuaClassName = "Float2Channel",
            PropertyOrder = {
                nameof(Float2ChannelChunk.Version),
                nameof(Float2ChannelChunk.Param),
                nameof(Float2ChannelChunk.Frames),
                nameof(Float2ChannelChunk.Values),
            }
        });

        Register<FollowCameraDataChunk>(new()
        {
            LuaClassName = "FollowCameraData",
            PropertyOrder = {
                nameof(FollowCameraDataChunk.Index),
                nameof(FollowCameraDataChunk.Rotation),
                nameof(FollowCameraDataChunk.Elevation),
                nameof(FollowCameraDataChunk.Magnitude),
                nameof(FollowCameraDataChunk.TargetOffset),
            }
        });

        Register<FrameControllerChunk>(new()
        {
            LuaClassName = "FrameController",
            PropertyOrder = {
                nameof(FrameControllerChunk.Version),
                nameof(FrameControllerChunk.Name),
                nameof(FrameControllerChunk.Type),
                nameof(FrameControllerChunk.CycleMode),
                nameof(FrameControllerChunk.NumCycles),
                nameof(FrameControllerChunk.InfiniteCycle),
                nameof(FrameControllerChunk.HierarchyName),
                nameof(FrameControllerChunk.AnimationName),
            }
        });

        // TODO: FrameControllerList - Missing matching Lua file

        // TODO: FrontendGroup2 - Missing matching Lua file

        Register<FrontendGroupChunk>(new()
        {
            LuaClassName = "FrontendGroup",
            PropertyOrder = {
                nameof(FrontendGroupChunk.Name),
                nameof(FrontendGroupChunk.Version),
                nameof(FrontendGroupChunk.Alpha),
            }
        });

        Register<FrontendImageResourceChunk>(new()
        {
            LuaClassName = "FrontendImageResource",
            PropertyOrder = {
                nameof(FrontendImageResourceChunk.Name),
                nameof(FrontendImageResourceChunk.Version),
                nameof(FrontendImageResourceChunk.Filename),
            }
        });

        Register<FrontendLanguageChunk>(new()
        {
            LuaClassName = "FrontendLanguage",
            PropertyOrder = {
                nameof(FrontendLanguageChunk.Name),
                nameof(FrontendLanguageChunk.Language),
                nameof(FrontendLanguageChunk.Modulo),
                nameof(FrontendLanguageChunk.Entries),
            }
        });

        // TODO: FrontendLayer2 - Missing matching Lua file

        Register<FrontendLayerChunk>(new()
        {
            LuaClassName = "FrontendLayer",
            PropertyOrder = {
                nameof(FrontendLayerChunk.Name),
                nameof(FrontendLayerChunk.Version),
                nameof(FrontendLayerChunk.Visible),
                nameof(FrontendLayerChunk.Editable),
                nameof(FrontendLayerChunk.Alpha),
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
            PropertyOrder = {
                nameof(FrontendPolygonChunk.Name),
                nameof(FrontendPolygonChunk.Version),
                nameof(FrontendPolygonChunk.Translucency),
                nameof(FrontendPolygonChunk.Points),
                nameof(FrontendPolygonChunk.Colours),
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
            PropertyOrder = {
                nameof(FrontendPure3DResourceChunk.Name),
                nameof(FrontendPure3DResourceChunk.Version),
                nameof(FrontendPure3DResourceChunk.Filename),
                nameof(FrontendPure3DResourceChunk.InventoryName),
                nameof(FrontendPure3DResourceChunk.CameraName),
                nameof(FrontendPure3DResourceChunk.AnimationName),
            }
        });

        Register<FrontendScreenChunk>(new()
        {
            LuaClassName = "FrontendScreen",
            PropertyOrder = {
                nameof(FrontendScreenChunk.Name),
                nameof(FrontendScreenChunk.Version),
                nameof(FrontendScreenChunk.PageNames),
            }
        });

        Register<FrontendStringHardCodedChunk>(new()
        {
            LuaClassName = "FrontendStringHardCoded",
            PropertyOrder = {
                nameof(FrontendStringHardCodedChunk.String),
            }
        });

        Register<FrontendStringTextBibleChunk>(new()
        {
            LuaClassName = "FrontendStringTextBible",
            PropertyOrder = {
                nameof(FrontendStringTextBibleChunk.BibleName),
                nameof(FrontendStringTextBibleChunk.StringID),
            }
        });

        Register<FrontendTextBibleChunk>(new()
        {
            LuaClassName = "FrontendTextBible",
            PropertyOrder = {
                nameof(FrontendTextBibleChunk.Name),
            }
        });

        Register<FrontendTextBibleResourceChunk>(new()
        {
            LuaClassName = "FrontendTextBibleResource",
            PropertyOrder = {
                nameof(FrontendTextBibleResourceChunk.Name),
                nameof(FrontendTextBibleResourceChunk.Version),
                nameof(FrontendTextBibleResourceChunk.Filename),
                nameof(FrontendTextBibleResourceChunk.InventoryName),
            }
        });

        Register<FrontendTextStyleResourceChunk>(new()
        {
            LuaClassName = "FrontendTextStyleResource",
            PropertyOrder = {
                nameof(FrontendTextStyleResourceChunk.Name),
                nameof(FrontendTextStyleResourceChunk.Version),
                nameof(FrontendTextStyleResourceChunk.Filename),
                nameof(FrontendTextStyleResourceChunk.InventoryName),
            }
        });

        Register<GameAttrChunk>(new()
        {
            LuaClassName = "GameAttr",
            PropertyOrder = {
                nameof(GameAttrChunk.Name),
                nameof(GameAttrChunk.Version),
            }
        });

        Register<GameAttributeColourParameterChunk>(new()
        {
            LuaClassName = "GameAttributeColourParameter",
            PropertyOrder = {
                nameof(GameAttributeColourParameterChunk.Name),
                nameof(GameAttributeColourParameterChunk.Value),
            }
        });

        Register<GameAttributeFloatParameterChunk>(new()
        {
            LuaClassName = "GameAttributeFloatParameter",
            PropertyOrder = {
                nameof(GameAttributeFloatParameterChunk.Name),
                nameof(GameAttributeFloatParameterChunk.Value),
            }
        });

        Register<GameAttributeIntegerParameterChunk>(new()
        {
            LuaClassName = "GameAttributeIntegerParameter",
            PropertyOrder = {
                nameof(GameAttributeIntegerParameterChunk.Name),
                nameof(GameAttributeIntegerParameterChunk.Value),
            }
        });

        Register<GameAttributeMatrixParameterChunk>(new()
        {
            LuaClassName = "GameAttributeMatrixParameter",
            PropertyOrder = {
                nameof(GameAttributeMatrixParameterChunk.Name),
                nameof(GameAttributeMatrixParameterChunk.Value),
            }
        });

        Register<GameAttributeVectorParameterChunk>(new()
        {
            LuaClassName = "GameAttributeVectorParameter",
            PropertyOrder = {
                nameof(GameAttributeVectorParameterChunk.Name),
                nameof(GameAttributeVectorParameterChunk.Value),
            }
        });

        Register<HistoryChunk>(new()
        {
            LuaClassName = "History",
            PropertyOrder = {
                nameof(HistoryChunk.History),
            }
        });

        Register<ImageChunk>(new()
        {
            LuaClassName = "Image",
            PropertyOrder = {
                nameof(ImageChunk.Name),
                nameof(ImageChunk.Version),
                nameof(ImageChunk.Width),
                nameof(ImageChunk.Height),
                nameof(ImageChunk.Bpp),
                nameof(ImageChunk.Palettized),
                nameof(ImageChunk.HasAlpha),
                nameof(ImageChunk.Format),
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
                foreach (var b in imageDataChunk.ImageData.ToArray())
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
            PropertyOrder = {
                nameof(IndexListChunk.Indices),
            }
        });

        Register<InstanceListChunk>(new()
        {
            LuaClassName = "InstanceList",
            PropertyOrder = {
                nameof(InstanceListChunk.Name),
            }
        });

        Register<InstParticleSystemChunk>(new()
        {
            LuaClassName = "InstParticleSystem",
            PropertyOrder = {
                nameof(InstParticleSystemChunk.ParticleType),
                nameof(InstParticleSystemChunk.MaxInstances),
            }
        });

        Register<InstStatEntityChunk>(new()
        {
            LuaClassName = "InstStatEntity",
            PropertyOrder = {
                nameof(InstStatEntityChunk.Name),
                nameof(InstStatEntityChunk.Version),
                nameof(InstStatEntityChunk.HasAlpha),
            }
        });

        Register<InstStatPhysChunk>(new()
        {
            LuaClassName = "InstStatPhys",
            PropertyOrder = {
                nameof(InstStatPhysChunk.Name),
                nameof(InstStatPhysChunk.Version),
                nameof(InstStatPhysChunk.HasAlpha),
            }
        });

        Register<IntegerChannelChunk>(new()
        {
            LuaClassName = "IntegerChannel",
            PropertyOrder = {
                nameof(IntegerChannelChunk.Version),
                nameof(IntegerChannelChunk.Param),
                nameof(IntegerChannelChunk.Frames),
                nameof(IntegerChannelChunk.Values),
            }
        });

        Register<IntersectChunk>(new()
        {
            LuaClassName = "Intersect",
            PropertyOrder = {
                nameof(IntersectChunk.Indices),
                nameof(IntersectChunk.Positions),
                nameof(IntersectChunk.Normals),
            }
        });

        Register<IntersectionChunk>(new()
        {
            LuaClassName = "Intersection",
            PropertyOrder = {
                nameof(IntersectionChunk.Name),
                nameof(IntersectionChunk.Position),
                nameof(IntersectionChunk.Radius),
                nameof(IntersectionChunk.TrafficBehaviour),
            }
        });

        Register<IntersectMesh2Chunk>(new()
        {
            LuaClassName = "IntersectMesh2",
            PropertyOrder = {
                nameof(IntersectMesh2Chunk.SurfaceType),
            }
        });

        Register<IntersectMeshChunk>(new()
        {
            LuaClassName = "IntersectMesh",
            PropertyOrder = {
                nameof(IntersectMeshChunk.Name),
            }
        });

        Register<LensFlareChunk>(new()
        {
            LuaClassName = "LensFlare",
            PropertyOrder = {
                nameof(LensFlareChunk.Name),
                nameof(LensFlareChunk.Version),
            }
        });

        // TODO: LensFlareGroup - Missing matching Lua file

        Register<LightChunk>(new()
        {
            LuaClassName = "Light",
            PropertyOrder = {
                nameof(LightChunk.Name),
                nameof(LightChunk.Version),
                nameof(LightChunk.Type),
                nameof(LightChunk.Colour),
                nameof(LightChunk.Constant),
                nameof(LightChunk.Linear),
                nameof(LightChunk.Squared),
                nameof(LightChunk.Enabled),
            }
        });

        // TODO: LightConeParam - Missing matching Lua file

        Register<LightDirectionChunk>(new()
        {
            LuaClassName = "LightDirection",
            PropertyOrder = {
                nameof(LightDirectionChunk.Direction),
            }
        });

        Register<LightGroupChunk>(new()
        {
            LuaClassName = "LightGroup",
            PropertyOrder = {
                nameof(LightGroupChunk.Name),
                nameof(LightGroupChunk.Lights),
            }
        });

        Register<LightIlluminationTypeChunk>(new()
        {
            LuaClassName = "LightIlluminationType",
            PropertyOrder = {
                nameof(LightIlluminationTypeChunk.IlluminationType),
            }
        });

        Register<LightPositionChunk>(new()
        {
            LuaClassName = "LightPosition",
            PropertyOrder = {
                nameof(LightPositionChunk.Position),
            }
        });

        Register<LightShadowChunk>(new()
        {
            LuaClassName = "LightShadow",
            PropertyOrder = {
                nameof(LightShadowChunk.Shadow),
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
                        if (!staticCameraData.OneShot.HasValue)
                            break;
                        var flags = 0u;
                        if (staticCameraData.OneShot.Value)
                            flags |= 1u;
                        if (staticCameraData.DisableFOV!.Value)
                            flags |= (1u << 1);
                        sb.Append($", {FormatLuaValue(flags)}");
                        if (!staticCameraData.CutInOut.HasValue || !staticCameraData.CarOnly.HasValue)
                            break;
                        sb.Append($", {FormatLuaValue(staticCameraData.CutInOut.Value ? 1u : 0u)}");
                        var flags2 = 0u;
                        if (staticCameraData.CarOnly.Value)
                            flags2 |= 1u;
                        if (staticCameraData.OnFootOnly!.Value)
                            flags2 |= (1u << 1);
                        sb.Append($", {FormatLuaValue(flags2)}");
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
            PropertyOrder = {
                nameof(LocatorMatrixChunk.Matrix),
            }
        });

        Register<MatrixListChunk>(new()
        {
            LuaClassName = "MatrixList",
            PropertyOrder = {
                nameof(MatrixListChunk.Matrices),
            }
        });

        Register<MatrixPaletteChunk>(new()
        {
            LuaClassName = "MatrixPalette",
            PropertyOrder = {
                nameof(MatrixPaletteChunk.Matrices),
            }
        });

        // TODO: MemoryImageIndexList - Missing matching Lua file

        // TODO: MemoryImageVertexDescription - Missing matching Lua file

        // TODO: MemoryImageVertexList - Missing matching Lua file

        Register<MeshChunk>(new()
        {
            LuaClassName = "Mesh",
            PropertyOrder = {
                nameof(MeshChunk.Name),
                nameof(MeshChunk.Version),
            }
        });

        // TODO: MeshStats - Missing matching Lua file

        // TODO: MultiColourList - Missing matching Lua file

        Register<MultiController2Chunk>(new()
        {
            LuaClassName = "MultiController2",
            PropertyOrder = {
                nameof(MultiController2Chunk.Version),
                nameof(MultiController2Chunk.Name),
                nameof(MultiController2Chunk.CycleMode),
                nameof(MultiController2Chunk.NumCycles),
                nameof(MultiController2Chunk.InfiniteCycle),
                nameof(MultiController2Chunk.NumFrames),
                nameof(MultiController2Chunk.FrameRate),
            }
        });

        Register<MultiControllerChunk>(new()
        {
            LuaClassName = "MultiController",
            PropertyOrder = {
                nameof(MultiControllerChunk.Name),
                nameof(MultiControllerChunk.Version),
                nameof(MultiControllerChunk.Length),
                nameof(MultiControllerChunk.Framerate),
            }
        });

        Register<MultiControllerTrackChunk>(new()
        {
            LuaClassName = "MultiControllerTrack",
            PropertyOrder = {
                nameof(MultiControllerTrackChunk.Version),
                nameof(MultiControllerTrackChunk.Name),
                nameof(MultiControllerTrackChunk.Type),
            }
        });

        Register<MultiControllerTracksChunk>(new()
        {
            LuaClassName = "MultiControllerTracks",
            PropertyOrder = {
                nameof(MultiControllerTracksChunk.Tracks),
            }
        });

        Register<NormalListChunk>(new()
        {
            LuaClassName = "NormalList",
            PropertyOrder = {
                nameof(NormalListChunk.Normals),
            }
        });

        Register<OldBaseEmitterChunk>(new()
        {
            LuaClassName = "OldBaseEmitter",
            PropertyOrder = {
                nameof(OldBaseEmitterChunk.Version),
                nameof(OldBaseEmitterChunk.Name),
                nameof(OldBaseEmitterChunk.ParticleType),
                nameof(OldBaseEmitterChunk.GeneratorType),
                nameof(OldBaseEmitterChunk.ZTest),
                nameof(OldBaseEmitterChunk.ZWrite),
                nameof(OldBaseEmitterChunk.Fog),
                nameof(OldBaseEmitterChunk.MaxParticles),
                nameof(OldBaseEmitterChunk.InfiniteLife),
                nameof(OldBaseEmitterChunk.RotationalCohesion),
                nameof(OldBaseEmitterChunk.TranslationCohesion),
            }
        });

        Register<OldBillboardDisplayInfoChunk>(new()
        {
            LuaClassName = "OldBillboardDisplayInfo",
            PropertyOrder = {
                nameof(OldBillboardDisplayInfoChunk.Version),
                nameof(OldBillboardDisplayInfoChunk.Rotation),
                nameof(OldBillboardDisplayInfoChunk.CutOffMode),
                nameof(OldBillboardDisplayInfoChunk.UVOffsetRange),
                nameof(OldBillboardDisplayInfoChunk.SourceRange),
                nameof(OldBillboardDisplayInfoChunk.EdgeRange),
            }
        });

        Register<OldBillboardPerspectiveInfoChunk>(new()
        {
            LuaClassName = "OldBillboardPerspectiveInfo",
            PropertyOrder = {
                nameof(OldBillboardPerspectiveInfoChunk.Version),
                nameof(OldBillboardPerspectiveInfoChunk.PerspectiveScale),
            }
        });

        Register<OldBillboardQuadChunk>(new()
        {
            LuaClassName = "OldBillboardQuad",
            PropertyOrder = {
                nameof(OldBillboardQuadChunk.Version),
                nameof(OldBillboardQuadChunk.Name),
                nameof(OldBillboardQuadChunk.BillboardMode),
                nameof(OldBillboardQuadChunk.Translation),
                nameof(OldBillboardQuadChunk.Colour),
                nameof(OldBillboardQuadChunk.UV0),
                nameof(OldBillboardQuadChunk.UV1),
                nameof(OldBillboardQuadChunk.UV2),
                nameof(OldBillboardQuadChunk.UV3),
                nameof(OldBillboardQuadChunk.Width),
                nameof(OldBillboardQuadChunk.Height),
                nameof(OldBillboardQuadChunk.Distance),
                nameof(OldBillboardQuadChunk.UVOffset),
            }
        });

        Register<OldBillboardQuadGroupChunk>(new()
        {
            LuaClassName = "OldBillboardQuadGroup",
            PropertyOrder = {
                nameof(OldBillboardQuadGroupChunk.Version),
                nameof(OldBillboardQuadGroupChunk.Name),
                nameof(OldBillboardQuadGroupChunk.Shader),
                nameof(OldBillboardQuadGroupChunk.ZTest),
                nameof(OldBillboardQuadGroupChunk.ZWrite),
                nameof(OldBillboardQuadGroupChunk.Occlusion),
            }
        });

        Register<OldColourOffsetListChunk>(new()
        {
            LuaClassName = "OldColourOffsetList",
            PropertyOrder = {
                nameof(OldColourOffsetListChunk.Version),
                nameof(OldColourOffsetListChunk.Offsets),
            }
        });

        Register<OldEmitterAnimationChunk>(new()
        {
            LuaClassName = "OldEmitterAnimation",
            PropertyOrder = {
                nameof(OldEmitterAnimationChunk.Version),
            }
        });

        Register<OldExpressionOffsetsChunk>(new()
        {
            LuaClassName = "OldExpressionOffsets",
            PropertyOrder = {
                nameof(OldExpressionOffsetsChunk.PrimitiveGroupIndices),
            }
        });

        // TODO: OldFrameController2 - Missing matching Lua file

        Register<OldFrameControllerChunk>(new()
        {
            LuaClassName = "OldFrameController",
            PropertyOrder = {
                nameof(OldFrameControllerChunk.Version),
                nameof(OldFrameControllerChunk.Name),
                nameof(OldFrameControllerChunk.Type),
                nameof(OldFrameControllerChunk.FrameOffset),
                nameof(OldFrameControllerChunk.HierarchyName),
                nameof(OldFrameControllerChunk.AnimationName),
            }
        });

        Register<OldGeneratorAnimationChunk>(new()
        {
            LuaClassName = "OldGeneratorAnimation",
            PropertyOrder = {
                nameof(OldGeneratorAnimationChunk.Version),
            }
        });

        Register<OldIndexOffsetListChunk>(new()
        {
            LuaClassName = "OldIndexOffsetList",
            PropertyOrder = {
                nameof(OldIndexOffsetListChunk.Version),
                nameof(OldIndexOffsetListChunk.Offsets),
            }
        });

        Register<OldLocatorChunk>(new()
        {
            LuaClassName = "Locator3",
            PropertyOrder = {
                nameof(OldLocatorChunk.Name),
                nameof(OldLocatorChunk.Version),
                nameof(OldLocatorChunk.Position),
            }
        });

        Register<OldOffsetListChunk>(new()
        {
            LuaClassName = "OldOffsetList",
            PropertyOrder = {
                nameof(OldOffsetListChunk.KeyIndex),
                nameof(OldOffsetListChunk.Offsets),
                nameof(OldOffsetListChunk.PrimGroupIndex),
            }
        });

        Register<OldParticleAnimationChunk>(new()
        {
            LuaClassName = "OldParticleAnimation",
            PropertyOrder = {
                nameof(OldParticleAnimationChunk.Version),
            }
        });

        Register<OldParticleInstancingInfoChunk>(new()
        {
            LuaClassName = "OldParticleInstancingInfo",
            PropertyOrder = {
                nameof(OldParticleInstancingInfoChunk.Version),
                nameof(OldParticleInstancingInfoChunk.MaxInstances),
            }
        });

        Register<OldPrimitiveGroupChunk>(new()
        {
            LuaClassName = "OldPrimitiveGroup",
            PropertyOrder = {
                nameof(OldPrimitiveGroupChunk.Version),
                nameof(OldPrimitiveGroupChunk.ShaderName),
                nameof(OldPrimitiveGroupChunk.PrimitiveType),
                nameof(OldPrimitiveGroupChunk.NumVertices),
                nameof(OldPrimitiveGroupChunk.NumIndices),
                nameof(OldPrimitiveGroupChunk.NumMatrices),
            }
        });

        Register<OldScenegraphBranchChunk>(new()
        {
            LuaClassName = "OldScenegraphBranch",
            PropertyOrder = {
                nameof(OldScenegraphBranchChunk.Name),
            }
        });

        Register<OldScenegraphDrawableChunk>(new()
        {
            LuaClassName = "OldScenegraphDrawable",
            PropertyOrder = {
                nameof(OldScenegraphDrawableChunk.Name),
                nameof(OldScenegraphDrawableChunk.DrawableName),
                nameof(OldScenegraphDrawableChunk.IsTranslucent),
            }
        });

        Register<OldScenegraphLightGroupChunk>(new()
        {
            LuaClassName = "OldScenegraphLightGroup",
            PropertyOrder = {
                nameof(OldScenegraphLightGroupChunk.Name),
                nameof(OldScenegraphLightGroupChunk.LightGroupName),
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
            PropertyOrder = {
                nameof(OldScenegraphSortOrderChunk.SortOrder),
            }
        });

        Register<OldScenegraphTransformChunk>(new()
        {
            LuaClassName = "OldScenegraphTransform",
            PropertyOrder = {
                nameof(OldScenegraphTransformChunk.Name),
                nameof(OldScenegraphTransformChunk.Transform),
            }
        });

        Register<OldScenegraphVisibilityChunk>(new()
        {
            LuaClassName = "OldScenegraphVisibility",
            PropertyOrder = {
                nameof(OldScenegraphVisibilityChunk.Name),
                nameof(OldScenegraphVisibilityChunk.IsVisible),
            }
        });

        Register<OldSpriteEmitterChunk>(new()
        {
            LuaClassName = "OldSpriteEmitter",
            PropertyOrder = {
                nameof(OldSpriteEmitterChunk.Version),
                nameof(OldSpriteEmitterChunk.Name),
                nameof(OldSpriteEmitterChunk.ShaderName),
                nameof(OldSpriteEmitterChunk.AngleMode),
                nameof(OldSpriteEmitterChunk.Angle),
                nameof(OldSpriteEmitterChunk.TextureAnimMode),
                nameof(OldSpriteEmitterChunk.NumTextureFrames),
                nameof(OldSpriteEmitterChunk.TextureFrameRate),
            }
        });

        Register<OldVector2OffsetListChunk>(new()
        {
            LuaClassName = "OldVector2OffsetList",
            PropertyOrder = {
                nameof(OldVector2OffsetListChunk.Version),
                nameof(OldVector2OffsetListChunk.Offsets),
                nameof(OldVector2OffsetListChunk.Param),
            }
        });

        Register<OldVectorOffsetListChunk>(new()
        {
            LuaClassName = "OldVectorOffsetList",
            PropertyOrder = {
                nameof(OldVectorOffsetListChunk.Version),
                nameof(OldVectorOffsetListChunk.Offsets),
                nameof(OldVectorOffsetListChunk.Param),
            }
        });

        Register<OldVertexAnimKeyFrameChunk>(new()
        {
            LuaClassName = "OldVertexAnimKeyFrame",
            PropertyOrder = {
                nameof(OldVertexAnimKeyFrameChunk.Version),
                nameof(OldVertexAnimKeyFrameChunk.Name),
            }
        });

        Register<PackedNormalListChunk>(new()
        {
            LuaClassName = "PackedNormalList",
            PropertyOrder = {
                nameof(PackedNormalListChunk.Normals),
            }
        });

        // TODO: ParticlePlaneGenerator - Missing matching Lua file

        // TODO: ParticlePointGenerator - Missing matching Lua file

        Register<ParticleSystem2Chunk>(new()
        {
            LuaClassName = "ParticleSystem2",
            PropertyOrder = {
                nameof(ParticleSystem2Chunk.Version),
                nameof(ParticleSystem2Chunk.Name),
                nameof(ParticleSystem2Chunk.FactoryName),
            }
        });

        // TODO: ParticleSystem - Missing matching Lua file

        Register<ParticleSystemFactoryChunk>(new()
        {
            LuaClassName = "ParticleSystemFactory",
            PropertyOrder = {
                nameof(ParticleSystemFactoryChunk.Version),
                nameof(ParticleSystemFactoryChunk.Name),
                nameof(ParticleSystemFactoryChunk.FrameRate),
                nameof(ParticleSystemFactoryChunk.NumAnimFrames),
                nameof(ParticleSystemFactoryChunk.NumOLFrames),
                nameof(ParticleSystemFactoryChunk.CycleAnim),
                nameof(ParticleSystemFactoryChunk.EnableSorting),
            }
        });

        Register<PathChunk>(new()
        {
            LuaClassName = "Path",
            PropertyOrder = {
                nameof(PathChunk.Positions),
            }
        });

        // TODO: PhotonMap - Missing matching Lua file

        Register<PhysicsInertiaMatrixChunk>(new()
        {
            LuaClassName = "PhysicsInertiaMatrix",
            PropertyOrder = {
                nameof(PhysicsInertiaMatrixChunk.Matrix),
            }
        });

        Register<PhysicsJointChunk>(new()
        {
            LuaClassName = "PhysicsJoint",
            PropertyOrder = {
                nameof(PhysicsJointChunk.Index),
                nameof(PhysicsJointChunk.Volume),
                nameof(PhysicsJointChunk.Stiffness),
                nameof(PhysicsJointChunk.MaxAngle),
                nameof(PhysicsJointChunk.MinAngle),
                nameof(PhysicsJointChunk.DegreesOfFreedom),
            }
        });

        Register<PhysicsObjectChunk>(new()
        {
            LuaClassName = "PhysicsObject",
            PropertyOrder = {
                nameof(PhysicsObjectChunk.Name),
                nameof(PhysicsObjectChunk.Version),
                nameof(PhysicsObjectChunk.MaterialName),
                nameof(PhysicsObjectChunk.NumJoints),
                nameof(PhysicsObjectChunk.Volume),
                nameof(PhysicsObjectChunk.RestingSensitivity),
            }
        });

        Register<PhysicsVectorChunk>(new()
        {
            LuaClassName = "PhysicsVector",
            PropertyOrder = {
                nameof(PhysicsVectorChunk.Vector),
            }
        });

        Register<PositionListChunk>(new()
        {
            LuaClassName = "PositionList",
            PropertyOrder = {
                nameof(PositionListChunk.Positions),
            }
        });

        // TODO: PrimitiveGroup - Missing matching Lua file

        Register<QuaternionChannelChunk>(new()
        {
            LuaClassName = "QuaternionChannel",
            PropertyOrder = {
                nameof(QuaternionChannelChunk.Version),
                nameof(QuaternionChannelChunk.Param),
                nameof(QuaternionChannelChunk.Frames),
                nameof(QuaternionChannelChunk.Values),
            }
        });

        Register<RailCamChunk>(new()
        {
            LuaClassName = "RailCam",
            PropertyOrder = {
                nameof(RailCamChunk.Name),
                nameof(RailCamChunk.Behaviour),
                nameof(RailCamChunk.MinRadius),
                nameof(RailCamChunk.MaxRadius),
                nameof(RailCamChunk.TrackRail),
                nameof(RailCamChunk.TrackDist),
                nameof(RailCamChunk.ReverseSense),
                nameof(RailCamChunk.FOV),
                nameof(RailCamChunk.TargetOffset),
                nameof(RailCamChunk.AxisPlay),
                nameof(RailCamChunk.PositionLag),
                nameof(RailCamChunk.TargetLag),
            }
        });

        Register<RenderStatusChunk>(new()
        {
            LuaClassName = "RenderStatus",
            CustomOverride = chunk =>
            {
                var renderStatusChunk = (RenderStatusChunk)chunk;
                return FormatLuaValue(!renderStatusChunk.CastShadow);
            }
        });

        Register<RoadChunk>(new()
        {
            LuaClassName = "Road",
            PropertyOrder = {
                nameof(RoadChunk.Name),
                nameof(RoadChunk.Type),
                nameof(RoadChunk.StartIntersection),
                nameof(RoadChunk.EndIntersection),
                nameof(RoadChunk.MaximumCars),
                nameof(RoadChunk.Speed),
                nameof(RoadChunk.Intelligence),
                nameof(RoadChunk.Shortcut),
            }
        });

        Register<RoadDataSegmentChunk>(new()
        {
            LuaClassName = "RoadDataSegment",
            PropertyOrder = {
                nameof(RoadDataSegmentChunk.Name),
                nameof(RoadDataSegmentChunk.Type),
                nameof(RoadDataSegmentChunk.Lanes),
                nameof(RoadDataSegmentChunk.HasShoulder),
                nameof(RoadDataSegmentChunk.Direction),
                nameof(RoadDataSegmentChunk.Top),
                nameof(RoadDataSegmentChunk.Bottom),
            }
        });

        Register<RoadSegmentChunk>(new()
        {
            LuaClassName = "RoadSegment",
            PropertyOrder = {
                nameof(RoadSegmentChunk.Name),
                nameof(RoadSegmentChunk.RoadDataSegment),
                nameof(RoadSegmentChunk.Transform),
                nameof(RoadSegmentChunk.Scale),
            }
        });

        // TODO: ScenegraphBranch - Missing matching Lua file

        Register<ScenegraphChunk>(new()
        {
            LuaClassName = "Scenegraph",
            PropertyOrder = {
                nameof(ScenegraphChunk.Name),
                nameof(ScenegraphChunk.Version),
            }
        });

        // TODO: ScenegraphRoot - Missing matching Lua file

        // TODO: ScenegraphTransform - Missing matching Lua file

        Register<SetChunk>(new()
        {
            LuaClassName = "Set",
            PropertyOrder = {
                nameof(SetChunk.Name),
                nameof(SetChunk.Version),
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
            PropertyOrder = {
                nameof(ShaderColourParameterChunk.Param),
                nameof(ShaderColourParameterChunk.Value),
            }
        });

        // TODO: ShaderDefinition - Missing matching Lua file

        Register<ShaderFloatParameterChunk>(new()
        {
            LuaClassName = "ShaderFloatParameter",
            PropertyOrder = {
                nameof(ShaderFloatParameterChunk.Param),
                nameof(ShaderFloatParameterChunk.Value),
            }
        });

        Register<ShaderIntegerParameterChunk>(new()
        {
            LuaClassName = "ShaderIntegerParameter",
            PropertyOrder = {
                nameof(ShaderIntegerParameterChunk.Param),
                nameof(ShaderIntegerParameterChunk.Value),
            }
        });

        Register<ShaderTextureParameterChunk>(new()
        {
            LuaClassName = "ShaderTextureParameter",
            PropertyOrder = {
                nameof(ShaderTextureParameterChunk.Param),
                nameof(ShaderTextureParameterChunk.Value),
            }
        });

        // TODO: ShadowMesh - Missing matching Lua file

        // TODO: ShadowSkin - Missing matching Lua file

        // TODO: Skeleton2 - Missing matching Lua file

        Register<SkeletonChunk>(new()
        {
            LuaClassName = "Skeleton",
            PropertyOrder = {
                nameof(SkeletonChunk.Name),
                nameof(SkeletonChunk.Version),
            }
        });

        // TODO: SkeletonJoint2 - Missing matching Lua file

        Register<SkeletonJointBonePreserveChunk>(new()
        {
            LuaClassName = "SkeletonJointBonePreserve",
            PropertyOrder = {
                nameof(SkeletonJointBonePreserveChunk.PreserveBoneLengths),
            }
        });

        Register<SkeletonJointChunk>(new()
        {
            LuaClassName = "SkeletonJoint",
            PropertyOrder = {
                nameof(SkeletonJointChunk.Name),
                nameof(SkeletonJointChunk.Parent),
                nameof(SkeletonJointChunk.DOF),
                nameof(SkeletonJointChunk.FreeAxis),
                nameof(SkeletonJointChunk.PrimaryAxis),
                nameof(SkeletonJointChunk.SecondaryAxis),
                nameof(SkeletonJointChunk.TwistAxis),
                nameof(SkeletonJointChunk.RestPose),
            }
        });

        Register<SkeletonJointMirrorMapChunk>(new()
        {
            LuaClassName = "SkeletonJointMirrorMap",
            PropertyOrder = {
                nameof(SkeletonJointMirrorMapChunk.MappedJointIndex),
                nameof(SkeletonJointMirrorMapChunk.XAxisMap),
                nameof(SkeletonJointMirrorMapChunk.YAxisMap),
                nameof(SkeletonJointMirrorMapChunk.ZAxisMap),
            }
        });

        // TODO: SkeletonPartition - Missing matching Lua file

        Register<SkinChunk>(new()
        {
            LuaClassName = "Skin",
            PropertyOrder = {
                nameof(SkinChunk.Name),
                nameof(SkinChunk.Version),
                nameof(SkinChunk.SkeletonName),
            }
        });

        // TODO: SmartProp - Missing matching Lua file

        // TODO: SortOrder - Missing matching Lua file

        Register<SpatialNodeChunk>(new()
        {
            LuaClassName = "TreeNode2",
            PropertyOrder = {
                nameof(SpatialNodeChunk.SplitAxis),
                nameof(SpatialNodeChunk.SplitPosition),
                nameof(SpatialNodeChunk.StaticEntityLimit),
                nameof(SpatialNodeChunk.StaticPhysEntityLimit),
                nameof(SpatialNodeChunk.IntersectEntityLimit),
                nameof(SpatialNodeChunk.DynaPhysEntityLimit),
                nameof(SpatialNodeChunk.FenceEntityLimit),
                nameof(SpatialNodeChunk.RoadSegmentEntityLimit),
                nameof(SpatialNodeChunk.PathSegmentEntityLimit),
                nameof(SpatialNodeChunk.AnimEntityLimit),
            }
        });

        Register<SplineChunk>(new()
        {
            LuaClassName = "Spline",
            PropertyOrder = {
                nameof(SplineChunk.Name),
                nameof(SplineChunk.Positions),
            }
        });

        Register<SpriteChunk>(new()
        {
            LuaClassName = "Sprite",
            PropertyOrder = {
                nameof(SpriteChunk.Name),
                nameof(SpriteChunk.NativeX),
                nameof(SpriteChunk.NativeY),
                nameof(SpriteChunk.Shader),
                nameof(SpriteChunk.ImageWidth),
                nameof(SpriteChunk.ImageHeight),
                nameof(SpriteChunk.BlitBorder),
            }
        });

        // TODO: SpriteParticleEmitter - Missing matching Lua file

        Register<StatePropCallbackDataChunk>(new()
        {
            LuaClassName = "StatePropCallbackData",
            PropertyOrder = {
                nameof(StatePropCallbackDataChunk.Name),
                nameof(StatePropCallbackDataChunk.Event),
                nameof(StatePropCallbackDataChunk.OnFrame),
            }
        });

        Register<StatePropDataV1Chunk>(new()
        {
            LuaClassName = "StatePropDataV1",
            PropertyOrder = {
                nameof(StatePropDataV1Chunk.Version),
                nameof(StatePropDataV1Chunk.Name),
                nameof(StatePropDataV1Chunk.ObjectFactoryName),
            }
        });

        Register<StatePropEventDataChunk>(new()
        {
            LuaClassName = "StatePropEventData",
            PropertyOrder = {
                nameof(StatePropEventDataChunk.Name),
                nameof(StatePropEventDataChunk.ToState),
                nameof(StatePropEventDataChunk.Event),
            }
        });

        Register<StatePropFrameControllerDataChunk>(new()
        {
            LuaClassName = "StatePropFrameControllerData",
            PropertyOrder = {
                nameof(StatePropFrameControllerDataChunk.Name),
                nameof(StatePropFrameControllerDataChunk.Cyclic),
                nameof(StatePropFrameControllerDataChunk.NumCycles),
                nameof(StatePropFrameControllerDataChunk.HoldFrame),
                nameof(StatePropFrameControllerDataChunk.MinFrame),
                nameof(StatePropFrameControllerDataChunk.MaxFrame),
                nameof(StatePropFrameControllerDataChunk.RelativeSpeed),
            }
        });

        Register<StatePropStateDataV1Chunk>(new()
        {
            LuaClassName = "StatePropStateDataV1",
            PropertyOrder = {
                nameof(StatePropStateDataV1Chunk.Name),
                nameof(StatePropStateDataV1Chunk.AutoTransition),
                nameof(StatePropStateDataV1Chunk.OutState),
                nameof(StatePropStateDataV1Chunk.OutFrame),
            }
        });

        Register<StatePropVisibilitiesDataChunk>(new()
        {
            LuaClassName = "StatePropVisibilitiesData",
            PropertyOrder = {
                nameof(StatePropVisibilitiesDataChunk.Name),
                nameof(StatePropVisibilitiesDataChunk.IsVisible),
            }
        });

        Register<StaticEntityChunk>(new()
        {
            LuaClassName = "StaticEntity",
            PropertyOrder = {
                nameof(StaticEntityChunk.Name),
                nameof(StaticEntityChunk.Version),
                nameof(StaticEntityChunk.HasAlpha),
            }
        });

        Register<StaticPhysChunk>(new()
        {
            LuaClassName = "StaticPhys",
            PropertyOrder = {
                nameof(StaticPhysChunk.Name),
                nameof(StaticPhysChunk.Version),
            }
        });

        // TODO: TangentList - Missing matching Lua file

        Register<TerrainTypeListChunk>(new()
        {
            LuaClassName = "SurfaceTypeList",
            PropertyOrder = {
                nameof(TerrainTypeListChunk.Version),
                nameof(TerrainTypeListChunk.Types),
            }
        });

        Register<TextureAnimationChunk>(new()
        {
            LuaClassName = "TextureAnimation",
            PropertyOrder = {
                nameof(TextureAnimationChunk.Name),
                nameof(TextureAnimationChunk.Version),
                nameof(TextureAnimationChunk.MaterialName),
                nameof(TextureAnimationChunk.NumFrames),
                nameof(TextureAnimationChunk.FrameRate),
                nameof(TextureAnimationChunk.Cyclic),
            }
        });

        Register<TextureChunk>(new()
        {
            LuaClassName = "Texture",
            PropertyOrder = {
                nameof(TextureChunk.Name),
                nameof(TextureChunk.Version),
                nameof(TextureChunk.Width),
                nameof(TextureChunk.Height),
                nameof(TextureChunk.Bpp),
                nameof(TextureChunk.AlphaDepth),
                nameof(TextureChunk.NumMipMaps),
                nameof(TextureChunk.TextureType),
                nameof(TextureChunk.UsageHint),
                nameof(TextureChunk.Priority),
            }
        });

        Register<TextureFontChunk>(new()
        {
            LuaClassName = "TextureFont",
            PropertyOrder = {
                nameof(TextureFontChunk.Version),
                nameof(TextureFontChunk.Name),
                nameof(TextureFontChunk.Shader),
                nameof(TextureFontChunk.FontSize),
                nameof(TextureFontChunk.FontWidth),
                nameof(TextureFontChunk.FontHeight),
                nameof(TextureFontChunk.FontBaseLine),
            }
        });

        Register<TextureGlyphListChunk>(new()
        {
            LuaClassName = "TextureGlyphList",
            PropertyOrder = {
                nameof(TextureGlyphListChunk.Glyphs),
            }
        });

        // TODO: Topology - Missing matching Lua file

        Register<TreeChunk>(new()
        {
            LuaClassName = "Tree",
            PropertyOrder = {
                nameof(TreeChunk.BoundsMin),
                nameof(TreeChunk.BoundsMax),
            }
        });

        Register<TreeNodeChunk>(new()
        {
            LuaClassName = "TreeNode",
            PropertyOrder = {
                nameof(TreeNodeChunk.SubTreeSize),
                nameof(TreeNodeChunk.ParentOffset),
            }
        });

        Register<TriggerVolumeChunk>(new()
        {
            LuaClassName = "TriggerVolume",
            PropertyOrder = {
                nameof(TriggerVolumeChunk.Name),
                nameof(TriggerVolumeChunk.Type),
                nameof(TriggerVolumeChunk.HalfExtents),
                nameof(TriggerVolumeChunk.Matrix),
            }
        });

        Register<UVListChunk>(new()
        {
            LuaClassName = "UVList",
            PropertyOrder = {
                nameof(UVListChunk.Channel),
                nameof(UVListChunk.UVs),
            }
        });

        Register<Vector1DOFChannelChunk>(new()
        {
            LuaClassName = "Vector1DOFChannel",
            PropertyOrder = {
                nameof(Vector1DOFChannelChunk.Version),
                nameof(Vector1DOFChannelChunk.Param),
                nameof(Vector1DOFChannelChunk.DynamicIndex),
                nameof(Vector1DOFChannelChunk.Constants),
                nameof(Vector1DOFChannelChunk.Frames),
                nameof(Vector1DOFChannelChunk.Values),
            }
        });

        Register<Vector2DOFChannelChunk>(new()
        {
            LuaClassName = "Vector2DOFChannel",
            PropertyOrder = {
                nameof(Vector2DOFChannelChunk.Version),
                nameof(Vector2DOFChannelChunk.Param),
                nameof(Vector2DOFChannelChunk.StaticIndex),
                nameof(Vector2DOFChannelChunk.Constants),
                nameof(Vector2DOFChannelChunk.Frames),
                nameof(Vector2DOFChannelChunk.Values),
            }
        });

        // TODO: Vector2OffsetList - Missing matching Lua file

        Register<Vector3DOFChannelChunk>(new()
        {
            LuaClassName = "Vector3DOFChannel",
            PropertyOrder = {
                nameof(Vector3DOFChannelChunk.Version),
                nameof(Vector3DOFChannelChunk.Param),
                nameof(Vector3DOFChannelChunk.Frames),
                nameof(Vector3DOFChannelChunk.Values),
            }
        });

        // TODO: VertexAnimKeyFrame - Missing matching Lua file

        // TODO: VertexAnimKeyFrameList - Missing matching Lua file

        // TODO: VertexCompressionHint - Missing matching Lua file

        Register<VertexShaderChunk>(new()
        {
            LuaClassName = "VertexShader",
            PropertyOrder = {
                nameof(VertexShaderChunk.Name),
            }
        });

        // TODO: VisibilityAnimChannel - Missing matching Lua file

        // TODO: VisibilityAnim - Missing matching Lua file

        Register<VolumeImageChunk>(new()
        {
            LuaClassName = "VolumeImage",
            PropertyOrder = {
                nameof(VolumeImageChunk.Name),
                nameof(VolumeImageChunk.Version),
                nameof(VolumeImageChunk.Width),
                nameof(VolumeImageChunk.Height),
                nameof(VolumeImageChunk.Depth),
                nameof(VolumeImageChunk.Bpp),
                nameof(VolumeImageChunk.Palettized),
                nameof(VolumeImageChunk.HasAlpha),
                nameof(VolumeImageChunk.Format),
            }
        });

        Register<WalkerCameraDataChunk>(new()
        {
            LuaClassName = "WalkerCameraData",
            PropertyOrder = {
                nameof(WalkerCameraDataChunk.Index),
                nameof(WalkerCameraDataChunk.MinMagnitude),
                nameof(WalkerCameraDataChunk.MaxMagnitude),
                nameof(WalkerCameraDataChunk.Elevation),
                nameof(WalkerCameraDataChunk.TargetOffset),
            }
        });

        Register<WallChunk>(new()
        {
            LuaClassName = "Fence2",
            PropertyOrder = {
                nameof(WallChunk.Start),
                nameof(WallChunk.End),
                nameof(WallChunk.Normal),
            }
        });

        Register<WeightListChunk>(new()
        {
            LuaClassName = "WeightList",
            PropertyOrder = {
                nameof(WeightListChunk.Weights),
            }
        });

        // TODO: WorldCollisionObject - Missing matching Lua file

        Register<WorldSphereChunk>(new()
        {
            LuaClassName = "WorldSphere",
            PropertyOrder = {
                nameof(WorldSphereChunk.Name),
                nameof(WorldSphereChunk.Version),
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
        var sb = new StringBuilder();
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
                    {
                        var bytes = Encoding.UTF8.GetBytes([c]);
                        foreach (var b in bytes)
                            sb.AppendFormat("\\x{0:X2}", b);
                    }
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
