using System;
using System.Collections.Generic;

namespace Ck2.Save.File
{
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    public class DataLine : AbstractDataElement, IDataElement
    {
        public string Name => AsKeyVal != null ? AsKeyVal.Key : string.Empty;

        public IEnumerable<IDataElement> GetDescendants(string name)
        {
            return new IDataElement[] { };
        }



        public bool IsBlock => AsKeyVal?.Value.IsBlock ?? false;

        public DataBlock AsBlock => (DataBlock) (IsBlock ? AsKeyVal.Value : null);

        private string _asText;
        public string AsText
        {
            get
            {
                if (_asText != null)
                    return _asText;
                else
                    return AsKeyVal.ToWritableString(0);
            }
            set
            {
                _asText = value;
                ToBestRepresentation();
            }
        }

        public Property AsKeyVal;
        public bool HasTriedKeyVal = false;

        public IList<IDataElement> Children => AsKeyVal == null ? new IDataElement[0] : AsKeyVal.Value.Children;

        public override IDataElement Parent { get; }
        public int NestingLevel { get; }


        public DataLine(IDataElement parent, int nestingLevel)
        {
            Parent = parent;
            NestingLevel = nestingLevel;
        }


        public IDataElement ProcessLine(string line)
        {
            _asText = line.Trim();
            HasTriedKeyVal = false;
            ToBestRepresentation();

            return AsBlock ?? Parent;
        }

        private void ToBestRepresentation()
        {
            if (HasTriedKeyVal == true)
                return;

            var keyval = Property.FromDataLine(this);
            AsKeyVal = keyval;
            HasTriedKeyVal = true;

            // Don't keep duplicates in memory
            if (AsKeyVal != null)
                _asText = null;
        }


        public override string ToString()
        {
            if (AsKeyVal == null)
                return $"DL(t) <{AsText}>";
            else
                return $"DL(kv) <{AsKeyVal}>";
        }

        public string ToIndentedString()
        {
            if (AsKeyVal == null)
                return new string('\t', Math.Max(0, NestingLevel-1)) + AsText;
            else
                return new string('\t', NestingLevel) + AsKeyVal.ToWritableString(0);
        }

        public string ToUnindentedString()
        {
            if (AsKeyVal == null)
                return AsText;
            else
                return AsKeyVal.ToWritableString(0);
        }
    }
}