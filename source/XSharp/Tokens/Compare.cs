using System;
using Spruce.Attribs;

namespace XSharp.Tokens
{
    [GroupToken(typeof(CompareReg08Const), typeof(CompareReg16Const), typeof(CompareReg32Const),
        typeof(CompareReg08Var), typeof(CompareReg16Var), typeof(CompareReg32Var),
        typeof(CompareRegVarAddr), typeof(CompareReg8Int8),
        typeof(CompareReg16Int16), typeof(CompareReg32Int32), typeof(CompareConstConst), typeof(CompareConstVar),
        typeof(CompareConstVarAddr), typeof(CompareConstInt8), typeof(CompareConstInt16), typeof(CompareConstInt32),
        typeof(CompareVarConst), typeof(CompareVarVar), typeof(CompareVarVarAddr), typeof(CompareVarInt8),
        typeof(CompareVarInt16), typeof(CompareVarInt32), typeof(CompareVarAddrConst), typeof(CompareVarAddrVar),
        typeof(CompareVarAddrVarAddr), typeof(CompareVarAddrInt32), typeof(CompareInt8Int8), typeof(CompareInt8Const),
        typeof(CompareInt8Var), typeof(CompareInt16Int16), typeof(CompareInt16Const), typeof(CompareInt16Var),
        typeof(CompareInt32Int32), typeof(CompareInt32Const), typeof(CompareInt32Var), typeof(CompareInt32VarAddr))]
    public class Compare : Spruce.Tokens.Compound
    {
    }

    public abstract class Compare<TLeftValueType, TComparatorType, TRightValueType> : Compare
    {
        public Compare()
        {
            mInternals.Add((Spruce.Tokens.Token) Activator.CreateInstance(typeof(TLeftValueType)));
            mInternals.Add((Spruce.Tokens.Token) Activator.CreateInstance(typeof(TComparatorType)));
            mInternals.Add((Spruce.Tokens.Token) Activator.CreateInstance(typeof(TRightValueType)));
        }
    }

    [GroupToken(typeof(CompareReg08Const), typeof(CompareReg16Const), typeof(CompareReg32Const))]
    public class CompareRegConst
    {
    }

    public class CompareReg08Const : Compare<Reg08, OpCompare, Const>
    {
    }

    public class CompareReg16Const : Compare<Reg16, OpCompare, Const>
    {
    }

    public class CompareReg32Const : Compare<Reg32, OpCompare, Const>
    {
    }

    [GroupToken(typeof(CompareReg08Var), typeof(CompareReg16Var), typeof(CompareReg32Var))]
    public class CompareRegVar
    {
    }

    public class CompareReg08Var : Compare<Reg08, OpCompare, Variable>
    {
    }

    public class CompareReg16Var : Compare<Reg16, OpCompare, Variable>
    {
    }

    public class CompareReg32Var : Compare<Reg32, OpCompare, Variable>
    {
    }

    public class CompareRegVarAddr : Compare<Reg32, OpCompare, VariableAddress>
    {
    }

    public class CompareReg8Int8 : Compare<Reg08, OpCompare, Int08u>
    {
    }

    public class CompareReg16Int16 : Compare<Reg16, OpCompare, Int16u>
    {
    }

    public class CompareReg32Int32 : Compare<Reg32, OpCompare, Int32u>
    {
    }

    public class CompareConstConst : Compare<Const, OpCompare, Const>
    {
    }

    public class CompareConstVar : Compare<Const, OpCompare, Variable>
    {
    }

    public class CompareConstVarAddr : Compare<Const, OpCompare, VariableAddress>
    {
    }

    public class CompareConstInt8 : Compare<Const, OpCompare, Int08u>
    {
    }

    public class CompareConstInt16 : Compare<Const, OpCompare, Int16u>
    {
    }

    public class CompareConstInt32 : Compare<Const, OpCompare, Int32u>
    {
    }

    public class CompareVarConst : Compare<Variable, OpCompare, Const>
    {
    }

    public class CompareVarVar : Compare<Variable, OpCompare, Variable>
    {
    }

    public class CompareVarVarAddr : Compare<Variable, OpCompare, VariableAddress>
    {
    }

    public class CompareVarInt8 : Compare<Variable, OpCompare, Int08u>
    {
    }

    public class CompareVarInt16 : Compare<Variable, OpCompare, Int16u>
    {
    }

    public class CompareVarInt32 : Compare<Variable, OpCompare, Int32u>
    {
    }

    public class CompareVarAddrConst : Compare<VariableAddress, OpCompare, Const>
    {
    }

    public class CompareVarAddrVar : Compare<VariableAddress, OpCompare, Variable>
    {
    }

    public class CompareVarAddrVarAddr : Compare<VariableAddress, OpCompare, VariableAddress>
    {
    }

    public class CompareVarAddrInt32 : Compare<VariableAddress, OpCompare, Int32u>
    {
    }

    public class CompareInt8Int8 : Compare<Int08u, OpCompare, Int08u>
    {
    }

    public class CompareInt8Const : Compare<Int08u, OpCompare, Const>
    {
    }

    public class CompareInt8Var : Compare<Int08u, OpCompare, Variable>
    {
    }

    public class CompareInt16Int16 : Compare<Int16u, OpCompare, Int16u>
    {
    }

    public class CompareInt16Const : Compare<Int16u, OpCompare, Const>
    {
    }

    public class CompareInt16Var : Compare<Int16u, OpCompare, Variable>
    {
    }

    public class CompareInt32Int32 : Compare<Int32u, OpCompare, Int32u>
    {
    }

    public class CompareInt32Const : Compare<Int32u, OpCompare, Const>
    {
    }

    public class CompareInt32Var : Compare<Int32u, OpCompare, Variable>
    {
    }

    public class CompareInt32VarAddr : Compare<Int32u, OpCompare, VariableAddress>
    {
    }
}
