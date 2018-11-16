using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Exporter.Formats;
using ElsevierMaterials.Exporter.Models;
using ElsevierMaterials.Models.Domain.Export;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterialsMVC.BL.Binders.ConditionBasic;
using ElsevierMaterialsMVC.BL.Binders.PropertyBasic;
using Ionic.Zip;

namespace ElsevierMaterialsMVC.BL.Binders.Exporter
{

    //TODO: Mogla sam da naporavim apstraktnu klasu, koja bi za svaki export vracala odgovarajuci format
    public class ExportFormaterBinder
    {


        public MemoryStream ExportData(string[] types, int[] materials, ElsevierMaterials.Models.Domain.Export.Exporter exporter)
        {

            MemoryStream zipStream = new MemoryStream();
            zipStream.Seek(0, SeekOrigin.Begin);


            using (ZipFile zip = new ZipFile())
            {


                bool atLeastOneMaterialHasProperties = false;
                foreach (var rowId in materials)
                {
                    ElsevierMaterials.Models.Domain.Export.Material material = exporter.Materials.Where(m => m.MaterialInfo.RowId == rowId).FirstOrDefault();

                    bool materialContainsPropertiesForExport = false;
                    foreach (var prop in material.Properties)
                    {
                        int notMappedPropertyId = prop.ElsBasicInfo.SourceTypeId != 0 ? prop.ElsBasicInfo.SourceTypeId : prop.ElsBasicInfo.TypeId;
                        TMPropertyTypeEnum propertyId = MapElsPropertyId(notMappedPropertyId, (SourceTypeEnum)material.MaterialInfo.SourceId); 
                       
                        if (exporter.Properties.Contains(propertyId))
                        {
                            materialContainsPropertiesForExport = true;
                            atLeastOneMaterialHasProperties = true;

                            break;
                        }
                    }

                    if (materialContainsPropertiesForExport)
                    {
                        MemoryStream mz = AddMaterialToZip(types, material, exporter);
                        mz.Seek(0, SeekOrigin.Begin);
                        zip.AddEntry(material.MaterialInfo.Name.Replace("/", "") + "_" + material.MaterialInfo.RowId + ".zip", mz);
                    }

                }
                if (atLeastOneMaterialHasProperties)
                {
                    zip.Save(zipStream);
                }
            }
            return zipStream;
        }

        public string GetPropertiesForExport(int sourceMaterialId, int sourceId, int subgroupId, List<PropertyFilter> properties, ElsevierMaterials.Models.Domain.Export.Exporter exporter, out List<PropertyFilter> propertiesForExport, IMaterialsContextUow materialContextUow)
        {
            propertiesForExport = new List<PropertyFilter>();
            PropertyBinder propertyBinder = new PropertyBinder();
            ConditionBinder conditionBinder = new ConditionBinder();
            string message = "";
            string propertes1 = "";
            string propertes2 = "";
            string ulOpen = "<ul>";
            string ulClose = "</ul>";
            string title1 = "The following properties are added to export: ";
            string title2 = "The following properties cannot be exported: ";



            foreach (var addedProperty in properties)
            {
                ElsevierMaterials.Models.Condition condition = conditionBinder.FillCondition(sourceMaterialId, sourceId, subgroupId, materialContextUow, addedProperty);

                int notMappedPropertyId = addedProperty.SourceTypeId != 0 ? addedProperty.SourceTypeId : addedProperty.TypeId;
                TMPropertyTypeEnum propertyId = MapElsPropertyId(notMappedPropertyId, (SourceTypeEnum)sourceId); 
            
                if (!exporter.Properties.Contains(propertyId))
                {
                    if (propertes2 == "")
                    {
                        propertes2 = propertes2 + ulOpen;
                    }
                    propertes2 = propertes2 + "<li>" + propertyBinder.FillPropertyName(sourceMaterialId, sourceId, subgroupId, materialContextUow, addedProperty, condition) + "</li>";
                }
                else
                {

                    propertiesForExport.Add(addedProperty);
                    if (propertes1 == "")
                    {
                        propertes1 = propertes1 + ulOpen;
                    }

                    propertes1 = propertes1 + "<li>" + propertyBinder.FillPropertyName(sourceMaterialId, sourceId, subgroupId, materialContextUow, addedProperty, condition) + "</li>";
                }
            }

            if (propertes1 != "" && propertes2 != "")
            {
                message = title1 + propertes1 + ulClose + "<br/>" + title2 + propertes2 + ulClose;
            }
            else if (propertes1 != "")
            {
                message = title1 + propertes1 + ulClose;
            }
            else if (propertes2 != "")
            {
                message = title2 + propertes2 + ulClose;
            }


            return message;
        }


