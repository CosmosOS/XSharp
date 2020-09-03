'use strict';

import * as vscode from 'vscode';
import { EOL, platform } from 'os'
import * as cp from 'child_process';
import * as fs from 'fs';
import * as path from 'path';
import { Platform, Operator, Operators, Register } from './language';
import { BuildTaskProvider } from './buildTaskProvider';
import { Console } from 'console';

let currentPlatform: Platform = Platform.x86;
let functions: Map<vscode.Uri, XSharpFunction[]> = new Map<vscode.Uri, XSharpFunction[]>();
let compilerPath: string;

let taskProvider: vscode.Disposable;

let storagePath: string;

// settings
let compileOnSave: boolean;
let compileOutputPath: string;
let nasmPath: string;

export function activate(context: vscode.ExtensionContext) {
    //compilerPath = context.asAbsolutePath("compiler/xsc.exe");

    storagePath = context.storagePath != undefined ? context.storagePath : context.extensionPath;

    compileOnSave = vscode.workspace.getConfiguration("xsharp").get<boolean>("compileOnSave");
    compileOutputPath = vscode.workspace.getConfiguration("xsharp").get<string>("compileOutputPath");
    compilerPath = vscode.workspace.getConfiguration("xsharp").get<string>("compilerPath");
    nasmPath = vscode.workspace.getConfiguration("xsharp").get<string>("nasmPath");
    checkCompilerPathSet();

    taskProvider = vscode.tasks.registerTaskProvider("process", new BuildTaskProvider(""));

    let compileAsmToBin = vscode.commands.registerCommand("xsharp.createBin", function (){
        if(compileOutputPath == ""){
            vscode.window.showErrorMessage("Output path for compiler must be set!");
            return;
        }
        if(fs.readdirSync(compileOutputPath).length == 0){
            vscode.window.showErrorMessage("No files found in " + compileOutputPath + " to convert to bin");
            return;
        }
        if(nasmPath == ""){
            vscode.window.showErrorMessage("Nasm Path must be set!");
            return;
        }
        let files : string[] = [];
        fs.readdirSync(compileOutputPath).forEach(file => {
            if(path.extname(file) == ".asm"){
                files.push(path.join(compileOutputPath, file));
                console.log("Adding " + path.join(compileOutputPath, file) + " to files to convert");
            }
        });
        let command = "\"" + nasmPath + "\" -f bin -o " + compileOutputPath + "output.bin ";
        files.forEach(f => command +=  "\"" + f + "\" ");
        console.log("Executing command: " + command);
        cp.exec(command, (error, stdout, stderr) => {
            if(error != null){
                console.log("Error: " + error.message);
                vscode.window.showErrorMessage("Error while running nasm: " + error.message);
            }
            console.log("stdout: " + stdout)
        });
        vscode.window.showInformationMessage("Succesfully created output.bin file!");
    });

    let languageCompileFile = vscode.commands.registerTextEditorCommand("xsharp.compileFile", function (textEditor) {
        checkCompilerPathSet();
        if(compileOutputPath == ""){
            compileOutputPath = path.join(path.dirname(textEditor.document.fileName), "/bin/");
            vscode.workspace.getConfiguration("xsharp").update("compileOutputPath", compileOutputPath);
        }
        console.log("Triggered compile single file");
        if (vscode.languages.match("xsharp", textEditor.document)) {
            let unsaved: boolean = false;

            if (textEditor.document.isDirty) {
                unsaved = true;
            }

            compileDocument(textEditor.document, true);
            vscode.window.showInformationMessage("Compiled current file");
        }
    });

    let languageCompileAllFiles = vscode.commands.registerTextEditorCommand("xsharp.compileAllFiles", function (textEditor) {
        checkCompilerPathSet();
        if(compileOutputPath == ""){
            compileOutputPath = path.join(path.dirname(textEditor.document.fileName), "/bin/")
        }
        let visibleDocumentsUri: vscode.Uri[] = new Array<vscode.Uri>();

        vscode.window.visibleTextEditors.forEach(function (textEditor) {
            if (vscode.languages.match("xsharp", textEditor.document)) {
                visibleDocumentsUri.push(textEditor.document.uri);
                compileDocument(textEditor.document, textEditor.document.isDirty)
            }
        });

        vscode.workspace.findFiles("*.xs").then(function (uri) {
            uri.forEach(u => function () {
                if (visibleDocumentsUri.find(docUri => docUri == u) == undefined) {
                    let textDoc = vscode.workspace.textDocuments.find(doc => doc.uri == u);

                    if (textDoc != undefined) {
                        compileDocument(textDoc);
                    }
                    else {
                        vscode.workspace.openTextDocument(u).then(compileDocument);
                    }
                }
            });
        });

        vscode.window.showInformationMessage("Compiled all files in current directory");
    });

    if (vscode.workspace.rootPath != undefined) {
        vscode.workspace.findFiles("*.xs").then(function (uri) {
            uri.forEach(u => vscode.workspace.openTextDocument(u).then(parseFunctions));
        })
    }
    else {
        vscode.window.visibleTextEditors.forEach(e => parseFunctions(e.document));
    }

    let languageOnActiveTextEditorChanged = vscode.window.onDidChangeActiveTextEditor(function (e) {
        if (e != undefined && vscode.languages.match("xsharp", e.document)) {
            //updateCurrentPlatform(e);
        }
    });

    let languageOnTextSelectionChanged = vscode.window.onDidChangeTextEditorSelection(function (e) {
        if (e.textEditor.document.languageId == "xsharp" &&
            (currentPlatform == undefined ||
                e.selections.find(s => s.intersection(e.textEditor.document.lineAt(0).range) != undefined) != undefined)) {
            //updateCurrentPlatform(e.textEditor);
        }
    });

    let languageOnDocumentOpened = vscode.workspace.onDidOpenTextDocument(function (e) {
        if (vscode.languages.match("xsharp", e)) {
            parseFunctions(e);
        }
    });

    let languageOnDocumentSaved = vscode.workspace.onDidSaveTextDocument(function (e) {
        if (vscode.languages.match("xsharp", e)) {
            parseFunctions(e);

            if (compileOnSave) {
                compileDocument(e);
            }
        }
    });

    let languageHoverProvider = vscode.languages.registerHoverProvider("xsharp", {
        provideHover(document, position, token) {
            let text = document.getText(document.getWordRangeAtPosition(position, /\w+/g));
            let registers = currentPlatform.Registers;

            if (registers.some(r => r.Name == text)) {
                return new vscode.Hover(registers.find(r => r.Name == text).Description);
            } else {
                let regexp = new RegExp(Operators.map(o => escapeRegExp(o.Symbol)).join("|"));
                let operator = document.getText(document.getWordRangeAtPosition(position, regexp));

                if (Operators.some(o => o.Symbol == operator)) {
                    return new vscode.Hover(Operators.find(o => o.Symbol == operator).Description);
                }
            }
        }
    });

    let languageCompletionProvider = vscode.languages.registerCompletionItemProvider("xsharp", {
        provideCompletionItems(document, position, token) {
            let completionList = new vscode.CompletionList();

            currentPlatform.Registers.forEach(r =>
                completionList.items.push(new vscode.CompletionItem(r.Name, vscode.CompletionItemKind.Variable)));

            functions.forEach(u => u.forEach(f =>
                completionList.items.push(new vscode.CompletionItem(f.Name, vscode.CompletionItemKind.Function))))

            return completionList;
        }
    }, "");

    let languageSymbolDefinitions = vscode.languages.registerDefinitionProvider("xsharp", {
        provideDefinition(document, position, token) {
            let functionWordRange = document.getWordRangeAtPosition(position, /\w+(?=\(\))/g);

            if (functionWordRange != undefined) {
                let functionName = document.getText(functionWordRange);
                let xsharpFunction: XSharpFunction;

                for (let functionsArray of functions.values()) {
                    xsharpFunction = functionsArray.find(f => f.Name == functionName);

                    if (xsharpFunction != undefined) {
                        return xsharpFunction.Location;
                    }
                }
            }
        }
    });

    // let languageOnTypeFormattingEditProvider = vscode.languages.registerOnTypeFormattingEditProvider("xsharp", {
    //     provideOnTypeFormattingEdits(document, position, token) {
    //         let wordRange = document.getWordRangeAtPosition(position, /\/?\*/g);

    //         if (position.isEqual(wordRange.end)) {
    //             let insertSpaces = vscode.window.activeTextEditor.options.insertSpaces;
    //             let tabSize = <number>vscode.window.activeTextEditor.options.tabSize;
    //             let tab = insertSpaces ? " ".repeat(tabSize) : "\t";
    //             let newLine = EOL;

    //             if (wordRange.start.character > 0) {
    //                 if (insertSpaces) {
    //                     newLine += " ".repeat(wordRange.start.character + 1);
    //                 }
    //                 else {
    //                     // needs testing
    //                     newLine += "\t".repeat((wordRange.start.character + 1) / tabSize) +
    //                         " ".repeat((wordRange.start.character + 1) % tabSize);
    //                 }
    //             }

    //             return [new vscode.TextEdit(new vscode.Range(new vscode.Position(position.line + 1, (newLine + tab).length),
    //                 new vscode.Position(position.line + 1, (newLine + tab).length)), newLine + tab + newLine + "*/")];
    //         }
    //     }
    // }, "*", EOL);

    context.subscriptions.push(languageOnActiveTextEditorChanged);
    context.subscriptions.push(languageOnTextSelectionChanged);
    context.subscriptions.push(languageOnDocumentOpened);
    context.subscriptions.push(languageOnDocumentSaved);
    context.subscriptions.push(languageHoverProvider);
    context.subscriptions.push(languageCompletionProvider);
    context.subscriptions.push(languageSymbolDefinitions);
    //context.subscriptions.push(languageOnTypeFormattingEditProvider);
}

