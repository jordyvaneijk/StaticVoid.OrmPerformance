﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Runner.Config
{
    public class RunnerConfig : IRunnerConfig
    {
        public int NumberOfRuns
        {
            get { return 3; }
        }

        public int DiscardWorst
        {
            get { return 1; }
        }

        public int DiscardHighestMemory
        {
            get { return 1; }
        }


        public int MaximumSampleSize
        {
            get { return 100; }
        }
    }
}