        private MemoryStream AddMaterialToZip(string[] typesString, ElsevierMaterials.Models.Domain.Export.Material material, ElsevierMaterials.Models.Domain.Export.Exporter exporter)
        {



            MemoryStream msMaterial = new MemoryStream();
            msMaterial.Seek(0, SeekOrigin.Begin);

            using (ZipFile zip = new ZipFile())
            {
                foreach (var item in typesString)
                {
                    IList<ElsevierMaterials.Models.Domain.Export.Property> properties = material.Properties;

                    ExportTypeEnum exportType = (ExportTypeEnum)int.Parse(item);
                    bool hasAtLeatsOnePropertyForExport = false;
                    ExportType ep = exporter.ExportTypes.Where(m => m.ExportTypeId == exportType).FirstOrDefault();
                    IList<TMPropertyTypeEnum> propertiesForSelectedExport = ep.Properties;
                    foreach (var prop in material.Properties)
                    {
                        int notMappedPropertyId = prop.ElsBasicInfo.SourceTypeId != 0 ? prop.ElsBasicInfo.SourceTypeId : prop.ElsBasicInfo.TypeId;
                        TMPropertyTypeEnum propertyId = MapElsPropertyId(notMappedPropertyId, (SourceTypeEnum)material.MaterialInfo.SourceId);                       

                        if (propertiesForSelectedExport.Contains(propertyId))
                        {
                            hasAtLeatsOnePropertyForExport = true;
                            break;
                        }
                    }
                    if (hasAtLeatsOnePropertyForExport)
                    {
                        MemoryStream mz = getZipStream(int.Parse(item), properties, material);
                        mz.Seek(0, SeekOrigin.Begin);

                        if (exportType == ExportTypeEnum.Esi)
                        {
                            zip.AddEntry("ESI ProCAST.zip", mz);
                        }
                        else if (exportType == ExportTypeEnum.Siemens)
                        {
                            zip.AddEntry("Siemens NX.zip", mz);
                        }
                        else
                        {
                            zip.AddEntry(exportType + ".zip", mz);
                        }
                    }

                }
                zip.Save(msMaterial);
            }

            return msMaterial;
        }

        //TODO:-Ovde je mogao da se iskoristi factoyPatern koji bi vracao u memory stream u zavisnosti od tipa
        public MemoryStream getZipStream(int type, IList<ElsevierMaterials.Models.Domain.Export.Property> properties, ElsevierMaterials.Models.Domain.Export.Material material)
        {

            switch ((ExportTypeEnum)type)
            {
                case ExportTypeEnum.Radioss:
                    return getRadiosZipStream(material, properties);
                case ExportTypeEnum.Abaqus:
                    return GetAbaqusZipStream(material, properties);
                case ExportTypeEnum.SolidWorks:
                    return GetSolidWorksZipStream(material, properties);
                case ExportTypeEnum.SolidEdge:
                    return GetSolidEdgeZipStream(material, properties);
                case ExportTypeEnum.Esi:
                    return GetESIZipStream(material, properties);
                case ExportTypeEnum.ESIPamCrash:
                    return GetEsiPamCrashZipStream(material, properties);
                case ExportTypeEnum.ANSYS:
                    return GetANSYSZipStream(material, properties);
                case ExportTypeEnum.Siemens:
                    return GetSiemensZipStream(material, properties);
                case ExportTypeEnum.LsDyna:
                    return GetLsDynaZipStream(material, properties);
                case ExportTypeEnum.FEMAP:
                    return GetFEMAPZipStream(material, properties);
                case ExportTypeEnum.NASTRAN:
                    return GetNastranZipStream(material, properties);
                case ExportTypeEnum.Excel:
                    return GetKTMXlsZipStream(material, properties);
                case ExportTypeEnum.KTMXml:
                    return GetKTMXmlZipStream(material, properties);
                case ExportTypeEnum.PTCCreo:
                    return GetPTCCreoZipStream(material, properties);
                case ExportTypeEnum.AutodeskNastran:
                    return GetAutodeskNastranZipStream(material, properties);

                default:
                    break;
            }
            return null;
        }


