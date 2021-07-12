using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace SiGlaz.Common.ImageAlignment
{
    public abstract class MetrologySystemReference : ParameterBase
    {
        public const string FileExt = "msr";
        public const string FileFilter = "Reference File (*.msr)|*.msr";

        public new static int CurrentVersion
        {
            get
            {
                return 1;
            }
        }

        public float DeviceLeft = 0;
        public float DeviceTop = 0;
        public float DeviceOrientation = 0;
        
        public MetrologySystem MetrologySystem = null;
        public AlignmentSettings AlignmentSettings = null;

        public byte[] ReferenceImage = null;
        public byte[] Regions = null;

        protected MetrologySystemReference()
        {
        }

        protected MetrologySystemReference(
            float deviceLeft, float deviceTop, float deviceOrientation,
            MetrologySystem metrologySystem, AlignmentSettings alignmentSettings,
            byte[] referenceImage, byte[] regions)
        {
            DeviceLeft = deviceLeft;
            DeviceTop = deviceTop;
            DeviceOrientation = deviceOrientation;

            MetrologySystem = metrologySystem;
            AlignmentSettings = alignmentSettings;

            ReferenceImage = referenceImage;
            Regions = regions;
        }

        public virtual PointF TransformToLTDeviceCoordinate(PointF pt)
        {
            PointF[] pts = new PointF[] { pt };
            using (System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix())
            {
                m.Translate(DeviceLeft, DeviceTop);
                m.Rotate(-DeviceOrientation);
                m.Invert();

                m.TransformPoints(pts);
            }

            return pts[0];
        }

        public static PointF TransformToImageCoordinate(
            PointF pt, float absLeft, float absTop, float absRotationAngle)
        {
            PointF[] pts = new PointF[] { pt };

            using (System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix())
            {
                m.Translate(absLeft, absTop);
                m.Rotate(-absRotationAngle);

                m.TransformPoints(pts);
            }

            return pts[0];
        }

        public virtual System.Drawing.Drawing2D.Matrix ToImageTransformer
        {
            get
            {
                System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();

                m.RotateAt(DeviceOrientation, new PointF(DeviceLeft, DeviceTop));

                m.Invert();

                return m;
            }
        }

        protected override void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);

            bin.Write(DeviceLeft);
            bin.Write(DeviceTop);
            bin.Write(DeviceOrientation);

            MetrologySystem.Serialize(bin);
            AlignmentSettings.Serialize(bin);

            BinarySerializationCommon.Write(bin, ReferenceImage);
            BinarySerializationCommon.Write(bin, Regions);
        }

        protected override void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            DeviceLeft = bin.ReadSingle();
            DeviceTop = bin.ReadSingle();
            DeviceOrientation = bin.ReadSingle();

            MetrologySystem = MetrologySystem.Deserialize(bin);
            AlignmentSettings = AlignmentSettings.Deserialize(bin);

            ReferenceImage = BinarySerializationCommon.ReadBytes(bin);
            Regions = BinarySerializationCommon.ReadBytes(bin);
        }

        public static MetrologySystemReference Deserialize(System.IO.BinaryReader bin)
        {
            return ParameterBase.BaseDeserialize(bin) as MetrologySystemReference;
        }

        public static MetrologySystemReference Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as MetrologySystemReference;
        }
    }

    public class ABSMetrologySystemReference : MetrologySystemReference
    {
        public const string DefaultFileName = "ABSReferenceFile.msr";
        public new static int CurrentVersion
        {
            get
            {
                return 1;
            }
        }

        public ABSMetrologySystemReference()
        {
        }

        public ABSMetrologySystemReference(
            float deviceLeft, float deviceTop, float deviceOrientation,
            MetrologySystem metrologySystem, ABSAlignmentSettings alignmentSettings,
            byte[] referenceImage, byte[] regions) : base(
            deviceLeft, deviceTop, deviceOrientation,
            metrologySystem, alignmentSettings, referenceImage, regions)
        {            
        }        

        protected new void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);            
        }

        protected new void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();
        }

        public new static ABSMetrologySystemReference Deserialize(System.IO.BinaryReader bin)
        {
            return ParameterBase.BaseDeserialize(bin) as ABSMetrologySystemReference;
        }

        public new static ABSMetrologySystemReference Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as ABSMetrologySystemReference;
        }

        public static MetrologySystemReference GetDefaultABSMetrologySystemReference()
        {
            string defaultFilePath =
                Path.Combine(
                    SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(),
                    ABSMetrologySystemReference.DefaultFileName);

            return ABSMetrologySystemReference.Deserialize(defaultFilePath);
        }
    }    
}
