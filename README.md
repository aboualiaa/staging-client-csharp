# staging-client-csharp

A cancer staging client library for C# applications.

## Supported staging algorithms

### EOD

Extent of Disease (EOD) is a set of three data items that describe how far a cancer has spread at the time of diagnosis. EOD 2018 is effective for cases diagnosed in 2018 and later.
 
In each EOD schema, valid values, definitions, and registrar notes are provided for
 
- EOD Primary Tumor
- EOD Lymph Nodes
- EOD Mets
- Summary Stage 2018
- Site-Specific Data Items (SSDIs), including grade, pertinent to the schema

For cancer cases diagnosed January 1, 2018 and later, the NCI SEER program will collect Extent of Disease (EOD) revised for 2018 and Summary Stage 2018. The schemas have been developed to be compatible with the AJCC 8th Edition chapter definitions. 

All of the standard setting organizations will collect the predictive and prognostic factors through Site Specific Data Items (SSDIs). Unlike the SSFs, these data items have formats and code structures specific to the data item.
 
Versions supported:

- 1.7 (released August 2019)

### TNM

TNM is a widely accepted system of cancer staging. TNM stands for Tumor, Nodes, and Metastasis. T is assigned based on the extent of involvement at the primary tumor site, N for the extent of involvement in regional lymph nodes, and M for distant spread. Clinical TNM is assigned prior to treatment and pathologic TNM is assigned based on clinical information plus information from surgery. The clinical TNM and the pathologic TNM values are summarized as clinical stage group or pathologic stage group.

For each cancer site, or schema, valid values, definitions, and registrar notes are provided for clinical TNM and stage group, pathologic TNM and stage group, and relevant Site-Specific Factors (SSFs).

