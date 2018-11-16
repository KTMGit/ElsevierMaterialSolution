using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Models;
using Api.Models; 

namespace ElsevierMaterialsMVC.Models.MaterialDetails
{
    public class CreepDataModel
    {
        public IList<ElsevierMaterials.Models.MaterialCondition> MaterialConditions { get; set; }
        public IList<TestCondition> Conditions { get; set; }
        public ElsevierMaterials.Models.Domain.CreepData.CreepDataContainer Data { get; set; }
        public IList<string> AllReferences { get; set; }
        
        public CreepDataModel()
        {
            Conditions = new List<TestCondition>();
            MaterialConditions = new List<ElsevierMaterials.Models.MaterialCondition>();
        }
    }
}