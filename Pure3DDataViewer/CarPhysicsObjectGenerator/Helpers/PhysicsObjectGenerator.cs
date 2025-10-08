using NetP3DLib.Numerics;
using NetP3DLib.P3D.Chunks;
using System.Diagnostics;
using System.Numerics;

namespace CarPhysicsObjectGenerator.Helpers;

public static class PhysicsObjectGenerator
{
    private static readonly HashSet<string> JointNames =
    [
        "DoorDRot", "DoorPRot", "TrunkRot", "HoodRot", "w0", "w1", "w2", "w3"
    ];
    private static readonly HashSet<string> JointNamesBV =
    [
        "w0", "w1", "w2", "w3"
    ];

    public static PhysicsObjectChunk GeneratePhysicsObject(SkeletonChunk skeleton, CollisionObjectChunk collisionObject, bool BV = false)
    {
        var jointNames = BV ? JointNamesBV : JointNames;

        var collisionVolume = collisionObject.GetFirstChunkOfType<CollisionVolumeChunk>() ?? throw new InvalidDataException($"No Collision Volume found in {collisionObject}.");
        var physicsObject = new PhysicsObjectChunk(collisionObject.Name, 1, "", skeleton.NumJoints, 0, 1);

        var joints = skeleton.GetChunksOfType<SkeletonJointChunk>();

        for (int jointIndex = 0; jointIndex < joints.Count; jointIndex++)
        {
            var joint = joints[jointIndex];
            if (jointIndex == 0 || jointNames.Contains(joint.Name))
            {
                var physicsJoint = new PhysicsJointChunk(jointIndex, 0, 0, 0, 0, PhysicsJointChunk.DegreesOfFreedom.Fixed0D);

                // YAY HARDCODING
                switch (joint.Name)
                {
                    case "DoorDRot":
                    case "DoorPRot":
                    case "TrunkRot":
                        physicsJoint.Stiffness = 0.8f;
                        physicsJoint.MinAngle = 0.0f;
                        physicsJoint.MaxAngle = 1.0f;
                        physicsJoint.DOF = PhysicsJointChunk.DegreesOfFreedom.Hinge1D;
                        break;
                    case "HoodRot":
                        physicsJoint.Stiffness = 0.5f;
                        physicsJoint.MinAngle = 0.0f;
                        physicsJoint.MaxAngle = 0.5f;
                        physicsJoint.DOF = PhysicsJointChunk.DegreesOfFreedom.Hinge1D;
                        break;
                }

                var relevantVolumes = collisionVolume.GetChunksOfType<CollisionVolumeChunk>()
                    .Where(v => v.ObjectReferenceIndex == jointIndex)
                    .ToList();

                if (relevantVolumes.Count != 0)
                {
                    physicsJoint.Volume += relevantVolumes.Sum(CalculateVolumeMass);

                    var centreOfMass = CalculatecentreOfMass(relevantVolumes);
                    var physicsVector = new PhysicsVectorChunk(centreOfMass);
                    physicsJoint.Children.Add(physicsVector);

                    var inertiaMatrix = CalculateInertiaMatrix(relevantVolumes, centreOfMass);
                    var physicsInertiaMatrixChunk = new PhysicsInertiaMatrixChunk(inertiaMatrix);
                    physicsJoint.Children.Add(physicsInertiaMatrixChunk);
                }
                else
                {
                    var physicsVector = new PhysicsVectorChunk(Vector3.Zero);
                    physicsJoint.Children.Add(physicsVector);

                    var physicsInertiaMatrixChunk = new PhysicsInertiaMatrixChunk(SymmetricMatrix3x3.Zero);
                    physicsJoint.Children.Add(physicsInertiaMatrixChunk);
                }

                physicsObject.Children.Add(physicsJoint);
                physicsObject.Volume += physicsJoint.Volume;
            }
        }

        return physicsObject;
    }

    private static Vector3 CalculatecentreOfMass(List<CollisionVolumeChunk> volumes)
    {
        var totalMass = 0f;
        var weightedSum = Vector3.Zero;

        foreach (var volume in volumes)
        {
            var vector = volume.Children[0].GetFirstChunkOfType<CollisionVectorChunk>();
            if (vector == null) continue;

            var mass = CalculateVolumeMass(volume);
            totalMass += mass;
            weightedSum += vector.Vector * mass;
        }

        return totalMass > 0 ? weightedSum / totalMass : Vector3.Zero;
    }

    private static float CalculateVolumeMass(CollisionVolumeChunk volume)
    {
        if (volume.Children.OfType<CollisionSphereChunk>().FirstOrDefault() is CollisionSphereChunk sphere)
            return 4f / 3f * MathF.PI * MathF.Pow(sphere.Radius, 3);

        if (volume.Children.OfType<CollisionOrientedBoundingBoxChunk>().FirstOrDefault() is CollisionOrientedBoundingBoxChunk obb)
            return 8f * obb.HalfExtents.X * obb.HalfExtents.Y * obb.HalfExtents.Z;

        if (volume.Children.OfType<CollisionCylinderChunk>().FirstOrDefault() is CollisionCylinderChunk cylinder)
            return MathF.PI * MathF.Pow(cylinder.Radius, 2) * (cylinder.HalfLength * 2);

        return 0f;
    }

