using JeniusApps.Nightingale.Converters.OData;
using JeniusApps.Nightingale.Data.Models;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using System.IO;
using System.Linq;
using System.Xml;
using Xunit;

namespace Nightingale.Test
{
    public class ODataConverterTest
    {
        private IEdmModel model;
        private Item Item;
        private ODataConverter converter;
        public ODataConverterTest()
        {
            // This line gives us only the reference edmmodel.
            // It isn't used in the converter.
            CsdlReader.TryParse(XmlReader.Create(new StringReader(sampleMetadataDocument)), out model, out _);
            converter = new ODataConverter();
            Item = converter.ConvertCollection(sampleMetadataDocument);
        }

        [Fact]
        public void TheEntityContainerNameMustMatchTheItemName()
        {
            Assert.Equal(model.EntityContainer.Name, Item.Name);
        }

        [Fact]
        public void TheUrlMustHaveAPlaceHolderForBaseUrl()
        {
            var entityset =model.EntityContainer.Elements.First(d => d.ContainerElementKind == EdmContainerElementKind.EntitySet).Name;
            Assert.Contains("{{" + ODataConverter.NameOfPlaceHolder + "}}/", Item.
                Children.
                First(d => d.Name == entityset).
                    Children.
                    First().Url.Base);
        }

        [Fact]
        public void AllSingleEntityUrlsMustHaveAPlaceHolderForTheId()
        {
            var entityset = model.EntityContainer.Elements.First(d => d.ContainerElementKind == EdmContainerElementKind.EntitySet).Name;
            Assert.Contains("{{id}}", Item.
                Children.
                First(d => d.Name == entityset).
                    Children.
                    First(r => r.Name == "Single").Url.Base);
        }

        [Fact]
        public void ASingletonOnlyCreatesTwoRequests()
        {
            var singeltonEntity = model.EntityContainer.Elements.First(d => d.ContainerElementKind == EdmContainerElementKind.Singleton);

            Assert.Single(Item.Children.Where(d => d.Name == singeltonEntity.Name));
            Assert.Equal(2, Item.Children.First(d => d.Name == singeltonEntity.Name).Children.Count());
        }

        [Fact]
        public void AEntityCreatesFourRequests()
        {
            var entity = model.EntityContainer.Elements.First(d => d.ContainerElementKind == EdmContainerElementKind.EntitySet);
            Assert.Single(Item.Children.Where(d => d.Name == entity.Name));
            Assert.Equal(4, Item.Children.First(d => d.Name == entity.Name).Children.Count());
        }

        [Fact]
        public void ConvertingAWrongMetadataDocumentThrowsAnError()
        {
            Assert.Throws<ODataException>(() => converter.ConvertCollection("random string"));
        }

        /// <summary>
        /// This is a OData metadocument sample
        /// It was original written here: https://github.com/OData/AspNetCoreOData/blob/d324fc04bd291cdda061310f0e56ff7aefd3ac00/test/Microsoft.AspNetCore.OData.Tests/Formatter/Deserialization/ODataResourceDeserializerTests.cs#L1149
        /// </summary>
        private const string sampleMetadataDocument = @"<?xml version=""1.0"" encoding=""utf-8""?>
<edmx:Edmx Version=""4.0""
    xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
  <edmx:DataServices m:DataServiceVersion=""4.0"" m:MaxDataServiceVersion=""4.0""
      xmlns:m=""http://docs.oasis-open.org/odata/ns/metadata"">
    <Schema Namespace=""ODataDemo""
        xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityType Name=""Product"">
        <Key>
          <PropertyRef Name=""ID"" />
        </Key>
        <Property Name=""ID"" Type=""Edm.Int32"" Nullable=""false"" />
        <Property Name=""Name"" Type=""Edm.String"" />
        <Property Name=""Description"" Type=""Edm.String"" />
        <Property Name=""ReleaseDate"" Type=""Edm.DateTimeOffset"" Nullable=""false"" />
        <Property Name=""DiscontinuedDate"" Type=""Edm.DateTimeOffset"" />
        <Property Name=""PublishDate"" Type=""Edm.Date"" />
        <Property Name=""Rating"" Type=""Edm.Int32"" Nullable=""false"" />
        <Property Name=""Price"" Type=""Edm.Decimal"" Nullable=""false"" />
        <NavigationProperty Name=""Category"" Type=""ODataDemo.Category"" Partner=""Products"" />
        <NavigationProperty Name=""Supplier"" Type=""ODataDemo.Supplier"" Partner=""Products"" />
      </EntityType>
      <EntityType Name=""Category"">
        <Key>
          <PropertyRef Name=""ID"" />
        </Key>
        <Property Name=""ID"" Type=""Edm.Int32"" Nullable=""false"" />
        <Property Name=""Name"" Type=""Edm.String"" />
        <NavigationProperty Name=""Products"" Type=""Collection(ODataDemo.Product)"" Partner=""Category"" />
      </EntityType>
      <EntityType Name=""Supplier"">
        <Key>
          <PropertyRef Name=""ID"" />
        </Key>
        <Property Name=""ID"" Type=""Edm.Int32"" Nullable=""false"" />
        <Property Name=""Name"" Type=""Edm.String"" />
        <Property Name=""Address"" Type=""ODataDemo.Address"" />
        <Property Name=""Location"" Type=""Edm.GeographyPoint"" SRID=""Variable"" />
        <Property Name=""Concurrency"" Type=""Edm.Int32"" Nullable=""false"" />
        <NavigationProperty Name=""Products"" Type=""Collection(ODataDemo.Product)"" Partner=""Supplier"" />
      </EntityType>
      <ComplexType Name=""Address"">
        <Property Name=""Street"" Type=""Edm.String"" />
        <Property Name=""City"" Type=""Edm.String"" />
        <Property Name=""State"" Type=""Edm.String"" />
        <Property Name=""ZipCode"" Type=""Edm.String"" />
        <Property Name=""CountryOrRegion"" Type=""Edm.String"" />
      </ComplexType>
      <Function Name=""GetProductsByRating"" m:HttpMethod=""GET"">
        <ReturnType Type=""Collection(ODataDemo.Product)"" />
        <Parameter Name=""rating"" Type=""Edm.Int32"" Nullable=""false"" />
      </Function>
      <EntityContainer Name=""DemoService"" m:IsDefaultEntityContainer=""true"">
        <EntitySet Name=""Products"" EntityType=""ODataDemo.Product"">
          <NavigationPropertyBinding Path=""Category"" Target=""Categories"" />
          <NavigationPropertyBinding Path=""Supplier"" Target=""Suppliers"" />
        </EntitySet>
        <EntitySet Name=""Categories"" EntityType=""ODataDemo.Category"">
          <NavigationPropertyBinding Path=""Products"" Target=""Products"" />
        </EntitySet>
        <Singleton Name=""Me"" Type=""ODataDemo.Supplier""/>
        <EntitySet Name=""Suppliers"" EntityType=""ODataDemo.Supplier"">
          <NavigationPropertyBinding Path=""Products"" Target=""Products"" />
          <Annotation Term=""Org.OData.Core.V1.OptimisticConcurrency"">
            <Collection>
              <PropertyPath>Concurrency</PropertyPath>
            </Collection>
          </Annotation>
        </EntitySet>
        <FunctionImport Name=""GetProductsByRating"" Function=""ODataDemo.GetProductsByRating"" EntitySet=""Products"" IncludeInServiceDocument=""true"" />
      </EntityContainer>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>";
    }
}
