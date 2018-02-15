namespace Spruce {
    public class CodePoint {
        public string FullText { get; }
        public int TextStart { get; }
        public int TextEnd { get; }
        public Tokens.Token Token { get; }
        public object Value { get; }

        public CodePoint(string aFullText, int aTextStart, int aTextEnd, Tokens.Token aToken, object aValue) {
            FullText = aFullText;
            //
            TextStart = aTextStart;
            TextEnd = aTextEnd;
            //
            Token = aToken;
            Value = aValue;
        }

        // Useful during debugging. Otherwise not currently used but could be by user in Emitters.
        public override string ToString() {
            return Value.ToString();
        }
    }
}
