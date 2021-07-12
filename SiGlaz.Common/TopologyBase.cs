using System;

namespace SiGlaz.Common
{
	public enum TOPOLOGY_TYPE : byte
	{
		Circle,
		Grid,
		Radial,
		Advanced
	}
	public abstract class TopologyBase : ICloneable
	{
		public	TOPOLOGY_TYPE	eType;
		public	object Clone()
		{
			return MemberwiseClone();
		}
	}
}
