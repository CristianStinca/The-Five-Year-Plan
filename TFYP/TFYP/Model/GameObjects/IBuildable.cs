using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.Model.GameObjects
{
    internal struct Coordinate
    {
        public Coordinate(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        int x { get; } // column
        int y { get; } //  row
    }

    internal struct Dimension
    {
        public Dimension(int _width, int _height)
        {
            width = _width;
            height = _height;
        }

        int width { get; } // column
        int height { get; } //  row

        public static Dimension DEFAULT = new Dimension(1, 1);
    }

    enum EBuildable
    {
        None,
        Stadium
    }

    //public static class TypesConverison
    //{
    //    private static bool isInit = false;
    //    private static Dictionary<string, string> dict = null; // can be converted into <str, List<str>> to potentially get random imgs

    //    private static void Init()
    //    {
    //        dict = new()
    //        {
    //            { EBuildable.None.ToString(), "empty_tile" },
    //            { EBuildable.Stadium.ToString(), "stadium_tile" }
    //        };
    //    }

    //    private static void CheckInit()
    //    {
    //        if (!isInit)
    //        {
    //            Init();
    //            isInit = true;
    //        }
    //    }

    //    public static Dictionary<string, string> GetDict()
    //    {
    //        CheckInit();
    //        return dict;
    //    }

    //    public static string? GetVal(string key)
    //    {
    //        CheckInit();
    //        string val = null;
    //        dict.TryGetValue(key, out val);

    //        return val;
    //    }
    //}
}