        public MemoryStream getRadiosZipStream(Material material, IList<Property> properties)
        {

            string exportSN = "";
            string exportEN = "";
            string exportMAT1 = "";
            string exportMAT4 = "";
            string exportMATHF = "";
            MemoryStream memoryStreamEN = null;
            MemoryStream memoryStreamSN = null;

            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);

            exportMAT1 = AltairHyperworks.MAT1Export(propertiesMapped, material.MaterialInfo.Name, material.MaterialInfo.Standard);
            MemoryStream memoryStreamMAT1 = new MemoryStream();
            TextWriter twMAT1 = new StreamWriter(memoryStreamMAT1);
            twMAT1.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(exportMAT1)));
            twMAT1.Flush();

            exportMAT4 = AltairHyperworks.MAT4Export(propertiesMapped, material.MaterialInfo.Name, material.MaterialInfo.Standard);
            MemoryStream memoryStreamMAT4 = new MemoryStream();
            TextWriter twMAT4 = new StreamWriter(memoryStreamMAT4);
            twMAT4.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(exportMAT4)));
            twMAT4.Flush();

            exportMATHF = AltairHyperworks.MATHFExport(propertiesMapped, material.MaterialInfo.Name, material.MaterialInfo.Standard);
            MemoryStream memoryStreamMATHF = new MemoryStream();
            TextWriter twMATHF = new StreamWriter(memoryStreamMATHF);
            twMATHF.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(exportMATHF)));
            twMATHF.Flush();


            exportSN = AltairHyperworks.MATFATSNExport(propertiesMapped, material.MaterialInfo.Name, material.MaterialInfo.Standard);
            memoryStreamSN = new MemoryStream();
            TextWriter twSN = new StreamWriter(memoryStreamSN);
            exportSN = AltairHyperworks.MATFATSNExport(propertiesMapped, material.MaterialInfo.Name, material.MaterialInfo.Standard);
            twSN.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(exportSN)));
            twSN.Flush();

            memoryStreamEN = new MemoryStream();
            TextWriter twEN = new StreamWriter(memoryStreamEN);
            exportEN = AltairHyperworks.MATFATENExport(propertiesMapped, material.MaterialInfo.Name, material.MaterialInfo.Standard);
            twEN.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(exportEN)));
            twEN.Flush();

            MemoryStream zipStream = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                if (AltairHyperworks.HasAtLeastOnePropertyMAT1(propertiesMapped) == true)
                {
                    memoryStreamMAT1.Seek(0, SeekOrigin.Begin);
                    zip.AddEntry("MAT1.fem", memoryStreamMAT1);

                }
                if (AltairHyperworks.HasAtLeastOnePropertyMAT4(propertiesMapped) == true)
                {
                    memoryStreamMAT4.Seek(0, SeekOrigin.Begin);
                    zip.AddEntry("MAT4.fem", memoryStreamMAT4);

                }
                if (AltairHyperworks.HasAtLeastOnePropertyMATFAT_SN(propertiesMapped) == true)
                {
                    memoryStreamSN.Seek(0, SeekOrigin.Begin);
                    zip.AddEntry("MATFAT_SN.fem", memoryStreamSN);

                }
                if (AltairHyperworks.HasAtLeastOnePropertyMATFAT_EN(propertiesMapped) == true)
                {
                    memoryStreamEN.Seek(0, SeekOrigin.Begin);
                    zip.AddEntry("MATFAT_EN.fem", memoryStreamEN);

                }

                if (AltairHyperworks.HasAtLeastOnePropertyMATHF(propertiesMapped) == true)
                {
                    memoryStreamMATHF.Seek(0, SeekOrigin.Begin);
                    zip.AddEntry("MATHF.parm", memoryStreamMATHF);
                }

                zip.Save(zipStream);


                return zipStream;
            }

        }

        public MemoryStream GetAbaqusZipStream(ElsevierMaterials.Models.Domain.Export.Material material, IList<ElsevierMaterials.Models.Domain.Export.Property> properties)
        {

            string inp = Abaqus.FillAbaqus(MapProperites(properties, material.MaterialInfo.SourceId), material.MaterialInfo.Name);
            MemoryStream memoryStreamAbaqus = new MemoryStream();
            TextWriter twAbaqus = new StreamWriter(memoryStreamAbaqus);
            twAbaqus.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(inp)));
            twAbaqus.Flush();
            MemoryStream zipStreamAbaqus = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                memoryStreamAbaqus.Seek(0, SeekOrigin.Begin);

                //zip.AddEntry("Abaqus_" + material.MaterialInfo.Name.Replace(" ", "_") + "_" + material.MaterialInfo.Standard.Replace(" ", "_") + ".inp", memoryStreamAbaqus);
                zip.AddEntry("Abaqus_" + material.MaterialInfo.Name.Replace(" ", "_").Replace("/", "") + ".inp", memoryStreamAbaqus);
                zip.Save(zipStreamAbaqus);
            }
            return zipStreamAbaqus;
        }

        private MemoryStream GetSolidWorksZipStream(Material material, IList<Property> properties)
        {
            bool isSteel = material.MaterialInfo.TypeId == 710 ? true : false;

            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);

            XDocument sldmat = SolidWorks.FillXMLSolidWork(material.MaterialInfo.Standard, material.MaterialInfo.Name, isSteel, propertiesMapped);
            MemoryStream memoryStreamSolidWorks = new MemoryStream();
            TextWriter twSolidWorks = new StreamWriter(memoryStreamSolidWorks);
            twSolidWorks.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(sldmat.ToString())));
            twSolidWorks.Flush();
            MemoryStream zipStreamSolidWorks = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                memoryStreamSolidWorks.Seek(0, SeekOrigin.Begin);
                zip.AddEntry("SolidWorks " + material.MaterialInfo.Name.Replace("/", "") + " " + material.MaterialInfo.Standard.Replace("-", "") + ".sldmat", memoryStreamSolidWorks);
                zip.Save(zipStreamSolidWorks);
            }
            return zipStreamSolidWorks;
        }

        private MemoryStream GetSolidEdgeZipStream(Material material, IList<Property> properties)
        {

            bool isSteel = material.MaterialInfo.TypeId == 710 ? true : false;
            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);


            XDocument xml = SolidEdge.FillXMLSolidEdge(material.MaterialInfo.Standard, material.MaterialInfo.Name, isSteel, propertiesMapped);
            MemoryStream memoryStreamSolidEdge = new MemoryStream();
            TextWriter twSolidEdge = new StreamWriter(memoryStreamSolidEdge);
            twSolidEdge.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(xml.ToString())));
            twSolidEdge.Flush();

            MemoryStream zipStreamSolidEdge = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                memoryStreamSolidEdge.Seek(0, SeekOrigin.Begin);
                zip.AddEntry("SolidEdge " + material.MaterialInfo.Name.Replace("/", "") + " " + material.MaterialInfo.Standard.Replace("-", "") + ".xml", memoryStreamSolidEdge);
                zip.Save(zipStreamSolidEdge);
            }
            return zipStreamSolidEdge;
        }

        public MemoryStream GetPTCCreoZipStream(Material material, IList<Property> properties)
        {
            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);

            string pc = PTCCreo.FillPTCCreo(material.MaterialInfo.Standard, material.MaterialInfo.Name, propertiesMapped);
            MemoryStream memoryStreamEsiPamCrash = new MemoryStream();
            TextWriter twEsiPamCrash = new StreamWriter(memoryStreamEsiPamCrash);
            twEsiPamCrash.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(pc)));
            twEsiPamCrash.Flush();

            MemoryStream zipStreamEsiPamCrash = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                memoryStreamEsiPamCrash.Seek(0, SeekOrigin.Begin);
                zip.AddEntry("PTC Creo " + material.MaterialInfo.Name.Replace("/", "") + " " + material.MaterialInfo.Standard.Replace("-", "") + ".mtl", memoryStreamEsiPamCrash);
                zip.Save(zipStreamEsiPamCrash);
            }
            return zipStreamEsiPamCrash;
        }

        public MemoryStream GetKTMXmlZipStream(Material material, IList<Property> properties)
        {

            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);

            XDocument ktmXmlString = KTMXml.FillKTMXml(material.MaterialInfo.Standard, material.MaterialInfo.Name, propertiesMapped);
            MemoryStream memoryStreamktmXml = new MemoryStream();
            TextWriter twktmXml = new StreamWriter(memoryStreamktmXml);
            twktmXml.Write(System.Text.ASCIIEncoding.UTF8.GetString(Encoding.UTF8.GetBytes(ktmXmlString.ToStringWithDeclaration())));
            twktmXml.Flush();
            MemoryStream zipStreamktmXml = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                memoryStreamktmXml.Seek(0, SeekOrigin.Begin);
                zip.AddEntry("ktm XML " + material.MaterialInfo.Name.Replace("/", "") + " " + material.MaterialInfo.Standard.Replace("-", "") + ".xml", memoryStreamktmXml);
                zip.Save(zipStreamktmXml);
            }
            return zipStreamktmXml;
        }

        public MemoryStream GetKTMXlsZipStream(Material material, IList<Property> properties)
        {
            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);

            MemoryStream memoryStreamKTMXls = KTMXLS.FillKTMXLS(propertiesMapped);
            MemoryStream zipStreamKTMXls = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                memoryStreamKTMXls.Seek(0, SeekOrigin.Begin);
                zip.AddEntry("Excel " + material.MaterialInfo.Name.Replace("/", "") + " " + material.MaterialInfo.Standard.Replace("-", "") + ".xls", memoryStreamKTMXls);
                zip.Save(zipStreamKTMXls);
            }
            return zipStreamKTMXls;
        }

        private MemoryStream GetNastranZipStream(Material material, IList<Property> properties)
        {
            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);


            string NASTRANString = NASTRAN.FillNASTRAN(material.MaterialInfo.MaterialId, material.MaterialInfo.Standard, material.MaterialInfo.Name, propertiesMapped);
            MemoryStream memoryStreamNASTRAN = new MemoryStream();
            TextWriter twNASTRAN = new StreamWriter(memoryStreamNASTRAN);
            twNASTRAN.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(NASTRANString)));
            twNASTRAN.Flush();

            MemoryStream zipStreamNASTRAN = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                memoryStreamNASTRAN.Seek(0, SeekOrigin.Begin);
                zip.AddEntry("MSC Nastran " + material.MaterialInfo.Name + " " + material.MaterialInfo.Standard.Replace("-", "") + ".dat", memoryStreamNASTRAN);
                zip.Save(zipStreamNASTRAN);
            }

            return zipStreamNASTRAN;
        }

        private MemoryStream GetAutodeskNastranZipStream(Material material, IList<Property> properties)
        {
            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);
            bool isSteel = material.MaterialInfo.TypeId == 710 ? true : false;
            XDocument xml = AutodeskNastran.FillXML(material.MaterialInfo.MaterialId, material.MaterialInfo.Name, material.MaterialInfo.Standard, isSteel, propertiesMapped);
            MemoryStream memoryStreamAutodeskNastran = new MemoryStream();
            TextWriter twAutodeskNastran = new StreamWriter(memoryStreamAutodeskNastran);
            twAutodeskNastran.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(xml.Declaration.ToString() + Environment.NewLine + xml.ToString())));

            twAutodeskNastran.Flush();

            MemoryStream zipStreamSolidEdge = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                memoryStreamAutodeskNastran.Seek(0, SeekOrigin.Begin);
                zip.AddEntry("AutodeskNastran " + material.MaterialInfo.Name.Replace("/", "") + " " + material.MaterialInfo.Standard.Replace("-", "") + ".sldmat", memoryStreamAutodeskNastran);
                zip.Save(zipStreamSolidEdge);
            }
            return zipStreamSolidEdge;
        }

        public MemoryStream GetFEMAPZipStream(Material material, IList<Property> properties)
        {
            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);

            string FEMAPString = FEMAP.FillFEMAP(material.MaterialInfo.Standard, material.MaterialInfo.Name, propertiesMapped);
            MemoryStream memoryStreamFEMAP = new MemoryStream();
            TextWriter twFEMAP = new StreamWriter(memoryStreamFEMAP);
            twFEMAP.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(FEMAPString)));
            twFEMAP.Flush();

            MemoryStream zipStreamFEMAP = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                memoryStreamFEMAP.Seek(0, SeekOrigin.Begin);
                zip.AddEntry("FEMAP " + material.MaterialInfo.Name.Replace("/", "") + " " + material.MaterialInfo.Standard.Replace("-", "") + ".eps", memoryStreamFEMAP);
                zip.Save(zipStreamFEMAP);
            }

            return zipStreamFEMAP;
        }

        private MemoryStream GetLsDynaZipStream(Material material, IList<Property> properties)
        {


            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);


            string exportLsMAT1 = "";
            string exportLsMAT24 = "";

            exportLsMAT1 = LsDyna.FillMat1(material.MaterialInfo.MaterialId, propertiesMapped);
            MemoryStream memoryStreamLsMAT1 = new MemoryStream();
            TextWriter twLsMAT1 = new StreamWriter(memoryStreamLsMAT1);
            twLsMAT1.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(exportLsMAT1)));
            twLsMAT1.Flush();

            exportLsMAT24 = LsDyna.FillMat24(propertiesMapped);
            MemoryStream memoryStreamLsMAT4 = new MemoryStream();
            TextWriter twLsMAT4 = new StreamWriter(memoryStreamLsMAT4);
            twLsMAT4.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(exportLsMAT24)));
            twLsMAT4.Flush();

            MemoryStream zipStreamLsDyna = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                bool hasPhysical = (from u in propertiesMapped where u.Type == TMPropertyTypeEnum.PhysicalDensity || u.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient || u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity select u).Any();
                if (hasPhysical)
                {
                    memoryStreamLsMAT1.Seek(0, SeekOrigin.Begin);
                    zip.AddEntry("mat_1.key", memoryStreamLsMAT1);
                }

                bool hasStressStrain = (from u in propertiesMapped where u.Type == TMPropertyTypeEnum.PlasticStrainStress select u).Any();
                if (hasStressStrain)
                {
                    memoryStreamLsMAT4.Seek(0, SeekOrigin.Begin);
                    zip.AddEntry("mat_24.key", memoryStreamLsMAT4);
                }
                zip.Save(zipStreamLsDyna);
            }
            return zipStreamLsDyna;
        }

        private MemoryStream GetSiemensZipStream(Material material, IList<Property> properties)
        {

            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);
            bool isSteel = material.MaterialInfo.TypeId == 710 ? true : false;

            //TODO:-Kasnije postaviti tip plus materijala
            PlusGroup materialGroup = PlusGroup.Polymers;

            XDocument siemensString = NXSiemens.FillNXSiemens(material.MaterialInfo.Standard, material.MaterialInfo.Name, isSteel, materialGroup, propertiesMapped);
            MemoryStream memoryStreamsiemens = new MemoryStream();
            TextWriter twsiemens = new StreamWriter(memoryStreamsiemens);
            twsiemens.Write(System.Text.ASCIIEncoding.UTF8.GetString(Encoding.UTF8.GetBytes(siemensString.ToStringWithDeclaration())));
            twsiemens.Flush();
            MemoryStream zipStreamsiemens = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                memoryStreamsiemens.Seek(0, SeekOrigin.Begin);
                zip.AddEntry("Siemens NX" + material.MaterialInfo.Name.Replace("/", "") + " " + material.MaterialInfo.Standard.Replace("-", "") + ".xml", memoryStreamsiemens);
                zip.Save(zipStreamsiemens);
            }
            return zipStreamsiemens;
        }

        public MemoryStream GetANSYSZipStream(Material material, IList<Property> properties)
        {
            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);

            XDocument ansysString = ANSYS.FillANSYS(material.MaterialInfo.MaterialId, material.MaterialInfo.Name, propertiesMapped);
            MemoryStream memoryStreamANSYS = new MemoryStream();
            TextWriter twANSYS = new StreamWriter(memoryStreamANSYS);
            twANSYS.Write(System.Text.ASCIIEncoding.UTF8.GetString(Encoding.UTF8.GetBytes(ansysString.ToStringWithDeclaration())));
            twANSYS.Flush();
            MemoryStream zipStreamANSYS = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                memoryStreamANSYS.Seek(0, SeekOrigin.Begin);
                zip.AddEntry("ANSYS " + material.MaterialInfo.Name.Replace("/", "") + " " + material.MaterialInfo.Standard.Replace("-", "") + ".xml", memoryStreamANSYS);
                zip.Save(zipStreamANSYS);
            }
            return zipStreamANSYS;
        }

        public MemoryStream GetEsiPamCrashZipStream(Material material, IList<Property> properties)
        {

            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);
            string pc = ESIPAMCrash.FillESIPamCrash(material.MaterialInfo.MaterialId, material.MaterialInfo.Standard, material.MaterialInfo.Name, propertiesMapped);
            MemoryStream memoryStreamEsiPamCrash = new MemoryStream();
            TextWriter twEsiPamCrash = new StreamWriter(memoryStreamEsiPamCrash);
            twEsiPamCrash.Write(System.Text.ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(pc)));
            twEsiPamCrash.Flush();

            MemoryStream zipStreamEsiPamCrash = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                memoryStreamEsiPamCrash.Seek(0, SeekOrigin.Begin);
                zip.AddEntry("ESI Pam-Crash " + material.MaterialInfo.Name.Replace("/", "") + " " + material.MaterialInfo.Standard.Replace("-", "") + ".pc", memoryStreamEsiPamCrash);
                zip.Save(zipStreamEsiPamCrash);
            }
            return zipStreamEsiPamCrash;
        }

        public MemoryStream GetESIZipStream(Material material, IList<Property> properties)
        {
            IList<ExportPropertyGeneral> propertiesMapped = MapProperites(properties, material.MaterialInfo.SourceId);

            XDocument xml1 = ESI.FillXmlFileNew(material.MaterialInfo.Name, propertiesMapped);
            MemoryStream memoryStreamEsi = new MemoryStream();
            TextWriter twEsi = new StreamWriter(memoryStreamEsi);
            twEsi.Write(System.Text.ASCIIEncoding.UTF8.GetString(Encoding.UTF8.GetBytes(xml1.ToStringWithDeclaration())));
            twEsi.Flush();

            MemoryStream zipStreamEsi = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                memoryStreamEsi.Seek(0, SeekOrigin.Begin);
                zip.AddEntry("Esi ProCAST " + material.MaterialInfo.Name.Replace("/", "") + " " + material.MaterialInfo.Standard.Replace("-", "") + ".xml", memoryStreamEsi);

                if (propertiesMapped.Where(p => p.Type == TMPropertyTypeEnum.StressStrain && p.SelectedForExports[ExportTypeEnum.Esi] == true).Any())
                {
                    ESI.AddEntryForSS1(zip, propertiesMapped);
                }
                zip.Save(zipStreamEsi);
            }
            return zipStreamEsi;
        }

        private IList<ExportPropertyGeneral> MapProperites(IList<ElsevierMaterials.Models.Domain.Export.Property> propertiesEls, int sourceId)
        {
            IList<ExportPropertyGeneral> properties = new List<ExportPropertyGeneral>();

            foreach (var property in propertiesEls)
            {
                ExportPropertyGeneral prop = new ExportPropertyGeneral();

                //TODO: Vise ti Basic Info ne treba jer sve imas u els info
                prop.Value = property.Value;
                prop.Temperature = property.Temperature;
                prop.Name = property.ElsBasicInfo.Name;
                prop.Unit = property.ElsBasicInfo.Unit;

                int notMappedPropertyId = property.ElsBasicInfo.SourceTypeId != 0 ? property.ElsBasicInfo.SourceTypeId : property.ElsBasicInfo.TypeId;
                TMPropertyTypeEnum propertyId = MapElsPropertyId(notMappedPropertyId, (SourceTypeEnum)sourceId); 
        
                prop.Type = propertyId;

                properties.Add(prop);

            }
            return properties;
        }





        public TMPropertyTypeEnum MapElsPropertyId(int propertyType, SourceTypeEnum source)
        {
            if (source == SourceTypeEnum.Els)
            {
                switch (propertyType)
                {
                    case 6:
                        return TMPropertyTypeEnum.PhysicalThermalConductivity;

                    case 10:
                        return TMPropertyTypeEnum.PhysicalSpecificThermalCapacity;

                    case 2:
                        return TMPropertyTypeEnum.PhysicalDensity;

                    case 238:
                        //tensile modulus
                        return TMPropertyTypeEnum.PhysicalModulusOfElasticity;

                    case 243:
                        //ultimate tensile strength
                        return TMPropertyTypeEnum.MechanicalTensile;

                    case 241:
                        //tensile yield strength
                        return TMPropertyTypeEnum.MechanicalYield;

                    case 239:
                        return TMPropertyTypeEnum.MechanicalElongation;

                    case 4:
                        return TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion;

                    case 11:
                        return TMPropertyTypeEnum.PhysicalPoissonCoefficient;

                    default:
                        return TMPropertyTypeEnum.None;
                }
            }
            else if (source == SourceTypeEnum.TMMetals)
            {
                switch (propertyType)
                {
                    case 1:
                        return TMPropertyTypeEnum.PhysicalThermalConductivity;

                    case 2:
                        return TMPropertyTypeEnum.PhysicalSpecificThermalCapacity;

                    case 4:
                        return TMPropertyTypeEnum.PhysicalDensity;

                    case 5:
                        //tensile modulus
                        return TMPropertyTypeEnum.PhysicalModulusOfElasticity;

                    case 6:
                        //ultimate tensile strength
                        return TMPropertyTypeEnum.MechanicalTensile;

                    case 7:
                        //tensile yield strength
                        return TMPropertyTypeEnum.MechanicalYield;

                    case 8:
                        return TMPropertyTypeEnum.MechanicalElongation;

                    case 10:
                        return TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion;

                    case 11:
                        return TMPropertyTypeEnum.PhysicalPoissonCoefficient;

                    default:
                        return TMPropertyTypeEnum.None;
                }
            }
            else if (source == SourceTypeEnum.TMPlus)
            {
                {
                    switch (propertyType)
                    {

                        //case 1:
                        //    return TMPropertyTypeEnum.PhysicalThermalConductivity;

                        case 2:
                            return TMPropertyTypeEnum.MechanicalTensile;

                        case 253:
                            return TMPropertyTypeEnum.MechanicalYield;

                        case 8:
                            //tensile modulus
                            return TMPropertyTypeEnum.PhysicalThermalConductivity;

                        case 7:
                            //ultimate tensile strength
                            return TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion;

                        case 11:
                            //tensile yield strength
                            return TMPropertyTypeEnum.PhysicalDensity;

                        case 10:
                            return TMPropertyTypeEnum.PhysicalPoissonCoefficient;

                        case 4:
                            return TMPropertyTypeEnum.PhysicalModulusOfElasticity;

                        //case 11:
                        //    return TMPropertyTypeEnum.PhysicalPoissonCoefficient;

                        default:
                            return TMPropertyTypeEnum.None;
                    }
                }


            }

            return TMPropertyTypeEnum.None;
        }

    }
}