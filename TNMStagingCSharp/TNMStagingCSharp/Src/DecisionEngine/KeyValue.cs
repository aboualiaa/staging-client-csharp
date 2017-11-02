﻿// Copyright (C) 2017 Information Management Services, Inc.

using System;


namespace TNMStagingCSharp.Src.DecisionEngine
{
    public interface IKeyValue
    {
        //========================================================================================================================
        // Return the key
        // @return a String key
        //========================================================================================================================
        String getKey();

        //========================================================================================================================
        // Return the value
        // @return a String value
        //========================================================================================================================
        String getValue();
    }
}

