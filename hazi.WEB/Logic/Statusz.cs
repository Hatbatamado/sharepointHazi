using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public enum TorlesStatus
    {
        Inaktiv,
        BejelentettKerelem,
        ElfogadottKerelem,
        Torles,
        Elutasitott
    };

    public enum JovaHagyasStatus
    {
        [EnumDisplayStringAttribute("Rögzítve")]
        Rogzitve,
        [EnumDisplayStringAttribute("Elutasítva")]
        Elutasitva,
        [EnumDisplayStringAttribute("Jóváhagyva")]
        Jovahagyva
    };
}