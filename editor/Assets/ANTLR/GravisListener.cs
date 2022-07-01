//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Gravis.g4 by ANTLR 4.7.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="GravisParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public interface IGravisListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.file_input"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFile_input([NotNull] GravisParser.File_inputContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.file_input"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFile_input([NotNull] GravisParser.File_inputContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStmt([NotNull] GravisParser.StmtContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStmt([NotNull] GravisParser.StmtContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.link_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLink_stmt([NotNull] GravisParser.Link_stmtContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.link_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLink_stmt([NotNull] GravisParser.Link_stmtContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.def_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDef_stmt([NotNull] GravisParser.Def_stmtContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.def_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDef_stmt([NotNull] GravisParser.Def_stmtContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.node_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNode_def([NotNull] GravisParser.Node_defContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.node_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNode_def([NotNull] GravisParser.Node_defContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.input_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterInput_def([NotNull] GravisParser.Input_defContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.input_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitInput_def([NotNull] GravisParser.Input_defContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.output_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOutput_def([NotNull] GravisParser.Output_defContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.output_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOutput_def([NotNull] GravisParser.Output_defContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.const_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConst_def([NotNull] GravisParser.Const_defContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.const_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConst_def([NotNull] GravisParser.Const_defContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.if_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIf_def([NotNull] GravisParser.If_defContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.if_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIf_def([NotNull] GravisParser.If_defContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.opr_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOpr_def([NotNull] GravisParser.Opr_defContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.opr_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOpr_def([NotNull] GravisParser.Opr_defContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.subspace_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSubspace_def([NotNull] GravisParser.Subspace_defContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.subspace_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSubspace_def([NotNull] GravisParser.Subspace_defContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.self_subspace_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSelf_subspace_def([NotNull] GravisParser.Self_subspace_defContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.self_subspace_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSelf_subspace_def([NotNull] GravisParser.Self_subspace_defContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.dotted_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDotted_name([NotNull] GravisParser.Dotted_nameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.dotted_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDotted_name([NotNull] GravisParser.Dotted_nameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.node_inst"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNode_inst([NotNull] GravisParser.Node_instContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.node_inst"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNode_inst([NotNull] GravisParser.Node_instContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.comp_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterComp_op([NotNull] GravisParser.Comp_opContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.comp_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitComp_op([NotNull] GravisParser.Comp_opContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="GravisParser.arith_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArith_op([NotNull] GravisParser.Arith_opContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="GravisParser.arith_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArith_op([NotNull] GravisParser.Arith_opContext context);
}
