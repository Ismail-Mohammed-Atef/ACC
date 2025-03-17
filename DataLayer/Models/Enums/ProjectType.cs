using System.ComponentModel;

namespace DataLayer.Models.Enums
{
    public enum ProjectType
    {
        [Description("Institutional")]
        Institutional,

        [Description("Court House")]
        CourtHouse,

        [Description("Military Facility")]
        MilitaryFacility,

        [Description("Prison Correctional Facility")]
        PrisonCorrectionalFacility,

        [Description("Museum")]
        Museum,

        [Description("Religious Building")]
        ReligiousBuilding,

        [Description("Recreation Building")]
        RecreationBuilding,

        [Description("Education Facility")]
        EducationFacility,

        [Description("Research Facility Laboratory")]
        ResearchFacilityLaboratory,

        [Description("Library")]
        Library,

        [Description("Government Building")]
        GovernmentBuilding,

        [Description("Dormitory")]
        Dormitory,

        [Description("Commercial")]
        Commercial,

        [Description("Theme Park")]
        ThemePark,

        [Description("Parking Structure Garage")]
        ParkingStructureGarage,

        [Description("Data Center")]
        DataCenter,

        [Description("Convention Center")]
        ConventionCenter,

        [Description("Hotel Motel")]
        HotelMotel,

        [Description("Retail")]
        Retail,

        [Description("Performing Arts")]
        PerformingArts,

        [Description("Industrial Energy")]
        IndustrialEnergy,

        [Description("Oil Gas")]
        OilGas,

        [Description("Solar Farm")]
        SolarFarm
    }
}