using System;
using System.Collections.Generic;

namespace Hackathon_Service.Models.Epic
{
    public class Link
    {
        public string relation { get; set; }
        public string url { get; set; }
    }

    public class Search
    {
        public string mode { get; set; }
    }

    public class Identifier
    {
        public string use { get; set; }
        public string system { get; set; }
        public string value { get; set; }
    }

    public class Patient
    {
        public string display { get; set; }
        public string reference { get; set; }
    }

    public class Prescriber
    {
        public string display { get; set; }
        public string reference { get; set; }
    }

    public class MedicationReference
    {
        public string display { get; set; }
        public string reference { get; set; }
    }

    public class Coding
    {
        public string system { get; set; }
        public string code { get; set; }
        public string display { get; set; }
    }

    public class Route
    {
        public string text { get; set; }
        public List<Coding> coding { get; set; }
    }

    public class Method
    {
        public string text { get; set; }
        public List<Coding> coding { get; set; }
    }

    public class BoundsPeriod
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }

    public class Repeat
    {
        public decimal frequency { get; set; }
        public decimal period { get; set; }
        public string periodUnits { get; set; }
        public BoundsPeriod boundsPeriod { get; set; }
    }

    public class Timing
    {
        public Repeat repeat { get; set; }
    }

    public class DoseQuantity
    {
        public decimal value { get; set; }
        public string unit { get; set; }
        public string code { get; set; }
        public string system { get; set; }
    }

    public class DosageInstruction
    {
        public string text { get; set; }
        public bool asNeededBoolean { get; set; }
        public Route route { get; set; }
        public Method method { get; set; }
        public Timing timing { get; set; }
        public DoseQuantity doseQuantity { get; set; }
    }

    public class ValidityPeriod
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }

    public class ExpectedSupplyDuration
    {
        public decimal value { get; set; }
        public string unit { get; set; }
        public string code { get; set; }
        public string system { get; set; }
    }

    public class Quantity
    {
        public decimal value { get; set; }
        public string unit { get; set; }
    }

    public class DispenseRequest
    {
        public ValidityPeriod validityPeriod { get; set; }
        public decimal? numberOfRepeatsAllowed { get; set; }
        public ExpectedSupplyDuration expectedSupplyDuration { get; set; }
        public Quantity quantity { get; set; }
    }

    public class Resource
    {
        public string resourceType { get; set; }
        public string dateWritten { get; set; }
        public string status { get; set; }
        public string id { get; set; }
        public List<Identifier> identifier { get; set; }
        public Patient patient { get; set; }
        public Prescriber prescriber { get; set; }
        public MedicationReference medicationReference { get; set; }
        public List<DosageInstruction> dosageInstruction { get; set; }
        public DispenseRequest dispenseRequest { get; set; }
    }

    public class Entry
    {
        public string fullUrl { get; set; }
        public Search search { get; set; }
        public List<Link> link { get; set; }
        public Resource resource { get; set; }
    }

    public class MedicationOrder
    {
        public string resourceType { get; set; }
        public string type { get; set; }
        public decimal total { get; set; }
        public List<Link> link { get; set; }
        public List<Entry> entry { get; set; }
    }
}