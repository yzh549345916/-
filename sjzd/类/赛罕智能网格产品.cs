﻿using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Drawing.Charts;
using Aspose.Words.Tables;
using sjzd.类;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using static sjzd.环保局预报;

namespace sjzd
{
    internal class 赛罕智能网格产品
    {
        //输入数组每行内容为：旗县名称+区站号+未来三天分别的天气、风向风速、最低气温、最高气温，因此列数为2+4*3。方法将数组中的指定要素保存至发布单
        private List<IDName> iDNames = new List<IDName>();

        public List<YBList> DCWordNew(short sc, string Names, ref string myerror, ref string error)
        {
            DateTime date = DateTime.Now;

            市台检验数据库 stjysjk = new 市台检验数据库();
            List<市台检验数据库.StationList> qfLists = stjysjk.获取赛罕智能网格站点列表(14);
            string[] nameSZ = Names.Split(',');
            string strID = "";
            foreach (市台检验数据库.StationList stationList in qfLists)
            {
                foreach (string name in nameSZ)
                {
                    try
                    {
                        if (name.Trim() == stationList.Name.Trim())
                        {
                            string idLS = stationList.GJID;
                            if (!strID.Contains(idLS))
                            {
                                strID += '\'' + idLS + '\'' + ',';
                            }

                            iDNames.Add(new IDName
                            {
                                ID = stationList.ID,
                                Name = stationList.Name,
                                GJID = stationList.GJID
                            });
                        }
                    }
                    catch
                    {
                    }
                }
            }

            try
            {
                if (strID.Length > 2)
                {
                    strID = strID.Substring(0, strID.Length - 1);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return new List<YBList>();
            }

            return CLSJ(date, sc, strID, iDNames, ref myerror);
        }

        public void idnameCSH(string Names)
        {
            DateTime date = DateTime.Now;

            市台检验数据库 stjysjk = new 市台检验数据库();
            List<市台检验数据库.StationList> qfLists = stjysjk.获取赛罕智能网格站点列表(14);
            string[] nameSZ = Names.Split(',');
            foreach (市台检验数据库.StationList stationList in qfLists)
            {
                foreach (string name in nameSZ)
                {
                    try
                    {
                        if (name.Trim() == stationList.Name.Trim())
                        {
                            iDNames.Add(new IDName
                            {
                                ID = stationList.ID,
                                Name = stationList.Name,
                                GJID = stationList.GJID
                            });
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public string 处理环保数据(List<YBList> dataList, short sc, ref string error)
        {
            DateTime date = DateTime.Now;
            if (dataList.Count > 0)
            {
                try
                {
                    string SJsaPath = Environment.CurrentDirectory + @"\产品\赛罕区智能网格预报产品\";
                    SJsaPath += date.ToString("yyyy") + "\\" + date.ToString("MM") + "月\\";
                    if (!Directory.Exists(SJsaPath))
                    {
                        Directory.CreateDirectory(SJsaPath);
                    }

                    SJsaPath += date.ToString("yyyy年MM月dd日") + sc.ToString().PadLeft(2, '0') + "时周末郊游气象预报.docx";
                    Document doc = new Document(); //SJMBPath
                    DocumentBuilder builder = new DocumentBuilder(doc);
                    //builder.InsertBreak(BreakType.SectionBreakNewPage);
                    builder.PageSetup.Orientation = Orientation.Landscape; //更改纸张方向
                    builder.Font.Size = 30;
                    builder.Font.Bold = true;
                    builder.Font.Name = "微软雅黑";
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write("周末郊游气象预报");
                    builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                    builder.CellFormat.Borders.Color = Color.Black;

                    //builder.MoveToBookmark("预报日期");
                    builder.Font.Size = 14;
                    builder.Font.Name = "宋体";
                    builder.Font.Bold = false;
                    builder.Write("\r\n赛罕区气象台                                        ");
                    builder.Write(date.ToString("yyyy年MM月dd日") + sc.ToString().PadLeft(2, '0') + "时");
                    builder.InsertParagraph();
                    //builder.InsertParagraph();
                    //builder.MoveToBookmark("预报0");
                    Shape lineShape = new Shape(doc, ShapeType.Line);
                    lineShape.Width = 620;
                    Stroke stroke = lineShape.Stroke;
                    stroke.On = true;
                    stroke.Weight = 5.5;
                    stroke.Color = Color.Red;
                    stroke.LineStyle = ShapeLineStyle.ThinThick;
                    builder.InsertNode(lineShape);

                    List<Table> tables = new List<Table>();
                    Shape shape = builder.InsertChart(ChartType.Line, 600, 320);
                    builder.InsertBreak(BreakType.PageBreak);

                    Shape shapeVis = builder.InsertChart(ChartType.Line, 600, 420);
                    builder.InsertBreak(BreakType.PageBreak);

                    Shape shapeErh = builder.InsertChart(ChartType.Line, 600, 420);
                    builder.InsertBreak(BreakType.PageBreak);


                    Shape shapePre = builder.InsertChart(ChartType.Line, 600, 420);
                    builder.InsertBreak(BreakType.PageBreak);


                    Shape shapeFS = builder.InsertChart(ChartType.Line, 600, 420);
                    builder.InsertBreak(BreakType.PageBreak);
                    builder.Write(date.ToString("yyyy年MM月dd日") + sc.ToString().PadLeft(2, '0') + "时气温变化情况");

                    Table table = builder.StartTable();
                    builder.EndTable();
                    builder.InsertBreak(BreakType.PageBreak);
                    builder.Write(date.ToString("yyyy年MM月dd日") + sc.ToString().PadLeft(2, '0') + "时能见度变化情况");
                    Table tableVis = builder.StartTable();
                    builder.EndTable();
                    builder.InsertBreak(BreakType.PageBreak);
                    builder.Write(date.ToString("yyyy年MM月dd日") + sc.ToString().PadLeft(2, '0') + "时相对湿度变化情况");
                    Table tableErh = builder.StartTable();
                    builder.EndTable();
                    ; builder.InsertBreak(BreakType.PageBreak);
                    builder.Write(date.ToString("yyyy年MM月dd日") + sc.ToString().PadLeft(2, '0') + "时降水量变化情况");
                    Table tablePre = builder.StartTable();
                    builder.EndTable();
                    ; builder.InsertBreak(BreakType.PageBreak);
                    builder.Write(date.ToString("yyyy年MM月dd日") + sc.ToString().PadLeft(2, '0') + "时风向风速变化情况");

                    Table tableFS = builder.StartTable();
                    Table tableFX = builder.StartTable();


                    tables.Add(table);
                    tables.Add(tableVis);
                    tables.Add(tableErh);
                    tables.Add(tablePre);
                    tables.Add(tableFS);
                    tables.Add(tableFX);
                    Chart chart = shape.Chart;
                    Chart chartVis = shapeVis.Chart;
                    Chart chartErh = shapeErh.Chart;
                    Chart chartPre = shapePre.Chart;
                    Chart chartFS = shapeFS.Chart;
                    chart.Series.Clear();
                    chartVis.Series.Clear();
                    chartErh.Series.Clear();
                    chartPre.Series.Clear();
                    chartFS.Series.Clear();
                    //chart.AxisX.TickLabelSpacing = 1;//坐标间隔
                    chart.AxisX.Crosses = AxisCrosses.Minimum;
                    chartVis.AxisX.Crosses = AxisCrosses.Minimum;
                    chart.Title.Text = date.ToString("yyyy年MM月dd日") + sc.ToString().PadLeft(2, '0') + "时气温变化情况";
                    chartVis.Title.Text = date.ToString("yyyy年MM月dd日") + sc.ToString().PadLeft(2, '0') + "时能见度变化情况";
                    chartErh.Title.Text = date.ToString("yyyy年MM月dd日") + sc.ToString().PadLeft(2, '0') + "时相对湿度变化情况";
                    chartPre.Title.Text = date.ToString("yyyy年MM月dd日") + sc.ToString().PadLeft(2, '0') + "时降水量变化情况";
                    chartFS.Title.Text = date.ToString("yyyy年MM月dd日") + sc.ToString().PadLeft(2, '0') + "时风速变化情况";
                    int visMin = 999999; //保存最小能见度，确定坐标轴
                    for (int i = 0; i < iDNames.Count; i++)
                    {
                        string[] timsSZ = new string[24];
                        double[] temSZ = new double[24];
                        double[] visSZ = new double[24];
                        double[] erhSZ = new double[24];
                        double[] preSZ = new double[24];
                        double[] fsSZ = new double[24];
                        string[] fxSZ = new string[24];
                        List<YBList> lists1 = dataList.FindAll(y => y.ID == iDNames[i].ID).OrderBy(y => y.SX).ToList();
                        DateTime dt1 = Convert.ToDateTime(date.ToString("yyyy-MM-dd"));
                        dt1 = dt1.AddHours(sc);
                        foreach (YBList yBList in lists1)
                        {
                            timsSZ[yBList.SX / 3 - 1] = dt1.AddHours(yBList.SX).ToString("dd日HH时");
                            temSZ[yBList.SX / 3 - 1] = yBList.TEM;
                            visSZ[yBList.SX / 3 - 1] = yBList.VIS;
                            erhSZ[yBList.SX / 3 - 1] = yBList.ERH;
                            preSZ[yBList.SX / 3 - 1] = yBList.PRE;
                            fsSZ[yBList.SX / 3 - 1] = yBList.doubleFS;
                            fxSZ[yBList.SX / 3 - 1] = yBList.FX;
                            if (visMin > yBList.VIS)
                                visMin = yBList.VIS;
                        }

                        ChartSeries series0 = chart.Series.Add(iDNames[i].Name, timsSZ, temSZ);
                        ChartSeries series1 = chartVis.Series.Add(iDNames[i].Name, timsSZ, visSZ);
                        ChartSeries series2 = chartErh.Series.Add(iDNames[i].Name, timsSZ, erhSZ);
                        ChartSeries series3 = chartPre.Series.Add(iDNames[i].Name, timsSZ, preSZ);
                        ChartSeries series4 = chartFS.Series.Add(iDNames[i].Name, timsSZ, fsSZ);
                        series0.Smooth = true;
                        series1.Smooth = true;
                        series2.Smooth = true;
                        series3.Smooth = true;
                        series4.Smooth = true;
                        if (i == 0)
                        {
                            // series0.Marker.Symbol = MarkerSymbol.Dash;
                            //series0.Marker.Size = 50;


                            foreach (Table tableLs in tables)
                            {
                                Row row = new Row(doc);
                                tableLs.AppendChild(row);
                                Cell cell1 = new Cell(doc);
                                row.AppendChild(cell1);
                                // Add a blank paragraph to the cell.
                                Paragraph pa = new Paragraph(doc);
                                pa.ParagraphFormat.Style.Font.Size = 7;
                                pa.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                cell1.AppendChild(pa);
                                // Add the text.
                                cell1.FirstParagraph.AppendChild(new Run(doc, "            "));
                                cell1.CellFormat.Width = 30;
                                cell1.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                                // Create the specified number of cells for each row.
                                foreach (string time in timsSZ)
                                {
                                    Cell cell = new Cell(doc);
                                    cell.CellFormat.Width = 27;
                                    cell.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                                    row.AppendChild(cell);
                                    // Add a blank paragraph to the cell.
                                    Paragraph pa1 = new Paragraph(doc);
                                    pa1.ParagraphFormat.Style.Font.Size = 7;
                                    pa1.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                    cell.AppendChild(pa1);

                                    // Add the text.
                                    cell.FirstParagraph.AppendChild(new Run(doc, time));
                                }


                                tableLs.AutoFit(AutoFitBehavior.FixedColumnWidths);
                            }
                        }

                        for (int j = 0; j < tables.Count; j++)
                        {
                            Table tableLs = tables[j];

                            Row row = new Row(doc);
                            tableLs.AppendChild(row);
                            Cell cell1 = new Cell(doc);
                            row.AppendChild(cell1);
                            // Add a blank paragraph to the cell.
                            Paragraph pa = new Paragraph(doc);
                            pa.ParagraphFormat.Style.Font.Size = 7;
                            pa.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            cell1.AppendChild(pa);

                            // Add the text.
                            cell1.FirstParagraph.AppendChild(new Run(doc, iDNames[i].Name));
                            // Create the specified number of cells for each row.
                            switch (j)
                            {
                                case 0:
                                    {
                                        foreach (double data in temSZ)
                                        {
                                            Cell cell = new Cell(doc);
                                            cell.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                                            row.AppendChild(cell);
                                            // Add a blank paragraph to the cell.
                                            Paragraph pa1 = new Paragraph(doc);
                                            pa1.ParagraphFormat.Style.Font.Size = 7;
                                            pa1.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                            cell.AppendChild(pa1);


                                            // Add the text.
                                            cell.FirstParagraph.AppendChild(new Run(doc, data.ToString()));
                                        }

                                        break;
                                    }

                                case 1:
                                    foreach (double data in visSZ)
                                    {
                                        Cell cell = new Cell(doc);
                                        cell.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                                        row.AppendChild(cell);
                                        // Add a blank paragraph to the cell.
                                        Paragraph pa1 = new Paragraph(doc);
                                        pa1.ParagraphFormat.Style.Font.Size = 7;
                                        pa1.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        cell.AppendChild(pa1);

                                        // Add the text.
                                        cell.FirstParagraph.AppendChild(new Run(doc, data.ToString()));
                                    }

                                    break;
                                case 2:
                                    foreach (double data in erhSZ)
                                    {
                                        Cell cell = new Cell(doc);
                                        cell.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                                        row.AppendChild(cell);
                                        // Add a blank paragraph to the cell.
                                        Paragraph pa1 = new Paragraph(doc);
                                        pa1.ParagraphFormat.Style.Font.Size = 7;
                                        pa1.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        cell.AppendChild(pa1);

                                        // Add the text.
                                        cell.FirstParagraph.AppendChild(new Run(doc, data.ToString()));
                                    }

                                    break;
                                case 3:
                                    foreach (double data in preSZ)
                                    {
                                        Cell cell = new Cell(doc);
                                        cell.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                                        row.AppendChild(cell);
                                        // Add a blank paragraph to the cell.
                                        Paragraph pa1 = new Paragraph(doc);
                                        pa1.ParagraphFormat.Style.Font.Size = 7;
                                        pa1.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        cell.AppendChild(pa1);

                                        // Add the text.
                                        cell.FirstParagraph.AppendChild(new Run(doc, data.ToString()));
                                    }

                                    break;
                                case 4:
                                    foreach (double data in fsSZ)
                                    {
                                        Cell cell = new Cell(doc);
                                        cell.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                                        row.AppendChild(cell);
                                        // Add a blank paragraph to the cell.
                                        Paragraph pa1 = new Paragraph(doc);
                                        pa1.ParagraphFormat.Style.Font.Size = 7;
                                        pa1.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        cell.AppendChild(pa1);

                                        // Add the text.
                                        cell.FirstParagraph.AppendChild(new Run(doc, data.ToString("F2")));
                                    }

                                    break;
                                default:
                                    foreach (string data in fxSZ)
                                    {
                                        Cell cell = new Cell(doc);
                                        cell.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                                        row.AppendChild(cell);
                                        // Add a blank paragraph to the cell.
                                        Paragraph pa1 = new Paragraph(doc);
                                        pa1.ParagraphFormat.Style.Font.Size = 7;
                                        pa1.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        cell.AppendChild(pa1);

                                        // Add the text.
                                        cell.FirstParagraph.AppendChild(new Run(doc, data));
                                    }

                                    break;
                            }
                        }
                    }
                    //更改坐标轴

                    chartVis.AxisY.Scaling.Minimum = new AxisBound(Convert.ToDouble(Math.Floor(Convert.ToDecimal(visMin / 1000)) * 1000));
                    doc.Save(SJsaPath);
                    return SJsaPath;
                }

                catch (Exception)
                {
                }
            }
            else
            {
                error = "国家级智能网格数据获取失败，无法制作产品";
            }

            return "";
        }

        public string GetFXFS(double v, double u)
        {
            string fxfs = "";
            double fx = 999.9; //风向

            if ((u > 0) & (v > 0))
            {
                fx = 270 - Math.Atan(v / u) * 180 / Math.PI;
            }
            else if ((u < 0) & (v > 0))
            {
                fx = 90 - Math.Atan(v / u) * 180 / Math.PI;
            }
            else if ((u < 0) & (v < 0))
            {
                fx = 90 - Math.Atan(v / u) * 180 / Math.PI;
            }
            else if ((u > 0) & (v < 0))
            {
                fx = 270 - Math.Atan(v / u) * 180 / Math.PI;
            }
            else if ((u == 0) & (v > 0))
            {
                fx = 180;
            }
            else if ((u == 0) & (v < 0))
            {
                fx = 0;
            }
            else if ((u > 0) & (v == 0))
            {
                fx = 270;
            }
            else if ((u < 0) & (v == 0))
            {
                fx = 90;
            }
            else if ((u == 0) & (v == 0))
            {
                fx = 999.9;
            }

            //风速是uv分量的平方和

            double fs = Math.Sqrt(Math.Pow(u, 2) + Math.Pow(v, 2));
            int intfx = Convert.ToInt32(Math.Round(fx / 45, 0));
            switch (intfx)
            {
                case 0:
                    fxfs = "北风";
                    break;
                case 1:
                    fxfs = "东北风";
                    break;
                case 2:
                    fxfs = "东风";
                    break;
                case 3:
                    fxfs = "东南风";
                    break;
                case 4:
                    fxfs = "南风";
                    break;
                case 5:
                    fxfs = "西南风";
                    break;
                case 6:
                    fxfs = "西风";
                    break;
                case 7:
                    fxfs = "西北风";
                    break;
                case 999017:
                    fxfs = "静风";
                    break;
                default:
                    fxfs = "北风";
                    break;
            }

            fxfs += ',';
            if (fs >= 0 && fs <= 0.2)
            {
                fxfs += "0级";
            }
            else if (fs >= 0.3 && fs <= 1.5)
            {
                fxfs += "1级";
            }
            else if (fs >= 1.6 && fs <= 3.3)
            {
                fxfs += "2级";
            }
            else if (fs >= 3.4 && fs <= 5.4)
            {
                fxfs += "3级";
            }
            else if (fs >= 5.5 && fs <= 7.9)
            {
                fxfs += "4级";
            }
            else if (fs >= 8 && fs <= 10.7)
            {
                fxfs += "5级";
            }
            else if (fs >= 10.8 && fs <= 13.8)
            {
                fxfs += "6级";
            }
            else if (fs >= 13.9 && fs <= 17.1)
            {
                fxfs += "7级";
            }
            else if (fs >= 17.2 && fs <= 20.7)
            {
                fxfs += "8级";
            }
            else if (fs >= 20.8 && fs <= 24.4)
            {
                fxfs += "9级";
            }
            else if (fs >= 24.5 && fs <= 28.4)
            {
                fxfs += "10级";
            }
            else if (fs >= 28.5 && fs <= 32.6)
            {
                fxfs += "11级";
            }
            else if (fs >= 32.7 && fs <= 36.9)
            {
                fxfs += "12级";
            }
            else if (fs >= 37 && fs <= 41.4)
            {
                fxfs += "13级";
            }
            else if (fs >= 41.5 && fs <= 46.1)
            {
                fxfs += "14级";
            }
            else if (fs >= 46.2 && fs <= 50.9)
            {
                fxfs += "15级";
            }
            else if (fs >= 51 && fs <= 56)
            {
                fxfs += "16级";
            }
            else if (fs >= 56.1 && fs <= 61.2)
            {
                fxfs += "17级";
            }
            else if (fs >= 61.3)
            {
                fxfs += "17级以上";
            }
            else
            {
                fxfs += "3级";
            }

            return fxfs;
        }

        public double GetFS(double v, double u)
        {
            double fx = 999.9; //风向

            if ((u > 0) & (v > 0))
            {
                fx = 270 - Math.Atan(v / u) * 180 / Math.PI;
            }
            else if ((u < 0) & (v > 0))
            {
                fx = 90 - Math.Atan(v / u) * 180 / Math.PI;
            }
            else if ((u < 0) & (v < 0))
            {
                fx = 90 - Math.Atan(v / u) * 180 / Math.PI;
            }
            else if ((u > 0) & (v < 0))
            {
                fx = 270 - Math.Atan(v / u) * 180 / Math.PI;
            }
            else if ((u == 0) & (v > 0))
            {
                fx = 180;
            }
            else if ((u == 0) & (v < 0))
            {
                fx = 0;
            }
            else if ((u > 0) & (v == 0))
            {
                fx = 270;
            }
            else if ((u < 0) & (v == 0))
            {
                fx = 90;
            }
            else if ((u == 0) & (v == 0))
            {
                fx = 999.9;
            }

            //风速是uv分量的平方和

            double fs = Math.Sqrt(Math.Pow(u, 2) + Math.Pow(v, 2));
            int intfx = Convert.ToInt32(Math.Round(fx / 45, 0));


            return fs;
        }

        public List<YBList> CLSJ(DateTime date, short sc, string strID, List<IDName> iDNames, ref string error)
        {
            List<YBList> list = new List<YBList>();

            try
            {
                XmlConfig util = new XmlConfig(Environment.CurrentDirectory + @"\设置文件\智能网格设置.xml");
                string con = util.Read("OtherConfig", "xzjxhDB");


                con = util.Read("OtherConfig", "DB");
                using (SqlConnection mycon = new SqlConnection(con))
                {
                    mycon.Open(); //打开
                    string sql = string.Format("select * from 全国智能网格预报服务产品3h240 where StatioID in ({0}) and sc='{2}'and sx in (3,6,9,12,15,18,21,24,27,30,33,36,39,42,45,48,51,54,57,60,63,66,69,72) and date='{1}'", strID, date.ToString("yyyy-MM-dd"), sc);
                    SqlCommand sqlman = new SqlCommand(sql, mycon);
                    SqlDataReader sqlreader = sqlman.ExecuteReader();
                    if (sqlreader.HasRows)
                    {
                        while (sqlreader.Read())
                        {
                            try
                            {
                                double v = sqlreader.IsDBNull(sqlreader.GetOrdinal("WIV10")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("WIV10")), 2), u = sqlreader.IsDBNull(sqlreader.GetOrdinal("WIU10")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("WIU10")), 2);
                                string fxfs = GetFXFS(v, u);

                                List<IDName> ll = iDNames.FindAll(y => y.GJID == sqlreader.GetString(sqlreader.GetOrdinal("StatioID"))).ToList();
                                for (int j = 0; j < ll.Count; j++)
                                {
                                    list.Add(new YBList
                                    {
                                        Name = ll[j].Name,
                                        ID = ll[j].ID,
                                        TEM = sqlreader.IsDBNull(sqlreader.GetOrdinal("TEM")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("TEM")), 2),
                                        ERH = sqlreader.IsDBNull(sqlreader.GetOrdinal("ERH")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("ERH")), 2),
                                        PRE = sqlreader.IsDBNull(sqlreader.GetOrdinal("PRE_3H")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("PRE_3H")), 1),
                                        SX = sqlreader.GetInt16(sqlreader.GetOrdinal("SX")),
                                        FX = fxfs.Split(',')[0],
                                        FS = fxfs.Split(',')[1],
                                        doubleFS = GetFS(v, u),
                                        VIS = sqlreader.IsDBNull(sqlreader.GetOrdinal("VIS")) ? -999999 : Convert.ToInt32(Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("VIS")), 0))
                                    });
                                }
                            }
                            catch (Exception ex1)
                            {
                                error += ex1.Message + "\r\n";
                                list.Clear();
                                sqlreader.Close();
                                DateTime dt1 = date;
                                short sc1 = sc;
                                if (sc == 8)
                                {
                                    sc1 = 20;
                                    dt1 = dt1.AddDays(-1);
                                }
                                else
                                    sc1 = 8;

                                sql = string.Format("select * from 全国智能网格预报服务产品3h240 where StatioID in ({0}) and sc='{2}' and sx in (15,18,21,24,27,30,33,36,39,42,45,48,51,54,57,60,63,66,69,72,75,78,81,84) and date='{1}'", strID, dt1.ToString("yyyy-MM-dd"), sc1);
                                sqlman = new SqlCommand(sql, mycon);
                                sqlreader = sqlman.ExecuteReader();
                                if (sqlreader.HasRows)
                                {
                                    while (sqlreader.Read())
                                    {
                                        try
                                        {
                                            double v = sqlreader.IsDBNull(sqlreader.GetOrdinal("WIV10")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("WIV10")), 2), u = sqlreader.IsDBNull(sqlreader.GetOrdinal("WIU10")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("WIU10")), 2);
                                            string fxfs = GetFXFS(v, u);
                                            List<IDName> ll = iDNames.FindAll(y => y.GJID == sqlreader.GetString(sqlreader.GetOrdinal("StatioID"))).ToList();
                                            for (int j = 0; j < ll.Count; j++)
                                            {
                                                list.Add(new YBList
                                                {
                                                    Name = ll[j].Name,
                                                    ID = ll[j].ID,
                                                    TEM = sqlreader.IsDBNull(sqlreader.GetOrdinal("TEM")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("TEM")), 2),
                                                    ERH = sqlreader.IsDBNull(sqlreader.GetOrdinal("ERH")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("ERH")), 2),
                                                    PRE = sqlreader.IsDBNull(sqlreader.GetOrdinal("PRE_3H")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("PRE_3H")), 1),
                                                    SX = (short)(sqlreader.GetInt16(sqlreader.GetOrdinal("SX")) - 12),
                                                    FX = fxfs.Split(',')[0],
                                                    FS = fxfs.Split(',')[1],
                                                    doubleFS = GetFS(v, u),
                                                    VIS = sqlreader.IsDBNull(sqlreader.GetOrdinal("VIS")) ? -999999 : Convert.ToInt32(Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("VIS")), 0))
                                                });
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            error += ex.Message + "\r\n";
                                        }
                                    }
                                }

