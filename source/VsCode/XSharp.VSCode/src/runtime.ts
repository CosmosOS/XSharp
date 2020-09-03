/*---------------------------------------------------------
 * Based on Microsoft example
 *--------------------------------------------------------*/

import { readFileSync } from 'fs';
import { EventEmitter } from 'events';

/**
 * A Mock runtime with minimal debugger functionality.
 */
export class XSharpRuntime extends EventEmitter {

	// the initial (and one and only) file we are 'debugging'
	private _sourceFile: string = '';
	public get sourceFile() {
		return this._sourceFile;
	}

	// the contents (= lines) of the one and only file
	private _sourceLines: string[] = [];

	// This is the next line that will be 'executed'
	private _currentLine = 0;
	private _currentColumn: number | undefined;

	private _noDebug = false;

	constructor() {
		super();
	}

	/**
	 * Start executing the given program.
	 */
	public start(program: string, stopOnEntry: boolean, noDebug: boolean) {

		this._noDebug = noDebug;

		this.loadSource(program);
		this._currentLine = -1;
	}
	
	// private methods

	private loadSource(file: string) {
		if (this._sourceFile !== file) {
			this._sourceFile = file;
			this._sourceLines = readFileSync(this._sourceFile).toString().split('\n');
		}
	}
}