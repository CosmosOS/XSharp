using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens
{
    public class OpEquals : Match {
        public OpEquals() : base("=") { }
    }

    public class OpSlashBack : Match {
        public OpSlashBack() : base(@"\") { }
    }

    public class OpSlashFwd : Match {
        public OpSlashFwd() : base("/") { }
    }

    public class OpPlus : Match {
        public OpPlus() : base("+") { }
    }

    public class OpMinus : Match {
        public OpMinus() : base("-") { }
    }

    public class OpStar : Match {
        public OpStar() : base("*") { }
    }

    public class OpPipe : Match {
        public OpPipe() : base("|") { }
    }

    public class OpTilde : Match {
        public OpTilde() : base("~") { }
    }

    public class OpPercent : Match {
        public OpPercent() : base("%") { }
    }

    public class OpCaret : Match {
        public OpCaret() : base("^") { }
    }

    public class OpAt : Match {
        public OpAt() : base("@") { }
    }
}
