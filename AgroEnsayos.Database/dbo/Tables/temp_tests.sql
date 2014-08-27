CREATE TABLE [dbo].[temp_ensayos] (
    [Fuente]              VARCHAR (50)  NULL,
    [Provincia]           VARCHAR (50)  NULL,
    [Localidad]           VARCHAR (50)  NULL,
    [Mal]                 VARCHAR (50)  NULL,
    [Establecimiento]     VARCHAR (50)  NULL,
    [Campana]             VARCHAR (50)  NULL,
    [FechaSiembra]        DATETIME      NULL,
    [FechaCosecha]        DATETIME      NULL,
    [Producto]            VARCHAR (100) NULL,
    [ColumnaVacia]        VARCHAR (50)  NULL,
    [Rinde]               FLOAT (53)    NULL,
    [RindeAjustado]       FLOAT (53)    NULL,
    [Indice]              VARCHAR (50)  NULL,
    [Quebrado]            VARCHAR (50)  NULL,
    [Vuelco]              VARCHAR (50)  NULL,
    [AlturaPlanta]        VARCHAR (50)  NULL,
    [Humedad]             VARCHAR (50)  NULL,
    [EsPromedio]          VARCHAR (50)  NULL,
    [PlantasXHectarea]    VARCHAR (50)  NULL,
    [DiasFloracion]       VARCHAR (50)  NULL,
    [EmergenciaFloracion] VARCHAR (50)  NULL,
    [Observaciones]       VARCHAR (100) NULL,
    [Archivo]             VARCHAR (100) NULL,
    [Fungicida]           BIT           NULL,
    [Row]                 INT           NULL
);



