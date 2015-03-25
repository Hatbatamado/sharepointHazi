using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public enum hibak
    {
        nincsHiba,
        HibasDatum,
        KezdetiDatumRegebbiMainal,
        HibasKezdetiIdo,
        HibasVegeIdo,
        HibasKezdetiErtekek,
        HibasVegeErtekek,
        VegeKezdetiElott,
        IbNincsDBben,
        NemEngedelyezettEleres,
        Ismeretlen
    };

    public enum vizsgalat
    {
        MasodpercNulla,
        CsakDatum
    };
}