async function checkCompilerPathSet() {
    compilerPath = vscode.workspace.getConfiguration("xsharp").get("compilerPath");
    while (compilerPath == "") {
        compilerPath = await vscode.window.showInputBox({prompt:"Path to compiler", ignoreFocusOut: true});
    }
    vscode.workspace.getConfiguration("xsharp").update("compilerPath", compilerPath);
}

export function deactivate() : void{ 
    if (taskProvider) {
        taskProvider.dispose();
    }
}

function updateCurrentPlatform(e: vscode.TextEditor) {
    // currently X# only supports x86
    if (e.document.languageId == "xsharp") {
        let firstLine = e.document.lineAt(0).text;

        if (firstLine.startsWith("#define Platform ")) {
            let platform = Platform[firstLine.replace("#define Platform ", "").trim()];

            if (platform != undefined) {
                currentPlatform = platform;
            }
        }
    }
}

function parseFunctions(document: vscode.TextDocument) {
    if (functions.has(document.uri)) {
        functions.get(document.uri).length = 0;
    }
    else {
        functions.set(document.uri, []);
    }

    for (var i = 0; i < document.lineCount; i++) {
        let line = document.lineAt(i);
        let regexp = /function (\w+)/g;
        var result: RegExpExecArray;

        while ((result = regexp.exec(line.text)) !== null) {
            functions.get(document.uri).push(new XSharpFunction(result[1],
                new vscode.Location(document.uri, new vscode.Position(i, result.index + "function ".length))));
            regexp.lastIndex++;
        }
    }
}

