﻿namespace StaticVoid.OrmPerformance.Harness.EFTypeProvider4

open System.Data.Linq
open System.Data.EntityClient
open FSharp.Data.TypeProviders
open StaticVoid.OrmPerformance.Harness.Contract
open FSharp.Data.Runtime

type internal TestDb = SqlEntityConnection<ConnectionStringName="TestDB",Pluralize=true> //SqlEntityConnection<ConnectionString="Server=localhost;Database=StaticVoid.OrmPerformance.Test;Integrated Security=SSPI;MultipleActiveResultSets=true", Pluralize=true>

[<AutoOpen>]
module internal DbExtensions =

    type TestDb with
        static member GetConfiguredContext(cnstr:IConnectionString) =
            TestDb.GetDataContext(cnstr.FormattedConnectionString + "MultipleActiveResultSets=true;")