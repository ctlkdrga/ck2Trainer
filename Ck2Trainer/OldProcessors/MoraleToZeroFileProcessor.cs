﻿namespace Ck2.Trainer.OldProcessors
{
    public class MoraleToZeroFileProcessor : IFileProcessor
    {
        public string Process(string line)
        {
            if (line.Trim().Contains("morale="))
            {
                line = "morale=0.0";
            }

            return line;
        }



    }
}