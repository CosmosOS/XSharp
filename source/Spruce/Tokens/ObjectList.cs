using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spruce.Tokens {
    public class ObjectList<T> : Token {
        protected readonly bool mIgnoreCase;
        protected readonly Dictionary<string, T> mList = new Dictionary<string, T>();

        protected ObjectList(string[] aKeyList, T[] aObjList, string aNoobChars = "", bool aIgnoreCase = true) {
            mIgnoreCase = aIgnoreCase;
            mMaxLength = aKeyList.Max(q => q.Length);
            BuildChars(aKeyList, aNoobChars, aIgnoreCase);

            for (int i = 0; i < aKeyList.Length; i++) {
                mList.Add(mIgnoreCase ? aKeyList[i].ToUpper() : aKeyList[i], aObjList[i]);
            }
        }

        protected override bool Check(string aText) {
            return mList.ContainsKey(mIgnoreCase ? aText.ToUpper() : aText);
        }

        protected override object Transform(string aText) {
            return mList[mIgnoreCase ? aText.ToUpper() : aText];
        }
    }
}
