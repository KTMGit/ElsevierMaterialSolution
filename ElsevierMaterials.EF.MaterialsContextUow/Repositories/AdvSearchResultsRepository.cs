using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.EF.Common.Models;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.AdvancedSearch;
using ElsevierMaterials.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace ElsevierMaterials.EF.MaterialsContextUow.Repositories
{
    public class AdvSearchResultsRepository : BaseRepository<AdvSearchResults>, IAdvSearchResultsRepository
    {
        public AdvSearchResultsRepository(DbContext dbContext)
            : base(dbContext)
        {
        }


        public IEnumerable<int> GetMaterialsByAdvancedSearch(bool withTracking, AdvSearchFiltersAll filters, string sessionId, IQueryable<EquivalentProperty> propIds, IQueryable<EquivalentMaterial> matIds)
        {
            //string fiedList = "[MaterialID], [PropertyID], [Value], [ValueMin], [ValueMax]";
            string fieldList = "[MaterialID], 0 AS [PropertyID], CAST(1.0 as FLOAT) AS [Value], CAST(2.0 AS FLOAT) AS [ValueMin], CAST(3.0 AS FLOAT) AS [ValueMax]";
            string fieldListMat = "[ID] AS [MaterialID], 0 AS [PropertyID], CAST(1.0 as FLOAT) AS [Value], CAST(2.0 AS FLOAT) AS [ValueMin], CAST(3.0 AS FLOAT) AS [ValueMax]";
           

            //StringBuilder sb = new StringBuilder();
            //sb.Append(";");
            int optCounter = 0;

            int argCounter = -1;
            IList<object> allArgs = new List<object>();

            IList<AdvSearchSqlQueryDef> AdvSearchSqlQueryDefs = new List<AdvSearchSqlQueryDef>();
            string queryToAppend = "";

            foreach (AdvSearchFilters f in filters.AllFilters)
            {
                // To return the data in Set A that doesn’t overlap with B, use A EXCEPT B.
                // To return only the data that overlaps in the two sets, use A INTERSECT B.
                // To return the data in Set B that doesn’t overlap with A, use B EXCEPT A.
                // To return the data in all three areas without duplicates, use A UNION B.
                // To return the data in all three areas, including duplicates, use A UNION ALL B.
                // To return the data in the non-overlapping areas of both sets, use (A UNION B) except (A INTERSECT B), or perhaps (A EXCEPT B) UNION (B EXCEPT A)
                //
                // If EXCEPT or INTERSECT is used together with other operators in an expression, it is evaluated in the context of the following precedence:
                // 1. Expressions in parentheses
                // 2. The INTERSECT operator
                // 3. EXCEPT and UNION evaluated from left to right based on their position in the expression
                //
                // Limitation: Max number of parameters for T-SQL command is 2100!

                AdvSearchSqlQueryDef oneAdvSearchSqlQueryDef = new AdvSearchSqlQueryDef() { Filter = f };
                optCounter++;

                queryToAppend = "";
                IList<object> oneArgs = new List<object>();

                switch (f.propertyType)
                {
                    case PropertyType.NotDefined:
                        break;

                    case PropertyType.Material:
                        oneAdvSearchSqlQueryDef.Operator = f.binaryOperators;
                        queryToAppend = "";

                        // reset first binary operator to AND
                        f.binaryOperators = (optCounter == 1 ? BinaryOperators.And : f.binaryOperators);

                        switch (f.binaryOperators)
                        {
                            case BinaryOperators.NotDefined:
                                break;
                            case BinaryOperators.And:
                                //sb.Append(optCounter > 1 ? " INTERSECT " : "");
                                queryToAppend = PrepareMaterial(f, fieldListMat, ref argCounter, ref allArgs, ref oneArgs);
                                break;
                            case BinaryOperators.Or:
                                //sb.Append(optCounter > 1 ? " UNION " : "");
                                queryToAppend = PrepareMaterial(f, fieldListMat, ref argCounter, ref allArgs, ref oneArgs);
                                break;
                            case BinaryOperators.Not:
                                //sb.Append(optCounter > 1 ? " EXCEPT " : "");
                                queryToAppend = PrepareMaterial(f, fieldListMat, ref argCounter, ref allArgs, ref oneArgs);
                                break;
                            default:
                                break;
                        }
                        //sb.Append(queryToAppend);
                        oneAdvSearchSqlQueryDef.Query = queryToAppend;
                        break;

                    case PropertyType.Property:
                        oneAdvSearchSqlQueryDef.Operator = f.binaryOperators;
                        queryToAppend = "";

                        // reset first binary operator to AND
                        f.binaryOperators = (optCounter == 1 ? BinaryOperators.And : f.binaryOperators);

                        switch (f.binaryOperators)
                        {
                            case BinaryOperators.NotDefined:
                                break;
                            case BinaryOperators.And:
                                //sb.Append(optCounter > 1 ? " INTERSECT " : "");
                                queryToAppend = prepareLogicalOperator(f, fieldList, ref argCounter, ref allArgs, ref oneArgs);
                                break;
                            case BinaryOperators.Or:
                                //sb.Append(optCounter > 1 ? " UNION " : "");
                                queryToAppend = prepareLogicalOperator(f, fieldList, ref argCounter, ref allArgs, ref oneArgs);
                                break;
                            case BinaryOperators.Not:
                                //sb.Append(optCounter > 1 ? " EXCEPT " : "");
                                queryToAppend = prepareLogicalOperator(f, fieldList, ref argCounter, ref allArgs, ref oneArgs);
                                break;
                            default:
                                break;
                        }
                        //sb.Append(queryToAppend);
                        oneAdvSearchSqlQueryDef.Query = queryToAppend;
                        break;

                    default:
                        break;
                }

                oneAdvSearchSqlQueryDef.Args = oneArgs;
                AdvSearchSqlQueryDefs.Add(oneAdvSearchSqlQueryDef);
            }

            // Add source condition
            int sourceId = -1;
            int databookId = -1;

            if (filters.SelectedSource != null && filters.SelectedSource != "0"    /* "0" = All */)
            {
                string[] ids = filters.SelectedSource.Split(',');
                sourceId = ids[0] != null ? int.Parse(ids[0]) : -1;
                databookId = ids[1] != null ? int.Parse(ids[1]) : -1;

                AdvSearchFilters fSource = new AdvSearchFilters();
                AdvSearchSqlQueryDef oneAdvSearchSqlQueryDefSource = new AdvSearchSqlQueryDef();
                IList<object> oneArgsSource = new List<object>();

                if (databookId == 0 && sourceId != 0 && sourceId != -1)
                {
                    fSource = new AdvSearchFilters()
                   {
                       propertyType = PropertyType.Material,
                       binaryOperators = BinaryOperators.And,
                       propertyId = sourceId,
                       propertyName = "SOURCE:"
                   };
                    queryToAppend = PrepareMaterial(fSource, fieldListMat, ref argCounter, ref allArgs, ref oneArgsSource);
                }
                else if (databookId != 0 && databookId != -1 && sourceId != 0 && sourceId != -1)
                {
                    fSource = new AdvSearchFilters()
                   {
                       propertyType = PropertyType.Material,
                       binaryOperators = BinaryOperators.And,
                       propertyId = databookId,
                       propertyName = "DATABOOK:"
                   };
                    queryToAppend = PrepareMaterial(fSource, fieldListMat, ref argCounter, ref allArgs, ref oneArgsSource);
                }

                oneAdvSearchSqlQueryDefSource.Filter = fSource;
                oneAdvSearchSqlQueryDefSource.Query = queryToAppend;
                oneAdvSearchSqlQueryDefSource.Args = oneArgsSource;
                oneAdvSearchSqlQueryDefSource.Operator = fSource.binaryOperators;
                AdvSearchSqlQueryDefs.Add(oneAdvSearchSqlQueryDefSource);
            }


            // (with TM data)
            int nextStartIndex = 0;
            int queryCounter = 0;

            IList<EquivalentMaterial> matIdsM = matIds.Where(m => m.SourceId == 2).ToList();
            IList<EquivalentMaterial> matIdsP = matIds.Where(m => m.SourceId == 3).ToList();
            IList<EquivalentProperty> propIdsM = propIds.Where(p => p.SourceId == 2).ToList();
            IList<EquivalentProperty> propIdsP = propIds.Where(p => p.SourceId == 3).ToList();

            IService service = new TotalMateriaService();
            IPlusService servicePLUS = new TMPlusService();

            IEnumerable<int> dsAll = null;
            foreach (AdvSearchSqlQueryDef qDef in AdvSearchSqlQueryDefs)
            {
                queryCounter++;

                // Prepare arguments for SQL Query (renumerate them)
                for (int i = 0; i < qDef.Args.Count(); i++)
                {
                    qDef.Query = qDef.Query.Replace("{" + (nextStartIndex).ToString() + "}", "{" + i.ToString() + "}");
                    nextStartIndex++;
                }

                /////////
                string cmdText = qDef.Query;
                object[] arg = qDef.Args.ToArray();
                IEnumerable<int> ds = DataSet.SqlQuery(cmdText, arg).Select(m => m.MaterialID);




                // Call Web service for TM property data and make UNION with ds from the EMS dabase
                if ((databookId == 0 || databookId == -1) && filters.IsChemical == false)
                {

                    if (qDef.Filter.propertyType == PropertyType.Property)
                    {
                        // SELECT SourcePropertyId FROM [EquivalentProperty] WHERE [EquivalentProperty].SourceId=2 AND [EquivalentProperty].PropertyId = propertyId
                        int propertyId = qDef.Filter.propertyId;


                        if (sourceId == 2 || sourceId == -1)
                        {
                            //
                            // Step 1. Metals
                            //
                            EquivalentProperty epM = propIdsM.Where(p => p.PropertyId == propertyId).FirstOrDefault();

                            // Call service only if relation is defined in the equivalent table
                            if (epM != null)
                            {
                                //int pIdMetalI = propIdsM.Where(p => p.PropertyId == propertyId).FirstOrDefault().SourcePropertyId;
                                int pIdMetalI = epM.SourcePropertyId;
                                IList<AdvSearchFilters> filtersForService = new List<AdvSearchFilters>();

                                //Make object clone and do c;  See SearchResultsBinder.cs
                                filtersForService.Add(new AdvSearchFilters()
                                {
                                    propertyType = qDef.Filter.propertyType,
                                    logicalOperators = qDef.Filter.logicalOperators,
                                    binaryOperators = qDef.Filter.binaryOperators,
                                    propertyId = pIdMetalI, /* !!! calling byRef, original value is changed !!! */
                                    propertyName = qDef.Filter.propertyName,
                                    valueFrom = qDef.Filter.valueFrom,
                                    valueTo = qDef.Filter.valueTo,
                                    valueFrom_orig = qDef.Filter.valueFrom_orig,
                                    valueTo_orig = qDef.Filter.valueTo_orig,
                                    unitId = qDef.Filter.unitId,
                                    unitName = qDef.Filter.unitName
                                });

                                // Service returns list of SourceMaterialId
                                IList<int> listOfTMMetalIds = service.GetMaterialIdsForAdvSearchPropertiesFromService(sessionId, new AdvSearchFiltersAll() { AllFilters = filtersForService });
                                // Find material IDs in EMS database
                                IList<int> res2M = (from u1 in matIdsM join u2 in listOfTMMetalIds on u1.SourceMaterialId equals (int)u2 select u1.MaterialId).ToList();

                                ds = ds.Concat(res2M).Distinct();

                            }  // End of Call service only if relation is defined in the equivalent table
                            //
                            // End of Step 1. Metals
                        }


                        //if (sourceId == 3 || sourceId == -1)
                        if ((sourceId == 3 || sourceId == -1) && filters.IsChemical == false)
                        {
                            //
                            // Step 2. PLUS
                            //
                            EquivalentProperty epP = propIdsP.Where(p => p.PropertyId == propertyId).FirstOrDefault();

                            // Call service only if relation is defined in the equivalent table
                            if (epP != null)
                            {
                                //int pIdMetalI = propIdsM.Where(p => p.PropertyId == propertyId).FirstOrDefault().SourcePropertyId;
                                int pIdPlusI = epP.SourcePropertyId;
                                IList<AdvSearchFilters> filtersForService = new List<AdvSearchFilters>();

                                //Make object clone and do c;  See SearchResultsBinder.cs
                                filtersForService.Add(new AdvSearchFilters()
                                {
                                    propertyType = qDef.Filter.propertyType,
                                    logicalOperators = qDef.Filter.logicalOperators,
                                    binaryOperators = qDef.Filter.binaryOperators,
                                    propertyId = pIdPlusI, /* !!! calling byRef, original value is changed !!! */
                                    propertyName = qDef.Filter.propertyName,
                                    valueFrom = qDef.Filter.valueFrom,
                                    valueTo = qDef.Filter.valueTo,
                                    valueFrom_orig = qDef.Filter.valueFrom_orig,
                                    valueTo_orig = qDef.Filter.valueTo_orig,
                                    unitId = qDef.Filter.unitId,
                                    unitName = qDef.Filter.unitName
                                });

                                // Service returns list of SourceMaterialId
                                IList<int> listOfTMPLUSIds = servicePLUS.GetMaterialIdsForAdvSearchPropertiesFromServicePLUS(sessionId, new AdvSearchFiltersAll() { AllFilters = filtersForService });
                                // Find material IDs in EMS database
                                IList<int> res2P = (from u1 in matIdsP join u2 in listOfTMPLUSIds on u1.SourceMaterialId equals (int)u2 select u1.MaterialId).ToList();

                                ds = ds.Concat(res2P).Distinct();

                            }  // End of Call service only if relation is defined in the equivalent table

                            //ds = ds.Concat(res2P).Distinct();
                            //
                            // End of Step 2. PLUS
                        }


                    } // End of Call Web service
                }


                if (!withTracking)
                {
                    if (queryCounter == 1)
                    {
                        dsAll = ds;
                    }
                    else
                    {
                        switch (qDef.Operator)
                        {
                            case BinaryOperators.NotDefined:
                                break;
                            case BinaryOperators.And:
                                // INTERSECT
                                dsAll = dsAll.Intersect(ds);
                                break;
                            case BinaryOperators.Or:
                                // UNION
                                dsAll = dsAll.Union(ds);
                                break;
                            case BinaryOperators.Not:
                                // EXCEPT
                                dsAll = dsAll.Except(ds);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return dsAll;

        }



        private string PrepareMaterial(AdvSearchFilters f, string fieldListMat, ref int argCounter, ref IList<object> allArgs, ref IList<object> oneArgs)
        {
            object[] args;
            StringBuilder sb = new StringBuilder();

            //if (f.propertyName.ToUpper().StartsWith("SOURCE:") || f.propertyName.ToUpper().StartsWith("DATABOOK:"))
            //{
            //    sb.Append(string.Concat(" SELECT ", fieldListMat, " FROM [QuickSearchFullText] "));
            //}
            //else
            //{
            //    sb.Append(string.Concat(" SELECT DISTINCT ", fieldListMat, " FROM [MaterialTaxonomy]"));
            //}

            sb.Append(string.Concat(" SELECT DISTINCT ", fieldListMat, " FROM [MaterialTaxonomy]"));


            if (f.propertyName.ToUpper().StartsWith("TYPE:"))
            {
                argCounter++;
                allArgs.Add(f.propertyId);
                oneArgs.Add(f.propertyId);
                args = new object[] { argCounter };
                //sb.Append(String.Format(" WHERE [type_ID] = {{{0}}}", args));
                sb.Append(String.Format(" WHERE [Level1] = {{{0}}}", args));
            }

            if (f.propertyName.ToUpper().StartsWith("CLASS:"))
            {
                argCounter++;
                allArgs.Add(f.propertyId);
                oneArgs.Add(f.propertyId);
                args = new object[] { argCounter };
                //sb.Append(String.Format(" WHERE [group_ID] = {{{0}}}", args));
                sb.Append(String.Format(" WHERE [Level2] = {{{0}}}", args));
            }

            if (f.propertyName.ToUpper().StartsWith("SUBCLASS:"))
            {
                argCounter++;
                allArgs.Add(f.propertyId);
                oneArgs.Add(f.propertyId);
                args = new object[] { argCounter };
                //sb.Append(String.Format(" WHERE [class_ID] = {{{0}}}", args));
                sb.Append(String.Format(" WHERE [Level3] = {{{0}}}", args));
            }

            if (f.propertyName.ToUpper().StartsWith("GROUP:"))
            {
                argCounter++;
                allArgs.Add(f.propertyId);
                oneArgs.Add(f.propertyId);
                args = new object[] { argCounter };
                //sb.Append(String.Format(" WHERE [subClass_ID] = {{{0}}}", args));
                sb.Append(String.Format(" WHERE [Level4] = {{{0}}}", args));
            }


            // Sources; hard coded arguments because of linq doesn't accept "LIKE" clause in parameter oblique
            if (f.propertyName.ToUpper().StartsWith("SOURCE:"))
            {
                //argCounter++;
                //allArgs.Add(f.propertyId);
                //oneArgs.Add(f.propertyId);
                //args = new object[] { argCounter };
                //sb.Append(String.Format(" WHERE [source_IDs] LIKE '%,{{{0}}},%'", args));
                sb.Append(String.Format(" WHERE [source_IDs] LIKE '%,{0},%'", f.propertyId));
            }

            if (f.propertyName.ToUpper().StartsWith("DATABOOK:"))
            {
                //argCounter++;
                //allArgs.Add(f.propertyId);
                //oneArgs.Add(f.propertyId);
                //args = new object[] { argCounter };
                //sb.Append(String.Format(" WHERE [databook_IDs] LIKE '%,{{{0}}},%'", args));
                sb.Append(String.Format(" WHERE [databook_IDs] LIKE '%,{0},%'", f.propertyId));
            }
            // End of Sources


            return string.Concat("(", sb.ToString().Trim(), ")");
        }

        private string prepareLogicalOperator(AdvSearchFilters f, string fieldList, ref int argCounter, ref IList<object> allArgs, ref IList<object> oneArgs)
        {
            // 0 - Temperature
            // 1 - Pressure
            // 2 - Wavelength
            IList<Tuple<decimal?, decimal?>> conditions = new List<Tuple<decimal?, decimal?>>();
            conditions.Add(new Tuple<decimal?, decimal?>(null, null));
            conditions.Add(new Tuple<decimal?, decimal?>(null, null));
            conditions.Add(new Tuple<decimal?, decimal?>(null, null));

            if (f.isPropertyConditionsActive)
            {

                foreach (PropertyConditionModel c in f.PropertyConditions)
                {
                    decimal? valFrom = null;
                    if (c.ValueFrom != "" && c.ValueFrom != null)
                        valFrom = decimal.Parse(c.ValueFrom);
                    decimal? valTo = null;
                    if (c.ValueTo != "" && c.ValueTo != null)
                        valTo = decimal.Parse(c.ValueTo);

                    if (c.Condition.X_label.Trim().ToLower() == "temperature")
                    {
                        conditions[0] = new Tuple<decimal?, decimal?>(valFrom, valTo);
                    }
                    else if (c.Condition.X_label.Trim().ToLower() == "pressure")
                    {
                        conditions[1] = new Tuple<decimal?, decimal?>(valFrom, valTo);
                    }
                    else if (c.Condition.X_label.Trim().ToLower() == "wavelength")
                    {
                        conditions[2] = new Tuple<decimal?, decimal?>(valFrom, valTo);
                    }

                }
            }

            object[] args;
            StringBuilder sb = new StringBuilder();
            switch (f.logicalOperators)
            {
                case LogicalOperators.NotDefined:
                    break;

                case LogicalOperators.Exists:
                    argCounter++;
                    allArgs.Add(f.propertyId);
                    oneArgs.Add(f.propertyId);
                    // add conditions
                    argCounter++;
                    allArgs.Add(conditions[0].Item1);
                    oneArgs.Add(conditions[0].Item1);
                    argCounter++;
                    allArgs.Add(conditions[0].Item2);
                    oneArgs.Add(conditions[0].Item2);
                    argCounter++;
                    allArgs.Add(conditions[1].Item1);
                    oneArgs.Add(conditions[1].Item1);
                    argCounter++;
                    allArgs.Add(conditions[1].Item2);
                    oneArgs.Add(conditions[1].Item2);
                    argCounter++;
                    allArgs.Add(conditions[2].Item1);
                    oneArgs.Add(conditions[2].Item2);
                    argCounter++;
                    allArgs.Add(conditions[2].Item2);
                    oneArgs.Add(conditions[2].Item2);
                    args = new object[] { argCounter - 6, argCounter - 5, argCounter - 4, argCounter - 3, argCounter - 2, argCounter - 1, argCounter };
                    //sb.Append(string.Concat(" SELECT ", fieldList, String.Format(" FROM [fn_EMS_AdvSearch_exists] ({{{0}}}, {{{1}}}, {{{2}}}, {{{3}}}, {{{4}}}, {{{5}}}, {{{6}}})", args)));
                    sb.Append(string.Concat(" SELECT ", fieldList, String.Format(" FROM [fn_EMS_AdvSearch_exists_mp] ({{{0}}}, {{{1}}}, {{{2}}}, {{{3}}}, {{{4}}}, {{{5}}}, {{{6}}})", args)));

                    break;

                case LogicalOperators.Eq:
                    argCounter++;
                    allArgs.Add(f.propertyId);
                    oneArgs.Add(f.propertyId);
                    argCounter++;
                    allArgs.Add(f.valueFrom);
                    oneArgs.Add(f.valueFrom);
                    // add conditions
                    argCounter++;
                    allArgs.Add(conditions[0].Item1);
                    oneArgs.Add(conditions[0].Item1);
                    argCounter++;
                    allArgs.Add(conditions[0].Item2);
                    oneArgs.Add(conditions[0].Item2);
                    argCounter++;
                    allArgs.Add(conditions[1].Item1);
                    oneArgs.Add(conditions[1].Item1);
                    argCounter++;
                    allArgs.Add(conditions[1].Item2);
                    oneArgs.Add(conditions[1].Item2);
                    argCounter++;
                    allArgs.Add(conditions[2].Item1);
                    oneArgs.Add(conditions[2].Item2);
                    argCounter++;
                    allArgs.Add(conditions[2].Item2);
                    oneArgs.Add(conditions[2].Item2);
                    args = new object[] { argCounter - 7, argCounter - 6, argCounter - 5, argCounter - 4, argCounter - 3, argCounter - 2, argCounter - 1, argCounter };
                    //sb.Append(string.Concat(" SELECT ", fieldList, String.Format(" FROM [fn_EMS_AdvSearch_equal] ({{{0}}}, {{{1}}})", args)));
                    sb.Append(string.Concat(" SELECT ", fieldList, String.Format(" FROM [fn_EMS_AdvSearch_equal_mp] ({{{0}}}, {{{1}}}, {{{2}}}, {{{3}}}, {{{4}}}, {{{5}}}, {{{6}}}, {{{7}}})", args)));
                    break;

                case LogicalOperators.Lte:
                    argCounter++;
                    allArgs.Add(f.propertyId);
                    oneArgs.Add(f.propertyId);
                    argCounter++;
                    allArgs.Add(f.valueFrom);
                    oneArgs.Add(f.valueFrom);
                    // add conditions
                    argCounter++;
                    allArgs.Add(conditions[0].Item1);
                    oneArgs.Add(conditions[0].Item1);
                    argCounter++;
                    allArgs.Add(conditions[0].Item2);
                    oneArgs.Add(conditions[0].Item2);
                    argCounter++;
                    allArgs.Add(conditions[1].Item1);
                    oneArgs.Add(conditions[1].Item1);
                    argCounter++;
                    allArgs.Add(conditions[1].Item2);
                    oneArgs.Add(conditions[1].Item2);
                    argCounter++;
                    allArgs.Add(conditions[2].Item1);
                    oneArgs.Add(conditions[2].Item2);
                    argCounter++;
                    allArgs.Add(conditions[2].Item2);
                    oneArgs.Add(conditions[2].Item2);
                    args = new object[] { argCounter - 7, argCounter - 6, argCounter - 5, argCounter - 4, argCounter - 3, argCounter - 2, argCounter - 1, argCounter };
                    //sb.Append(string.Concat(" SELECT ", fieldList, String.Format(" FROM [fn_EMS_AdvSearch_lessThanOrEqualTo] ({{{0}}}, {{{1}}})", args)));
                    sb.Append(string.Concat(" SELECT ", fieldList, String.Format(" FROM [fn_EMS_AdvSearch_lessThanOrEqualTo_mp] ({{{0}}}, {{{1}}}, {{{2}}}, {{{3}}}, {{{4}}}, {{{5}}}, {{{6}}}, {{{7}}})", args)));
                    break;

                case LogicalOperators.Gte:
                    argCounter++;
                    allArgs.Add(f.propertyId);
                    oneArgs.Add(f.propertyId);
                    argCounter++;
                    allArgs.Add(f.valueFrom);
                    oneArgs.Add(f.valueFrom);
                    // add conditions
                    argCounter++;
                    allArgs.Add(conditions[0].Item1);
                    oneArgs.Add(conditions[0].Item1);
                    argCounter++;
                    allArgs.Add(conditions[0].Item2);
                    oneArgs.Add(conditions[0].Item2);
                    argCounter++;
                    allArgs.Add(conditions[1].Item1);
                    oneArgs.Add(conditions[1].Item1);
                    argCounter++;
                    allArgs.Add(conditions[1].Item2);
                    oneArgs.Add(conditions[1].Item2);
                    argCounter++;
                    allArgs.Add(conditions[2].Item1);
                    oneArgs.Add(conditions[2].Item2);
                    argCounter++;
                    allArgs.Add(conditions[2].Item2);
                    oneArgs.Add(conditions[2].Item2);
                    args = new object[] { argCounter - 7, argCounter - 6, argCounter - 5, argCounter - 4, argCounter - 3, argCounter - 2, argCounter - 1, argCounter };
                    //sb.Append(string.Concat(" SELECT ", fieldList, String.Format(" FROM [fn_EMS_AdvSearch_greaterThanOrEqualTo] ({{{0}}}, {{{1}}})", args)));
                    sb.Append(string.Concat(" SELECT ", fieldList, String.Format(" FROM [fn_EMS_AdvSearch_greaterThanOrEqualTo_mp] ({{{0}}}, {{{1}}}, {{{2}}}, {{{3}}}, {{{4}}}, {{{5}}}, {{{6}}}, {{{7}}})", args)));
                    break;

                case LogicalOperators.Between:
                    argCounter++;
                    allArgs.Add(f.propertyId);
                    oneArgs.Add(f.propertyId);
                    argCounter++;
                    allArgs.Add(f.valueFrom);
                    oneArgs.Add(f.valueFrom);
                    argCounter++;
                    allArgs.Add(f.valueTo);
                    oneArgs.Add(f.valueTo);
                    // add conditions
                    argCounter++;
                    allArgs.Add(conditions[0].Item1);
                    oneArgs.Add(conditions[0].Item1);
                    argCounter++;
                    allArgs.Add(conditions[0].Item2);
                    oneArgs.Add(conditions[0].Item2);
                    argCounter++;
                    allArgs.Add(conditions[1].Item1);
                    oneArgs.Add(conditions[1].Item1);
                    argCounter++;
                    allArgs.Add(conditions[1].Item2);
                    oneArgs.Add(conditions[1].Item2);
                    argCounter++;
                    allArgs.Add(conditions[2].Item1);
                    oneArgs.Add(conditions[2].Item2);
                    argCounter++;
                    allArgs.Add(conditions[2].Item2);
                    oneArgs.Add(conditions[2].Item2);

                    args = new object[] { argCounter - 8, argCounter - 7, argCounter - 6, argCounter - 5, argCounter - 4, argCounter - 3, argCounter - 2, argCounter - 1, argCounter };
                    //sb.Append(string.Concat(" SELECT ", fieldList, String.Format(" FROM [fn_EMS_AdvSearch_isBetween] ({{{0}}}, {{{1}}}, {{{2}}})", args)));
                    sb.Append(string.Concat(" SELECT ", fieldList, String.Format(" FROM [fn_EMS_AdvSearch_isBetween_mp] ({{{0}}}, {{{1}}}, {{{2}}}, {{{3}}}, {{{4}}}, {{{5}}}, {{{6}}}, {{{7}}}, {{{8}}})", args)));
                    break;

                default:
                    break;
            }

            return string.Concat("(", sb.ToString().Trim(), ")");
        }

        private string PrepareMaterialFromStructure(string recordIds)
        {

            StringBuilder sb = new StringBuilder();
            string fieldListMat = "[ID] AS [MaterialID], 0 AS [PropertyID], CAST(1.0 as FLOAT) AS [Value], CAST(2.0 AS FLOAT) AS [ValueMin], CAST(3.0 AS FLOAT) AS [ValueMax]";
            sb.Append(" SELECT " + fieldListMat + "FROM [View_EMS_QuickSearchFullText_ByRecordId] where record_id in (");
            sb.Append(recordIds);
            sb.Append(")");
            return sb.ToString();
        }

        public IEnumerable<int> MaterialStructureSearch(string recordIds)
        {
            string cmdText = PrepareMaterialFromStructure(recordIds);
            IEnumerable<int> ds = DataSet.SqlQuery(cmdText).Select(m => m.MaterialID);
            return ds;
        }

    }
}
