using System.ComponentModel;

namespace DataLayer.Models.Enums
{
    public enum CompanyType
    {
        [Description("Architecture")]
        Architecture,

        [Description("Communications")]
        Communications,

        [Description("Communications Data")]
        Communications_Data,

        [Description("Concrete")]
        Concrete,

        [Description("Concrete Cast In Place")]
        Concrete_CastInPlace,

        [Description("Concrete Precast")]
        Concrete_Precast,

        [Description("Construction Management")]
        ConstructionManagement,

        [Description("Conveying Equipment")]
        ConveyingEquipment,

        [Description("Conveying Equipment Elevators")]
        ConveyingEquipment_Elevators,

        [Description("Demolition")]
        Demolition,

        [Description("Earthwork")]
        Earthwork,

        [Description("Earthwork Site Excavation Grading")]
        Earthwork_SiteExcavationGrading,

        [Description("Electrical")]
        Electrical,

        [Description("Electrical Power Generation")]
        ElectricalPowerGeneration,

        [Description("Electronic Safety Security")]
        ElectronicSafetySecurity,

        [Description("Equipment")]
        Equipment,

        [Description("Equipment Kitchen Appliances")]
        Equipment_KitchenAppliances,

        [Description("Exterior Improvements")]
        ExteriorImprovements,

        [Description("Exterior Fences Gates")]
        Exterior_FencesGates
    }
}