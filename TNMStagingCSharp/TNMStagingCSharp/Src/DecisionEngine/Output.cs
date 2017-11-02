﻿// Copyright (C) 2017 Information Management Services, Inc.

using System;


namespace TNMStagingCSharp.Src.DecisionEngine
{
    public interface IOutput
    {
        //========================================================================================================================
        // The key representing the field name
        // @return a String name
        //========================================================================================================================
        String getKey();

        //========================================================================================================================
        // If supplied, the value of the field is verified to be contained in the table
        // @return a String representing the lookup table name
        //========================================================================================================================
        String getTable();

        //========================================================================================================================
        // If supplied, a default value to give the field at the beginning of the staging process
        // @return a default value to be set for the output
        //========================================================================================================================
        String getDefault();
    }
}


