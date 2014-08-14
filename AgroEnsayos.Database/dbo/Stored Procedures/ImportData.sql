CREATE PROCEDURE ImportData AS
BEGIN
	SET IDENTITY_INSERT Companies ON
	INSERT INTO Companies (Id, Name, IsDisabled, Address, ZipCode, Locality, Country, Phone, Fax, Email, LogoUrl)
	SELECT Id, Nombre, Deshabilitada, domicilio, codigo_postal, localidad, pais, telefono, fax, email, url_logo FROM AgroEnsayos.dbo.Empresas
	SET IDENTITY_INSERT Companies OFF
	
	SET IDENTITY_INSERT Users ON
	INSERT INTO Users (Id, CompanyId, Name, Password, Role, IsDisabled)
	SELECT Id, EmpresaId, Name, Password, Role, IsDisabled FROM AgroEnsayos.dbo.Users 
	SET IDENTITY_INSERT Users OFF
	
	SET IDENTITY_INSERT Categories ON
	INSERT INTO Categories (Id, ParentId, Name)
	SELECT Id, PadreId, Nombre FROM AgroEnsayos.dbo.Categorias 
	SET IDENTITY_INSERT Categories OFF
	
	SET IDENTITY_INSERT Campaigns ON
	INSERT INTO Campaigns (Id, CategoryId, Name)
	SELECT Id, CategoriaId, Nombre FROM AgroEnsayos.dbo.Campanas 
	SET IDENTITY_INSERT Campaigns OFF
		
	SET IDENTITY_INSERT Places ON
	INSERT INTO Places (Id, Region, Province, Department, Header, Locality, Latitude, Longitude, Perimeter)
	SELECT Id, Region, Provincia, Departamento, Cabecera, Localidad, Latitud, Longitud, Perimetro FROM AgroEnsayos.dbo.Lugares 
	SET IDENTITY_INSERT Places OFF
		
	SET IDENTITY_INSERT Products ON
	INSERT INTO Products (Id, CategoryId, CompanyId, Name, Description, Material, IsHybrid, Cycle, IsConventional, DaysToFlowering, DaysToMaturity, PlantHeight, IsNew, Height, EntryDate, IsDisabled)
	SELECT Id, CategoriaId, EmpresaId, Nombre, DescripcionPG, Material, EsHibrido, Ciclo, EsConvencional, DiasFloracion, DiasMadurez, AlturaPlanta, EsNuevo, Alta, FechaCarga, Deshabilitado FROM AgroEnsayos.dbo.Productos 
	SET IDENTITY_INSERT Products OFF
		
	INSERT INTO ProductPlace (ProductId, PlaceId)
	SELECT ProductoId, LugarId FROM AgroEnsayos.dbo.ProductoLugares 
	
	SET IDENTITY_INSERT Attributes ON
	INSERT INTO Attributes (Id, Family, Name, DataType, Tags, IsFilter, IsDisabled)
	SELECT Id, Rubro, Nombre, TipoDato, Tags, UsarComoFiltro, Deshabilitado FROM AgroEnsayos.dbo.Atributos 
	SET IDENTITY_INSERT Attributes OFF
		
	INSERT INTO AttributeCategory (CategoryId, AttributeId)
	SELECT CategoriaId, AtributoId FROM AgroEnsayos.dbo.AtributoCategorias 
	
	
	INSERT INTO AttributeMappings (AttributeId, MappedValue, OriginalValue, Scale)
	SELECT distinct pa.AtributoId, ae.Equivalencia, ae.Valor, ae.Escala
	FROM AgroEnsayos.dbo.ProductoAtributos pa
	JOIN AgroEnsayos.dbo.Atributos a on a.Id = pa.AtributoId
	JOIN AgroEnsayos.dbo.AtributoEquivalencias ae on ae.Valor = pa.Valor and a.Id = ae.AtributoId	

	INSERT INTO ProductAttribute (ProductId, AttributeMappingId)
	SELECT pa.ProductoId, am.AttributeMappingId
	FROM AgroEnsayos.dbo.ProductoAtributos pa
	JOIN AttributeMappings am on pa.AtributoId = am.AttributeId and pa.Valor = am.OriginalValue
		
	SET IDENTITY_INSERT Tests ON
	INSERT INTO Tests (Id, CampaignId, ProductId, PlaceId, Yield, Source, Establishment, PlantingDate, HarvestDate, [Index], Observations, [File])
	SELECT Id, CampanaId, ProductoId, LugarId, Rinde, Fuente, Establecimiento, FechaSiembra, FechaCosecha, Indice, Observaciones, Archivo FROM AgroEnsayos.dbo.Ensayos 
	SET IDENTITY_INSERT Tests OFF
END