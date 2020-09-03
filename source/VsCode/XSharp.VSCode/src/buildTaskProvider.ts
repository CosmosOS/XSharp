import * as path from 'path';
import * as vscode from 'vscode';

export class BuildTaskProvider implements vscode.TaskProvider {
	private tasks: vscode.Task[] | undefined;
    static CustomBuildScriptType = 'XSharp';

	// We use a CustomExecution task when state needs to be shared accross runs of the task or when 
	// the task requires use of some VS Code API to run.
	// If you don't need to share state between runs and if you don't need to execute VS Code API in your task, 
	// then a simple ShellExecution or ProcessExecution should be enough.
	// Since our build has this shared state, the CustomExecution is used below.
	private sharedState: string | undefined;

	constructor(private workspaceRoot: string) { }

	public async provideTasks(): Promise<vscode.Task[]> {
		return this.getTasks();
    }
    
	public resolveTask(_task: vscode.Task): vscode.Task | undefined {
		const flavor: string = _task.definition.flavor;
		if (flavor) {
			const definition: BuildTaskProvider = <any>_task.definition;
			return this.getTask();
		}
		return undefined;
	}

	private getTasks(): vscode.Task[] {
		if (this.tasks !== undefined) {
			return this.tasks;
		}
		this.tasks = [];
        this.tasks!.push(this.getTask());
		return this.tasks;
	}

	private getTask(): vscode.Task {
        let definition = {
            type: "process",
        };
		return new vscode.Task(definition, vscode.TaskScope.Workspace, `Build Task`,
        BuildTaskProvider.CustomBuildScriptType, new vscode.ProcessExecution("${command:xsharp.compileAllFiles}"));
	}
}
