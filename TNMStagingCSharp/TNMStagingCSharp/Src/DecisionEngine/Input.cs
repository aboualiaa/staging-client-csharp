﻿// Copyright (C) 2017 Information Management Services, Inc.

using System;


namespace TNMStagingCSharp.Src.DecisionEngine
{
    public interface IInput
    {
        //========================================================================================================================
        // The key representing the field name
        // @return a String name
        //========================================================================================================================
        String getKey();

        //========================================================================================================================
        // If supplied, the value a field will be assigned when it is not supplied
        // @return A String representing the default value
        //========================================================================================================================
        String getDefault();

        //========================================================================================================================
        // If supplied, the value of the field is verified to be contained in the table
        // @return a String representing the lookup table name
        //========================================================================================================================
        String getTable();

        //========================================================================================================================
        // Return true if the field is used in the calculation
        // @return a Boolean representing whether the field is used in the calculation
        //========================================================================================================================
        bool getUsedForStaging();
    }
}


