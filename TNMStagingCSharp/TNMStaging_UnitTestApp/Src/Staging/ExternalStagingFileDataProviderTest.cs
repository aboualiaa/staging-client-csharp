﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TNMStagingCSharp.Src.DecisionEngine;
using TNMStagingCSharp.Src.Staging;
using TNMStagingCSharp.Src.Staging.Entities;
using System.IO;
using System.Reflection;
using System.Collections.Generic;


namespace TNMStaging_UnitTestApp.Src
{
    [TestClass]
    public class ExternalStagingFileDataProviderTest
    {
        private static TNMStagingCSharp.Src.Staging.Staging _STAGING;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            String basedir = System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\..\\";
            if (System.IO.Directory.GetCurrentDirectory().IndexOf("x64") >= 0) basedir += "\\..\\";

            String sFilePath = basedir + "Resources\\Test\\Misc\\external_algorithm.zip";

            FileStream SourceStream = File.Open(sFilePath, FileMode.Open);

            ExternalStagingFileDataProvider provider = new ExternalStagingFileDataProvider(SourceStream);

            _STAGING = TNMStagingCSharp.Src.Staging.Staging.getInstance(provider);
        }

        [TestMethod]
        public void testExternalLoad()
        {
            Assert.AreEqual("testing", _STAGING.getAlgorithm());
            Assert.AreEqual("99.99", _STAGING.getVersion());
            Assert.AreEqual(1, _STAGING.getSchemaIds().Count);
            Assert.AreEqual(62, _STAGING.getTableIds().Count);

            StagingSchema schema = _STAGING.getSchema("urethra");
            Assert.IsNotNull(schema);
            Assert.AreEqual("testing", schema.getAlgorithm());
            Assert.AreEqual("99.99", schema.getVersion());

            StagingTable table = _STAGING.getTable("ajcc_descriptor_codes");
            Assert.IsNotNull(table);
            Assert.AreEqual("testing", table.getAlgorithm());
            Assert.AreEqual("99.99", table.getVersion());
            Assert.AreEqual(6, table.getTableRows().Count);

            HashSet<String> involved = _STAGING.getInvolvedTables("urethra");
            Assert.AreEqual(62, involved.Count);
            Assert.IsTrue(involved.Contains("mets_eval_ipa"));

            StagingData data = new StagingData();
            data.setInput("site", "C680");
            data.setInput("hist", "8000");
            data.setInput("behavior", "3");
            data.setInput("grade", "9");
            data.setInput("year_dx", "2013");
            data.setInput("cs_input_version_original", "020550");
            data.setInput("extension", "100");
            data.setInput("extension_eval", "9");
            data.setInput("nodes", "100");
            data.setInput("nodes_eval", "9");
            data.setInput("mets", "10");
            data.setInput("mets_eval", "9");

            // perform the staging
            _STAGING.stage(data);

            Assert.AreEqual(StagingData.Result.STAGED, data.getResult());
            Assert.AreEqual("urethra", data.getSchemaId());
            Assert.AreEqual(0, data.getErrors().Count);
            Assert.AreEqual(37, data.getPath().Count);

            // check output
            Assert.AreEqual("129", data.getOutput("schema_number"));
            Assert.AreEqual("020550", data.getOutput("csver_derived"));

            // AJCC 6
            Assert.AreEqual("70", data.getOutput("stor_ajcc6_stage"));

            // AJCC 7
            Assert.AreEqual("700", data.getOutput("stor_ajcc7_stage"));

            // Summary Stage
            Assert.AreEqual("7", data.getOutput("stor_ss77"));
            Assert.AreEqual("7", data.getOutput("stor_ss2000"));
        }
    }
}
