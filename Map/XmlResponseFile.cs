
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/search/local/ws/rest/v1")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://schemas.microsoft.com/search/local/ws/rest/v1", IsNullable=false)]
public partial class Response
{

    private string copyrightField;

    private string brandLogoUriField;

    private byte statusCodeField;

    private string statusDescriptionField;

    private string authenticationResultCodeField;

    private string traceIdField;

    private ResponseResourceSets resourceSetsField;

    /// <remarks/>
    public string Copyright
    {
        get
        {
            return this.copyrightField;
        }
        set
        {
            this.copyrightField = value;
        }
    }

    /// <remarks/>
    public string BrandLogoUri
    {
        get
        {
            return this.brandLogoUriField;
        }
        set
        {
            this.brandLogoUriField = value;
        }
    }

    /// <remarks/>
    public byte StatusCode
    {
        get
        {
            return this.statusCodeField;
        }
        set
        {
            this.statusCodeField = value;
        }
    }

    /// <remarks/>
    public string StatusDescription
    {
        get
        {
            return this.statusDescriptionField;
        }
        set
        {
            this.statusDescriptionField = value;
        }
    }

    /// <remarks/>
    public string AuthenticationResultCode
    {
        get
        {
            return this.authenticationResultCodeField;
        }
        set
        {
            this.authenticationResultCodeField = value;
        }
    }

    /// <remarks/>
    public string TraceId
    {
        get
        {
            return this.traceIdField;
        }
        set
        {
            this.traceIdField = value;
        }
    }

