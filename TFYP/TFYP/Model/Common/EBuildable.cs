using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.City;
using TFYP.Model.Zones;
using ProtoBuf;

namespace TFYP.Model.Common
{
    [ProtoContract]
    public enum EBuildable
    {
        None,
        Stadium,
        School,
        University,
        PoliceStation,
        Residential,
        DoneResidential,
        Industrial,
        Service,
        Road,
        Inaccessible
    }
}
