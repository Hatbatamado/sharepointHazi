using hazi.DAL;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public class ExcelReportClass
    {
        public ExcelReportClass(HttpServerUtility Server, HttpResponse Response)
        {
            byte[] result = null;
            String path = Server.MapPath(Konstansok.ReportTemplatesPath + "ReportTemplate.xlsx");

            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (ExcelPackage p = new ExcelPackage(ms, stream))
                    {
                        using (hazi2Entities db = new hazi2Entities())
                        {
                            ExcelWorksheet ws = p.Workbook.Worksheets[1];
                            List<IdoBejelentes> bejelentesek = db.IdoBejelentes1.ToList();
                            ws.Cells["A2"].LoadFromCollection(bejelentesek.Select(
                            b => new
                            {
                                IdoBejelentesID = b.ID,
                                KezdetiDatum = DatumKiiratas(b.KezdetiDatum),
                                VegeDatum = DatumKiiratas(b.VegeDatum),
                                UserName = b.UserName,
                                LastEdit = b.UtolsoModosito,
                                LastEditTime = DatumKiiratas((DateTime)b.UtolsoModositas),
                                JovaStatusz = JovaStatuszKonv(b.Statusz),
                                JogcimID = b.JogcimID,
                                JogcimNev = b.Jogcim.Cim,
                                JogcimStatusz = b.Jogcim.Inaktiv,
                                RogBejSzin = SzinBeallit(b.Jogcim.Szin, true),
                                JovBejSzin = SzinBeallit(b.Jogcim.Szin, false)
                            }));

                            //táblázat sor változtatás
                            ws.Tables[0].TableXml.InnerXml =
                                ws.Tables[0].TableXml.InnerXml.Replace("ref=\"A1:L2\"",
                                String.Format("ref=\"A1:L{0}\"", bejelentesek.Count + 1));

                            result = p.GetAsByteArray();
                        }
                    }
                }
            }
            if (result != null)
            {
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.AddHeader("content-disposition", "attachment;filename=Report"
                    + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xlsx");
                Response.BinaryWrite(result);
                Response.Flush();
                Response.End();
            }
        }

        private string JovaStatuszKonv(string statusz)
        {
            string[] seged = statusz.Split('&');
            if (seged.Length > 1)
                return seged[1];
            else
                return "Nincs";
        }

        private string SzinBeallit(string szin, bool Elso)
        {
            string[] seged = szin.Split('#');
            if (seged.Length > 2)
            {
                if (Elso)
                    return seged[1];
                else
                    return seged[2];
            }
            else
            {
                if (Elso)
                    return Konstansok.RogzitveAlapSzin;
                else
                    return Konstansok.JovahagyvaAlapSzin;
            }
        }

        private string DatumKiiratas(DateTime datum)
        {
            return datum.ToString();
        }
    }
}