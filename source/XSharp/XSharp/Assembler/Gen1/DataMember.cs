using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XSharp.Assembler
{
    public class DataMember : BaseAssemblerElement, IComparable<DataMember>
    {
        public const string IllegalIdentifierChars = "&.,+$<>{}-`\'/\\ ()[]*!=";

        public string Name { get; }
        public IEnumerable<string> AdditionalNames { get; }
        public bool IsComment { get; set; }
        public byte[] RawDefaultValue { get; set; }
        public uint Alignment { get; set; }
        public bool IsGlobal { get; set; }
        protected object[] UntypedDefaultValue;
        public string RawAsm = null;
        private string Size;
        private string StringValue;
        private Type Type;

        // Hack for not to emit raw data. See RawAsm
        public DataMember()
        {
            Name = "Dummy";
        }

        public DataMember(string aName, string aValue, bool noConvert = false)
        {
            if (noConvert)
            {
                Name = aName;
                StringValue = aValue;
            }
            else
            {
                Name = aName;
                var xBytes = Encoding.ASCII.GetBytes(aValue);
                var xBytes2 = new byte[xBytes.Length + 1];
                xBytes.CopyTo(xBytes2, 0);
                xBytes2[xBytes2.Length - 1] = 0;
                RawDefaultValue = xBytes2; StringValue = aValue;
            } 
        }

        public DataMember(string aName, string aValue, Type aType)
        {
            Name = aName;
            Type = aType;
            StringValue = aValue;
        }

        public DataMember(string aName, string size, string aValue)
        {
            Name = aName;
            Size = size;
            StringValue = aValue;
        }

        public DataMember(string aName, params object[] aDefaultValue)
        {
            Name = aName;
            UntypedDefaultValue = aDefaultValue;
        }

        public DataMember(string aName, byte[] aDefaultValue)
        {
            Name = aName;
            RawDefaultValue = aDefaultValue;
        }

        public DataMember(string aName, params short[] aDefaultValue)
        {
            Name = aName;
            UntypedDefaultValue = aDefaultValue.Cast<object>().ToArray();
        }

        public DataMember(string aName, params ushort[] aDefaultValue)
        {
            Name = aName;
            UntypedDefaultValue = aDefaultValue.Cast<object>().ToArray();
        }

        public DataMember(string aName, params uint[] aDefaultValue)
        {
            Name = aName;
            UntypedDefaultValue = aDefaultValue.Cast<object>().ToArray();
        }

        public DataMember(string aName, params int[] aDefaultValue)
        {
            Name = aName;
            UntypedDefaultValue = aDefaultValue.Cast<object>().ToArray();
        }

        public DataMember(string aName, params ulong[] aDefaultValue)
        {
            Name = aName;
            UntypedDefaultValue = aDefaultValue.Cast<object>().ToArray();
        }

        public DataMember(string aName, params long[] aDefaultValue)
        {
            Name = aName;
            UntypedDefaultValue = aDefaultValue.Cast<object>().ToArray();
        }

        public DataMember(string aName, Stream aData)
        {
            Name = aName;
            RawDefaultValue = new byte[aData.Length];
            aData.Read(RawDefaultValue, 0, RawDefaultValue.Length);
        }

        public DataMember(string aName, IEnumerable<string> aAdditionalNames, byte[] aDefaultValue)
        {
            Name = aName;
            AdditionalNames = aAdditionalNames;
            RawDefaultValue = aDefaultValue;
        }

        public static string FilterStringForIncorrectChars(string aName)
        {
            string xTempResult = aName;
            foreach (char c in IllegalIdentifierChars)
            {
                xTempResult = xTempResult.Replace(c, '_');
            }
            return xTempResult;
        }

        public override void WriteText(Assembler aAssembler, TextWriter aOutput)
        {
            if (RawAsm != null)
            {
                aOutput.WriteLine(RawAsm);
                return;
            }

            if (RawDefaultValue != null)
            {
                if (RawDefaultValue.Length == 0)
                {
                    aOutput.Write(Name);
                    aOutput.Write(":");
                    return;
                }
                if (RawDefaultValue.Length < 250 ||
                    (from item in RawDefaultValue
                     group item by item
                     into i
                     select i).Count() > 1)
                {
                    if (IsGlobal)
                    {
                        aOutput.Write("\tglobal ");
                        aOutput.WriteLine(Name);

                        if (AdditionalNames != null && AdditionalNames.Count() > 0)
                        {
                            foreach (var xName in AdditionalNames)
                            {
                                aOutput.Write("\tglobal");
                                aOutput.WriteLine(xName);
                            }
                        }
                    }

                    aOutput.WriteLine(Name + ":");

                    if (AdditionalNames != null && AdditionalNames.Count() > 0)
                    {
                        foreach(var xName in AdditionalNames)
                        {
                            aOutput.WriteLine("\t" + xName + ":");
                        }
                    }

                    aOutput.Write("\t  db ");
                    for (int i = 0; i < (RawDefaultValue.Length - 1); i++)
                    {
                        aOutput.Write(RawDefaultValue[i]);
                        aOutput.Write(", ");
                    }
                    aOutput.Write(RawDefaultValue.Last());
                }
                else
                {
                    if (IsGlobal)
                    {
                        aOutput.Write("global ");
                        aOutput.WriteLine(Name);

                        if (AdditionalNames != null && AdditionalNames.Count() > 0)
                        {
                            foreach (var xName in AdditionalNames)
                            {
                                aOutput.Write("\tglobal");
                                aOutput.WriteLine(xName);
                            }
                        }
                    }

                    if (AdditionalNames != null && AdditionalNames.Count() > 0)
                    {
                        aOutput.WriteLine(Name + ":");
                        foreach (var xName in AdditionalNames)
                        {
                            aOutput.WriteLine("\t" + xName + ":");
                        }
                    }
                    else
                    {
                        aOutput.Write(Name + ":");
                    }

                    aOutput.Write("\t  TIMES ");
                    aOutput.Write(RawDefaultValue.Length);
                    aOutput.Write(" db ");
                    aOutput.Write(RawDefaultValue[0]);
                }
                return;
            }
            if (UntypedDefaultValue != null)
            {
                if (IsGlobal)
                {
                    aOutput.Write("global ");
                    aOutput.WriteLine(Name);
                }
                aOutput.Write(Name);

                aOutput.Write(" " + GetStringFromType(UntypedDefaultValue[0].GetType()) + " ");

                Func<object, string> xGetTextForItem = delegate(object aItem)
                                                       {
                                                           if (!(aItem is ElementReference xElementRef))
                                                           {
                                                               return (aItem ?? 0).ToString();
                                                           }

                                                           if (xElementRef.Offset == 0)
                                                           {
                                                               return xElementRef.Name;
                                                           }
                                                           return xElementRef.Name + " + " + xElementRef.Offset;
                                                       };
                for (int i = 0; i < (UntypedDefaultValue.Length - 1); i++)
                {
                    aOutput.Write(xGetTextForItem(UntypedDefaultValue[i]));
                    aOutput.Write(", ");
                }
                aOutput.Write(xGetTextForItem(UntypedDefaultValue.Last()));
                return;
            }

            if (StringValue != null)
            {
                if (Type != null)
                {
                    aOutput.Write(Name);
                    aOutput.Write(" ");
                    aOutput.Write(GetStringFromType(Type));
                    aOutput.Write(" ");
                    aOutput.Write(StringValue);   
                }
                else if (Size != null)
                {
                    aOutput.Write(Name);
                    aOutput.Write(" ");
                    aOutput.Write(Size);
                    aOutput.Write(" ");
                    aOutput.Write(StringValue);
                }
                else
                {
                    aOutput.Write(Name);
                    aOutput.Write(" ");
                    aOutput.Write(StringValue);
                }
                return;
            }

            throw new Exception("Situation unsupported!");
        }

        public string GetStringFromType(Type aType)
        {
            if (aType == typeof(long) || aType == typeof(ulong) || aType == typeof(double))
            {
                return "dq";
            }
            else if (aType == typeof(short) || aType == typeof(ushort))
            {
                return "dw";
            }
            else if (aType == typeof(char) || aType == typeof(byte))
            {
                return "db";
            }
            else
            {
                return "dd";
            }
        }

        public int CompareTo(DataMember other)
        {
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        public override ulong? ActualAddress
        {
            get
            {
                // TODO: for now, we dont have any data alignment
                return StartAddress;
            }
        }

        public override void UpdateAddress(Assembler aAssembler, ref ulong aAddress)
        {
            if (Alignment > 0)
            {
                if (aAddress % Alignment != 0)
                {
                    aAddress += Alignment - (aAddress % Alignment);
                }
            }
            base.UpdateAddress(aAssembler, ref aAddress);
            if (RawDefaultValue != null)
            {
                aAddress += (ulong) RawDefaultValue.Length;
            }
            if (UntypedDefaultValue != null)
            {
                // TODO: what to do with 64bit target platforms? right now we only support 32bit
                aAddress += (ulong) (UntypedDefaultValue.Length * 4);
            }
        }

        public override bool IsComplete(Assembler aAssembler)
        {
            if (RawAsm != null)
            {
                return true;
            }

            if (UntypedDefaultValue != null && UntypedDefaultValue.Length > 0)
            {
                foreach (var xReference in (from item in UntypedDefaultValue
                                            let xRef = item as ElementReference
                                            where xRef != null
                                            select xRef))
                {
                    var xRef = aAssembler.TryResolveReference(xReference);

                    if (xRef == null)
                    {
                        return false;
                    }

                    if (!xRef.IsComplete(aAssembler))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override void WriteData(Assembler aAssembler, Stream aOutput)
        {
            if (UntypedDefaultValue != null && UntypedDefaultValue.Length > 0)
            {
                for (int i = 0; i < UntypedDefaultValue.Length; i++)
                {
                    if (UntypedDefaultValue[i] is ElementReference xRef)
                    {
                        var xTheRef = aAssembler.TryResolveReference(xRef);
                        if (xTheRef == null)
                        {
                            throw new Exception("Reference not found!");
                        }
                        if (!xTheRef.ActualAddress.HasValue)
                        {
                            Console.Write("");
                        }
                        aOutput.Write(BitConverter.GetBytes(xTheRef.ActualAddress.Value), 0, 4);
                    }
                    else
                    {
                        if (UntypedDefaultValue[i] is int)
                        {
                            aOutput.Write(BitConverter.GetBytes((int)UntypedDefaultValue[i]), 0, 4);
                        }
                        else
                        {
                            if (UntypedDefaultValue[i] is uint)
                            {
                                aOutput.Write(BitConverter.GetBytes((uint)UntypedDefaultValue[i]), 0, 4);
                            }
                            else
                            {
                                throw new Exception("Invalid value inside UntypedDefaultValue");
                            }
                        }
                    }
                }
            }
            else
            {
                aOutput.Write(RawDefaultValue, 0, RawDefaultValue.Length);
            }
        }
    }
}