    private static SymmetricMatrix3x3 CalculateInertiaMatrix(List<CollisionVolumeChunk> volumes, Vector3 centreOfMass)
    {
        var inertiaMatrix = new SymmetricMatrix3x3();

        var totalMass = 0f;
        foreach (var volume in volumes)
        {
            var vector = volume.Children[0].GetFirstChunkOfType<CollisionVectorChunk>();
            if (vector == null) continue;

            var mass = CalculateVolumeMass(volume);
            totalMass += mass;

            if (volume.GetFirstChunkOfType<CollisionOrientedBoundingBoxChunk>() is CollisionOrientedBoundingBoxChunk obb)
            {
                var ex = obb.HalfExtents.X * 2;
                var ey = obb.HalfExtents.Y * 2;
                var ez = obb.HalfExtents.Z * 2;
                const float massFactor = 1f / 12f;

                float[,] inertia = new float[3, 3];
                inertia[0, 0] = massFactor * (ey * ey + ez * ez);
                inertia[1, 1] = massFactor * (ex * ex + ez * ez);
                inertia[2, 2] = massFactor * (ex * ex + ey * ey);

                var vectors = obb.GetChunksOfType<CollisionVectorChunk>();
                if (vectors.Count != 4)
                    throw new InvalidDataException("A Collision Oriented Bounding Box Chunk does not have the correct number of sub vectors.");

                var matrixX = vectors[1].Vector;
                var matrixY = vectors[2].Vector;
                var matrixZ = vectors[3].Vector;
                float[,] rot =
                {
                    { matrixX.X, matrixX.Y, matrixX.Z },
                    { matrixY.X, matrixY.Y, matrixY.Z },
                    { matrixZ.X, matrixZ.Y, matrixZ.Z },
                };

                var rotT = Transpose(rot);
                var inertiaRotated = Multiply(Multiply(rotT, inertia), rot);

                var localMatrix = new SymmetricMatrix3x3(
                    inertiaRotated[0, 0], inertiaRotated[0, 1], inertiaRotated[0, 2],
                    inertiaRotated[1, 1], inertiaRotated[1, 2],
                    inertiaRotated[2, 2]
                );

                var centre = vectors[0].Vector - centreOfMass;
                var localMatrixTranslated = SymmetricMatrix3x3.Translate(localMatrix, centre);

                inertiaMatrix += localMatrixTranslated;
            }
            else if (volume.GetFirstChunkOfType<CollisionSphereChunk>() is CollisionSphereChunk sphere)
            {
                var radius = sphere.Radius;
                const float massFactor = 2f / 5f;

                var inertiaValue = massFactor * radius * radius;

                var vectors = sphere.GetChunksOfType<CollisionVectorChunk>();
                if (vectors.Count != 1)
                    throw new InvalidDataException("A Collision Sphere Chunk does not have the correct number of sub vectors.");

                var localMatrix = new SymmetricMatrix3x3(inertiaValue, 0, 0, inertiaValue, 0, inertiaValue);

                var centre = vectors[0].Vector - centreOfMass;
                var localMatrixTranslated = SymmetricMatrix3x3.Translate(localMatrix, centre);

                inertiaMatrix += localMatrixTranslated;
            }
            else if (volume.GetFirstChunkOfType<CollisionCylinderChunk>() is CollisionCylinderChunk cylinder)
            {
                var radius = cylinder.Radius;
                var halfLength = cylinder.HalfLength;
                const float massFactor = 1f;

                var inertiaX = 1f / 12f * massFactor * (3 * radius * radius + halfLength * halfLength);
                var inertiaY = 1f / 2f * massFactor * radius * radius;
                var inertiaZ = inertiaX;

                var vectors = cylinder.GetChunksOfType<CollisionVectorChunk>();
                if (vectors.Count != 2)
                    throw new InvalidDataException("A Collision Cylinder Chunk does not have the correct number of sub vectors.");

                var direction = vectors[1];

                var matrixX = direction.Vector;
                var matrixY = direction.Vector;
                var matrixZ = direction.Vector;

                float[,] rot =
                {
                    { matrixX.X, matrixX.Y, matrixX.Z },
                    { matrixY.X, matrixY.Y, matrixY.Z },
                    { matrixZ.X, matrixZ.Y, matrixZ.Z },
                };

                var rotT = Transpose(rot);
                var inertiaMatrixLocal = new float[3, 3]
                {
                    { inertiaX, 0, 0 },
                    { 0, inertiaY, 0 },
                    { 0, 0, inertiaZ }
                };

                var inertiaRotated = Multiply(Multiply(rotT, inertiaMatrixLocal), rot);

                var localMatrix = new SymmetricMatrix3x3(
                    inertiaRotated[0, 0], inertiaRotated[0, 1], inertiaRotated[0, 2],
                    inertiaRotated[1, 1], inertiaRotated[1, 2],
                    inertiaRotated[2, 2]
                );

                var centre = vectors[0].Vector - centreOfMass;
                var localMatrixTranslated = SymmetricMatrix3x3.Translate(localMatrix, centre);

                inertiaMatrix += localMatrixTranslated;
            }
            else
            {
                Debugger.Break();
            }
        }

        return totalMass * inertiaMatrix;
    }

    private static float[,] Transpose(float[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        float[,] result = new float[cols, rows];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[j, i] = matrix[i, j];
            }
        }

        return result;
    }

    private static float[,] Multiply(float[,] a, float[,] b)
    {
        int aRows = a.GetLength(0);
        int aCols = a.GetLength(1);
        int bCols = b.GetLength(1);

        float[,] result = new float[aRows, bCols];

        for (int i = 0; i < aRows; i++)
        {
            for (int j = 0; j < bCols; j++)
            {
                result[i, j] = 0;
                for (int k = 0; k < aCols; k++)
                {
                    result[i, j] += a[i, k] * b[k, j];
                }
            }
        }

        return result;
    }
}
