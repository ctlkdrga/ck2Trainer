﻿using Ck2.Save;

namespace Ck2.Trainer.Processors
{
    public interface ICk2Processor
    {
        void ApplyToNode(DataBlock node);
    }
}