    /// <remarks/>
    public ResponseResourceSets ResourceSets
    {
        get
        {
            return this.resourceSetsField;
        }
        set
        {
            this.resourceSetsField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
public partial class ResponseResourceSets
{

    private ResponseResourceSetsResourceSet resourceSetField;

    /// <remarks/>
    public ResponseResourceSetsResourceSet ResourceSet
    {
        get
        {
            return this.resourceSetField;
        }
        set
        {
            this.resourceSetField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
public partial class ResponseResourceSetsResourceSet
{

    private byte estimatedTotalField;

    private ResponseResourceSetsResourceSetLocation[] resourcesField;

    /// <remarks/>
    public byte EstimatedTotal
    {
        get
        {
            return this.estimatedTotalField;
        }
        set
        {
            this.estimatedTotalField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Location", IsNullable = false)]
    public ResponseResourceSetsResourceSetLocation[] Resources
    {
        get
        {
            return this.resourcesField;
        }
        set
        {
            this.resourcesField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
public partial class ResponseResourceSetsResourceSetLocation
{

    private string nameField;

    private ResponseResourceSetsResourceSetLocationPoint pointField;

    private ResponseResourceSetsResourceSetLocationBoundingBox boundingBoxField;

    private string entityTypeField;

    private ResponseResourceSetsResourceSetLocationAddress addressField;

    private string confidenceField;

    private string matchCodeField;

    private ResponseResourceSetsResourceSetLocationGeocodePoint geocodePointField;

    /// <remarks/>
    public string Name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    public ResponseResourceSetsResourceSetLocationPoint Point
    {
        get
        {
            return this.pointField;
        }
        set
        {
            this.pointField = value;
        }
    }

    /// <remarks/>
    public ResponseResourceSetsResourceSetLocationBoundingBox BoundingBox
    {
        get
        {
            return this.boundingBoxField;
        }
        set
        {
            this.boundingBoxField = value;
        }
    }

    /// <remarks/>
    public string EntityType
    {
        get
        {
            return this.entityTypeField;
        }
        set
        {
            this.entityTypeField = value;
        }
    }

    /// <remarks/>
    public ResponseResourceSetsResourceSetLocationAddress Address
    {
        get
        {
            return this.addressField;
        }
        set
        {
            this.addressField = value;
        }
    }

    /// <remarks/>
    public string Confidence
    {
        get
        {
            return this.confidenceField;
        }
        set
        {
            this.confidenceField = value;
        }
    }

    /// <remarks/>
    public string MatchCode
    {
        get
        {
            return this.matchCodeField;
        }
        set
        {
            this.matchCodeField = value;
        }
    }

    /// <remarks/>
    public ResponseResourceSetsResourceSetLocationGeocodePoint GeocodePoint
    {
        get
        {
            return this.geocodePointField;
        }
        set
        {
            this.geocodePointField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
public partial class ResponseResourceSetsResourceSetLocationPoint
{

    private decimal latitudeField;

    private decimal longitudeField;

    /// <remarks/>
    public decimal Latitude
    {
        get
        {
            return this.latitudeField;
        }
        set
        {
            this.latitudeField = value;
        }
    }

    /// <remarks/>
    public decimal Longitude
    {
        get
        {
            return this.longitudeField;
        }
        set
        {
            this.longitudeField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
public partial class ResponseResourceSetsResourceSetLocationBoundingBox
{

    private decimal southLatitudeField;

    private decimal westLongitudeField;

    private decimal northLatitudeField;

    private decimal eastLongitudeField;

    /// <remarks/>
    public decimal SouthLatitude
    {
        get
        {
            return this.southLatitudeField;
        }
        set
        {
            this.southLatitudeField = value;
        }
    }

    /// <remarks/>
    public decimal WestLongitude
    {
        get
        {
            return this.westLongitudeField;
        }
        set
        {
            this.westLongitudeField = value;
        }
    }

    /// <remarks/>
    public decimal NorthLatitude
    {
        get
        {
            return this.northLatitudeField;
        }
        set
        {
            this.northLatitudeField = value;
        }
    }

    /// <remarks/>
    public decimal EastLongitude
    {
        get
        {
            return this.eastLongitudeField;
        }
        set
        {
            this.eastLongitudeField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
public partial class ResponseResourceSetsResourceSetLocationAddress
{

    private string adminDistrictField;

    private string countryRegionField;

    private string formattedAddressField;

    private string localityField;

    /// <remarks/>
    public string AdminDistrict
    {
        get
        {
            return this.adminDistrictField;
        }
        set
        {
            this.adminDistrictField = value;
        }
    }

    /// <remarks/>
    public string CountryRegion
    {
        get
        {
            return this.countryRegionField;
        }
        set
        {
            this.countryRegionField = value;
        }
    }

    /// <remarks/>
    public string FormattedAddress
    {
        get
        {
            return this.formattedAddressField;
        }
        set
        {
            this.formattedAddressField = value;
        }
    }

    /// <remarks/>
    public string Locality
    {
        get
        {
            return this.localityField;
        }
        set
        {
            this.localityField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
public partial class ResponseResourceSetsResourceSetLocationGeocodePoint
{

    private decimal latitudeField;

    private decimal longitudeField;

    private string calculationMethodField;

    private string usageTypeField;

    /// <remarks/>
    public decimal Latitude
    {
        get
        {
            return this.latitudeField;
        }
        set
        {
            this.latitudeField = value;
        }
    }

    /// <remarks/>
    public decimal Longitude
    {
        get
        {
            return this.longitudeField;
        }
        set
        {
            this.longitudeField = value;
        }
    }

    /// <remarks/>
    public string CalculationMethod
    {
        get
        {
            return this.calculationMethodField;
        }
        set
        {
            this.calculationMethodField = value;
        }
    }

    /// <remarks/>
    public string UsageType
    {
        get
        {
            return this.usageTypeField;
        }
        set
        {
            this.usageTypeField = value;
        }
    }
}

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]







public partial class ResponseResourceSetsResourceSetResourcesResource
{

    private string isPrivateResidenceField;

    private ResponseResourceSetsResourceSetResourcesResourceBusinessLocationEntity[] businessesAtLocationField;

    private ResponseResourceSetsResourceSetResourcesResourceAddressOfLocation addressOfLocationField;

    private object vendorIdsField;

    /// <remarks/>
    

    
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
public partial class ResponseResourceSetsResourceSetResourcesResourceBusinessLocationEntity
{

    private ResponseResourceSetsResourceSetResourcesResourceBusinessLocationEntityBusinessAddress businessAddressField;

    private ResponseResourceSetsResourceSetResourcesResourceBusinessLocationEntityBusinessInfo businessInfoField;

    
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
public partial class ResponseResourceSetsResourceSetResourcesResourceBusinessLocationEntityBusinessAddress
{

    private decimal latitudeField;

    private decimal longitudeField;

    private string addressLineField;

    private string localityField;

    private string adminDivisionField;

    private string countryIso2Field;

    private uint postalCodeField;

    private string formattedAddressField;


}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
public partial class ResponseResourceSetsResourceSetResourcesResourceBusinessLocationEntityBusinessInfo
{

    private string idField;

    private string entityNameField;

    private object urlField;

    private string phoneField;

    private uint typeIdField;

    private object otherTypeIdsField;

    private string typesField;

    private object otherTypesField;


}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
public partial class ResponseResourceSetsResourceSetResourcesResourceAddressOfLocation
{

    private ResponseResourceSetsResourceSetResourcesResourceAddressOfLocationAddressInfo addressInfoField;

}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
public partial class ResponseResourceSetsResourceSetResourcesResourceAddressOfLocationAddressInfo
{

    private decimal latitudeField;

    private decimal longitudeField;

    private string addressLineField;

    private string localityField;

    private string neighborhoodField;

    private string adminDivisionField;

    private string countryIso2Field;

    private uint postalCodeField;

    private string formattedAddressField;


}

