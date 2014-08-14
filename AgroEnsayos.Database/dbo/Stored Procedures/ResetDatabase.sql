CREATE PROCEDURE [dbo].[ResetDatabase]	
AS
BEGIN
	delete from Users
	delete from Tests
	delete from Campaigns
	delete from ProductAttribute
	delete from AttributeMappings
	delete from AttributeCategory
	delete from Attributes
	delete from ProductPlace
	delete from Places
	delete from Products
	delete from Companies
	delete from Categories
	
	delete from Departments
	delete from Localities
	delete from Provinces
	delete from Temp_Places
	delete from Temp_Products
	delete from Temp_Tests

	DBCC CHECKIDENT (Companies, RESEED, 0)
	DBCC CHECKIDENT (Users, RESEED, 0)
	DBCC CHECKIDENT (Tests, RESEED, 0)
	DBCC CHECKIDENT (Campaigns, RESEED, 0)
	DBCC CHECKIDENT (AttributeMappings, RESEED, 0)
	DBCC CHECKIDENT (Attributes, RESEED, 0)
	DBCC CHECKIDENT (Places, RESEED, 0)
	DBCC CHECKIDENT (Products, RESEED, 0)
	DBCC CHECKIDENT (Categories, RESEED, 0)

END