function compileDocument(document: vscode.TextDocument, unsaved: boolean = false) {
    let inputPath: string;
    if (unsaved) {
        if (!fs.existsSync(storagePath)) {
            fs.mkdirSync(storagePath);
        }

        let i: number = 1;

        while (fs.existsSync(inputPath = path.join(storagePath, "temp_" + i + ".xs"))) {
            i++;
        }

        fs.writeFileSync(inputPath, document.getText());
    }
    else {
        inputPath = document.uri.fsPath;
    }

    if (!fs.existsSync(compileOutputPath)) {
        fs.mkdirSync(compileOutputPath);
    }

    let passed : boolean = false;
    try {
        var command = compilerPath + " " + inputPath + " -Out:" + path.join(compileOutputPath, path.basename(document.fileName).replace(".xs", ".asm")) + " -Gen2 -CPU:X86";
        cp.exec(command, (error, stdout, sterr) => {
            if(error != null){
                console.log("error: " + error.name + error.message)
                vscode.window.showErrorMessage("Error while compiling: " + error.message);
            } else{
                passed = true;
            }
        });
    }
    catch (e){
        console.log("error:" + e);
    }
    finally {
        if (unsaved && passed) {
            fs.unlinkSync(inputPath);
        }
    }
}

class XSharpFunction {
    Name: string;
    Location: vscode.Location;

    constructor(name: string, location: vscode.Location) {
        this.Name = name;
        this.Location = location;
    }
}

function escapeRegExp(str: string) {
    return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
}