                                return list;
                            }
                        }
                    }
                    else
                    {
                        sqlreader.Close();
                        DateTime dt1 = date;
                        short sc1 = sc;
                        if (sc == 8)
                        {
                            sc1 = 20;
                            dt1 = dt1.AddDays(-1);
                        }
                        else
                            sc1 = 8;

                        sql = string.Format("select * from 全国智能网格预报服务产品3h240 where StatioID in ({0}) and sc='{2}' and sx in (15,18,21,24,27,30,33,36,39,42,45,48,51,54,57,60,63,66,69,72,75,78,81,84) and date='{1}'", strID, dt1.ToString("yyyy-MM-dd"), sc1);
                        sqlman = new SqlCommand(sql, mycon);
                        sqlreader = sqlman.ExecuteReader();
                        if (sqlreader.HasRows)
                        {
                            while (sqlreader.Read())
                            {
                                try
                                {
                                    double v = sqlreader.IsDBNull(sqlreader.GetOrdinal("WIV10")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("WIV10")), 2), u = sqlreader.IsDBNull(sqlreader.GetOrdinal("WIU10")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("WIU10")), 2);
                                    string fxfs = GetFXFS(v, u);
                                    List<IDName> ll = iDNames.FindAll(y => y.GJID == sqlreader.GetString(sqlreader.GetOrdinal("StatioID"))).ToList();
                                    for (int j = 0; j < ll.Count; j++)
                                    {
                                        try
                                        {
                                            list.Add(new YBList
                                            {
                                                Name = ll[j].Name,
                                                ID = ll[j].ID,
                                                TEM = sqlreader.IsDBNull(sqlreader.GetOrdinal("TEM")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("TEM")), 2),
                                                ERH = sqlreader.IsDBNull(sqlreader.GetOrdinal("ERH")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("ERH")), 2),
                                                PRE = sqlreader.IsDBNull(sqlreader.GetOrdinal("PRE_3H")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("PRE_3H")), 1),
                                                SX = (short)(sqlreader.GetInt16(sqlreader.GetOrdinal("SX")) - 12),
                                                FX = fxfs.Split(',')[0],
                                                FS = fxfs.Split(',')[1],
                                                doubleFS = GetFS(v, u),
                                                VIS = sqlreader.IsDBNull(sqlreader.GetOrdinal("VIS")) ? -999999 : Convert.ToInt32(Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("VIS")), 0))
                                            });
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    error += ex.Message + "\r\n";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            if (list.Count > 1)
            {
                list = list.OrderBy(y => y.ID).ThenBy(y => y.SX).ToList();
                if (iDNames.Count * 24 != list.Count)
                {
                    for (short i = 3; i < 73; i = (short)(i + 3))
                    {
                        foreach (IDName j in iDNames)
                        {
                            if (!list.Exists(y => y.ID == j.ID && y.SX == i)) //如果数据遗漏
                            {
                                try
                                {
                                    if (list.Exists(y => y.ID == j.ID && y.SX == i - 3)) //如果前一时次的数据存在，则用前一时次的数据弥补
                                    {
                                        YBList ybList = list.Find(y => y.ID == j.ID && y.SX == i - 3);
                                        list.Add(new YBList
                                        {
                                            Name = j.Name,
                                            ID = j.ID,
                                            TEM = ybList.TEM,
                                            ERH = ybList.ERH,
                                            PRE = ybList.PRE,
                                            SX = i,
                                            FX = ybList.FX,
                                            FS = ybList.FS,
                                            doubleFS = ybList.doubleFS,
                                            VIS = ybList.VIS
                                        });
                                        error += j.Name + i + "小时数据不存在，已经用" + (i - 3) + "小时数据代替" + "\r\n";
                                    }
                                    else if (list.Exists(y => y.ID == j.ID && y.SX == i + 3)) //如果后一时次的数据存在，则用后一时次的数据弥补
                                    {
                                        YBList ybList = list.Find(y => y.ID == j.ID && y.SX == i + 3);
                                        list.Add(new YBList
                                        {
                                            Name = j.Name,
                                            ID = j.ID,
                                            TEM = ybList.TEM,
                                            ERH = ybList.ERH,
                                            PRE = ybList.PRE,
                                            SX = i,
                                            FX = ybList.FX,
                                            FS = ybList.FS,
                                            doubleFS = ybList.doubleFS,
                                            VIS = ybList.VIS
                                        });
                                        error += j.Name + i + "小时数据不存在，已经用" + (i + 3) + "小时数据代替" + "\r\n";
                                    }
                                    else //如果临近两个时次都不存在，则查找前一个起报时次
                                    {
                                        try
                                        {
                                            XmlConfig util = new XmlConfig(Environment.CurrentDirectory + @"\设置文件\智能网格设置.xml");
                                            string con = util.Read("OtherConfig", "xzjxhDB");
                                            using (SqlConnection mycon = new SqlConnection(con))
                                            {
                                                mycon.Open(); //打开
                                                DateTime dt1 = date;
                                                short sc1 = sc;
                                                if (sc == 8)
                                                {
                                                    sc1 = 20;
                                                    dt1 = dt1.AddDays(-1);
                                                }
                                                else
                                                    sc1 = 8;

                                                string sql = string.Format("select * from 全国智能网格预报服务产品3h240 where StatioID = ({0}) and sc='{1}' and sx ='{2}'", j.ID, sc1, i + 12);
                                                SqlCommand sqlman = new SqlCommand(sql, mycon);
                                                SqlDataReader sqlreader = sqlman.ExecuteReader();
                                                while (sqlreader.Read())
                                                {
                                                    try
                                                    {
                                                        double v = sqlreader.IsDBNull(sqlreader.GetOrdinal("WIV10")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("WIV10")), 2), u = sqlreader.IsDBNull(sqlreader.GetOrdinal("WIU10")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("WIU10")), 2);
                                                        string fxfs = GetFXFS(v, u);
                                                        list.Add(new YBList
                                                        {
                                                            Name = j.Name,
                                                            ID = j.ID,
                                                            TEM = sqlreader.IsDBNull(sqlreader.GetOrdinal("TEM")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("TEM")), 2),
                                                            ERH = sqlreader.IsDBNull(sqlreader.GetOrdinal("ERH")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("ERH")), 2),
                                                            PRE = sqlreader.IsDBNull(sqlreader.GetOrdinal("PRE_3H")) ? -999999 : Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("PRE_3H")), 2),
                                                            SX = i,
                                                            FX = fxfs.Split(',')[0],
                                                            FS = fxfs.Split(',')[1],
                                                            doubleFS = GetFS(v, u),
                                                            VIS = sqlreader.IsDBNull(sqlreader.GetOrdinal("VIS")) ? -999999 : Convert.ToInt32(Math.Round(sqlreader.GetFloat(sqlreader.GetOrdinal("VIS")), 0))
                                                        });
                                                        error += j.Name + i + "小时数据不存在，已经用" + sc1 + "时的数据代替" + "\r\n";
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        error += ex.Message + "\r\n";
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            error += ex.Message + "\r\n";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    error += ex.Message + "\r\n";
                                }
                            }
                        }
                    }
                }

                for (short i = 3; i < 73; i = (short)(i + 3))
                {
                    foreach (IDName j in iDNames)
                    {
                        YBList itemLS = list.First(y => y.ID == j.ID && y.SX == i);
                        if (Math.Abs(itemLS.TEM) > 100) //如果数据异常
                        {
                            try
                            {
                                if (list.Exists(y => y.ID == j.ID && y.SX == i - 3)) //如果前一时次的数据存在，则用前一时次的数据弥补
                                {
                                    YBList ybList = list.Find(y => y.ID == j.ID && y.SX == i - 3);
                                    if (Math.Abs(ybList.TEM) < 100)
                                    {
                                        list.First(y => y.ID == j.ID && y.SX == i).TEM = ybList.TEM;
                                        error += j.Name + i + "小时温度数据异常，已经用" + (i - 3) + "小时数据代替" + "\r\n";
                                    }
                                    else if (list.Exists(y => y.ID == j.ID && y.SX == i + 3)) //如果后一时次的数据存在，则用后一时次的数据弥补
                                    {
                                        ybList = list.Find(y => y.ID == j.ID && y.SX == i + 3);
                                        if (Math.Abs(ybList.TEM) < 100)
                                        {
                                            list.First(y => y.ID == j.ID && y.SX == i).TEM = ybList.TEM;
                                            error += j.Name + i + "小时温度数据异常，已经用" + (i + 3) + "小时数据代替" + "\r\n";
                                        }
                                    }
                                }
                                else if (list.Exists(y => y.ID == j.ID && y.SX == i + 3)) //如果后一时次的数据存在，则用后一时次的数据弥补
                                {
                                    YBList ybList = list.Find(y => y.ID == j.ID && y.SX == i + 3);
                                    if (Math.Abs(ybList.TEM) < 100)
                                    {
                                        list.First(y => y.ID == j.ID && y.SX == i).TEM = ybList.TEM;
                                        error += j.Name + i + "小时温度数据异常，已经用" + (i + 3) + "小时数据代替" + "\r\n";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                error += ex.Message + "\r\n";
                            }
                        }

                        if (Math.Abs(itemLS.PRE) > 1000) //如果数据异常
                        {
                            try
                            {
                                if (list.Exists(y => y.ID == j.ID && y.SX == i - 3)) //如果前一时次的数据存在，则用前一时次的数据弥补
                                {
                                    YBList ybList = list.Find(y => y.ID == j.ID && y.SX == i - 3);
                                    if (Math.Abs(ybList.PRE) < 1000)
                                    {
                                        list.First(y => y.ID == j.ID && y.SX == i).PRE = ybList.PRE;
                                        error += j.Name + i + "小时降水量数据异常，已经用" + (i - 3) + "小时数据代替" + "\r\n";
                                    }
                                    else if (list.Exists(y => y.ID == j.ID && y.SX == i + 3)) //如果后一时次的数据存在，则用后一时次的数据弥补
                                    {
                                        ybList = list.Find(y => y.ID == j.ID && y.SX == i + 3);
                                        if (Math.Abs(ybList.PRE) < 1000)
                                        {
                                            list.First(y => y.ID == j.ID && y.SX == i).PRE = ybList.PRE;
                                            error += j.Name + i + "小时降水量数据异常，已经用" + (i + 3) + "小时数据代替" + "\r\n";
                                        }
                                    }
                                }
                                else if (list.Exists(y => y.ID == j.ID && y.SX == i + 3)) //如果后一时次的数据存在，则用后一时次的数据弥补
                                {
                                    YBList ybList = list.Find(y => y.ID == j.ID && y.SX == i + 3);
                                    if (Math.Abs(ybList.PRE) < 1000)
                                    {
                                        list.First(y => y.ID == j.ID && y.SX == i).PRE = ybList.PRE;
                                        error += j.Name + i + "小时降水量数据异常，已经用" + (i + 3) + "小时数据代替" + "\r\n";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                error += ex.Message + "\r\n";
                            }
                        }

                        if (Math.Abs(itemLS.doubleFS) > 1000) //如果数据异常
                        {
                            try
                            {
                                if (list.Exists(y => y.ID == j.ID && y.SX == i - 3)) //如果前一时次的数据存在，则用前一时次的数据弥补
                                {
                                    YBList ybList = list.Find(y => y.ID == j.ID && y.SX == i - 3);
                                    if (Math.Abs(ybList.doubleFS) < 1000)
                                    {
                                        list.First(y => y.ID == j.ID && y.SX == i).doubleFS = ybList.doubleFS;
                                        list.First(y => y.ID == j.ID && y.SX == i).FX = ybList.FX;
                                        error += j.Name + i + "小时风数据异常，已经用" + (i - 3) + "小时数据代替" + "\r\n";
                                    }
                                    else if (list.Exists(y => y.ID == j.ID && y.SX == i + 3)) //如果后一时次的数据存在，则用后一时次的数据弥补
                                    {
                                        ybList = list.Find(y => y.ID == j.ID && y.SX == i + 3);
                                        if (Math.Abs(ybList.doubleFS) < 1000)
                                        {
                                            list.First(y => y.ID == j.ID && y.SX == i).doubleFS = ybList.doubleFS;
                                            list.First(y => y.ID == j.ID && y.SX == i).FX = ybList.FX;
                                            error += j.Name + i + "小时风数据异常，已经用" + (i + 3) + "小时数据代替" + "\r\n";
                                        }
                                    }
                                }
                                else if (list.Exists(y => y.ID == j.ID && y.SX == i + 3)) //如果后一时次的数据存在，则用后一时次的数据弥补
                                {
                                    YBList ybList = list.Find(y => y.ID == j.ID && y.SX == i + 3);
                                    if (Math.Abs(ybList.doubleFS) < 1000)
                                    {
                                        list.First(y => y.ID == j.ID && y.SX == i).doubleFS = ybList.doubleFS;
                                        list.First(y => y.ID == j.ID && y.SX == i).FX = ybList.FX;
                                        error += j.Name + i + "小时风数据异常，已经用" + (i + 3) + "小时数据代替" + "\r\n";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                error += ex.Message + "\r\n";
                            }
                        }

                        if (Math.Abs(itemLS.ERH) > 100) //如果数据异常
                        {
                            try
                            {
                                if (list.Exists(y => y.ID == j.ID && y.SX == i - 3)) //如果前一时次的数据存在，则用前一时次的数据弥补
                                {
                                    YBList ybList = list.Find(y => y.ID == j.ID && y.SX == i - 3);
                                    if (Math.Abs(ybList.ERH) <= 100)
                                    {
                                        list.First(y => y.ID == j.ID && y.SX == i).ERH = ybList.ERH;
                                        error += j.Name + i + "小时相对湿度数据异常，已经用" + (i - 3) + "小时数据代替" + "\r\n";
                                    }
                                    else if (list.Exists(y => y.ID == j.ID && y.SX == i + 3)) //如果后一时次的数据存在，则用后一时次的数据弥补
                                    {
                                        ybList = list.Find(y => y.ID == j.ID && y.SX == i + 3);
                                        if (Math.Abs(ybList.ERH) <= 100)
                                        {
                                            list.First(y => y.ID == j.ID && y.SX == i).ERH = ybList.ERH;
                                            error += j.Name + i + "小时相对湿度数据异常，已经用" + (i + 3) + "小时数据代替" + "\r\n";
                                        }
                                    }
                                }
                                else if (list.Exists(y => y.ID == j.ID && y.SX == i + 3)) //如果后一时次的数据存在，则用后一时次的数据弥补
                                {
                                    YBList ybList = list.Find(y => y.ID == j.ID && y.SX == i + 3);
                                    if (Math.Abs(ybList.ERH) <= 100)
                                    {
                                        list.First(y => y.ID == j.ID && y.SX == i).ERH = ybList.ERH;
                                        error += j.Name + i + "小时相对湿度数据异常，已经用" + (i + 3) + "小时数据代替" + "\r\n";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                error += ex.Message + "\r\n";
                            }
                        }

                        if (itemLS.VIS < 0) //如果数据异常
                        {
                            try
                            {
                                if (list.Exists(y => y.ID == j.ID && y.SX == i - 3)) //如果前一时次的数据存在，则用前一时次的数据弥补
                                {
                                    YBList ybList = list.Find(y => y.ID == j.ID && y.SX == i - 3);
                                    if (itemLS.VIS < 0)
                                    {
                                        list.First(y => y.ID == j.ID && y.SX == i).VIS = ybList.VIS;
                                        error += j.Name + i + "小时能见度数据异常，已经用" + (i - 3) + "小时数据代替" + "\r\n";
                                    }
                                    else if (list.Exists(y => y.ID == j.ID && y.SX == i + 3)) //如果后一时次的数据存在，则用后一时次的数据弥补
                                    {
                                        ybList = list.Find(y => y.ID == j.ID && y.SX == i + 3);
                                        if (itemLS.VIS < 0)
                                        {
                                            list.First(y => y.ID == j.ID && y.SX == i).VIS = ybList.VIS;
                                            error += j.Name + i + "小时能见度数据异常，已经用" + (i + 3) + "小时数据代替" + "\r\n";
                                        }
                                    }
                                }
                                else if (list.Exists(y => y.ID == j.ID && y.SX == i + 3)) //如果后一时次的数据存在，则用后一时次的数据弥补
                                {
                                    YBList ybList = list.Find(y => y.ID == j.ID && y.SX == i + 3);
                                    if (itemLS.VIS < 0)
                                    {
                                        list.First(y => y.ID == j.ID && y.SX == i).VIS = ybList.VIS;
                                        error += j.Name + i + "小时能见度数据异常，已经用" + (i + 3) + "小时数据代替" + "\r\n";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                error += ex.Message + "\r\n";
                            }
                        }
                    }
                }

                list = list.OrderBy(y => y.ID).ThenBy(y => y.SX).ToList();
            }

            return list;
        }


        public class IDName
        {
            public string ID { get; set; }
            public string GJID { get; set; }
            public string Name { get; set; }
        }
    }
}