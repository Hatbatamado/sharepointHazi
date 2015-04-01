using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public enum TorlesStatus
    {
        [EnumDisplayStringAttribute("Inaktív")]
        Inaktiv,
        [EnumDisplayStringAttribute("BejelentettKérelem")]
        BejelentettKerelem,
        [EnumDisplayStringAttribute("ElfogadottKérelem")]
        ElfogadottKerelem,
        [EnumDisplayStringAttribute("Törlés")]
        Torles,
        [EnumDisplayStringAttribute("Elutasított")]
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