TNM categories, stage groups, and definitions are based on the Union for International Cancer Control ([UICC](http://www.uicc.org/)) TNM 7th edition classification.  UICC 7th edition and AJCC 7th edition TNM categories and stage groups are very similar; however, there are some differences.

For diagnosis years 2016-2017, SEER Summary Stage 2000 is required. SEER Summary Stage 2000 should be collected manually unless the registry is collecting Collaborative Stage, which would derive Summary Stage 2000.

Versions supported:

- 1.9 (released June 2018)

### Collaborative Staging

[Collaborative Stage](https://cancerstaging.org/cstage/) is a unified data collection system designed to provide a common data set to meet the needs of all three
staging systems (TNM, SEER EOD, and SEER SS). It provides a comprehensive system to improve data quality by standardizing rules for timing, clinical and pathologic
assessments, and compatibility across all of the systems for all cancer sites.

Versions supported:

- 02.05.50 (released August 2018)

## Download

To download [the beta version of staging library - TNMStagingCSharp_v29.zip](https://github.com/imsweb/staging-client-csharp/releases/download/v2.8.4-beta/TNMStagingCSharp_v29.zip).

The download zip file contains the TNM Staging DLL and associated files. For more information, please reference the accompanying readme.txt file. Detailed documentation on how to use the DLL can be found in the [Wiki](https://github.com/imsweb/staging-client-csharp/wiki/).

## Requirements

Functional Requirements: You will need the .NET Framework 4.5.2 or higher installed to use this library. 

Data Requirements: You will need the algorithm data files for the TNM Staging Library to work properly. At present there are CS 02.05.50, TNM 1.9, and EOD 1.7 algorithms. You can find a copy of these data files within the TNM Staging source code in the Resources\Algorithms directory. The algorithm data files can be either in separate JSON files, or can be collected together in a compressed file such as .ZIP or .GZ. You can download the zip versions of [CS 02.05.50](https://github.com/imsweb/staging-client-csharp/releases/download/v2.4-beta/CS_02_05_50.zip), [TNM 1.9](https://github.com/imsweb/staging-client-csharp/releases/download/v2.3-beta/TNM_19.zip), and [EOD Public 1.7](https://github.com/imsweb/staging-client-csharp/releases/download/v2.8.3-beta/EOD_Public_17.zip) here. 

## Usage

More detailed documentation can be found in the [Wiki](https://github.com/imsweb/staging-client-csharp/wiki/).

### Get a `Staging` instance

Everything starts with getting an instance of the `Staging` object.  There are `DataProvider` objects for each staging algorithm and version.  The `Staging`
object is thread safe and cached so subsequent calls to `Staging.getInstance()` will return the same object.

To use a DataProvider, you will need a copy of a staging algorithm with the staging library. Each algorithm is composed of a collection of JSON files which represent the schemas and tables of that algorithm. These algorithm files can either be separate JSON files and in a directory on your hard drive, or they can be in a single zip file. 
The staging library contains 3 algorithms in the Resources sub directory. Included with the release of the staging library, we also include zip versions of the algorithms. We recommend using the zip versions as they are easier to maintain and replace with newer versions. 

For example, to use the Collaborative Stage algorithm (in a single zip file), use the ExternalStagingFileDataProvider class. This option allows you to use a single zip file containing the entire CS algorithm.
```csharp
using System.IO;
using TNMStagingCSharp.Src.Staging;

TNMStagingCSharp.Src.Staging.Staging _STAGING;
FileStream SourceStream = File.Open("CS_02_05_50.zip", FileMode.Open);
ExternalStagingFileDataProvider provider = new ExternalStagingFileDataProvider(SourceStream);

_STAGING = TNMStagingCSharp.Src.Staging.Staging.getInstance(provider);
```

To use the Collaborative Stage algorithm (in separate JSON files), use the CsDataProvider class. This option requires that you have the Algorithm files (JSON) in a directory named Algorithms \ CS \ [Version Number].
```csharp
using TNMStagingCSharp.Src.Staging.CS;

TNMStagingCSharp.Src.Staging.Staging _STAGING;
_STAGING = TNMStagingCSharp.Src.Staging.Staging.getInstance(CsDataProvider.getInstance(CsVersion.LATEST));
```


### Schemas

Schemas represent sets of specific staging instructions.  Determining the schema to use for staging is based on primary site, histology and sometimes additional
discrimator values.  Schemas include the following information:

- schema identifier (i.e. "prostate")
- algorithm identifier (i.e. "cs")
- algorithm version (i.e. "02.05.50")
- name
- title, subtitle, description and notes
- schema selection criteria
- input definitions describing the data needed for staging
- list of table identifiers involved in the schema
- a list of initial output values set at the start of staging
- a list of mappings which represent the logic used to calculate staging output

To get a list of all schema identifiers,

```csharp
HashSet<String> schemaIds = staging.getSchemaIds();
```

To get a single schema by identifer,

```csharp
StagingSchema schema = staging.getSchema("prostate");
```

### Tables

Tables represent the building blocks of the staging instructions specified in schemas.  Tables are used to define schema selection criteria, input validation and staging logic.
Tables include the following information:

- table identifier (i.e. "ajcc7_stage")
- algorithm identifier (i.e. "cs")
- algorithm version (i.e. "02.05.50")
- name
- title, subtitle, description, notes and footnotes
- list of column definitions
- list of table data

To get a list of all table identifiers,

```csharp
HashSet<String> tableIds = staging.getTableIds();
```

That list will be quite large.  To get a list of table indentifiers involved in a particular schema,

```csharp
HashSet<String> tableIds = staging.getInvolvedTables("prostate");
```

To get a single table by identifer,

```csharp
StagingTable table = staging.getTable("ajcc7_stage");
```

### Lookup a schema

A common operation is to look up a schema based on primary site, histology and optionally one or more discriminators.  Each staging algorithm has a `SchemaLookup` object
customized for the specific inputs needed to lookup a schema.

For Collaborative Staging, use the `CsSchemaLookup` object.  Here is a lookup based on site and histology.

```csharp
List<StagingSchema> lookup = staging.lookupSchema(new CsSchemaLookup("C629", "9231"));
Assert.AreEqual(1, lookup.Count);
Assert.AreEqual("testis", lookup[0].getId());
```

If the call returns a single result, then it was successful.  If it returns more than one result, then it needs a discriminator.  Information about the required discriminator
is included in the list of results.  In the Collaborative Staging example, the field `ssf25` is always used as the discriminator.  Other staging algorithms may use different
sets of discriminators that can be determined based on the result.

```csharp
// do not supply a discriminator
List<StagingSchema> lookup = staging.lookupSchema(new CsSchemaLookup("C111", "8200"));
Assert.AreEqual(2, lookup.Count);
foreach (StagingSchema schema in lookup)
    Assert.IsTrue(schema.getSchemaDiscriminators().Contains(CsStagingData.SSF25_KEY));

// supply a discriminator
lookup = staging.lookupSchema(new CsSchemaLookup("C111", "8200", "010"));
Assert.AreEqual(1, lookup.Count);
Assert.AreEqual("nasopharynx", lookup[0].getId());
Assert.AreEqual(34, lookup[0].getSchemaNum());
```

### Calculate stage

Staging a case requires first knowing which schema you are working with.  Once you have the schema, you can tell which fields (keys) need to be collected and supplied
to the `stage` method call.

A `StagingData` object is used to make staging calls.  All inputs to staging should be set on the `StagingData` object and the staging call will add the results.  The
results include:

- output - all output values resulting from the calculation
- errors - a list of errors and their descriptions
- path - an ordered list of the tables that were used in the calculation

Each algorithm has a specific `StagingData` entity which helps with preparing and evaluating staging calls.  For Collaborative Staging, use `CsStagingData`.  One
difference between this library and the original Collaborative Stage library is that you do not have pass all 25 site-specific factors for every staging call. Only
include the ones that are used in the schema being staged.

```csharp
CsStagingData data = new CsStagingData();
data.setInput(CsInput.PRIMARY_SITE, "C680");
data.setInput(CsInput.HISTOLOGY, "8000");
data.setInput(CsInput.BEHAVIOR, "3");
data.setInput(CsInput.GRADE, "9");
data.setInput(CsInput.DX_YEAR, "2013");
data.setInput(CsInput.CS_VERSION_ORIGINAL, "020550");
data.setInput(CsInput.TUMOR_SIZE, "075");
data.setInput(CsInput.EXTENSION, "100");
data.setInput(CsInput.EXTENSION_EVAL, "9");
data.setInput(CsInput.LYMPH_NODES, "100");
data.setInput(CsInput.LYMPH_NODES_EVAL, "9");
data.setInput(CsInput.REGIONAL_NODES_POSITIVE, "99");
data.setInput(CsInput.REGIONAL_NODES_EXAMINED, "99");
data.setInput(CsInput.METS_AT_DX, "10");
data.setInput(CsInput.METS_EVAL, "9");
data.setInput(CsInput.LVI, "9");
data.setInput(CsInput.AGE_AT_DX, "060");
data.setSsf(1, "020");

// perform the staging
staging.stage(data);

Assert.AreEqual(StagingData.Result.STAGED, data.getResult());
Assert.AreEqual("urethra", data.getSchemaId());
Assert.AreEqual(0, data.getErrors().Count);
Assert.AreEqual(37, data.getPath().Count);

// check output
Assert.AreEqual("129", data.getOutput(CsOutput.SCHEMA_NUMBER));
Assert.AreEqual("020550", data.getOutput(CsOutput.CSVER_DERIVED));

// AJCC 6
Assert.AreEqual("T1", data.getOutput(CsOutput.AJCC6_T));
Assert.AreEqual("c", data.getOutput(CsOutput.AJCC6_TDESCRIPTOR));
Assert.AreEqual("N1", data.getOutput(CsOutput.AJCC6_N));
Assert.AreEqual("c", data.getOutput(CsOutput.AJCC6_NDESCRIPTOR));
Assert.AreEqual("M1", data.getOutput(CsOutput.AJCC6_M));
Assert.AreEqual("c", data.getOutput(CsOutput.AJCC6_MDESCRIPTOR));
Assert.AreEqual("IV", data.getOutput(CsOutput.AJCC6_STAGE));
Assert.AreEqual("10", data.getOutput(CsOutput.STOR_AJCC6_T));
Assert.AreEqual("c", data.getOutput(CsOutput.STOR_AJCC6_TDESCRIPTOR));
Assert.AreEqual("10", data.getOutput(CsOutput.STOR_AJCC6_N));
Assert.AreEqual("c", data.getOutput(CsOutput.STOR_AJCC6_NDESCRIPTOR));
Assert.AreEqual("10", data.getOutput(CsOutput.STOR_AJCC6_M));
Assert.AreEqual("c", data.getOutput(CsOutput.STOR_AJCC6_MDESCRIPTOR));
Assert.AreEqual("70", data.getOutput(CsOutput.STOR_AJCC6_STAGE));

// AJCC 7
Assert.AreEqual("T1", data.getOutput(CsOutput.AJCC7_T));
Assert.AreEqual("c", data.getOutput(CsOutput.AJCC7_TDESCRIPTOR));
Assert.AreEqual("N1", data.getOutput(CsOutput.AJCC7_N));
Assert.AreEqual("c", data.getOutput(CsOutput.AJCC7_NDESCRIPTOR));
Assert.AreEqual("M1", data.getOutput(CsOutput.AJCC7_M));
Assert.AreEqual("c", data.getOutput(CsOutput.AJCC7_MDESCRIPTOR));
Assert.AreEqual("IV", data.getOutput(CsOutput.AJCC7_STAGE));
Assert.AreEqual("100", data.getOutput(CsOutput.STOR_AJCC7_T));
Assert.AreEqual("c", data.getOutput(CsOutput.STOR_AJCC6_TDESCRIPTOR));
Assert.AreEqual("100", data.getOutput(CsOutput.STOR_AJCC7_N));
Assert.AreEqual("c", data.getOutput(CsOutput.STOR_AJCC7_NDESCRIPTOR));
Assert.AreEqual("100", data.getOutput(CsOutput.STOR_AJCC7_M));
Assert.AreEqual("c", data.getOutput(CsOutput.STOR_AJCC7_MDESCRIPTOR));
Assert.AreEqual("700", data.getOutput(CsOutput.STOR_AJCC7_STAGE));

// Summary Stage
Assert.AreEqual("L", data.getOutput(CsOutput.SS1977_T));
Assert.AreEqual("RN", data.getOutput(CsOutput.SS1977_N));
Assert.AreEqual("D", data.getOutput(CsOutput.SS1977_M));
Assert.AreEqual("D", data.getOutput(CsOutput.SS1977_STAGE));
Assert.AreEqual("L", data.getOutput(CsOutput.SS2000_T));
Assert.AreEqual("RN", data.getOutput(CsOutput.SS2000_N));
Assert.AreEqual("D", data.getOutput(CsOutput.SS2000_M));
Assert.AreEqual("D", data.getOutput(CsOutput.SS2000_STAGE));
Assert.AreEqual("7", data.getOutput(CsOutput.STOR_SS1977_STAGE));
Assert.AreEqual("7", data.getOutput(CsOutput.STOR_SS2000_STAGE));
```

## About SEER

The Surveillance, Epidemiology and End Results ([SEER](http://seer.cancer.gov)) Program is a premier source for cancer statistics in the United States. The SEER
Program collects information on incidence, prevalence and survival from specific geographic areas representing 28 percent of the US population and reports on all
these data plus cancer mortality data for the entire country.


